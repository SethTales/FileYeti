using System;
using System.Net.Sockets;
using System.Text;
using FileYeti.SharedModels.Enums;
using FileYeti.SharedModels.Responses;
using FileYetiServer.Models;
using Newtonsoft.Json;

namespace FileYetiServer.Handlers
{
    public class TerminationHandler : ICommandHandler
    {
        private readonly TcpListener _server;
        private readonly TcpClient _client;

        public TerminationHandler(TcpListener server, TcpClient client)
        {
            _server = server;
            _client = client;
        }

        public void Handle(NetworkStream stream, RequestHeaders headers)
        {
            if (_client.Connected)
            {
                var uploadChunkResponse = new TerminationResponse
                {
                    JobGuid = headers.JobGuid,
                    Status = JobStatus.Paused
                };
                var jsonResponse = JsonConvert.SerializeObject(uploadChunkResponse);
                byte[] responseMessage = Encoding.ASCII.GetBytes(jsonResponse);
                stream.Write(responseMessage, 0, responseMessage.Length);
                _client.Close();
            }

            stream.Close();
            _server.Stop();
            Environment.Exit(0);
        }
    }
}
