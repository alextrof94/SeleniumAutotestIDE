﻿using Newtonsoft.Json;
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

namespace SeleniumAutotest
{
    public partial class Form1 : Form
    {
        private const string AppName = "Selenium Autotest IDE by alextrof94";

        private Project Project { get; set; }

        bool NeedUpdateTestFields = true;

        public Form1()
        {
            InitializeComponent();
            Project = new Project();
            Project.ParametersUpdated += Project_ParametersGenerated;
            Project.RunAutotestFinished += Project_RunAutotestFinished;
            ChProjectRegenerateParameters.Checked = Project.RegenerateParametersOnRun;
            LiTests.DataSource = Project.Autotests;
            toolTip1.SetToolTip(BuTestAdd, "Добавить автотест");
            toolTip1.SetToolTip(BuTestDelete, "Удалить автотест");
            toolTip1.SetToolTip(BuTestClone, "Дублировать автотест");
            toolTip1.SetToolTip(BuTestUp, "Переместить автотест выше");
            toolTip1.SetToolTip(BuTestDown, "Переместить автотест ниже");

            toolTip1.SetToolTip(BuStepAdd, "Добавить шаг [CTRL+A]");
            toolTip1.SetToolTip(BuStepDelete, "Удалить выделенный шаг [CTRL+Delete]");
            toolTip1.SetToolTip(BuStepClone, "Дублировать выделенный шаг [CTRL+D]");
            toolTip1.SetToolTip(BuStepUp, "Переместить шаг выше [CTRL+Up]");
            toolTip1.SetToolTip(BuStepDown, "Переместить шаг ниже [CTRL+Down]");
            toolTip1.SetToolTip(BuStepCopy, "Скопировать шаг [CTRL+C]");
            toolTip1.SetToolTip(BuStepPaste, "Вставить шаг [CTRL+V]");
            toolTip1.SetToolTip(BuStepClearFocus, "Сбросить фокус с шага [Escape]");
            toolTip1.SetToolTip(BuStepReloadTree, "Обновить дерево [CTRL+R]");
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
                    string str = File.ReadAllText(path);
                    Project = JsonConvert.DeserializeObject<Project>(str);
                    foreach (Autotest autotest in Project.Autotests)
                    {
                        autotest.ResetGuid();
                    }
                    Project.ResetAllParentsForAutotests();
                    Project.ParametersUpdated += Project_ParametersGenerated;
                    Project.RunAutotestFinished += Project_RunAutotestFinished;
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
                string str = File.ReadAllText(ofd.FileName);
                Project = JsonConvert.DeserializeObject<Project>(str);
                foreach (Autotest autotest in Project.Autotests)
                {
                    autotest.ResetGuid();
                }
                Project.ResetAllParentsForAutotests();
                Project.ParametersUpdated += Project_ParametersGenerated;
                Project.RunAutotestFinished += Project_RunAutotestFinished;
                ChProjectRegenerateParameters.Checked = Project.RegenerateParametersOnRun;
                Text = $"{AppName} - {Project.Name}";
                LiTests.DataSource = Project.Autotests;
                File.WriteAllText("./LastFilePath.txt", ofd.FileName);
                Project_ParametersGenerated();
            }
        }

        private void BuFileSave_Click(object sender, EventArgs e)
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

        private void BuTestRun_Click(object sender, EventArgs e)
        {
            if (Project.RunAutotest())
            {
                BuTestRun.Enabled = false;
                BuTestStop.Enabled = true;
            }
        }

        private void BuTestStop_Click(object sender, EventArgs e)
        {
            BuTestStop.Enabled = false;
            Project.StopAutotest();
            BuTestRun.Enabled = true;
            ReloadTree();
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
            if (TrSteps.Font.Size < 6) { return; }
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
            str += "Param3 = Test3%Param2%/randomD2/Test3 => \"Test3Test2A127BTest274Test3\"\r\n\r\n";

            str += "ParamNull = TestN%PARAM1%/randomD2//null/TestN => null\r\n\r\n";

            str += "TODAY = ^DateTime.Now.ToShortDateString()^ => текущая дата в формате dd.mm.yyyy (для RU-локали)\r\n";
            str += "TODAY = ^(10+int.Parse(%Param0%)).ToString()^ => \"21\"\r\n\r\n";

            str += "Не используйте одинаковые именя для параметров проекта и параметров теста\r\n\r\n";

            str += "Для параметров, которые будут заполнены во время выполнения автотестов оставьте пустой шаблон\r\n\r\n";
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
            TeStepSelector.Visible = false;
            TeStepValue.Visible = false;
            NuStepWait.Visible = false;
            ChStepIgnoreError.Visible = false;
            TeStepParameter.Visible = false;
            switch (type)
            {
                case StepTypes.Group:
                case StepTypes.Click:
                case StepTypes.DoubleClick:
                    break;
                case StepTypes.WaitElement:
                    TeStepSelector.Visible = true;
                    NuStepWait.Visible = true;
                    break;
                case StepTypes.CheckElement:
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
                case StepTypes.EnterValue:
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
                case StepTypes.CheckClass:
                case StepTypes.CheckClassNotExists:
                    TeStepValue.Visible = true;
                    ChStepIgnoreError.Visible = true;
                    break;
                case StepTypes.ReadAttributeToParameter:
                    TeStepSelector.Visible = true;
                    TeStepParameter.Visible = true;
                    break;
                case StepTypes.ReadAddressToParameter:
                case StepTypes.ReadTextToParameter:
                    TeStepParameter.Visible = true;
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
                CoStepType.SelectedItem = StepType.descriptions[selectedStep.Type];
                NeedUpdateTestFields = true;
                return;
            }
            selectedStep.Substeps.Clear();
            UpdateStepField(nameof(TestStep.Type), StepType.GetTypeByName(CoStepType.SelectedItem.ToString()));
            SetStepFieldsVisible(selectedStep.Type);
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

            Project.SelectAutotest((Autotest)LiTests.SelectedItem);

            TeTestName.Text = Project.SelectedAutotest.Name;
            LaTime.Text = "Время выполнения: " + Project.SelectedAutotest.CompleteTime;
            ChTestRegenerateParameters.Checked = Project.SelectedAutotest.RegenerateParametersOnRun;
            ChTestRunAfterPrevious.Checked = Project.SelectedAutotest.RunAfterPrevious;

            foreach (var test in Project.Autotests)
            {
                test.StateUpdated -= AutotestUpdated;
                test.ParametersUpdated -= SelectedAutotest_ParametersGenerated;
            }
            Project.SelectedAutotest.StateUpdated += AutotestUpdated;
            Project.SelectedAutotest.ParametersUpdated += SelectedAutotest_ParametersGenerated;

            Project.SelectedAutotest.ResetAllParentsForSteps();
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

        private void Project_RunAutotestFinished()
        {
            this.Invoke(new Action(() =>
            {
                BuTestRun.Enabled = true;
                BuTestStop.Enabled = false;
                LaTime.Text = "Время выполнения: " + Project.SelectedAutotest.CompleteTime;
                ReloadTree();
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
                switch (step.Type)
                {
                    case StepTypes.CheckElement:
                    case StepTypes.CheckText:
                    case StepTypes.CheckAttribute:
                    case StepTypes.CheckClass:
                    case StepTypes.CheckClassNotExists:
                        node.ImageIndex = 0;
                        break;

                    case StepTypes.DoubleClick:
                    case StepTypes.Click:
                        node.ImageIndex = 1;
                        break;

                    case StepTypes.EnterValue:
                        node.ImageIndex = 2;
                        break;

                    case StepTypes.WaitElement:
                        node.ImageIndex = 3;
                        break;

                    case StepTypes.Group: 
                        node.ImageIndex = 4; 
                        break;

                    case StepTypes.Open: 
                        node.ImageIndex = 5; 
                        break;

                    case StepTypes.ReadAddressToParameter:
                    case StepTypes.ReadTextToParameter:
                    case StepTypes.ReadAttributeToParameter:
                        node.ImageIndex = 7;
                        break;

                    case StepTypes.WaitTime:
                        node.ImageIndex = 8; 
                        break;

                }
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
                }
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

            if (TrSteps.SelectedNode != null)
            {
                var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

                var appliableTypes = StepType.GetElementsForStepType(selectedStep.Type);
                if (appliableTypes.Count == 0)
                {
                    return;
                }
                var type = appliableTypes.First();
                selectedStep.Substeps.Add(new TestStep()
                {
                    Type = type,
                    Name = StepType.descriptions[type]
                });
            }
            else
            {
                var appliableTypes = StepType.GetElementsForStepType(StepTypes.Group);
                if (appliableTypes.Count == 0)
                {
                    return;
                }
                var type = appliableTypes.First();
                Project.SelectedAutotest.Root.Substeps.Add(new TestStep()
                {
                    Type = type,
                    Name = StepType.descriptions[type]
                });
            }
            Project.SelectedAutotest.ResetAllParentsForSteps();
            ReloadTree();
        }

        private void StepDelete()
        {
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            selectedStep.Parent.Substeps.Remove(selectedStep);
            Project.SelectedAutotest.ResetAllParentsForSteps();
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
            Project.SelectedAutotest.ResetAllParentsForSteps();
            ReloadTree();
        }

        private void StepMoveUp()
        {
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            selectedStep.MoveUp();

            Project.SelectedAutotest.ResetAllParentsForSteps();
            ReloadTree();
        }

        private void StepMoveDown()
        {
            if (Project.SelectedAutotest == null) { return; }
            if (TrSteps.SelectedNode == null) { return; }

            var selectedStep = Project.SelectedAutotest.FindStepById((Guid)TrSteps.SelectedNode.Tag);

            selectedStep.MoveDown();

            Project.SelectedAutotest.ResetAllParentsForSteps();
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

            Project.SelectedAutotest.ResetAllParentsForSteps();
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

            TeStepName.Text = selectedStep.Name;
            TeStepSelector.Text = selectedStep.Selector;
            TeStepValue.Text = selectedStep.Value;
            TeStepParameter.Text = selectedStep.Parameter;
            ChStepIgnoreError.Checked = selectedStep.IgnoreError;
            try
            {
                NuStepWait.Value = (decimal)selectedStep.SecondsToWait;
            }
            catch
            {
                NuStepWait.Value = (decimal)0.001f;
            }
            RiLog.Text = selectedStep.Error;

            CoStepType.Items.Clear();
            foreach (var type in StepType.GetElementsForStepType(selectedStep.Parent.Type))
            {
                CoStepType.Items.Add(StepType.descriptions[type]);
            }
            CoStepType.SelectedItem = StepType.descriptions[selectedStep.Type];

            SetStepFieldsVisible(selectedStep.Type);
            NeedUpdateTestFields = true;
        }

        private void TrSteps_KeyDown(object sender, KeyEventArgs e)
        {
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
    }
}