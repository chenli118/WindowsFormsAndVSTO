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
            string apiurl = @"http://dict.youdao.com/jsonapi?jsonversion=2&q=" + word;
            System.Net.Http.HttpResponseMessage response = httpClient.GetAsync(apiurl).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var wtext = Newtonsoft.Json.Linq.JObject.Parse(response.Content.ReadAsStringAsync().Result);
                var vDict = wtext["meta"]["dicts"];

                foreach (var item in vDict.Children())
                {
                    if (item.ToString() == "ec" && wtext["ec"]["exam_type"] != null)
                    {
                        bIn = wtext["ec"]["exam_type"].ToString().Replace("\r\n", "").Replace("\"", "");
                        // wValue = wtext["ec"]["word"][0]["trs"].ToString().Replace("\r\n", "").Replace("\"", "");

                    }
                    if (item.ToString() == "rel_word")
                    {
                        var rels = wtext["rel_word"]["rels"];
                        foreach (var ws in rels.Children())
                        {
                            if (ws.HasValues)
                                foreach (var ws2 in ws.Children())
                                {
                                    string spk = "\"word\":(?<wk>[^\r]+)";
                                    var msk = System.Text.RegularExpressions.Regex.Matches(ws2.ToString(), spk);
                                    string spv = "\"tran\":(?<wv>[^\r]+)";
                                    var ms = System.Text.RegularExpressions.Regex.Matches(ws2.ToString(), spv);
                                    if (null != msk && msk.Count > 0)
                                        for (int i = 0; i < msk.Count; i++)
                                        {
                                            wValue += msk[i].Groups["wk"].Value.Replace("\"", "").Replace(",", " :") + ms[i].Groups["wv"].Value.Replace("\"", "");
                                        }
                                }
                        }

                    }
                    if (wtext["rel_word"] == null && item.ToString() == "syno")
                    {
                        var synos = wtext["syno"]["synos"];

                        string spv = "\"tran\":(?<wv>[^\r]+)";
                        var ms = System.Text.RegularExpressions.Regex.Matches(synos.ToString(), spv);
                        if (null != ms && ms.Count > 0)
                            for (int i = 0; i < ms.Count; i++)
                            {
                                wValue += ms[i].Groups["wv"].Value.Replace("\"", "");
                            }
                    }
                    if (item.ToString() == "collins")
                    {
                        var collins = wtext["collins"];
                        string spv = "\"star\":(?<wv>[^\r]+)";
                        var ms = System.Text.RegularExpressions.Regex.Matches(collins.ToString(), spv);
                        if (null != ms && ms.Count > 0)
                            for (int i = 0; i < ms.Count; i++)
                            {
                                star = ms[i].Groups["wv"].Value.Replace("\"", "");
                            }
                    }
                }
                if (wValue.Length == 0 && wtext["web_trans"] != null)
                {
                    var web_trans = wtext["web_trans"]["web-translation"];
                    foreach (var web in web_trans.Children())
                    {

                        string spk = "\"key\":(?<wk>[^\r]+)";
                        var msk = System.Text.RegularExpressions.Regex.Matches(web.ToString(), spk);
                        string sp = "\"value\":(?<wv>[^\r]+)";
                        var ms = System.Text.RegularExpressions.Regex.Matches(web.ToString(), sp);
                        if (null != msk && msk.Count > 0)
                            for (int i = 0; i < msk.Count; i++)
                            {
                                wValue += msk[i].Groups["wk"].Value.Replace("\"", "").Replace(",", " :") + ms[i].Groups["wv"].Value.Replace("\"", "");
                            }

                    }
                }

            }
            return wValue.TrimEnd(',') + bIn + " star :" + star;
        }
    }
}
