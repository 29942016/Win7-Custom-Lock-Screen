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
        API.Tools _PatchingAPI = new API.Tools();
        API.ImageTools _ImageAPI = new API.ImageTools();

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
            if (diagResult == DialogResult.OK && _ImageAPI.ImageErrorChecking(dgFindPicture.FileName))
            {
                newBackground = dgFindPicture.FileName;
                int[] newArr = new int[2];

                //Set display output to user
                txtDir.Text = newBackground;
                //lblProperties.Text = _PatchingAPI.convertArrayToString<int>(ref _ImageAPI.getImageDimensions(dgFindPicture.FileName));
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
            _PatchingAPI.setOEMBackground(1);
            
            //Move File to Windows Lockscreen Background
            if (_PatchingAPI.setBackgroundImage(newBackground))
                MessageBox.Show("Lockscreen successfully updated.", "Success");
            else
                MessageBox.Show("Failed to change lockscreen.", "Failed");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _PatchingAPI.setOEMBackground(0);
            pictureBox1.Image = null;
            MessageBox.Show("Lockscreen reset.", "Success.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}
