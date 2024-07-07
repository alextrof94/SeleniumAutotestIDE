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
using System.Collections.ObjectModel;

namespace SeleniumAutotest
{
    public enum StepStates
    {
        NotStarted, Passed, Error, IgnoredError, Skipped
    }

    public enum SelectorType
    {
        ID, Class, XPath, Tag, PartLink
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
        [JsonIgnore]
        public TestStep PrevStep;

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
            Parent = null;
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

        private void StepWork(IWebDriver driver, Action StateUpdated, bool slowMode, bool selectFoundElements, bool canIgnoreErrorIfStepIgnoringThem = true, int staleErrorCount = 0)
        {
            try
            {
                try
                {
                    if (!Enabled)
                    {
                        this.StepState = StepStates.Skipped;
                        return;
                    }

                    if (this.Selector == null) { this.Selector = ""; }
                    if (this.Value == null) { this.Value = ""; }

                    bool needToSlow = false;
                    switch (Type)
                    {
                        case StepTypes.Open:
                            driver.Navigate().GoToUrl(ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters));
                            break;
                        case StepTypes.RefreshPage:
                            driver.Navigate().Refresh();
                            break;
                        case StepTypes.FindElement:
                            {
                                needToSlow = true;
                                var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(this.SecondsToWait));
                                IWebElement el = null;
                                if (this.Parent.FoundElement != null)
                                {
                                    IReadOnlyCollection<IWebElement> tempElementsBySelector = null;
                                    switch (SelectorType)
                                    {
                                        case SelectorType.ID:
                                            tempElementsBySelector = wait.Until(d => this.Parent.FoundElement.FindElements(By.Id(selector)));
                                            break;
                                        case SelectorType.Class:
                                            tempElementsBySelector = wait.Until(d => this.Parent.FoundElement.FindElements(By.ClassName(selector)));
                                            break;
                                        case SelectorType.XPath:
                                            tempElementsBySelector = wait.Until(d => this.Parent.FoundElement.FindElements(By.XPath(selector)));
                                            break;
                                        case SelectorType.Tag:
                                            tempElementsBySelector = wait.Until(d => this.Parent.FoundElement.FindElements(By.TagName(selector)));
                                            break;
                                        case SelectorType.PartLink:
                                            tempElementsBySelector = wait.Until(d => this.Parent.FoundElement.FindElements(By.PartialLinkText(selector)));
                                            break;
                                    }
                                    el = tempElementsBySelector.FirstOrDefault(element => IsChildOf(driver, element, this.Parent.FoundElement));
                                    if (el == null)
                                    {
                                        var selectorParent = ValuesFromParameters.ProcessInput(this.Parent.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                        string fullXPath = GetElementXPath(driver, this.Parent.FoundElement);
                                        throw new Exception($"Дочерний элемент {SelectorType}=\"{selector}\" не найден в родителе: {this.Parent.SelectorType}=\"{selectorParent}\"\r\nПолный путь родителя:\r\n{fullXPath}\r\n");
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
                                        case SelectorType.Tag:
                                            el = wait.Until(d => driver.FindElement(By.TagName(selector)));
                                            break;
                                        case SelectorType.PartLink:
                                            el = wait.Until(d => driver.FindElement(By.PartialLinkText(selector)));
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
                            }
                            break;
                        case StepTypes.CheckElement:
                            {
                                var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                IWebElement el = null;
                                if (this.Parent.FoundElement != null)
                                {
                                    IReadOnlyCollection<IWebElement> tempElementsBySelector = null;
                                    switch (SelectorType)
                                    {
                                        case SelectorType.ID:
                                            tempElementsBySelector = this.Parent.FoundElement.FindElements(By.Id(selector));
                                            break;
                                        case SelectorType.Class:
                                            tempElementsBySelector = this.Parent.FoundElement.FindElements(By.ClassName(selector));
                                            break;
                                        case SelectorType.XPath:
                                            tempElementsBySelector = this.Parent.FoundElement.FindElements(By.XPath(selector));
                                            break;
                                        case SelectorType.Tag:
                                            tempElementsBySelector = this.Parent.FoundElement.FindElements(By.TagName(selector));
                                            break;
                                        case SelectorType.PartLink:
                                            tempElementsBySelector = this.Parent.FoundElement.FindElements(By.PartialLinkText(selector));
                                            break;
                                    }
                                    el = tempElementsBySelector.FirstOrDefault(element => IsChildOf(driver, element, this.Parent.FoundElement));
                                    if (el == null)
                                    {
                                        var selectorParent = ValuesFromParameters.ProcessInput(this.Parent.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                        string fullXPath = GetElementXPath(driver, this.Parent.FoundElement);
                                        throw new Exception($"Дочерний элемент {SelectorType}=\"{selector}\" не найден в родителе: {this.Parent.SelectorType}=\"{selectorParent}\"\r\nПолный путь родителя:\r\n{fullXPath}\r\n");
                                    }
                                }
                                else
                                {
                                    switch (SelectorType)
                                    {
                                        case SelectorType.ID:
                                            el = driver.FindElement(By.Id(selector));
                                            break;
                                        case SelectorType.Class:
                                            el = driver.FindElement(By.ClassName(selector));
                                            break;
                                        case SelectorType.XPath:
                                            el = driver.FindElement(By.XPath(selector));
                                            break;
                                        case SelectorType.Tag:
                                            el = driver.FindElement(By.TagName(selector));
                                            break;
                                        case SelectorType.PartLink:
                                            el = driver.FindElement(By.PartialLinkText(selector));
                                            break;
                                    }
                                }
                            }
                            break;
                        case StepTypes.EnterText:
                            {
                                needToSlow = true;
                                string value = ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                var el = this.Parent.FoundElement;
                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript("arguments[0].value = '';", el);
                                el.SendKeys(value);
                            }
                            break;
                        case StepTypes.SetAttribute:
                            {
                                needToSlow = true;
                                string value = ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                var el = this.Parent.FoundElement;
                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", el, this.Selector, value);
                            }
                            break;
                        case StepTypes.Click:
                            {
                                needToSlow = true;
                                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                                var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                                el.Click();
                            }
                            break;
                        case StepTypes.JsClick:
                            {
                                needToSlow = true;
                                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                                var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", el);
                            }
                            break;
                        case StepTypes.AltClick:
                            {
                                needToSlow = true;
                                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                                var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                                Actions action = new Actions(driver);
                                action.Click(el).Build().Perform();
                            }
                            break;
                        case StepTypes.DoubleClick:
                            {
                                needToSlow = true;
                                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
                                var el = wait.Until(ExpectedConditions.ElementToBeClickable(this.Parent.FoundElement));
                                Actions action = new Actions(driver);
                                action.DoubleClick(el).Build().Perform();
                            }
                            break;
                        case StepTypes.JsEvent:
                            {
                                needToSlow = true;
                                var el = this.Parent.FoundElement;
                                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript($"arguments[0].dispatchEvent(new Event('{Value}'));", el);
                            }
                            break;
                        case StepTypes.CheckText:
                            {
                                var el = this.Parent.FoundElement;
                                if (!IsMatchMask(el.Text, ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)))
                                {
                                    throw new Exception($"Ожидалось [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}], было [{el.Text}]");
                                }
                            }
                            break;
                        case StepTypes.CheckAttribute:
                            {
                                var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                var el = this.Parent.FoundElement;
                                if (el.GetAttribute(selector) == null)
                                {
                                    throw new Exception($"Атрибут {selector} не найден");
                                }
                                if (!IsMatchMask(el.GetAttribute(selector), ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)))
                                {
                                    var errValue = el.GetAttribute(selector);
                                    throw new Exception($"Ожидалось [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}], было [{errValue}]");
                                }
                            }
                            break;
                        case StepTypes.CheckClassExists:
                            {
                                if (!this.Parent.FoundElement.GetAttribute("class").Contains(ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)))
                                {
                                    throw new Exception($"Класс [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}] не найден в элементе");
                                }
                            }
                            break;
                        case StepTypes.CheckClassNotExists:
                            {
                                if (this.Parent.FoundElement.GetAttribute("class").Contains(ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)))
                                {
                                    throw new Exception($"Класс [{ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters)}] не найден в элементе");
                                }
                            }
                            break;
                        case StepTypes.WaitTime:
                            {
                                Thread.Sleep((int)(this.SecondsToWait * 1000f));
                            }
                            break;
                        case StepTypes.ReadAttributeToParameter:
                            {
                                var el = this.Parent.FoundElement;
                                var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                var value = el.GetAttribute(selector) ?? "";
                                SetParameter(value, this.Parameter);
                            }
                            break;
                        case StepTypes.ReadTextToParameter:
                            {
                                var el = this.Parent.FoundElement;
                                var value = el.Text;
                                SetParameter(value, this.Parameter);
                            }
                            break;
                        case StepTypes.ReadAddressToParameter:
                            {
                                var value = driver.Url;
                                SetParameter(value, this.Parameter);
                            }
                            break;
                        case StepTypes.CompareParameters:
                            {
                                var value1 = ValuesFromParameters.ProcessInput(this.Value, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                var value2 = ValuesFromParameters.ProcessInput(this.Parameter, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                                if (value1 != value2)
                                {
                                    throw new Exception($"Param1 [{value1}] ({value1.GetType()})\r\nParam2 [{value2}] ({value2.GetType()})");
                                }
                            }
                            break;
                        case StepTypes.InputToParameterByUser:
                            {
                                InputDataForm inputDataForm = new InputDataForm(this.Parameter, this.Name);
                                if (inputDataForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    throw new Exception($"Не введён параметр");
                                }
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
                            }
                            break;
                        default:
                            needToSlow = false;
                            break;
                    }
                    StateUpdated?.Invoke();
                    if (needToSlow && slowMode)
                    {
                        Thread.Sleep(1000);
                    }
                    Error = "";
                    this.StepState = StepStates.Passed;
                }
                catch (StaleElementReferenceException ex)
                {
                    staleErrorCount++;
                    if (staleErrorCount < 10)
                    {
                        Thread.Sleep(1000);
                        StepWork(driver, StateUpdated, slowMode, selectFoundElements, canIgnoreErrorIfStepIgnoringThem, staleErrorCount);
                    }
                    else
                    {
                        var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                        this.Error = $"Элемент {SelectorType}=\"{selector}\" невалиден по неизвестной причине после 10 секунд ожидания \r\n\r\n" + ex.ToString();
                        throw;
                    }
                }
                catch (NoSuchElementException ex)
                {
                    var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                    this.Error = $"Элемент {SelectorType}=\"{selector}\" не найден\r\n\r\n" + ex.ToString();
                    throw;
                }
                catch (InvalidSelectorException ex)
                {
                    var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                    this.Error = $"Селектор имеет ошибку {SelectorType}=\"{selector}\"\r\n\r\n" + ex.ToString();
                    throw;
                }
                catch (WebDriverTimeoutException ex)
                {
                    var selector = ValuesFromParameters.ProcessInput(this.Selector, ParentAutotest.ParentProject.Parameters, ParentAutotest.Parameters);
                    this.Error = $"Элемент {SelectorType}=\"{selector}\" не найден за отведённое время\r\n\r\n" + ex.ToString();
                    throw;
                }
                catch (Exception ex)
                {
                    this.Error = ex.ToString();
                    throw;
                }
            }
            catch
            {
                this.StepState = (this.IgnoreError && canIgnoreErrorIfStepIgnoringThem) ? StepStates.IgnoredError : StepStates.Error;
                if (this.StepState == StepStates.Error)
                {
                    ParentAutotest.ErrorStep = this;
                    throw;
                }
            }
        }

        private string GetElementXPath(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (string)js.ExecuteScript(
                "function getElementXPath(element) {" +
                "   if (element.id !== '') {" +
                "       return 'id(\"' + element.id + '\")';" +
                "   }" +
                "   if (element === document.body) {" +
                "       return element.tagName;" +
                "   }" +
                "   var ix = 0;" +
                "   var siblings = element.parentNode.childNodes;" +
                "   for (var i = 0; i < siblings.length; i++) {" +
                "       var sibling = siblings[i];" +
                "       if (sibling === element) {" +
                "           return getElementXPath(element.parentNode) + '/' + element.tagName + '[' + (ix + 1) + ']';" +
                "       }" +
                "       if (sibling.nodeType === 1 && sibling.tagName === element.tagName) {" +
                "           ix++;" +
                "       }" +
                "   }" +
                "}" +
                "return getElementXPath(arguments[0]);", element);
        }

        private bool IsChildOf(IWebDriver driver, IWebElement child, IWebElement parent)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (bool)js.ExecuteScript(
                "var child = arguments[0];" +
                "var parent = arguments[1];" +
                "while (child.parentNode) {" +
                "    if (child.parentNode === parent) {" +
                "        return true;" +
                "    }" +
                "    child = child.parentNode;" +
                "}" +
                "return false;",
                child, parent);
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

        private bool IsMatchMask(string input, string pattern)
        {
            pattern = pattern.Replace("\\?", ".").Replace("\\*", ".*");
            string regexPattern = "^" + pattern + "$";
            return Regex.IsMatch(input, regexPattern);
        }

        public void ClearState()
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

        public void AutoModeRun(IWebDriver driver, CancellationToken token, Action StateUpdated, bool slowMode, bool selectFoundElements)
        {
            StepWork(driver, StateUpdated, slowMode, selectFoundElements);

            foreach (var substep in Substeps)
            {
                if (token.IsCancellationRequested)
                    break;
                substep.AutoModeRun(driver, token, StateUpdated, slowMode, selectFoundElements);
            }
        }

        public void StepModeRun(IWebDriver driver, Action StateUpdated, bool selectFoundElements)
        {
            StepWork(driver, StateUpdated, false, selectFoundElements, false);
        }
    }
}
