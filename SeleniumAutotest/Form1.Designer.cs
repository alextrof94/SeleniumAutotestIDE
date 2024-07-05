namespace SeleniumAutotest
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BuFileOpenLast = new System.Windows.Forms.ToolStripMenuItem();
            this.BuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.BuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.BuFileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.инфоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BuTestParametersHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.BuCantDownloadDriverHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.LiTests = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChSelectFoundElements = new System.Windows.Forms.CheckBox();
            this.ChSlowMode = new System.Windows.Forms.CheckBox();
            this.LaRunTime = new System.Windows.Forms.Label();
            this.BuTestDown = new System.Windows.Forms.Button();
            this.ImButtons = new System.Windows.Forms.ImageList(this.components);
            this.BuTestUp = new System.Windows.Forms.Button();
            this.LaTestTime = new System.Windows.Forms.Label();
            this.BuTestStop = new System.Windows.Forms.Button();
            this.BuTestClone = new System.Windows.Forms.Button();
            this.BuTestDelete = new System.Windows.Forms.Button();
            this.BuTestAdd = new System.Windows.Forms.Button();
            this.BuTestRun = new System.Windows.Forms.Button();
            this.GrTestSteps = new System.Windows.Forms.GroupBox();
            this.PaMiddleDownMiddle = new System.Windows.Forms.Panel();
            this.BuStepDelete = new System.Windows.Forms.Button();
            this.BuStepAdd = new System.Windows.Forms.Button();
            this.BuStepClone = new System.Windows.Forms.Button();
            this.TrSteps = new System.Windows.Forms.TreeView();
            this.ImTree = new System.Windows.Forms.ImageList(this.components);
            this.BuFontDecrease = new System.Windows.Forms.Button();
            this.BuFontIncrease = new System.Windows.Forms.Button();
            this.BuStepDown = new System.Windows.Forms.Button();
            this.BuStepReloadTree = new System.Windows.Forms.Button();
            this.BuStepUp = new System.Windows.Forms.Button();
            this.BuStepPaste = new System.Windows.Forms.Button();
            this.BuStepClearFocus = new System.Windows.Forms.Button();
            this.BuStepCopy = new System.Windows.Forms.Button();
            this.PaMiddleDownDown = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.RiLog = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CoStepTypeGroup = new System.Windows.Forms.ComboBox();
            this.ChStepIsEnabled = new System.Windows.Forms.CheckBox();
            this.TeStepParameter = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ChStepIgnoreError = new System.Windows.Forms.CheckBox();
            this.TeStepValue = new System.Windows.Forms.TextBox();
            this.LaStepValue = new System.Windows.Forms.Label();
            this.NuStepWait = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.TeStepSelector = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CoStepType = new System.Windows.Forms.ComboBox();
            this.TeStepName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TeTestName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GrTestParameters = new System.Windows.Forms.GroupBox();
            this.ChTestRegenerateParameters = new System.Windows.Forms.CheckBox();
            this.BuTestGenerateParameters = new System.Windows.Forms.Button();
            this.DaTestParameters = new System.Windows.Forms.DataGridView();
            this.CoTestParameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CoTestParametersPattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CoTestParameterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ChProjectRegenerateParameters = new System.Windows.Forms.CheckBox();
            this.BuProjectGenerateParameters = new System.Windows.Forms.Button();
            this.DaProjectParameters = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrTestSettings = new System.Windows.Forms.GroupBox();
            this.ChTestRunAfterPrevious = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.PaRight = new System.Windows.Forms.Panel();
            this.PaMiddle = new System.Windows.Forms.Panel();
            this.CoStepSelectorType = new System.Windows.Forms.ComboBox();
            this.SpRight = new System.Windows.Forms.Splitter();
            this.PaLeft = new System.Windows.Forms.Panel();
            this.SpLeft = new System.Windows.Forms.Splitter();
            this.PaRightDown = new System.Windows.Forms.Panel();
            this.SpRightDown = new System.Windows.Forms.Splitter();
            this.PaRightUp = new System.Windows.Forms.Panel();
            this.PaMiddleUp = new System.Windows.Forms.Panel();
            this.PaMIddleDown = new System.Windows.Forms.Panel();
            this.PaMiddleDownDownRight = new System.Windows.Forms.Panel();
            this.SpMiddleDownDownRight = new System.Windows.Forms.Splitter();
            this.PaMiddleDownDownLeft = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.GrTestSteps.SuspendLayout();
            this.PaMiddleDownMiddle.SuspendLayout();
            this.PaMiddleDownDown.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NuStepWait)).BeginInit();
            this.GrTestParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DaTestParameters)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DaProjectParameters)).BeginInit();
            this.GrTestSettings.SuspendLayout();
            this.PaRight.SuspendLayout();
            this.PaMiddle.SuspendLayout();
            this.PaLeft.SuspendLayout();
            this.PaRightDown.SuspendLayout();
            this.PaRightUp.SuspendLayout();
            this.PaMiddleUp.SuspendLayout();
            this.PaMIddleDown.SuspendLayout();
            this.PaMiddleDownDownRight.SuspendLayout();
            this.PaMiddleDownDownLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.инфоToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1404, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BuFileOpenLast,
            this.BuFileOpen,
            this.BuFileSave,
            this.BuFileImport});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // BuFileOpenLast
            // 
            this.BuFileOpenLast.Name = "BuFileOpenLast";
            this.BuFileOpenLast.Size = new System.Drawing.Size(184, 22);
            this.BuFileOpenLast.Text = "Открыть последний";
            this.BuFileOpenLast.Click += new System.EventHandler(this.BuFileOpenLast_Click);
            // 
            // BuFileOpen
            // 
            this.BuFileOpen.Name = "BuFileOpen";
            this.BuFileOpen.Size = new System.Drawing.Size(184, 22);
            this.BuFileOpen.Text = "Открыть";
            this.BuFileOpen.Click += new System.EventHandler(this.BuFileOpen_Click);
            // 
            // BuFileSave
            // 
            this.BuFileSave.Name = "BuFileSave";
            this.BuFileSave.Size = new System.Drawing.Size(184, 22);
            this.BuFileSave.Text = "Сохранить";
            this.BuFileSave.Click += new System.EventHandler(this.BuFileSave_Click);
            // 
            // BuFileImport
            // 
            this.BuFileImport.Name = "BuFileImport";
            this.BuFileImport.Size = new System.Drawing.Size(184, 22);
            this.BuFileImport.Text = "Импорт из строки";
            this.BuFileImport.Visible = false;
            // 
            // инфоToolStripMenuItem
            // 
            this.инфоToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BuTestParametersHelp,
            this.BuCantDownloadDriverHelp});
            this.инфоToolStripMenuItem.Name = "инфоToolStripMenuItem";
            this.инфоToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.инфоToolStripMenuItem.Text = "Инфо";
            // 
            // BuTestParametersHelp
            // 
            this.BuTestParametersHelp.Name = "BuTestParametersHelp";
            this.BuTestParametersHelp.Size = new System.Drawing.Size(208, 22);
            this.BuTestParametersHelp.Text = "Справка по параметрам";
            this.BuTestParametersHelp.Click += new System.EventHandler(this.BuTestParametersHelp_Click);
            // 
            // BuCantDownloadDriverHelp
            // 
            this.BuCantDownloadDriverHelp.Name = "BuCantDownloadDriverHelp";
            this.BuCantDownloadDriverHelp.Size = new System.Drawing.Size(208, 22);
            this.BuCantDownloadDriverHelp.Text = "Нет доступа к интернету";
            this.BuCantDownloadDriverHelp.Click += new System.EventHandler(this.BuCantDownloadDriverHelp_Click);
            // 
            // LiTests
            // 
            this.LiTests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LiTests.FormattingEnabled = true;
            this.LiTests.Location = new System.Drawing.Point(6, 58);
            this.LiTests.Name = "LiTests";
            this.LiTests.Size = new System.Drawing.Size(226, 407);
            this.LiTests.TabIndex = 1;
            this.LiTests.SelectedIndexChanged += new System.EventHandler(this.LiTests_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ChSelectFoundElements);
            this.groupBox1.Controls.Add(this.ChSlowMode);
            this.groupBox1.Controls.Add(this.LaRunTime);
            this.groupBox1.Controls.Add(this.BuTestDown);
            this.groupBox1.Controls.Add(this.BuTestUp);
            this.groupBox1.Controls.Add(this.LaTestTime);
            this.groupBox1.Controls.Add(this.BuTestStop);
            this.groupBox1.Controls.Add(this.BuTestClone);
            this.groupBox1.Controls.Add(this.BuTestDelete);
            this.groupBox1.Controls.Add(this.BuTestAdd);
            this.groupBox1.Controls.Add(this.BuTestRun);
            this.groupBox1.Controls.Add(this.LiTests);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 587);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Автотесты";
            // 
            // ChSelectFoundElements
            // 
            this.ChSelectFoundElements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChSelectFoundElements.AutoSize = true;
            this.ChSelectFoundElements.Location = new System.Drawing.Point(6, 480);
            this.ChSelectFoundElements.Name = "ChSelectFoundElements";
            this.ChSelectFoundElements.Size = new System.Drawing.Size(189, 17);
            this.ChSelectFoundElements.TabIndex = 13;
            this.ChSelectFoundElements.Text = "Выделять найденные элементы";
            this.ChSelectFoundElements.UseVisualStyleBackColor = true;
            // 
            // ChSlowMode
            // 
            this.ChSlowMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChSlowMode.AutoSize = true;
            this.ChSlowMode.Location = new System.Drawing.Point(6, 503);
            this.ChSlowMode.Name = "ChSlowMode";
            this.ChSlowMode.Size = new System.Drawing.Size(122, 17);
            this.ChSlowMode.TabIndex = 12;
            this.ChSlowMode.Text = "Медленный режим";
            this.ChSlowMode.UseVisualStyleBackColor = true;
            // 
            // LaRunTime
            // 
            this.LaRunTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LaRunTime.AutoSize = true;
            this.LaRunTime.Location = new System.Drawing.Point(7, 538);
            this.LaRunTime.Name = "LaRunTime";
            this.LaRunTime.Size = new System.Drawing.Size(141, 13);
            this.LaRunTime.TabIndex = 11;
            this.LaRunTime.Text = "Время выполнения общее";
            // 
            // BuTestDown
            // 
            this.BuTestDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuTestDown.ImageIndex = 7;
            this.BuTestDown.ImageList = this.ImButtons;
            this.BuTestDown.Location = new System.Drawing.Point(141, 19);
            this.BuTestDown.Name = "BuTestDown";
            this.BuTestDown.Size = new System.Drawing.Size(33, 33);
            this.BuTestDown.TabIndex = 10;
            this.BuTestDown.UseVisualStyleBackColor = true;
            this.BuTestDown.Click += new System.EventHandler(this.BuTestDown_Click);
            // 
            // ImButtons
            // 
            this.ImButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImButtons.ImageStream")));
            this.ImButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.ImButtons.Images.SetKeyName(0, "icons8-add-32.png");
            this.ImButtons.Images.SetKeyName(1, "icons8-clear-32.png");
            this.ImButtons.Images.SetKeyName(2, "icons8-copy-32.png");
            this.ImButtons.Images.SetKeyName(3, "icons8-duplicate-32.png");
            this.ImButtons.Images.SetKeyName(4, "icons8-edit-32.png");
            this.ImButtons.Images.SetKeyName(5, "icons8-paste-32.png");
            this.ImButtons.Images.SetKeyName(6, "icons8-reload-32.png");
            this.ImButtons.Images.SetKeyName(7, "icons8-sort-down-32.png");
            this.ImButtons.Images.SetKeyName(8, "icons8-sort-up-32.png");
            this.ImButtons.Images.SetKeyName(9, "icons8-delete-32.png");
            this.ImButtons.Images.SetKeyName(10, "icons8-decrease-font-32.png");
            this.ImButtons.Images.SetKeyName(11, "icons8-increase-font-32.png");
            // 
            // BuTestUp
            // 
            this.BuTestUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuTestUp.ImageIndex = 8;
            this.BuTestUp.ImageList = this.ImButtons;
            this.BuTestUp.Location = new System.Drawing.Point(102, 19);
            this.BuTestUp.Name = "BuTestUp";
            this.BuTestUp.Size = new System.Drawing.Size(33, 33);
            this.BuTestUp.TabIndex = 9;
            this.BuTestUp.UseVisualStyleBackColor = true;
            this.BuTestUp.Click += new System.EventHandler(this.BuTestUp_Click);
            // 
            // LaTestTime
            // 
            this.LaTestTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LaTestTime.AutoSize = true;
            this.LaTestTime.Location = new System.Drawing.Point(7, 523);
            this.LaTestTime.Name = "LaTestTime";
            this.LaTestTime.Size = new System.Drawing.Size(136, 13);
            this.LaTestTime.TabIndex = 7;
            this.LaTestTime.Text = "Время выполнения теста";
            // 
            // BuTestStop
            // 
            this.BuTestStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BuTestStop.Enabled = false;
            this.BuTestStop.Location = new System.Drawing.Point(91, 558);
            this.BuTestStop.Name = "BuTestStop";
            this.BuTestStop.Size = new System.Drawing.Size(75, 23);
            this.BuTestStop.TabIndex = 6;
            this.BuTestStop.Text = "Стоп";
            this.BuTestStop.UseVisualStyleBackColor = true;
            this.BuTestStop.Click += new System.EventHandler(this.BuTestStop_Click);
            // 
            // BuTestClone
            // 
            this.BuTestClone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuTestClone.ImageIndex = 3;
            this.BuTestClone.ImageList = this.ImButtons;
            this.BuTestClone.Location = new System.Drawing.Point(45, 19);
            this.BuTestClone.Name = "BuTestClone";
            this.BuTestClone.Size = new System.Drawing.Size(33, 33);
            this.BuTestClone.TabIndex = 5;
            this.BuTestClone.UseVisualStyleBackColor = true;
            this.BuTestClone.Click += new System.EventHandler(this.BuTestClone_Click);
            // 
            // BuTestDelete
            // 
            this.BuTestDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuTestDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuTestDelete.ImageIndex = 9;
            this.BuTestDelete.ImageList = this.ImButtons;
            this.BuTestDelete.Location = new System.Drawing.Point(199, 19);
            this.BuTestDelete.Name = "BuTestDelete";
            this.BuTestDelete.Size = new System.Drawing.Size(33, 33);
            this.BuTestDelete.TabIndex = 4;
            this.BuTestDelete.UseVisualStyleBackColor = true;
            this.BuTestDelete.Click += new System.EventHandler(this.BuTestDelete_Click);
            // 
            // BuTestAdd
            // 
            this.BuTestAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuTestAdd.ImageIndex = 0;
            this.BuTestAdd.ImageList = this.ImButtons;
            this.BuTestAdd.Location = new System.Drawing.Point(6, 19);
            this.BuTestAdd.Name = "BuTestAdd";
            this.BuTestAdd.Size = new System.Drawing.Size(33, 33);
            this.BuTestAdd.TabIndex = 3;
            this.BuTestAdd.UseVisualStyleBackColor = true;
            this.BuTestAdd.Click += new System.EventHandler(this.BuTestAdd_Click);
            // 
            // BuTestRun
            // 
            this.BuTestRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BuTestRun.Location = new System.Drawing.Point(10, 558);
            this.BuTestRun.Name = "BuTestRun";
            this.BuTestRun.Size = new System.Drawing.Size(75, 23);
            this.BuTestRun.TabIndex = 2;
            this.BuTestRun.Text = "Запуск";
            this.BuTestRun.UseVisualStyleBackColor = true;
            this.BuTestRun.Click += new System.EventHandler(this.BuTestRun_Click);
            // 
            // GrTestSteps
            // 
            this.GrTestSteps.Controls.Add(this.PaMiddleDownMiddle);
            this.GrTestSteps.Controls.Add(this.PaMiddleDownDown);
            this.GrTestSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrTestSteps.Enabled = false;
            this.GrTestSteps.Location = new System.Drawing.Point(3, 0);
            this.GrTestSteps.Name = "GrTestSteps";
            this.GrTestSteps.Size = new System.Drawing.Size(788, 541);
            this.GrTestSteps.TabIndex = 3;
            this.GrTestSteps.TabStop = false;
            this.GrTestSteps.Text = "Шаги";
            // 
            // PaMiddleDownMiddle
            // 
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepDelete);
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepAdd);
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepClone);
            this.PaMiddleDownMiddle.Controls.Add(this.TrSteps);
            this.PaMiddleDownMiddle.Controls.Add(this.BuFontDecrease);
            this.PaMiddleDownMiddle.Controls.Add(this.BuFontIncrease);
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepDown);
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepReloadTree);
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepUp);
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepPaste);
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepClearFocus);
            this.PaMiddleDownMiddle.Controls.Add(this.BuStepCopy);
            this.PaMiddleDownMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaMiddleDownMiddle.Location = new System.Drawing.Point(3, 16);
            this.PaMiddleDownMiddle.Name = "PaMiddleDownMiddle";
            this.PaMiddleDownMiddle.Size = new System.Drawing.Size(782, 323);
            this.PaMiddleDownMiddle.TabIndex = 17;
            // 
            // BuStepDelete
            // 
            this.BuStepDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepDelete.ImageIndex = 9;
            this.BuStepDelete.ImageList = this.ImButtons;
            this.BuStepDelete.Location = new System.Drawing.Point(746, 3);
            this.BuStepDelete.Name = "BuStepDelete";
            this.BuStepDelete.Size = new System.Drawing.Size(33, 33);
            this.BuStepDelete.TabIndex = 5;
            this.BuStepDelete.UseVisualStyleBackColor = true;
            this.BuStepDelete.Click += new System.EventHandler(this.BuStepDelete_Click);
            // 
            // BuStepAdd
            // 
            this.BuStepAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepAdd.ImageIndex = 0;
            this.BuStepAdd.ImageList = this.ImButtons;
            this.BuStepAdd.Location = new System.Drawing.Point(707, 3);
            this.BuStepAdd.Name = "BuStepAdd";
            this.BuStepAdd.Size = new System.Drawing.Size(33, 33);
            this.BuStepAdd.TabIndex = 4;
            this.BuStepAdd.UseVisualStyleBackColor = true;
            this.BuStepAdd.Click += new System.EventHandler(this.BuStepAdd_Click);
            // 
            // BuStepClone
            // 
            this.BuStepClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepClone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepClone.ImageIndex = 3;
            this.BuStepClone.ImageList = this.ImButtons;
            this.BuStepClone.Location = new System.Drawing.Point(746, 42);
            this.BuStepClone.Name = "BuStepClone";
            this.BuStepClone.Size = new System.Drawing.Size(33, 33);
            this.BuStepClone.TabIndex = 6;
            this.BuStepClone.UseVisualStyleBackColor = true;
            this.BuStepClone.Click += new System.EventHandler(this.BuStepClone_Click);
            // 
            // TrSteps
            // 
            this.TrSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrSteps.HideSelection = false;
            this.TrSteps.ImageIndex = 0;
            this.TrSteps.ImageList = this.ImTree;
            this.TrSteps.Location = new System.Drawing.Point(3, 3);
            this.TrSteps.Name = "TrSteps";
            this.TrSteps.SelectedImageIndex = 0;
            this.TrSteps.Size = new System.Drawing.Size(698, 316);
            this.TrSteps.TabIndex = 2;
            this.TrSteps.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.TrSteps_ChangeExpanded);
            this.TrSteps.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TrSteps_ChangeExpanded);
            this.TrSteps.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TrSteps_NodeMouseClick);
            this.TrSteps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TrSteps_KeyDown);
            this.TrSteps.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TrSteps_KeyUp);
            // 
            // ImTree
            // 
            this.ImTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImTree.ImageStream")));
            this.ImTree.TransparentColor = System.Drawing.Color.Transparent;
            this.ImTree.Images.SetKeyName(0, "icons8-right-16.png");
            this.ImTree.Images.SetKeyName(1, "icons8-folder-16.png");
            this.ImTree.Images.SetKeyName(2, "icons8-find-16.png");
            this.ImTree.Images.SetKeyName(3, "icons8-click-16.png");
            this.ImTree.Images.SetKeyName(4, "icons8-edit-16.png");
            this.ImTree.Images.SetKeyName(5, "icons8-check-16.png");
            this.ImTree.Images.SetKeyName(6, "icons8-lightning-bolt-16.png");
            this.ImTree.Images.SetKeyName(7, "icons8-save-16.png");
            this.ImTree.Images.SetKeyName(8, "icons8-stopwatch-16.png");
            this.ImTree.Images.SetKeyName(9, "icons8-open-16.png");
            this.ImTree.Images.SetKeyName(10, "icons8-user-16.png");
            // 
            // BuFontDecrease
            // 
            this.BuFontDecrease.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BuFontDecrease.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuFontDecrease.ImageIndex = 10;
            this.BuFontDecrease.ImageList = this.ImButtons;
            this.BuFontDecrease.Location = new System.Drawing.Point(746, 286);
            this.BuFontDecrease.Name = "BuFontDecrease";
            this.BuFontDecrease.Size = new System.Drawing.Size(33, 33);
            this.BuFontDecrease.TabIndex = 14;
            this.BuFontDecrease.UseVisualStyleBackColor = true;
            this.BuFontDecrease.Click += new System.EventHandler(this.BuFontDecrease_Click);
            // 
            // BuFontIncrease
            // 
            this.BuFontIncrease.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BuFontIncrease.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuFontIncrease.ImageIndex = 11;
            this.BuFontIncrease.ImageList = this.ImButtons;
            this.BuFontIncrease.Location = new System.Drawing.Point(707, 286);
            this.BuFontIncrease.Name = "BuFontIncrease";
            this.BuFontIncrease.Size = new System.Drawing.Size(33, 33);
            this.BuFontIncrease.TabIndex = 13;
            this.BuFontIncrease.UseVisualStyleBackColor = true;
            this.BuFontIncrease.Click += new System.EventHandler(this.BuFontIncrease_Click);
            // 
            // BuStepDown
            // 
            this.BuStepDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepDown.ImageIndex = 7;
            this.BuStepDown.ImageList = this.ImButtons;
            this.BuStepDown.Location = new System.Drawing.Point(707, 81);
            this.BuStepDown.Name = "BuStepDown";
            this.BuStepDown.Size = new System.Drawing.Size(33, 33);
            this.BuStepDown.TabIndex = 7;
            this.BuStepDown.UseVisualStyleBackColor = true;
            this.BuStepDown.Click += new System.EventHandler(this.BuStepDown_Click);
            // 
            // BuStepReloadTree
            // 
            this.BuStepReloadTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepReloadTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepReloadTree.ImageIndex = 6;
            this.BuStepReloadTree.ImageList = this.ImButtons;
            this.BuStepReloadTree.Location = new System.Drawing.Point(707, 247);
            this.BuStepReloadTree.Name = "BuStepReloadTree";
            this.BuStepReloadTree.Size = new System.Drawing.Size(33, 33);
            this.BuStepReloadTree.TabIndex = 10;
            this.BuStepReloadTree.UseVisualStyleBackColor = true;
            this.BuStepReloadTree.Click += new System.EventHandler(this.BuStepReloadTree_Click);
            // 
            // BuStepUp
            // 
            this.BuStepUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepUp.ImageIndex = 8;
            this.BuStepUp.ImageList = this.ImButtons;
            this.BuStepUp.Location = new System.Drawing.Point(707, 42);
            this.BuStepUp.Name = "BuStepUp";
            this.BuStepUp.Size = new System.Drawing.Size(33, 33);
            this.BuStepUp.TabIndex = 8;
            this.BuStepUp.UseVisualStyleBackColor = true;
            this.BuStepUp.Click += new System.EventHandler(this.BuStepUp_Click);
            // 
            // BuStepPaste
            // 
            this.BuStepPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepPaste.ImageIndex = 5;
            this.BuStepPaste.ImageList = this.ImButtons;
            this.BuStepPaste.Location = new System.Drawing.Point(746, 120);
            this.BuStepPaste.Name = "BuStepPaste";
            this.BuStepPaste.Size = new System.Drawing.Size(33, 33);
            this.BuStepPaste.TabIndex = 12;
            this.BuStepPaste.UseVisualStyleBackColor = true;
            this.BuStepPaste.Click += new System.EventHandler(this.BuStepPaste_Click);
            // 
            // BuStepClearFocus
            // 
            this.BuStepClearFocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepClearFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepClearFocus.ImageIndex = 1;
            this.BuStepClearFocus.ImageList = this.ImButtons;
            this.BuStepClearFocus.Location = new System.Drawing.Point(746, 81);
            this.BuStepClearFocus.Name = "BuStepClearFocus";
            this.BuStepClearFocus.Size = new System.Drawing.Size(33, 33);
            this.BuStepClearFocus.TabIndex = 9;
            this.BuStepClearFocus.UseVisualStyleBackColor = true;
            this.BuStepClearFocus.Click += new System.EventHandler(this.BuStepClearFocus_Click);
            // 
            // BuStepCopy
            // 
            this.BuStepCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BuStepCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuStepCopy.ImageIndex = 2;
            this.BuStepCopy.ImageList = this.ImButtons;
            this.BuStepCopy.Location = new System.Drawing.Point(707, 120);
            this.BuStepCopy.Name = "BuStepCopy";
            this.BuStepCopy.Size = new System.Drawing.Size(33, 33);
            this.BuStepCopy.TabIndex = 11;
            this.BuStepCopy.UseVisualStyleBackColor = true;
            this.BuStepCopy.Click += new System.EventHandler(this.BuStepCopy_Click);
            // 
            // PaMiddleDownDown
            // 
            this.PaMiddleDownDown.Controls.Add(this.PaMiddleDownDownLeft);
            this.PaMiddleDownDown.Controls.Add(this.SpMiddleDownDownRight);
            this.PaMiddleDownDown.Controls.Add(this.PaMiddleDownDownRight);
            this.PaMiddleDownDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PaMiddleDownDown.Location = new System.Drawing.Point(3, 339);
            this.PaMiddleDownDown.Name = "PaMiddleDownDown";
            this.PaMiddleDownDown.Size = new System.Drawing.Size(782, 199);
            this.PaMiddleDownDown.TabIndex = 15;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.RiLog);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 199);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Лог шага";
            // 
            // RiLog
            // 
            this.RiLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RiLog.Location = new System.Drawing.Point(6, 19);
            this.RiLog.Name = "RiLog";
            this.RiLog.Size = new System.Drawing.Size(188, 174);
            this.RiLog.TabIndex = 0;
            this.RiLog.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.CoStepSelectorType);
            this.groupBox3.Controls.Add(this.CoStepTypeGroup);
            this.groupBox3.Controls.Add(this.ChStepIsEnabled);
            this.groupBox3.Controls.Add(this.TeStepParameter);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.ChStepIgnoreError);
            this.groupBox3.Controls.Add(this.TeStepValue);
            this.groupBox3.Controls.Add(this.LaStepValue);
            this.groupBox3.Controls.Add(this.NuStepWait);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.TeStepSelector);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.CoStepType);
            this.groupBox3.Controls.Add(this.TeStepName);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(579, 199);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Параметры шага";
            // 
            // CoStepTypeGroup
            // 
            this.CoStepTypeGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CoStepTypeGroup.FormattingEnabled = true;
            this.CoStepTypeGroup.Items.AddRange(new object[] {
            "Группа шагов",
            "Ожидать нахождения элемента",
            "Кликнуть",
            "Проверить значение",
            "Проверка нахождения элемента",
            "Ждать время"});
            this.CoStepTypeGroup.Location = new System.Drawing.Point(69, 42);
            this.CoStepTypeGroup.Name = "CoStepTypeGroup";
            this.CoStepTypeGroup.Size = new System.Drawing.Size(325, 21);
            this.CoStepTypeGroup.TabIndex = 18;
            this.CoStepTypeGroup.SelectedIndexChanged += new System.EventHandler(this.CoStepTypeGroup_SelectedIndexChanged);
            // 
            // ChStepIsEnabled
            // 
            this.ChStepIsEnabled.AutoSize = true;
            this.ChStepIsEnabled.Location = new System.Drawing.Point(9, 19);
            this.ChStepIsEnabled.Name = "ChStepIsEnabled";
            this.ChStepIsEnabled.Size = new System.Drawing.Size(68, 17);
            this.ChStepIsEnabled.TabIndex = 17;
            this.ChStepIsEnabled.Text = "Активен";
            this.ChStepIsEnabled.UseVisualStyleBackColor = true;
            this.ChStepIsEnabled.CheckedChanged += new System.EventHandler(this.ChStepIsEnabled_CheckedChanged);
            // 
            // TeStepParameter
            // 
            this.TeStepParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TeStepParameter.Location = new System.Drawing.Point(141, 173);
            this.TeStepParameter.Name = "TeStepParameter";
            this.TeStepParameter.Size = new System.Drawing.Size(429, 20);
            this.TeStepParameter.TabIndex = 16;
            this.TeStepParameter.Visible = false;
            this.TeStepParameter.TextChanged += new System.EventHandler(this.TeStepParameter_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Параметр";
            // 
            // ChStepIgnoreError
            // 
            this.ChStepIgnoreError.AutoSize = true;
            this.ChStepIgnoreError.Location = new System.Drawing.Point(83, 19);
            this.ChStepIgnoreError.Name = "ChStepIgnoreError";
            this.ChStepIgnoreError.Size = new System.Drawing.Size(138, 17);
            this.ChStepIgnoreError.TabIndex = 14;
            this.ChStepIgnoreError.Text = "Игнорировать ошибку";
            this.ChStepIgnoreError.UseVisualStyleBackColor = true;
            this.ChStepIgnoreError.Visible = false;
            this.ChStepIgnoreError.CheckedChanged += new System.EventHandler(this.ChStepIgnoreError_CheckedChanged);
            // 
            // TeStepValue
            // 
            this.TeStepValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TeStepValue.Location = new System.Drawing.Point(141, 147);
            this.TeStepValue.Name = "TeStepValue";
            this.TeStepValue.Size = new System.Drawing.Size(429, 20);
            this.TeStepValue.TabIndex = 11;
            this.TeStepValue.Visible = false;
            this.TeStepValue.TextChanged += new System.EventHandler(this.TeStepValue_TextChanged);
            // 
            // LaStepValue
            // 
            this.LaStepValue.AutoSize = true;
            this.LaStepValue.Location = new System.Drawing.Point(6, 150);
            this.LaStepValue.Name = "LaStepValue";
            this.LaStepValue.Size = new System.Drawing.Size(55, 13);
            this.LaStepValue.TabIndex = 10;
            this.LaStepValue.Text = "Значение";
            // 
            // NuStepWait
            // 
            this.NuStepWait.DecimalPlaces = 3;
            this.NuStepWait.Location = new System.Drawing.Point(141, 121);
            this.NuStepWait.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.NuStepWait.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.NuStepWait.Name = "NuStepWait";
            this.NuStepWait.Size = new System.Drawing.Size(79, 20);
            this.NuStepWait.TabIndex = 9;
            this.NuStepWait.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NuStepWait.Visible = false;
            this.NuStepWait.ValueChanged += new System.EventHandler(this.NuStepWait_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Время ожидания (сек)";
            // 
            // TeStepSelector
            // 
            this.TeStepSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TeStepSelector.Location = new System.Drawing.Point(141, 95);
            this.TeStepSelector.Name = "TeStepSelector";
            this.TeStepSelector.Size = new System.Drawing.Size(429, 20);
            this.TeStepSelector.TabIndex = 7;
            this.TeStepSelector.Visible = false;
            this.TeStepSelector.TextChanged += new System.EventHandler(this.TeStepSelector_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Селектор";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Тип";
            // 
            // CoStepType
            // 
            this.CoStepType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CoStepType.FormattingEnabled = true;
            this.CoStepType.Items.AddRange(new object[] {
            "Группа шагов",
            "Ожидать нахождения элемента",
            "Кликнуть",
            "Проверить значение",
            "Проверка нахождения элемента",
            "Ждать время"});
            this.CoStepType.Location = new System.Drawing.Point(400, 42);
            this.CoStepType.Name = "CoStepType";
            this.CoStepType.Size = new System.Drawing.Size(170, 21);
            this.CoStepType.TabIndex = 4;
            this.CoStepType.SelectedIndexChanged += new System.EventHandler(this.CoStepType_SelectedIndexChanged);
            // 
            // TeStepName
            // 
            this.TeStepName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TeStepName.Location = new System.Drawing.Point(69, 69);
            this.TeStepName.Name = "TeStepName";
            this.TeStepName.Size = new System.Drawing.Size(501, 20);
            this.TeStepName.TabIndex = 3;
            this.TeStepName.TextChanged += new System.EventHandler(this.TeStepName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Название";
            // 
            // TeTestName
            // 
            this.TeTestName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TeTestName.Location = new System.Drawing.Point(69, 19);
            this.TeTestName.Name = "TeTestName";
            this.TeTestName.Size = new System.Drawing.Size(523, 20);
            this.TeTestName.TabIndex = 1;
            this.TeTestName.TextChanged += new System.EventHandler(this.TeTestName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название";
            // 
            // GrTestParameters
            // 
            this.GrTestParameters.Controls.Add(this.ChTestRegenerateParameters);
            this.GrTestParameters.Controls.Add(this.BuTestGenerateParameters);
            this.GrTestParameters.Controls.Add(this.DaTestParameters);
            this.GrTestParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrTestParameters.Enabled = false;
            this.GrTestParameters.Location = new System.Drawing.Point(0, 0);
            this.GrTestParameters.Name = "GrTestParameters";
            this.GrTestParameters.Size = new System.Drawing.Size(372, 192);
            this.GrTestParameters.TabIndex = 13;
            this.GrTestParameters.TabStop = false;
            this.GrTestParameters.Text = "Параметры теста";
            // 
            // ChTestRegenerateParameters
            // 
            this.ChTestRegenerateParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChTestRegenerateParameters.AutoSize = true;
            this.ChTestRegenerateParameters.Location = new System.Drawing.Point(6, 142);
            this.ChTestRegenerateParameters.Name = "ChTestRegenerateParameters";
            this.ChTestRegenerateParameters.Size = new System.Drawing.Size(356, 17);
            this.ChTestRegenerateParameters.TabIndex = 6;
            this.ChTestRegenerateParameters.Text = "Генерировать параметры автоматически при запуске автотеста";
            this.ChTestRegenerateParameters.UseVisualStyleBackColor = true;
            this.ChTestRegenerateParameters.CheckedChanged += new System.EventHandler(this.ChTestRegenerateParameters_CheckedChanged);
            // 
            // BuTestGenerateParameters
            // 
            this.BuTestGenerateParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BuTestGenerateParameters.Location = new System.Drawing.Point(6, 163);
            this.BuTestGenerateParameters.Name = "BuTestGenerateParameters";
            this.BuTestGenerateParameters.Size = new System.Drawing.Size(360, 23);
            this.BuTestGenerateParameters.TabIndex = 5;
            this.BuTestGenerateParameters.Text = "Сгенерировать";
            this.BuTestGenerateParameters.UseVisualStyleBackColor = true;
            this.BuTestGenerateParameters.Click += new System.EventHandler(this.BuTestGenerateParameters_Click);
            // 
            // DaTestParameters
            // 
            this.DaTestParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DaTestParameters.BackgroundColor = System.Drawing.Color.White;
            this.DaTestParameters.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DaTestParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DaTestParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CoTestParameterName,
            this.CoTestParametersPattern,
            this.CoTestParameterValue});
            this.DaTestParameters.EnableHeadersVisualStyles = false;
            this.DaTestParameters.Location = new System.Drawing.Point(6, 16);
            this.DaTestParameters.MultiSelect = false;
            this.DaTestParameters.Name = "DaTestParameters";
            this.DaTestParameters.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DaTestParameters.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.DaTestParameters.Size = new System.Drawing.Size(360, 120);
            this.DaTestParameters.TabIndex = 0;
            this.DaTestParameters.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DaTestParameters_CellEndEdit);
            // 
            // CoTestParameterName
            // 
            this.CoTestParameterName.HeaderText = "Название";
            this.CoTestParameterName.Name = "CoTestParameterName";
            this.CoTestParameterName.Width = 80;
            // 
            // CoTestParametersPattern
            // 
            this.CoTestParametersPattern.HeaderText = "Шаблон/Значение";
            this.CoTestParametersPattern.Name = "CoTestParametersPattern";
            this.CoTestParametersPattern.Width = 110;
            // 
            // CoTestParameterValue
            // 
            this.CoTestParameterValue.HeaderText = "Итог";
            this.CoTestParameterValue.Name = "CoTestParameterValue";
            this.CoTestParameterValue.ReadOnly = true;
            this.CoTestParameterValue.Width = 110;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ChProjectRegenerateParameters);
            this.groupBox5.Controls.Add(this.BuProjectGenerateParameters);
            this.groupBox5.Controls.Add(this.DaProjectParameters);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(372, 392);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Параметры проекта";
            // 
            // ChProjectRegenerateParameters
            // 
            this.ChProjectRegenerateParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChProjectRegenerateParameters.AutoSize = true;
            this.ChProjectRegenerateParameters.Location = new System.Drawing.Point(6, 342);
            this.ChProjectRegenerateParameters.Name = "ChProjectRegenerateParameters";
            this.ChProjectRegenerateParameters.Size = new System.Drawing.Size(356, 17);
            this.ChProjectRegenerateParameters.TabIndex = 6;
            this.ChProjectRegenerateParameters.Text = "Генерировать параметры автоматически при запуске автотеста";
            this.ChProjectRegenerateParameters.UseVisualStyleBackColor = true;
            this.ChProjectRegenerateParameters.CheckedChanged += new System.EventHandler(this.ChProjectRegenerateParameters_CheckedChanged);
            // 
            // BuProjectGenerateParameters
            // 
            this.BuProjectGenerateParameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BuProjectGenerateParameters.Location = new System.Drawing.Point(6, 363);
            this.BuProjectGenerateParameters.Name = "BuProjectGenerateParameters";
            this.BuProjectGenerateParameters.Size = new System.Drawing.Size(360, 23);
            this.BuProjectGenerateParameters.TabIndex = 5;
            this.BuProjectGenerateParameters.Text = "Сгенерировать";
            this.BuProjectGenerateParameters.UseVisualStyleBackColor = true;
            this.BuProjectGenerateParameters.Click += new System.EventHandler(this.BuProjectGenerateParameters_Click);
            // 
            // DaProjectParameters
            // 
            this.DaProjectParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DaProjectParameters.BackgroundColor = System.Drawing.Color.White;
            this.DaProjectParameters.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DaProjectParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DaProjectParameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.DaProjectParameters.EnableHeadersVisualStyles = false;
            this.DaProjectParameters.Location = new System.Drawing.Point(6, 16);
            this.DaProjectParameters.MultiSelect = false;
            this.DaProjectParameters.Name = "DaProjectParameters";
            this.DaProjectParameters.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DaProjectParameters.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.DaProjectParameters.Size = new System.Drawing.Size(360, 320);
            this.DaProjectParameters.TabIndex = 0;
            this.DaProjectParameters.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DaProjectParameters_CellEndEdit);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Название";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Шаблон/Значение";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 110;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Итог";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 110;
            // 
            // GrTestSettings
            // 
            this.GrTestSettings.Controls.Add(this.ChTestRunAfterPrevious);
            this.GrTestSettings.Controls.Add(this.TeTestName);
            this.GrTestSettings.Controls.Add(this.label1);
            this.GrTestSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrTestSettings.Enabled = false;
            this.GrTestSettings.Location = new System.Drawing.Point(3, 0);
            this.GrTestSettings.Name = "GrTestSettings";
            this.GrTestSettings.Size = new System.Drawing.Size(788, 46);
            this.GrTestSettings.TabIndex = 15;
            this.GrTestSettings.TabStop = false;
            this.GrTestSettings.Text = "Настройки теста";
            // 
            // ChTestRunAfterPrevious
            // 
            this.ChTestRunAfterPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChTestRunAfterPrevious.AutoSize = true;
            this.ChTestRunAfterPrevious.Location = new System.Drawing.Point(598, 21);
            this.ChTestRunAfterPrevious.Name = "ChTestRunAfterPrevious";
            this.ChTestRunAfterPrevious.Size = new System.Drawing.Size(184, 17);
            this.ChTestRunAfterPrevious.TabIndex = 2;
            this.ChTestRunAfterPrevious.Text = "Запускать после предыдущего";
            this.ChTestRunAfterPrevious.UseVisualStyleBackColor = true;
            this.ChTestRunAfterPrevious.CheckedChanged += new System.EventHandler(this.ChTestRunAfterPrevious_CheckedChanged);
            // 
            // PaRight
            // 
            this.PaRight.Controls.Add(this.PaRightUp);
            this.PaRight.Controls.Add(this.SpRightDown);
            this.PaRight.Controls.Add(this.PaRightDown);
            this.PaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.PaRight.Location = new System.Drawing.Point(1032, 24);
            this.PaRight.Name = "PaRight";
            this.PaRight.Size = new System.Drawing.Size(372, 587);
            this.PaRight.TabIndex = 17;
            // 
            // PaMiddle
            // 
            this.PaMiddle.Controls.Add(this.PaMIddleDown);
            this.PaMiddle.Controls.Add(this.PaMiddleUp);
            this.PaMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaMiddle.Location = new System.Drawing.Point(238, 24);
            this.PaMiddle.Name = "PaMiddle";
            this.PaMiddle.Size = new System.Drawing.Size(791, 587);
            this.PaMiddle.TabIndex = 2;
            // 
            // CoStepSelectorType
            // 
            this.CoStepSelectorType.FormattingEnabled = true;
            this.CoStepSelectorType.Location = new System.Drawing.Point(69, 95);
            this.CoStepSelectorType.Name = "CoStepSelectorType";
            this.CoStepSelectorType.Size = new System.Drawing.Size(66, 21);
            this.CoStepSelectorType.TabIndex = 19;
            this.CoStepSelectorType.Text = "id";
            this.CoStepSelectorType.Visible = false;
            this.CoStepSelectorType.SelectedIndexChanged += new System.EventHandler(this.CoStepSelectorType_SelectedIndexChanged);
            // 
            // SpRight
            // 
            this.SpRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.SpRight.Location = new System.Drawing.Point(1029, 24);
            this.SpRight.Name = "SpRight";
            this.SpRight.Size = new System.Drawing.Size(3, 587);
            this.SpRight.TabIndex = 21;
            this.SpRight.TabStop = false;
            // 
            // PaLeft
            // 
            this.PaLeft.Controls.Add(this.groupBox1);
            this.PaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.PaLeft.Location = new System.Drawing.Point(0, 24);
            this.PaLeft.Name = "PaLeft";
            this.PaLeft.Size = new System.Drawing.Size(238, 587);
            this.PaLeft.TabIndex = 22;
            // 
            // SpLeft
            // 
            this.SpLeft.Location = new System.Drawing.Point(238, 24);
            this.SpLeft.Name = "SpLeft";
            this.SpLeft.Size = new System.Drawing.Size(3, 587);
            this.SpLeft.TabIndex = 23;
            this.SpLeft.TabStop = false;
            // 
            // PaRightDown
            // 
            this.PaRightDown.Controls.Add(this.GrTestParameters);
            this.PaRightDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PaRightDown.Location = new System.Drawing.Point(0, 395);
            this.PaRightDown.Name = "PaRightDown";
            this.PaRightDown.Size = new System.Drawing.Size(372, 192);
            this.PaRightDown.TabIndex = 15;
            // 
            // SpRightDown
            // 
            this.SpRightDown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SpRightDown.Location = new System.Drawing.Point(0, 392);
            this.SpRightDown.Name = "SpRightDown";
            this.SpRightDown.Size = new System.Drawing.Size(372, 3);
            this.SpRightDown.TabIndex = 16;
            this.SpRightDown.TabStop = false;
            // 
            // PaRightUp
            // 
            this.PaRightUp.Controls.Add(this.groupBox5);
            this.PaRightUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaRightUp.Location = new System.Drawing.Point(0, 0);
            this.PaRightUp.Name = "PaRightUp";
            this.PaRightUp.Size = new System.Drawing.Size(372, 392);
            this.PaRightUp.TabIndex = 17;
            // 
            // PaMiddleUp
            // 
            this.PaMiddleUp.Controls.Add(this.GrTestSettings);
            this.PaMiddleUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.PaMiddleUp.Location = new System.Drawing.Point(0, 0);
            this.PaMiddleUp.Name = "PaMiddleUp";
            this.PaMiddleUp.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.PaMiddleUp.Size = new System.Drawing.Size(791, 46);
            this.PaMiddleUp.TabIndex = 16;
            // 
            // PaMIddleDown
            // 
            this.PaMIddleDown.Controls.Add(this.GrTestSteps);
            this.PaMIddleDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaMIddleDown.Location = new System.Drawing.Point(0, 46);
            this.PaMIddleDown.Name = "PaMIddleDown";
            this.PaMIddleDown.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.PaMIddleDown.Size = new System.Drawing.Size(791, 541);
            this.PaMIddleDown.TabIndex = 17;
            // 
            // PaMiddleDownDownRight
            // 
            this.PaMiddleDownDownRight.Controls.Add(this.groupBox4);
            this.PaMiddleDownDownRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.PaMiddleDownDownRight.Location = new System.Drawing.Point(582, 0);
            this.PaMiddleDownDownRight.Name = "PaMiddleDownDownRight";
            this.PaMiddleDownDownRight.Size = new System.Drawing.Size(200, 199);
            this.PaMiddleDownDownRight.TabIndex = 5;
            // 
            // SpMiddleDownDownRight
            // 
            this.SpMiddleDownDownRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.SpMiddleDownDownRight.Location = new System.Drawing.Point(579, 0);
            this.SpMiddleDownDownRight.Name = "SpMiddleDownDownRight";
            this.SpMiddleDownDownRight.Size = new System.Drawing.Size(3, 199);
            this.SpMiddleDownDownRight.TabIndex = 6;
            this.SpMiddleDownDownRight.TabStop = false;
            // 
            // PaMiddleDownDownLeft
            // 
            this.PaMiddleDownDownLeft.Controls.Add(this.groupBox3);
            this.PaMiddleDownDownLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PaMiddleDownDownLeft.Location = new System.Drawing.Point(0, 0);
            this.PaMiddleDownDownLeft.Name = "PaMiddleDownDownLeft";
            this.PaMiddleDownDownLeft.Size = new System.Drawing.Size(579, 199);
            this.PaMiddleDownDownLeft.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1404, 611);
            this.Controls.Add(this.SpLeft);
            this.Controls.Add(this.PaMiddle);
            this.Controls.Add(this.PaLeft);
            this.Controls.Add(this.SpRight);
            this.Controls.Add(this.PaRight);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Selenium Autotest IDE by alextrof94";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.GrTestSteps.ResumeLayout(false);
            this.PaMiddleDownMiddle.ResumeLayout(false);
            this.PaMiddleDownDown.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NuStepWait)).EndInit();
            this.GrTestParameters.ResumeLayout(false);
            this.GrTestParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DaTestParameters)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DaProjectParameters)).EndInit();
            this.GrTestSettings.ResumeLayout(false);
            this.GrTestSettings.PerformLayout();
            this.PaRight.ResumeLayout(false);
            this.PaMiddle.ResumeLayout(false);
            this.PaLeft.ResumeLayout(false);
            this.PaRightDown.ResumeLayout(false);
            this.PaRightUp.ResumeLayout(false);
            this.PaMiddleUp.ResumeLayout(false);
            this.PaMIddleDown.ResumeLayout(false);
            this.PaMiddleDownDownRight.ResumeLayout(false);
            this.PaMiddleDownDownLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem BuFileSave;
        private System.Windows.Forms.ListBox LiTests;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BuTestDelete;
        private System.Windows.Forms.Button BuTestAdd;
        private System.Windows.Forms.Button BuTestRun;
        private System.Windows.Forms.GroupBox GrTestSteps;
        private System.Windows.Forms.TreeView TrSteps;
        private System.Windows.Forms.TextBox TeTestName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CoStepType;
        private System.Windows.Forms.TextBox TeStepName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button BuTestClone;
        private System.Windows.Forms.Button BuStepDelete;
        private System.Windows.Forms.Button BuStepAdd;
        private System.Windows.Forms.Button BuStepClone;
        private System.Windows.Forms.ToolStripMenuItem BuFileImport;
        private System.Windows.Forms.TextBox TeStepSelector;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown NuStepWait;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TeStepValue;
        private System.Windows.Forms.Label LaStepValue;
        private System.Windows.Forms.RichTextBox RiLog;
        private System.Windows.Forms.Button BuStepUp;
        private System.Windows.Forms.Button BuStepDown;
        private System.Windows.Forms.Button BuTestStop;
        private System.Windows.Forms.Button BuStepClearFocus;
        private System.Windows.Forms.Button BuStepReloadTree;
        private System.Windows.Forms.Label LaTestTime;
        private System.Windows.Forms.ImageList ImTree;
        private System.Windows.Forms.Button BuStepPaste;
        private System.Windows.Forms.Button BuStepCopy;
        private System.Windows.Forms.CheckBox ChStepIgnoreError;
        private System.Windows.Forms.ToolStripMenuItem BuFileOpenLast;
        private System.Windows.Forms.GroupBox GrTestParameters;
        private System.Windows.Forms.DataGridView DaTestParameters;
        private System.Windows.Forms.Button BuTestGenerateParameters;
        private System.Windows.Forms.CheckBox ChTestRegenerateParameters;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox ChProjectRegenerateParameters;
        private System.Windows.Forms.Button BuProjectGenerateParameters;
        private System.Windows.Forms.DataGridView DaProjectParameters;
        private System.Windows.Forms.DataGridViewTextBoxColumn CoTestParameterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CoTestParametersPattern;
        private System.Windows.Forms.DataGridViewTextBoxColumn CoTestParameterValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.GroupBox GrTestSettings;
        private System.Windows.Forms.CheckBox ChTestRunAfterPrevious;
        private System.Windows.Forms.TextBox TeStepParameter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ImageList ImButtons;
        private System.Windows.Forms.Button BuFontDecrease;
        private System.Windows.Forms.Button BuFontIncrease;
        private System.Windows.Forms.Button BuTestDown;
        private System.Windows.Forms.Button BuTestUp;
        private System.Windows.Forms.ToolStripMenuItem инфоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BuCantDownloadDriverHelp;
        private System.Windows.Forms.Label LaRunTime;
        private System.Windows.Forms.CheckBox ChStepIsEnabled;
        private System.Windows.Forms.CheckBox ChSlowMode;
        private System.Windows.Forms.CheckBox ChSelectFoundElements;
        private System.Windows.Forms.ToolStripMenuItem BuTestParametersHelp;
        private System.Windows.Forms.Panel PaRight;
        private System.Windows.Forms.Panel PaMiddle;
        private System.Windows.Forms.Panel PaMiddleDownDown;
        private System.Windows.Forms.Panel PaMiddleDownMiddle;
        private System.Windows.Forms.ComboBox CoStepTypeGroup;
        private System.Windows.Forms.ComboBox CoStepSelectorType;
        private System.Windows.Forms.Splitter SpRight;
        private System.Windows.Forms.Panel PaLeft;
        private System.Windows.Forms.Panel PaRightUp;
        private System.Windows.Forms.Splitter SpRightDown;
        private System.Windows.Forms.Panel PaRightDown;
        private System.Windows.Forms.Panel PaMiddleUp;
        private System.Windows.Forms.Splitter SpLeft;
        private System.Windows.Forms.Panel PaMIddleDown;
        private System.Windows.Forms.Panel PaMiddleDownDownLeft;
        private System.Windows.Forms.Splitter SpMiddleDownDownRight;
        private System.Windows.Forms.Panel PaMiddleDownDownRight;
    }
}

