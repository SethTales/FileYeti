using System;
using System.Collections.Generic;
using System.Text;

namespace FileYetiServer.Models
{
    public class UpdateJobRequest
    {
        public Guid JobGuid { get; set; }
        public DateTime ReceiptDateTime { get; set; }
        public JobStatus Status { get; set; }
    }
}
