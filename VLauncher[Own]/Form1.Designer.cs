namespace VLauncher_Own_
{
    partial class lform
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
            this.Launch_btn = new System.Windows.Forms.Button();
            this.Debug = new System.Windows.Forms.CheckBox();
            this.hasAccount = new System.Windows.Forms.CheckBox();
            this.username_tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Login_tb = new System.Windows.Forms.TextBox();
            this.Passwd_tb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.gamedir_tb = new System.Windows.Forms.TextBox();
            this.ChDir_btn = new System.Windows.Forms.Button();
            this.ram_track = new System.Windows.Forms.TrackBar();
            this.ram_label = new System.Windows.Forms.Label();
            this.myJava = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.ram_track)).BeginInit();
            this.SuspendLayout();
            // 
            // Launch_btn
            // 
            this.Launch_btn.Location = new System.Drawing.Point(12, 12);
            this.Launch_btn.Name = "Launch_btn";
            this.Launch_btn.Size = new System.Drawing.Size(116, 34);
            this.Launch_btn.TabIndex = 0;
            this.Launch_btn.Text = "Launch Minecraft";
            this.Launch_btn.UseVisualStyleBackColor = true;
            this.Launch_btn.Click += new System.EventHandler(this.LaunchBTN_Click);
            // 
            // Debug
            // 
            this.Debug.AutoSize = true;
            this.Debug.Location = new System.Drawing.Point(413, 22);
            this.Debug.Name = "Debug";
            this.Debug.Size = new System.Drawing.Size(85, 17);
            this.Debug.TabIndex = 2;
            this.Debug.Text = "DebugMode";
            this.Debug.UseVisualStyleBackColor = true;
            // 
            // hasAccount
            // 
            this.hasAccount.AutoSize = true;
            this.hasAccount.Location = new System.Drawing.Point(413, 46);
            this.hasAccount.Name = "hasAccount";
            this.hasAccount.Size = new System.Drawing.Size(77, 17);
            this.hasAccount.TabIndex = 3;
            this.hasAccount.Text = "LegitGame";
            this.hasAccount.UseVisualStyleBackColor = true;
            this.hasAccount.CheckedChanged += new System.EventHandler(this.hasAccount_CheckedChanged);
            // 
            // username_tb
            // 
            this.username_tb.Location = new System.Drawing.Point(289, 20);
            this.username_tb.MaxLength = 10;
            this.username_tb.Name = "username_tb";
            this.username_tb.Size = new System.Drawing.Size(100, 20);
            this.username_tb.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(228, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Username";
            // 
            // Login_tb
            // 
            this.Login_tb.Location = new System.Drawing.Point(289, 46);
            this.Login_tb.Name = "Login_tb";
            this.Login_tb.Size = new System.Drawing.Size(100, 20);
            this.Login_tb.TabIndex = 8;
            // 
            // Passwd_tb
            // 
            this.Passwd_tb.Location = new System.Drawing.Point(289, 72);
            this.Passwd_tb.Name = "Passwd_tb";
            this.Passwd_tb.Size = new System.Drawing.Size(100, 20);
            this.Passwd_tb.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(245, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Email";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(230, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-17, 510);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Password";
            // 
            // gamedir_tb
            // 
            this.gamedir_tb.Location = new System.Drawing.Point(157, 199);
            this.gamedir_tb.Name = "gamedir_tb";
            this.gamedir_tb.Size = new System.Drawing.Size(250, 20);
            this.gamedir_tb.TabIndex = 10;
            // 
            // ChDir_btn
            // 
            this.ChDir_btn.Location = new System.Drawing.Point(418, 196);
            this.ChDir_btn.Name = "ChDir_btn";
            this.ChDir_btn.Size = new System.Drawing.Size(75, 23);
            this.ChDir_btn.TabIndex = 11;
            this.ChDir_btn.Text = "game dir";
            this.ChDir_btn.UseVisualStyleBackColor = true;
            this.ChDir_btn.Click += new System.EventHandler(this.ChDir_btn_Click);
            // 
            // ram_track
            // 
            this.ram_track.Location = new System.Drawing.Point(289, 286);
            this.ram_track.Name = "ram_track";
            this.ram_track.Size = new System.Drawing.Size(166, 45);
            this.ram_track.TabIndex = 12;
            this.ram_track.TickFrequency = 256;
            this.ram_track.Scroll += new System.EventHandler(this.ram_track_Scroll);
            // 
            // ram_label
            // 
            this.ram_label.AutoSize = true;
            this.ram_label.Location = new System.Drawing.Point(462, 295);
            this.ram_label.Name = "ram_label";
            this.ram_label.Size = new System.Drawing.Size(35, 13);
            this.ram_label.TabIndex = 13;
            this.ram_label.Text = "HELP";
            // 
            // myJava
            // 
            this.myJava.AutoSize = true;
            this.myJava.Location = new System.Drawing.Point(413, 70);
            this.myJava.Name = "myJava";
            this.myJava.Size = new System.Drawing.Size(82, 17);
            this.myJava.TabIndex = 14;
            this.myJava.Text = "build-in java";
            this.myJava.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox1.Items.AddRange(new object[] {
            "ModPack1",
            "ModPack_2",
            "Something else"});
            this.comboBox1.Location = new System.Drawing.Point(12, 286);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 15;
            this.comboBox1.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 317);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(478, 13);
            this.progressBar1.TabIndex = 16;
            this.progressBar1.Tag = "";
            this.progressBar1.Value = 35;
            this.progressBar1.Visible = false;
            // 
            // lform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 342);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.myJava);
            this.Controls.Add(this.ram_label);
            this.Controls.Add(this.ram_track);
            this.Controls.Add(this.ChDir_btn);
            this.Controls.Add(this.gamedir_tb);
            this.Controls.Add(this.Passwd_tb);
            this.Controls.Add(this.Login_tb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.username_tb);
            this.Controls.Add(this.hasAccount);
            this.Controls.Add(this.Debug);
            this.Controls.Add(this.Launch_btn);
            this.Name = "lform";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ram_track)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Launch_btn;
        private System.Windows.Forms.CheckBox Debug;
        private System.Windows.Forms.CheckBox hasAccount;
        private System.Windows.Forms.TextBox username_tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Login_tb;
        private System.Windows.Forms.TextBox Passwd_tb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox gamedir_tb;
        private System.Windows.Forms.Button ChDir_btn;
        private System.Windows.Forms.TrackBar ram_track;
        private System.Windows.Forms.Label ram_label;
        private System.Windows.Forms.CheckBox myJava;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

