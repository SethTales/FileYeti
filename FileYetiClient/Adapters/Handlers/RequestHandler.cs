using System;
using System.Text;
using FileYetiServer.Models;
using Newtonsoft.Json;

namespace FileYetiClient.Adapters.Handlers
{
    public interface IRequestHandler
    {
        byte[] SerializeHeaders(RequestHeaders headers);
        byte[] SerializeData(RequestHeaders headers, byte[] serializedHeaders, byte[] sourceData);
    }
    public class RequestHandler : IRequestHandler
    {
        private readonly int _headerSize;

        public RequestHandler(int headerSize)
        {
            _headerSize = headerSize;
        }

        public byte[] SerializeHeaders(RequestHeaders headers)
        {
            var headersString = JsonConvert.SerializeObject(headers);
            byte[] headersBytes = Encoding.ASCII.GetBytes(headersString);
            byte[] paddedArray = new byte[256];
            Array.Copy(headersBytes, paddedArray, headersBytes.Length);
            return paddedArray;
        }

        public byte[] SerializeData(RequestHeaders headers, byte[] serializedHeaders, byte[] sourceData)
        {
            var requestData = new byte[_headerSize + headers.ChunkSizeBytes];
            Array.Copy(serializedHeaders, 0, requestData, 0, serializedHeaders.Length);
            Array.Copy(sourceData, 0, requestData, _headerSize, sourceData.Length);
            return requestData;
        }
    }
}
