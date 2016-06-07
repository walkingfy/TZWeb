using System;
using System.IO;
using System.Net;
using System.Text;

namespace Tz.Core.Tools
{
    public class WebHelper
    {

        public static string GetUrlRequestContent(string url)
        {
            Uri uri = new Uri(url);
            WebRequest webRequest = WebRequest.Create(uri);
            Stream s = webRequest.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd();
            return all;
        }
    }
}