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
    public partial class Control : Form
    {
        public Control()
        {
            InitializeComponent();
        }

        private void mnuLogin_Click(object sender, EventArgs e)
        {
            groupBox1.Show();
            pbHome.Hide();
        }

        private void Control_Load(object sender, EventArgs e)
        {

            groupBox1.Hide();
            pbLogin.Hide();
            pbIm.Hide();
            mnuLogout.Enabled = false;
            danhMụcToolStripMenuItem.Enabled = false;
            
            
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text == "admin" && txtPassword.Text == "admin")
            {
                danhMụcToolStripMenuItem.Enabled = true;
                mnuLogout.Enabled = true;
                mnuLogin.Enabled = false;
                pbLogin.Show();
                pbIm.Show();
                groupBox1.Hide();
            }
            else if (txtEmail.Text != "admin" || txtPassword.Text != "admin")
            {
                MessageBox.Show("Username or password incorrect, please check again!");
            }

            
        }

        private void mnuDanhMucSinhVien_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmStudent frmStudent = new frmStudent();
            frmStudent.Show();
            frmStudent.FormClosed += (sender, e) =>
            {
                txtEmail.Clear();
                txtPassword.Clear();
                this.Show();
            };
        }

        private void mnuDanhMucGrade_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmGrade frmStudent = new frmGrade();
            frmStudent.Show();
            frmStudent.FormClosed += (sender, e) =>
            {
                txtEmail.Clear();
                txtPassword.Clear();
                this.Show();
            };
        }

        private void mnuDanhMucCourse_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmCourse frmStudent = new frmCourse();
            frmGrade frmGrade = new frmGrade();
            frmGrade.Close();
            frmStudent.Show();
            frmStudent.FormClosed += (sender, e) =>
            {
                txtEmail.Clear();
                txtPassword.Clear();
                this.Show();
            };
        }
        private void btnCancel_Click(object sender, EventArgs e) => Close();

        private void mnuExit_Click(object sender, EventArgs e) => Close();

        private void mnuLogout_Click(object sender, EventArgs e)
        {
            groupBox1.Hide();
            mnuLogout.Enabled = false;
            danhMụcToolStripMenuItem.Enabled = false;
            mnuLogin.Enabled = true;
            pbIm.Hide();
            pbLogin.Hide();
            pbHome.Show();
        }

        private void pbLogin_Click(object sender, EventArgs e)
        {

        }

        private void pbIm_Click(object sender, EventArgs e)
        {

        }

        private void pbHome_Click(object sender, EventArgs e)
        {

        }

        private void pbHome_Click_1(object sender, EventArgs e)
        {

        }
    }
}
