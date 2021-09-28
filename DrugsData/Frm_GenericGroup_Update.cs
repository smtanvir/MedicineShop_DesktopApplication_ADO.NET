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
    public partial class Frm_GenericGroup_Update : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Integrated Security=true");

        int indexRow;
        public Frm_GenericGroup_Update()
        {
            InitializeComponent();
        }

        private void Frm_GenericGroup_View_Load(object sender, EventArgs e)
        {
            ShowData();
            Default();
        }


        private void dataGridView_ShowData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            DataGridViewRow row = dataGridView_ShowData.Rows[indexRow];

            txtGroupCode.Text = row.Cells[0].Value.ToString();
            txtGroup.Text = row.Cells[1].Value.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnEdit.Hide();
            btnUpdate.Show();
            btnClear.Enabled = true;
            txtGroup.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValid())
                {
                    if (txtGroupCode.Text != "")
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE tbl_genericGroup SET genericGroup=@genericGroup WHERE genericGroupCode=@gnCode", connection);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@gnCode", txtGroupCode.Text);
                        cmd.Parameters.AddWithValue("@genericGroup", txtGroup.Text);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Data updated successfully", "Update Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Default();
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data updated failed", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
                ShowData();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text == "")
                {
                    MessageBox.Show("You haven\'t search anything!!!", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_genericGroup WHERE genericGroupCode=@gnCode", connection);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@gnCode", txtSearch.Text);

                    connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        txtGroupCode.Text = dr[0].ToString();
                        txtGroup.Text = dr[1].ToString();
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data doesn\'t found!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            finally
            {
                txtSearch.Clear();
                connection.Close();

                btnUpdate.Hide();
                btnEdit.Show();
            }
        }

        private void Default()
        {
            btnUpdate.Hide();
            btnEdit.Show();
            btnClear.Enabled = false;
            txtGroupCode.Enabled = false;
            txtGroup.Enabled = false;
            txtGroup.Clear();
            txtGroupCode.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtGroup.Clear();
            txtGroupCode.Clear();
        }

        private bool IsValid()
        {
            if (txtGroup.Text == "")
            {
                MessageBox.Show("Please write new Generic Group", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ShowData()
        {
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT genericGroupCode [Generic Group Code],genericGroup [Drugs Generic Group] FROM tbl_genericGroup", connection);
            DataTable dataTable = new DataTable();
            sqlData.Fill(dataTable);
            dataGridView_ShowData.DataSource = dataTable;
        }
    }
}
