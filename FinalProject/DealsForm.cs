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
    public partial class DealsForm : Form
    {
        string connectionString = "server=.\\SQLEXPRESS;user id=wajd;password=123456";
        SqlConnection connection;

        string oldDealID = "";

        public DealsForm()
        {
            InitializeComponent();
        }

        private void DealsLoad()
        {
            string sql = "SELECT DealID,Customers.CompanyName,ProductName,ShippingCompanies.CompanyName,"                +
                         "EmployeeName,DateOpened,DateClosed,DealCost  FROM Deals "                                      +
                         "left JOIN Customers ON Deals.CustomerID = Customers.CustomerID "                               +
                         "left JOIN Products ON Deals.ProductCode = Products.ProductCode "                               +
                         "left JOIN ShippingCompanies ON Deals.ShippingCompanyID = ShippingCompanies.ShippingCompanyID " +
                         "left JOIN Employees ON Deals.ResponsibleEmployeeID = Employees.EmployeeID ";

            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void ComboBox1LoadList()
        {
            string sql = "SELECT ProductCode,ProductName FROM Products ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "ProductName";
            comboBox1.ValueMember = "ProductCode";
        }

        private void ComboBox2LoadList() 
        {
            string sql = "SELECT ShippingCompanyID, CompanyName FROM ShippingCompanies ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "CompanyName";
            comboBox2.ValueMember = "ShippingCompanyID";
        }

        private void ComboBox3LoadList()
        {
            string sql = "SELECT EmployeeID, EmployeeName FROM Employees ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "EmployeeName";
            comboBox3.ValueMember = "EmployeeID";
        }

        private void ComboBox4LoadList()
        {
            string sql = "SELECT CustomerID, CompanyName FROM Customers ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            comboBox4.DataSource = dt;
            comboBox4.DisplayMember = "CompanyName";
            comboBox4.ValueMember = "CustomerID";
        }

        private void DealsForm_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionString);

            connection.Open();
            DealsLoad();
            ComboBox1LoadList();
            ComboBox2LoadList();
            ComboBox3LoadList();
            ComboBox4LoadList();

        }

        private void DealsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ADD a Deal


           string sql = "INSERT INTO Deals VALUES "  +
                        "(@CustomerID,"              +
                        "@ProductCode,"              +
                        "@ShippingCompanyID,"        +
                        "@DateOpened,"               +
                        "@DateClosed,"               +
                        "@ResponsibleEmployeeID,"    +
                        "@DealCost)";

            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@CustomerID",              comboBox4.SelectedValue);   
            command.Parameters.AddWithValue("@ProductCode",             comboBox1.SelectedValue);
            command.Parameters.AddWithValue("@ShippingCompanyID", comboBox2.SelectedValue);

            command.Parameters.AddWithValue("@DateOpened",              dateTimePicker1.Value);
            command.Parameters.AddWithValue("@DateClosed",              dateTimePicker2.Value);
            command.Parameters.AddWithValue("@ResponsibleEmployeeID",   comboBox3.SelectedValue);
            command.Parameters.AddWithValue("@DealCost",                textBox5.Text);

            command.ExecuteNonQuery();
            DealsLoad();
            ClearAll();
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Load into fields to update  
            string sql = "SELECT * FROM Deals ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView2.DataSource = dt;



            label5.Text = dataGridView2.Rows[e.RowIndex].Cells["DealID"].Value.ToString();
            oldDealID = label5.Text;

            textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells["DealCost"].Value.ToString();
            comboBox1.SelectedValue = dataGridView2.Rows[e.RowIndex].Cells["ProductCode"].Value.ToString();
            comboBox4.SelectedValue = dataGridView2.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString();
            comboBox2.SelectedValue = dataGridView2.Rows[e.RowIndex].Cells["ShippingCompanyID"].Value.ToString();
            comboBox3.SelectedValue = dataGridView2.Rows[e.RowIndex].Cells["ResponsibleEmployeeID"].Value.ToString();
            String dateOpened = dataGridView2.Rows[e.RowIndex].Cells["DateOpened"].Value.ToString();
            String dateClosed = dataGridView2.Rows[e.RowIndex].Cells["DateClosed"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dateOpened);
            dateTimePicker2.Value = Convert.ToDateTime(dateClosed);
        }
        private void ClearAll()
        {
            oldDealID = "";
            label5.Text = "______________";
            textBox5.Text = "";
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = DateTime.Now.Date;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //update row    

            if (oldDealID == "")
            {
                MessageBox.Show("Please Double-click a row to Update it");
                return;

            }

            string sql = "UPDATE Deals "                                    +
                        "SET CustomerID=@CustomerID, " +
                        "    ProductCode=@ProductCode, "                    +
                        "    ShippingCompanyID=@ShippingCompanyID, "        +
                        "    DateOpened=@DateOpened, "                      +
                        "    DateClosed=@DateClosed, "                      +
                        "    ResponsibleEmployeeID=@ResponsibleEmployeeID, "+
                        "    DealCost=@DealCost "                          +
                        " Where DealID=@oldDealID ";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@CustomerID",            comboBox4.SelectedValue);
                    command.Parameters.AddWithValue("@ProductCode",           comboBox1.SelectedValue);
                    command.Parameters.AddWithValue("@ShippingCompanyID",     comboBox2.SelectedValue);

                    command.Parameters.AddWithValue("@DateOpened",            dateTimePicker1.Value);
                    command.Parameters.AddWithValue("@DateClosed",            dateTimePicker2.Value);
                    command.Parameters.AddWithValue("@ResponsibleEmployeeID", comboBox3.SelectedValue);
                    command.Parameters.AddWithValue("@DealCost",              textBox5.Text);
                    command.Parameters.AddWithValue("@oldDealID",             oldDealID);
                
                command.ExecuteNonQuery();
              DealsLoad();
                ClearAll();
                
            }
            catch (Exception error)
            {
                
                if (error.Message.IndexOf("FK_") > -1) //if FK-Related error Found
                    MessageBox.Show("update this row failed because it is linked to another table. Try to delete the related row first");
                MessageBox.Show(error.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Delete a record

            if (oldDealID == "")
            {
                MessageBox.Show("Please Double-click a row to select it");
                return;

            }

            string sql = "DELETE FROM Deals " +
                        "Where DealID=@oldDealID";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@oldDealID", oldDealID);
                command.ExecuteNonQuery();

                DealsLoad();
                ClearAll();
            }

            catch (Exception error)
            {
                if (error.Message.IndexOf("FK_") > -1) //if FK-Related error Found
                    MessageBox.Show("update this row failed because it is linked to another table. Try to delete the related row first");
                MessageBox.Show(error.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
