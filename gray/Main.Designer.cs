namespace Gray
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.fileNameBox = new System.Windows.Forms.TextBox();
            this.chooseFile = new System.Windows.Forms.Button();
            this.previewBox = new System.Windows.Forms.PictureBox();
            this.graying = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SubAreaSize = new System.Windows.Forms.TextBox();
            this.FindAreas = new System.Windows.Forms.Button();
            this.DOGDiff = new System.Windows.Forms.Button();
            this.RecPosition = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DOGFind = new System.Windows.Forms.Button();
            this.peaklevel = new System.Windows.Forms.TextBox();
            this.reset = new System.Windows.Forms.Button();
            this.GaossionCor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Gaossion = new System.Windows.Forms.Button();
            this.average2BW = new System.Windows.Forms.Button();
            this.shift = new System.Windows.Forms.Button();
            this.imageCol = new System.Windows.Forms.ComboBox();
            this.clearDraw = new System.Windows.Forms.Button();
            this.FreeDraw = new System.Windows.Forms.Button();
            this.picHeight = new System.Windows.Forms.Label();
            this.picWidth = new System.Windows.Forms.Label();
            this.drawRect = new System.Windows.Forms.Button();
            this.BW2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开分析文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存问ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更改图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.RAMDisplay = new System.Windows.Forms.ToolStripStatusLabel();
            this.CPUDisplay = new System.Windows.Forms.ToolStripStatusLabel();
            this.process = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.timeTick = new System.Windows.Forms.ToolStripStatusLabel();
            this.mouseStrip = new System.Windows.Forms.ToolStripStatusLabel();
            this.average = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.保存图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileNameBox
            // 
            this.fileNameBox.Enabled = false;
            this.fileNameBox.Font = new System.Drawing.Font("SimSun", 12F);
            this.fileNameBox.Location = new System.Drawing.Point(25, 148);
            this.fileNameBox.Margin = new System.Windows.Forms.Padding(4);
            this.fileNameBox.Name = "fileNameBox";
            this.fileNameBox.Size = new System.Drawing.Size(470, 30);
            this.fileNameBox.TabIndex = 0;
            // 
            // chooseFile
            // 
            this.chooseFile.Font = new System.Drawing.Font("SimSun", 12F);
            this.chooseFile.Location = new System.Drawing.Point(25, 203);
            this.chooseFile.Margin = new System.Windows.Forms.Padding(4);
            this.chooseFile.Name = "chooseFile";
            this.chooseFile.Size = new System.Drawing.Size(155, 32);
            this.chooseFile.TabIndex = 2;
            this.chooseFile.Text = "更新图片";
            this.chooseFile.UseVisualStyleBackColor = true;
            this.chooseFile.Click += new System.EventHandler(this.ChooseFile_Click);
            // 
            // previewBox
            // 
            this.previewBox.Location = new System.Drawing.Point(20, 30);
            this.previewBox.Margin = new System.Windows.Forms.Padding(4);
            this.previewBox.Name = "previewBox";
            this.previewBox.Size = new System.Drawing.Size(800, 600);
            this.previewBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.previewBox.TabIndex = 3;
            this.previewBox.TabStop = false;
            this.previewBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PreviewBox_MouseDown);
            this.previewBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PreviewBox_MouseMove);
            this.previewBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PreviewBox_MouseUp);
            // 
            // graying
            // 
            this.graying.Font = new System.Drawing.Font("SimSun", 12F);
            this.graying.Location = new System.Drawing.Point(342, 203);
            this.graying.Margin = new System.Windows.Forms.Padding(4);
            this.graying.Name = "graying";
            this.graying.Size = new System.Drawing.Size(153, 32);
            this.graying.TabIndex = 4;
            this.graying.Text = "灰度";
            this.graying.UseVisualStyleBackColor = true;
            this.graying.Click += new System.EventHandler(this.Graying_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SubAreaSize);
            this.groupBox1.Controls.Add(this.FindAreas);
            this.groupBox1.Controls.Add(this.DOGDiff);
            this.groupBox1.Controls.Add(this.RecPosition);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.DOGFind);
            this.groupBox1.Controls.Add(this.peaklevel);
            this.groupBox1.Controls.Add(this.reset);
            this.groupBox1.Controls.Add(this.GaossionCor);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Gaossion);
            this.groupBox1.Controls.Add(this.average2BW);
            this.groupBox1.Controls.Add(this.shift);
            this.groupBox1.Controls.Add(this.imageCol);
            this.groupBox1.Controls.Add(this.clearDraw);
            this.groupBox1.Controls.Add(this.FreeDraw);
            this.groupBox1.Controls.Add(this.picHeight);
            this.groupBox1.Controls.Add(this.picWidth);
            this.groupBox1.Controls.Add(this.drawRect);
            this.groupBox1.Controls.Add(this.BW2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.graying);
            this.groupBox1.Controls.Add(this.chooseFile);
            this.groupBox1.Controls.Add(this.fileNameBox);
            this.groupBox1.Font = new System.Drawing.Font("SimSun", 12F);
            this.groupBox1.Location = new System.Drawing.Point(875, 63);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(518, 630);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工作空间";
            // 
            // SubAreaSize
            // 
            this.SubAreaSize.Location = new System.Drawing.Point(193, 525);
            this.SubAreaSize.Name = "SubAreaSize";
            this.SubAreaSize.Size = new System.Drawing.Size(77, 30);
            this.SubAreaSize.TabIndex = 35;
            // 
            // FindAreas
            // 
            this.FindAreas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.FindAreas.Font = new System.Drawing.Font("SimSun", 12F);
            this.FindAreas.Location = new System.Drawing.Point(25, 525);
            this.FindAreas.Margin = new System.Windows.Forms.Padding(4);
            this.FindAreas.Name = "FindAreas";
            this.FindAreas.Size = new System.Drawing.Size(148, 32);
            this.FindAreas.TabIndex = 34;
            this.FindAreas.Text = "查找子区域";
            this.FindAreas.UseVisualStyleBackColor = true;
            this.FindAreas.Click += new System.EventHandler(this.FindAreas_Click);
            // 
            // DOGDiff
            // 
            this.DOGDiff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DOGDiff.Font = new System.Drawing.Font("SimSun", 12F);
            this.DOGDiff.Location = new System.Drawing.Point(337, 488);
            this.DOGDiff.Margin = new System.Windows.Forms.Padding(4);
            this.DOGDiff.Name = "DOGDiff";
            this.DOGDiff.Size = new System.Drawing.Size(124, 32);
            this.DOGDiff.TabIndex = 33;
            this.DOGDiff.Text = "显示DOG差分结果";
            this.DOGDiff.UseVisualStyleBackColor = true;
            this.DOGDiff.Click += new System.EventHandler(this.DOGDiff_Click);
            // 
            // RecPosition
            // 
            this.RecPosition.AutoSize = true;
            this.RecPosition.Location = new System.Drawing.Point(333, 329);
            this.RecPosition.Name = "RecPosition";
            this.RecPosition.Size = new System.Drawing.Size(29, 20);
            this.RecPosition.TabIndex = 32;
            this.RecPosition.Text = "  ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(175, 329);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 20);
            this.label2.TabIndex = 32;
            this.label2.Text = "选取的区域为：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 494);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "阈值";
            // 
            // DOGFind
            // 
            this.DOGFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DOGFind.Font = new System.Drawing.Font("SimSun", 12F);
            this.DOGFind.Location = new System.Drawing.Point(193, 488);
            this.DOGFind.Margin = new System.Windows.Forms.Padding(4);
            this.DOGFind.Name = "DOGFind";
            this.DOGFind.Size = new System.Drawing.Size(124, 32);
            this.DOGFind.TabIndex = 31;
            this.DOGFind.Text = "显示极值点";
            this.DOGFind.UseVisualStyleBackColor = true;
            this.DOGFind.Click += new System.EventHandler(this.DOG_Click);
            // 
            // peaklevel
            // 
            this.peaklevel.Location = new System.Drawing.Point(96, 488);
            this.peaklevel.Name = "peaklevel";
            this.peaklevel.Size = new System.Drawing.Size(77, 30);
            this.peaklevel.TabIndex = 15;
            // 
            // reset
            // 
            this.reset.Font = new System.Drawing.Font("SimSun", 12F);
            this.reset.Location = new System.Drawing.Point(188, 203);
            this.reset.Margin = new System.Windows.Forms.Padding(4);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(146, 32);
            this.reset.TabIndex = 30;
            this.reset.Text = "重设当前图片";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // GaossionCor
            // 
            this.GaossionCor.Location = new System.Drawing.Point(397, 451);
            this.GaossionCor.Name = "GaossionCor";
            this.GaossionCor.Size = new System.Drawing.Size(77, 30);
            this.GaossionCor.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(294, 456);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 28;
            this.label4.Text = "高斯系数";
            // 
            // Gaossion
            // 
            this.Gaossion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Gaossion.Font = new System.Drawing.Font("SimSun", 12F);
            this.Gaossion.Location = new System.Drawing.Point(175, 451);
            this.Gaossion.Margin = new System.Windows.Forms.Padding(4);
            this.Gaossion.Name = "Gaossion";
            this.Gaossion.Size = new System.Drawing.Size(112, 31);
            this.Gaossion.TabIndex = 27;
            this.Gaossion.Text = "高斯平滑";
            this.Gaossion.UseVisualStyleBackColor = true;
            this.Gaossion.Click += new System.EventHandler(this.Gaossion_Click);
            // 
            // average2BW
            // 
            this.average2BW.Font = new System.Drawing.Font("SimSun", 12F);
            this.average2BW.Location = new System.Drawing.Point(25, 451);
            this.average2BW.Margin = new System.Windows.Forms.Padding(4);
            this.average2BW.Name = "average2BW";
            this.average2BW.Size = new System.Drawing.Size(138, 31);
            this.average2BW.TabIndex = 26;
            this.average2BW.Text = "平均二值";
            this.average2BW.UseVisualStyleBackColor = true;
            this.average2BW.Click += new System.EventHandler(this.Average2BW_Click);
            // 
            // shift
            // 
            this.shift.Font = new System.Drawing.Font("SimSun", 12F);
            this.shift.Location = new System.Drawing.Point(368, 55);
            this.shift.Margin = new System.Windows.Forms.Padding(4);
            this.shift.Name = "shift";
            this.shift.Size = new System.Drawing.Size(127, 28);
            this.shift.TabIndex = 1;
            this.shift.Text = "图片切换";
            this.shift.UseVisualStyleBackColor = true;
            this.shift.Click += new System.EventHandler(this.Shift_Click);
            // 
            // imageCol
            // 
            this.imageCol.FormattingEnabled = true;
            this.imageCol.Location = new System.Drawing.Point(25, 55);
            this.imageCol.Name = "imageCol";
            this.imageCol.Size = new System.Drawing.Size(336, 28);
            this.imageCol.TabIndex = 19;
            this.imageCol.SelectedIndexChanged += new System.EventHandler(this.ImageCol_SelectedIndexChanged);
            // 
            // clearDraw
            // 
            this.clearDraw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.clearDraw.Font = new System.Drawing.Font("SimSun", 12F);
            this.clearDraw.Location = new System.Drawing.Point(179, 364);
            this.clearDraw.Margin = new System.Windows.Forms.Padding(4);
            this.clearDraw.Name = "clearDraw";
            this.clearDraw.Size = new System.Drawing.Size(138, 26);
            this.clearDraw.TabIndex = 18;
            this.clearDraw.Text = "清除绘制图形";
            this.clearDraw.UseVisualStyleBackColor = true;
            this.clearDraw.Click += new System.EventHandler(this.ClearDraw_Click);
            // 
            // FreeDraw
            // 
            this.FreeDraw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.FreeDraw.Font = new System.Drawing.Font("SimSun", 12F);
            this.FreeDraw.Location = new System.Drawing.Point(25, 364);
            this.FreeDraw.Margin = new System.Windows.Forms.Padding(4);
            this.FreeDraw.Name = "FreeDraw";
            this.FreeDraw.Size = new System.Drawing.Size(138, 26);
            this.FreeDraw.TabIndex = 17;
            this.FreeDraw.Text = "自由绘制(关)";
            this.FreeDraw.UseVisualStyleBackColor = true;
            this.FreeDraw.Click += new System.EventHandler(this.FreeDraw_Click);
            // 
            // picHeight
            // 
            this.picHeight.AutoSize = true;
            this.picHeight.Location = new System.Drawing.Point(361, 370);
            this.picHeight.Name = "picHeight";
            this.picHeight.Size = new System.Drawing.Size(0, 20);
            this.picHeight.TabIndex = 16;
            // 
            // picWidth
            // 
            this.picWidth.AutoSize = true;
            this.picWidth.Location = new System.Drawing.Point(171, 370);
            this.picWidth.Name = "picWidth";
            this.picWidth.Size = new System.Drawing.Size(0, 20);
            this.picWidth.TabIndex = 15;
            // 
            // drawRect
            // 
            this.drawRect.Font = new System.Drawing.Font("SimSun", 12F);
            this.drawRect.Location = new System.Drawing.Point(25, 324);
            this.drawRect.Margin = new System.Windows.Forms.Padding(4);
            this.drawRect.Name = "drawRect";
            this.drawRect.Size = new System.Drawing.Size(138, 26);
            this.drawRect.TabIndex = 8;
            this.drawRect.Text = "框选分析区域";
            this.drawRect.UseVisualStyleBackColor = true;
            this.drawRect.Click += new System.EventHandler(this.Equal_Click);
            // 
            // BW2
            // 
            this.BW2.Font = new System.Drawing.Font("SimSun", 12F);
            this.BW2.Location = new System.Drawing.Point(342, 266);
            this.BW2.Margin = new System.Windows.Forms.Padding(4);
            this.BW2.Name = "BW2";
            this.BW2.Size = new System.Drawing.Size(153, 32);
            this.BW2.TabIndex = 7;
            this.BW2.Text = "二值化";
            this.BW2.UseVisualStyleBackColor = true;
            this.BW2.Click += new System.EventHandler(this.BW2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(156, 271);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "二值化分界点0-255";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(25, 268);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(125, 30);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "120";
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox1_KeyPress);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.编辑ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1422, 33);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开分析文件ToolStripMenuItem,
            this.保存ToolStripMenuItem,
            this.另存问ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(66, 29);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 打开分析文件ToolStripMenuItem
            // 
            this.打开分析文件ToolStripMenuItem.Name = "打开分析文件ToolStripMenuItem";
            this.打开分析文件ToolStripMenuItem.Size = new System.Drawing.Size(218, 30);
            this.打开分析文件ToolStripMenuItem.Text = "打开分析文件";
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(218, 30);
            this.保存ToolStripMenuItem.Text = "保存";
            // 
            // 另存问ToolStripMenuItem
            // 
            this.另存问ToolStripMenuItem.Name = "另存问ToolStripMenuItem";
            this.另存问ToolStripMenuItem.Size = new System.Drawing.Size(218, 30);
            this.另存问ToolStripMenuItem.Text = "另存为";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(218, 30);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click_1);
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.更改图片ToolStripMenuItem,
            this.重新计算ToolStripMenuItem,
            this.保存图片ToolStripMenuItem});
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(66, 29);
            this.编辑ToolStripMenuItem.Text = "编辑";
            // 
            // 更改图片ToolStripMenuItem
            // 
            this.更改图片ToolStripMenuItem.Name = "更改图片ToolStripMenuItem";
            this.更改图片ToolStripMenuItem.Size = new System.Drawing.Size(224, 30);
            this.更改图片ToolStripMenuItem.Text = "更改图片";
            this.更改图片ToolStripMenuItem.Click += new System.EventHandler(this.ChooseFile_Click);
            // 
            // 重新计算ToolStripMenuItem
            // 
            this.重新计算ToolStripMenuItem.Name = "重新计算ToolStripMenuItem";
            this.重新计算ToolStripMenuItem.Size = new System.Drawing.Size(224, 30);
            this.重新计算ToolStripMenuItem.Text = "重新计算";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(66, 29);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RAMDisplay,
            this.CPUDisplay,
            this.process,
            this.progressBar,
            this.timeTick,
            this.mouseStrip,
            this.average});
            this.statusStrip1.Location = new System.Drawing.Point(0, 727);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(0, 45, 0, 45);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1422, 26);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // RAMDisplay
            // 
            this.RAMDisplay.Margin = new System.Windows.Forms.Padding(36, 3, 36, 3);
            this.RAMDisplay.Name = "RAMDisplay";
            this.RAMDisplay.Size = new System.Drawing.Size(68, 20);
            this.RAMDisplay.Text = "工作内存";
            // 
            // CPUDisplay
            // 
            this.CPUDisplay.Margin = new System.Windows.Forms.Padding(36, 3, 36, 3);
            this.CPUDisplay.Name = "CPUDisplay";
            this.CPUDisplay.Size = new System.Drawing.Size(70, 20);
            this.CPUDisplay.Text = "CPU效能";
            // 
            // process
            // 
            this.process.Margin = new System.Windows.Forms.Padding(36, 3, 36, 3);
            this.process.Name = "process";
            this.process.Size = new System.Drawing.Size(71, 20);
            this.process.Text = "分析完成!";
            // 
            // progressBar
            // 
            this.progressBar.Margin = new System.Windows.Forms.Padding(36, 3, 36, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(151, 20);
            // 
            // timeTick
            // 
            this.timeTick.Margin = new System.Windows.Forms.Padding(36, 3, 36, 3);
            this.timeTick.Name = "timeTick";
            this.timeTick.Size = new System.Drawing.Size(68, 20);
            this.timeTick.Text = "已用时间";
            // 
            // mouseStrip
            // 
            this.mouseStrip.Name = "mouseStrip";
            this.mouseStrip.Size = new System.Drawing.Size(42, 20);
            this.mouseStrip.Text = "鼠标:";
            // 
            // average
            // 
            this.average.Name = "average";
            this.average.Size = new System.Drawing.Size(72, 20);
            this.average.Text = "平均灰度:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.previewBox);
            this.groupBox2.Location = new System.Drawing.Point(17, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(844, 647);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "图片显示";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // 保存图片ToolStripMenuItem
            // 
            this.保存图片ToolStripMenuItem.Name = "保存图片ToolStripMenuItem";
            this.保存图片ToolStripMenuItem.Size = new System.Drawing.Size(224, 30);
            this.保存图片ToolStripMenuItem.Text = "保存图片";
            this.保存图片ToolStripMenuItem.Click += new System.EventHandler(this.保存图片ToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1422, 753);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("SimSun", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.previewBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileNameBox;
        private System.Windows.Forms.Button chooseFile;
        private System.Windows.Forms.PictureBox previewBox;
        private System.Windows.Forms.Button graying;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开分析文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存问ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更改图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重新计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        public System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel RAMDisplay;
        private System.Windows.Forms.ToolStripStatusLabel CPUDisplay;
        private System.Windows.Forms.ToolStripStatusLabel process;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.ToolStripStatusLabel timeTick;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BW2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button drawRect;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label picWidth;
        private System.Windows.Forms.Label picHeight;
        private System.Windows.Forms.Button FreeDraw;
        private System.Windows.Forms.ToolStripStatusLabel mouseStrip;
        private System.Windows.Forms.Button clearDraw;
        private System.Windows.Forms.ComboBox imageCol;
        private System.Windows.Forms.Button shift;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button average2BW;
        private System.Windows.Forms.ToolStripStatusLabel average;
        private System.Windows.Forms.TextBox GaossionCor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Gaossion;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button DOGFind;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox peaklevel;
        private System.Windows.Forms.Label RecPosition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DOGDiff;
        private System.Windows.Forms.Button FindAreas;
        private System.Windows.Forms.TextBox SubAreaSize;
        private System.Windows.Forms.ToolStripMenuItem 保存图片ToolStripMenuItem;
    }
}

