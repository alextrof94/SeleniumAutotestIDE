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

namespace SeleniumAutotest
{
    public enum StepStates
    {
        NotStarted, Passed, Error, IgnoredError
    }

    [Serializable]
    internal class TestStep
    {
        public Guid Id { get; set; } // TODO: check need serialization
        public List<TestStep> Substeps { get; set; }

        public string Name { get; set; }
        public StepTypes Type { get; set; }
        public string Selector { get; set; }
        public float SecondsToWait { get; set; }
        public string Value { get; set; }
        public string Parameter { get; set; }
        public bool IgnoreError { get; set; }

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
            Selector = "";
            SecondsToWait = 30;
            Value = "";
            Parameter = "";
            Value = "";
            IgnoreError = false;
            Expanded = true;
        }

        public override string ToString()
        {
            switch (this.Type)
            {
                case StepTypes.Click:
                case StepTypes.DoubleClick:
                case StepTypes.Group:
                    return Name;
                case StepTypes.EnterValue:
                    return $"{Name} [{Value}]";
                case StepTypes.CheckElement:
                    return $"{Name} | {Selector}" + ((IgnoreError)?" | IgnoreError":"");
                case StepTypes.WaitTime:
                    return $"{Name} ({SecondsToWait})";
                case StepTypes.WaitElement:
                    return $"{Name} | {Selector} ({SecondsToWait})";
                case StepTypes.CheckClass:
                    return $"{Name} [class={Value}]" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.CheckClassNotExists:
                    return $"{Name} [class={Value}]" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.CheckText:
                    return $"{Name} [TEXT={Value}]" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.CompareParameters:
                    return $"{Name} [{Value} = {Parameter}]";
                case StepTypes.Open:
                    return $"{Name} | {Value}";
                case StepTypes.CheckAttribute:
                    return $"{Name} [{Selector}={Value}]" + ((IgnoreError) ? " | IgnoreError" : "");
                case StepTypes.ReadAttributeToParameter:
                    return $"{Name} [{Selector} => {Parameter}]";
                case StepTypes.ReadTextToParameter:
                    return $"{Name} [TEXT => {Parameter}]";
                case StepTypes.ReadAddressToParameter:
                    return $"{Name} [URL => {Parameter}]";
                default:
                    return "!!! " + Name;
            }
        }

        public string GetValue(string str)
        {
            string res = ValuesFromParameters.ProcessInput(str, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
            return res;
        }

        public bool Run(IWebDriver driver, CancellationToken token, Action StateUpdated)
        {
            bool needToContinue = true;
            try
            {
                if (this.Selector == null)
                {
                    this.Selector = "";
                }
                if (this.Value == null)
                {
                    this.Value = "";
                }
                switch (Type)
                {
                    case StepTypes.Open:
                        driver.Navigate().GoToUrl(ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters));
                        this.StepState = StepStates.Passed;
                        break;
                    case StepTypes.WaitElement:
                        {
                            if (this.Parent.FoundElement != null)
                            {
                                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(this.SecondsToWait));
                                var el = wait.Until(d => this.Parent.FoundElement.FindElement(By.XPath(this.Selector)));
                                FoundElement = el;
                            }
                            else
                            {
                                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(this.SecondsToWait));
                                var el = wait.Until(d => driver.FindElement(By.XPath(this.Selector)));
                                FoundElement = el;
                            }

                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.EnterValue:
                        {
                            var el = this.Parent.FoundElement;
                            el.Clear();
                            el.SendKeys(ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters));
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.Click:
                        {
                            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                            var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                            el.Click();
                            /* alternate method with JS try, but without error
                            var el = this.Parent.FoundElement;
                            try
                            {
                                el.Click();
                            }
                            catch (ElementNotInteractableException)
                            {
                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el);
                            }
                            */
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
                    case StepTypes.CheckText:
                        {
                            var el = this.Parent.FoundElement;
                            if (el.Text == ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters))
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
                            var el = this.Parent.FoundElement;
                            if (el != null && el.GetAttribute(this.Selector) != null && el.GetAttribute(this.Selector) == ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters))
                            {
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.StepState = StepStates.Error;
                                string errValue = "Атрибут не найден";
                                if (el.GetAttribute(this.Selector) != null)
                                {
                                    errValue = el.GetAttribute(this.Selector);
                                }
                                this.Error = $"Ожидалось [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}], было [{errValue}]";
                            }
                        }
                        break;
                    case StepTypes.CheckElement:
                        {
                            IReadOnlyCollection<IWebElement> elements = null;
                            if (this.Parent.FoundElement != null)
                            {
                                elements = this.Parent.FoundElement.FindElements(By.XPath(this.Selector));
                            }
                            else
                            {
                                elements = driver.FindElements(By.XPath(this.Selector));
                            }
                            if (elements.Count > 0)
                            {
                                this.StepState = StepStates.Passed;
                            }
                            else
                            {
                                this.StepState = StepStates.Error;
                                this.Error = $"Элемент {this.Selector} не найден";
                            }
                        }
                        break;
                    case StepTypes.CheckClass:
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
                            var value = el.GetAttribute(this.Selector).ToString();
                            foreach (var param in ParentAutotest.ParentProject.Parameters)
                            {
                                if (param.Name == this.Parameter)
                                {
                                    param.Value = value;
                                    ParentAutotest.ParentProject.InvokeParametersUpdated();
                                }
                            }
                            foreach (var param in ParentAutotest.Parameters)
                            {
                                if (param.Name == this.Parameter)
                                {
                                    param.Value = value;
                                    ParentAutotest.InvokeParametersUpdated();
                                }
                            }
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.ReadTextToParameter:
                        {
                            var el = this.Parent.FoundElement;
                            var value = el.Text.ToString();
                            foreach (var param in ParentAutotest.ParentProject.Parameters)
                            {
                                if (param.Name == this.Parameter)
                                {
                                    param.Value = value;
                                    ParentAutotest.ParentProject.InvokeParametersUpdated();
                                }
                            }
                            foreach (var param in ParentAutotest.Parameters)
                            {
                                if (param.Name == this.Parameter)
                                {
                                    param.Value = value;
                                    ParentAutotest.InvokeParametersUpdated();
                                }
                            }
                            this.StepState = StepStates.Passed;
                        }
                        break;
                    case StepTypes.ReadAddressToParameter:
                        {
                            var value = driver.Url;
                            foreach (var param in ParentAutotest.ParentProject.Parameters)
                            {
                                if (param.Name == this.Parameter)
                                {
                                    param.Value = value;
                                    ParentAutotest.ParentProject.InvokeParametersUpdated();
                                }
                            }
                            foreach (var param in ParentAutotest.Parameters)
                            {
                                if (param.Name == this.Parameter)
                                {
                                    param.Value = value;
                                    ParentAutotest.InvokeParametersUpdated();
                                }
                            }
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
                    default:
                        this.StepState = StepStates.Passed;
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
                    }
                }
                StateUpdated?.Invoke();
                foreach (var substep in Substeps)
                {
                    if (token.IsCancellationRequested || !needToContinue)
                        break;
                    needToContinue = substep.Run(driver, token, StateUpdated);
                }
            }
            catch (Exception ex)
            {
                this.StepState = StepStates.Error;
                Error = ex.ToString();
                needToContinue = false;
            }
            return needToContinue;
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
