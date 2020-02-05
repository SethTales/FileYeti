using System.Net.Sockets;
using System.Text;
using FileYeti.SharedModels.Enums;
using FileYeti.SharedModels.Responses;
using FileYetiServer.Data.Repositories;
using FileYetiServer.Models;
using Newtonsoft.Json;

namespace FileYetiServer.Handlers
{
    public class InitiateJobHandler : ICommandHandler
    {
        private readonly ITransferJobRepository _jobRepository;

        public InitiateJobHandler(ITransferJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public void Handle(NetworkStream stream, RequestHeaders headers, TcpClient client)
        {
            if (client.Connected)
            {
                var jobGuid = _jobRepository.CreateJob(headers);
                var createJobResponse = new UpdateJobResponse
                {
                    JobGuid = jobGuid,
                    Status = JobStatus.Received
                };
                var jsonResponse = JsonConvert.SerializeObject(createJobResponse);
                byte[] responseMessage = Encoding.ASCII.GetBytes(jsonResponse);
                stream.Write(responseMessage, 0, responseMessage.Length);
            }
        }
    }
}
