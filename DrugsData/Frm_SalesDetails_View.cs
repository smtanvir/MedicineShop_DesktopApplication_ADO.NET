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
    public partial class Frm_SalesDetails_View : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner; Trusted_Connection=true;");
        public Frm_SalesDetails_View()
        {
            InitializeComponent();
        }

        private void Frm_SalesDetails_View_Load(object sender, EventArgs e)
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT s.salesID [Transaction ID], c.firstName+' '+c.lastName[Customer Name],c.contactNo[Contact No],m.medicineName[Medicine],s.salesDate[Sales Date],s.quantity[Sales Quantity],s.discount[Discount Amount],s.netPrice[Net Payable]  FROM tbl_sales s INNER JOIN tbl_Medicine m ON s.medicineID=m.medicineID INNER JOIN tbl_customers c ON s.cusotmersID=c.customerID", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
