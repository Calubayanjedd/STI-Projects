using HospitalDashboard;
using System.Data;
using System.Data.SqlClient;

namespace Assigning
{
    public partial class Assign : Form
    {
        private string connectionString = "Data Source=LAPTOP-FT905FTC\\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;"; 
        public Assign()
        {
            InitializeComponent();
            LoadData();


        }
        private void LoadData()
        {
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter("Select doctorID, firstname FROM DoctorInfo", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridViewdoctor.DataSource = dt;

                //

                sda = new SqlDataAdapter("SELECT nurseID, firstname FROM NurseInfo", con);
                dt = new DataTable();
                sda.Fill(dt);
                dataGridViewnurse.DataSource = dt;

                //

                sda = new SqlDataAdapter("SELECT patientID, firstname FROM PatientInfo", con);
                dt = new DataTable();
                sda.Fill(dt);
                dataGridViewpatient.DataSource = dt;

                //

                sda = new SqlDataAdapter("SELECT roomID, roomNumber FROM Room", con);
                dt = new DataTable();
                sda.Fill(dt);
                dataGridViewroom.DataSource = dt;

                sda = new SqlDataAdapter("select assignID, doctorID, nurseID, patientID, roomID from Assign", con);
                dt = new DataTable();
                sda.Fill(dt);
                dataGridViewassign.DataSource = dt;

            }
        }
        /*
        Create table Assign(
        assignID int primary key identity,
        doctorID int foreign key references DoctorInfo(doctorID),
        nurseID int foreign key references NurseInfo(nurseID),
        patientID int foreign key references PatientInfo(patientID),
        roomID int foreign key references Room(roomID));
         */

        private void Assign_Click(object sender, EventArgs e)
        {
            if (dataGridViewdoctor.SelectedRows.Count > 0 && dataGridViewnurse.SelectedRows.Count > 0 && dataGridViewpatient.SelectedRows.Count > 0 && dataGridViewroom.SelectedRows.Count > 0)
            {
                int dcID = (int)dataGridViewdoctor.SelectedRows[0].Cells["doctorID"].Value;
                int nID = (int)dataGridViewnurse.SelectedRows[0].Cells["nurseID"].Value;
                int patID = (int)dataGridViewpatient.SelectedRows[0].Cells["patientID"].Value;
                int rID = (int)dataGridViewroom.SelectedRows[0].Cells["roomID"].Value;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if the selected doctor and nurse already have 4 patients assigned
                    SqlCommand cmdCountDoctorPatients = new SqlCommand("SELECT COUNT(*) FROM Assign WHERE doctorID = @doctorID", conn);
                    cmdCountDoctorPatients.Parameters.AddWithValue("@doctorID", dcID);
                    int doctorPatientCount = (int)cmdCountDoctorPatients.ExecuteScalar();

                    SqlCommand cmdCountNursePatients = new SqlCommand("SELECT COUNT(*) FROM Assign WHERE nurseID = @nurseID", conn);
                    cmdCountNursePatients.Parameters.AddWithValue("@nurseID", nID);
                    int nursePatientCount = (int)cmdCountNursePatients.ExecuteScalar();

                    if (doctorPatientCount >= 4 || nursePatientCount >= 4)
                    {
                        MessageBox.Show("Doctor or Nurse already has 4 patients assigned.");
                    }
                    else
                    {
                        // Check if the selected patient already has a doctor and nurse assigned
                        SqlCommand cmdCheckExistingAssignment = new SqlCommand("SELECT COUNT(*) FROM Assign WHERE patientID = @patientID", conn);
                        cmdCheckExistingAssignment.Parameters.AddWithValue("@patientID", patID);
                        int existingAssignmentCount = (int)cmdCheckExistingAssignment.ExecuteScalar();

                        if (existingAssignmentCount > 0)
                        {
                            MessageBox.Show("Patient already has a doctor and nurse assigned.");
                        }
                        else
                        {
                            // Check if the selected room is not already assigned
                            SqlCommand cmdCheckRoomAssignment = new SqlCommand("SELECT COUNT(*) FROM Assign WHERE roomID = @roomID", conn);
                            cmdCheckRoomAssignment.Parameters.AddWithValue("@roomID", rID);
                            int roomAssignmentCount = (int)cmdCheckRoomAssignment.ExecuteScalar();

                            if (roomAssignmentCount > 0)
                            {
                                MessageBox.Show("Room is already assigned.");
                            }
                            else
                            {
                                SqlCommand cmd = new SqlCommand("INSERT INTO Assign (doctorID, nurseID, patientID, roomID) VALUES (@doctorID, @nurseID, @patientID, @roomID)", conn);
                                cmd.Parameters.AddWithValue("@doctorID", dcID);
                                cmd.Parameters.AddWithValue("@nurseID", nID);
                                cmd.Parameters.AddWithValue("@patientID", patID);
                                cmd.Parameters.AddWithValue("@roomID", rID);

                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Assigned successfully.");
                                LoadData();
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select a row from each grid.");
            }
        }

        private void Assigning_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridViewassign.SelectedRows.Count > 0)
            {
                int assID = (int)dataGridViewassign.SelectedRows[0].Cells["assignID"].Value;
                using(SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from Assign Where assignID = @assignID", con);
                    cmd.Parameters.AddWithValue("@assignID", assID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Removed successfully");
                    LoadData();
                }
            }
        }

        private void Home_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Hide();
        }
    }
}
