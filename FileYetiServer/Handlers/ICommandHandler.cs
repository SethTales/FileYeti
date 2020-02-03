using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using FileYetiServer.Models;

namespace FileYetiServer.Handlers
{
    public interface ICommandHandler
    {
        void Handle(NetworkStream stream, RequestHeaders headers);
    }
}
