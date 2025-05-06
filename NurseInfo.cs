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

    public partial class NurseInfo : Form
    {
        private string connectionString = "Data Source=LAPTOP-FT905FTC\\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;";
        public NurseInfo()
        {
            InitializeComponent();
        }
        private DataTable dt = new DataTable();
        private DataSet ds = new DataSet();
        private string selectednurseID;

        public DataTable Source()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM NurseInfo";
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
        private void F13_UpdateEventHandler1(object sender, NurseRegistration.UpdateEventArgs args)
        {
            nurse_data.DataSource = Source();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            nurse_data.DataSource = Source();
            nurse_data.CellClick += dataGridViewnurse_CellClick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NurseRegistration nreg = new NurseRegistration(this);
            nreg.UpdateEventHandler += F13_UpdateEventHandler1;
            nreg.Show();
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            HomePage hm = new HomePage();
            hm.Show();
            this.Hide();
        }

        private void unreg_btn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectednurseID))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM NurseInfo WHERE nurseID = @nurseID";
                    cmd.Parameters.AddWithValue("@nurseID", selectednurseID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Nurse unregistered successfully.");
                        nurse_data.DataSource = Source(); // Refresh the DataGridView after unregistering
                    }
                    else
                    {
                        MessageBox.Show("Failed to unregister Nurse.");
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
                MessageBox.Show("Please select a nurse to unregister.");
            }
        }
        private void dataGridViewnurse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = nurse_data.Rows[e.RowIndex];

                string nurseID = row.Cells[0].Value.ToString();

                selectednurseID = nurseID;
            }
        }

        private void nurse_data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
