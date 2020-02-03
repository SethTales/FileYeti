using System;
using System.Collections.Generic;
using System.Text;
using FileYeti.SharedModels.Enums;

namespace FileYeti.SharedModels.Responses
{
    public class UpdateJobResponse
    {
        public Guid JobGuid { get; set; }
        public JobStatus Status { get; set; }
    }
}
