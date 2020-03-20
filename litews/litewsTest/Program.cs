using litews;
using System;
using System.Net;

namespace litewsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress iPAddress = IPAddress.Any;
            int Port = 80;
            string RootPath = "E:\\MyWebServerRoot\\";
            if (args.Length == 3)
            {
                iPAddress = IPAddress.Parse(args[0]);
                Port = int.Parse(args[1]);
                RootPath = args[2] + "\\";
            }
            Server server = new Server(iPAddress, Port, RootPath);
            server.SuperList.Add(new RequestTextHandle());
            server.SuperList.Add(new RequextImgHandle());
            server.Start();
            while (server.IsWork)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
