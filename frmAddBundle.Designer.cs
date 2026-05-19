namespace ninelivesbooks
{
    partial class frmAddBundle
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblBundleStatus = new System.Windows.Forms.Label();
            this.lblBundleTheme = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.txtTheme = new System.Windows.Forms.TextBox();
            this.txtBundleName = new System.Windows.Forms.TextBox();
            this.panelDesc = new System.Windows.Forms.Panel();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddImg = new System.Windows.Forms.Button();
            this.btnClearImg = new System.Windows.Forms.Button();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lstBook = new System.Windows.Forms.ListView();
            this.lstBookBundle = new System.Windows.Forms.ListView();
            this.BtnSaveBundle = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            this.panelDesc.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(245)))), ((int)(((byte)(239)))));
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblBundleStatus);
            this.panel1.Controls.Add(this.lblBundleTheme);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtWeight);
            this.panel1.Controls.Add(this.numPrice);
            this.panel1.Controls.Add(this.cbStatus);
            this.panel1.Controls.Add(this.txtTheme);
            this.panel1.Controls.Add(this.txtBundleName);
            this.panel1.Controls.Add(this.panelDesc);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1584, 349);
            this.panel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.label6.Location = new System.Drawing.Point(891, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Weight";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.label3.Location = new System.Drawing.Point(700, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "Price";
            // 
            // lblBundleStatus
            // 
            this.lblBundleStatus.AutoSize = true;
            this.lblBundleStatus.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblBundleStatus.Location = new System.Drawing.Point(431, 123);
            this.lblBundleStatus.Name = "lblBundleStatus";
            this.lblBundleStatus.Size = new System.Drawing.Size(45, 16);
            this.lblBundleStatus.TabIndex = 13;
            this.lblBundleStatus.Text = "Status";
            // 
            // lblBundleTheme
            // 
            this.lblBundleTheme.AutoSize = true;
            this.lblBundleTheme.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lblBundleTheme.Location = new System.Drawing.Point(63, 123);
            this.lblBundleTheme.Name = "lblBundleTheme";
            this.lblBundleTheme.Size = new System.Drawing.Size(49, 16);
            this.lblBundleTheme.TabIndex = 12;
            this.lblBundleTheme.Text = "Theme";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.label2.Location = new System.Drawing.Point(63, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Bundle Name";
            // 
            // txtWeight
            // 
            this.txtWeight.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.txtWeight.Location = new System.Drawing.Point(894, 102);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.ReadOnly = true;
            this.txtWeight.Size = new System.Drawing.Size(109, 22);
            this.txtWeight.TabIndex = 10;
            this.txtWeight.Text = "0.0 g";
            this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 2;
            this.numPrice.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.numPrice.Location = new System.Drawing.Point(703, 100);
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(120, 22);
            this.numPrice.TabIndex = 9;
            this.numPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbStatus
            // 
            this.cbStatus.AllowDrop = true;
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "AVAILABLE",
            "SOLD",
            "UNAVAILABLE"});
            this.cbStatus.Location = new System.Drawing.Point(434, 138);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(121, 24);
            this.cbStatus.TabIndex = 8;
            // 
            // txtTheme
            // 
            this.txtTheme.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.txtTheme.Location = new System.Drawing.Point(66, 140);
            this.txtTheme.Name = "txtTheme";
            this.txtTheme.Size = new System.Drawing.Size(288, 22);
            this.txtTheme.TabIndex = 7;
            // 
            // txtBundleName
            // 
            this.txtBundleName.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.txtBundleName.Location = new System.Drawing.Point(66, 79);
            this.txtBundleName.Name = "txtBundleName";
            this.txtBundleName.Size = new System.Drawing.Size(489, 22);
            this.txtBundleName.TabIndex = 5;
            // 
            // panelDesc
            // 
            this.panelDesc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(245)))), ((int)(((byte)(239)))));
            this.panelDesc.Controls.Add(this.txtDescription);
            this.panelDesc.Controls.Add(this.panel8);
            this.panelDesc.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelDesc.Location = new System.Drawing.Point(67, 191);
            this.panelDesc.Name = "panelDesc";
            this.panelDesc.Size = new System.Drawing.Size(489, 122);
            this.panelDesc.TabIndex = 4;
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(0, 33);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(489, 89);
            this.txtDescription.TabIndex = 17;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(70)))), ((int)(((byte)(50)))));
            this.panel8.Controls.Add(this.label4);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(489, 33);
            this.panel8.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.label4.Location = new System.Drawing.Point(200, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "Description";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnAddImg);
            this.panel2.Controls.Add(this.btnClearImg);
            this.panel2.Controls.Add(this.picImage);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(1199, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(339, 349);
            this.panel2.TabIndex = 3;
            // 
            // btnAddImg
            // 
            this.btnAddImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnAddImg.FlatAppearance.BorderSize = 0;
            this.btnAddImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddImg.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnAddImg.Location = new System.Drawing.Point(36, 282);
            this.btnAddImg.Name = "btnAddImg";
            this.btnAddImg.Size = new System.Drawing.Size(117, 48);
            this.btnAddImg.TabIndex = 6;
            this.btnAddImg.Text = "Add Image";
            this.btnAddImg.UseVisualStyleBackColor = false;
            this.btnAddImg.Click += new System.EventHandler(this.btnAddImg_Click);
            // 
            // btnClearImg
            // 
            this.btnClearImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnClearImg.FlatAppearance.BorderSize = 0;
            this.btnClearImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearImg.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearImg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnClearImg.Location = new System.Drawing.Point(190, 282);
            this.btnClearImg.Name = "btnClearImg";
            this.btnClearImg.Size = new System.Drawing.Size(117, 48);
            this.btnClearImg.TabIndex = 7;
            this.btnClearImg.Text = "Remove";
            this.btnClearImg.UseVisualStyleBackColor = false;
            this.btnClearImg.Click += new System.EventHandler(this.btnClearImg_Click);
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(62, 59);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(219, 206);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 1;
            this.picImage.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(70)))), ((int)(((byte)(50)))));
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(339, 53);
            this.panel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Georgia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.label1.Location = new System.Drawing.Point(115, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bundle Cover";
            // 
            // lstBook
            // 
            this.lstBook.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lstBook.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.lstBook.HideSelection = false;
            this.lstBook.Location = new System.Drawing.Point(6, 397);
            this.lstBook.Name = "lstBook";
            this.lstBook.Size = new System.Drawing.Size(685, 414);
            this.lstBook.TabIndex = 1;
            this.lstBook.UseCompatibleStateImageBehavior = false;
            // 
            // lstBookBundle
            // 
            this.lstBookBundle.Font = new System.Drawing.Font("Georgia", 9.75F);
            this.lstBookBundle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.lstBookBundle.HideSelection = false;
            this.lstBookBundle.Location = new System.Drawing.Point(882, 397);
            this.lstBookBundle.Name = "lstBookBundle";
            this.lstBookBundle.Size = new System.Drawing.Size(535, 414);
            this.lstBookBundle.TabIndex = 2;
            this.lstBookBundle.UseCompatibleStateImageBehavior = false;
            // 
            // BtnSaveBundle
            // 
            this.BtnSaveBundle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.BtnSaveBundle.FlatAppearance.BorderSize = 0;
            this.BtnSaveBundle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSaveBundle.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSaveBundle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.BtnSaveBundle.Location = new System.Drawing.Point(1434, 744);
            this.BtnSaveBundle.Name = "BtnSaveBundle";
            this.BtnSaveBundle.Size = new System.Drawing.Size(141, 57);
            this.BtnSaveBundle.TabIndex = 7;
            this.BtnSaveBundle.Text = "Save Bundle";
            this.BtnSaveBundle.UseVisualStyleBackColor = false;
            this.BtnSaveBundle.Click += new System.EventHandler(this.BtnSaveBundle_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnClear.Location = new System.Drawing.Point(1434, 680);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(141, 48);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnCancel.Location = new System.Drawing.Point(1434, 626);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 48);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnAdd.Location = new System.Drawing.Point(730, 431);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(117, 48);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnRemove.FlatAppearance.BorderSize = 0;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemove.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnRemove.Location = new System.Drawing.Point(730, 501);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(117, 48);
            this.btnRemove.TabIndex = 11;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(167)))), ((int)(((byte)(86)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.btnSearch.Location = new System.Drawing.Point(600, 367);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(91, 24);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Georgia", 10.75F);
            this.txtSearch.Location = new System.Drawing.Point(6, 367);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(588, 24);
            this.txtSearch.TabIndex = 13;
            // 
            // frmAddBundle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.BtnSaveBundle);
            this.Controls.Add(this.lstBookBundle);
            this.Controls.Add(this.lstBook);
            this.Controls.Add(this.panel1);
            this.Name = "frmAddBundle";
            this.Text = "Add Bundle";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            this.panelDesc.ResumeLayout(false);
            this.panelDesc.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAddImg;
        private System.Windows.Forms.Button btnClearImg;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelDesc;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lstBook;
        private System.Windows.Forms.ListView lstBookBundle;
        private System.Windows.Forms.Button BtnSaveBundle;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown numPrice;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.TextBox txtTheme;
        private System.Windows.Forms.TextBox txtBundleName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblBundleStatus;
        private System.Windows.Forms.Label lblBundleTheme;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
    }
}