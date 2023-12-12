using Microsoft.AspNetCore.Http;
using System.IO;

public class FileHelper
{
    public static IFormFile ConvertToIFormFile(string filePath, string contentType = "application/octet-stream")
    {
        // Kiểm tra xem file có tồn tại không
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        // Đọc nội dung của file vào một mảng byte
        byte[] fileBytes = File.ReadAllBytes(filePath);

        // Tạo MemoryStream từ mảng byte đã đọc
        using (MemoryStream ms = new MemoryStream(fileBytes))
        {
            // Tạo IFormFile từ MemoryStream và contentType
            IFormFile formFile = new FormFile(ms, 0, ms.Length, "file", Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return formFile;
        }
    }
}
