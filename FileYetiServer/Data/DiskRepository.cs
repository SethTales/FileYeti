using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileYetiServer.Data
{
    public interface IDiskRepository
    {
        void StreamBytesToFile(string path, byte[] bytes);
        void RenameFile(string basePath, string oldName, string newName);
    }
    public class DiskRepository : IDiskRepository
    {
        public void StreamBytesToFile(string path, byte[] bytes)
        {
            using (var stream = new FileStream(path, FileMode.Append))
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
