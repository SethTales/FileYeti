using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FileYetiClient.Adapters.Handlers
{
    public interface IResponseHandler
    {
        TResponseType DeserializeResponse<TResponseType>(byte[] serializedResponse);
    }

    public class ResponseHandler : IResponseHandler
    {
        public TResponseType DeserializeResponse<TResponseType>(byte[] serializedResponse)
        {
            var responseString = Encoding.ASCII.GetString(serializedResponse, 0, serializedResponse.Length);
            return JsonConvert.DeserializeObject<TResponseType>(responseString);
        }
    }
}
