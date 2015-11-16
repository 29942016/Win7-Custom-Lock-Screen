using System;
using System.Drawing;
using System.Windows.Forms;

/*
   # Final Build - 16/11/2015
   # Oliver.Buckler@Gmail.com
   # https://github.com/Oliver-Buckler/Win7-Custom-Lock-Screen
*/

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
                Tuple<int, int> dimensions = new Tuple<int, int>(0, 0); //Stores image dimensions
                newBackground = dgFindPicture.FileName; //short hand reference for image
                 
                //Set display output to user
                txtDir.Text = newBackground;
                //Set Label with image dimensions
                dimensions = _ImageAPI.getImageDimensions(newBackground);
                lblProperties.Text = dimensions.Item1.ToString() + 'x' + dimensions.Item2.ToString();
                //Display image in imagebox 
                pictureBox1.ImageLocation = newBackground;
                //Display image is ok label
                lblStatus.ForeColor = Color.LimeGreen;
                lblStatus.Text = "Valid Image";
                //Enable the apply option
                btnApply.Enabled = true;
                lblProperties.Visible = true;
            }
            else
            {
                //Invalid label
                lblStatus.Text = "Invalid image!";
                lblStatus.ForeColor = Color.DarkRed;
                //Disable the apply option 
                btnApply.Enabled = false;
                lblProperties.Visible = false;
                //Hide display box
                pictureBox1.Image = null;
                //Output to user it won't work.
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
            lblProperties.Visible = false;
            MessageBox.Show("Lockscreen reset.", "Success.");

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
    }
}
