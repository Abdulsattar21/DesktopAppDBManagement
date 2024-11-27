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
    public partial class ShippersForm : Form
    {
        string connectionString = "server=.\\SQLEXPRESS;user id=wajd;password=123456";
        SqlConnection connection;

       string  oldShippingCompanyID=""    ;

        public ShippersForm()
        {
            InitializeComponent();
        }

        private void ShippersLoad()
        {
            string sql = "SELECT * FROM ShippingCompanies ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void ShippersForm_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionString);

            connection.Open();
            ShippersLoad();
        }


        private void ShippersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();

        }

        private void ClearAll()
        {
            oldShippingCompanyID = "";
            label5.Text = "______________";
            textBox2.Text = textBox3.Text = textBox4.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ADD a company

            string sql = "INSERT INTO ShippingCompanies VALUES (@CompanyName,@Address,@Phone)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CompanyName", textBox2.Text);
            command.Parameters.AddWithValue("@Address", textBox3.Text);
            command.Parameters.AddWithValue("@Phone", textBox4.Text);
            command.ExecuteNonQuery();
            ShippersLoad();
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Delete a record

            if (oldShippingCompanyID == "")
            {
                MessageBox.Show("Please Double-click a row to select it");
                return;

            }

            string sql = "DELETE FROM ShippingCompanies " +
                        "Where ShippingCompanyID=@oldShippingCompanyID";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@oldShippingCompanyID", oldShippingCompanyID);

                command.ExecuteNonQuery();

                ShippersLoad();

                ClearAll();





            }
            catch (Exception error)
            {
                if (error.Message.IndexOf("FK_") > -1) //if FK-Related error Found
                    MessageBox.Show("update this row failed because it is linked to another table. Try to delete the related row first");
                MessageBox.Show(error.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            //Load into fields to update  

            label5.Text = dataGridView1.Rows[e.RowIndex].Cells["ShippingCompanyID"].Value.ToString();
            oldShippingCompanyID = label5.Text;

            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["CompanyName"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Address"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Phone"].Value.ToString();

        }

        private void button3_Click(object sender, EventArgs e)
        {

            //update row    

            if (oldShippingCompanyID == "")
            {
                MessageBox.Show("Please Double-click a row to Edit it");
                return;

            }

            string sql = "UPDATE ShippingCompanies "            +
                        " SET    CompanyName=@CompanyName, "    +
                        "        Address=@Address, "             +
                        "        Phone=@Phone "                 +
                        " Where  ShippingCompanyID=@oldShippingCompanyID ";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CompanyName", textBox2.Text);
                command.Parameters.AddWithValue("@Address", textBox3.Text);
                command.Parameters.AddWithValue("@Phone", textBox4.Text);
                command.Parameters.AddWithValue("@oldShippingCompanyID", oldShippingCompanyID);

                command.ExecuteNonQuery();
                ShippersLoad();
                ClearAll();

            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
               
            }
        }
    }
}
