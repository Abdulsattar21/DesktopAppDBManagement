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
    public partial class EmployeesForm : Form
    {
        string connectionString = "server=.\\SQLEXPRESS; user id=wajd;password=123456";
        SqlConnection connection;

        string oldEmployeeID = "";

        public EmployeesForm()
        {
            InitializeComponent();
        }

        private void ClearAll()
        {
            oldEmployeeID = "";
            label9.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = textBox7.Text = "";
            dateTimePicker1.Value = DateTime.Now.Date;
        }
        private void EmployeesLoad()
        {
            string sql = "SELECT * FROM Employees ";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            
        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void EmployeesForm_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionString);

            connection.Open();
            EmployeesLoad();

        }

        private void EmployeesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();

        }

      

        private void button1_Click_1(object sender, EventArgs e)
        {

            //ADD an Employee
            
            
                string sql = "INSERT INTO Employees VALUES (@EmployeeName,@JobTitle,@HiringDate,@Salary,@Phone,@Email,@Address)";
                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@EmployeeName", textBox2.Text);
                command.Parameters.AddWithValue("@JobTitle",     textBox3.Text);
                command.Parameters.AddWithValue("@HiringDate",   dateTimePicker1.Value);
                command.Parameters.AddWithValue("@Salary",       textBox6.Text);
                command.Parameters.AddWithValue("@Phone",        textBox4.Text);
                command.Parameters.AddWithValue("@Email",        textBox7.Text);
                command.Parameters.AddWithValue("@Address",      textBox5.Text);

                command.ExecuteNonQuery();
                EmployeesLoad();
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //update row    

            if (oldEmployeeID == "")
            {
                MessageBox.Show("Please Double-click a row to Update it");
                return;

            }

            string sql = "UPDATE Employees " +
                        " SET    EmployeeName=@EmployeeName, " +
                        "    JobTitle=@JobTitle, " +
                        "    HiringDate=@HiringDate, " +
                        "    Salary=@Salary, " +
                        "    Phone=@Phone, " +
                        "    Email=@Email, " +
                        "    Address=@Address " +
                        " Where EmployeeID=@oldEmployeeID ";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@EmployeeName", textBox2.Text);
                command.Parameters.AddWithValue("@JobTitle", textBox3.Text);
                command.Parameters.AddWithValue("@HiringDate", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@Salary", textBox6.Text);
                command.Parameters.AddWithValue("@Phone", textBox4.Text);
                command.Parameters.AddWithValue("@Email", textBox7.Text);
                command.Parameters.AddWithValue("@Address", textBox5.Text);
                command.Parameters.AddWithValue("@oldEmployeeID", oldEmployeeID);


                command.ExecuteNonQuery();
                EmployeesLoad();
                
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

            label9.Text = dataGridView1.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
            oldEmployeeID = label9.Text;

            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["JobTitle"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["Address"].Value.ToString();

            textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells["Salary"].Value.ToString();           
            textBox7.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();

            String hiringDate = dataGridView1.Rows[e.RowIndex].Cells["HiringDate"].Value.ToString();
            if (hiringDate != "")
            { 
             dateTimePicker1.Value = Convert.ToDateTime(hiringDate);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //Delete a record

            if (oldEmployeeID == "")
            {
                MessageBox.Show("Please Double-click a row to select it");
                return;
            }

            string sql = "DELETE FROM Employees " +
                        "Where EmployeeID=@oldEmployeeID";
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@oldEmployeeID", oldEmployeeID);
                command.ExecuteNonQuery();

                EmployeesLoad();
                ClearAll();
            }

            catch (Exception error)
            {
                if (error.Message.IndexOf("FK_") > -1) //if FK-Related error Found
                    MessageBox.Show("update this row failed because it is linked to another table. Try to delete the related row first");
                MessageBox.Show( error.Message);
            }
        }
    }
}
