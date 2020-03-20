using litews.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace litews.AcceptHandle
{
    public interface IRequestHandle
    {
        //处理请求的类别
        string HandleType { get; }


        ResponseModel GenerateResponse(string ServerPath, RequestModel RequestData);
    }
}
