using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace PL.Helper
{
    public static class DocumentsSettings
    {
        public static string UploadFile(IFormFile file,string FolderName)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", FolderName);

            var fileName = $"{Guid.NewGuid()}{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile(string FolderName,string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", FolderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
