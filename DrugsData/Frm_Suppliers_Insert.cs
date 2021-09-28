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
    public partial class Frm_Suppliers_Insert : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.; Initial Catalog=MS_Medicine_Corner; Integrated Security=true");
        public Frm_Suppliers_Insert()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsValid()
        {
            if (txtCompanyName.Text == "")
            {
                MessageBox.Show("Please enter your Company Name!!!", "Failed", MessageBoxButtons.RetryCancel,MessageBoxIcon.Error);
                return false;
            }
            else if (txtOfficeLocation.Text == string.Empty)
            {
                MessageBox.Show("Please enter your company location!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtContactPerson.Text == "")
            {
                MessageBox.Show("Please enter contact person name!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtContactNo.Text == "")
            {
                MessageBox.Show("Please enter contact number!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtContactNo.Text.Length !=11)
            {
                MessageBox.Show("Contact Number must be 11 digit!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Please enter e-mail addrees!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter street address!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if(txtPostalCode.Text=="")
            {
                MessageBox.Show("Please enter postal code!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtPostalCode.Text.Length != 4)
            {
                MessageBox.Show("Postal code must be in 4 digit!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    if (MessageBox.Show("Do you save data ?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO tbl_suppliers (companyName,officeLocation,contactPersonName,contactNo,email,streetAddress,postalCode) VALUES(@company,@location,@contactName,@contactNo,@email,@address,@postalCode)", connection);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@company", txtCompanyName.Text);
                        cmd.Parameters.AddWithValue("@location", txtOfficeLocation.Text);
                        cmd.Parameters.AddWithValue("@contactName", txtContactPerson.Text);
                        cmd.Parameters.AddWithValue("@contactNo", txtContactNo.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@postalCode", txtPostalCode.Text);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data inserted successfully!!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetAll();
                        
                    }
                    else
                    {
                        MessageBox.Show("Data doesn\'t saved!!!\nPlease try again!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data doesn\'t saved!!!\nPlease try again!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            

        }
        private void ResetAll()
        {
            txtAddress.Clear();
            txtCompanyName.Clear();
            txtContactNo.Clear();
            txtContactPerson.Clear();
            txtEmail.Clear();
            txtOfficeLocation.Clear();
            txtPostalCode.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
    }
}
