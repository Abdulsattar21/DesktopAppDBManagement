using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Main : Form
    {
       

        public Main()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.ShowDialog();

        }

        private void productsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProductsForm productsForm = new ProductsForm();
            productsForm.ShowDialog();

        }

        private void stockToolStripMenuItem1_Click(object sender, EventArgs e)
        {
          

        }

        private void shippersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShippersForm shippersForm = new ShippersForm();
            shippersForm.ShowDialog();
        }

        private void categoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CategoryForm categoryForm = new CategoryForm();
            categoryForm.ShowDialog();

        }

        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DealsForm dealsForm = new DealsForm();
            dealsForm.ShowDialog();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomersForm customerForm = new CustomersForm();
            customerForm.ShowDialog();

        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeesForm employeesForm = new EmployeesForm();
            employeesForm.ShowDialog();

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void Main_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
