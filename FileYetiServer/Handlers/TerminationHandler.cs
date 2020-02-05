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

        public TerminationHandler(TcpListener server)
        {
            _server = server;
        }

        public void Handle(NetworkStream stream, RequestHeaders headers, TcpClient client)
        {
            if (client.Connected)
            {
                var terminationResponse = new TerminationResponse
                {
                    JobGuid = headers.JobGuid,
                    Status = JobStatus.Paused
                };
                var jsonResponse = JsonConvert.SerializeObject(terminationResponse);
                byte[] responseMessage = Encoding.ASCII.GetBytes(jsonResponse);
                stream.Write(responseMessage, 0, responseMessage.Length);
                client.Close();
            }

            stream.Close();
            _server.Stop();
            Environment.Exit(0);
        }
    }
}
