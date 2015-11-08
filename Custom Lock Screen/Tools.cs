using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
namespace Custom_Lock_Screen
{
    class Tools
    {
        string backgroundDirectory, 
               backgroundName = "\\backgroundDefault.jpg";

        bool OS64 = Environment.Is64BitOperatingSystem;

        private void is64BitOS()
        {
            if (OS64)
            {
                backgroundDirectory = Environment.GetEnvironmentVariable("windir") + @"\sysnative\oobe\info\backgrounds";
            }
            else
            {
                backgroundDirectory = Environment.GetEnvironmentVariable("windir") + @"\System32\oobe\info\backgrounds";
            }
        }

        // --- Image API ---
        private long getImageSize(string picture)
        {
            long size = new FileInfo(picture).Length;
            return size;
        }
        public bool ImageErrorChecking(string ImageLocation)
        {
            bool result;

            //Convert the string to lower case
            ImageLocation = ImageLocation.ToLower();

            //Check if the file is a image.
            if ((result = new[] { ".jpg", ".gif", ".jpeg", ".png", ".bmp", ".tiff", ".exif" }.Any(ImageLocation.Contains)) == false)
                return false;
            //Check if the image is under 256kb
            else if (getImageSize(ImageLocation) > 256000)
                return false;
            //Return that the image is correctly formatted.
            else
                return true;

        }
        // --- END OF IMAGE API ---

        //  --- Registry API ---
        public void setOEMBackground(int enabled)
        {
            const string keyValue = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Background";

            //Set 64 bit registry
            using (var hklm64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var akey = hklm64.OpenSubKey(keyValue, true))
                    akey.SetValue("OEMBackground", enabled);
            //Set 32 bit registry
            using (var hklm32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                using (var akey = hklm32.OpenSubKey(keyValue, true))
                    akey.SetValue("OEMBackground", enabled);
                      
        }
        // --- END OF REGISTRY API---

        public bool setBackgroundImage(string imageLocation)
        {
            is64BitOS();

            //Create directory if doesnt exist.
           if (!Directory.Exists(backgroundDirectory))
            {
               string temp;
               
               if(OS64)
                    temp = Environment.GetEnvironmentVariable("windir") + @"\Sysnative\";
               else
                   temp = Environment.GetEnvironmentVariable("windir") + @"\System32\";


                Directory.CreateDirectory(temp + @"oobe\info");
                Directory.CreateDirectory(temp + @"oobe\info\backgrounds");

            }

            //If folder exists, delete it 
            if (File.Exists(backgroundDirectory + backgroundName))
                File.Delete(backgroundDirectory + backgroundName);
            
            //Copy the new image to system32\oobe\info\backgrounds\.
            try
            { 
               
                File.Copy(imageLocation, backgroundDirectory + backgroundName);
                System.Diagnostics.Debug.Print(backgroundDirectory + backgroundName);
            }
            catch (Exception e)
            {
                //Copy Failed
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return false;
            }

            //Copy was successfull
            return true;
        }
    }
}
