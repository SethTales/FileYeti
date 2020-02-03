using System;
using System.Linq;
using FileYeti.SharedModels.Enums;
using FileYetiServer.Data.Models;
using FileYetiServer.Models;

namespace FileYetiServer.Data.Repositories
{
    public interface ITransferJobRepository
    {
        Guid CreateJob(RequestHeaders headers);
        void UpdateJob(RequestHeaders request);
        TransferJob RetrieveJob(Guid jobGuid);
    }

    public class TransferJobRepository : ITransferJobRepository
    {
        public Guid CreateJob(RequestHeaders headers)
        {
            var job = new TransferJob
            {
                JobGuid = Guid.NewGuid(),
                ChunkSizeBytes = headers.ChunkSizeBytes,
                TotalChunks = headers.TotalChunks,
                Status = JobStatus.Received
            };
            using (var dbContext = GetContext())
            {
                dbContext.TransferJobs.Add(job);
                dbContext.SaveChanges();
            }
            return job.JobGuid;
        }

        public void UpdateJob(RequestHeaders headers)
        {
            using (var dbContext = GetContext())
            {
                var existingJob = dbContext.TransferJobs.First(j => j.JobGuid == headers.JobGuid);
                existingJob.LastChunkRecieved = headers.ReceiptTimeStamp;
                existingJob.Status = headers.CommandType == CommandType.UploadChunk ? JobStatus.Processing :
                    headers.CommandType == CommandType.CompleteJob ? JobStatus.Complete : existingJob.Status;
                existingJob.TotalChunksReceived = headers.CommandType == CommandType.CompleteJob ? existingJob.TotalChunksReceived : headers.ChunkNumber;
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

        private FileYetiServerDbContext GetContext()
        {
            return new FileYetiServerDbContext();
        }
    }
}
