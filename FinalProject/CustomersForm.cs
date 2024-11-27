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
    public partial class CustomersForm : Form
    {
        string connectionString = "server=.\\SQLEXPRESS;user id=wajd;password=123456";
        SqlConnection connection;
        string oldCustomerID = "";

        public CustomersForm()
        {
            InitializeComponent();
        }

        private void CustomerLoad()
        {
            string sql = "SELECT * FROM Customers " ;
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;


        }
        private void CustomersForm_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionString);

            connection.Open();
            CustomerLoad();

        }

        private void CustomersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ADD a Customer

            string sql = "INSERT INTO Customers VALUES (@CompanyName,@Address,@Phone,@Email)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CompanyName", textBox2.Text);
            command.Parameters.AddWithValue("@Address", textBox3.Text);
            command.Parameters.AddWithValue("@Phone", textBox4.Text);
            command.Parameters.AddWithValue("@Email", textBox5.Text);
            command.ExecuteNonQuery();
            CustomerLoad();
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            ClearAll();


        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Load into fields to update  

            label5.Text = dataGridView1.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString();
            oldCustomerID = label5.Text;

            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["CompanyName"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Address"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
          
        }
        private void ClearAll()
        {
            oldCustomerID = "";
            label5.Text = "______________";
                textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text =  "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //update row    

            if (oldCustomerID == "")
            {
                MessageBox.Show("Please Double-click a row to Update it");
                return;

            }

            string sql = "UPDATE Customers " +
                        " SET    CompanyName=@CompanyName, " +
                        "        Address=@Address, " +
                        "        Phone=@Phone, " +
                        "        Email=@Email " +
                        " Where  CustomerID=@oldCustomerID ";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CompanyName", textBox2.Text);
                command.Parameters.AddWithValue("@Address", textBox3.Text);
                command.Parameters.AddWithValue("@Phone", textBox4.Text);
                command.Parameters.AddWithValue("@Email", textBox5.Text);
                command.Parameters.AddWithValue("@oldCustomerID", oldCustomerID);

                command.ExecuteNonQuery();
                CustomerLoad();

                ClearAll();

            }

            catch (Exception error)
            {

         MessageBox.Show(error.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Delete a record

            if (oldCustomerID == "")
            {
                MessageBox.Show("Please Double-click a row to select it");
                return;

            }

            string sql = "DELETE FROM Customers " +
                        "Where CustomerID=@oldCustomerID";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@oldCustomerID", oldCustomerID);
                command.ExecuteNonQuery();

                CustomerLoad();  
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
