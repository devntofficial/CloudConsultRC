using CloudConsult.Doctor.Domain.Services;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class FileService : IFileService
    {
        public List<FileInfo> GetFilesInfoFromDirectory(string directory, string searchPattern)
        {
            var directoryInfo = new DirectoryInfo(directory);
            return directoryInfo.GetFiles(searchPattern).ToList();
        }
    }
}
