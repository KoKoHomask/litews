using litews.AcceptHandle;
using litews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace litews
{
    public class Server : IDisposable
    {
        private TcpListener myListener;
        private int port = 80; // 选者任何闲置端口
        private readonly string ServerPath;
        private bool isWork = false;
        public bool IsWork { get => isWork; }
        public List<IRequestHandle> SuperList { get; set; } = new List<IRequestHandle>();
        public Server(IPAddress ListenAddress, int ListenPort, string RootPath)
        {
            port = ListenPort;
            ServerPath = RootPath;
            myListener = new TcpListener(IPAddress.Any, port);
        }
        void ListenerBeginCall(IAsyncResult iResult)
        {
            var listener = iResult.AsyncState as TcpListener;
            Socket clientSocket = listener.EndAcceptSocket(iResult);
            Console.WriteLine("Socket Type " + clientSocket.SocketType);
            ConnectModel connect = new ConnectModel() { rData = new byte[1024], client = clientSocket };
            clientSocket.BeginReceive(connect.rData, 0, 1024, 0, ResultCallBace, connect);
            if (clientSocket.Connected)
                Console.WriteLine("\nClient Connected!!\n==================\nCLient IP {0}\n", clientSocket.RemoteEndPoint);
            if (isWork)
                listener.BeginAcceptSocket(ListenerBeginCall, listener);
        }
        void ResultCallBace(IAsyncResult asyncCall)
        {
            var connect = asyncCall.AsyncState as ConnectModel;

            string sBuffer = Encoding.Default.GetString(connect.rData).Replace("\0", "");
            HttpRequestAnalysis requestHeaders = new HttpRequestAnalysis();
            var req = requestHeaders.GetRequestHeaders(sBuffer);

            foreach (var single in SuperList)
            {
                if (req.Accept == null) break;
                //var isContain = req.Accept.Select(x => x.Contains(single.HandleType) == true).Where(y => y == true).FirstOrDefault();
                //var isContain= req.Accept.Select(x => single.HandleType.Split(',').Select(y => x.Contains(y) == true).FirstOrDefault()).FirstOrDefault();
                var isContain = req.Accept.Select(x => single.HandleType.Split(',').Select(y => x.Contains(y)).Where(y => y == true).FirstOrDefault()).Where(x => x == true).FirstOrDefault();
                if (isContain)
                {
                    var response = single.GenerateResponse(ServerPath, req);
                    if (response != null)
                    {
                        connect.client.Send(response.HeadeData);
                        connect.client.Send(response.BodyData);
                        connect.rData = new byte[1024];
                        if (isWork && req.Connection.ToLower() == "keep-alive")
                            connect.client.BeginReceive(connect.rData, 0, 1024, 0, ResultCallBace, connect);
                        return;
                    }
                }
            }
            connect.client.Close();
        }

        ~Server()
        {
            Dispose();
        }
        public void Dispose()
        {
            isWork = false;
            myListener.Stop();
        }
        public void Start()
        {
            try
            {
                isWork = true;
                myListener.Start();
                myListener.BeginAcceptSocket(ListenerBeginCall, myListener);
                Console.WriteLine("Web Server Running... Press ^C to Stop...");
            }
            catch (Exception e)
            {
                isWork = false;
                Console.WriteLine("兼听端口时发生错误 :" + e.ToString());
            }
        }
        public void Stop()
        {
            Dispose();
        }

    }
}
