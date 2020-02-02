using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using FileYetiServer.Models;
using Newtonsoft.Json;

namespace FileYetiServerTests.IntegrationTests
{
    internal class TestTcpClient
    {
        internal const int HeaderSize = 256;
        
        internal void SendData(RequestType requestType, int chunkSizeBytes, string sourcePath)
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
                        FileName = sourcePath.Split(Path.PathSeparator).Last(),
                        ChunkSizeBytes = chunkSizeBytes,
                        RequestType = RequestType.InitiateUpload,
                        TotalChunks = numberOfChunks
                    };
                    NetworkStream stream = client.GetStream();

                    SendHeaders(stream, headers);

                    for (int i = 0; i < numberOfChunks; i++)
                    {
                        byte[] chunkBytes = new byte[headers.ChunkSizeBytes];
                        while ((i = reader.Read(chunkBytes, 0, chunkBytes.Length)) < chunkSizeBytes * (i + 1))
                        { }

                        SendDataWithHeaders(stream, headers, chunkBytes);
                        // Buffer to store the response bytes.
                        var data = new Byte[256];

                        // String to store the response ASCII representation.
                        String responseData = String.Empty;

                        // Read the first batch of the TcpServer response bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        Console.WriteLine("Received: {0}", responseData);

                    }

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
            return (int) streamLength / chunkSizeBytes + 1;
        }

        private void SendHeaders(NetworkStream stream, RequestHeaders headers)
        {
            var headersBytes = CreatePaddedHeadersArray(headers);
            stream.Write(headersBytes, 0, headersBytes.Length);
        }

        private void SendDataWithHeaders(NetworkStream stream, RequestHeaders headers, byte[] sourceData)
        {
            var headersBytes = CreatePaddedHeadersArray(headers);
            var requestData = new byte[HeaderSize + headers.ChunkSizeBytes];
            Array.Copy(headersBytes, 0, requestData, 0, headersBytes.Length);
            Array.Copy(sourceData, 0, requestData, HeaderSize, sourceData.Length);
            stream.Write(requestData, 0, requestData.Length);

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
