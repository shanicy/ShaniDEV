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

            Parallel.For(0, 1, Go);

            //var input = "";
            //Console.WriteLine("Enter a message. 'Q' to quit.");
            //while ((input = Console.ReadLine()) != "Q")
            //{
            //    JobDetails j = new JobDetails();

            //    Console.WriteLine("Enter ID");
            //    j.Id = int.Parse(Console.ReadLine());

            //    Console.WriteLine("Enter resolution H");
            //    int h = int.Parse(Console.ReadLine());
            //    Console.WriteLine("Enter resolution W");
            //    int w = int.Parse(Console.ReadLine());

            //    j.Resolutions.Add(new Resolution(w, h));

            //    j.Status = "Pending";

            //    j.Uri = new Uri("https://" + j.Id + j.Resolutions[0].Width + j.Resolutions[0].Height);

            //    proxy.ProcessJob(j);

            //    Console.WriteLine("Enter a message. 'Q' to quit.");

            //}

            Console.ReadKey();
        }

        private static void Go(int obj)
        {
            var j = GetJob();
            j.Status = "Pending";
            proxy.ProcessJob(j);

        }

        private static int counter = 0;
        private static object locker = new object();

        private static JobDetails GetJob()
        {
            JobDetails ej = new JobDetails();

            lock(locker)
                ej.Id = counter++;

            ej.Uri = new Uri("http://www.test.com");
            ej.Resolutions.Add(new Resolution(1024, 768));
            ej.Resolutions.Add(new Resolution(1920, 1080));

            return ej;
        }
    }
}
