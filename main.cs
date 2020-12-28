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
            Console.WriteLine("Validating whitelist integrity..");

            string userName = Environment.UserName;
            string authfile = $@"C:\Users\{userName}\spiroauthorization.cpp";
            bool exists = File.Exists(authfile);


            if (exists == true)
            {
                WebClient wc = new WebClient();
                string receive = wc.DownloadString("im not leaking this lmao");
                StreamReader sr = new StreamReader($@"C:\Users\{userName}\auth.txt");
                string line = sr.ReadLine();

                if (receive.Contains(line))
                {

                 

                    string who, x, data;
                    int y, z, timeout;
    

                    Console.WriteLine("Loading assets..");

                    Console.Write("Please provide a IP to ping: ");

                    who = Console.ReadLine();

                    Console.Write("How many times should I ping that ip: ");

                    x = Console.ReadLine();

                    y = Int32.Parse(x);


                    Console.WriteLine(" ");

                    for (z = 0; z < y; ++z)
                    {
                        Console.WriteLine($"----Ping {z + 1} ----");

                        AutoResetEvent waiter = new AutoResetEvent(false);

                        Ping pingSender = new Ping();

                        pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

                        data = "bruhhhhhhhhhhhhhhhhhhhhhhhhhhhhh";
                        byte[] buffer = Encoding.ASCII.GetBytes(data);

                        timeout = 12000;

                        PingOptions options = new PingOptions(64, true);

                        Console.WriteLine("Time to live: {0}", options.Ttl);
                        Console.WriteLine("Don't fragment: {0}", options.DontFragment);

                        pingSender.SendAsync(who, timeout, buffer, options, waiter);

                        waiter.WaitOne();


                    }
                }
                else
                {
                    Console.WriteLine("Failed to validate whitelist integrety.");
                    string awaitleave = Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Could not find data to read. Starting authorization process.");
                Console.Write("Please enter your authorization token: ");
                string authinput = Console.ReadLine();
                File.WriteAllText(authfile, authinput);
                Console.WriteLine("Attempted to authorize, please relaunch  Pinger.");
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
