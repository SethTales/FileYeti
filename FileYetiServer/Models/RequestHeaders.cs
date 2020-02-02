using System;
using System.Collections.Generic;
using System.Text;

namespace FileYetiServer.Models
{
    public class RequestHeaders
    {
        public Guid JobGuid { get; set; }
        public string FileName { get; set; }
        public RequestType RequestType { get; set; }
        public int TotalChunks { get; set; }
        public int ChunkSizeBytes { get; set; }
        public int ChunkNumber { get; set; }

    }
}
