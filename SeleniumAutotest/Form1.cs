using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Input;
using OpenQA.Selenium.DevTools.V124.Debugger;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SeleniumAutotest
{
    public partial class Form1 : Form
    {
        // TODO:
        // StepByStep mode
        private const string Version = "v1.1";
        private const string AppName = "Selenium Autotest IDE by alextrof94 " + Version;

        private Project Project { get; set; }

        bool NeedUpdateTestFields = true;
        bool NeedToReselectAutotest = true;

        public Form1()
        {
            InitializeComponent(); 
            Project = new Project();
            Project.ParametersUpdated += Project_ParametersGenerated;
            Project.RunAutotestFinished += Project_RunAutotestFinished;
            Project.SelectedAutotestChanged += Project_SelectedAutotestChanged;
            ChProjectRegenerateParameters.Checked = Project.RegenerateParametersOnRun;
            LiTests.DataSource = Project.Autotests;
            toolTip1.SetToolTip(BuTestAdd, "Добавить автотест");
            toolTip1.SetToolTip(BuTestDelete, "Удалить автотест");
            toolTip1.SetToolTip(BuTestClone, "Дублировать автотест");
            toolTip1.SetToolTip(BuTestUp, "Переместить автотест выше");
            toolTip1.SetToolTip(BuTestDown, "Переместить автотест ниже");

            toolTip1.SetToolTip(BuTestRun, "Запустить автотест");
            toolTip1.SetToolTip(BuTestStop, "Остановить автотест");
            toolTip1.SetToolTip(BuTestRunStepMode, "Запустить в режиме пошагового выполнения");
            toolTip1.SetToolTip(BuTestStepModePrev, "На шаг назад в режиме пошагового выполнения");

            toolTip1.SetToolTip(BuStepAdd, "Добавить шаг [CTRL+A]");
            toolTip1.SetToolTip(BuStepDelete, "Удалить выделенный шаг [CTRL+Delete]");
            toolTip1.SetToolTip(BuStepClone, "Дублировать выделенный шаг [CTRL+D]");
            toolTip1.SetToolTip(BuStepUp, "Переместить шаг выше [CTRL+Up]");
            toolTip1.SetToolTip(BuStepDown, "Переместить шаг ниже [CTRL+Down]");
            toolTip1.SetToolTip(BuStepCopy, "Скопировать шаг [CTRL+C]");
            toolTip1.SetToolTip(BuStepPaste, "Вставить шаг [CTRL+V]");
            toolTip1.SetToolTip(BuStepClearFocus, "Сбросить фокус с шага [Escape]");

            toolTip1.SetToolTip(BuStepReloadTree, "Обновить дерево [CTRL+R]");
            toolTip1.SetToolTip(BuFontIncrease, "Увеличить размер текста в дереве");
            toolTip1.SetToolTip(BuFontDecrease, "Уменьшить размер текста в дереве");
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
            str += "SUMM = ^(10+int.Parse(%Param0%)).ToString()^ => ^(10+int.Parse(11)).ToString()^ => \"21\"\r\n\r\n";

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
            switch (type)
            {
                case StepTypes.Group:
                case StepTypes.RefreshPage:
                case StepTypes.Click:
                case StepTypes.AltClick:
                case StepTypes.JsClick:
                case StepTypes.DoubleClick:
                    break;
                case StepTypes.FindElement:
                    CoStepSelectorType.Visible = true;
                    TeStepSelector.Visible = true;
                    NuStepWait.Visible = true;
                    break;
                case StepTypes.CheckElement:
                    CoStepSelectorType.Visible = true;
                    TeStepSelector.Visible = true;
                    NuStepWait.Visible = true;
                    ChStepIgnoreError.Visible = true;
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

        private void TeStepName_TextChanged(object sender, EventArgs e)
        {
            UpdateStepField(nameof(TestStep.Name), TeStepName.Text);
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
            UpdateStepField(nameof(TestStep.Type), StepType.GetTypeByNameAndGroup(CoStepType.SelectedItem.ToString(), CoStepTypeGroup.SelectedItem.ToString()));
            SetStepFieldsVisible(selectedStep.Type);
            //ReloadTree();
        }
        private void CoStepSelectorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStepField(nameof(TestStep.SelectorType), CoStepSelectorType.SelectedIndex);
        }

        private void TeStepSelector_TextChanged(object sender, EventArgs e)
        {
            UpdateStepField(nameof(TestStep.Selector), TeStepSelector.Text);
        }

        private void NuStepWait_ValueChanged(object sender, EventArgs e)
        {
            UpdateStepField(nameof(TestStep.SecondsToWait), (float)NuStepWait.Value);
        }

        private void TeStepValue_TextChanged(object sender, EventArgs e)
        {
            UpdateStepField(nameof(TestStep.Value), TeStepValue.Text);
        }

        private void TeStepParameter_TextChanged(object sender, EventArgs e)
        {
            UpdateStepField(nameof(TestStep.Parameter), TeStepParameter.Text);
        }

        private void ChStepIgnoreError_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStepField(nameof(TestStep.IgnoreError), ChStepIgnoreError.Checked);
        }

        private void ChStepIsEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStepField(nameof(TestStep.Enabled), ChStepIsEnabled.Checked);
        }

        #endregion StepFields

        // TODO: Rework: move to project
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
            if (!NeedUpdateTestFields) { return; }
            if (Project.SelectedAutotest == null) { return; }

            Project.SelectedAutotest.Parameters.Clear();
            foreach (DataGridViewRow row in DaTestParameters.Rows)
            {
                if (row.Cells[0].Value == null && row.Cells[1].Value == null && row.Cells[2].Value == null)
                    continue;
                Project.SelectedAutotest.Parameters.Add(new Parameter { 
                    Name = row.Cells[0].Value?.ToString(), 
                    Pattern = row.Cells[1].Value?.ToString(), 
                    Value = row.Cells[2].Value?.ToString() 
                });
            }
        }

        private void DaProjectParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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
                if (TrSteps.SelectedNode != null)
                {
                    var parentStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

                    var appliableTypes = StepType.StepTypesGroups.First(x => x.Parents.Contains(parentStep.Type)).Types;
                    if (appliableTypes.Count == 0)
                    {
                        return;
                    }
                    var type = appliableTypes.First();
                    parentStep.Substeps.Add(new TestStep()
                    {
                        Type = type,
                        Name = StepType.Descriptions[type]
                    });
                }
                else
                {
                    var appliableTypes = StepType.StepTypesGroups.First(x => x.Parents.Count == 0 || x.Parents.Contains(StepTypes.Group)).Types;
                    if (appliableTypes.Count == 0)
                    {
                        return;
                    }
                    var type = appliableTypes.First();
                    Project.SelectedAutotest.Root.Substeps.Add(new TestStep()
                    {
                        Type = type,
                        Name = StepType.Descriptions[type]
                    });
                }
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
            try
            {
                NuStepWait.Value = (decimal)selectedStep.SecondsToWait;
            }
            catch
            {
                NuStepWait.Value = (decimal)0.001f;
            }
            RiLog.Text = selectedStep.Error;


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
            CoStepType.Items.Clear();
            foreach (var type in ((StepTypesGroup)CoStepTypeGroup.SelectedItem).Types)
            {
                CoStepType.Items.Add(StepType.Descriptions[type]);
            }
            CoStepType.SelectedIndex = 0;
            TeStepName.Text = CoStepTypeGroup.Text;
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
    }
}
