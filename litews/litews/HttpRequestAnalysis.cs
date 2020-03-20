using litews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace litews
{
    public enum RequestType
    {
        GET,
        POST,
    }
    public class HttpRequestAnalysis
    {
        string TestStr = @"GET /u014683488/article/details/52253514 HTTP/1.1
Host: blog.csdn.net
Connection: keep-alive
Cache-Control: max-age=0
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.63 Safari/537.36 Edg/78.0.276.19
Sec-Fetch-User: ?1
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3
Sec-Fetch-Site: same-origin
Sec-Fetch-Mode: navigate
Referer: https://xxx.xxx.xxx/xxx/xxx/xxx/xxx
Accept-Encoding: gzip, deflate, br
Accept-Language: zh-CN,zh;q=0.9
Cookie: Hm_lpvt_6bcd52f51e9b3dce32bec4a3997715ac=1571388276; c-login-auto=29";
        public RequestModel GetRequestHeaders(string HeaderStr)
        {
            bool maybePostData = false;
            string ProcessStr = HeaderStr;
            RequestModel requestModel = new RequestModel();
            ProcessStr = ProcessStr.Replace("\n", "");
            ProcessStr = ProcessStr.Replace("\0", "");
            var ItemArray = ProcessStr.Split('\r');
            for (int i = 0; i < ItemArray.Length; i++)
            {
                if (ItemArray[i].ToString().Length < 1)
                {
                    if (requestModel.Type== RequestType.POST && requestModel.ContentLength?.Length>0)
                        maybePostData = true;
                    continue; 
                }
                if (ItemArray[i].Replace(" ", "").ToLower().IndexOf("get") == 0)
                {
                    requestModel.Type = RequestType.GET;
                    var tmpA = ItemArray[i].Split(' ');
                    for (int j = 0; j < tmpA.Length; j++)
                    {
                        if (tmpA[j].ToLower() == "get")
                        {
                            if (tmpA.Length > j + 1)
                                requestModel.Path = tmpA[j + 1];
                            if (tmpA.Length > j + 2)
                                requestModel.HttpVersion = tmpA[j + 2];
                            break;
                        }
                    }
                    continue;
                }
                else if (ItemArray[i].Replace(" ", "").ToLower().IndexOf("post") == 0)
                {
                    requestModel.Type = RequestType.POST;
                    var tmpA = ItemArray[i].Split(' ');
                    for (int j = 0; j < tmpA.Length; j++)
                    {
                        if (tmpA[j].ToLower() == "post")
                        {
                            if (tmpA.Length > j + 1)
                                requestModel.Path = tmpA[j + 1];
                            if (tmpA.Length > j + 2)
                                requestModel.HttpVersion = tmpA[j + 2];
                            break;
                        }
                    }
                    continue;
                }
                else
                {
                    FillModel(requestModel, ItemArray[i],maybePostData);
                }
            }
            return requestModel;
        }
        int otherIndex = 0;
        private void FillModel(RequestModel requestModel, string str,bool isPostData)
        {
            bool isAdd = false;
            
            Type type = requestModel.GetType();
            var properties = type.GetProperties();
            var tmp = str.Replace(" ", "");
            tmp = tmp.Replace("-", "").ToLower();

            string values = "";
            string head = "";
            try
            {
                values = str.Substring(str.IndexOf(":") + 1).Replace(" ", "");
                head = str.Substring(0, str.IndexOf(":")).Replace(" ", "");
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Name.ToLower() == nameof(requestModel.Other).ToLower())
                    {
                        otherIndex = i;
                    }
                    if (tmp.IndexOf(properties[i].Name.ToLower()) == 0 && properties[i].Name.Length == head.Replace("-", "").Length)//匹配成功
                    {

                        var tmpType = properties[i].PropertyType;
                        if (tmpType == typeof(String))
                        { isAdd = true; properties[i].SetValue(requestModel, values); }
                        else if (tmpType == typeof(List<string>))
                        { isAdd = true; properties[i].SetValue(requestModel, values.Split(',').ToList()); }
                    }
                }
            }
            catch { }
            if (!isAdd)
            {
                if (isPostData)
                {
                    head = "postdata";
                    values = str;
                }
                var lst = properties[otherIndex].GetValue(requestModel) as List<OtherRequestModel>;
                if (lst == null)
                    lst = new List<OtherRequestModel>();
                lst.Add(new OtherRequestModel() { Head = head, Body = values });
                properties[otherIndex].SetValue(requestModel, lst);
            }
        }
    }
}
