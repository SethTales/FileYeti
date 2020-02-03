using System;
using FileYeti.SharedModels.Enums;

namespace FileYetiServer.Models
{
    public class RequestHeaders
    {
        public Guid JobGuid { get; set; }
        public string FileName { get; set; }
        public CommandType CommandType { get; set; }
        public int TotalChunks { get; set; }
        public int ChunkSizeBytes { get; set; }
        public int ChunkNumber { get; set; }
        public DateTime ReceiptTimeStamp { get; set; }

    }
}
