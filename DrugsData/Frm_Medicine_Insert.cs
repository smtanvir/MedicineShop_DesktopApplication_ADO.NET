using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DrugsData
{
    public partial class FrmAddNewMedicine : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Trusted_Connection=true");
        string imgloc = "";
        public enum PackSizeUnit
        {
            Pcs,
            Box,
            Cartoon
        }
        public FrmAddNewMedicine()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e) => Close();

        private void FrmAddNewMedicine_Load(object sender, EventArgs e)
        {
            LoadPackSizeUnit();
            LoadGenericGroup();
            LoadSupplierCompany();
            LoadShelfNo();
        }

        private void LoadPackSizeUnit()
        {
            string[] packSizeUnit = Enum.GetNames(typeof(PackSizeUnit));
            foreach (var psu in packSizeUnit)
            {
                cmb_Medicine_SizeUnit.Items.Add(psu);
                cmb_Stock_QtyUnit.Items.Add(psu);
            }
            cmb_Medicine_SizeUnit.SelectedIndex = (int)PackSizeUnit.Pcs;
            cmb_Stock_QtyUnit.SelectedIndex = (int)PackSizeUnit.Pcs;
        }

        private void LoadGenericGroup()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_genericGroup ", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            cmb_MedicineGenericGroup.ValueMember = "genericGroupCode";
            cmb_MedicineGenericGroup.DisplayMember = "genericGroup";
            cmb_MedicineGenericGroup.DataSource = dt;
        }

        private void LoadSupplierCompany()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT suppliersID,companyName FROM tbl_suppliers", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            cmb_MedicineCompanyName.ValueMember = "suppliersID";
            cmb_MedicineCompanyName.DisplayMember = "companyName";
            cmb_MedicineCompanyName.DataSource = dt;
        }

        private void LoadShelfNo()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT shelfID,shelfNumber FROM tbl_drugShelf", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            cmb_Medicine_ShelfNo.ValueMember = "shelfID";
            cmb_Medicine_ShelfNo.DisplayMember = "shelfNumber";
            cmb_Medicine_ShelfNo.DataSource = dt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    if (MessageBox.Show("Do you save data ?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("EXEC sp_InsertMedicineWithStock @medicineName,@genericGroupID,@supplierCompanyID,@packSize,@sizeUnit,@shelfID,@unitPrice,@discountRate,@image,@initialStock,@quantityUnit", connection);

                        byte[] img = null;
                        FileStream fs = new FileStream(imgloc, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        img = br.ReadBytes((int)fs.Length);

                        cmd.Parameters.AddWithValue("@medicineName", txtMedicineName.Text);
                        cmd.Parameters.AddWithValue("@genericGroupID", cmb_MedicineGenericGroup.SelectedValue);
                        cmd.Parameters.AddWithValue("@supplierCompanyID", cmb_MedicineCompanyName.SelectedValue);
                        cmd.Parameters.AddWithValue("@packSize", txtMedicinePackSize.Text);
                        cmd.Parameters.AddWithValue("@sizeUnit", cmb_Medicine_SizeUnit.Text);
                        cmd.Parameters.AddWithValue("@shelfID", cmb_Medicine_ShelfNo.SelectedValue);
                        cmd.Parameters.AddWithValue("@unitPrice", txtMedicine_UnitPrice.Text);
                        cmd.Parameters.AddWithValue("@discountRate", txtMedicine_DiscountRate.Text);
                        cmd.Parameters.AddWithValue("@image", img);
                        cmd.Parameters.AddWithValue("@initialStock", txtStock_QtyStock.Text);
                        cmd.Parameters.AddWithValue("@quantityUnit", cmb_Stock_QtyUnit.Text);

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

                MessageBox.Show("Data doesn\'t saved!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            openFile.Title = "Select Employee Picture";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                imgloc = openFile.FileName.ToString();
                pictureBox_MedicineProduct.ImageLocation = imgloc;
            }
            lblPictuer.Hide();
        }

        private void ResetAll()
        {
            txtMedicineName.Clear();
            txtMedicinePackSize.Clear();
            txtMedicine_DiscountRate.Clear();
            txtMedicine_UnitPrice.Clear();
            txtStock_QtyStock.Clear();
            cmb_MedicineCompanyName.SelectedIndex = 0;
            cmb_MedicineGenericGroup.SelectedIndex = 0;
            cmb_Medicine_ShelfNo.SelectedIndex = 0;
            cmb_Medicine_SizeUnit.SelectedIndex = 0;
            cmb_Stock_QtyUnit.SelectedIndex = 0;
            pictureBox_MedicineProduct.Image = null;
        }

        private bool IsValid()
        {
            if (txtMedicineName.Text == "")
            {
                MessageBox.Show("Please enter Medicine name!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
           
            if (cmb_MedicineGenericGroup.Text == "")
            {
                MessageBox.Show("Please choose Medicine Generic Group!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (cmb_MedicineCompanyName.Text == "")
            {
                MessageBox.Show("Please chosse Company Name!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtMedicinePackSize.Text == "")
            {
                MessageBox.Show("Please enter Pack Size!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (cmb_Medicine_SizeUnit.Text == "")
            {
                MessageBox.Show("Please chosse Medicine Pack Size Unit!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (cmb_Medicine_ShelfNo.Text == "")
            {
                MessageBox.Show("Please chosse Shelf No!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtMedicine_UnitPrice.Text == "")
            {
                MessageBox.Show("Please enter per unit price!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtMedicine_DiscountRate.Text == "")
            {
                MessageBox.Show("Please enter discount rate!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtStock_QtyStock.Text == "")
            {
                MessageBox.Show("Please enter Quantity of Medicine!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (cmb_Stock_QtyUnit.Text == "")
            {
                MessageBox.Show("Please chosse Medicine Pack Size Unit!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
    }
}
