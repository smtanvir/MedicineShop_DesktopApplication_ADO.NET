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
using System.Drawing.Imaging;

namespace DrugsData
{
    public partial class Frm_Employee_Update : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner; Trusted_Connection=true;");
        string imgloc = "";
        public Frm_Employee_Update()
        {
            InitializeComponent();
        }

        private void Frm_Update_Employee_Load(object sender, EventArgs e)
        {
            txtEmployeeID.Enabled = false;
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtNID.Enabled = false;
            dtp_DOB.Enabled = false;
            txtPostalCode.Enabled = false;
            txtEmail.Enabled = false;
            txtContact.Enabled = false;
            txtAddress.Enabled = false;
            txtCity.Enabled = false;
            txtPostalCode.Enabled = false;
            cmbGender.Enabled = false;
            cmbDepartment.Enabled = false;
            cmbDesignation.Enabled = false;

            btnUpdate.Hide();
            btnUpdate.Enabled = false;
            btnEdit.Enabled = false;
            btnBrowse.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text != "")
            {
                SearchValidData();
            }
            else
            {
                MessageBox.Show("You haven't search anything!!!", "Failed", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
            }
            btnEdit.Enabled = true;
        }
        private bool SearchValidData()
        {
            if (txtEmployeeID.Text == "" && txtFirstName.Text == "" && txtLastName.Text == "" && txtNID.Text == "" && txtAddress.Text == "" && txtCity.Text == "" && txtContact.Text == "" && txtEmail.Text == "" && txtPostalCode.Text == "" && dtp_DOB.Text == "" && cmbDesignation.Text == "" && cmbGender.Text == "")
            {
                MessageBox.Show("No data found!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                searchData();
            }
            return true;
        }
        private void searchData()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_employees WHERE contactNo=@contact", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@contact", txtSearch.Text);
                LoadDesignation();
                LoadGender();

                connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    txtEmployeeID.Text = dataReader[0].ToString();
                    txtFirstName.Text = dataReader[1].ToString();
                    txtLastName.Text = dataReader[2].ToString();
                    dtp_DOB.Value = (DateTime)dataReader[3];
                    cmbGender.SelectedValue = dataReader[4].ToString();
                    txtNID.Text = dataReader[5].ToString();
                    txtEmail.Text = dataReader[6].ToString();
                    txtContact.Text = dataReader[7].ToString();
                    txtAddress.Text = dataReader[8].ToString();
                    txtPostalCode.Text = dataReader[9].ToString();
                    txtCity.Text = dataReader[10].ToString();
                    cmbDesignation.SelectedValue = dataReader[11].ToString();
                    byte[] img = (byte[])dataReader[12];

                    if (img != null)
                    {
                        MemoryStream imgStream = new MemoryStream(img);
                        pictureBox1.Image = Image.FromStream(imgStream);
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("No data found!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            }

            connection.Close();
        }
        private void LoadDesignation()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT designationID,designation,department FROM tbl_designation", connection);
            DataTable dataTable = new DataTable();
            sda.Fill(dataTable);
            cmbDesignation.ValueMember = "designationID";
            cmbDesignation.DisplayMember = "designation";
            cmbDesignation.DataSource = dataTable;
            cmbDepartment.DisplayMember = "department";
            cmbDepartment.DataSource = dataTable;
        }

        private void LoadGender()
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_gender", connection);
            DataTable dataTable = new DataTable();
            sda.Fill(dataTable);
            cmbGender.ValueMember = "genderID";
            cmbGender.DisplayMember = "gender";
            cmbGender.DataSource = dataTable;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtEmployeeID.Enabled = false;
            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtNID.Enabled = true;
            dtp_DOB.Enabled = true;
            txtPostalCode.Enabled = true;
            txtEmail.Enabled = true;
            txtContact.Enabled = true;
            txtAddress.Enabled = true;
            txtCity.Enabled = true;
            txtPostalCode.Enabled = true;
            cmbGender.Enabled = true;
            cmbDepartment.Enabled = true;
            cmbDesignation.Enabled = true;

            btnEdit.Hide();
            btnUpdate.Show();
            btnUpdate.Enabled = true;
            btnBrowse.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetAll()
        {
            txtEmployeeID.Clear();
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtNID.Text = "";
            txtPostalCode.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtContact.Text = "";
            txtEmail.Text = "";
            cmbDesignation.SelectedIndex = 0;
            cmbGender.SelectedIndex = 0;
            cmbDepartment.SelectedIndex = 0;
            pictureBox1.Image = null;
            dtp_DOB.Value = DateTime.Now;
            txtSearch.Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                byte[] img = null;
                FileStream file = new FileStream(imgloc, FileMode.Open, FileAccess.ReadWrite);
                BinaryReader binaryReader = new BinaryReader(file);
                img = binaryReader.ReadBytes((int)file.Length);

                SqlCommand cmd = new SqlCommand("UPDATE tbl_employees SET firstName=@fname,lastName=@lname,dateOfBirth=@dob,genderID=@gender,nationalID=@nid,email=@email,contactNo=@contact,streetAddress=@address,postalCode=@ZIP,city=@city,designationID=@des,empImage=@image WHERE employeeID=@empID", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@empID", txtEmployeeID.Text);
                cmd.Parameters.AddWithValue("@fname", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@lname", txtLastName.Text);
                cmd.Parameters.AddWithValue("@dob", dtp_DOB.Value);
                cmd.Parameters.AddWithValue("@gender", cmbGender.SelectedValue);
                cmd.Parameters.AddWithValue("@nid", txtNID.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@contact", txtContact.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@ZIP", txtPostalCode.Text);
                cmd.Parameters.AddWithValue("@city", txtCity.Text);
                cmd.Parameters.AddWithValue("@des", cmbDesignation.SelectedValue);
                cmd.Parameters.AddWithValue("@image", img);

                connection.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data updated successfull", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connection.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Data updated failed!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|All Files(*.*)|*.*";
            openFile.Title = "Select employee new picture";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                imgloc = openFile.FileName;
                pictureBox1.ImageLocation = imgloc;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.InitializeComponent();
        }
    }
}
