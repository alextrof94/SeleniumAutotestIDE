using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace SeleniumAutotest
{
    [Serializable]
    internal class Autotest
    {
        public string Name { get; set; }
        public TestStep Root { get; set; }
        public List<Parameter> Parameters { get; set; }
        public bool RunAfterPrevious { get; set; } = false;
        public bool RegenerateParametersOnRun { get; set; } = true;

        public event Action StateUpdated;
        public event Action RunFinished;
        public event Action ParametersUpdated;

        [JsonIgnore]
        public TimeSpan CompleteTime { get; set; }
        [JsonIgnore]
        public Project ParentProject { get; set; }
        [JsonIgnore]
        public bool RunGotError { get; set; }
        [JsonIgnore]
        public TestStep ErrorStep { get; set; }

        public Autotest()
        {
            Name = "New Autotest";
            Root = new TestStep();
            Parameters = new List<Parameter>();
        }

        public override string ToString()
        {
            return Name;
        }

        public void ResetAllParentsForSteps(Project parent)
        {
            ParentProject = parent;
            ResetParentsForStep(Root);
        }

        private void ResetParentsForStep(TestStep step)
        {
            foreach (var substep in step.Substeps)
            {
                substep.Parent = step;
                substep.ParentAutotest = this;
                ResetParentsForStep(substep);
            }
        }

        public TestStep FindStepById(Guid id)
        {
            return FindStepByIdInSubsteps(Root, id);
        }

        private TestStep FindStepByIdInSubsteps(TestStep step, Guid id)
        {
            if (step.Id == id)
            {
                return step;
            }
            foreach (var substep in step.Substeps)
            {
                var res = FindStepByIdInSubsteps(substep, id);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
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
                par.Value = ValuesFromParameters.ProcessInput(par.Pattern, ParentProject.Parameters, Parameters);
            }
            InvokeParametersUpdated();
        }

        public void InvokeParametersUpdated()
        {
            ParametersUpdated?.Invoke();
        }

        private string TryToDownloadDriver()
        {
            if (Directory.Exists("./Chrome") && Directory.Exists("./Chrome/DEF") && File.Exists("./Chrome/DEF/chromedriver.exe"))
            {
                return "./Chrome/DEF/";
            }
            else
            {
                var config = new ChromeConfig();
                return Path.GetDirectoryName(new DriverManager().SetUpDriver(config, version: config.GetMatchingBrowserVersion()));
            }
        }

        public void ResetGuid()
        {
            foreach (var substep in Root.Substeps)
            {
                substep.ResetGuid();
            }
        }

        public void AutoModeRun(CancellationToken token, bool slowMode, bool selectFoundElements)
        {
            try
            {
                RunGotError = false;
                ErrorStep = null;
                if (RegenerateParametersOnRun)
                {
                    GenerateParameters();
                }
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("--disable-notifications");
                string driverPath = TryToDownloadDriver();
                IWebDriver driver = new ChromeDriver(driverPath, options);
                driver.Manage().Window.Maximize();
                try
                {
                    foreach (var substep in Root.Substeps)
                    {
                        substep.ClearState();
                    }
                    foreach (var substep in Root.Substeps)
                    {
                        if (token.IsCancellationRequested)
                            break;
                        substep.AutoModeRun(driver, token, StateUpdated, slowMode, selectFoundElements);
                    }
                    Thread.Sleep(3000);
                }
                catch
                {
                    RunGotError = true;
                }
                finally
                {
                    driver.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            RunFinished?.Invoke();
        }

        TestStep CurrentStep = null;
        IWebDriver WebDriver = null;

        private TestStep GetNextStep(TestStep step)
        {
            // root
            // L step 0 
            //   L substep 0-0
            //   L substep 0-1
            //   L substep 0-2
            // L step 1
            //   L substep 1-0

            // get first not started substep (step 0 -> substep 0-0)
            var indexNow = (step.Parent == null) ? 0 : step.Parent.Substeps.IndexOf(step);
            List<TestStep> validSubsteps = step.Substeps.Where(x => (x.StepState == StepStates.NotStarted || x.StepState == StepStates.Error) && x.Enabled && (step.Parent == null || step.Parent.Substeps.IndexOf(x) > indexNow)).ToList();
            if (validSubsteps.Count() > 0)
            {
                return validSubsteps[0];
            }

            // if parent is not exists - root finished, complete (root -> null)
            if (step.Parent == null)
                return null;

            // get next substep (substep 0-0 -> substep 0-2 if substep 0-1 disabled)
            var index = step.Parent.Substeps.IndexOf(step);
            bool allNextChecked = false;
            while (!allNextChecked)
            {
                index++;
                if (index < step.Parent.Substeps.Count - 1)
                {
                    if (step.Parent.Substeps[index].Enabled)
                    {
                        return step.Parent.Substeps[index];
                    }
                }
                else
                {
                    allNextChecked = true;
                }
            }

            // return next step for parent (substep 0-2 -> step 1)
            return GetNextStep(step.Parent);
        }

        public bool StepModeRun(bool selectFoundElements)
        {
            if (Root.Substeps.Count == 0) { return false; }

            if (RegenerateParametersOnRun)
            {
                GenerateParameters();
            }
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--disable-notifications");
            string driverPath = TryToDownloadDriver();
            WebDriver = new ChromeDriver(driverPath, options);
            WebDriver.Manage().Window.Maximize();
            foreach (var substep in Root.Substeps)
            {
                substep.ClearState();
            }
            CurrentStep = Root.Substeps[0];
            StepModeContinue(selectFoundElements, true);

            return true;
        }

        public void StepModeContinue(bool selectFoundElements, bool firstStep = false)
        {
            try
            {
                if (!firstStep)
                {
                    TestStep prevStep = CurrentStep;
                    CurrentStep = GetNextStep(CurrentStep);
                    if (CurrentStep == null)
                    {
                        StepModeStop();
                        RunFinished?.Invoke();
                        return;
                    }
                    CurrentStep.PrevStep = prevStep;
                }
                CurrentStep.StepModeRun(WebDriver, StateUpdated, selectFoundElements);
                StateUpdated?.Invoke();
            }
            catch 
            {
                CurrentStep = CurrentStep.PrevStep;
                StateUpdated?.Invoke();
            }
        }

        public void StepModeStop()
        {
            WebDriver.Quit();
        }

        public void StepModeStepBack()
        {
            if (CurrentStep.PrevStep == null) { return; }
            CurrentStep.StepState = StepStates.NotStarted;
            CurrentStep = CurrentStep.PrevStep;
            StateUpdated?.Invoke();
        }
    }
}
