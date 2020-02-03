using System;
using FileYeti.SharedModels.Enums;

namespace FileYeti.SharedModels.Responses
{
    public class UploadChunkResponse : UpdateJobResponse
    {
        public int ChunkNumber { get; set; }
    }
}
