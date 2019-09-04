using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAndVSTO
{
    public class YDDicHelper
    {
        private static YDDicHelper yDDicHelper;
        private static readonly object oc = new object();
        
        public YDDicHelper() { }
        public static YDDicHelper Create()
        {
            if (yDDicHelper == null)
            {
                lock (oc)
                {
                    if (yDDicHelper == null)
                        yDDicHelper = new YDDicHelper();
                }
            }
            return yDDicHelper;
        }
        public string TransText(string word)
        {
            string wKey = string.Empty;
            string wValue = string.Empty;
            string bIn = string.Empty;
            string star = string.Empty;
           
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            string apiurl = @"http://dict.youdao.com/jsonapi?jsonversion=2&q="+word;
            System.Net.Http.HttpResponseMessage response = httpClient.GetAsync(apiurl).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var wtext = Newtonsoft.Json.Linq.JObject.Parse(response.Content.ReadAsStringAsync().Result);
                var vDict = wtext["meta"]["dicts"];

                foreach (var item in vDict)
                {
                    if (item.ToString() == "ec")
                    {
                        foreach (var v in wtext["ec"])
                        {
                            bIn = v.ToString().Split(':')[1].Replace("\"","").Replace("\r","").Replace("\n", "");
                            break;
                        }
                    }
                    #region
                    //if (item.ToString() == "collins")
                    //{
                    //    foreach (var v in wtext["collins"])
                    //    {
                    //        if (v.HasValues)
                    //            foreach (var x in v)
                    //            {
                    //                if (x.HasValues) foreach (var x2 in x.Children())
                    //                    {
                    //                        if (x2.HasValues)
                    //                        {
                    //                            foreach (var x3 in x2.Children())
                    //                            {
                    //                                if (x3.HasValues)
                    //                                {
                    //                                    foreach (var x4 in x3)
                    //                                    {
                    //                                        if (x4.HasValues)
                    //                                            foreach (var x5 in x4)
                    //                                            {
                    //                                                if (x5.HasValues)
                    //                                                    foreach (var x6 in x5)
                    //                                                    {
                    //                                                        string sp = "\"star\":(?<wv>[^\r]+)";
                    //                                                        var ms = System.Text.RegularExpressions.Regex.Matches(x6.ToString(), sp, System.Text.RegularExpressions.RegexOptions.Multiline);
                    //                                                        foreach (System.Text.RegularExpressions.Match g in ms)
                    //                                                        {
                    //                                                            wValue += g.Groups["wv"].Value.Replace("\"", "");
                    //                                                        }
                    //                                                    }
                    //                                            }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //            }
                    //    }
                    //}
                    #endregion
                    if (item.ToString() == "web_trans")
                    {
                        foreach (var v in wtext["web_trans"])
                        {
                            if (v.HasValues)
                            {
                                string spk = "\"key\":(?<wk>[^\r]+)";
                                var msk = System.Text.RegularExpressions.Regex.Matches(v.ToString(), spk);
                                string sp = "\"value\":(?<wv>[^\r]+)";
                                var ms = System.Text.RegularExpressions.Regex.Matches(v.ToString(), sp);
                                if(null != msk &&  msk.Count>0)
                                for (int i = 0; i < msk.Count; i++)
                                {
                                    wValue += msk[i].Groups["wk"].Value.Replace("\"", "").Replace(","," :") + ms[i].Groups["wv"].Value.Replace("\"", "");
                                }
                                                        
                                break;
                            }
                        }
                    }
                }
            }
            return wValue + "|"+ bIn;
        }
    }
}
