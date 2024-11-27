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
    public partial class ProductsForm : Form
    {
        string connectionString = "server=.\\SQLEXPRESS;user id=wajd;password=123456";
        SqlConnection connection;
        string oldProductCode = "";
        public ProductsForm()
        {
            InitializeComponent();
        }

        private void ProductsLoad()
        {
            string sql = "SELECT * FROM Products ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;

        }
        private void ComboBox1LoadList()
        {     
            string sql = "SELECT CategoryCode,CategoryName FROM Category ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "CategoryName";
            comboBox1.ValueMember = "CategoryCode";
        }
       /* private void ComboBox2LoadList()
        {
        
            DataTable dt = new DataTable();
            dt.Columns.Add("DiscontinuedCode", typeof(string));
            dt.Columns.Add("Discontinued", typeof(string));
            dt.Rows.Add("0", "No");
            dt.Rows.Add("1", "Yes");

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Discontinued";
          //  comboBox1.ValueMember = "DiscontinuedCode";
        }*/

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProductsForm_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionString);

            connection.Open();
            ProductsLoad();
            ComboBox1LoadList();
          

        }

        

        private void ProductsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //ADD a Product
            try
            {
                string sql = "INSERT INTO Products VALUES (@ProductName,@CategoryCode,@UnitsInStock,@SalePrice,@ReleaseDate,@Discontinued)";

                SqlCommand command = new SqlCommand(sql, connection);
                if (comboBox2.Text == "") { 
                MessageBox.Show("please enter a proper value in Discontinued field!", "Error");
                    ProductsLoad();
                    return;
            }
                command.Parameters.AddWithValue("@ProductName"  , textBox1.Text);
                command.Parameters.AddWithValue("@CategoryCode" , comboBox1.SelectedValue);
                command.Parameters.AddWithValue("@UnitsInStock" , textBox2.Text);
                command.Parameters.AddWithValue("@SalePrice"    , textBox3.Text);
                command.Parameters.AddWithValue("@ReleaseDate"  , dateTimePicker1.Value);
                command.Parameters.AddWithValue("@Discontinued" , comboBox2.GetItemText(comboBox2.SelectedItem));

                command.ExecuteNonQuery();    
                ProductsLoad();
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
                ClearAll();

                    //Another way to scrolldown// dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.RowCount-1].Cells[0];
                }
                catch (Exception error)
                {
                MessageBox.Show(error.Message);
                }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //Delete a record

            if (oldProductCode == "")
            {
                MessageBox.Show("Please Double-click a row to select it");
                return;
            }

            string sql = "DELETE FROM Products " +
                        "Where ProductCode=@oldProductCode";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@oldProductCode", oldProductCode);
                command.ExecuteNonQuery();

                ProductsLoad();
                ClearAll();
            }

            catch (Exception error)
            {
                if (error.Message.IndexOf("FK_") > -1) //if FK-Related error Found
                    MessageBox.Show("update this row failed because it is linked to another table. Try to delete the related row first");
                MessageBox.Show(error.Message);

            }
        }
        private void ClearAll()
        {
            oldProductCode = "";
            textBox1.Text = textBox2.Text = textBox3.Text = "";

            dateTimePicker1.Value = DateTime.Now.Date;
     
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Load into fields to update  

            label8.Text = dataGridView1.Rows[e.RowIndex].Cells["ProductCode"].Value.ToString();
            oldProductCode = label8.Text;

            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["UnitsInStock"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["SalePrice"].Value.ToString();

            comboBox1.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells["CategoryCode"].Value.ToString();
            comboBox2.SelectedIndex=comboBox2.FindStringExact(dataGridView1.Rows[e.RowIndex].Cells["Discontinued"].Value.ToString()) ;         

            String releaseDate = dataGridView1.Rows[e.RowIndex].Cells["ReleaseDate"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(releaseDate);
           
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //update row    

            if (oldProductCode == "")
            {
                MessageBox.Show("Please Double-click a row to Update it");
                return;

            }

            string sql = "UPDATE Products " +
                                         " SET ProductName=@ProductName,"   +
                                         "CategoryCode=@CategoryCode, "     +
                                         "UnitsInStock=@UnitsInStock, "     +
                                         "SalePrice=@SalePrice, "           +
                                         "ReleaseDate=@ReleaseDate, "       +
                                         "Discontinued=@Discontinued "      +
                                         " Where ProductCode=@oldProductCode ";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ProductName", textBox1.Text );
                command.Parameters.AddWithValue("@CategoryCode", comboBox1.SelectedValue);
                command.Parameters.AddWithValue("@UnitsInStock", textBox2.Text);
                command.Parameters.AddWithValue("@SalePrice", textBox3.Text);
                command.Parameters.AddWithValue("@ReleaseDate", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@Discontinued", comboBox2.GetItemText(comboBox2.SelectedItem));
                command.Parameters.AddWithValue("@oldProductCode", oldProductCode);
                command.ExecuteNonQuery();

                ProductsLoad();
                ClearAll();

            }
            catch (Exception error)
            {

                if (error.Message.IndexOf("FK_") > -1) //if FK-Related error Found
                    MessageBox.Show("update this row failed because it is linked to another table. Try to delete the related row first");
                MessageBox.Show(error.Message);

            }
        }
    }
}
