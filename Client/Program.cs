using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Client.EncodingServiceRef;

namespace Client
{
    class Program
    {
        static EncodingServiceClient proxy;

        static void Main(string[] args)
        {
            proxy = new EncodingServiceClient();
            proxy.Open();
            Console.WriteLine("Connected");

            Parallel.For(1, 2, FakeAJob);

            Console.WriteLine("DONE");
            Console.ReadKey();
        }

        private static void FakeAJob(int obj)
        {
            var j = GetJob();
            j.Status = EncodingJobStatus.Pending;
            proxy.ProcessJob(j);

        }

        private static object locker = new object();

        private static JobDetails GetJob()
        {
            JobDetails ej = new JobDetails();

            ej.Uri = new Uri("http://www.test.com");
            ej.Resolutions.Add(new Resolution(1024, 768));
            ej.Resolutions.Add(new Resolution(1920, 1080));

            return ej;
        }
    }
}
