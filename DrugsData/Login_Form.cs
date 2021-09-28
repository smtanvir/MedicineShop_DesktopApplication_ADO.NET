using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrugsData
{
    public partial class Frm_Login : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Trusted_Connection=true");
        public Frm_Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_user WHERE  userName='" + txtusername.Text + "' and userPassword='" + txtpassword.Text + "'", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                Frm_Main mainPage = new Frm_Main();
                mainPage.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("User Name or Password don't match. Please Enter valid Data", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                txtusername.Clear();
                txtpassword.Clear();
                txtusername.Focus();
            }
    }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Frm_RegistrationForm regForm = new Frm_RegistrationForm();
            this.Hide();
            regForm.Show();
        }
    }
}
