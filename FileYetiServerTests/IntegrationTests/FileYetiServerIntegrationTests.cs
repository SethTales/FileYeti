using System;
using System.IO;
using FileYetiServer.Data;
using FileYetiServer.Services;
using NUnit.Framework;

namespace FileYetiServerTests.IntegrationTests
{
    [TestFixture]
    public class FileYetiServerIntegrationTests
    {
        private ListenerService _listenerService;
        private ITransferJobRepository _jobRepository;
        private IDiskRepository _diskRepository;
        private string _storageRoot;
        private TestTcpClient _testTcpClient;

        [SetUp]
        public void SetUp()
        {
            _jobRepository = new TransferJobRepository();
            _diskRepository = new DiskRepository();
            _storageRoot = "C:/temp";
            _listenerService = new ListenerService(_jobRepository, _diskRepository, _storageRoot);
            _testTcpClient = new TestTcpClient();
        }

        public void Test()
        {

        }
    }
}
