using Assigning;
using St._Rome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HospitalDashboard
{
    public partial class HomePage : Form
    {
        
        bool sidebarExpand;
        bool staffCollapsed;
        public HomePage()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void SidebarTimer_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();

        }

        private void Dashboard_Click(object sender, EventArgs e)
        {
            AboutUs dash = new AboutUs();
            dash.Show();
            this.Hide();
        }

        private void StaffTimer_Tick(object sender, EventArgs e)
        {
            if (staffCollapsed)
            {
                staffContainer.Height += 10;
                if (staffContainer.Height == staffContainer.MaximumSize.Height)
                {
                    staffCollapsed = false;
                    staffTimer.Stop();
                }
            }
            else
            {
                staffContainer.Height -= 10;
                if (staffContainer.Height == staffContainer.MinimumSize.Height)
                {
                    staffCollapsed = true;
                    staffTimer.Stop();
                }
            }
        }

        private void StaffContainer_Click(object sender, EventArgs e)
        {
            staffTimer.Start();
        }

        private void Panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            //doctor
            DoctorsInfo doc = new DoctorsInfo();
            doc.Show();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            LoginForm bck = new LoginForm();
            bck.Show();
            this.Hide();
        }

        private void Patient_btn_Click(object sender, EventArgs e)
        {
            PatientInfo patient = new PatientInfo();
            patient.Show();
            this.Hide();

        }

        private void AssignButton_Click(object sender, EventArgs e)
        {

            Assign agn = new Assign();
            agn.Show();
            this.Hide();
        }

        private void RoomsButton_Click(object sender, EventArgs e)
        {
            RoomInfo rooms = new RoomInfo();
            rooms.Show(); 
            this.Hide();
        }

        private void Panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void NotifBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Panel8_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void nurseButton_Click(object sender, EventArgs e)
        {
            //nurse
            NurseInfo nurse = new NurseInfo();
            nurse.Show();
            this.Hide();
        }

        private void nurseButton_Click_1(object sender, EventArgs e)
        {
            NurseInfo nurse = new NurseInfo();
            nurse.Show();
            this.Hide();

        }
    }
}
