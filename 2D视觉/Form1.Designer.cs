namespace _6524
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.数据 = new System.Windows.Forms.TabPage();
            this.logNetAnalysisControl1 = new HslCommunication.LogNet.LogNetAnalysisControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pLC设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.机械手示教ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.机械手控制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.模型设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.圆孔检测ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.字符条码二维码识别ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.形状匹配ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.像素匹配ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通讯设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.语言设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.简体中文ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.英语ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.西班牙语ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开发者选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作员登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开发者登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Bg_Main = new System.ComponentModel.BackgroundWorker();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button12 = new System.Windows.Forms.Button();
            this.btn_system_state = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bg_robot = new System.ComponentModel.BackgroundWorker();
            this.Bg_PLC_heartbeat = new System.ComponentModel.BackgroundWorker();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.数据.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            this.helpProvider1.SetShowHelp(this.panel2, ((bool)(resources.GetObject("panel2.ShowHelp"))));
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.数据);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.helpProvider1.SetShowHelp(this.tabControl1, ((bool)(resources.GetObject("tabControl1.ShowHelp"))));
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.helpProvider1.SetShowHelp(this.tabPage1, ((bool)(resources.GetObject("tabPage1.ShowHelp"))));
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.helpProvider1.SetShowHelp(this.tableLayoutPanel1, ((bool)(resources.GetObject("tableLayoutPanel1.ShowHelp"))));
            // 
            // 数据
            // 
            this.数据.Controls.Add(this.logNetAnalysisControl1);
            resources.ApplyResources(this.数据, "数据");
            this.数据.Name = "数据";
            this.helpProvider1.SetShowHelp(this.数据, ((bool)(resources.GetObject("数据.ShowHelp"))));
            this.数据.UseVisualStyleBackColor = true;
            // 
            // logNetAnalysisControl1
            // 
            resources.ApplyResources(this.logNetAnalysisControl1, "logNetAnalysisControl1");
            this.logNetAnalysisControl1.Name = "logNetAnalysisControl1";
            this.helpProvider1.SetShowHelp(this.logNetAnalysisControl1, ((bool)(resources.GetObject("logNetAnalysisControl1.ShowHelp"))));
            this.logNetAnalysisControl1.Load += new System.EventHandler(this.logNetAnalysisControl1_Load);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.文件ToolStripMenuItem,
            this.帮助ToolStripMenuItem,
            this.登录ToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            this.helpProvider1.SetShowHelp(this.menuStrip1, ((bool)(resources.GetObject("menuStrip1.ShowHelp"))));
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pLC设置ToolStripMenuItem,
            this.相机设置ToolStripMenuItem,
            this.机械手示教ToolStripMenuItem,
            this.机械手控制ToolStripMenuItem,
            this.模型设置ToolStripMenuItem,
            this.通讯设置ToolStripMenuItem,
            this.语言设置ToolStripMenuItem,
            this.开发者选项ToolStripMenuItem});
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            resources.ApplyResources(this.设置ToolStripMenuItem, "设置ToolStripMenuItem");
            // 
            // pLC设置ToolStripMenuItem
            // 
            this.pLC设置ToolStripMenuItem.Name = "pLC设置ToolStripMenuItem";
            resources.ApplyResources(this.pLC设置ToolStripMenuItem, "pLC设置ToolStripMenuItem");
            this.pLC设置ToolStripMenuItem.Click += new System.EventHandler(this.pLC设置ToolStripMenuItem_Click);
            // 
            // 相机设置ToolStripMenuItem
            // 
            this.相机设置ToolStripMenuItem.Name = "相机设置ToolStripMenuItem";
            resources.ApplyResources(this.相机设置ToolStripMenuItem, "相机设置ToolStripMenuItem");
            this.相机设置ToolStripMenuItem.Click += new System.EventHandler(this.相机设置ToolStripMenuItem_Click);
            // 
            // 机械手示教ToolStripMenuItem
            // 
            this.机械手示教ToolStripMenuItem.Name = "机械手示教ToolStripMenuItem";
            resources.ApplyResources(this.机械手示教ToolStripMenuItem, "机械手示教ToolStripMenuItem");
            this.机械手示教ToolStripMenuItem.Click += new System.EventHandler(this.机械手示教ToolStripMenuItem_Click);
            // 
            // 机械手控制ToolStripMenuItem
            // 
            this.机械手控制ToolStripMenuItem.Name = "机械手控制ToolStripMenuItem";
            resources.ApplyResources(this.机械手控制ToolStripMenuItem, "机械手控制ToolStripMenuItem");
            this.机械手控制ToolStripMenuItem.Click += new System.EventHandler(this.机械手控制ToolStripMenuItem_Click);
            // 
            // 模型设置ToolStripMenuItem
            // 
            this.模型设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.圆孔检测ToolStripMenuItem,
            this.字符条码二维码识别ToolStripMenuItem,
            this.形状匹配ToolStripMenuItem,
            this.像素匹配ToolStripMenuItem});
            this.模型设置ToolStripMenuItem.Name = "模型设置ToolStripMenuItem";
            resources.ApplyResources(this.模型设置ToolStripMenuItem, "模型设置ToolStripMenuItem");
            this.模型设置ToolStripMenuItem.Click += new System.EventHandler(this.模型设置ToolStripMenuItem_Click);
            // 
            // 圆孔检测ToolStripMenuItem
            // 
            this.圆孔检测ToolStripMenuItem.Name = "圆孔检测ToolStripMenuItem";
            resources.ApplyResources(this.圆孔检测ToolStripMenuItem, "圆孔检测ToolStripMenuItem");
            this.圆孔检测ToolStripMenuItem.Click += new System.EventHandler(this.圆孔检测ToolStripMenuItem_Click);
            // 
            // 字符条码二维码识别ToolStripMenuItem
            // 
            this.字符条码二维码识别ToolStripMenuItem.Name = "字符条码二维码识别ToolStripMenuItem";
            resources.ApplyResources(this.字符条码二维码识别ToolStripMenuItem, "字符条码二维码识别ToolStripMenuItem");
            this.字符条码二维码识别ToolStripMenuItem.Click += new System.EventHandler(this.字符条码二维码识别ToolStripMenuItem_Click);
            // 
            // 形状匹配ToolStripMenuItem
            // 
            this.形状匹配ToolStripMenuItem.Name = "形状匹配ToolStripMenuItem";
            resources.ApplyResources(this.形状匹配ToolStripMenuItem, "形状匹配ToolStripMenuItem");
            this.形状匹配ToolStripMenuItem.Click += new System.EventHandler(this.形状匹配ToolStripMenuItem_Click);
            // 
            // 像素匹配ToolStripMenuItem
            // 
            this.像素匹配ToolStripMenuItem.Name = "像素匹配ToolStripMenuItem";
            resources.ApplyResources(this.像素匹配ToolStripMenuItem, "像素匹配ToolStripMenuItem");
            this.像素匹配ToolStripMenuItem.Click += new System.EventHandler(this.像素匹配ToolStripMenuItem_Click);
            // 
            // 通讯设置ToolStripMenuItem
            // 
            this.通讯设置ToolStripMenuItem.Name = "通讯设置ToolStripMenuItem";
            resources.ApplyResources(this.通讯设置ToolStripMenuItem, "通讯设置ToolStripMenuItem");
            this.通讯设置ToolStripMenuItem.Click += new System.EventHandler(this.通讯设置ToolStripMenuItem_Click);
            // 
            // 语言设置ToolStripMenuItem
            // 
            this.语言设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.简体中文ToolStripMenuItem,
            this.英语ToolStripMenuItem,
            this.西班牙语ToolStripMenuItem});
            this.语言设置ToolStripMenuItem.Name = "语言设置ToolStripMenuItem";
            resources.ApplyResources(this.语言设置ToolStripMenuItem, "语言设置ToolStripMenuItem");
            // 
            // 简体中文ToolStripMenuItem
            // 
            this.简体中文ToolStripMenuItem.Name = "简体中文ToolStripMenuItem";
            resources.ApplyResources(this.简体中文ToolStripMenuItem, "简体中文ToolStripMenuItem");
            this.简体中文ToolStripMenuItem.Click += new System.EventHandler(this.简体中文ToolStripMenuItem_Click);
            // 
            // 英语ToolStripMenuItem
            // 
            this.英语ToolStripMenuItem.Name = "英语ToolStripMenuItem";
            resources.ApplyResources(this.英语ToolStripMenuItem, "英语ToolStripMenuItem");
            this.英语ToolStripMenuItem.Click += new System.EventHandler(this.英语ToolStripMenuItem_Click);
            // 
            // 西班牙语ToolStripMenuItem
            // 
            this.西班牙语ToolStripMenuItem.Name = "西班牙语ToolStripMenuItem";
            resources.ApplyResources(this.西班牙语ToolStripMenuItem, "西班牙语ToolStripMenuItem");
            this.西班牙语ToolStripMenuItem.Click += new System.EventHandler(this.西班牙语ToolStripMenuItem_Click);
            // 
            // 开发者选项ToolStripMenuItem
            // 
            this.开发者选项ToolStripMenuItem.Name = "开发者选项ToolStripMenuItem";
            resources.ApplyResources(this.开发者选项ToolStripMenuItem, "开发者选项ToolStripMenuItem");
            this.开发者选项ToolStripMenuItem.Click += new System.EventHandler(this.开发者选项ToolStripMenuItem_Click);
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建ToolStripMenuItem,
            this.打开ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            resources.ApplyResources(this.文件ToolStripMenuItem, "文件ToolStripMenuItem");
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            resources.ApplyResources(this.新建ToolStripMenuItem, "新建ToolStripMenuItem");
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            resources.ApplyResources(this.打开ToolStripMenuItem, "打开ToolStripMenuItem");
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            resources.ApplyResources(this.退出ToolStripMenuItem, "退出ToolStripMenuItem");
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            resources.ApplyResources(this.帮助ToolStripMenuItem, "帮助ToolStripMenuItem");
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            resources.ApplyResources(this.关于ToolStripMenuItem, "关于ToolStripMenuItem");
            // 
            // 登录ToolStripMenuItem
            // 
            this.登录ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.操作员登录ToolStripMenuItem,
            this.开发者登录ToolStripMenuItem});
            this.登录ToolStripMenuItem.Name = "登录ToolStripMenuItem";
            resources.ApplyResources(this.登录ToolStripMenuItem, "登录ToolStripMenuItem");
            // 
            // 操作员登录ToolStripMenuItem
            // 
            this.操作员登录ToolStripMenuItem.Name = "操作员登录ToolStripMenuItem";
            resources.ApplyResources(this.操作员登录ToolStripMenuItem, "操作员登录ToolStripMenuItem");
            this.操作员登录ToolStripMenuItem.Click += new System.EventHandler(this.操作员登录ToolStripMenuItem_Click);
            // 
            // 开发者登录ToolStripMenuItem
            // 
            this.开发者登录ToolStripMenuItem.Name = "开发者登录ToolStripMenuItem";
            resources.ApplyResources(this.开发者登录ToolStripMenuItem, "开发者登录ToolStripMenuItem");
            this.开发者登录ToolStripMenuItem.Click += new System.EventHandler(this.开发者登录ToolStripMenuItem_Click);
            // 
            // Bg_Main
            // 
            this.Bg_Main.WorkerReportsProgress = true;
            this.Bg_Main.WorkerSupportsCancellation = true;
            this.Bg_Main.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Bg_Main_DoWork);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button12);
            this.panel3.Controls.Add(this.btn_system_state);
            this.panel3.Controls.Add(this.panel1);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            this.helpProvider1.SetShowHelp(this.panel3, ((bool)(resources.GetObject("panel3.ShowHelp"))));
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.button12, "button12");
            this.button12.FlatAppearance.BorderSize = 0;
            this.button12.Name = "button12";
            this.helpProvider1.SetShowHelp(this.button12, ((bool)(resources.GetObject("button12.ShowHelp"))));
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btn_system_state
            // 
            resources.ApplyResources(this.btn_system_state, "btn_system_state");
            this.btn_system_state.BackColor = System.Drawing.Color.Transparent;
            this.btn_system_state.FlatAppearance.BorderColor = System.Drawing.Color.Chartreuse;
            this.btn_system_state.FlatAppearance.BorderSize = 5;
            this.btn_system_state.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_system_state.Name = "btn_system_state";
            this.helpProvider1.SetShowHelp(this.btn_system_state, ((bool)(resources.GetObject("btn_system_state.ShowHelp"))));
            this.btn_system_state.UseVisualStyleBackColor = false;
            this.btn_system_state.Click += new System.EventHandler(this.button26_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Menu;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.helpProvider1.SetShowHelp(this.panel1, ((bool)(resources.GetObject("panel1.ShowHelp"))));
            // 
            // bg_robot
            // 
            this.bg_robot.WorkerReportsProgress = true;
            this.bg_robot.WorkerSupportsCancellation = true;
            this.bg_robot.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bg_robot_DoWork);
            // 
            // Bg_PLC_heartbeat
            // 
            this.Bg_PLC_heartbeat.WorkerReportsProgress = true;
            this.Bg_PLC_heartbeat.WorkerSupportsCancellation = true;
            this.Bg_PLC_heartbeat.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Bg_PLC_heartbeat_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.数据.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage 数据;
        private HslCommunication.LogNet.LogNetAnalysisControl logNetAnalysisControl1;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pLC设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 相机设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 模型设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 通讯设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker Bg_Main;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.ToolStripMenuItem 语言设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 简体中文ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 英语ToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btn_system_state;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.ToolStripMenuItem 西班牙语ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开发者选项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作员登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开发者登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 机械手控制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 机械手示教ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bg_robot;
        private System.ComponentModel.BackgroundWorker Bg_PLC_heartbeat;
        private System.Windows.Forms.ToolStripMenuItem 圆孔检测ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 字符条码二维码识别ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 形状匹配ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 像素匹配ToolStripMenuItem;
    }
}

