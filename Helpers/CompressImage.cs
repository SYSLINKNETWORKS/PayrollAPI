using System;
using System.Drawing;
using System.IO;

namespace TWP_API_Payroll.Generic {
    public class CompressImage {

        public byte[] GetCompressImage (byte[] imageBytes) {
            Image original_img_obj = byteArrayToImage (imageBytes);
            Image compressed_img_obj = CompressedImage (original_img_obj.Width, original_img_obj.Height, original_img_obj);
            byte[] result = getsmallByteArray (compressed_img_obj);

            if (result.Length < imageBytes.Length) {
                imageBytes = result;
            }
            return imageBytes;

        }
        // convert byte array into image 
        private Image byteArrayToImage (byte[] byteArrayIn) {
            MemoryStream ms = new MemoryStream (byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream (ms);
            return returnImage;
        }

        private Image CompressedImage (int iwidth, int iheight, System.Drawing.Image img) {
            // CREATE AN IMAGE OBJECT USING ORIGINAL WIDTH AND HEIGHT.
            // ALSO DEFINE A PIXEL FORMAT (FOR RICH RGB COLOR).

            System.Drawing.Image objOptImage = new System.Drawing.Bitmap (iwidth, iheight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);

            // GET THE ORIGINAL IMAGE.
            // RE-DRAW THE IMAGE USING THE NEWLY OBTAINED PIXEL FORMAT.
            using (System.Drawing.Graphics oGraphic = System.Drawing.Graphics.FromImage (objOptImage)) {
                var _1 = oGraphic;
                System.Drawing.Rectangle oRectangle = new System.Drawing.Rectangle (0, 0, iwidth, iheight);

                _1.DrawImage (img, oRectangle);
            }

            // SAVE THE OPTIMIZED IMAGE.
            //            objOptImage.Save(HttpContext.Current.Server.MapPath("~/images/" + sImageName), System.Drawing.Imaging.ImageFormat.Jpeg);
            //img.Dispose();
            //objOptImage.Dispose();
            return objOptImage;
        }
        // convert image into  byte array in JPEG FORMAT
        private byte[] imageToByteArrayJPEG (Image imageIn) {
            MemoryStream ms = new MemoryStream ();
            imageIn.Save (ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray ();
        }

        // convert image into  byte array in PNG FORMAT
        private byte[] imageToByteArrayPNG (Image imageIn) {
            MemoryStream ms = new MemoryStream ();
            imageIn.Save (ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray ();
        }
        private byte[] getsmallByteArray (Image compressed_img_obj) {
            byte[] result = null;
            byte[] fileBytesPng = imageToByteArrayPNG (compressed_img_obj);
            byte[] fileBytesJpeg = imageToByteArrayJPEG (compressed_img_obj);

            if (fileBytesJpeg.Length <= fileBytesPng.Length) {
                result = fileBytesJpeg;
            } else if (fileBytesPng.Length < fileBytesJpeg.Length) {
                result = fileBytesPng;
            }

            return result;
        }
    }
}