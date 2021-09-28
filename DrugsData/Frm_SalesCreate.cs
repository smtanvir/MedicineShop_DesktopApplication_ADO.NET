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
using System.IO;

namespace DrugsData
{

    public partial class Frm_SalesInsert : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner; Trusted_Connection=true;");

        public enum PackSizeUnit
        {
            Pcs,
            Box,
            Cartoon
        }
        public Frm_SalesInsert()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Frm_SalesInsert_Load(object sender, EventArgs e)
        {
            txtTransaction.Enabled = false;
            dtp_SalesDate.Enabled = false;
            txtCustomerID.Enabled = false;
            txtCustomerContact.Enabled = false;
            txtCustomerAddress.Enabled = false;

            txtMedicineID.Enabled = false;
            txtUnitPrice.Enabled = false;
            txtStock.Enabled = false;

            txtEmpID.Enabled = false;


            LoadNewTxnID();
            LoadMedicineInfo();
            LoadCustomerName();
            LoadSalespersonName();
            LoadSizeUnit();
        }

        private void LoadSizeUnit()
        {
            string[] packSizeUnit = Enum.GetNames(typeof(PackSizeUnit));
            foreach (var psu in packSizeUnit)
            {
                cmbSizeUnit.Items.Add(psu);
            }
            cmbSizeUnit.SelectedIndex = (int)PackSizeUnit.Pcs;
        }

        private void LoadSalespersonName()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT employeeID,firstName from tbl_employees", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            cmbEmpName.ValueMember = "employeeID";
            cmbEmpName.DisplayMember = "firstName";
            cmbEmpName.DataSource = dt;
        }

        private void LoadCustomerName()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT customerID, firstName FROM tbl_customers", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            cmbCustomerName.ValueMember = "customerID";
            cmbCustomerName.DisplayMember = "firstName";
            cmbCustomerName.DataSource = dt;
        }

        private void LoadNewTxnID()
        {
            SqlCommand cmd = new SqlCommand("SELECT MAX(salesID)+1 FROM tbl_sales", connection);
            connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                txtTransaction.Text = dr[0].ToString();
            }
            connection.Close();
        }

        private void LoadMedicineInfo()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT medicineName FROM tbl_Medicine", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            cmbMedicineName.ValueMember = "medicineID";
            cmbMedicineName.DisplayMember = "medicineName";
            cmbMedicineName.DataSource = dt;
        }

        private void cmbMedicineName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT m.medicineID,m.unitPrice,s.quantity FROM tbl_Medicine m INNER JOIN tbl_stock s  ON m.medicineID=s.medicineID WHERE m.medicineName=@medicine", connection);
            cmd.Parameters.AddWithValue("@medicine", cmbMedicineName.Text);

            connection.Open();

            SqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.Read())
            {
                txtMedicineID.Text = dataReader[0].ToString();
                txtUnitPrice.Text = dataReader[1].ToString();
                txtStock.Text = dataReader[2].ToString();
            }
            connection.Close();
        }

        private void cmbCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT customerID, contactNo,streetAddress FROM tbl_customers WHERE firstName=@fname ", connection);
            cmd.Parameters.AddWithValue("@fname", cmbCustomerName.Text);

            connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtCustomerID.Text = dr[0].ToString();
                txtCustomerContact.Text = dr[1].ToString();
                txtCustomerAddress.Text = dr[2].ToString();
            }
            connection.Close();

        }

        private void cmbEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT employeeID,firstName from tbl_employees  WHERE firstName=@fname ", connection);
            cmd.Parameters.AddWithValue("@fname", cmbEmpName.Text);

            connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtEmpID.Text = dr[0].ToString();
            }
            connection.Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

            try
            {

                if (MessageBox.Show("Do you save data ?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("EXEC sp_AddMedicineToSales @customerID,@medicineID,@quantity,@employeeID", connection);
                    
                    cmd.Parameters.AddWithValue("@customerID", txtCustomerID.Text);
                    cmd.Parameters.AddWithValue("@medicineID", txtMedicineID.Text);
                    cmd.Parameters.AddWithValue("@quantity", txtSalesQuantity.Text);
                    cmd.Parameters.AddWithValue("@employeeID", txtEmpID.Text);
                   
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
            catch (Exception)
            {

                MessageBox.Show("Data doesn\'t saved!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connection.Close();
            }
        }

        private void ResetAll()
        {
            LoadNewTxnID();
            LoadMedicineInfo();
            LoadCustomerName();
            LoadSalespersonName();
            LoadSizeUnit();
        }
    }
}
