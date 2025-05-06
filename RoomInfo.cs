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
    public partial class RoomInfo : Form
    {
        string Cn = @"Data Source=LAPTOP-FT905FTC\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;";
        public RoomInfo()
        {
            InitializeComponent();
        }

        private void Homebutton_Click(object sender, EventArgs e)
        {
            HomePage home = new HomePage();
            home.Show();
            this.Hide();
        }

        /*
        create table Room(
        roomID int primary key identity(800,1),
        roomNumber varchar(10) not null,
        doctorID int,
        patientID int,
        nurseID int,
        foreign key (doctorID) references DoctorInfo(doctorID),
        foreign key (patientID) references PatientInfo(patientID),
        foreign key (nurseID) references NurseInfo(nurseID)
        );
         */
        private void RefreshRoomData()
        {
           
               using(SqlConnection con = new SqlConnection(Cn))
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(@"select R.roomID, R.roomNumber, A.assignID, A.doctorID, A.nurseID, A.patientID from Room R join Assign A on R.roomID = A.roomID", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataRoom.DataSource = dt;
            }

               using(SqlConnection conn = new SqlConnection(Cn))
            {
                conn.Open();
                SqlDataAdapter sda1 = new SqlDataAdapter("SELECT roomID, roomNumber From Room", conn);
                DataTable dt1 = new DataTable();
                sda1.Fill(dt1);
                dataGridViewassign.DataSource = dt1;
            }
            
        }

        private void RoomInfo_Load(object sender, EventArgs e)
        {
            RefreshRoomData();
        }

        private void homebtn_Click(object sender, EventArgs e)
        {
            HomePage hp = new HomePage();
            hp.Show();
            this.Hide();
        }
    }
}