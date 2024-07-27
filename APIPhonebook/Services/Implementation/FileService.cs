using APIPhonebook.Services.Contract;
using System.Diagnostics.CodeAnalysis;

namespace APIPhonebook.Services.Implementation
{
    [ExcludeFromCodeCoverage]
    public class FileService : IFileService
    {
        public byte[] ToByteArray(IFormFile file)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                bytes = ms.ToArray();
            }
            return bytes;
        }
    }
}
