using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Microsoft.Win32;
namespace Custom_Lock_Screen.API
{
    class Tools
    {
        //Local path for lockscreen image (depends on cpu architecture.
        string backgroundDirectory;
        //Default background image.
        string backgroundName = "\\backgroundDefault.jpg";

        //Check if x64 or x86
        bool OS64 = Environment.Is64BitOperatingSystem;


        //Sets the local path depending on x64 or x86.
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
        //  Enables custom background key in registry.
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
        //Looks for directory, if it can't be found, will create it,
        //Then copy the desired image to the folder.
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

            //If folder exists, delete it.
            if (File.Exists(backgroundDirectory + backgroundName))
                File.Delete(backgroundDirectory + backgroundName);
            
            //Copy the new image to system32\oobe\info\backgrounds\.
            try
            { 
                File.Copy(imageLocation, backgroundDirectory + backgroundName);
            }
            catch (Exception e)
            {
                //Copy Failed.
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return false;
            }
            //Copy was successfull.
            return true;
        }
    }
}
