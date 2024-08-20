using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace SeleniumAutotest
{
    public partial class Form1 : Form
    {
        // TODO:
        // translate
        // refactoring
        // DONT CLOSE BROWSER IF ERROR
        private const string Version = "v1.7.1";
        private const string AppName = "Selenium Autotest IDE " + Version;

        private Project Project { get; set; }

        bool NeedUpdateTestFields = true;
        bool NeedToReselectAutotest = true;

        public Form1()
        {
            InitializeComponent();
            Text = AppName + " - Новый проект";
            Project = new Project();
            Project.ParametersUpdated += Project_ParametersGenerated;
            Project.RunAutotestFinished += Project_RunAutotestFinished;
            Project.SelectedAutotestChanged += Project_SelectedAutotestChanged;
            ChProjectRegenerateParameters.Checked = Project.RegenerateParametersOnRun;
            LiTests.DataSource = Project.Autotests;

            var selectorTypes = Enum.GetValues(typeof(SelectorType)).Cast<SelectorType>().Select(v => v.ToString()).ToList();
            foreach (var item in selectorTypes)
            {
                CoStepSelectorType.Items.Add(item);
            }
        }

        #region Buttons

        private void BuFileOpenLast_Click(object sender, EventArgs e)
        {
            if (File.Exists("./LastFilePath.txt"))
            {
                string path = File.ReadAllText("./LastFilePath.txt");
                if (File.Exists(path))
                {
                    Project.ParametersUpdated -= Project_ParametersGenerated;
                    Project.RunAutotestFinished -= Project_RunAutotestFinished;
                    Project.SelectedAutotestChanged -= Project_SelectedAutotestChanged;
                    string str = File.ReadAllText(path);
                    Project = JsonConvert.DeserializeObject<Project>(str);
                    foreach (Autotest autotest in Project.Autotests)
                    {
                        autotest.ResetGuid();
                    }
                    Project.ResetAllParentsForAutotests();
                    Project.ParametersUpdated += Project_ParametersGenerated;
                    Project.RunAutotestFinished += Project_RunAutotestFinished;
                    Project.SelectedAutotestChanged += Project_SelectedAutotestChanged;
                    ChProjectRegenerateParameters.Checked = Project.RegenerateParametersOnRun;
                    Text = $"{AppName} - {Project.Name}";
                    LiTests.DataSource = Project.Autotests;
                    Project_ParametersGenerated();
                }
            }
        }

        private void BuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "*.autotest|*.autotest"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Project.ParametersUpdated -= Project_ParametersGenerated;
                Project.RunAutotestFinished -= Project_RunAutotestFinished;
                Project.SelectedAutotestChanged -= Project_SelectedAutotestChanged;
                string str = File.ReadAllText(ofd.FileName);
                Project = JsonConvert.DeserializeObject<Project>(str);
                foreach (Autotest autotest in Project.Autotests)
                {
                    autotest.ResetGuid();
                }
                Project.ResetAllParentsForAutotests();
                Project.ParametersUpdated += Project_ParametersGenerated;
                Project.RunAutotestFinished += Project_RunAutotestFinished;
                Project.SelectedAutotestChanged += Project_SelectedAutotestChanged;
                ChProjectRegenerateParameters.Checked = Project.RegenerateParametersOnRun;
                Text = $"{AppName} - {Project.Name}";
                LiTests.DataSource = Project.Autotests;
                File.WriteAllText("./LastFilePath.txt", ofd.FileName);
                Project_ParametersGenerated();
            }
        }

        private void BuFileSave_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void BuTestAdd_Click(object sender, EventArgs e)
        {
            Project.AddAutotest(new Autotest());
            LiTests_SelectedIndexChanged(LiTests, new EventArgs());
        }

        private void BuTestDelete_Click(object sender, EventArgs e)
        {
            Project.DeleteSelectedAutotest();
            LiTests_SelectedIndexChanged(LiTests, new EventArgs());
        }

        private void BuTestClone_Click(object sender, EventArgs e)
        {
            Project.CloneSelectedAutotest();
        }

        private void BuTestUp_Click(object sender, EventArgs e)
        {
            Project.TestMoveUp();
            LiTests.SelectedItem = Project.SelectedAutotest;
        }

        private void BuTestDown_Click(object sender, EventArgs e)
        {
            Project.TestMoveDown();
            LiTests.SelectedItem = Project.SelectedAutotest;
        }


        private void BuStepAdd_Click(object sender, EventArgs e)
        {
            StepAdd();
        }

        private void BuStepDelete_Click(object sender, EventArgs e)
        {
            StepDelete();
        }

        private void BuStepClone_Click(object sender, EventArgs e)
        {
            StepClone();
        }


        private void BuStepUp_Click(object sender, EventArgs e)
        {
            StepMoveUp();
        }

        private void BuStepDown_Click(object sender, EventArgs e)
        {
            StepMoveDown();
        }

        private void BuStepClearFocus_Click(object sender, EventArgs e)
        {
            StepDisfocus();
        }

        private void BuStepReloadTree_Click(object sender, EventArgs e)
        {
            ReloadTree();
        }

        private void BuStepCopy_Click(object sender, EventArgs e)
        {
            StepCopy();
        }

        private void BuStepPaste_Click(object sender, EventArgs e)
        {
            StepPaste();
        }

        private void BuFontIncrease_Click(object sender, EventArgs e)
        {
            if (TrSteps.Font.Size > 20) { return; }
            TrSteps.Font = new Font(TrSteps.Font.FontFamily, TrSteps.Font.Size + 1, TrSteps.Font.Style);
        }

        private void BuFontDecrease_Click(object sender, EventArgs e)
        {
            if (TrSteps.Font.Size < 9) { return; }
            TrSteps.Font = new Font(TrSteps.Font.FontFamily, TrSteps.Font.Size - 1, TrSteps.Font.Style);
        }

        private void BuTestParametersHelp_Click(object sender, EventArgs e)
        {
            string str = "Параметры поддерживают автогенерацию и скриптинг\r\n";
            str += "/randomDX/ - генерация X цифр (0-9)\r\n";
            str += "/randomLX/ - генерация X букв (a-Z)\r\n";
            str += "/randomCX/ - генерация X знаков (0-9,a-Z)\r\n";
            str += "/null/ - содержание этого макроса всегда вернёт null-строку\r\n";
            str += "%Parameter% - Использование значения другого параметра (работает до 3-й вложенности)\r\n";
            str += "^CODE^ - Использование простого кода после применения макросов\r\n\r\n";

            str += "Param0 = 11 => \"11\"\r\n";
            str += "PARAM1 = A127B => \"A127B\"\r\n";
            str += "Param2 = Test2%PARAM1%Test2 => \"Test2A127BTest2\"\r\n";
            str += "Param3 = Test3%Param2%/randomD2/Test3 => Test3Test2A127BTest2/randomD2/Test3 => \"Test3Test2A127BTest274Test3\"\r\n\r\n";

            str += "ParamNull = TestN%PARAM1%/randomD2//null/TestN => null\r\n\r\n";

            str += "TODAY = ^DateTime.Now.ToShortDateString()^ => текущая дата в формате dd.mm.yyyy (для RU-локали)\r\n";
            str += "SUMM = ^(10+%Param0%).ToString()^ => ^(10+11).ToString()^ => \"21\"\r\n\r\n";

            str += "Не используйте одинаковые имена для параметров проекта и параметров теста\r\n\r\n";

            str += "Для параметров, которые будут заполнены во время выполнения автотестов оставьте пустой шаблон\r\n\r\n";

            str += "Параметры, автогенерацию и скриптинг можно использовать в полях Значение и Селектор\r\n\r\n";

            str += "Поле Значение для сравнивающих шагов поддерживает маскирование (\\? - 1 символ, \\* - несколько символов), т.е. проверку на \"a\\?c\\*\" пройдёт, например, строка \"abcdefg\" \r\n\r\n";
            MessageBox.Show(str, "Справка по параметрам");
        }

        private void BuProjectGenerateParameters_Click(object sender, EventArgs e)
        {
            Project.GenerateParameters();
        }

        private void BuTestGenerateParameters_Click(object sender, EventArgs e)
        {
            if (Project.SelectedAutotest == null) { return; }
            Project.SelectedAutotest.GenerateParameters();
        }

        #endregion Buttons

        #region StepFields
        private void TrSteps_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ChangeSelectedStep(e.Node);
        }

        private void SetStepFieldsVisible(StepTypes type)
        {
            LaStepValue.Text = "Значение";
            TeStepSelector.Visible = false;
            CoStepSelectorType.Visible = false;
            TeStepValue.Visible = false;
            NuStepWait.Visible = false;
            ChStepIgnoreError.Visible = false;
            TeStepParameter.Visible = false;
            ChStepScrollTo.Visible = false;
            ChStepIgnoreParent.Visible = false;
            switch (type)
            {
                case StepTypes.Group:
                case StepTypes.RefreshPage:
                case StepTypes.Click:
                case StepTypes.AltClick:
                case StepTypes.JsClick:
                case StepTypes.DoubleClick:
                case StepTypes.ScrollTo:
                    break;
                case StepTypes.FindElement:
                    CoStepSelectorType.Visible = true;
                    TeStepSelector.Visible = true;
                    NuStepWait.Visible = true;
                    ChStepScrollTo.Visible = true;
                    ChStepIgnoreParent.Visible = true;
                    break;
                case StepTypes.CheckElement:
                    CoStepSelectorType.Visible = true;
                    TeStepSelector.Visible = true;
                    NuStepWait.Visible = true;
                    ChStepIgnoreError.Visible = true;
                    ChStepIgnoreParent.Visible = true;
                    break;
                case StepTypes.Open:
                    TeStepValue.Visible = true;
                    break;
                case StepTypes.CheckText:
                    TeStepValue.Visible = true;
                    ChStepIgnoreError.Visible = true;
                    break;
                case StepTypes.JsEvent:
                case StepTypes.EnterText:
                    TeStepValue.Visible = true;
                    break;
                case StepTypes.WaitTime:
                    NuStepWait.Visible = true;
                    break;
                case StepTypes.CheckAttribute:
                    TeStepSelector.Visible = true;
                    TeStepValue.Visible = true;
                    ChStepIgnoreError.Visible = true;
                    break;
                case StepTypes.CheckClassExists:
                case StepTypes.CheckClassNotExists:
                    TeStepValue.Visible = true;
                    ChStepIgnoreError.Visible = true;
                    break;
                case StepTypes.SetAttribute:
                    TeStepSelector.Visible = true;
                    TeStepValue.Visible = true;
                    break;
                case StepTypes.ReadAttributeToParameter:
                    TeStepSelector.Visible = true;
                    TeStepParameter.Visible = true;
                    break;
                case StepTypes.ReadAddressToParameter:
                case StepTypes.ReadTextToParameter:
                case StepTypes.InputToParameterByUser:
                    TeStepParameter.Visible = true;
                    break;
                case StepTypes.CompareParameters:
                    LaStepValue.Text = "Параметр";
                    TeStepValue.Visible = true;
                    TeStepParameter.Visible = true;
                    ChStepIgnoreError.Visible = true;
                    break;
                case StepTypes.ScrollByPixels:
                    TeStepValue.Visible = true;
                    break;
                case StepTypes.JsCode:
                    TeStepValue.Visible = true;
                    break;
            }
        }

        private void UpdateStepField(string fieldName, object value)
        {
            if (NeedUpdateTestFields == false) { return; }
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            Type type = typeof(TestStep);
            PropertyInfo pi = type.GetProperty(fieldName);
            pi.SetValue(selectedStep, value);
            //ReloadTree();
        }

        private void CoStepType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NeedUpdateTestFields == false) { return; }
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            if (selectedStep.Substeps.Count > 0 && MessageBox.Show("Продолжить?", "Изменение типа приведёт к удалению дочерних элементов", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                NeedUpdateTestFields = false;
                CoStepType.SelectedItem = StepType.Descriptions[selectedStep.Type];
                NeedUpdateTestFields = true;
                return;
            }

            selectedStep.Substeps.Clear();

            TeStepName.Text = CoStepTypeGroup.Text + " " + CoStepType.Text;
            UpdateStepField(nameof(TestStep.Type), StepType.GetTypeByNameAndGroup(CoStepType.SelectedItem.ToString(), CoStepTypeGroup.SelectedItem.ToString()));
            SetStepFieldsVisible(selectedStep.Type);
            //ReloadTree();
        }

        private void ChStepParameterChanged(object sender, EventArgs e)
        {
            try
            {
                var control = sender as Control;
                object value = null;

                if (sender is CheckBox checkBox) { value = checkBox.Checked; }
                else if (sender is TextBox textBox) { value = textBox.Text; }
                else if (sender is ComboBox comboBox) { value = comboBox.SelectedIndex; }
                else if (sender is NumericUpDown numericUpDown) { value = (float)numericUpDown.Value; }
                else { throw new NotSupportedException("Unsupported control type"); }
                UpdateStepField((string)control.Tag, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        #endregion StepFields

        private void LiTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Autotest)LiTests.SelectedItem == null)
            {
                GrTestSettings.Enabled = false;
                GrTestSteps.Enabled = false;
                GrTestParameters.Enabled = false;
                return;
            }
            GrTestSettings.Enabled = true;
            GrTestSteps.Enabled = true;
            GrTestParameters.Enabled = true;

            if (NeedToReselectAutotest)
            {
                Project.SelectAutotest((Autotest)LiTests.SelectedItem);
            }

            TeTestName.Text = Project.SelectedAutotest.Name;
            LaTestTime.Text = "Время выполнения теста: " + Project.SelectedAutotest.CompleteTime;
            ChTestRegenerateParameters.Checked = Project.SelectedAutotest.RegenerateParametersOnRun;
            ChTestRunAfterPrevious.Checked = Project.SelectedAutotest.RunAfterPrevious;

            foreach (var test in Project.Autotests)
            {
                test.StateUpdated -= AutotestUpdated;
                test.ParametersUpdated -= SelectedAutotest_ParametersGenerated;
            }
            Project.SelectedAutotest.StateUpdated += AutotestUpdated;
            Project.SelectedAutotest.ParametersUpdated += SelectedAutotest_ParametersGenerated;

            Project.SelectedAutotest.ResetAllParentsForSteps(Project);
            ReloadTree();
            SelectedAutotest_ParametersGenerated();
        }

        private void SelectedAutotest_ParametersGenerated()
        {
            this.Invoke(new Action(() =>
            {
                if (Project.SelectedAutotest == null) { return; }

                NeedUpdateTestFields = false;
                DaTestParameters.Rows.Clear();
                foreach (var param in Project.SelectedAutotest.Parameters)
                {
                    DaTestParameters.Rows.Add(new object[] { param.Name, param.Pattern, param.Value });
                }
                NeedUpdateTestFields = true;
            }));
        }

        private void Project_RunAutotestFinished(string errorMsg)
        {
            this.Invoke(new Action(() =>
            {
                BuTestStop.Enabled = false;
                LaTestTime.Text = "Время выполнения теста: " + Project.SelectedAutotest.CompleteTime;
                LaRunTime.Text = "Время выполнения общее: " + Project.RunTime;
                ReloadTree();
                if (errorMsg != null && !StepMode)
                {
                    TreeNode nodeToBeSelected = FindNodeByGuid(TrSteps.Nodes, (Guid)Project.SelectedAutotest.ErrorStep.Id);
                    if (nodeToBeSelected != null)
                    {
                        TrSteps.SelectedNode = nodeToBeSelected;
                    }
                    MessageBox.Show(errorMsg, "Произошла ошибка во время выполнения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RiLog.Text = errorMsg;
                }
                StepMode = false;
                AutoMode = false;
                BuTestRun.Enabled = true;
                LiTests.Enabled = true;
                BuTestRunStepMode.Enabled = true;
                BuTestStepModePrev.Enabled = false;
            }));
        }

        private void Project_SelectedAutotestChanged()
        {
            this.Invoke(new Action(() =>
            {
                NeedToReselectAutotest = false;
                LiTests.SelectedItem = Project.SelectedAutotest;
                NeedToReselectAutotest = true;
                if (Project.SelectedAutotest.ErrorStep != null)
                {
                    TreeNode nodeToBeSelected = FindNodeByGuid(TrSteps.Nodes, (Guid)Project.SelectedAutotest.ErrorStep.Id);
                    if (nodeToBeSelected != null)
                    {
                        TrSteps.SelectedNode = nodeToBeSelected;
                    }
                }
            }));
        }

        private void AutotestUpdated()
        {
            this.Invoke(new Action(() =>
            {
                ReloadTree();
            }));
        }

        private void ReloadTree()
        {
            if (Project.SelectedAutotest == null) { return; }
            var selectedNode = TrSteps.SelectedNode;

            TrSteps.Nodes.Clear();
            AddTestStepsToNodes(TrSteps.Nodes, Project.SelectedAutotest.Root.Substeps);

            if (selectedNode != null && selectedNode.Tag != null)
            {
                TreeNode nodeToBeSelected = FindNodeByGuid(TrSteps.Nodes, (Guid)selectedNode.Tag);
                if (nodeToBeSelected != null)
                {
                    TrSteps.SelectedNode = nodeToBeSelected;
                }
            }
        }
        private TreeNode FindNodeByGuid(TreeNodeCollection nodes, Guid targetGuid)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is Guid nodeGuid && nodeGuid == targetGuid)
                {
                    return node;
                }

                TreeNode foundNode = FindNodeByGuid(node.Nodes, targetGuid);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null;
        }

        private void AddTestStepsToNodes(TreeNodeCollection parentNodeCollection, List<TestStep> substeps)
        {
            foreach (var step in substeps)
            {
                var node = parentNodeCollection.Add(step.ToString());
                node.Tag = step.Id;
                node.ImageIndex = StepType.GetIndexOfGroupByType(step.Type) + 1;
                switch (step.StepState)
                {
                    case StepStates.NotStarted:
                        node.BackColor = Color.White;
                        break;
                    case StepStates.Passed:
                        node.BackColor = Color.LightGreen;
                        break;
                    case StepStates.Error:
                        node.BackColor = Color.Red;
                        break;
                    case StepStates.IgnoredError:
                        node.BackColor = Color.Yellow;
                        break;
                    case StepStates.Skipped:
                        node.BackColor = Color.GreenYellow;
                        break;
                    case StepStates.Started:
                        node.BackColor = Color.LightGray;
                        break;
                }
                node.ForeColor = Color.Black;
                if (step.IgnoreError || !step.Enabled)
                {
                    if (step.IgnoreError)
                    {
                        node.ForeColor = Color.Gray;
                    }
                    if (!step.Enabled)
                    {
                        node.ForeColor = Color.LightGray;
                    }
                }
                node.Checked = step.Enabled;

                AddTestStepsToNodes(node.Nodes, step.Substeps);
                if (step.Expanded)
                {
                    node.Expand();
                }
            }
        }

        private void TeTestName_TextChanged(object sender, EventArgs e)
        {
            if (Project.SelectedAutotest == null) { return; }

            Project.SelectedAutotest.Name = TeTestName.Text;

            Project.Autotests.ResetBindings();
        }

        private void Project_ParametersGenerated()
        {
            this.Invoke(new Action(() =>
            {
                NeedUpdateTestFields = false;
                DaProjectParameters.Rows.Clear();
                foreach (var param in Project.Parameters)
                {
                    DaProjectParameters.Rows.Add(new object[] { param.Name, param.Pattern, param.Value });
                }
                NeedUpdateTestFields = true;
            }));
        }

        private void TrSteps_ChangeExpanded(object sender, TreeViewEventArgs e)
        {
            if (Project.SelectedAutotest == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)e.Node.Tag);

            selectedStep.Expanded = e.Node.IsExpanded;
        }

        private void ChTestRegenerateParameters_CheckedChanged(object sender, EventArgs e)
        {
            if (Project.SelectedAutotest == null) { return; }

            Project.SelectedAutotest.RegenerateParametersOnRun = ChTestRegenerateParameters.Checked;
        }

        private void DaTestParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            RefreshTestParameters();
        }

        private void DaProjectParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            RefreshProjectParameters();
        }

        private void ChProjectRegenerateParameters_CheckedChanged(object sender, EventArgs e)
        {
            Project.RegenerateParametersOnRun = ChProjectRegenerateParameters.Checked;
        }

        private void ChTestRunAfterPrevious_CheckedChanged(object sender, EventArgs e)
        {
            Project.SelectedAutotest.RunAfterPrevious = ChTestRunAfterPrevious.Checked;
        }

        private void StepAdd()
        {
            if (Project.SelectedAutotest == null) { return; }

            try
            {
                StepTypesGroup group;
                List<TestStep> stepsToAdd;
                List<StepTypes> appliableTypes;
                if (TrSteps.SelectedNode != null)
                {
                    var parentStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

                    group = StepType.StepTypesGroups.First(x => x.Parents.Contains(parentStep.Type));
                    appliableTypes = group.Types;
                    stepsToAdd = parentStep.Substeps;
                }
                else
                {
                    group = StepType.StepTypesGroups.First(x => x.Parents.Count == 0 || x.Parents.Contains(StepTypes.Group));
                    appliableTypes = group.Types;
                    stepsToAdd = Project.SelectedAutotest.Root.Substeps;
                }

                if (appliableTypes.Count == 0)
                {
                    return;
                }
                var type = appliableTypes.First();
                stepsToAdd.Add(new TestStep()
                {
                    Type = type,
                    Name = group.Name + " " + StepType.Descriptions[type]
                });

                Project.SelectedAutotest.ResetAllParentsForSteps(Project);
                ReloadTree();
            }
            catch { }
        }

        private void StepDelete()
        {
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            selectedStep.Parent.Substeps.Remove(selectedStep);
            Project.SelectedAutotest.ResetAllParentsForSteps(Project);
            ReloadTree();
        }

        private void StepClone()
        {
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            var step = selectedStep.Clone();
            step.ResetGuid();
            selectedStep.Parent.Substeps.Add(step);
            Project.SelectedAutotest.ResetAllParentsForSteps(Project);
            ReloadTree();
        }

        private void StepMoveUp()
        {
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            selectedStep.MoveUp();

            Project.SelectedAutotest.ResetAllParentsForSteps(Project);
            ReloadTree();
        }

        private void StepMoveDown()
        {
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            selectedStep.MoveDown();

            Project.SelectedAutotest.ResetAllParentsForSteps(Project);
            ReloadTree();
        }

        private void StepCopy()
        {
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            string str = JsonConvert.SerializeObject(selectedStep);
            Clipboard.SetText(str);
        }

        private void StepPaste()
        {
            if (Project.SelectedAutotest == null) { return; }

            if (TrSteps.SelectedNode != null)
            {
                var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);
                try
                {
                    var step = JsonConvert.DeserializeObject<TestStep>(Clipboard.GetText());
                    step.ResetGuid();
                    selectedStep.Substeps.Add(step);
                }
                catch { }
            }
            else
            {
                try
                {
                    var step = JsonConvert.DeserializeObject<TestStep>(Clipboard.GetText());
                    step.ResetGuid();
                    Project.SelectedAutotest.Root.Substeps.Add(step);
                }
                catch { }
            }

            Project.SelectedAutotest.ResetAllParentsForSteps(Project);
            ReloadTree();
        }

        private void StepDisfocus()
        {
            TrSteps.SelectedNode = null;
        }

        private void ChangeSelectedStep(TreeNode node)
        {
            if (node == null) { return; }
            if (Project.SelectedAutotest == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)node.Tag);

            NeedUpdateTestFields = false;

            CoStepTypeGroup.Items.Clear();
            foreach (var group in StepType.StepTypesGroups.Where(x => x.Parents.Contains(selectedStep.Parent?.Type)))
            {
                CoStepTypeGroup.Items.Add(group);
            }
            CoStepTypeGroup.SelectedItem = StepType.StepTypesGroups.First(x => x.Types.Contains(selectedStep.Type));

            CoStepType.Items.Clear();
            foreach (var type in ((StepTypesGroup)CoStepTypeGroup.SelectedItem).Types)
            {
                CoStepType.Items.Add(StepType.Descriptions[type]);
            }
            CoStepType.SelectedItem = StepType.Descriptions[selectedStep.Type];

            TeStepName.Text = selectedStep.Name;
            TeStepSelector.Text = selectedStep.Selector;
            CoStepSelectorType.SelectedIndex = (int)selectedStep.SelectorType;
            TeStepValue.Text = selectedStep.Value;
            TeStepParameter.Text = selectedStep.Parameter;
            ChStepIgnoreError.Checked = selectedStep.IgnoreError;
            ChStepIsEnabled.Checked = selectedStep.Enabled;
            ChStepIgnoreParent.Checked = selectedStep.IgnoreParent;
            ChStepScrollTo.Checked = selectedStep.ScrollTo;
            try
            {
                NuStepWait.Value = (decimal)selectedStep.SecondsToWait;
            }
            catch
            {
                NuStepWait.Value = (decimal)0.001f;
            }
            RiLog.Text = selectedStep.Log;


            SetStepFieldsVisible(selectedStep.Type);
            NeedUpdateTestFields = true;
        }

        private void TrSteps_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                ReloadTree();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                StepDisfocus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            if (e.Control) // CTRL is pressed
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        StepAdd();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Delete:
                        StepDelete();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.D:
                        StepClone();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Up:
                        StepMoveUp();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Down:
                        StepMoveDown();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.C:
                        StepCopy();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.V:
                        StepPaste();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                }
            }
            else
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    ChangeSelectedStep(TrSteps.SelectedNode);
                }
            }
        }

        private void BuCantDownloadDriverHelp_Click(object sender, EventArgs e)
        {
            string msg = "Чтобы использовать программу без доступа к интернету требуется:\r\n";
            msg += "1. Узнайте версию браузера в свойствах браузера\r\n";
            msg += "2. Скачайте ChromeDriver этой версии (главное, чтобы первое число совпадало, т.е. из XXX.YYY.ZZZ, достаточно совпадения XXX)\r\n";
            msg += "3. Положите chromedriver.exe в папку ./Chrome/DEF/ (если её нет - создать)\r\n";
            msg += "???\r\n";
            msg += "PROFIT\r\n\r\n";
            msg += "Перезапускать программу не потребуется, достаточно перезапустить автотест.";
            MessageBox.Show(msg, "Ручное скачивание драйвера");
        }

        private void CoStepTypeGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NeedUpdateTestFields == false) { return; }
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            if (selectedStep.Substeps.Count > 0 && MessageBox.Show("Продолжить?", "Изменение типа приведёт к удалению дочерних элементов", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                NeedUpdateTestFields = false;
                CoStepTypeGroup.SelectedItem = StepType.StepTypesGroups.FirstOrDefault(x => x.Types.Contains(selectedStep.Type));
                NeedUpdateTestFields = true;
                return;
            }

            selectedStep.Substeps.Clear();

            CoStepType.Items.Clear();
            foreach (var type in ((StepTypesGroup)CoStepTypeGroup.SelectedItem).Types)
            {
                CoStepType.Items.Add(StepType.Descriptions[type]);
            }
            CoStepType.SelectedIndex = 0;
            TeStepName.Text = CoStepTypeGroup.Text + " " + CoStepType.Text;
            ReloadTree();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopTests();
            var res = MessageBox.Show("Желаете сохранить проект перед выходом?", "Выход", MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            if (res == DialogResult.Yes)
            {
                SaveProject();
            }
        }

        private void SaveProject()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "*.autotest|*.autotest"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Project.Name = Path.GetFileNameWithoutExtension(sfd.FileName);
                Text = $"{AppName} - {Project.Name}";
                string str = JsonConvert.SerializeObject(Project);
                File.WriteAllText(sfd.FileName, str);
                File.WriteAllText("./LastFilePath.txt", sfd.FileName);
            }
        }

        private void Control_FocusLeave(object sender, EventArgs e)
        {
            ReloadTree();
        }


        bool StepMode = false;
        bool AutoMode = false;

        private void BuTestRun_Click(object sender, EventArgs e)
        {
            AutoMode = true;
            Project.RunAutotest(ChSlowMode.Checked, ChSelectFoundElements.Checked);
            BuTestRun.Enabled = false;
            BuTestStop.Enabled = true;
            BuTestRunStepMode.Enabled = false;
            LiTests.Enabled = false;
        }

        private void BuTestStop_Click(object sender, EventArgs e)
        {
            StopTests();
        }

        private void StopTests()
        {
            BuTestStop.Enabled = false;
            if (StepMode)
            {
                Project.StepModeStop();
                StepMode = false;
            }
            if (AutoMode)
            {
                Project.StopAutotest();
                AutoMode = false;
            }
            BuTestRun.Enabled = true;
            BuTestRunStepMode.Enabled = true;
            BuTestStepModePrev.Enabled = false;
            LiTests.Enabled = true;
            Opacity = 1;
            ReloadTree();
        }

        private void BuTestRunStepMode_Click(object sender, EventArgs e)
        {
            if (!StepMode)
            {
                if (Project.StepModeRun(ChSelectFoundElements.Checked))
                {
                    StepMode = true;
                    BuTestStop.Enabled = true;
                    BuTestStepModePrev.Enabled = true;
                    BuTestRun.Enabled = false;
                    LiTests.Enabled = false;
                }
                Activate();
                return;
            }

            Opacity = 0;
            Project.StepModeContinue(ChSelectFoundElements.Checked);
            ReloadTree();
            Opacity = 1;
        }

        private void BuTestStepModePrev_Click(object sender, EventArgs e)
        {
            Project.StepModeStepBack();
        }

        private void BuXpathHelp_Click(object sender, EventArgs e)
        {
            string msg = "XPATH - адрес элемента на странице\r\n";
            msg += "/ - поиск среди дочерних элементов первого уровня, // - поиск среди всех дочерних элементов\r\n";
            msg += "После идёт имя тега (div, span, li и т.п.), либо * - любой элемент\r\n";
            msg += "Затем можно описать условия по атрибутам или тексту в []\r\n";
            msg += "Для проверки текста - text()\r\n";
            msg += "Имена атрибутов надо начинать с @ (@class, @id, @data)\r\n\r\n";

            msg += "//div - первый div на странице\r\n";
            msg += "//*[@id='asd'] - любой элемент на странице, у которого id = asd\r\n";
            msg += "//*[text()='asd'] - любой элемент на странице, у которого текст = asd\r\n";
            msg += "//*[contains(@class, 'asd')] - любой элемент на странице, у которого есть класс asd\r\n";
            msg += "В условиях [] поддерживаются логические операнды and/or\r\n\r\n";

            msg += "//div[contains(@class, 'asd') and @data-item-marker='qwe'] - первый div на странице, у которого есть класс asd и атрибут data-item-marker = qwe\r\n\r\n";

            msg += "Для поиска дочерних элементов в подшагах надо использовать точку вначале\r\n";
            msg += ".//div - поиск первого дочернего элемента div в родителе\r\n\r\n";

            msg += "Можно сразу искать нужные дочерние элементы по структуре\r\n";
            msg += "//div[contains(@class, 'asd') and @data-item-marker='qwe']//span[text()='zxc'] - сначала ищется первый div на странице, у которого есть класс asd и атрибут data-item-marker = qwe, затем в нём ищется span с текстом zxc\r\n\r\n";
            msg += "//div[@id='asd']/span - сначала ищется первый div с id = asd, затем в нём выбирается первый span\r\n\r\n";

            msg += "В поиске элемента можно переходить в родителя с помощью /..\r\n";
            msg += "//div/..//span - сначала находим первый span, затем переходим в родителя и ищем в родителе span\r\n\r\n";
            MessageBox.Show(msg, "Справка по XPATH");
        }

        private void BuAbout_Click(object sender, EventArgs e)
        {
            string msg = "Автор: Трофимов Александр (alextrof94)\r\n";
            msg += "Программа распространяется бесплатно\r\n\r\n";
            msg += "Изображения взяты с сайта https://icons8.com";
            MessageBox.Show(msg, "О программе");
        }

        private void BuGithub_Click(object sender, EventArgs e)
        {
            string args = "/c start \"\" \"https://github.com/alextrof94/SeleniumAutotestIDE\"";
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = args,
                UseShellExecute = true
            });
        }

        private void BuDonate_Click(object sender, EventArgs e)
        {
            string args = "/c start \"\" \"https://boosty.to/goodvrgames/donate\"";
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = args,
                UseShellExecute = true
            });
        }

        private void DaProjectParameters_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            RefreshProjectParameters();
        }

        private void RefreshProjectParameters()
        {
            if (!NeedUpdateTestFields) { return; }

            Project.Parameters.Clear();
            foreach (DataGridViewRow row in DaProjectParameters.Rows)
            {
                if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null)
                    continue;
                Project.Parameters.Add(new Parameter
                {
                    Name = row.Cells[0].Value?.ToString(),
                    Pattern = row.Cells[1].Value?.ToString(),
                    Value = row.Cells[2].Value?.ToString()
                });
            }
        }

        private void DaTestParameters_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            RefreshTestParameters();
        }

        private void RefreshTestParameters()
        {
            if (!NeedUpdateTestFields) { return; }
            if (Project.SelectedAutotest == null) { return; }

            Project.SelectedAutotest.Parameters.Clear();
            foreach (DataGridViewRow row in DaTestParameters.Rows)
            {
                if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null)
                    continue;
                Project.SelectedAutotest.Parameters.Add(new Parameter
                {
                    Name = row.Cells[0].Value?.ToString(),
                    Pattern = row.Cells[1].Value?.ToString(),
                    Value = row.Cells[2].Value?.ToString()
                });
            }
        }

        private bool DataGridViewSwapRows(DataGridView dgv, int rowIndex1, int rowIndex2)
        {
            if (rowIndex1 == -1 || rowIndex2 == -1 || rowIndex1 == rowIndex2 || rowIndex1 >= dgv.Rows.Count - 1 || rowIndex2 >= dgv.Rows.Count - 1) { return false; }

            DataGridViewRow row1 = dgv.Rows[rowIndex1];
            DataGridViewRow row2 = dgv.Rows[rowIndex2];

            for (int i = 0; i < row1.Cells.Count; i++)
            {
                object temp = row1.Cells[i].Value;
                row1.Cells[i].Value = row2.Cells[i].Value;
                row2.Cells[i].Value = temp;
            }
            return true;
        }

        private void BuProjectParametersDown_Click(object sender, EventArgs e)
        {
            if (DaProjectParameters.SelectedRows.Count != 1) { return; }
            if (DaProjectParameters.SelectedRows[0].IsNewRow) { return; }

            int rowIndex = DaProjectParameters.SelectedRows[0].Index;
            if (DataGridViewSwapRows(DaProjectParameters, rowIndex, rowIndex + 1))
            {
                DaProjectParameters.Rows[rowIndex].Selected = false;
                DaProjectParameters.Rows[rowIndex + 1].Selected = true;
                RefreshProjectParameters();
            }
        }

        private void BuProjectParametersUp_Click(object sender, EventArgs e)
        {
            if (DaProjectParameters.SelectedRows.Count != 1) { return; }
            if (DaProjectParameters.SelectedRows[0].IsNewRow) { return; }

            int rowIndex = DaProjectParameters.SelectedRows[0].Index;
            if (DataGridViewSwapRows(DaProjectParameters, rowIndex, rowIndex - 1))
            {
                DaProjectParameters.Rows[rowIndex].Selected = false;
                DaProjectParameters.Rows[rowIndex - 1].Selected = true;
                RefreshProjectParameters();
            }
        }

        private void BuTestParametersUp_Click(object sender, EventArgs e)
        {
            if (DaTestParameters.SelectedRows.Count != 1) { return; }
            if (DaTestParameters.SelectedRows[0].IsNewRow) { return; }

            int rowIndex = DaTestParameters.SelectedRows[0].Index;
            if (DataGridViewSwapRows(DaTestParameters, rowIndex, rowIndex - 1))
            {
                DaTestParameters.Rows[rowIndex].Selected = false;
                DaTestParameters.Rows[rowIndex - 1].Selected = true;
                RefreshTestParameters();
            }
        }

        private void BuTestParametersDown_Click(object sender, EventArgs e)
        {
            if (DaTestParameters.SelectedRows.Count != 1) { return; }
            if (DaTestParameters.SelectedRows[0].IsNewRow) { return; }

            int rowIndex = DaTestParameters.SelectedRows[0].Index;
            if (DataGridViewSwapRows(DaTestParameters, rowIndex, rowIndex + 1))
            {
                DaTestParameters.Rows[rowIndex].Selected = false;
                DaTestParameters.Rows[rowIndex + 1].Selected = true;
                RefreshTestParameters();
            }
        }
    }
}
