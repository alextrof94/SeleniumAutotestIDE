using Newtonsoft.Json;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace SeleniumAutotest
{
    public enum StepStates
    {
        NotStarted, Passed, Error, IgnoredError, Skipped
    }

    public enum SelectorType
    {
        ID, Class, XPath
    }

    [Serializable]
    internal class TestStep
    {
        public Guid Id { get; set; } // TODO: check need serialization
        public List<TestStep> Substeps { get; set; }

        public string Name { get; set; }
        public StepTypes Type { get; set; }
        public SelectorType SelectorType { get; set; }
        public string Selector { get; set; }
        public float SecondsToWait { get; set; }
        public string Value { get; set; }
        public string Parameter { get; set; }
        public bool IgnoreError { get; set; }
        public bool Enabled { get; set; }

        public bool Expanded { get; set; }


        [JsonIgnore]
        public TestStep Parent { get; set; }
        [JsonIgnore]
        public Autotest ParentAutotest { get; set; }
        [JsonIgnore]
        public IWebElement FoundElement;
        [JsonIgnore]
        public StepStates StepState;
        [JsonIgnore]
        public string Error;

        public TestStep()
        {
            Id = Guid.NewGuid();
            Substeps = new List<TestStep>();
            SelectorType = SelectorType.XPath;
            Selector = "";
            SecondsToWait = 30;
            Value = "";
            Parameter = "";
            Value = "";
            IgnoreError = false;
            Expanded = true;
            Enabled = true;
        }

        public override string ToString()
        {
            switch (this.Type)
            {
                case StepTypes.Click:
                case StepTypes.JsClick:
                case StepTypes.AltClick:
                case StepTypes.DoubleClick:
                    return $"{Name} [{StepType.Descriptions[Type]}]";
                case StepTypes.Group:
                    return Name;
                case StepTypes.EnterText:
                    return $"{Name} [{Value}]";
                case StepTypes.SetAttribute:
                    return $"{Name} [{Selector}={Value}]";
                case StepTypes.CheckElement:
                    return $"{Name} | {SelectorType}={Selector}" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.WaitTime:
                    return $"{Name} ({SecondsToWait})";
                case StepTypes.FindElement:
                    return $"{Name} | {SelectorType}={Selector} ({SecondsToWait})";
                case StepTypes.CheckClassExists:
                    return $"{Name} [class={Value}]" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.CheckClassNotExists:
                    return $"{Name} [class={Value}]" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.CheckText:
                    return $"{Name} [TEXT={Value}]" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.CompareParameters:
                    return $"{Name} [{Value} = {Parameter}]";
                case StepTypes.Open:
                    return $"{Name} | {Value}";
                case StepTypes.RefreshPage:
                    return $"{Name}";
                case StepTypes.CheckAttribute:
                    return $"{Name} [{Selector}={Value}]" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.ReadAttributeToParameter:
                    return $"{Name} [{Selector} => {Parameter}]";
                case StepTypes.ReadTextToParameter:
                    return $"{Name} [TEXT => {Parameter}]";
                case StepTypes.ReadAddressToParameter:
                    return $"{Name} [URL => {Parameter}]";
                case StepTypes.InputToParameterByUser:
                    return $"{Name} [USER => {Parameter}]";
                case StepTypes.JsEvent:
                    return $"{Name} [EVENT={Value}]";
                default:
                    return "!!! " + Name;
            }
        }

        public string GetValue(string str)
        {
            string res = ValuesFromParameters.ProcessInput(str, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
            return res;
        }

        /// <returns>need to continue</returns>
        public bool Run(IWebDriver driver, CancellationToken token, Action StateUpdated, bool slowMode, bool selectFoundElements)
        {
            bool needToContinue = true;
            try
            {
                bool needToSlow = true;
                if (this.Selector == null)
                {
                    this.Selector = "";
                }
                if (this.Value == null)
                {
                    this.Value = "";
                }
                if (!Enabled)
                {
                    this.StepState = StepStates.Skipped;
                    return true;
                }
                switch (Type)
                {
                    case StepTypes.Open:
                        driver.Navigate().GoToUrl(ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters));
                        this.StepState = StepStates.Passed;
                        break;
                    case StepTypes.RefreshPage:
                        driver.Navigate().Refresh();
                        this.StepState = StepStates.Passed;
                        break;
                    case StepTypes.FindElement:
                        {
                            var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(this.SecondsToWait));
                            IWebElement el = null;
                            if (this.Parent.FoundElement != null)
                            {
                                switch (SelectorType)
                                {
                                    case SelectorType.ID:
                                        el = wait.Until(d => this.Parent.FoundElement.FindElement(By.Id(selector)));
                                        break;
                                    case SelectorType.Class:
                                        el = wait.Until(d => this.Parent.FoundElement.FindElement(By.ClassName(selector)));
                                        break;
                                    case SelectorType.XPath:
                                        el = wait.Until(d => this.Parent.FoundElement.FindElement(By.XPath(selector)));
                                        break;
                                }
                            }
                            else
                            {
                                switch (SelectorType)
                                {
                                    case SelectorType.ID:
                                        el = wait.Until(d => driver.FindElement(By.Id(selector)));
                                        break;
                                    case SelectorType.Class:
                                        el = wait.Until(d => driver.FindElement(By.ClassName(selector)));
                                        break;
                                    case SelectorType.XPath:
                                        el = wait.Until(d => driver.FindElement(By.XPath(selector)));
                                        break;
                                }
                            }
                            //wait.Until(d => el.Displayed);
                            FoundElement = el;
                            if (selectFoundElements)
                            {
                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript("arguments[0].style.border='2px solid red';", el);
                            }

                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.EnterText:
                        {
                            string value = ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                            var el = this.Parent.FoundElement;
                            //el.Clear();
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            js.ExecuteScript("arguments[0].value = '';", el);
                            el.SendKeys(value);
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.SetAttribute:
                        {
                            string value = ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                            var el = this.Parent.FoundElement;
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            js.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", el, this.Selector, value);
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.Click:
                        {
                            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                            var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                            el.Click();
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.JsClick:
                        {
                            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                            var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el);
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.AltClick:
                        {
                            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                            var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                            Actions action = new Actions(driver);
                            action.Click(el).Build().Perform();
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.DoubleClick:
                        {
                            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                            var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                            Actions action = new Actions(driver);
                            action.DoubleClick(el).Build().Perform();
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.JsEvent:
                        {
                            var el = this.Parent.FoundElement;
                            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                            js.ExecuteScript($"arguments[0].dispatchEvent(new Event('{Value}'));", el);
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.CheckText:
                        {
                            var el = this.Parent.FoundElement;
                            if (IsMatchMask(el.Text, ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)))
                            {
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.StepState = StepStates.Error;
                                this.Error = $"Ожидалось [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}], было [{el.Text}]";
                            }
                        }
                        break;
                    case StepTypes.CheckAttribute:
                        {
                            var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                            var el = this.Parent.FoundElement;
                            if (el != null && el.GetAttribute(selector) != null && IsMatchMask(el.GetAttribute(selector), ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)))
                            {
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.StepState = StepStates.Error;
                                string errValue = $"Атрибут {selector} не найден";
                                if (el.GetAttribute(selector) != null)
                                {
                                    errValue = el.GetAttribute(selector);
                                }
                                this.Error = $"Ожидалось [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}], было [{errValue}]";
                            }
                        }
                        break;
                    case StepTypes.CheckElement:
                        {
                            IReadOnlyCollection<IWebElement> elements = null;
                            var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                            if (this.Parent.FoundElement != null)
                            {
                                elements = this.Parent.FoundElement.FindElements(By.XPath(selector));
                            }
                            else
                            {
                                elements = driver.FindElements(By.XPath(selector));
                            }
                            if (elements.Count > 0)
                            {
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.StepState = StepStates.Error;
                                this.Error = $"Элемент {selector} не найден";
                            }
                        }
                        break;
                    case StepTypes.CheckClassExists:
                        {
                            if (this.Parent.FoundElement.GetAttribute("class").Contains(ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)))
                            {
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.StepState = StepStates.Error;
                                this.Error = $"Класс [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}] не найден в элементе";
                            }
                        }
                        break;
                    case StepTypes.CheckClassNotExists:
                        {
                            if (!this.Parent.FoundElement.GetAttribute("class").Contains(ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)))
                            {
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.StepState = StepStates.Error;
                                this.Error = $"Класс [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}] не найден в элементе";
                            }
                        }
                        break;
                    case StepTypes.WaitTime:
                        {
                            Thread.Sleep((int)(this.SecondsToWait * 1000f));
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.ReadAttributeToParameter:
                        {
                            var el = this.Parent.FoundElement;
                            var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                            var value = el.GetAttribute(selector) ?? "";
                            SetParameter(value, this.Parameter);
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.ReadTextToParameter:
                        {
                            var el = this.Parent.FoundElement;
                            var value = el.Text;
                            SetParameter(value, this.Parameter);
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.ReadAddressToParameter:
                        {
                            var value = driver.Url;
                            SetParameter(value, this.Parameter);
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.CompareParameters:
                        {
                            var value1 = ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                            var value2 = ValuesFromParameters.ProcessInput(this.Parameter, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                            if (value1 == value2)
                            {
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.Error = $"Param1 [{value1}] ({value1.GetType()})\r\nParam2 [{value2}] ({value2.GetType()})";
                                this.StepState = StepStates.Error;
                            }
                        }
                        break;
                    case StepTypes.InputToParameterByUser:
                        {
                            InputDataForm inputDataForm = new InputDataForm(this.Parameter);
                            if (inputDataForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                foreach (var param in ParentAutotest.ParentProject.Parameters)
                                {
                                    if (param.Name == this.Parameter)
                                    {
                                        param.Value = inputDataForm.Result;
                                        ParentAutotest.ParentProject.InvokeParametersUpdated();
                                    }
                                }
                                foreach (var param in ParentAutotest.Parameters)
                                {
                                    if (param.Name == this.Parameter)
                                    {
                                        param.Value = inputDataForm.Result;
                                        ParentAutotest.InvokeParametersUpdated();
                                    }
                                }
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.Error = $"Не введён параметр";
                                this.StepState = StepStates.Error;
                            }
                        }
                        break;
                    default:
                        this.StepState = StepStates.Passed;
                        needToSlow = false;
                        break;
                }
                if (this.StepState == StepStates.Error)
                {
                    if (this.IgnoreError)
                    {
                        this.StepState = StepStates.IgnoredError;
                    }
                    else
                    {
                        needToContinue = false;
                        ParentAutotest.ErrorStep = this;
                    }
                }
                StateUpdated?.Invoke();
                if (needToSlow && slowMode)
                {
                    Thread.Sleep(1000);
                }
                foreach (var substep in Substeps)
                {
                    if (token.IsCancellationRequested || !needToContinue)
                        break;
                    needToContinue = substep.Run(driver, token, StateUpdated, slowMode, selectFoundElements);
                }
            }
            catch (Exception ex)
            {
                if (this.IgnoreError)
                {
                    this.StepState = StepStates.IgnoredError;
                }
                else
                {
                    this.StepState = StepStates.Error;
                    needToContinue = false;
                    ParentAutotest.ErrorStep = this;
                }
                Error = ex.ToString();
            }
            return needToContinue;
        }

        private void SetParameter(string value, string paramName)
        {
            foreach (var param in ParentAutotest.ParentProject.Parameters)
            {
                if (param.Name == paramName)
                {
                    param.Value = value;
                    ParentAutotest.ParentProject.InvokeParametersUpdated();
                }
            }
            foreach (var param in ParentAutotest.Parameters)
            {
                if (param.Name == paramName)
                {
                    param.Value = value;
                    ParentAutotest.InvokeParametersUpdated();
                }
            }
        }

        bool IsMatchMask(string input, string pattern)
        {
            string regexPattern = "^" + Regex.Escape(pattern).Replace(@"\?", ".").Replace(@"\*", ".*") + "$";
            return Regex.IsMatch(input, regexPattern);
        }

        internal void ClearState()
        {
            this.StepState = StepStates.NotStarted;
            this.Error = "";
            foreach (var substep in Substeps)
            {
                substep.ClearState();
            }
        }

        public void MoveUp()
        {
            int index = this.Parent.Substeps.IndexOf(this);
            if (index > 0)
            {
                (this.Parent.Substeps[index], this.Parent.Substeps[index - 1]) = (this.Parent.Substeps[index - 1], this.Parent.Substeps[index]);
            }
        }

        public void MoveDown()
        {
            int index = this.Parent.Substeps.IndexOf(this);
            if (index < this.Parent.Substeps.Count - 1)
            {
                (this.Parent.Substeps[index], this.Parent.Substeps[index + 1]) = (this.Parent.Substeps[index + 1], this.Parent.Substeps[index]);
            }
        }

        public void ResetGuid()
        {
            this.Id = Guid.NewGuid();
            foreach (var substep in Substeps)
            {
                substep.ResetGuid();
            }
        }
    }
}
