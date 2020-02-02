using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FileYetiServer.Models
{
    public class TransferJob
    {
        [Key]
        public int JobId { get; set; }
        [Required]
        public Guid JobGuid { get; set; }
        public int TotalChunks { get; set; }
        public int TotalChunksReceived { get; set; }
        public int ChunkSizeBytes { get; set; }
        public DateTime LastChunkRecieved { get; set; }
        public JobStatus Status { get; set; }
    }
}
