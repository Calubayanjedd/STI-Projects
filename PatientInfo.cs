using Google.Protobuf.WellKnownTypes;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace St._Rome
{
    public partial class PatientInfo : Form
    {
        private string connectionString = "Data Source=LAPTOP-FT905FTC\\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;";
        public PatientInfo()
        {
            InitializeComponent();
        }
        private DataTable dt = new DataTable();
        private DataSet ds = new DataSet();
        private string selectedPatientID; //edited

        public DataTable Source()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            try
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM PatientInfo";
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
        private void F12_UpdateEventHandler1(object sender, PatientRegistration.UpdateEventArgs args)
        {
            dataGridView1.DataSource = Source();
        }
        private void patientbtn_Click(object sender, EventArgs e)
        {
            PatientRegistration pat = new PatientRegistration(this);
            pat.UpdateEventHandler += F12_UpdateEventHandler1;
            pat.Show();


        }

        private void Homebtn_Click(object sender, EventArgs e)
        {
            HomePage H = new HomePage();
            H.Show();
            this.Hide();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Source();
            dataGridView1.CellClick += dataGridView1_CellClick; //edited
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                // Assuming the patient ID is in the first column of the DataGridView
                string patientID = row.Cells[0].Value.ToString();
                // Store the selected patient ID in a variable for later use
                selectedPatientID = patientID;
            }
        }
        private void unreg_btn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedPatientID))
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                try
                {
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM PatientInfo WHERE PatientID = @PatientID";
                    cmd.Parameters.AddWithValue("@PatientID", selectedPatientID);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Patient unregistered successfully.");
                        dataGridView1.DataSource = Source(); // Refresh the DataGridView after unregistering
                    }
                    else
                    {
                        MessageBox.Show("Failed to unregister patient.");
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
                MessageBox.Show("Please select a patient to unregister.");
            }
        }
    }
}
