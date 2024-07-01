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

        public event Action ParametersUpdated;
        public event Action RunAutotestFinished;
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
                                Name = "Ждать появления поиска",
                                Type = StepTypes.WaitElement,
                                SecondsToWait = 10,
                                Selector = "//textarea[@title='Поиск']",
                                Substeps = new List<TestStep>
                                {
                                    new TestStep {
                                        Name = "Ввести Википедия",
                                        Type = StepTypes.EnterValue,
                                        Value = "Википедия"
                                    }
                                }
                            },
                            new TestStep() {
                                Name = "Ждать кнопку поиска",
                                Type = StepTypes.WaitElement,
                                SecondsToWait = 1,
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
                                Name = "Ждать появления элемента результатов",
                                Type = StepTypes.WaitElement,
                                SecondsToWait = 10,
                                Selector = "//div[@id='rso']",
                                Substeps = new List<TestStep>
                                {
                                    new TestStep() {
                                        Name = "Найти первый дочерний элемент a",
                                        Type = StepTypes.WaitElement,
                                        SecondsToWait = 1,
                                        Selector = "//a",
                                        Substeps = new List<TestStep>
                                        {
                                            new TestStep() {
                                                Name = "Найти первый дочерний элемент h3",
                                                Type = StepTypes.WaitElement,
                                                SecondsToWait = 1,
                                                Selector = "//h3",
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
                                Name = "Ждать появления элемента с лого",
                                Type = StepTypes.WaitElement,
                                SecondsToWait = 30,
                                Selector = "//a[@class='mw-wiki-logo']"
                            }
                        }
                    }
                };
                Autotests[0].ResetAllParentsForSteps();
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
                RunAutotestFinished?.Invoke();
                return; 
            }

            SelectedAutotest.CompleteTime = TestStopwatch.Elapsed;

            int ind = Autotests.IndexOf(SelectedAutotest);
            if (ind < Autotests.Count - 1 && Autotests[ind + 1].RunAfterPrevious)
            {
                SelectAutotest(Autotests[ind + 1]);
                SelectedAutotestChanged?.Invoke();
                Thread.Sleep(3000);
                RunAutotest();
            }
            else
            {
                RunStopwatch.Stop();
                RunTime = TestStopwatch.Elapsed;
                RunAutotestFinished?.Invoke();
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

        public bool RunAutotest()
        {
            if (SelectedAutotest == null) { return false; }
            RunStopwatch.Restart();
            if (RegenerateParametersOnRun)
            {
                GenerateParameters();
            }
            TestStopwatch.Restart();
            CancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => SelectedAutotest.Run(CancellationTokenSource.Token), CancellationTokenSource.Token);
            return true;
        }

        public void StopAutotest()
        {
            CancellationTokenSource?.Cancel();
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
    }
}
