using System;
using System.IO;

namespace FileYetiClient.Adapters
{
    public interface IReaderAdapter : IDisposable
    {
        void Read(byte[] buffer, int index, int count);
        void ReadToEnd();
        long StreamLength { get; }

    }

    public class ReaderAdapter : IReaderAdapter
    {
        private readonly BinaryReader _reader;
        public long StreamLength { get; }

        public ReaderAdapter (string sourcePath)
        {
            _reader = new BinaryReader(File.OpenRead(sourcePath));
            StreamLength = _reader.BaseStream.Length;
        }

        public void Read(byte[] buffer, int index, int count)
        {
            _reader.Read(buffer, index, count);
        }

        public void ReadToEnd()
        {
            //TODO: implement logic to handle the last chunk, where reading a number of empty bytes into the stream is causing the file to be corrupted
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
