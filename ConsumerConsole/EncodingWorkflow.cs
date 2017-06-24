using Common.Entities;
using Common.Interfaces;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConsumerConsole
{
    /// <summary>
    /// Represents the workflow for encoding job
    /// </summary>
    public class EncodingWorkflow : IEncodingFlow
    {
        #region Members

        private DAL.DAL m_DAL;

        public TransformBlock<JobDetails, JobDetails> download { get; private set; }

        #endregion

        #region Ctor

        public EncodingWorkflow()
        {

        }

        #endregion

        #region Methods

        public JobDetails Download(JobDetails jd)
        {
            // TODO: Validate status: If already downloaded, continue

            jd.Status = EncodingJobStatus.Downloading;
            // Update status in the DB 
            m_DAL.UpdateStatus(jd.Id, EncodingJobStatus.Downloading);

            // *** Download Logic.... 
            //using (var client = new WebClient())
            //{
            //    client.DownloadFile(jd.Uri, "filename");
            //}

            //jd.LocalDownloadedFile = "";

            Console.WriteLine("Downloading" + jd.Id);

            return jd;
        }

        public JobDetails Encode(JobDetails jd)
        {
            // TODO: Validate status: If already encoded, continue

            jd.Status = EncodingJobStatus.Encoding;
            // Update status in the DB 
            m_DAL.UpdateStatus(jd.Id, EncodingJobStatus.Encoding);

            // *** Encode Logic.... Use external FTP dll - edtftpnet is great
            // Return new path to encoded file

            Console.WriteLine("Encoding..." + jd.Id);

            return jd;
        }

        public void UploadToFTP(JobDetails jd)
        {
            // TODO: Validate status: If already uploaded, validate, set status to done.

            jd.Status = EncodingJobStatus.Uploading;

            // Update status in the DB 
            m_DAL.UpdateStatus(jd.Id, EncodingJobStatus.Uploading);

            Console.WriteLine("Uploading..." + jd.Id);

            // Get FTP path from the config
            // *** Upload To FTP Logic.... Use external FTP dll

            // Update DONE status in the DB 
            jd.Status = EncodingJobStatus.Completed;
            m_DAL.UpdateStatus(jd.Id, EncodingJobStatus.Completed);
        }

        public void PostToFlow(JobDetails j)
        {
            download.Post(j);
        }

        /// <summary>
        /// Signals the queue about job completion
        /// </summary>
        private void SignalCompletedToQueue()
        {
            // TODO: Implement - raise event back to the queue with the relevant id
        }

        /// <summary>
        /// Setup the data flow
        /// </summary>
        public void InitFlow()
        {
            m_DAL = new DAL.DAL();

            // TODO: Move the options into config
            ExecutionDataflowBlockOptions opt = new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 8 };

            download = new TransformBlock<JobDetails, JobDetails>(jd =>
            {
                try
                {
                    return Download(jd);
                }
                catch (Exception ex)
                {
                    // TODO: Write to log and error report. Can use custom excetion too.
                    throw new Exception(string.Format("Error while downloading the file, id: {0}. msg: {1}", jd, ex.Message));
                }
            }, opt);

            var encode = new TransformBlock<JobDetails, JobDetails>(jd =>
            {
                try
                {
                    return Encode(jd);
                }
                catch (Exception ex)
                {
                    // TODO: Write to log and error report. Can use custom excetion too.
                    throw new Exception(string.Format("Error while encoding the file, id: {0}. msg: {1}", jd, ex.Message));
                }
            }, opt);

            var ftp = new ActionBlock<JobDetails>(jd =>
            {
                try
                {
                    UploadToFTP(jd);
                }
                catch (Exception ex)
                {
                    // TODO: Write to log and error report. Can use custom excetion too.
                    throw new Exception(string.Format("Error while uploading to FTP, id: {0}. msg: {1}", jd, ex.Message));
                }
            }, opt);

            // Links
            download.LinkTo(encode);
            encode.LinkTo(ftp);

            // Completions and error handling
            download.Completion.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    ((IDataflowBlock)encode).Fault(t.Exception);
                else
                    encode.Complete();
            });

            encode.Completion.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    ((IDataflowBlock)ftp).Fault(t.Exception);
                else
                    ftp.Complete();
            });

            ftp.Completion.ContinueWith(t =>
            {
                // Because the queue should be durable, a signaling is necessary.
                SignalCompletedToQueue();
            });
        }

        #endregion
    }
}
