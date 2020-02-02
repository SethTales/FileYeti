using System;
using System.Linq;
using FileYetiServer.Models;

namespace FileYetiServer.Data
{
    public interface ITransferJobRepository
    {
        void AddJob(TransferJob job);
        TransferJob RetrieveJob(Guid jobGuid);
        void UpdateJob(UpdateJobRequest request);
    }

    public class TransferJobRepository : ITransferJobRepository
    {
        public void AddJob(TransferJob job)
        {
            using (var dbContext = GetContext())
            {
                dbContext.TransferJobs.Add(job);
                dbContext.SaveChanges();
            }
        }

        public TransferJob RetrieveJob(Guid jobGuid)
        {
            using (var dbContext = GetContext())
            {
                return dbContext.TransferJobs.FirstOrDefault(j => j.JobGuid == jobGuid);
            }
        }

        public void UpdateJob(UpdateJobRequest request)
        {
            using (var dbContext = GetContext())
            {
                var existingJob = dbContext.TransferJobs.First(j => j.JobGuid == request.JobGuid);
                existingJob.LastChunkRecieved = request.ReceiptDateTime;
                existingJob.Status = request.Status;
                existingJob.TotalChunksReceived = existingJob.TotalChunksReceived + 1;
                dbContext.SaveChanges();
            }
        }

        private FileYetiServerDbContext GetContext()
        {
            return new FileYetiServerDbContext();
        }
    }
}
