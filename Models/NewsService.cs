using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static VaccinationReservationPlatForm.ViewModel.NewsViewModel;

namespace VaccinationReservationPlatForm.Models
{
    public class NewsService
    {
        public List<NewsData> Crawl(string url)
        {
            string html =
                HttpHelper.DownloadHtml(url);
            // 疾管署首頁：https://www.cdc.gov.tw/
            // 疾管署新聞頁面：https://www.cdc.gov.tw/Bulletin/List/MmgtpeidAR5Ooai4-fgHzQ

            if (string.IsNullOrEmpty(html))
            {
                //拋異常
            }
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            // 利用 HtmlAgilityPack 將 Xpath 轉換成正則表達式
            string xPath = "//*[@id=\"jsMainList\"]/li/a";

            HtmlNodeCollection nodeList = document.DocumentNode.SelectNodes(xPath); //找多個節點
            string tempInnerText = "";
            string tempHerfValue = "";
            string href = "";
            string[] strReplace = { "\r", "\n", " " };
            List<NewsData> newsList = new List<NewsData>();

            if (nodeList != null)
            {
                int i = 0;

                foreach (HtmlNode node in nodeList)
                {
                    if (i < 6)
                    {
                        tempInnerText = node.InnerText;
                        tempHerfValue = node.Attributes["href"].Value;
                        //tempHerfValue = node.OuterHtml;
                        //href = tempHerfValue.Substring(tempHerfValue.IndexOf("=") + 2, tempHerfValue.IndexOf(">") - tempHerfValue.IndexOf("=") - 3);
                        newsList.Add(new NewsData() { Title = tempInnerText, Url = tempHerfValue });
                        i++;
                    }
                }
            }

            return newsList;
        }

    }
}
