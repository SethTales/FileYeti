using System;
using System.Collections.Generic;
using System.Text;

namespace FileYeti.SharedModels.Responses
{
    public class CompleteJobResponse : UpdateJobResponse
    {
        public int TotalChunksProcessed { get; set; }
    }
}
