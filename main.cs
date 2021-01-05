using System;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using System.IO;

namespace pinger
{
    class Program
    {
        public static void Main(string[] args)
        {
                  
            
                    string who, x, data;
                    int y, z, timeout;
   
                    Console.Write("Please provide a IP to ping: ");

                    who = Console.ReadLine();

                    Console.Write("How many times should I ping that ip: ");

                    x = Console.ReadLine();

                    y = Int32.Parse(x);

                    for (z = 0; z < y; ++z)
                    {
                        Console.WriteLine($"----Ping {z + 1} ----");

                        AutoResetEvent waiter = new AutoResetEvent(false);

                        Ping pingSender = new Ping();

                        pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

                        data = "12345678912345678912345678912345";
                        byte[] buffer = Encoding.ASCII.GetBytes(data);

                        timeout = 12000;

                        PingOptions options = new PingOptions(64, true);

                        Console.WriteLine("Time to live: {0}", options.Ttl);
                        Console.WriteLine("Don't fragment: {0}", options.DontFragment);

                        pingSender.SendAsync(who, timeout, buffer, options, waiter);

                        waiter.WaitOne();


                    }
                        Console.ReadLine();
        }


        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
                Console.WriteLine("Ping canceled");

                ((AutoResetEvent)e.UserState).Set();
            }

            if (e.Error != null)
            {
                Console.WriteLine("Ping failed");
                Console.WriteLine(e.Error.ToString());

                ((AutoResetEvent)e.UserState).Set();
            }

            PingReply reply = e.Reply;

            DisplayReply(reply);

            ((AutoResetEvent)e.UserState).Set();
        }

        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)
                return;

            Console.WriteLine("Ping status: {0}", reply.Status);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }

        }

    }
}
