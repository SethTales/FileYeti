using System;
using System.Collections.Generic;
using System.Text;

namespace FileYetiServer.Models
{
    public enum RequestType
    {
        InitiateUpload,
        UploadChunk,
        CompleteFileUpload,
        CompleteJob
    }
}
