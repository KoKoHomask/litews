using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace litews.Models
{
    enum ReponseType
    {
        HTTPHEADER,
        HTTPBODY,
    }
    internal class HttpModel
    {
        public byte[] Data { get; set; }
        public ReponseType RType { get; set; }
    }
    internal class ConnectModel
    {
        public Socket client { get; set; }
        public byte[] rData { get; set; }
    }
}
