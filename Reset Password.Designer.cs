namespace ninelivesbooks
{
    partial class frmResetPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResetPassword));
            this.lblCode = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblEnterEmail = new System.Windows.Forms.Label();
            this.lblTextConfirmation = new System.Windows.Forms.Label();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.lblCodeText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnValidarCodigo = new System.Windows.Forms.Button();
            this.btnEnter = new System.Windows.Forms.Button();
            this.txtConfirmNewPassword = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnQuit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.lblCode.Location = new System.Drawing.Point(250, 174);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(0, 16);
            this.lblCode.TabIndex = 0;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.lblPassword.Location = new System.Drawing.Point(331, 254);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(96, 16);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "New Password";
            this.lblPassword.Visible = false;
            // 
            // lblEnterEmail
            // 
            this.lblEnterEmail.AutoSize = true;
            this.lblEnterEmail.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblEnterEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.lblEnterEmail.Location = new System.Drawing.Point(249, 38);
            this.lblEnterEmail.Name = "lblEnterEmail";
            this.lblEnterEmail.Size = new System.Drawing.Size(195, 16);
            this.lblEnterEmail.TabIndex = 2;
            this.lblEnterEmail.Text = "Enter your Email or Username";
            // 
            // lblTextConfirmation
            // 
            this.lblTextConfirmation.AutoSize = true;
            this.lblTextConfirmation.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblTextConfirmation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.lblTextConfirmation.Location = new System.Drawing.Point(250, 82);
            this.lblTextConfirmation.Name = "lblTextConfirmation";
            this.lblTextConfirmation.Size = new System.Drawing.Size(0, 16);
            this.lblTextConfirmation.TabIndex = 3;
            // 
            // lblConfirm
            // 
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.lblConfirm.Location = new System.Drawing.Point(331, 311);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(180, 16);
            this.lblConfirm.TabIndex = 4;
            this.lblConfirm.Text = "Confirm your new password";
            this.lblConfirm.Visible = false;
            // 
            // lblConfirmPassword
            // 
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblConfirmPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.lblConfirmPassword.Location = new System.Drawing.Point(331, 366);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(0, 16);
            this.lblConfirmPassword.TabIndex = 5;
            // 
            // lblCodeText
            // 
            this.lblCodeText.AutoSize = true;
            this.lblCodeText.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblCodeText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.lblCodeText.Location = new System.Drawing.Point(249, 131);
            this.lblCodeText.Name = "lblCodeText";
            this.lblCodeText.Size = new System.Drawing.Size(225, 16);
            this.lblCodeText.TabIndex = 6;
            this.lblCodeText.Text = "Enter the code was receive in Email";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblConfirmPassword);
            this.panel1.Controls.Add(this.lblCodeText);
            this.panel1.Controls.Add(this.lblConfirm);
            this.panel1.Controls.Add(this.btnRegister);
            this.panel1.Controls.Add(this.lblPassword);
            this.panel1.Controls.Add(this.lblTextConfirmation);
            this.panel1.Controls.Add(this.btnValidarCodigo);
            this.panel1.Controls.Add(this.btnEnter);
            this.panel1.Controls.Add(this.lblCode);
            this.panel1.Controls.Add(this.txtConfirmNewPassword);
            this.panel1.Controls.Add(this.lblEnterEmail);
            this.panel1.Controls.Add(this.txtNewPassword);
            this.panel1.Controls.Add(this.txtCode);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(319, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 592);
            this.panel1.TabIndex = 7;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnRegister.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnRegister.Location = new System.Drawing.Point(411, 418);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(85, 31);
            this.btnRegister.TabIndex = 7;
            this.btnRegister.Text = "Enter";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Visible = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnValidarCodigo
            // 
            this.btnValidarCodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnValidarCodigo.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnValidarCodigo.Location = new System.Drawing.Point(617, 140);
            this.btnValidarCodigo.Name = "btnValidarCodigo";
            this.btnValidarCodigo.Size = new System.Drawing.Size(85, 31);
            this.btnValidarCodigo.TabIndex = 6;
            this.btnValidarCodigo.Text = "Enter";
            this.btnValidarCodigo.UseVisualStyleBackColor = false;
            this.btnValidarCodigo.Click += new System.EventHandler(this.btnValidarCodigo_Click);
            // 
            // btnEnter
            // 
            this.btnEnter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnEnter.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnEnter.Location = new System.Drawing.Point(617, 48);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(85, 31);
            this.btnEnter.TabIndex = 5;
            this.btnEnter.Text = "Enter";
            this.btnEnter.UseVisualStyleBackColor = false;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // txtConfirmNewPassword
            // 
            this.txtConfirmNewPassword.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.txtConfirmNewPassword.Location = new System.Drawing.Point(334, 273);
            this.txtConfirmNewPassword.Name = "txtConfirmNewPassword";
            this.txtConfirmNewPassword.Size = new System.Drawing.Size(227, 22);
            this.txtConfirmNewPassword.TabIndex = 4;
            this.txtConfirmNewPassword.UseSystemPasswordChar = true;
            this.txtConfirmNewPassword.Visible = false;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.txtNewPassword.Location = new System.Drawing.Point(334, 330);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(227, 22);
            this.txtNewPassword.TabIndex = 3;
            this.txtNewPassword.UseSystemPasswordChar = true;
            this.txtNewPassword.Visible = false;
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.txtCode.Location = new System.Drawing.Point(252, 149);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(309, 22);
            this.txtCode.TabIndex = 2;
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.txtEmail.Location = new System.Drawing.Point(251, 57);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(310, 22);
            this.txtEmail.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(51, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(174, 158);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnQuit.FlatAppearance.BorderSize = 0;
            this.btnQuit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnQuit.Image = ((System.Drawing.Image)(resources.GetObject("btnQuit.Image")));
            this.btnQuit.Location = new System.Drawing.Point(12, 12);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Padding = new System.Windows.Forms.Padding(5);
            this.btnQuit.Size = new System.Drawing.Size(26, 26);
            this.btnQuit.TabIndex = 8;
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // frmResetPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(70)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(1584, 731);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.panel1);
            this.Name = "frmResetPassword";
            this.Text = "Reset Password";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblEnterEmail;
        private System.Windows.Forms.Label lblTextConfirmation;
        private System.Windows.Forms.Label lblConfirm;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.Label lblCodeText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtConfirmNewPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnValidarCodigo;
        private System.Windows.Forms.Button btnQuit;
    }
}