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

namespace StudentManagement
{
    public partial class frmCourse : Form
    {
        private readonly StudentManagement1Context _db;

        public frmCourse()
        {
            InitializeComponent();
            _db = new StudentManagement1Context();
        }

        private void dgvCourse_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmCourse_Load(object sender, EventArgs e)
        {
            LoadCourse();
        }

        private void LoadCourse()
        {
            var Course = _db.Courses.Select(c => new
            {
                Code = c.Code,
                Name = c.Name
            }).ToList();
            dgvCourse.DataSource = Course;
        }

        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                string code = txtCode.Text;
                string name = txtNameCourse.Text;
                var query = _db.Courses.FirstOrDefault(c => c.Code == code);
                if (query == null)
                {
                    _db.Courses.Add(new Course { Code = code, Name = name });                   
                    if (_db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add successfull");
                        LoadCourse();
                        Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Code is already, you can edit it!");
                }
            }
        }

        private void Clear()
        {
            txtCode.Clear();
            txtNameCourse.Clear();
        }

        private void btnUpdateCourse_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                string code = dgvCourse.CurrentRow.Cells["Code"].Value.ToString();
                string codeCourse = txtCode.Text;
                string name = txtNameCourse.Text;
                if (code != null && name != null)
                {
                    var query = _db.Courses.FirstOrDefault(c => c.Code == code);
                    if (query != null)
                    {
                        query.Code = codeCourse;
                        query.Name = name;

                        if (_db.SaveChanges() > 0)
                        {
                            MessageBox.Show("Edit Successfully");
                            LoadCourse();
                            Clear();
                        }
                    }
                }
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvCourse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if(index >= 0)
            {
                txtCode.Text = dgvCourse.Rows[index].Cells["Code"].Value.ToString();
                txtNameCourse.Text = dgvCourse.Rows[index].Cells["Name"].Value.ToString();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtCode.Clear();
            txtNameCourse.Clear();
            txtSearch.Clear();
        }
        private bool CheckInput()
        {
            string mess = "";
            if(txtCode.Text.Length == 0)
            {
                mess += "Code is not empty\n";
            }
            if(txtNameCourse.Text.Length == 0)
            {
                mess += "Name is not empty\n";
            }
            if(txtCode.Text.Length > 10)
            {
                mess += "Code is too longer!\n";
            }
            if(txtNameCourse.Text.Length > 50)
            {
                mess += "Name is to longer!";
            }
            if(mess != "")
            {
                MessageBox.Show(mess);
                return false;
            }
            return true;
        }

        private void btnDeleteCourse_Click(object sender, EventArgs e)
        {
            string id = dgvCourse.CurrentRow.Cells["Code"].Value.ToString();
            var grade = _db.Grades.FirstOrDefault(c => c.Course.Code == id);
            if (grade == null)
            {

                var query = _db.Courses.FirstOrDefault(p => p.Code == id);
                if (query != null)
                {
                    _db.Courses.Remove(query);
                    if (_db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Delete succesfull");
                        LoadCourse();
                    }
                }
            }
            else
                MessageBox.Show("You cannot delete this Course. To delete, delete this Course in the previous Grade table",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadDgv();

        }

        private void loadDgv()
        {
            var list = from s in _db.Courses

                       where s.Code.Contains(txtSearch.Text)
                       select new
                       {
                           Code = s.Code,
                           Name = s.Name
                       };

            dgvCourse.DataSource = null;
            dgvCourse.DataSource = list.ToList();
        }
    }
}
