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
    public partial class Frm_CustomerUpdate : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Trusted_Connection=true");
        int indexRow;
        public Frm_CustomerUpdate()
        {
            InitializeComponent();
        }

        public enum Gender
        {
            Male=1,
            Female=2
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    if (MessageBox.Show("Do you update information ?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE tbl_customers SET firstName=@fname,lastName=@lname,contactNo=@contact,email=@email,streetAddress=@address,postalCode=@postalCode,city=@city WHERE customerID=@customerID", connection);
                        cmd.CommandType = CommandType.Text;

                        //int genderID;

                        cmd.Parameters.AddWithValue("@customerID", txtCustomerID.Text);
                        cmd.Parameters.AddWithValue("@fname", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@lname", txtLastName.Text);
                        //cmd.Parameters.AddWithValue("@gender", genderID);
                        cmd.Parameters.AddWithValue("@contact", txtContactNo.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@postalCode", txtPostalCode.Text);
                        cmd.Parameters.AddWithValue("@city", txtCity.Text);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data has been updated successful!", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAll();
                        LoadCustomerInfo();
                        LoadDefault();
                    }
                    else
                    {
                        MessageBox.Show("Data doesn\'t update!!!", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data update failed!!!", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnUpdate.Show();
            btnEdit.Hide();
            btnClear.Enabled = true;

            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            cmbGender.Enabled = true;
            txtContactNo.Enabled = true;
            txtEmail.Enabled = true;
            txtAddress.Enabled = true;
            txtPostalCode.Enabled = true;
            txtCity.Enabled = true;

            //LoadGender();
        }

        private void Frm_CustomerUpdate_Load(object sender, EventArgs e)
        {
            LoadCustomerInfo();
            LoadDefault();

            txtCustomerID.Enabled = false;
        }

        private void LoadCustomerInfo()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT c.customerID [Customer ID], c.firstName [First Name],c.lastName [Last Name], g.gender[Gender], c.contactNo [Contact No], c.email [E-mail], c.streetAddress [Address], c.postalCode [Postal Code],c.city [City] FROM tbl_customers c INNER JOIN tbl_gender g on c.genderID=g.genderID", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void LoadDefault()
        {
            btnUpdate.Hide();
            btnEdit.Show();
            btnEdit.Enabled = false;
            btnClear.Enabled = false;

            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            cmbGender.Enabled = false;
            txtContactNo.Enabled = false;
            txtEmail.Enabled = false;
            txtAddress.Enabled = false;
            txtPostalCode.Enabled = false;
            txtCity.Enabled = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;

            DataGridViewRow row = dataGridView1.Rows[indexRow];

            txtCustomerID.Text = row.Cells[0].Value.ToString();
            txtFirstName.Text = row.Cells[1].Value.ToString();
            txtLastName.Text = row.Cells[2].Value.ToString();
            cmbGender.Text = row.Cells[3].Value.ToString();
            txtContactNo.Text = row.Cells[4].Value.ToString();
            txtEmail.Text = row.Cells[5].Value.ToString();
            txtAddress.Text = row.Cells[6].Value.ToString();
            txtPostalCode.Text = row.Cells[7].Value.ToString();
            txtCity.Text = row.Cells[8].Value.ToString();

            btnEdit.Enabled = true;
        }

        //private void LoadGender()
        //{
        //    string[] gender = Enum.GetNames(typeof(Gender));
        //    foreach (var g in gender)
        //    {
        //        cmbGender.Items.Add(g);
        //    }
        //}

        private bool IsValid()
        {
            if (txtFirstName.Text == "")
            {
                MessageBox.Show("Please enter customer first name!!!", "Update Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtLastName.Text == "")
            {
                MessageBox.Show("Please enter customer last name!!!", "Update Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (cmbGender.Text == "")
            {
                MessageBox.Show("Please select gender!!!", "Update Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtContactNo.Text == "")
            {
                MessageBox.Show("Please enter customer contact number!!!", "Update Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Please enter e-mail address!!!", "Update Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter customer address!!!", "Update Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtPostalCode.Text == "")
            {
                MessageBox.Show("Please enter postal code!!!", "Update Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtCity.Text == "")
            {
                MessageBox.Show("Please enter city!!!", "Update Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ClearAll()
        {
            txtCustomerID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            cmbGender.Text = "";
            txtContactNo.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtPostalCode.Clear();
            txtPostalCode.Clear();
            txtCity.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
