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
    public partial class Frm_AddNewCustomer : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=LAPTOP-25K9NGFB;Initial Catalog=MS_Medicine_Corner; Integrated Security=true");
        public Frm_AddNewCustomer()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    if (MessageBox.Show("Do you save data ?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO tbl_customers (firstName,lastName,genderID,contactNo,email,streetAddress,postalCode,city) VALUES(@fname,@lname,@gender,@contact,@email,@address,@postalCode,@city)", connection);
                        cmd.CommandType = CommandType.Text;

                        int genderID = 1;
                        if (rdMale.Checked == true)
                        {
                            genderID = 1;
                        }
                        else if (rdFemale.Checked == true)
                        {
                            genderID = 2;
                        }
                        else
                        {
                            MessageBox.Show("You haven\'t select gender!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        }

                        cmd.Parameters.AddWithValue("@fname", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@lname", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@gender", genderID);
                        cmd.Parameters.AddWithValue("@contact", txtContactNumber.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@address", txtStreetAddress.Text);
                        cmd.Parameters.AddWithValue("@postalCode", txtPostalCode.Text);
                        cmd.Parameters.AddWithValue("@city", txtCity.Text);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data has been inserted successfully", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetAll();
                    }
                    else
                    {
                        MessageBox.Show("Data doesn\'t saved!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data doesn\'t saved!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private bool IsValid()
        {
            if (txtFirstName.Text == "")
            {
                MessageBox.Show("Please enter customer first name!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtLastName.Text == "")
            {
                MessageBox.Show("Please enter customer last name!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtContactNumber.Text == "")
            {
                MessageBox.Show("Please enter customer contact number!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Please enter e-mail address!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtStreetAddress.Text == "")
            {
                MessageBox.Show("Please enter customer address!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtPostalCode.Text == "")
            {
                MessageBox.Show("Please enter postal code or ZIP code!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtCity.Text == "")
            {
                MessageBox.Show("Please enter your city!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        private void ResetAll()
        {
            txtFirstName.Text = "";
            txtLastName.Text = string.Empty;
            rdFemale.Checked = false;
            rdMale.Checked = false;
            txtContactNumber.Text = "";
            txtEmail.Text = string.Empty;
            txtStreetAddress.Clear();
            txtPostalCode.Clear();
            txtCity.Clear();
        }
    }
}
