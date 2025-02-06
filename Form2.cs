using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        // Add a constructor that accepts the username
        public Form2(string username)
        {
            InitializeComponent();

            // Display a welcome message with the username
            this.Text = "Dashboard - Welcome " + username;
            Label welcomeLabel = new Label();
            welcomeLabel.Text = "Welcome, " + username + "!";
            welcomeLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            welcomeLabel.AutoSize = true;
            welcomeLabel.Location = new Point(20, 20);

            this.Controls.Add(welcomeLabel);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("FOODS button clicked!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
