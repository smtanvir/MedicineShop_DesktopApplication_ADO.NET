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
    public partial class Frm_Employee_Insert : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner; Trusted_Connection=true;");
        string imgloc = "";
        public Frm_Employee_Insert()
        {
            InitializeComponent();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    if (MessageBox.Show("Do you save data ?", "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO tbl_employees (firstName,lastName,dateOfBirth,genderID,nationalID,email,contactNo,streetAddress,postalCode,city,designationID,empImage) VALUES (@fname,@lname,@dob,@gender,@nid,@email,@contact,@address,@zip,@city,@des,@img)", connection);
                        cmd.CommandType = CommandType.Text;


                        int genderID;
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
                            throw new Exception("You have to must select Gender!!!");
                        }

                        byte[] img = null;
                        FileStream fs = new FileStream(imgloc, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);
                        img = br.ReadBytes((int)fs.Length);


                        cmd.Parameters.AddWithValue("@fname", txtFirstName.Text);
                        cmd.Parameters.AddWithValue("@lname", txtLastName.Text);
                        cmd.Parameters.AddWithValue("@dob", dtp_Date_Of_Birth.Value);
                        cmd.Parameters.AddWithValue("@gender", genderID);
                        cmd.Parameters.AddWithValue("@nid", txtNationalID.Text);
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@contact", txtContact.Text);
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("@zip", txtPostalCode.Text);
                        cmd.Parameters.AddWithValue("@city", txtCity.Text);
                        cmd.Parameters.AddWithValue("@des", cmbDesignation.SelectedValue);
                        cmd.Parameters.Add(new SqlParameter("@img", img));

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data inserted successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetAll();
                    }
                    else
                    {
                        MessageBox.Show("Data haven't saved!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    }
                }

                
            }
            catch (Exception)
            {

                MessageBox.Show("Data haven't saved!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
            }

            finally
            {
                connection.Close();
            }
        }

        private void Frm_Add_New_Employee_Load(object sender, EventArgs e)
        {
            LoadDesignation();
        }

        private void LoadDesignation()
        {
            SqlDataAdapter dataReader = new SqlDataAdapter("SELECT designationID,designation,department FROM tbl_designation", connection);
            DataTable dataTable = new DataTable();
            dataReader.Fill(dataTable);
            cmbDesignation.ValueMember = "designationID";
            cmbDesignation.DisplayMember = "designation";
            cmbDesignation.DataSource = dataTable;
            cmbDepartment.DisplayMember = "department";
            cmbDepartment.DataSource = dataTable;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JPG Files(*.jpg)|*.jpg|PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            openFile.Title = "Select Employee Picture";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                imgloc = openFile.FileName.ToString();
                pictureBox_Employee.ImageLocation = imgloc;
            }
            lblPictuer.Hide();
        }

        private void ResetAll()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtNationalID.Text = "";
            txtPostalCode.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtContact.Text = "";
            txtEmail.Text = "";
            cmbDesignation.SelectedIndex = 0;
            rdFemale.Checked = false;
            rdMale.Checked = false;
            cmbDepartment.SelectedIndex = 0;
            pictureBox_Employee.Image = null;
            dtp_Date_Of_Birth.Value = DateTime.Now;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        private bool IsValid()
        {
            if (txtFirstName.Text == "")
            {
                MessageBox.Show("Please enter Employee First name!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtLastName.Text == "")
            {
                MessageBox.Show("Please enter Employee Last name!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtNationalID.Text == "")
            {
                MessageBox.Show("Please enter National ID Card No!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (dtp_Date_Of_Birth.Value == DateTime.Now)
            {
                MessageBox.Show("Please choose Date of Birth!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtEmail.Text == "")
            {
                MessageBox.Show("Please enter e-mail address!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtContact.Text == "")
            {
                MessageBox.Show("Please enter contact number!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtContact.Text.Length !=11)
            {
                MessageBox.Show("Please contact number must be 11 digit!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter Employee address!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtPostalCode.Text == "")
            {
                MessageBox.Show("Please enter Postal Code!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtPostalCode.Text.Length !=4)
            {
                MessageBox.Show("Postal Code must be in 4 digit!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (txtCity.Text == "")
            {
                MessageBox.Show("Please enter city!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            if (cmbDesignation.Text == "")
            {
                MessageBox.Show("Please choose Employee designation!!!", "Insert Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
