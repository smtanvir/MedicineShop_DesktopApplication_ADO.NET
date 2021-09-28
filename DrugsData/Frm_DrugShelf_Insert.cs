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
    public partial class Frm_DrugShelf_Insert : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Integrated Security=true;");
        public Frm_DrugShelf_Insert()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetAll();
        }

        private void Frm_DrugShelf_Insert_Load(object sender, EventArgs e)
        {

            txtShelfCode.Enabled = false;
            LoadShelfCode();
        }
        private void LoadShelfCode()
        {
            SqlCommand cmd = new SqlCommand("SELECT MAX(shelfID)+1 FROM tbl_drugShelf", connection);
            cmd.CommandType = CommandType.Text;
            connection.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.Read())
            {
                txtShelfCode.Text = dataReader[0].ToString();
            }
            connection.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbl_drugShelf (shelfNumber)VALUES('" + txtShelfNumber.Text + "')", connection);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data inserted successfully!!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                    ResetAll();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data doesn\'t saved!!!\nAlready exist or wrong format!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }

        private void ResetAll()
        {
            txtShelfNumber.Clear();
            LoadShelfCode();
        }

        private bool IsValid()
        {
            if (txtShelfNumber.Text == "")
            {
                MessageBox.Show("Please enter Drug\'s Shelf Number", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                return false;
            }

            if (txtShelfNumber.Text.Length != 5)
            {
                MessageBox.Show("Please enter Shelf Number like as: A-001", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
    }
}
