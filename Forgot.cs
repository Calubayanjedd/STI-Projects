using Google.Protobuf.WellKnownTypes;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace St._Rome
{
    public partial class Forgot : Form
    {
        public Forgot()
        {
            InitializeComponent();
        }


        private void Forgot_Load(object sender, EventArgs e)
        {

        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-FT905FTC\\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;";
            string user = usertbx.Text;
            string pass = newpasstbx.Text;
            string cpass = confirmpasstbx.Text;

            string query = "UPDATE AdminLogin SET password = '" + pass + "' where username = '" + user + "'";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    int minLength = 7;
                    int maxLength = 25;
                    if (pass.Length <= minLength || pass.Length >= maxLength)
                    {
                        MessageBox.Show($"Password must be at least 8", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(cpass))
                    {
                        MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (pass != cpass)
                    {
                        MessageBox.Show("The password and confirm password do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (pass.Length > minLength && pass == cpass)
                    {
                        cmd.Parameters.AddWithValue("@password", pass);
                        cmd.Parameters.AddWithValue("@username", user);
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Successfully changed password.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoginForm lg = new LoginForm();
                            lg.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void newpasstbx_TextChanged(object sender, EventArgs e)
        {

        }
        private void progressBar2_Click(object sender, EventArgs e)
        {

        }
        private void progressBar2_Click_1(object sender, EventArgs e)
        {

        }

        private void confirmpasstbx_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginForm lg = new LoginForm();
            lg.Show();
            this.Hide();
        }

        private void usertbx_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
