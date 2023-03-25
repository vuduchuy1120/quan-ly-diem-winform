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

    public partial class frmGrade : Form

    {
        private readonly StudentManagement1Context _db;

        public frmGrade()
        {
            InitializeComponent();
            _db = new StudentManagement1Context();

        }

        private void dgvGrade_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvGrade_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (index >= 0)
            {
                txtStudent_ID.Text = dgvGrade.Rows[index].Cells[0].Value.ToString();
                txtStudent_ID.Enabled = false;
                txtGrade.Text = dgvGrade.Rows[index].Cells[3].Value.ToString();
                cbCourse.Text = dgvGrade.Rows[index].Cells[2].Value.ToString();
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        private void frmGrade_Load(object sender, EventArgs e)
        {
            var course = _db.Courses.Select(c => c.Code).Distinct().ToList();
            cbFilterCourse.Items.Add(" ");
            foreach (var filterCourse in course)
            {
                cbFilterCourse.Items.Add(filterCourse);
            }
            LoadGrade();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void LoadGrade()
        {
            var result = from g in _db.Grades
                         join c in _db.Courses on g.Course.Id equals c.Id
                         join s in _db.Students on g.StudentId equals s.RollNumber
                         select new
                         {
                             StudentId = g.StudentId,
                             FullName = s.FirstName + " " + s.LastName,
                             Course = c.Code,
                             Grade = g.Grade1
                         };
            dgvGrade.DataSource = result.ToList();

            var course = _db.Courses.Select(c => c.Code).Distinct().ToList();
            cbCourse.DataSource = course;
        }
        private bool CheckInput()
        {
            string mess = "";
            string id = txtStudent_ID.Text.Trim();
            string course = cbCourse.Text.Trim();
            var queryCourse = _db.Courses.FirstOrDefault(c => c.Code == course);
            var queryId = _db.Students.FirstOrDefault(c => c.RollNumber == id);
            if (queryId == null && id != "")
            {
                mess += "Student is not exist!\n";
            }
            if (queryCourse == null)
            {
                mess += "Course is not exist!\n";
            }
            try
            {
                float grade = float.Parse(txtGrade.Text.Trim());
                if (grade < 0 || grade > 10)
                {
                    throw new Exception(mess);
                }
            }
            catch (Exception ex)
            {
                mess += "Grade is not valid\n";
            }
            if (string.IsNullOrEmpty(id))
            {
                mess += "Student ID is not empty\n";
            }

            if (mess.Length > 0)
            {
                MessageBox.Show(mess);
                return false;
            }
            return true;

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                string id = txtStudent_ID.Text;
                float grade = float.Parse(txtGrade.Text.Trim());
                var CourseID = _db.Courses.FirstOrDefault(p => p.Code == cbCourse.Text.Trim()).Id;
                var query = _db.Grades.FirstOrDefault(c => c.StudentId == id && c.CourseId == CourseID);
                if (query == null)
                {
                    _db.Grades.Add(new Grade { StudentId = id, CourseId = CourseID, Grade1 = grade });
                    if (_db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add successfull");
                        LoadGrade();
                    }
                }
                else
                {
                    MessageBox.Show("This Student is already exist grade for this course, You only can delete or update!");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CheckInput())
            {
                string code = dgvGrade.CurrentRow.Cells["StudentId"].Value.ToString();
                int course = _db.Courses.FirstOrDefault(p => p.Code == cbCourse.Text.Trim()).Id;
                float grade = float.Parse((txtGrade.Text.Trim()));

                var query = _db.Grades.FirstOrDefault(c => c.StudentId == code && c.CourseId==course);
                if (query != null)
                {
                    query.StudentId = code;
                    query.CourseId = course;
                    query.Grade1 = grade;

                    if (_db.SaveChanges() > 0)
                    {
                        MessageBox.Show("Edit Successfully");
                        LoadGrade();
                    }
                }

            }
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            txtStudent_ID.Enabled = true;
            txtStudent_ID.Clear();
            txtGrade.Clear();
            cbFilterCourse.SelectedIndex = 0;
            txtFilterGrade.Clear();
            txtFilterID.Clear();
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string id = dgvGrade.CurrentRow.Cells["StudentId"].Value.ToString();
            var query = _db.Grades.FirstOrDefault(p => p.StudentId == id);
            if (query != null)
            {
                _db.Grades.Remove(query);
                if (_db.SaveChanges() > 0)
                {
                    MessageBox.Show("Delete");
                    LoadGrade();
                }
            }
        }

        private void txtFilterID_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void cbFilterCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void txtFilterGrade_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void Filter()
        {         
            var result = from g in _db.Grades
                         join c in _db.Courses on g.Course.Id equals c.Id
                         join s in _db.Students on g.StudentId equals s.RollNumber
                         where g.StudentId.Contains(txtFilterID.Text) && c.Code.Contains(cbFilterCourse.Text.Trim()) && g.Grade1.ToString().Contains(txtFilterGrade.Text)
                         select new
                         {
                             StudentId = g.StudentId,
                             FullName = s.FirstName + " " + s.LastName,
                             Course = c.Code,
                             Grade = g.Grade1
                         };
            

            dgvGrade.DataSource = null;
            dgvGrade.DataSource = result.ToList();
        }
    }
}

