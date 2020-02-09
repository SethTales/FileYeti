using System;
using System.IO;

namespace FileYetiClient.Adapters
{
    public interface IReaderAdapter : IDisposable
    {
        void Read(byte[] buffer, int index, int count);
        byte[] ReadToEnd();
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

        public byte[] ReadToEnd()
        {
            var currentPosition = _reader.BaseStream.Position;
            var finalChunkSize = StreamLength - currentPosition;
            var finalChunkBuffer = new byte[finalChunkSize];
            _reader.Read(finalChunkBuffer, 0, finalChunkBuffer.Length);
            return finalChunkBuffer;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
