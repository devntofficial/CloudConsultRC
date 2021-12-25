namespace CloudConsult.Doctor.Domain.Services
{
    public interface IFileService
    {
        List<FileInfo> GetFilesInfoFromDirectory(string directory, string searchPattern);
    }
}
