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
    public partial class Frm_EmployeesView : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Integrated Security=true");

        //int indexRow;

        public Frm_EmployeesView()
        {
            InitializeComponent();
        }

        private void Frm_EmployeesView_Load(object sender, EventArgs e)
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT e.employeeID [Employee ID], e.firstName+' '+e.lastName [Employee Name], e.dateOfBirth [Date of Birth], g.gender [Gender], e.nationalID [National ID Card],e.email[E-mail],e.contactNo[Contact No],e.streetAddress[Present Address],e.postalCode[Postal Code],e.city[City],d.designation[Designation],e.empImage[Employee Image]  FROM tbl_employees e INNER JOIN tbl_gender g ON e.genderID = g.genderID INNER JOIN tbl_designation d ON e.designationID = d.designationID", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            //dt.Columns.Add("EmployeeImage", Type.GetType("System.Byte[]"));

            //foreach (DataRow row in dt.Rows)
            //{
            //    row["EmployeeImage"] = File.ReadAllBytes(row["empImage"].ToString());
            //}
            
            dataGridView1.DataSource = dt;

        }

    }
}
