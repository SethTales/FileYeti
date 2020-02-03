using System;
using FileYeti.SharedModels.Enums;

namespace FileYetiServer.Models.Requests
{
    public class UpdateJobRequest
    {
        public Guid JobGuid { get; set; }
        public DateTime ReceiptTimeStamp { get; set; }
        public JobStatus Status { get; set; }
    }
}
