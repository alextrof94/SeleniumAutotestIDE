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

namespace SeleniumAutotest
{
    [Serializable]
    internal class Autotest
    {
        public string Name { get; set; }
        public TestStep Root {  get; set; }
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

        public Autotest()
        {
            Name = "New Autotest";
            Root = new TestStep();
            Parameters = new List<Parameter>();
        }

        public override string ToString() { 
            return Name;
        }

        public void ResetAllParentsForSteps()
        {
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

        public void Run(CancellationToken token)
        {
            try
            {
                RunGotError = false;
                if (RegenerateParametersOnRun)
                {
                    GenerateParameters();
                }
                string driverPath = TryToDownloadDriver();
                IWebDriver driver = new ChromeDriver(driverPath);
                driver.Manage().Window.Maximize();
                try
                {
                    foreach (var substep in Root.Substeps)
                    {
                        substep.ClearState();
                    }
                    bool needToContinue = true;
                    foreach (var substep in Root.Substeps)
                    {
                        if (token.IsCancellationRequested || !needToContinue)
                            break;
                        needToContinue = substep.Run(driver, token, StateUpdated);
                        if (!needToContinue)
                        {
                            RunGotError = true;
                        }
                    }
                    Thread.Sleep(3000);
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
    }
}
