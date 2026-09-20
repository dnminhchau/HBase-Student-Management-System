namespace HBaseStudentApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            dgvStudents = new DataGridView();
            txtStudentID = new TextBox();
            txtName = new TextBox();
            txtAge = new TextBox();
            txtEmail = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            pictureBox1 = new PictureBox();
            btnExport = new Button();
            btnImport = new Button();
            btnDownloadTemplate = new Button();
            button2 = new Button();
            btnDeleteAll = new Button();
            pbLoading = new ProgressBar();
            lblTime = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvStudents).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dgvStudents
            // 
            dgvStudents.BackgroundColor = Color.Snow;
            dgvStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudents.Location = new Point(562, 41);
            dgvStudents.Name = "dgvStudents";
            dgvStudents.RowHeadersWidth = 51;
            dgvStudents.Size = new Size(636, 391);
            dgvStudents.TabIndex = 0;
            dgvStudents.CellClick += dgvStudents_CellClick;
            // 
            // txtStudentID
            // 
            txtStudentID.Location = new Point(150, 109);
            txtStudentID.Name = "txtStudentID";
            txtStudentID.Size = new Size(389, 27);
            txtStudentID.TabIndex = 2;
            // 
            // txtName
            // 
            txtName.Location = new Point(150, 157);
            txtName.Name = "txtName";
            txtName.Size = new Size(389, 27);
            txtName.TabIndex = 3;
            // 
            // txtAge
            // 
            txtAge.Location = new Point(150, 209);
            txtAge.Name = "txtAge";
            txtAge.Size = new Size(389, 27);
            txtAge.TabIndex = 4;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(150, 256);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(389, 27);
            txtEmail.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.Chartreuse;
            btnAdd.FlatStyle = FlatStyle.Popup;
            btnAdd.Font = new Font("Cambria", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.Location = new Point(34, 313);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(109, 41);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "Thêm +";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.FlatStyle = FlatStyle.Popup;
            btnUpdate.Font = new Font("Cambria", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdate.Location = new Point(149, 313);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(80, 41);
            btnUpdate.TabIndex = 7;
            btnUpdate.Text = "Sửa ✎";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Red;
            btnDelete.FlatStyle = FlatStyle.Popup;
            btnDelete.Font = new Font("Cambria", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDelete.ForeColor = SystemColors.ButtonHighlight;
            btnDelete.Location = new Point(235, 313);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(81, 41);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.FlatStyle = FlatStyle.Popup;
            btnRefresh.Font = new Font("Cambria", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRefresh.Location = new Point(322, 313);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(217, 41);
            btnRefresh.TabIndex = 9;
            btnRefresh.Text = "Làm mới danh sách ↻";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1226, 497);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            // 
            // btnExport
            // 
            btnExport.BackColor = Color.SeaGreen;
            btnExport.FlatStyle = FlatStyle.Popup;
            btnExport.Font = new Font("Cambria", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExport.ForeColor = SystemColors.ButtonFace;
            btnExport.Location = new Point(34, 375);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(153, 41);
            btnExport.TabIndex = 16;
            btnExport.Text = "Xuất Excel";
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnImport
            // 
            btnImport.BackColor = Color.DarkBlue;
            btnImport.FlatStyle = FlatStyle.Popup;
            btnImport.Font = new Font("Cambria", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImport.ForeColor = SystemColors.ButtonFace;
            btnImport.Location = new Point(193, 375);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(170, 41);
            btnImport.TabIndex = 17;
            btnImport.Text = "Tải File Excel";
            btnImport.UseVisualStyleBackColor = false;
            btnImport.Click += btnImport_Click;
            // 
            // btnDownloadTemplate
            // 
            btnDownloadTemplate.BackColor = Color.Olive;
            btnDownloadTemplate.FlatStyle = FlatStyle.Popup;
            btnDownloadTemplate.Font = new Font("Cambria", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownloadTemplate.ForeColor = SystemColors.ButtonFace;
            btnDownloadTemplate.Location = new Point(369, 375);
            btnDownloadTemplate.Name = "btnDownloadTemplate";
            btnDownloadTemplate.Size = new Size(170, 41);
            btnDownloadTemplate.TabIndex = 18;
            btnDownloadTemplate.Text = "Tải Template";
            btnDownloadTemplate.UseVisualStyleBackColor = false;
            btnDownloadTemplate.Click += btnDownloadTemplate_Click;
            // 
            // button2
            // 
            button2.Location = new Point(867, 495);
            button2.Name = "button2";
            button2.Size = new Size(8, 8);
            button2.TabIndex = 19;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // btnDeleteAll
            // 
            btnDeleteAll.BackColor = Color.LightSkyBlue;
            btnDeleteAll.FlatStyle = FlatStyle.Popup;
            btnDeleteAll.Font = new Font("Cambria", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDeleteAll.ForeColor = SystemColors.ActiveCaptionText;
            btnDeleteAll.Location = new Point(34, 432);
            btnDeleteAll.Name = "btnDeleteAll";
            btnDeleteAll.Size = new Size(229, 41);
            btnDeleteAll.TabIndex = 20;
            btnDeleteAll.Text = "Xóa toàn bộ dữ liệu";
            btnDeleteAll.UseVisualStyleBackColor = false;
            btnDeleteAll.Click += btnDeleteAll_Click;
            // 
            // pbLoading
            // 
            pbLoading.Location = new Point(278, 432);
            pbLoading.Name = "pbLoading";
            pbLoading.Size = new Size(261, 41);
            pbLoading.TabIndex = 21;
            pbLoading.Visible = false;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTime.Location = new Point(562, 445);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(0, 28);
            lblTime.TabIndex = 22;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1226, 497);
            Controls.Add(lblTime);
            Controls.Add(pbLoading);
            Controls.Add(btnDeleteAll);
            Controls.Add(button2);
            Controls.Add(btnDownloadTemplate);
            Controls.Add(btnImport);
            Controls.Add(btnExport);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(txtEmail);
            Controls.Add(txtAge);
            Controls.Add(txtName);
            Controls.Add(txtStudentID);
            Controls.Add(dgvStudents);
            Controls.Add(pictureBox1);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            Text = "Students Information App";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvStudents).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvStudents;
        private TextBox txtStudentID;
        private TextBox txtName;
        private TextBox txtAge;
        private TextBox txtEmail;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnRefresh;
        private PictureBox pictureBox1;
        private Button btnExport;
        private Button btnImport;
        private Button btnDownloadTemplate;
        private Button button2;
        private Button btnDeleteAll;
        private ProgressBar pbLoading;
        private Label lblTime;
    }
}
