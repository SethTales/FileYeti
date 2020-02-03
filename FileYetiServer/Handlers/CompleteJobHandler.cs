using System.Net.Sockets;
using System.Text;
using FileYeti.SharedModels.Enums;
using FileYeti.SharedModels.Responses;
using FileYetiServer.Data.Repositories;
using FileYetiServer.Models;
using Newtonsoft.Json;

namespace FileYetiServer.Handlers
{
    public class CompleteJobHandler : ICommandHandler
    {
        private readonly ITransferJobRepository _jobRepository;

        public CompleteJobHandler(ITransferJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public void Handle(NetworkStream stream, RequestHeaders headers)
        {
            _jobRepository.UpdateJob(headers);
            var completeJobResponse = new CompleteJobResponse
            {
                JobGuid = headers.JobGuid,
                Status = JobStatus.Complete,
                TotalChunksProcessed = _jobRepository.RetrieveJob(headers.JobGuid).TotalChunksReceived
            };
            var jsonResponse = JsonConvert.SerializeObject(completeJobResponse);
            byte[] responseMessage = Encoding.ASCII.GetBytes(jsonResponse);
            stream.Write(responseMessage, 0, responseMessage.Length);
        }
    }
}
