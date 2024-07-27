namespace MVCPhonebook.Services.Contract
{
    public interface IFileService
    {
        string AddFileToUploads(IFormFile file);
    }
}
