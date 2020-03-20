using System;
using System.Collections.Generic;
using System.Text;

namespace litews.Models
{
    public class RequestModel
    {
        public RequestType Type { get; set; }
        public string Path { get; set; }
        public string HttpVersion { get; set; }
        public List<string> Accept { get; set; }
        public List<string> AcceptEncoding { get; set; }
        public List<string> AcceptLanguage { get; set; }
        public string Connection { get; set; }
        public string Cookie { get; set; }
        public string Host { get; set; }
        public string Refer { get; set; }
        public string UserAgent { get; set; }
        public string ContentLength { get; set; }
        public List<OtherRequestModel> Other { get; set; }

    }
}
