using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;  // MySQL Connector
using BCrypt.Net;             // BCrypt for Password Hashing

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // Database Connection String (adjust if needed)
        private string connectionString = "Server=127.0.0.1;Database=db_project;Uid=root;Pwd=Kenneth1110@;";

        public Form1()
        {
            InitializeComponent();
        }

        // Registration Logic (To insert hashed password into the database)
        private void RegisterUser(string username, string password)
        {
            // Check if the username already exists
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if the username already exists in the database
                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@username", username);
                    int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userCount > 0)
                    {
                        MessageBox.Show("Username already exists. Please choose another username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Hash the password before inserting into the database
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("User Registered Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("User Registration Failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Login Logic
        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;   // TextBox for Username
            string password = txtPassword.Text;   // TextBox for Password

            // Ensure username and password are provided
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();  // Open the connection to the database

                    string query = "SELECT password FROM users WHERE username = @username";  // SQL Query to fetch the password
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);  // Adding username to the query

                    var reader = cmd.ExecuteReader();  // Execute the query

                    if (reader.Read())  // If user exists
                    {
                        string storedHashedPassword = reader.GetString("password");  // Retrieve the stored hashed password

                        // Debugging Output - Check what's retrieved from the database
                        Console.WriteLine("Stored Hashed Password: " + storedHashedPassword);
                        Console.WriteLine("Entered Password: " + password);

                        // Verify password with bcrypt
                        if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                        {
                            MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            Form2 dashboard = new Form2(username); // Pass username to Dashboard
                            dashboard.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Add missing event handlers to prevent errors
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }

        // Call RegisterUser() function if you need to register a new user
        

        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;   // Get username from textbox
            string password = txtPassword.Text;   // Get password from textbox

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Call RegisterUser method to insert hashed password into the database
            RegisterUser(username, password);
        }
    }
}
