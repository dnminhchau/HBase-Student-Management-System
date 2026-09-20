using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace HBaseStudentApp
{
    public partial class MainForm : Form
    {
        private HBaseManager hbase = new HBaseManager();

        public MainForm()
        {
            InitializeComponent();
            SetupDataGridView();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            ShowLoading(true);
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                await LoadData();
            }
            finally
            {
                sw.Stop();
                HideLoading();
                lblTime.Text = $"Thời gian tải dữ liệu: {sw.Elapsed.TotalSeconds:F2} giây";
            }
        }

        private void SetupDataGridView()
        {
            dgvStudents.AutoGenerateColumns = false;
            dgvStudents.Columns.Clear();
            dgvStudents.MultiSelect = true;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Student ID",
                DataPropertyName = "StudentID",
                Name = "StudentID",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                DataPropertyName = "Name",
                Name = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Age",
                DataPropertyName = "Age",
                Name = "Age",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            });
            dgvStudents.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Email",
                DataPropertyName = "Email",
                Name = "Email",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        private async Task LoadData()
        {
            try
            {
                var list = await hbase.GetAllStudents();
                dgvStudents.DataSource = null;
                dgvStudents.DataSource = list;
            }
            catch (Exception ex)
            {
                if (this.Enabled)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var student = dgvStudents.Rows[e.RowIndex].DataBoundItem as Student;
                if (student != null)
                {
                    txtStudentID.Text = student.StudentID;
                    txtName.Text = student.Name;
                    txtAge.Text = student.Age;
                    txtEmail.Text = student.Email;
                }
            }
        }


        private async void btnAdd_Click(object sender, EventArgs e)
        {
            string studentId = txtStudentID.Text.Trim();
            string studentName = txtName.Text.Trim();

            if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(studentName))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ MSSV và Họ tên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            ShowLoading(true);
            try
            {
                var s = new Student
                {
                    StudentID = studentId,
                    Name = studentName,
                    Age = txtAge.Text.Trim(),
                    Email = txtEmail.Text.Trim()
                };

                await hbase.AddStudent(s);
                await LoadData();
                txtStudentID.Clear();
                txtName.Clear();
                txtAge.Clear();
                txtEmail.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi Chung");
            }
            finally
            {
                sw.Stop();
                HideLoading();
                lblTime.Text = $"Thời gian thêm sinh viên: {sw.Elapsed.TotalSeconds:F2} giây";
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            string studentId = txtStudentID.Text.Trim();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ lưới để cập nhật!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc muốn cập nhật thông tin cho MSSV: {studentId}?",
                                         "Xác nhận cập nhật", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No) return;

            Stopwatch sw = Stopwatch.StartNew();
            ShowLoading(true);
            try
            {
                var s = new Student
                {
                    StudentID = studentId,
                    Name = txtName.Text.Trim(),
                    Age = txtAge.Text.Trim(),
                    Email = txtEmail.Text.Trim()
                };

                await hbase.UpdateStudent(s);
                await LoadData();
                txtStudentID.Clear();
                txtName.Clear();
                txtAge.Clear();
                txtEmail.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi Chung");
            }
            finally
            {
                sw.Stop();
                HideLoading();
                lblTime.Text = $"Thời gian cập nhật dữ liệu: {sw.Elapsed.TotalSeconds:F2} giây";
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            var selectedRows = dgvStudents.SelectedRows;
            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sinh viên để xóa.", "Chưa chọn sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show($"Bạn có chắc muốn xóa {selectedRows.Count} sinh viên đã chọn?",
                                               "Xác nhận xóa",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question);

            if (confirmResult == DialogResult.No) return;

            List<string> idsToDelete = new List<string>();
            foreach (DataGridViewRow row in selectedRows)
            {
                var student = row.DataBoundItem as Student;
                if (student != null)
                {
                    idsToDelete.Add(student.StudentID);
                }
            }

            Stopwatch sw = Stopwatch.StartNew();
            ShowLoading(false);
            int successCount = 0;
            int failCount = 0;
            int counter = 0;
            pbLoading.Maximum = idsToDelete.Count + 1;
            StringBuilder errors = new StringBuilder();

            try
            {
                foreach (string id in idsToDelete)
                {
                    counter++;
                    try
                    {
                        await hbase.DeleteStudent(id);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        errors.AppendLine($"Lỗi xóa {id}: {ex.Message}");
                    }
                    pbLoading.Value = counter;
                }

                await LoadData();
                pbLoading.Value = pbLoading.Maximum;
            }
            finally
            {
                sw.Stop();
                HideLoading();
                lblTime.Text = $"Thời gian xóa sinh viên: {sw.Elapsed.TotalSeconds:F2} giây";
            }

            txtStudentID.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtEmail.Clear();

            string report = $"Hoàn tất!\nXóa thành công: {successCount}\nThất bại: {failCount}\n";
            if (failCount > 0)
            {
                report += $"\nChi tiết lỗi:\n{errors.ToString()}";
            }
            MessageBox.Show(report, "Kết quả xóa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnDeleteAll_Click(object sender, EventArgs e)
        {
            var list = dgvStudents.DataSource as List<Student>;

            if (list == null || list.Count == 0)
            {
                MessageBox.Show("Không có sinh viên nào để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa tất cả {list.Count} sinh viên?",
                "CẢNH BÁO",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.No)
            {
                return;
            }

            List<string> idsToDelete = list.Select(s => s.StudentID).ToList();

            Stopwatch sw = Stopwatch.StartNew();
            ShowLoading(false);
            int successCount = 0;
            int failCount = 0;
            int counter = 0;
            pbLoading.Maximum = idsToDelete.Count + 1;
            StringBuilder errors = new StringBuilder();

            try
            {
                foreach (string id in idsToDelete)
                {
                    counter++;
                    try
                    {
                        await hbase.DeleteStudent(id);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        errors.AppendLine($"Lỗi xóa {id}: {ex.Message}");
                    }
                    pbLoading.Value = counter;
                }

                await LoadData();
                pbLoading.Value = pbLoading.Maximum;
            }
            finally
            {
                sw.Stop();
                HideLoading();
                lblTime.Text = $"Thời gian xóa toàn bộ sinh viên: {sw.Elapsed.TotalSeconds:F2}  giây";
            }

            txtStudentID.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtEmail.Clear();

            string report = $"Hoàn tất!\nĐã xóa: {successCount} sinh viên.\nThất bại: {failCount}.\n";
            if (failCount > 0)
            {
                report += $"\nChi tiết lỗi:\n{errors.ToString()}";
            }
            MessageBox.Show(report, "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            ShowLoading(true);
            try
            {
                await LoadData();
            }
            finally
            {
                sw.Stop();
                HideLoading();
                lblTime.Text = $"Thời gian làm mới danh sách: {sw.Elapsed.TotalSeconds:F2}  giây";
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var list = dgvStudents.DataSource as List<Student>;
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV (Tách bằng chấm phẩy) (*.csv)|*.csv";
                sfd.Title = "Lưu file CSV";
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                sfd.FileName = $"DanhSachSinhVien_{timestamp}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("StudentID;Name;Age;Email");
                        foreach (var s in list)
                        {
                            sb.AppendLine($"{s.StudentID};{s.Name};{s.Age};{s.Email}");
                        }

                        File.WriteAllText(sfd.FileName, sb.ToString(), new UTF8Encoding(true));

                        MessageBox.Show("Xuất file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (MessageBox.Show("Bạn có muốn mở file vừa lưu không?", "Hoàn tất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var p = new System.Diagnostics.Process();
                            p.StartInfo = new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true };
                            p.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Có lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        sw.Stop();
                        lblTime.Text = $"Thời gian xuất: {sw.Elapsed.TotalSeconds:F2}  giây";
                    }
                }
            }
        }

        private void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV (Tách bằng chấm phẩy) (*.csv)|*.csv";
                sfd.Title = "Lưu file template CSV";
                sfd.FileName = "Template_SinhVien_Import.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string header = "StudentID;Name;Age;Email";
                        string example = "SV001;Nguyen Van A;20;a@example.com";
                        string content = header + Environment.NewLine + example;

                        File.WriteAllText(sfd.FileName, content, Encoding.UTF8);
                        MessageBox.Show("Lưu file template thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lưu file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV Files (*.csv)|*.csv";
                ofd.Title = "Chọn file CSV để nhập";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                var studentsToImport = new List<Student>();
                string filePath = ofd.FileName;
                int lineCounter = 1;
                var headerMap = new Dictionary<string, int>();

                Stopwatch sw = Stopwatch.StartNew();
                ShowLoading(false);

                try
                {
                    string[] allLines = File.ReadAllLines(filePath, Encoding.Default);

                    if (allLines.Length < 2)
                    {
                        MessageBox.Show("File CSV phải có ít nhất 1 hàng tiêu đề và 1 hàng dữ liệu.", "File rỗng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string headerLine = allLines[0];
                    string[] headers = headerLine.Split(';');

                    for (int i = 0; i < headers.Length; i++)
                    {
                        string cleanedHeader = headers[i].Trim().Trim(new char[] { '\uFEFF' });
                        if (!headerMap.ContainsKey(cleanedHeader))
                        {
                            headerMap.Add(cleanedHeader, i);
                        }
                    }

                    string[] requiredCols = { "StudentID", "Name", "Age", "Email" };
                    if (!requiredCols.All(col => headerMap.ContainsKey(col)))
                    {
                        MessageBox.Show("File CSV thiếu các cột bắt buộc. Cần có: StudentID, Name, Age, Email", "Lỗi Tiêu Đề", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    foreach (string line in allLines.Skip(1))
                    {
                        lineCounter++;
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] columns = line.Split(';');
                        if (columns.Length < headers.Length) continue;

                        var s = new Student
                        {
                            StudentID = columns[headerMap["StudentID"]].Trim(),
                            Name = columns[headerMap["Name"]].Trim(),
                            Age = columns[headerMap["Age"]].Trim(),
                            Email = columns[headerMap["Email"]].Trim()
                        };

                        if (!string.IsNullOrWhiteSpace(s.StudentID) && s.StudentID.Length < 20)
                        {
                            studentsToImport.Add(s);
                        }
                    }
                }
                catch (Exception exFile)
                {
                    if (exFile is System.IO.IOException && exFile.Message.Contains("being used by another process"))
                    {
                        MessageBox.Show("Lỗi: File này đang được mở. Vui lòng đóng file CSV lại và thử nhập lại.", "Lỗi Đọc File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Không thể đọc file (dòng {lineCounter}): {exFile.Message}", "Lỗi đọc file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }

                if (studentsToImport.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu sinh viên hợp lệ trong file.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Bạn có chắc muốn thêm {studentsToImport.Count} sinh viên vào cơ sở dữ liệu?", "Xác nhận nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                int successCount = 0;
                int failCount = 0;
                int counter = 0;
                pbLoading.Maximum = studentsToImport.Count + 1;
                StringBuilder errors = new StringBuilder();

                try
                {
                    foreach (var student in studentsToImport)
                    {
                        counter++;
                        try
                        {
                            await hbase.AddStudent(student);
                            successCount++;
                        }
                        catch (Exception exDb)
                        {
                            failCount++;
                            errors.AppendLine($"Lỗi thêm {student.StudentID}: {exDb.Message}");
                        }
                        pbLoading.Value = counter;
                    }

                    await LoadData();
                    pbLoading.Value = pbLoading.Maximum;
                }
                finally
                {
                    sw.Stop();
                    HideLoading();
                    lblTime.Text = $"Thời gian nhập dữ liệu từ file: {sw.Elapsed.TotalSeconds:F2}   giây";
                }

                string report = $"Hoàn tất!\n\nThành công: {successCount}\nThất bại: {failCount}\n";
                if (failCount > 0)
                {
                    report += $"\nChi tiết lỗi:\n{errors.ToString()}";
                }
                MessageBox.Show(report, "Kết quả nhập", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ShowLoading(bool indeterminate = true)
        {
            pbLoading.Visible = true;
            this.Enabled = false;

            if (indeterminate)
            {
                pbLoading.Style = ProgressBarStyle.Marquee;
            }
            else
            {
                pbLoading.Style = ProgressBarStyle.Blocks;
                pbLoading.Value = 0;
                pbLoading.Minimum = 0;
                pbLoading.Maximum = 100;
            }
        }

        private void HideLoading()
        {
            pbLoading.Visible = false;
            this.Enabled = true;
            pbLoading.Value = 0;
            pbLoading.Style = ProgressBarStyle.Marquee;
        }
    }
}
