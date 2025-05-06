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
using System.Transactions;
using System.Windows.Forms;

namespace St._Rome
{
    public partial class DoctorRegistration : Form
    {
        string ConnectionString = @"Data Source=LAPTOP-FT905FTC\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;";
        public DoctorRegistration(DoctorsInfo info)
        {
            InitializeComponent();

        }
        public delegate void UpdateDelegate(object sender, UpdateEventArgs args);
        public event UpdateDelegate UpdateEventHandler;

        public class UpdateEventArgs : EventArgs
        {
            public string Data { get; set; }
        }
        protected void addDoctor()
        {
            UpdateEventArgs args = new UpdateEventArgs();
            UpdateEventHandler.Invoke(this, args);
        }

        private void Form6_Load(object sender, EventArgs e)
        {

            DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
            for (int year = 1900; year <= DateTime.Today.Year; year++)
                this.combo_year.Items.Add(year.ToString());

            string[] month = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            foreach (string Month in month)
            {
                combo_month.Items.Add(Month);
            }
            string[] suffix = { "Jr.", "Sr.", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV" };
            foreach (string Suffix in suffix)
            {
                suf.Items.Add(Suffix);
            }
        }


        private void Button2_Click(object sender, EventArgs e)
        {

        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

            combo_date.DataSource = null;
            combo_date.Text = "day";
        }

        private void combo_month_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(combo_year.Text))
            {
                MessageBox.Show("Select a year");
                return;
            }
            int year = Convert.ToInt32(combo_year.Text);

            int month = combo_month.SelectedIndex;
            if (month >= 0)
            {
                combo_date.DataSource = null;

                month++;
                int days = DateTime.DaysInMonth(year, month);
                var range = Enumerable.Range(1, days);
                combo_date.DataSource = range.ToList();
            }
        }


        private void combo_date_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        

        /*
        create table DoctorInfo(
        doctorID int primary key identity(100,1),
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
                string suff = suf.Text;
                string id = adminID.Text;
                string fname = Firstname.Text;
                string mname = Middlename.Text;
                string lname = Lastname.Text;
                string nat = Nationality.Text;
                string addr = address.Text;
                string pnum = phonenum.Text;
                string bday = combo_year.Text + combo_month.Text + combo_date.Text;
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
                else if (combo_year.SelectedIndex < 1 && combo_month.SelectedIndex < 1 && combo_date.SelectedIndex < 1)
                {
                    MessageBox.Show("Please input birthday", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using (SqlCommand md = new SqlCommand("select * from AdminLogin where adminID = '" + id + "'"))
                {
                    using (SqlCommand cmd = connect.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO[dbo].[DoctorInfo]
                            ([adminID], [firstname], [lastname], [middlename], [address], [nationality], [gender], [date_of_birth], [phone_number], [suffix])
                            VALUES (@adminID, @firstname, @lastname, @middlename, @address, @nationality, @gender, @date_of_birth, @phone_number, @suffix);";
                        cmd.Parameters.AddWithValue("@adminID", id);
                        cmd.Parameters.AddWithValue("@firstname", fname);
                        cmd.Parameters.AddWithValue("@lastname", lname);
                        cmd.Parameters.AddWithValue("@middlename", mname);
                        cmd.Parameters.AddWithValue("@address", addr);
                        cmd.Parameters.AddWithValue("@nationality", nat);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@date_of_birth", combo_year.Text + " - " + combo_month.Text + " - " + combo_date.Text);
                        cmd.Parameters.AddWithValue("@phone_number", pnum);
                        cmd.Parameters.AddWithValue("@suffix", suff);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Doctor Registered Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addDoctor(); // Assuming this method adds the doctor to a UI or updates a list
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            finally
            {
                connect.Close();
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void phonenum_TextChanged(object sender, EventArgs e)
        {
            phonenum.MaxLength = 11;
        }

        private void Firstname_TextChanged(object sender, EventArgs e)
        {
            
        }

        /*SqlConnection connect = new SqlConnection(ConnectionString);
connect.Open();
try
{
    string fname = Firstname.Text;
    string mname = Middlename.Text;
    string lname = Lastname.Text;
    string nat = Nationality.Text;
    string addr = address.Text;
    string pnum = phonenum.Text;
    string bday = combo_year.Text + combo_month.Text + combo_date.Text;
    string gender = (malerad.Checked) ? "Male" : (fmalerad.Checked ? "Female" : "");

    // Validations
    if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(nat) || string.IsNullOrWhiteSpace(addr) || string.IsNullOrWhiteSpace(pnum) || combo_year.SelectedIndex < 1 || combo_month.SelectedIndex < 1 || combo_date.SelectedIndex < 1)
    {
        MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return; // Exit the method if any required field is empty
    }

    if (!Regex.IsMatch(fname, @"^[a-zA-Z\s]*$") || !Regex.IsMatch(lname, @"^[a-zA-Z]*$") || !Regex.IsMatch(nat, @"^[a-zA-Z]*$") || !Regex.IsMatch(addr, @"^[a-zA-Z0-9\s]*$"))
    {
        MessageBox.Show("Please enter valid information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return; // Exit the method if any field contains invalid characters
    }

    // SQL Database Insert
    using (SqlCommand cmd = connect.CreateCommand())
    {
        cmd.CommandText = @"INSERT INTO [dbo].[DoctorInfo] ([firstname], [lastname], [middlename], [address], [nationality], [gender], [date_of_birth], [phone_number])
                            VALUES (@firstname, @lastname, @middlename, @address, @nationality, @gender, @date_of_birth, @phone_number);";
        cmd.Parameters.AddWithValue("@firstname", fname);
        cmd.Parameters.AddWithValue("@lastname", lname);
        cmd.Parameters.AddWithValue("@middlename", mname);
        cmd.Parameters.AddWithValue("@address", addr);
        cmd.Parameters.AddWithValue("@nationality", nat);
        cmd.Parameters.AddWithValue("@gender", gender);
        cmd.Parameters.AddWithValue("@date_of_birth", combo_year.Text + "-" + combo_month.Text + "-" + combo_date.Text);
        cmd.Parameters.AddWithValue("@phone_number", pnum);
        cmd.ExecuteNonQuery();
    }

    MessageBox.Show("Doctor information added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    addDoctor(); // Assuming this method adds the doctor to a UI or updates a list
}
catch (Exception ex)
{
    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
        

        *************************************
        
         SqlConnection connect = new SqlConnection(ConnectionString);
connect.Open();
try
{
    string fname = Firstname.Text;
    string mname = Middlename.Text;
    string lname = Lastname.Text;
    string nat = Nationality.Text;
    string addr = address.Text;
    string pnum = phonenum.Text;
    string bday = combo_year.Text + combo_month.Text + combo_date.Text;
    string gender = (malerad.Checked) ? "Male" : ((fmalerad.Checked) ? "Female" : "");

    if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(nat) || string.IsNullOrWhiteSpace(addr) || string.IsNullOrWhiteSpace(pnum) || combo_year.SelectedIndex < 1 || combo_month.SelectedIndex < 1 || combo_date.SelectedIndex < 1)
    {
        MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    if (!Regex.IsMatch(fname, @"^[a-zA-Z\s]*$"))
    {
        MessageBox.Show("First name can only contain letters and spaces.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    if (!Regex.IsMatch(mname, @"^[a-zA-Z]*$"))
    {
        MessageBox.Show("Middle name can only contain letters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    if (!Regex.IsMatch(lname, @"^[a-zA-Z]*$"))
    {
        MessageBox.Show("Last name can only contain letters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    if (!Regex.IsMatch(nat, @"^[a-zA-Z]*$"))
    {
        MessageBox.Show("Nationality can only contain letters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    if (!Regex.IsMatch(addr, @"^[a-zA-Z0-9\s]*$"))
    {
        MessageBox.Show("Address can contain letters, numbers, and spaces only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    if (!Regex.IsMatch(pnum, @"^[0-9]*$"))
    {
        MessageBox.Show("Phone number can only contain numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    using (SqlCommand cmd = connect.CreateCommand())
    {
        cmd.CommandText = @"INSERT INTO [dbo].[DoctorInfo]
                            ([firstname], [lastname], [middlename], [address], [nationality], [gender], [date_of_birth], [phone_number])
                            VALUES (@firstname, @lastname, @middlename, @address, @nationality, @gender, @date_of_birth, @phone_number);";
        cmd.Parameters.AddWithValue("@firstname", fname);
        cmd.Parameters.AddWithValue("@lastname", lname);
        cmd.Parameters.AddWithValue("@middlename", mname);
        cmd.Parameters.AddWithValue("@address", addr);
        cmd.Parameters.AddWithValue("@nationality", nat);
        cmd.Parameters.AddWithValue("@gender", gender);
        cmd.Parameters.AddWithValue("@date_of_birth", combo_year.Text + "-" + combo_month.Text + "-" + combo_date.Text);
        cmd.Parameters.AddWithValue("@phone_number", pnum);
        cmd.ExecuteNonQuery();
    }

    MessageBox.Show("Doctor information added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    addDoctor(); // Assuming this method adds the doctor to a UI or updates a list
}
catch (Exception ex)
{
    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
        */
    }
}
