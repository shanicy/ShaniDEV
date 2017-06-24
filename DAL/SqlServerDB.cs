﻿using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using DAL.Properties;

namespace DAL
{
    public class SqlServerDB : IEncodingDB
    {
        public JobDetails GetJobDetails(long id)
        {
            try
            {
                using (var md = new EncodingJobsContext(Settings.Default.ConString))
                {
                    var job = md.EncodingJobs.FirstOrDefault(x => x.Id == id);

                    if (job != null)
                    {
                        return job;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public long InsertJob(JobDetails jd)
        {
            try
            {
                using (var md = new EncodingJobsContext(Settings.Default.ConString))
                {
                    md.EncodingJobs.InsertOnSubmit(jd);
                    md.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            return jd.Id;
        }

        public void UpdateStatus(long id, string status)
        {
            try
            {
                using (var md = new EncodingJobsContext(Settings.Default.ConString))
                {
                    var job = md.EncodingJobs.FirstOrDefault(x => x.Id == id);

                    if (job != null)
                    {
                        job.Status = status;

                        md.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
