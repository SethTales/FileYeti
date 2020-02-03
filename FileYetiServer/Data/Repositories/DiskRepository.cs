using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileYetiServer.Data.Repositories
{
    public interface IDiskRepository
    {
        void StreamBytesToFile(string fileName, byte[] bytes);
        void RenameFile(string basePath, string oldName, string newName);
    }

    public class DiskRepository : IDiskRepository
    {
        private readonly string _storageRoot;

        public DiskRepository(string storageRoot)
        {
            _storageRoot = storageRoot;
        }

        public void StreamBytesToFile(string fileName, byte[] bytes)
        {
            using (var stream = new FileStream(Path.Combine(_storageRoot, fileName), FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public void RenameFile(string basePath, string oldName, string newName)
        {
            File.Move(Path.Combine(basePath, oldName), Path.Combine(basePath, newName));
        }
    }
}
