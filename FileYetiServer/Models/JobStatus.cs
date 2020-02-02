using System;
using System.Collections.Generic;
using System.Text;

namespace FileYetiServer.Models
{
    public enum JobStatus
    {
        Received,
        Started,
        Paused,
        Complete,
        Errored
    }
}
