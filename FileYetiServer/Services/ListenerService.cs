using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using FileYetiServer.Data;
using FileYetiServer.Models;
using Newtonsoft.Json;

namespace FileYetiServer.Services
{
    public interface IListenerService
    {
        void Listen();
    }

    public class ListenerService : IListenerService
    {
        private readonly ITransferJobRepository _transferJobRepository;
        private readonly IDiskRepository _diskRepository;
        private readonly string _storageRoot;

        public ListenerService(ITransferJobRepository transferJobRepository, IDiskRepository diskRepository, string storageRoot)
        {
            _transferJobRepository = transferJobRepository;
            _diskRepository = diskRepository;
            _storageRoot = storageRoot;
        }

        public void Listen()
        {
            //TODO: Should this be multi-threaded?
            TcpListener server = null;
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Any;
            server = new TcpListener(localAddr, port);
            server.Start();

            Thread tcpListenerThread = new Thread(() =>
            {
                try
                {
                    Console.Write("Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    while (client.Connected)
                    {
                        NetworkStream stream = client.GetStream();

                        var requestHeaders = ReadRequestHeaders(stream);

                        switch (requestHeaders.RequestType)
                        {
                            case RequestType.InitiateUpload:
                                var jobGuid = CreateNewJob(requestHeaders);
                                byte[] msg =
                                    Encoding.ASCII.GetBytes($"{{\"JobGuid\":\"{jobGuid}\"}}");
                                stream.Write(msg, 0, msg.Length);
                                break;
                            case RequestType.UploadChunk:
                                StreamChunkToDisk(requestHeaders, stream);
                                UpdateExistingJob(requestHeaders);
                                break;
                            case RequestType.CompleteFileUpload:
                                StreamChunkToDisk(requestHeaders, stream);
                                UpdateExistingJob(requestHeaders);
                                break;
                            case RequestType.CompleteJob:
                                UpdateExistingJob(requestHeaders);
                                _diskRepository.RenameFile(_storageRoot, requestHeaders.JobGuid.ToString(),
                                    requestHeaders.FileName);
                                client.Close();
                                break;
                        }

                    }

                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    server.Stop();
                }
            });
            tcpListenerThread.Start();
        }

        private RequestHeaders ReadRequestHeaders(NetworkStream stream)
        {
            //get request headers, which are the first 256 bytes of each request
            byte[] requestHeaderBytes = new byte[256];
            stream.Read(requestHeaderBytes, 0, 256);
            var requestHeaders = JsonConvert.DeserializeObject<RequestHeaders>(Encoding.ASCII.GetString(requestHeaderBytes, 0,
                    requestHeaderBytes.Length));

            return requestHeaders;
        }

        private Guid CreateNewJob(RequestHeaders headers)
        {
            //TODO: record file name, number of chunks, bytes per chunk and job ID
            var job = new TransferJob
            {
                JobGuid = Guid.NewGuid(),
                ChunkSizeBytes = headers.ChunkSizeBytes,
                TotalChunks = headers.TotalChunks,
                Status = JobStatus.Received
            };
            _transferJobRepository.AddJob(job);
            return job.JobGuid;
        }

        private void UpdateExistingJob(RequestHeaders headers)
        {
            var updateJobRequest = new UpdateJobRequest
            {
                JobGuid = headers.JobGuid,
                ReceiptDateTime = DateTime.UtcNow,
                Status =
                    headers.RequestType == RequestType.UploadChunk ? JobStatus.Started :
                    headers.RequestType == RequestType.CompleteFileUpload ? JobStatus.Started :
                    headers.RequestType == RequestType.CompleteJob ? JobStatus.Complete : JobStatus.Received
            };
            _transferJobRepository.UpdateJob(updateJobRequest);
        }

        private void StreamChunkToDisk(RequestHeaders requestHeaders, NetworkStream stream)
        {
            var bytes = new byte[requestHeaders.ChunkSizeBytes];

            stream.Read(bytes, 0, bytes.Length);
            _diskRepository.StreamBytesToFile(Path.Combine(_storageRoot, requestHeaders.JobGuid.ToString()), bytes);

            byte[] msg = 
                Encoding.ASCII.GetBytes($"Processed chunk {requestHeaders.ChunkNumber} of {requestHeaders.TotalChunks}");
            
            // Send back a response.
            stream.Write(msg, 0, msg.Length);
        }

    }
}
