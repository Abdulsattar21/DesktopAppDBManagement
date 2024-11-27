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
    public partial class CategoryForm : Form
    {
        string connectionString = "server=.\\SQLEXPRESS;user id=wajd;password=123456";
        SqlConnection connection;
        string oldCategoryCode = "";


        public CategoryForm()
        {
            InitializeComponent();
        }


        private void CategoryLoad()
        {
            string sql = "SELECT * FROM Category ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;


        }
        private void CategoryForm_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionString);

            connection.Open();
            CategoryLoad();
        }

        private void CategoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ADD a category

            string sql = "INSERT INTO Category VALUES (@CategoryName,@Descreption)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryName", textBox2.Text);
            command.Parameters.AddWithValue("@Descreption", textBox3.Text);
            command.ExecuteNonQuery();
            CategoryLoad();
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

        }

        private void button3_Click(object sender, EventArgs e)
            {
            //update row    

            if (oldCategoryCode == "")
            {
                MessageBox.Show("Please Double-click a row to Edit it");
                return;

            }

            string sql = "UPDATE Category " +
                        " SET    CategoryName=@CategoryName, " +
                        "        Descreption=@Descreption "    +
                        " Where  CategoryCode=@oldCategoryCode ";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CategoryName", textBox2.Text);
                command.Parameters.AddWithValue("@Descreption", textBox3.Text);
                command.Parameters.AddWithValue("@oldCategoryCode", oldCategoryCode);

                command.ExecuteNonQuery();
                CategoryLoad();
                ClearAll();

            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Load into fields to update  

            label4.Text = dataGridView1.Rows[e.RowIndex].Cells["CategoryCode"].Value.ToString();
            oldCategoryCode = label4.Text;

            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["CategoryName"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["Descreption"].Value.ToString();
            
        }
        private void ClearAll()
        {
            oldCategoryCode = "";
            label4.Text = "______________";
            textBox2.Text = textBox3.Text = "";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Delete a record

            if (oldCategoryCode == "")
            {
                MessageBox.Show("Please Double-click a row to select it");
                return;

            }

            string sql = "DELETE FROM Category " +
                        "Where CategoryCode=@oldCategoryCode";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@oldCategoryCode", oldCategoryCode);

                command.ExecuteNonQuery();

                CategoryLoad();

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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
