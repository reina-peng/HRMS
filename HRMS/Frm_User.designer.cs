﻿
namespace HRMS
{
    partial class Frm_User
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_User));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtAdd = new System.Windows.Forms.TextBox();
            this.txtEmergencyP = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.txtEnName = new System.Windows.Forms.TextBox();
            this.comSex = new System.Windows.Forms.ComboBox();
            this.DTBir = new System.Windows.Forms.DateTimePicker();
            this.DTOBD = new System.Windows.Forms.DateTimePicker();
            this.DTBBD = new System.Windows.Forms.DateTimePicker();
            this.txtEmergencyC = new System.Windows.Forms.TextBox();
            this.基本資料１ = new System.Windows.Forms.GroupBox();
            this.labAccS = new System.Windows.Forms.Label();
            this.labOBS = new System.Windows.Forms.Label();
            this.labSup = new System.Windows.Forms.Label();
            this.labJobTitle = new System.Windows.Forms.Label();
            this.labDept = new System.Windows.Forms.Label();
            this.labName = new System.Windows.Forms.Label();
            this.labID = new System.Windows.Forms.Label();
            this.帳號狀態 = new System.Windows.Forms.Label();
            this.到職日 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.基本資料１.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(197, 189);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtEmail.Location = new System.Drawing.Point(342, 125);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(156, 30);
            this.txtEmail.TabIndex = 1;
            // 
            // txtAdd
            // 
            this.txtAdd.Location = new System.Drawing.Point(150, 145);
            this.txtAdd.Name = "txtAdd";
            this.txtAdd.Size = new System.Drawing.Size(492, 25);
            this.txtAdd.TabIndex = 4;
            // 
            // txtEmergencyP
            // 
            this.txtEmergencyP.Location = new System.Drawing.Point(150, 186);
            this.txtEmergencyP.Name = "txtEmergencyP";
            this.txtEmergencyP.Size = new System.Drawing.Size(148, 25);
            this.txtEmergencyP.TabIndex = 6;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(150, 104);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(148, 25);
            this.txtPhone.TabIndex = 5;
            // 
            // txtPassWord
            // 
            this.txtPassWord.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtPassWord.Location = new System.Drawing.Point(102, 55);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.Size = new System.Drawing.Size(121, 30);
            this.txtPassWord.TabIndex = 10;
            // 
            // txtEnName
            // 
            this.txtEnName.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtEnName.Location = new System.Drawing.Point(102, 125);
            this.txtEnName.Name = "txtEnName";
            this.txtEnName.Size = new System.Drawing.Size(121, 30);
            this.txtEnName.TabIndex = 9;
            // 
            // comSex
            // 
            this.comSex.FormattingEnabled = true;
            this.comSex.Items.AddRange(new object[] {
            "男",
            "女"});
            this.comSex.Location = new System.Drawing.Point(150, 64);
            this.comSex.Name = "comSex";
            this.comSex.Size = new System.Drawing.Size(148, 23);
            this.comSex.TabIndex = 15;
            // 
            // DTBir
            // 
            this.DTBir.Location = new System.Drawing.Point(150, 22);
            this.DTBir.Name = "DTBir";
            this.DTBir.Size = new System.Drawing.Size(148, 25);
            this.DTBir.TabIndex = 19;
            // 
            // DTOBD
            // 
            this.DTOBD.Enabled = false;
            this.DTOBD.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.DTOBD.Location = new System.Drawing.Point(601, 55);
            this.DTOBD.Name = "DTOBD";
            this.DTOBD.Size = new System.Drawing.Size(172, 30);
            this.DTOBD.TabIndex = 20;
            // 
            // DTBBD
            // 
            this.DTBBD.Enabled = false;
            this.DTBBD.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.DTBBD.Location = new System.Drawing.Point(601, 125);
            this.DTBBD.Name = "DTBBD";
            this.DTBBD.Size = new System.Drawing.Size(172, 30);
            this.DTBBD.TabIndex = 21;
            // 
            // txtEmergencyC
            // 
            this.txtEmergencyC.Location = new System.Drawing.Point(150, 227);
            this.txtEmergencyC.Name = "txtEmergencyC";
            this.txtEmergencyC.Size = new System.Drawing.Size(148, 25);
            this.txtEmergencyC.TabIndex = 22;
            // 
            // 基本資料１
            // 
            this.基本資料１.BackColor = System.Drawing.Color.SandyBrown;
            this.基本資料１.Controls.Add(this.labAccS);
            this.基本資料１.Controls.Add(this.labOBS);
            this.基本資料１.Controls.Add(this.labSup);
            this.基本資料１.Controls.Add(this.labJobTitle);
            this.基本資料１.Controls.Add(this.labDept);
            this.基本資料１.Controls.Add(this.labName);
            this.基本資料１.Controls.Add(this.labID);
            this.基本資料１.Controls.Add(this.帳號狀態);
            this.基本資料１.Controls.Add(this.到職日);
            this.基本資料１.Controls.Add(this.label11);
            this.基本資料１.Controls.Add(this.label5);
            this.基本資料１.Controls.Add(this.label6);
            this.基本資料１.Controls.Add(this.label14);
            this.基本資料１.Controls.Add(this.label7);
            this.基本資料１.Controls.Add(this.DTOBD);
            this.基本資料１.Controls.Add(this.label8);
            this.基本資料１.Controls.Add(this.label4);
            this.基本資料１.Controls.Add(this.label3);
            this.基本資料１.Controls.Add(this.label2);
            this.基本資料１.Controls.Add(this.DTBBD);
            this.基本資料１.Controls.Add(this.label1);
            this.基本資料１.Controls.Add(this.txtEmail);
            this.基本資料１.Controls.Add(this.txtEnName);
            this.基本資料１.Controls.Add(this.txtPassWord);
            this.基本資料１.Location = new System.Drawing.Point(215, 24);
            this.基本資料１.Name = "基本資料１";
            this.基本資料１.Size = new System.Drawing.Size(791, 189);
            this.基本資料１.TabIndex = 23;
            this.基本資料１.TabStop = false;
            this.基本資料１.Text = "員工資料";
            // 
            // labAccS
            // 
            this.labAccS.AutoSize = true;
            this.labAccS.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labAccS.Location = new System.Drawing.Point(608, 95);
            this.labAccS.Name = "labAccS";
            this.labAccS.Size = new System.Drawing.Size(78, 22);
            this.labAccS.TabIndex = 43;
            this.labAccS.Text = "帳號狀態";
            // 
            // labOBS
            // 
            this.labOBS.AutoSize = true;
            this.labOBS.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labOBS.Location = new System.Drawing.Point(608, 25);
            this.labOBS.Name = "labOBS";
            this.labOBS.Size = new System.Drawing.Size(78, 22);
            this.labOBS.TabIndex = 42;
            this.labOBS.Text = "在職狀態";
            // 
            // labSup
            // 
            this.labSup.AutoSize = true;
            this.labSup.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labSup.Location = new System.Drawing.Point(344, 95);
            this.labSup.Name = "labSup";
            this.labSup.Size = new System.Drawing.Size(44, 22);
            this.labSup.TabIndex = 41;
            this.labSup.Text = "主管";
            // 
            // labJobTitle
            // 
            this.labJobTitle.AutoSize = true;
            this.labJobTitle.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labJobTitle.Location = new System.Drawing.Point(344, 62);
            this.labJobTitle.Name = "labJobTitle";
            this.labJobTitle.Size = new System.Drawing.Size(44, 22);
            this.labJobTitle.TabIndex = 40;
            this.labJobTitle.Text = "職位";
            // 
            // labDept
            // 
            this.labDept.AutoSize = true;
            this.labDept.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labDept.Location = new System.Drawing.Point(344, 25);
            this.labDept.Name = "labDept";
            this.labDept.Size = new System.Drawing.Size(44, 22);
            this.labDept.TabIndex = 39;
            this.labDept.Text = "部門";
            // 
            // labName
            // 
            this.labName.AutoSize = true;
            this.labName.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labName.Location = new System.Drawing.Point(99, 95);
            this.labName.Name = "labName";
            this.labName.Size = new System.Drawing.Size(44, 22);
            this.labName.TabIndex = 38;
            this.labName.Text = "姓名";
            // 
            // labID
            // 
            this.labID.AutoSize = true;
            this.labID.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labID.Location = new System.Drawing.Point(99, 24);
            this.labID.Name = "labID";
            this.labID.Size = new System.Drawing.Size(78, 22);
            this.labID.TabIndex = 37;
            this.labID.Text = "員工編號";
            // 
            // 帳號狀態
            // 
            this.帳號狀態.AutoSize = true;
            this.帳號狀態.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.帳號狀態.Location = new System.Drawing.Point(523, 95);
            this.帳號狀態.Name = "帳號狀態";
            this.帳號狀態.Size = new System.Drawing.Size(78, 22);
            this.帳號狀態.TabIndex = 33;
            this.帳號狀態.Text = "帳號狀態";
            // 
            // 到職日
            // 
            this.到職日.AutoSize = true;
            this.到職日.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.到職日.Location = new System.Drawing.Point(523, 60);
            this.到職日.Name = "到職日";
            this.到職日.Size = new System.Drawing.Size(61, 22);
            this.到職日.TabIndex = 32;
            this.到職日.Text = "到職日";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(523, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(78, 22);
            this.label11.TabIndex = 31;
            this.label11.Text = "在職狀態";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(254, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 22);
            this.label5.TabIndex = 29;
            this.label5.Text = "公司信箱";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(254, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 22);
            this.label6.TabIndex = 28;
            this.label6.Text = "主管";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(523, 130);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 22);
            this.label14.TabIndex = 36;
            this.label14.Text = "離職日";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(254, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 22);
            this.label7.TabIndex = 27;
            this.label7.Text = "職位";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(254, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 22);
            this.label8.TabIndex = 26;
            this.label8.Text = "部門";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(8, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 22);
            this.label4.TabIndex = 25;
            this.label4.Text = "密碼";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(8, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 28);
            this.label3.TabIndex = 24;
            this.label3.Text = "英文名稱";
            this.label3.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(8, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 22);
            this.label2.TabIndex = 23;
            this.label2.Text = "姓名";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 22);
            this.label1.TabIndex = 22;
            this.label1.Text = "員工編號";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.SandyBrown;
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtEmergencyC);
            this.groupBox2.Controls.Add(this.txtAdd);
            this.groupBox2.Controls.Add(this.txtPhone);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.txtEmergencyP);
            this.groupBox2.Controls.Add(this.comSex);
            this.groupBox2.Controls.Add(this.DTBir);
            this.groupBox2.Location = new System.Drawing.Point(12, 233);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(994, 277);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "個人基本資料";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(901, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 42);
            this.button1.TabIndex = 40;
            this.button1.Text = "修改";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(38, 232);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(97, 15);
            this.label17.TabIndex = 39;
            this.label17.Text = "緊急連絡電話";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(38, 191);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(82, 15);
            this.label18.TabIndex = 38;
            this.label18.Text = "緊急連絡人";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(38, 150);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(37, 15);
            this.label13.TabIndex = 37;
            this.label13.Text = "住址";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(38, 109);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 15);
            this.label12.TabIndex = 30;
            this.label12.Text = "連絡電話";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(38, 68);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(37, 15);
            this.label15.TabIndex = 35;
            this.label15.Text = "性別";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(38, 27);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(37, 15);
            this.label16.TabIndex = 34;
            this.label16.Text = "生日";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Frm_User
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.NavajoWhite;
            this.ClientSize = new System.Drawing.Size(1018, 529);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.基本資料１);
            this.Name = "Frm_User";
            this.Text = "Frm_User";
            this.Load += new System.EventHandler(this.Frm_User_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.基本資料１.ResumeLayout(false);
            this.基本資料１.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtAdd;
        private System.Windows.Forms.TextBox txtEmergencyP;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtPassWord;
        private System.Windows.Forms.TextBox txtEnName;
        private System.Windows.Forms.ComboBox comSex;
        private System.Windows.Forms.DateTimePicker DTBir;
        private System.Windows.Forms.DateTimePicker DTOBD;
        private System.Windows.Forms.DateTimePicker DTBBD;
        private System.Windows.Forms.TextBox txtEmergencyC;
        private System.Windows.Forms.GroupBox 基本資料１;
        private System.Windows.Forms.Label 帳號狀態;
        private System.Windows.Forms.Label 到職日;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labName;
        private System.Windows.Forms.Label labAccS;
        private System.Windows.Forms.Label labOBS;
        private System.Windows.Forms.Label labSup;
        private System.Windows.Forms.Label labJobTitle;
        private System.Windows.Forms.Label labDept;
        internal System.Windows.Forms.Label labID;
    }
}