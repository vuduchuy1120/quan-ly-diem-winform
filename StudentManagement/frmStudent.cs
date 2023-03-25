using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
namespace StudentManagement
{
    public partial class frmStudent : Form
    {
        private readonly StudentManagement1Context _db;

        public frmStudent()
        {
            InitializeComponent();
            _db = new StudentManagement1Context();
        }


        private void LoadStudent()
        {
            var student = _db.Students.Select(e => new
            {
                RollNumber = e.RollNumber,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Address = e.Address,
                Gender = e.Gender,
                Dob = e.Dob,
                Major = e.Major.Description
            }).ToList();

            dataGridView1.DataSource = student;
            bool male = (bool)dataGridView1.CurrentRow.Cells["Gender"].Value;


            if (male)
            {
                rbMale.Checked = true;
                rbFemale.Checked = false;
            }
            else if (male)
            {
                rbFemale.Checked = true;
                rbMale.Checked = false;
            }


            var major = _db.Majors.Select(m => m.Description).Distinct().ToList();
            cbMajor.DataSource = major;

        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            LoadStudent();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

            if (index >= 0)
            {
                txtRollNumber.Text = dataGridView1.Rows[index].Cells[0].Value.ToString();
                txtFirstName.Text = dataGridView1.Rows[index].Cells[1].Value.ToString();
                txtLastName.Text = dataGridView1.Rows[index].Cells[2].Value.ToString();
                if ((bool)dataGridView1.Rows[index].Cells["Gender"].Value)
                {
                    rbMale.Checked = true;
                }
                else
                {
                    rbFemale.Checked = true;
                }
                cbMajor.Text = dataGridView1.Rows[index].Cells[7].Value.ToString(); 
                txtAddress.Text = dataGridView1.Rows[index].Cells[4].Value.ToString();
                txtEmail.Text = dataGridView1.Rows[index].Cells[3].Value.ToString();
                dateTimePicker1.Text = dataGridView1.Rows[index].Cells[6].Value.ToString();

            }
            btnAdd.Enabled = false;
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            txtRollNumber.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            var major = _db.Majors.Select(m => m.Description).Distinct().ToList();
            cbMajor.DataSource = major;

            string RollNumber = txtRollNumber.Text;
            if (checkInput())
            {
                var query = _db.Students.FirstOrDefault(c => c.RollNumber == RollNumber);
                if (query == null)
                {
                    _db.Students.Add(new Student
                    {
                        RollNumber = txtRollNumber.Text,
                        Gender = rbFemale.Checked ? false : true,
                        Dob = Convert.ToDateTime(dateTimePicker1.Text),
                        FirstName = txtFirstName.Text,
                        Email = txtEmail.Text,
                        MajorId = _db.Majors.FirstOrDefault(p => p.Description == cbMajor.Text).Id,
                        LastName = txtLastName.Text,
                        Address = txtAddress.Text,
                    });
                    if (_db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add successfull");
                        LoadStudent(); 
                    }
                }
                else
                {
                    MessageBox.Show("ID is already exist");
                }
            }
        }

        private void Clear()
        {
            txtRollNumber.DataBindings.Clear();
            txtFirstName.DataBindings.Clear();
            txtLastName.DataBindings.Clear();
            txtEmail.DataBindings.Clear();
            txtAddress.DataBindings.Clear();
            cbMajor.DataBindings.Clear();
            cbMajor.SelectedIndex = 0;

            dateTimePicker1.DataBindings.Clear();
            dateTimePicker1.Text = DateTime.Now.ToString();
            rbFemale.DataBindings.Clear();
            rbFemale.Checked = false;
            rbMale.DataBindings.Clear();
            rbMale.Checked = false;
            txtRollNumber.Text = String.Empty;
        }
        private void btnRefesh_Click(object sender, EventArgs e)
        {
            Clear();
            LoadStudent();
            txtRollNumber.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnAdd.Enabled = true;
            txtRollNumber.Enabled = true;
            txtSearchFirstName.Clear();
            txtSearchLastName.Clear();
            txtSearchRoll.Clear();
        }
        public bool checkInput()
        {
            string mess = "";
            string RollNumber = txtRollNumber.Text;
            string FirstName = txtFirstName.Text;
            string LastName = txtLastName.Text;
            dateTimePicker1.DataBindings.Clear();
            if (RollNumber.Length != 8)
            {
                mess += "RollNumber must be 8 character!\n";
            }
            if (String.IsNullOrEmpty(FirstName))
            {
                mess += "FirstName is not Empty\n";
            }
            if (String.IsNullOrEmpty(LastName))
            {
                mess += "LastName is not Empty\n";
            }
            if(FirstName.Length > 50 || LastName.Length > 50)
            {
                mess += "FirstName or LastName is too long!\n";
            }
            if (txtEmail.Text.Length > 50)
            {
                mess += "Email is too long!\n";
            }
            if (mess.Length > 0)
            {
                MessageBox.Show(mess, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
                return true;    
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
            DateTime dob =  Convert.ToDateTime(dateTimePicker1.Text);
            if (checkInput())
            {
                var old_emp = _db.Students.SingleOrDefault(p => p.RollNumber == txtRollNumber.Text);
                

                if (old_emp != null)
                {
                    old_emp.FirstName = txtFirstName.Text;
                    old_emp.LastName = txtLastName.Text;
                    old_emp.Major = _db.Majors.FirstOrDefault(p => p.Description == cbMajor.Text);
                    old_emp.Gender = rbFemale.Checked ? false : true;
                    old_emp.Dob = dob;
                    old_emp.Email = txtEmail.Text;
                    old_emp.Address = txtAddress.Text;
                    if (_db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Edit Successfully");
                        LoadStudent();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id =dataGridView1.CurrentRow.Cells["RollNumber"].Value.ToString();
           

            var grade = _db.Grades.FirstOrDefault(c => c.StudentId == id);
            if (grade == null)
            {
                var query = _db.Students.FirstOrDefault(p => p.RollNumber == id);

                if (query != null)
                {
                    _db.Students.Remove(query);
                    if (_db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Delete successfull!");
                        LoadStudent();
                    }
                }

            }
            else
            {
                MessageBox.Show("You cannot delete this student. To delete, delete this student in the previous Grade table",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            
        }

        private void txtSearchRoll_TextChanged(object sender, EventArgs e)
        {
            Loadgv();
        }

        private void txtSearchFirstName_TextChanged(object sender, EventArgs e)
        {
            Loadgv();
        }

        private void txtSearchLastName_TextChanged(object sender, EventArgs e)
        {
            Loadgv();
        }

        private void Loadgv()
        {
            var list = from s in _db.Students
                       
                       where s.RollNumber.Contains(txtSearchRoll.Text) && s.FirstName.Contains(txtSearchFirstName.Text)
                       && s.LastName.Contains(txtSearchLastName.Text)
                       select new
                       {
                           RollNumber = s.RollNumber,
                           FirstName = s.FirstName,
                           LastName = s.LastName,
                           Email = s.Email,
                           Address = s.Address,
                           Gender = s.Gender,
                           Dob = s.Dob,
                           Major = s.Major.Description,
                           
                       };

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list.ToList();
        }

        private void Sort_Click(object sender, EventArgs e)
        {
            var list = from s in _db.Students
                       orderby s.RollNumber descending
                       select new
                       {
                           RollNumber = s.RollNumber,
                           FirstName = s.FirstName,
                           LastName = s.LastName,
                           Email = s.Email,
                           Address = s.Address,
                           Gender = s.Gender,
                           Dob = s.Dob,
                           Major = s.Major.Description,
                       };
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list.ToList();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //gọi hàm ToExcel() với tham số là dtgDSHS và filename từ SaveFileDialog
                ToExcel(dataGridView1, saveFileDialog1.FileName);
            }
        }

        private void ToExcel(DataGridView dataGridView1, string fileName)
        {
            //khai báo thư viện hỗ trợ Microsoft.Office.Interop.Excel
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;
            try
            {
                //Tạo đối tượng COM.
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                //tạo mới một Workbooks bằng phương thức add()
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                //đặt tên cho sheet
                worksheet.Name = "Grade Management";

                // export header trong DataGridView
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                // export nội dung trong DataGridView
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value;
                    }
                }
                // sử dụng phương thức SaveAs() để lưu workbook với filename
                workbook.SaveAs(fileName);
                //đóng workbook
                workbook.Close();
                excel.Quit();
                MessageBox.Show("Xuất dữ liệu ra Excel thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var list = from s in _db.Students
                       where s.RollNumber.Contains(txtSearchRoll.Text) && s.FirstName.Contains(txtSearchFirstName.Text)
                       && s.LastName.Contains(txtSearchLastName.Text)
                       orderby s.RollNumber descending
                       select new
                       {
                           RollNumber = s.RollNumber,
                           FirstName = s.FirstName,
                           LastName = s.LastName,
                           Email = s.Email,
                           Address = s.Address,
                           Gender = s.Gender,
                           Dob = s.Dob,
                           Major = s.Major.Description,
                       };
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = list.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //gọi hàm ToExcel() với tham số là dtgDSHS và filename từ SaveFileDialog
                ToExcel(dataGridView1, saveFileDialog1.FileName);
            }
        }
    }
}
