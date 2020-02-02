using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace FileYetiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var senderService = new SenderService();
            senderService.SendData(32768, Path.Combine(Directory.GetCurrentDirectory(), "1000_files_1kb_each.zip"));
        }

        
    }
}
