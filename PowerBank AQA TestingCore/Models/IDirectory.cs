namespace PowerBank_AQA_TestingCore.Models
{
    public interface IDirectory
    {
        void Create();

        string Get();

        bool Exist();

        IEnumerable<FileInfo> GetFiles(string searchPattern);
    }
}
