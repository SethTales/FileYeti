using System;
using System.Collections.Generic;
using System.Net.Sockets;
using FileYeti.SharedModels.Enums;
using FileYetiServer.Data.Repositories;

namespace FileYetiServer.Handlers
{
    public class HandlerFactory
    {
        private readonly Dictionary<CommandType, (Type classType, object[] ctorArgs)> _commandTypeHandlerMap;

        public HandlerFactory(ITransferJobRepository jobRepository, IDiskRepository diskRepository, TcpListener server, TcpClient client)
        {
            _commandTypeHandlerMap =
                new Dictionary<CommandType, (Type, object[])>
                {
                    {CommandType.InitiateUpload, (typeof(InitiateUploadHandler), new object[] { jobRepository }) },
                    {CommandType.UploadChunk, (typeof(UploadChunkHandler), new object[] { jobRepository, diskRepository} )},
                    {CommandType.CompleteJob, (typeof(CompleteJobHandler), new object[] { jobRepository } )},
                    {CommandType.Terminate, (typeof(TerminationHandler), new object[] { server, client } )}
                };
        }

        public ICommandHandler SelectHandler(CommandType commandType)
        {
            var classType = _commandTypeHandlerMap[commandType].classType;
            return (ICommandHandler)Activator.CreateInstance(classType, _commandTypeHandlerMap[commandType].ctorArgs);
        }
    }
}
