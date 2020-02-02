using System;
using System.Collections.Generic;
using System.Text;

namespace FileYetiClient
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

    public enum RequestType
    {
        InitiateUpload,
        UploadChunk,
        CompleteFileUpload,
        CompleteJob
    }
}
