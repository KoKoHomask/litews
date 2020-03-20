using litews;
using litews.AcceptHandle;
using litews.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace litewsTest
{
    public class RequestTextHandle : IRequestHandle
    {
        protected string sMimeType = "text/";
        public string HandleType { get; } = "text/html,application/json";
        public RequestTextHandle()
        {

        }
        protected RequestTextHandle(string handleType)
        {
            HandleType = handleType;
        }

        public ResponseModel GenerateResponse(string ServerPath, RequestModel RequestData)
        {
            var req = RequestData;
            ResponseModel model = new ResponseModel();
            if (req.Type != RequestType.GET)
            {
                Console.WriteLine("只处理get请求类型..");
                return null;
            }

            Console.WriteLine("请求文件目录 : " + req.Path);

            if (req.Path.Length == 0)
            {
                var sErrorMessage = "<H2>Error!! Requested Directory does not exists</H2><Br>";
                model.HeadeData = Encoding.ASCII.GetBytes(MakeResponseHead.Generate(httpStatus.NOTFOUND, headInfo));// GeneralHeader(req.HttpVersion, "", sErrorMessage.Length, " 404 Not Found");
                model.BodyData = Encoding.Default.GetBytes(sErrorMessage);
                return model;
            }


            if (req.Path == "/")
            {
                // 取得请求文件名
                req.Path = "/index.html";
            }


            string extension = System.IO.Path.GetExtension(req.Path).Replace(".", "");
            if (extension.ToLower() == "js")
                extension = "javascript";
            var _mimeType = sMimeType + extension;

            var sPhysicalFilePath = (ServerPath + req.Path).Replace("/", "\\").Replace("\\\\", "\\").Replace("\\\\", "\\");
            Console.WriteLine("请求文件: " + sPhysicalFilePath);
            if (req.Path.ToLower().IndexOf("/api") >= 0)
            {
                _mimeType = sMimeType + "json";
                testmodel obj = new testmodel() { data = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 } };
                string str = JsonConvert.SerializeObject(obj);
                model.HeadeData = Encoding.ASCII.GetBytes(MakeResponseHead.Generate(httpStatus.OK, headInfo, str.Length, _mimeType));// GeneralHeader(req.HttpVersion, _mimeType, iTotBytes, " 200 OK");
                model.BodyData = Encoding.ASCII.GetBytes(str);
                return model;
            }
            else if (File.Exists(sPhysicalFilePath) == false)
            {

                var sErrorMessage = "<H2>404 Error! File Does Not Exists...</H2>";
                model.HeadeData = Encoding.ASCII.GetBytes(MakeResponseHead.Generate(httpStatus.NOTFOUND, headInfo, sErrorMessage.Length, _mimeType));//GeneralHeader(req.HttpVersion, "", sErrorMessage.Length, " 404 Not Found");//
                model.BodyData = Encoding.Default.GetBytes(sErrorMessage);
                return model;
            }
            else
            {
                int iTotBytes = 0;

                var sResponse = "";

                FileStream fs = new FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

                BinaryReader reader = new BinaryReader(fs);
                byte[] bytes = new byte[fs.Length];
                int read;
                while ((read = reader.Read(bytes, 0, bytes.Length)) != 0)
                {
                    sResponse = sResponse + Encoding.ASCII.GetString(bytes, 0, read);
                    iTotBytes = iTotBytes + read;
                }
                reader.Close();
                fs.Close();
                model.HeadeData = Encoding.ASCII.GetBytes(MakeResponseHead.Generate(httpStatus.OK, headInfo, iTotBytes, _mimeType));// GeneralHeader(req.HttpVersion, _mimeType, iTotBytes, " 200 OK");
                model.BodyData = bytes;
                return model;
            }
            throw new NotImplementedException();
        }
        List<ResponseExpansionModel> headInfo = new List<ResponseExpansionModel>()
        {
            new ResponseExpansionModel(){ Key="Server",Value="cx1193719-b"},
            new ResponseExpansionModel(){ Key="Accept-Ranges",Value="bytes"},
        };
    }
}
