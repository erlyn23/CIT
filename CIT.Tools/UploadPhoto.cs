using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Tools
{
    public static class UploadPhoto
    {
        public static async Task<string> UploadProfilePhotoAsync(string fileName, string photo)
        {
            string imagePath = $"wwwroot/ProfilePhotos";
            string[] imageSplitted = photo.Split(',');

            if (!imageSplitted[0].Contains("data:image"))
                throw new Exception("Debes subir una imagen válida, este no es una archivo de imagen");

            byte[] imageInBytes = Convert.FromBase64String(imageSplitted[1]);

            string path = Path.Combine(imagePath, fileName);


            using (var stream = new FileStream(path, FileMode.Create))
            {
                await stream.WriteAsync(imageInBytes, 0, imageInBytes.Length);
                await stream.FlushAsync();
            }

            return fileName;
        }
    }
}
