using litews;
using litews.AcceptHandle;
using litews.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace litewsTest
{
    public class RequestJsonHandle : IRequestHandle
    {
        protected string sMimeType = "text/";
        public string HandleType { get; } = "application/json";
        public RequestJsonHandle() { }
        protected RequestJsonHandle(string handleType)
        {
            HandleType = handleType;
        }
        public ResponseModel GenerateResponse(string ServerPath, RequestModel RequestData)
        {
            var req = RequestData;
            ResponseModel model = new ResponseModel();
            //if (req.Type != RequestType.GET)
            //{
            //    Console.WriteLine("只处理get请求类型..");
            //    return null;
            //}

            Console.WriteLine("请求文件目录 : " + req.Path);

            if (req.Path.Length == 0)
            {
                var sErrorMessage = "<H2>Error!! Requested Directory does not exists</H2><Br>";
                model.HeadeData = Encoding.ASCII.GetBytes(MakeResponseHead.Generate(httpStatus.NOTFOUND, headInfo));// GeneralHeader(req.HttpVersion, "", sErrorMessage.Length, " 404 Not Found");
                model.BodyData = Encoding.Default.GetBytes(sErrorMessage);
                return model;
            }
            var _mimeType = sMimeType + "html";
            if (req.Type == RequestType.POST||req.Path.ToLower().IndexOf("/api") >= 0)
            {
                _mimeType = sMimeType + "json";
                testmodel obj = new testmodel() { data = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 } };
                string str = JsonConvert.SerializeObject(obj);
                model.HeadeData = Encoding.ASCII.GetBytes(MakeResponseHead.Generate(httpStatus.OK, headInfo, str.Length, _mimeType));// GeneralHeader(req.HttpVersion, _mimeType, iTotBytes, " 200 OK");
                model.BodyData = Encoding.ASCII.GetBytes(str);
                return model;
            }
            else
            {
                var sErrorMessage = "<H2>404 Error! File Does Not Exists...</H2>";
                model.HeadeData = Encoding.ASCII.GetBytes(MakeResponseHead.Generate(httpStatus.NOTFOUND, headInfo, sErrorMessage.Length, _mimeType));//GeneralHeader(req.HttpVersion, "", sErrorMessage.Length, " 404 Not Found");//
                model.BodyData = Encoding.Default.GetBytes(sErrorMessage);
                return model;
            }

        }
        List<ResponseExpansionModel> headInfo = new List<ResponseExpansionModel>()
        {
            new ResponseExpansionModel(){ Key="Server",Value="cx1193719-b"},
            new ResponseExpansionModel(){ Key="Accept-Ranges",Value="bytes"},
            new ResponseExpansionModel(){Key="Access-Control-Allow-Origin",Value="*"},
        };
    }
}
