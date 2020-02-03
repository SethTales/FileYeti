using System.Net.Sockets;
using System.Text;
using FileYeti.SharedModels.Enums;
using FileYeti.SharedModels.Responses;
using FileYetiServer.Data.Repositories;
using FileYetiServer.Models;
using Newtonsoft.Json;

namespace FileYetiServer.Handlers
{
    public class UploadChunkHandler : ICommandHandler
    {
        private readonly ITransferJobRepository _jobRepository;
        private readonly IDiskRepository _diskRepository;

        public UploadChunkHandler(ITransferJobRepository jobRepository, IDiskRepository diskRepository)
        {
            _jobRepository = jobRepository;
            _diskRepository = diskRepository;
        }

        public void Handle(NetworkStream stream, RequestHeaders headers)
        {
            var bytes = new byte[headers.ChunkSizeBytes];

            stream.Read(bytes, 0, bytes.Length);
            _diskRepository.StreamBytesToFile(headers.FileName, bytes);
            _jobRepository.UpdateJob(headers);

            var uploadChunkResponse = new UploadChunkResponse
            {
                JobGuid = headers.JobGuid,
                ChunkNumber = headers.ChunkNumber,
                Status = JobStatus.Processing
            };
            var jsonResponse = JsonConvert.SerializeObject(uploadChunkResponse);
            byte[] responseMessage = Encoding.ASCII.GetBytes(jsonResponse);
            stream.Write(responseMessage, 0, responseMessage.Length);
        }
    }
}
