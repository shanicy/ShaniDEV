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
    public class EncodingWorkflow : IEncodingFlow
    {
        #region Members

        private DAL.DAL m_DAL;

        public TransformBlock<JobDetails, JobDetails> download { get; private set; }

        #endregion

        #region Methods

        public JobDetails Download(JobDetails jd)
        {
            // TODO: Validate status: If already downloaded, continue

            jd.Status = "Downloading";
            // Update status in the DB 
            m_DAL.UpdateStatus(jd.Id, "Downloading");

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

            jd.Status = "Encoding";
            // Update status in the DB 
            m_DAL.UpdateStatus(jd.Id, "Encoding");

            // *** Encode Logic.... Use external FTP dll
            // Return new path to encoded file

            Console.WriteLine("Encoding..." + jd.Id);

            return jd;
        }

        public void UploadToFTP(JobDetails jd)
        {
            // TODO: Validate status: If already uploaded, validate, set status to done.

            jd.Status = "Uploading to FTP";

            // Update status in the DB 
            m_DAL.UpdateStatus(jd.Id, "Uploading to FTP");

            Console.WriteLine("Uploading..." + jd.Id);

            // *** Upload Logic.... Use external FTP dll
            // Get FTP path from the config

            // Update DONE status in the DB 
            m_DAL.UpdateStatus(jd.Id, "DONE");
            jd.Status = "DONE";

        }

        public void PostToFlow(JobDetails j)
        {
            download.Post(j);
            //download.Complete();
        }

        /// <summary>
        /// Setup the data flow
        /// </summary>
        public void InitFlow()
        {
            // TODO: Move the options into config
            ExecutionDataflowBlockOptions opt = new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 8 };

            download = new TransformBlock<JobDetails, JobDetails>(jd =>
            {
                try
                {
                    Download(jd);
                }
                catch (Exception ex)
                {
                    // TODO: Write to log and error report
                    throw new Exception(string.Format("Error while downloading the file, id: {0}. msg: {1}", jd, ex.Message));
                }

                return jd;
            }, opt);

            var encode = new TransformBlock<JobDetails, JobDetails>(jd =>
            {
                try
                {
                    Encode(jd);
                }
                catch (Exception ex)
                {
                    // TODO: Write to log and error report
                    throw new Exception(string.Format("Error while encoding the file, id: {0}. msg: {1}", jd, ex.Message));
                }
                return jd;

            }, opt);

            var ftp = new ActionBlock<JobDetails>(jd =>
            {
                try
                {
                    UploadToFTP(jd);
                }
                catch (Exception ex)
                {
                    // TODO: Write to log and error report
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
        }

        #endregion

        #region Ctor

        public EncodingWorkflow()
        {
            m_DAL = new DAL.DAL();
        }

        #endregion
    }
}
