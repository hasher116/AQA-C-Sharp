using System;

namespace PowerBank_AQA_TestingCore.Models
{
    public abstract class DefaultDirectory : IDirectory
    {
        private readonly AsyncLocal<DirectoryInfo> _directory = new();

        public void Create()
        {
            _directory.Value = new DirectoryInfo(Get());
        }

        public bool Exist()
        {
            return _directory.Value.Exists;
        }

        public abstract string Get();

        public IEnumerable<FileInfo> GetFiles(string searchPattern)
        {
            return _directory.Value.GetFiles(searchPattern).ToList();
        }
    }
}
