using HospitalDashboard;
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

namespace St._Rome
{
    public partial class DoctorsInfo : Form
    {
        private string ConnectionString = "Data Source=LAPTOP-FT905FTC\\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;";
        public DoctorsInfo()
        {
            InitializeComponent();
        }
        private DataTable dt = new DataTable();
        private DataSet ds = new DataSet();
        private string selecteddoctorID;
        public DataTable Source()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM DoctorInfo";
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                ds.Clear();
                adap.Fill(ds);
                dt = ds.Tables[0];
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return dt;
        }
        private void F6_UpdateEventHandler1(object sender, DoctorRegistration.UpdateEventArgs args)
        {
            dataGridView1.DataSource = Source();
        }


        private void Form4_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Source();
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void add_doc_Click(object sender, EventArgs e)
        {
            DoctorRegistration dr = new DoctorRegistration(this);
            dr.UpdateEventHandler += F6_UpdateEventHandler1;
            dr.Show();
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                string doctorID = row.Cells[0].Value.ToString();

                selecteddoctorID = doctorID;
            }
        }

        private void unreg_btn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selecteddoctorID))
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM DoctorInfo WHERE doctorID = @doctorID";
                    cmd.Parameters.AddWithValue("@doctorID", selecteddoctorID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Doctor unregistered successfully.");
                        dataGridView1.DataSource = Source(); // Refresh the DataGridView after unregistering
                    }
                    else
                    {
                        MessageBox.Show("Failed to unregister Doctor.");
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a doctor to unregister.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
