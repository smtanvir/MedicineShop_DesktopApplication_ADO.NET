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
    public partial class Frm_Main : Form
    {
        public class DBconnection
        {
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=MS_Medicine_Corner;Integrated Security=True");
        }

        public Frm_Main()
        {
            InitializeComponent();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Frm_Login frmlogin = new Frm_Login();
            frmlogin.Show();
            this.Hide();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_AddNewCustomer newCustomer = new Frm_AddNewCustomer();
            newCustomer.MdiParent = this;
            newCustomer.Show();
        }

        private void AddNewProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAddNewMedicine addMedicine = new FrmAddNewMedicine();
            addMedicine.MdiParent = this;
            addMedicine.Show();
        }

        private void updateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Frm_CustomerUpdate update_Customer = new Frm_CustomerUpdate();
            update_Customer.MdiParent = this;
            update_Customer.Show();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Employee_Insert newEmployee = new Frm_Employee_Insert();
            newEmployee.MdiParent = this;
            newEmployee.Show();


        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Employee_Update update_Employee = new Frm_Employee_Update();
            update_Employee.MdiParent = this;
            update_Employee.Show();
        }

        private void addNewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Frm_Suppliers_Insert suppliers_Insert = new Frm_Suppliers_Insert();
            suppliers_Insert.MdiParent = this;
            suppliers_Insert.Show();
        }

        private void genericGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_GenericGroup_Insert group_Insert = new Frm_GenericGroup_Insert();
            group_Insert.MdiParent = this;
            group_Insert.Show();
        }

        private void drugShelfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_DrugShelf_Insert shelf_Insert = new Frm_DrugShelf_Insert();
            shelf_Insert.MdiParent = this;
            shelf_Insert.Show();
        }

        private void gToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void genericGroupToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Frm_GenericGroup_Update Update = new Frm_GenericGroup_Update();
            Update.MdiParent = this;
            Update.Show();
        }

        private void employeeDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_EmployeesView viewEmp = new Frm_EmployeesView();
            viewEmp.MdiParent = this;
            viewEmp.Show();
        }

        private void updateToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Frm_SupplersUpdate supplersUpdate = new Frm_SupplersUpdate();
            supplersUpdate.MdiParent = this;
            supplersUpdate.Show();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Frm_SupplersDelete supplersDelete = new Frm_SupplersDelete();
            supplersDelete.MdiParent = this;
            supplersDelete.Show();
        }

        private void CreateNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_SalesInsert salesInsert = new Frm_SalesInsert();
            salesInsert.MdiParent = this;
            salesInsert.Show();
        }

        private void medicineStockInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Report_StockInfo medicineStock = new Frm_Report_StockInfo();
            medicineStock.MdiParent = this;
            medicineStock.Show();
        }

        private void salesDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_SalesDetails_View viewSales = new Frm_SalesDetails_View();
            viewSales.MdiParent = this;
            viewSales.Show();
        }
    }
}
