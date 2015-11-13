using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Custom_Lock_Screen.API
{
    class ImageTools
    {
        //Returns image size in bytes
        private long getImageSize(string picture)
        {
            long size = new FileInfo(picture).Length;
            return size;
        }
        //Creates a Tuple to hold the WxH extracted from image properties
        public Tuple<int,int> getImageDimensions(string picture)
        { 
            int w, h;

            //Create Image object and apply its w+h to variables
            System.Drawing.Image img = System.Drawing.Image.FromFile(picture);
            w = img.Width;
            h = img.Height;      
             
            return Tuple.Create(w, h);
        }
        //check if image is under 256kb and is a valid file type
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

    }
}
