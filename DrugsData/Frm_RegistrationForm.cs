using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DrugsData
{
    public partial class Frm_RegistrationForm : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Trusted_Connection=true");
        public Frm_RegistrationForm()
        {
            InitializeComponent();
        }

        private void Frm_RegistrationForm_Load(object sender, EventArgs e)
        {

        }

        private void btnRegistration_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbl_user (firstName,lastName,contactNo,email,userName,userPassword) VALUES (@fname,@lname,@contact,@email,@username,@password)", connection);
                    cmd.Parameters.AddWithValue("@fname", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@lname", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@contact", txtContact.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Registration Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetAll();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Registration failed!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Frm_Login login = new Frm_Login();
            this.Hide();
            login.Show();
        }

        private void ResetAll()
        {
            txtContact.Clear();
            txtEmail.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPassword.Clear();
            txtUsername.Clear();
        }

        private bool IsValid()
        {
            if (txtFirstName.Text == null)
            {
                MessageBox.Show("Please Enter First Name", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtLastName.Text == null)
            {
                MessageBox.Show("Please Enter Last Name", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtEmail.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Email", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtContact.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Contact", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            else if (txtUsername.Text == "")
            {
                MessageBox.Show("Please Enter User Name", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtPassword.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;

        }
    }
}
