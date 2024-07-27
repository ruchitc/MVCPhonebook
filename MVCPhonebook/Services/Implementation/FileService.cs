using MVCPhonebook.Services.Contract;

namespace MVCPhonebook.Services.Implementation
{
    public class FileService : IFileService
    {
        public string AddFileToUploads(IFormFile file)
        {
            var fileName = string.Empty;
            if (file != null && file.Length > 0)
            {
                // Process the uploaded file(eq. save it to disk)
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);

                // Save the file to storage and set path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    fileName = file.FileName;
                }
            }

            return fileName;
        }
    }
}
