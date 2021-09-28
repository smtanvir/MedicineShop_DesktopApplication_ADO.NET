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
    public partial class Frm_SupplersUpdate : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Trusted_Connection=true");

        int indexRow;
        public Frm_SupplersUpdate()
        {
            InitializeComponent();
        }

        private void Frm_SupplersUpdate_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            LoadDefault();

            txtSuppliersID.Enabled = false;
        }

        private void LoadDefault()
        {
            txtCompanyName.Enabled = false;
            txtOfficeLocation.Enabled = false;
            txtContactPerson.Enabled = false;
            txtContactNo.Enabled = false;
            txtEmail.Enabled = false;
            txtAddress.Enabled = false;
            txtPostalCode.Enabled = false;

            btnClear.Enabled = false;
            btnUpdate.Hide();
            btnEdit.Show();
            btnEdit.Enabled = false;
        }

        private void LoadSuppliers()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT suppliersID [Suppliers ID],companyName [Company Name],officeLocation [Office Location],contactPersonName [Contact Person],contactNo [Contact No],email [E-mail],streetAddress [Address],postalCode [Postal Code] FROM tbl_suppliers", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[indexRow];

            txtSuppliersID.Text = row.Cells[0].Value.ToString();
            txtCompanyName.Text = row.Cells[1].Value.ToString();
            txtOfficeLocation.Text = row.Cells[2].Value.ToString();
            txtContactPerson.Text = row.Cells[3].Value.ToString();
            txtContactNo.Text = row.Cells[4].Value.ToString();
            txtEmail.Text = row.Cells[5].Value.ToString();
            txtAddress.Text = row.Cells[6].Value.ToString();
            txtPostalCode.Text = row.Cells[7].Value.ToString();

            btnEdit.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        private void ClearAll()
        {
            txtSuppliersID.Clear();
            txtCompanyName.Clear();
            txtOfficeLocation.Clear();
            txtContactPerson.Clear();
            txtContactNo.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtPostalCode.Clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnUpdate.Show();
            btnEdit.Hide();
            btnClear.Enabled = true;

            txtCompanyName.Enabled = true;
            txtOfficeLocation.Enabled = true;
            txtContactPerson.Enabled = true;
            txtContactNo.Enabled = true;
            txtEmail.Enabled = true;
            txtAddress.Enabled = true;
            txtPostalCode.Enabled = true;
        }

        private bool IsValid()
        {
            if (txtCompanyName.Text == "")
            {
                MessageBox.Show("Please enter Company Name!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtOfficeLocation.Text == "")
            {
                MessageBox.Show("Please enter company office location!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtContactPerson.Text == "")
            {
                MessageBox.Show("Please enter Contact Person Name!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtContactNo.Text == "")
            {
                MessageBox.Show("Please enter contact number!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtContactNo.Text.Length != 11)
            {
                MessageBox.Show("Contact Number must be in 11 digit", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Please enter e-mail address!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter address", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            else if (txtPostalCode.Text == "")
            {
                MessageBox.Show("Please enter postal code!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                if (MessageBox.Show("Do you want to save update ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (IsValid())
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE tbl_suppliers SET companyName=@company,officeLocation=@office,contactPersonName=@contactPerson,contactNo=@contactNo,email=@email,streetAddress=@address,postalCode=@postalCode WHERE suppliersID=@suppID", connection);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@suppID", txtSuppliersID.Text);
                        cmd.Parameters.AddWithValue("@company", txtCompanyName.Text);
                        cmd.Parameters.AddWithValue("@office", txtOfficeLocation.Text);
                        cmd.Parameters.AddWithValue("@contactPerson", txtContactPerson.Text);
                        cmd.Parameters.AddWithValue("@contactNo", txtContactNo.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@postalCode", txtPostalCode.Text);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data has been updated successfully!", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearAll();
                        LoadSuppliers();
                        LoadDefault();
                    }
                }
                else 
                {
                    MessageBox.Show("Data doeen\'t updated", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data doeen\'t updated", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();

            }
        }

    }
}

