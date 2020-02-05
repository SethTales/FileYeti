using System;
using System.Net.Sockets;
using FileYeti.SharedModels.Enums;
using FileYeti.SharedModels.Responses;
using FileYetiClient.Adapters.Handlers;
using FileYetiServer.Models;

namespace FileYetiClient.Adapters
{
    public interface ITcpClientAdapter : IDisposable
    {
        UpdateJobResponse SendInitiateJobRequest(string fileName, int chunkSizeBytes, int totalChunks);

        UpdateJobResponse SendUploadChunkRequest(string fileName, Guid jobGuid, int chunkNumber, byte[] sourceDataChunk);

        CompleteJobResponse SendCompleteJobRequest(Guid jobGuid);
    }
    public class TcpClientAdapter : ITcpClientAdapter
    {
        private readonly TcpClient _client;
        private readonly int _chunkSizeBytes;
        private readonly NetworkStream _stream;
        private readonly IRequestHandler _requestHandler;
        private readonly IResponseHandler _responseHandler;

        public TcpClientAdapter(string hostName, int port, int chunkSizeBytes, IRequestHandler requestHandler, IResponseHandler responseHandler)
        {
            _client = new TcpClient(hostName, port);
            _chunkSizeBytes = chunkSizeBytes;
            _stream = _client.GetStream();
            _requestHandler = requestHandler;
            _responseHandler = responseHandler;
        }

        public UpdateJobResponse SendInitiateJobRequest(string fileName, int chunkSizeBytes, int totalChunks)
        {
            var headers = new RequestHeaders
            {
                ChunkNumber = 0,
                FileName = fileName,
                ChunkSizeBytes = chunkSizeBytes,
                CommandType = CommandType.InitiateUpload,
                TotalChunks = totalChunks
            };
            var serializedHeaders = _requestHandler.SerializeHeaders(headers);
            _stream.Write(serializedHeaders, 0, serializedHeaders.Length);

            var data = new byte[256];
            _stream.Read(data, 0, data.Length);

            var createJobResponse = _responseHandler.DeserializeResponse<UpdateJobResponse>(data);
            return createJobResponse;
        }

        public UpdateJobResponse SendUploadChunkRequest(string fileName, Guid jobGuid, int chunkNumber, byte[] sourceDataChunk)
        {
            var headers = new RequestHeaders
            {
                ChunkNumber = chunkNumber,
                FileName = fileName,
                JobGuid = jobGuid,
                CommandType = CommandType.UploadChunk,
                ChunkSizeBytes = _chunkSizeBytes
            };
            var serializedHeaders = _requestHandler.SerializeHeaders(headers);
            var serializedData = _requestHandler.SerializeData(headers, serializedHeaders, sourceDataChunk);
            _stream.Write(serializedData, 0, serializedData.Length);

            var data = new byte[256];
            _stream.Read(data, 0, data.Length);

            var uploadChunkResponse = _responseHandler.DeserializeResponse<UpdateJobResponse>(data);
            return uploadChunkResponse;
        }

        public CompleteJobResponse SendCompleteJobRequest(Guid jobGuid)
        {
            var headers = new RequestHeaders
            {
                JobGuid = jobGuid,
                CommandType = CommandType.CompleteJob
            };
            var serializedHeaders = _requestHandler.SerializeHeaders(headers);
            _stream.Write(serializedHeaders, 0, serializedHeaders.Length);

            var data = new byte[256];
            _stream.Read(data, 0, data.Length);

            var completeJobRespose = _responseHandler.DeserializeResponse<CompleteJobResponse>(data);
            return completeJobRespose;
        }

        public void Dispose()
        {
            _stream.Close();
            _client.Close();
        }
    }
}
