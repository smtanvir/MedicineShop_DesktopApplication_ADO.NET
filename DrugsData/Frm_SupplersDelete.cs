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
    public partial class Frm_SupplersDelete : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Trusted_Connection=true");
        int indexRow;
        public Frm_SupplersDelete()
        {
            InitializeComponent();
        }

        private void Frm_SupplersDelete_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
            LoadDefault();
            txtSuppliersID.Enabled = false;
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

            btnRemove.Enabled = true;
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
            btnRemove.Enabled = false;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to delete record ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM tbl_suppliers WHERE suppliersID=@supplierID", connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@supplierID", txtSuppliersID.Text);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    LoadSuppliers();
                    
                    MessageBox.Show("Record has been deleted!!!", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Record doesn\'t deleted!!!", "Failed to Delete", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                ClearAll();
            }
            catch (Exception)
            {

                MessageBox.Show("Record doesn\'t deleted!!!", "Failed to Delete", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                connection.Close();
            }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
