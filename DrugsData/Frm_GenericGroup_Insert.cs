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
    public partial class Frm_GenericGroup_Insert : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Integrated Security=true");
        public Frm_GenericGroup_Insert()
        {
            InitializeComponent();
        }

        private void Frm_GenericGroup_Insert_Load(object sender, EventArgs e)
        {
            txtGroupCode.Enabled = false;
            LoadGenericGroup();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtGroup.Clear();
        }

        private bool IsValid()
        {
            if (txtGroup.Text == "")
            {
                MessageBox.Show("Please enter Medicine Generic Group!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
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
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbl_genericGroup (genericGroup) VALUES('"+txtGroup.Text+"')",connection);
                    cmd.CommandType = CommandType.Text;
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data inserted successfully!!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                    txtGroup.Clear();
                    LoadGenericGroup();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data doesn\'t saved!!!", "Failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            }
        }
        private void LoadGenericGroup()
        {
            SqlCommand cmd = new SqlCommand("SELECT MAX(genericGroupCode)+1  FROM tbl_genericGroup", connection);
            cmd.CommandType = CommandType.Text;
            connection.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.Read())
            {
                txtGroupCode.Text = dataReader[0].ToString();
            }
            connection.Close();
        }
    }
}
