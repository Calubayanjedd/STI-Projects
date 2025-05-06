using HospitalDashboard;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace St._Rome
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection("Data Source=LAPTOP-FT905FTC\\SQLEXPRESS;Initial Catalog=HospitalDatabase;Integrated Security=True;");

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Login_btn_Click(object sender, EventArgs e)
        {

            /*
             Create table AdminLogin (
             adminID int Primary key identity,
             username varchar(50) collate SQL_Latin1_General_CP1_CS_AS not null,
             password varchar(50) collate SQL_Latin1_General_CP1_CS_AS not null,
             
            );
            
             */ //SQL QUery LOGIN CASE SENSITIVE

            string username, user_pass;
            username = username_txtbox.Text;
            user_pass = password_txtbox.Text;
            try
            {
                String querry = "SELECT * FROM AdminLogin WHERE username = '" + username_txtbox.Text + "' AND password ='" + password_txtbox.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(querry, connection);

                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    username = username_txtbox.Text;
                    user_pass = password_txtbox.Text;
                    HomePage fr = new HomePage();
                    fr.Show();
                    this.Hide();

                }
                else if (!Regex.IsMatch(username, @"^[^\s][A-Za-z]+(?:\s[A-Za-z]+)*(?:\s[A-Za-z]+)?$"))
                {
                    MessageBox.Show("Space is not allowed in the beginning.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(user_pass))
                {
                    MessageBox.Show("Input your account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(user_pass))
                {
                    MessageBox.Show("Enter what is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    username_txtbox.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            finally
            {
                connection.Close();
            }
        }

        private void Signup_btn_Click(object sender, EventArgs e)
        {

        }

        private void chck_pass_CheckedChanged(object sender, EventArgs e)
        {
            if (chck_pass.Checked)
            {
                password_txtbox.UseSystemPasswordChar = false;
            }
            else
            {
                password_txtbox.UseSystemPasswordChar = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            Forgot forg = new Forgot();
            forg.Show();
            this.Hide();
        }
    }
}
