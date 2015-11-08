using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;


namespace Custom_Lock_Screen
{
    public partial class Form1 : Form
    {
        Tools API = new Tools();
        //URL to local image
        string newBackground = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //Get dialog prompt
            DialogResult diagResult = dgFindPicture.ShowDialog();

            //Check if the user pressed 'OK' & selected a valid image
            if (diagResult == DialogResult.OK && API.ImageErrorChecking(dgFindPicture.FileName))
            {
                newBackground = dgFindPicture.FileName;

                //Set display output to user
                txtDir.Text = newBackground;
                pictureBox1.ImageLocation = newBackground;
                lblStatus.ForeColor = Color.LimeGreen;
                lblStatus.Text = "Valid Image";
                btnApply.Enabled = true;
            }
            else
            {
                lblStatus.Text = "Invalid image!";
                lblStatus.ForeColor = Color.DarkRed;
                btnApply.Enabled = false;
                pictureBox1.Image = null;
                MessageBox.Show("Incorrect file.\nPlease check the image is under 256kb.", "Incorrect File.");
              
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //Make Registry Changes
            API.setOEMBackground(1);
            
            //Move File to Windows Lockscreen Background
            if (API.setBackgroundImage(newBackground))
                MessageBox.Show("Lockscreen successfully updated.", "Success");
            else
                MessageBox.Show("Failed to change lockscreen.", "Failed");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            API.setOEMBackground(0);
            pictureBox1.Image = null;
            MessageBox.Show("Lockscreen reset.", "Success.");
        }
    }
}
