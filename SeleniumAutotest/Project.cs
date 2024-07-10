using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeleniumAutotest
{
    [Serializable]
    internal class Project
    {
        public string Name { get; set; }
        public BindingList<Autotest> Autotests { get; set; }
        public List<Parameter> Parameters { get; set; }
        public bool RegenerateParametersOnRun { get; set; } = true;

        [JsonIgnore]
        public Autotest SelectedAutotest { get; set; }
        [JsonIgnore]
        private CancellationTokenSource CancellationTokenSource;
        [JsonIgnore]
        private Stopwatch TestStopwatch { get; set; } = new Stopwatch();
        [JsonIgnore]
        private Stopwatch RunStopwatch { get; set; } = new Stopwatch();
        [JsonIgnore]
        public TimeSpan RunTime { get; set; }
        [JsonIgnore]
        public bool SlowMode { get; set; }
        [JsonIgnore]
        public bool SelectFoundElements { get; set; }

        public event Action ParametersUpdated;
        public event Action<string> RunAutotestFinished;
        public event Action SelectedAutotestChanged;

        public Project()
        {
            Autotests = new BindingList<Autotest>();
            Parameters = new List<Parameter>();
            ResetAllParentsForAutotests();
        }

        public void AddAutotest(Autotest test)
        {
            Autotests.Add(test);
            if (Autotests.Count == 1)
            {
                Autotests[0].Parameters.Add(new Parameter() { Name = "Search", Pattern = "Википедия" });
                Autotests[0].Name = "Открыть википедию через гугл";
                Autotests[0].Root.Substeps = new List<TestStep>() {
                    new TestStep() {
                        Name = "Поиск википедии в гугле",
                        Type = StepTypes.Group,
                        Substeps = new List<TestStep>
                        {
                            new TestStep() {
                                Name = "Открыть гугл",
                                Type = StepTypes.Open,
                                Value = "https://www.google.com/"
                            },
                            new TestStep() {
                                Name = "Ждать появления поиска по XPATH",
                                Type = StepTypes.FindElement,
                                SecondsToWait = 30,
                                Selector = "//textarea[@title='Поиск']",
                                Substeps = new List<TestStep>
                                {
                                    new TestStep {
                                        Name = "Ввести Википедия из параметра",
                                        Type = StepTypes.EnterText,
                                        Value = "%Search%"
                                    }
                                }
                            },
                            new TestStep() {
                                Name = "Ждать кнопку поиска по XPATH",
                                Type = StepTypes.FindElement,
                                SecondsToWait = 30,
                                Selector = "//input[@name='btnK' and @value='Поиск в Google']",
                                Substeps = new List<TestStep>
                                {
                                    new TestStep {
                                        Name = "Клик",
                                        Type = StepTypes.Click
                                    }
                                }
                            }
                        }
                    },
                    new TestStep()
                    {
                        Name = "Нажать на первую ссылку в выборке",
                        Type = StepTypes.Group,
                        Substeps = new List<TestStep>
                        {
                            new TestStep() {
                                Name = "Ждать появления элемента результатов по ID",
                                Type = StepTypes.FindElement,
                                SecondsToWait = 30,
                                SelectorType = SelectorType.ID,
                                Selector = "rso",
                                Substeps = new List<TestStep>
                                {
                                    new TestStep() {
                                        Name = "Найти первый дочерний элемент a по Tag",
                                        Type = StepTypes.FindElement,
                                        SecondsToWait = 30,
                                        SelectorType = SelectorType.Tag,
                                        Selector = "a",
                                        Substeps = new List<TestStep>
                                        {
                                            new TestStep() {
                                                Name = "Найти первый дочерний элемент h3 по XPATH",
                                                Type = StepTypes.FindElement,
                                                SecondsToWait = 30,
                                                Selector = ".//h3",
                                                Substeps = new List<TestStep>
                                                {
                                                    new TestStep {
                                                        Name = "Клик",
                                                        Type = StepTypes.Click
                                                    }
                                                }
                                            },
                                        }
                                    },
                                }
                            }
                        }
                    },
                    new TestStep()
                    {
                        Name = "Проверка загрузки вики",
                        Type = StepTypes.Group,
                        Substeps = new List<TestStep>
                        {
                            new TestStep() {
                                Name = "Ждать появления элемента с лого по Классу",
                                Type = StepTypes.FindElement,
                                SecondsToWait = 30,
                                SelectorType = SelectorType.Class,
                                Selector = "mw-wiki-logo"
                            }
                        }
                    }
                };
                GenerateParameters();
                Autotests[0].ResetGuid();
                Autotests[0].ResetAllParentsForSteps(this);
            }
            ResetAllParentsForAutotests();
        }

        public void DeleteSelectedAutotest()
        {
            if (SelectedAutotest == null) { return; }
            Autotests.Remove(SelectedAutotest);
        }

        public void CloneSelectedAutotest()
        {
            if (SelectedAutotest == null) { return; }
            var autotest = SelectedAutotest.Clone();
            autotest.ResetGuid();
            autotest.ResetAllParentsForSteps(this);
            Autotests.Add(autotest);
        }

        public void SelectAutotest(Autotest selected)
        {
            if (SelectedAutotest != null)
            {
                SelectedAutotest.RunFinished -= SelectedAutotest_RunCompleted;
            }
            SelectedAutotest = selected;
            if (SelectedAutotest != null)
            {
                SelectedAutotest.RunFinished += SelectedAutotest_RunCompleted;
            }
        }

        private void SelectedAutotest_RunCompleted()
        {
            TestStopwatch.Stop();
            if (SelectedAutotest.RunGotError)
            {
                RunStopwatch.Stop();
                RunTime = TestStopwatch.Elapsed;
                RunAutotestFinished?.Invoke(SelectedAutotest.ErrorStep?.Log);
                return; 
            }

            SelectedAutotest.CompleteTime = TestStopwatch.Elapsed;

            int ind = Autotests.IndexOf(SelectedAutotest);
            if (ind < Autotests.Count - 1 && Autotests[ind + 1].RunAfterPrevious)
            {
                SelectAutotest(Autotests[ind + 1]);
                SelectedAutotestChanged?.Invoke();
                Thread.Sleep(3000);
                RunAutotest(SlowMode, SelectFoundElements, false);
            }
            else
            {
                RunStopwatch.Stop();
                RunTime = RunStopwatch.Elapsed;
                RunAutotestFinished?.Invoke(SelectedAutotest.ErrorStep?.Log);
            }
        }

        public void GenerateParameters()
        {
            foreach (var par in Parameters)
            {
                if (string.IsNullOrEmpty(par.Pattern)) { continue; }
                par.Value = "";
            }
            foreach (var par in Parameters)
            {
                if (string.IsNullOrEmpty(par.Pattern)) { continue; }
                par.Value = ValuesFromParameters.ProcessInput(par.Pattern, Parameters, null);
            }
            InvokeParametersUpdated();
        }

        public void InvokeParametersUpdated()
        {
            ParametersUpdated?.Invoke();
        }

        public void ResetAllParentsForAutotests()
        {
            foreach (var autotest in Autotests)
            {
                autotest.ParentProject = this;
            }
        }

        public bool TestMoveUp()
        {
            if (SelectedAutotest == null) { return false; }

            var ind = Autotests.IndexOf(SelectedAutotest);
            if (ind == 0) { return false; }

            (Autotests[ind-1], Autotests[ind]) = (Autotests[ind], Autotests[ind-1]);

            SelectAutotest(Autotests[ind-1]);
            return true;
        }

        public bool TestMoveDown()
        {
            if (SelectedAutotest == null) { return false; }

            var ind = Autotests.IndexOf(SelectedAutotest);
            if (ind == Autotests.Count - 1) { return false; }

            (Autotests[ind + 1], Autotests[ind]) = (Autotests[ind], Autotests[ind + 1]);
            SelectAutotest(Autotests[ind + 1]);
            return true;
        }

        public void RunAutotest(bool slowMode, bool selectFoundElements, bool needResetTimer = true)
        {
            if (SelectedAutotest == null) { return; }
            SlowMode = slowMode;
            SelectFoundElements = selectFoundElements;
            if (needResetTimer)
            {
                RunStopwatch.Restart();
            }
            if (RegenerateParametersOnRun)
            {
                GenerateParameters();
            }
            TestStopwatch.Restart();
            CancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => SelectedAutotest.AutoModeRun(CancellationTokenSource.Token, slowMode, selectFoundElements), CancellationTokenSource.Token);
            return;
        }

        public void StopAutotest()
        {
            CancellationTokenSource?.Cancel();
        }

        public bool StepModeRun(bool selectFoundElements)
        {
            if (SelectedAutotest == null) { return false; }
            if (RegenerateParametersOnRun)
            {
                GenerateParameters();
            }
            return SelectedAutotest.StepModeRun(selectFoundElements);
        }

        public void StepModeContinue(bool selectFoundElements)
        {
            SelectedAutotest.StepModeContinue(selectFoundElements);
        }

        internal void StepModeStop()
        {
            SelectedAutotest.StepModeStop();
        }

        public void StepModeStepBack()
        {
            SelectedAutotest.StepModeStepBack();
        }
    }
}
