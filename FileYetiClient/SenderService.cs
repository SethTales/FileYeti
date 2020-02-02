using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace FileYetiClient
{
    public class SenderService
    {
        private const int HeaderSize = 256;

        public void SendData(int chunkSizeBytes, string sourcePath)
        {
            try
            {
                Int32 port = 13000;
                TcpClient client = new TcpClient("Lenovo-34070", port);

                using (var reader = new BinaryReader(File.OpenRead(sourcePath)))
                {
                    var numberOfChunks = CalculateNumberOfChunks(reader.BaseStream.Length, chunkSizeBytes);
                    var headers = new RequestHeaders
                    {
                        ChunkNumber = 0,
                        FileName = sourcePath.Split(Path.DirectorySeparatorChar).Last(),
                        ChunkSizeBytes = chunkSizeBytes,
                        RequestType = RequestType.InitiateUpload,
                        TotalChunks = numberOfChunks
                    };
                    NetworkStream stream = client.GetStream();

                    var jobGuid = SendHeaders(stream, headers);

                    for (int i = 0; i < numberOfChunks; i++)
                    {
                        byte[] chunkBytes = new byte[headers.ChunkSizeBytes];
                        reader.Read(chunkBytes, 0, chunkBytes.Length);

                        headers.ChunkNumber = i + 1;
                        headers.RequestType = RequestType.UploadChunk;
                        headers.JobGuid = jobGuid;
                        SendDataWithHeaders(stream, headers, chunkBytes);
                        // Buffer to store the response bytes.
                        var data = new Byte[256];

                        // Read the first batch of the TcpServer response bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);
                        var responseData = Encoding.ASCII.GetString(data, 0, bytes);
                        Console.WriteLine("Received: {0}", responseData);
                    }

                    headers.RequestType = RequestType.CompleteJob;
                    CompleteJob(stream, headers);

                    // Close everything.
                    stream.Close();
                    client.Close();
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        private int CalculateNumberOfChunks(long streamLength, int chunkSizeBytes)
        {
            return (int)streamLength / chunkSizeBytes + 1;
        }

        private Guid SendHeaders(NetworkStream stream, RequestHeaders headers)
        {
            var headersBytes = CreatePaddedHeadersArray(headers);
            stream.Write(headersBytes, 0, headersBytes.Length);
            var data = new Byte[256];
            Int32 bytes = stream.Read(data, 0, data.Length);
            var jobGuidDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Encoding.ASCII.GetString(data, 0, bytes));
            return Guid.Parse(jobGuidDict["JobGuid"]);
        }

        private void SendDataWithHeaders(NetworkStream stream, RequestHeaders headers, byte[] sourceData)
        {
            var headersBytes = CreatePaddedHeadersArray(headers);
            var requestData = new byte[HeaderSize + headers.ChunkSizeBytes];
            Array.Copy(headersBytes, 0, requestData, 0, headersBytes.Length);
            Array.Copy(sourceData, 0, requestData, HeaderSize, sourceData.Length);
            stream.Write(requestData, 0, requestData.Length);

        }

        private void CompleteJob(NetworkStream stream, RequestHeaders headers)
        {
            var headerBytes = CreatePaddedHeadersArray(headers);
            stream.Write(headerBytes, 0, headerBytes.Length);
        }

        private byte[] CreatePaddedHeadersArray(RequestHeaders headers)
        {
            var headersString = JsonConvert.SerializeObject(headers);
            byte[] headersBytes = Encoding.ASCII.GetBytes(headersString);
            byte[] paddedArray = new byte[256];
            Array.Copy(headersBytes, paddedArray, headersBytes.Length);
            return paddedArray;
        }
    }
}
