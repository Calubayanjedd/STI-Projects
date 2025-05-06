using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace St._Rome
{
    public partial class NurseRegistration : Form
    {
        string ConnectionString = @"Data Source=LAPTOP-FT905FTC\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;";
        public NurseRegistration(NurseInfo info)
        {
            InitializeComponent();
        }
        public delegate void UpdateDelegate(object sender, UpdateEventArgs args);
        public event UpdateDelegate UpdateEventHandler;

        public class UpdateEventArgs : EventArgs
        {
            public string Data { get; set; }
        }
        protected void addNurse()
        {
            UpdateEventArgs args = new UpdateEventArgs();
            UpdateEventHandler.Invoke(this, args);
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
            for (int year = 1900; year <= DateTime.Today.Year; year++)
                this.year_cbx.Items.Add(year.ToString());

            string[] month = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            foreach (string Month in month)
            {
                month_cbx.Items.Add(Month);
            }
            string[] suffix = { "Jr.", "Sr.", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV" };
            foreach (string Suffix in suffix)
            {
                suf.Items.Add(Suffix);
            }

        }

        private void year_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            day_cbx.DataSource = null;
            day_cbx.Text = "day";
        }

        private void month_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(year_cbx.Text))
            {
                MessageBox.Show("Select a year");
                return;
            }
            int year = Convert.ToInt32(year_cbx.Text);

            int month = month_cbx.SelectedIndex;
            if (month >= 0)
            {
                day_cbx.DataSource = null;

                month++;
                int days = DateTime.DaysInMonth(year, month);
                var range = Enumerable.Range(1, days);
                day_cbx.DataSource = range.ToList();
            }
        }

        private void day_cbx_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /*
        create table NurseInfo(
        nurseID int primary key identity(300,1),
        adminID int,
        firstname varchar(55),
        middlename varchar(55),
        lastname varchar(55),
        nationality varchar(55),
        address varchar(100),
        gender varchar(55),
        date_of_birth varchar(100),
        phone_number varchar(55)
        foreign key (adminID) references AdminLogin(adminID)
        );
         */
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection(ConnectionString);
            connect.Open();
            try
            {
                string id = adminID.Text;
                string suff = suf.Text;
                string fname = fname_tbx.Text;
                string mname = mname_tbx.Text;
                string lname = lname_tbx.Text;
                string nat = nationality_tbx.Text;
                string addr = add_tbx.Text;
                string pnum = phone_tbx.Text;
                string gender = (malerad.Checked) ? "Male" : ((fmalerad.Checked) ? "Female" : "");
                if (!Regex.IsMatch(fname + mname + lname, @"^[^\s][A-Za-z]+(?:\s[A-Za-z]+)*(?:\s[A-Za-z]+)?$") || !Regex.IsMatch(nat, @"^[a-zA-Z]*$") || !Regex.IsMatch(addr, @"^[a-zA-Z0-9\s]*$"))
                {
                    MessageBox.Show("Please enter valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method if validation fails
                }
                else if (!Regex.IsMatch(pnum, @"^09\d{9}$"))
                {
                    MessageBox.Show("Number must start at '09'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(fname))
                {
                    MessageBox.Show("Enter first name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(lname))
                {
                    MessageBox.Show("Enter last name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(nat))
                {
                    MessageBox.Show("Enter nationality", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(addr))
                {
                    MessageBox.Show("Enter address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (string.IsNullOrWhiteSpace(pnum))
                {
                    MessageBox.Show("Enter number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (year_cbx.SelectedIndex < 1 && month_cbx.SelectedIndex < 1 && day_cbx.SelectedIndex < 1)
                {
                    MessageBox.Show("Please input birthday", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using (SqlCommand md = new SqlCommand("select * from AdminLogin where adminID = '" + id + "'"))
                {
                    using (SqlCommand cmd = connect.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO[dbo].[NurseInfo]
                           ([adminID], [firstname]
                         ,[lastname]
                            ,[middlename]
                           ,[address]
                          ,[nationality]
                          ,[gender]
                           ,[date_of_birth]
                           ,[phone_number], [suffix]) VALUES (@adminID, @firstname, @lastname, @middlename, @address, @nationality, @gender, @date_of_birth, @Phone_number, @suffix);";
                        cmd.Parameters.AddWithValue("@adminID", id);
                        cmd.Parameters.AddWithValue("firstname", fname_tbx.Text);
                        cmd.Parameters.AddWithValue("lastname", lname_tbx.Text);
                        cmd.Parameters.AddWithValue("middlename", mname_tbx.Text);
                        cmd.Parameters.AddWithValue("address", add_tbx.Text);
                        cmd.Parameters.AddWithValue("nationality", nationality_tbx.Text);
                        cmd.Parameters.AddWithValue("gender", gender);
                        cmd.Parameters.AddWithValue("date_of_birth", year_cbx.Text + " - " + month_cbx.Text + " - " + day_cbx.Text);
                        cmd.Parameters.AddWithValue("phone_number", phone_tbx.Text);
                        cmd.Parameters.AddWithValue("@suffix", suff);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Nurse Registered Successfully.", "Successfully", MessageBoxButtons.OK);
                addNurse();
                this.Hide();
            }
            catch
            {
                MessageBox.Show("Error");
            }
            finally
            {
                connect.Close();
            }
        }

        private void nurse_ID_Click(object sender, EventArgs e)
        {

        }

        private void phone_tbx_TextChanged(object sender, EventArgs e)
        {
            phone_tbx.MaxLength = 11;
        }
    }
}
