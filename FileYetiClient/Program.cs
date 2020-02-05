using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using FileYetiClient.Adapters;
using FileYetiClient.Services;
using Newtonsoft.Json;

namespace FileYetiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var chunkSizeBytes = Int32.Parse(args[0]);
            var sourcePath = args[1];
            var adapterFactory = new AdapterFactory();
            var senderService = new SenderService(adapterFactory, chunkSizeBytes, sourcePath);
            senderService.Send();
        }

        
    }
}
