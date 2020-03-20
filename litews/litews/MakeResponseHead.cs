using litews.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace litews
{

    public enum httpStatus
    {
        OK = 0,
        NOTFOUND,
        SERVERERROR,

    }
    public class MakeResponseHead
    {
        public static string Generate(httpStatus httpStatus, List<ResponseExpansionModel> headList, int iTotBytes = 0, string content_Type = "text/html;", string httpVersion = "HTTP/1.1")
        {

            String sBuffer = "";
            sBuffer = sBuffer + httpVersion + " " + GetResponseStatus(httpStatus) + "\r\n";
            sBuffer = sBuffer + "Content-Type: " + content_Type + "\r\n";
            foreach (var tmp in headList)
            {
                sBuffer += tmp.Key + ": " + tmp.Value + "\r\n";
            }
            if (iTotBytes != 0)
                sBuffer = sBuffer + "Content-Length: " + iTotBytes + "\r\n\r\n";
            return sBuffer;
        }
        private static string GetResponseStatus(httpStatus status)
        {
            switch (status)
            {
                case httpStatus.OK: return "200 OK";
                case httpStatus.NOTFOUND: return "404 Not Found";
                default: return "500 Server Err";
            }

        }
    }
}
