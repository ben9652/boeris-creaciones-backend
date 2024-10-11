﻿using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace BoerisCreaciones.Service.Helpers
{
    public class MultimediaManging
    {
        public static async Task<string> UploadImage(IFormFile file, string rootPath, string controllerName)
        {
            string contentType = file.ContentType;
            if (file == null || file.Length == 0)
                throw new ArgumentException("No se proporcionó una imagen válida");

            var uploadsPath = Path.Combine(rootPath, controllerName);
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            string extension;
            switch (contentType)
            {
                case "image/jpeg":
                    extension = ".jpg";
                    break;
                case "image/png":
                    extension = ".png";
                    break;
                default:
                    throw new FormatException("Formato no soportado");
            }

            // Generar un nombre único para la imagen
            var fileName = Guid.NewGuid().ToString() + extension;

            // Ruta para guardar la imagen en el servidor
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var image = Image.Load(file.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(500, 500)
                }));

                await image.SaveAsync(filePath, new JpegEncoder { Quality = 70 });
            }

            return fileName;
        }

        public static bool DeleteImage(string image, string rootPath, string controllerName)
        {
            string imageFullPath = Path.Combine(rootPath, controllerName, image);
            if (File.Exists(imageFullPath))
            {
                File.Delete(imageFullPath);
                return true;
            }

            return false;
        }
    }
}