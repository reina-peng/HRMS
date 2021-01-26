
namespace HRMS
{
    partial class Login
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblAccount = new System.Windows.Forms.Label();
            this.gpbLogin = new System.Windows.Forms.GroupBox();
            this.gpbOnboard = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOnboardPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOnboardAccount = new System.Windows.Forms.TextBox();
            this.btnOnboard = new System.Windows.Forms.Button();
            this.gpbLogin.SuspendLayout();
            this.gpbOnboard.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(74, 151);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "登入";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(80, 78);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(69, 22);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "1111";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(694, 403);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "離開";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtAccount
            // 
            this.txtAccount.Location = new System.Drawing.Point(80, 34);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(69, 22);
            this.txtAccount.TabIndex = 3;
            this.txtAccount.Text = "achieshen";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(24, 81);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(29, 12);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "密碼";
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Location = new System.Drawing.Point(24, 37);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(29, 12);
            this.lblAccount.TabIndex = 5;
            this.lblAccount.Text = "帳號";
            // 
            // gpbLogin
            // 
            this.gpbLogin.Controls.Add(this.btnLogin);
            this.gpbLogin.Controls.Add(this.lblAccount);
            this.gpbLogin.Controls.Add(this.txtPassword);
            this.gpbLogin.Controls.Add(this.lblPassword);
            this.gpbLogin.Controls.Add(this.txtAccount);
            this.gpbLogin.Location = new System.Drawing.Point(137, 86);
            this.gpbLogin.Name = "gpbLogin";
            this.gpbLogin.Size = new System.Drawing.Size(207, 265);
            this.gpbLogin.TabIndex = 6;
            this.gpbLogin.TabStop = false;
            this.gpbLogin.Text = "登入";
            // 
            // gpbOnboard
            // 
            this.gpbOnboard.Controls.Add(this.btnOnboard);
            this.gpbOnboard.Controls.Add(this.label1);
            this.gpbOnboard.Controls.Add(this.txtOnboardAccount);
            this.gpbOnboard.Controls.Add(this.txtOnboardPassword);
            this.gpbOnboard.Controls.Add(this.label2);
            this.gpbOnboard.Location = new System.Drawing.Point(449, 86);
            this.gpbOnboard.Name = "gpbOnboard";
            this.gpbOnboard.Size = new System.Drawing.Size(203, 279);
            this.gpbOnboard.TabIndex = 7;
            this.gpbOnboard.TabStop = false;
            this.gpbOnboard.Text = "新人報到";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "帳號";
            // 
            // txtOnboardPassword
            // 
            this.txtOnboardPassword.Location = new System.Drawing.Point(86, 93);
            this.txtOnboardPassword.Name = "txtOnboardPassword";
            this.txtOnboardPassword.Size = new System.Drawing.Size(69, 22);
            this.txtOnboardPassword.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "密碼";
            // 
            // txtOnboardAccount
            // 
            this.txtOnboardAccount.Location = new System.Drawing.Point(86, 49);
            this.txtOnboardAccount.Name = "txtOnboardAccount";
            this.txtOnboardAccount.Size = new System.Drawing.Size(69, 22);
            this.txtOnboardAccount.TabIndex = 7;
            // 
            // btnOnboard
            // 
            this.btnOnboard.Location = new System.Drawing.Point(65, 172);
            this.btnOnboard.Name = "btnOnboard";
            this.btnOnboard.Size = new System.Drawing.Size(75, 23);
            this.btnOnboard.TabIndex = 6;
            this.btnOnboard.Text = "報到";
            this.btnOnboard.UseVisualStyleBackColor = true;
            this.btnOnboard.Click += new System.EventHandler(this.btnOnboard_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gpbOnboard);
            this.Controls.Add(this.gpbLogin);
            this.Controls.Add(this.btnExit);
            this.Name = "Login";
            this.Text = "Form1";
            this.gpbLogin.ResumeLayout(false);
            this.gpbLogin.PerformLayout();
            this.gpbOnboard.ResumeLayout(false);
            this.gpbOnboard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.GroupBox gpbLogin;
        private System.Windows.Forms.GroupBox gpbOnboard;
        private System.Windows.Forms.Button btnOnboard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOnboardAccount;
        private System.Windows.Forms.TextBox txtOnboardPassword;
        private System.Windows.Forms.Label label2;
    }
}

