using System;
using System.Collections.Generic;
using System.Text;

namespace FileYetiServer.Models
{
    public class InitiateUploadRequest
    {
        public string FileName { get; set; }
        public int NumberOfChunks { get; set; }
        public int BytesPerChunk { get; set; }
        public Guid JobGuid { get; set; }

    }
}
