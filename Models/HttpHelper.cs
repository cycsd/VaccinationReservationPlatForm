using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.Models
{
    public class HttpHelper
    {
        public static string DownloadHtml(string url)
        {
            string html = string.Empty;
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 30 * 1000;
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36";
                Encoding encode = Encoding.UTF8;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        //logger.Warn(string.Format("抓取{0}網址返回失敗，response.StatusCode為{1}", url, response.StatusCode));
                    }
                    else
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(response.GetResponseStream(), encode);
                            html = sr.ReadToEnd(); //讀取資料
                            sr.Close();
                        }
                        catch (Exception ex)
                        {
                            html = null;
                        }
                    }
                }

            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message.Equals("遠端伺服器回傳錯誤：(306)。"))
                {
                    //logger.Error("遠端伺服器回傳錯誤：(306)。");
                    html = "遠端伺服器回傳錯誤：(306)。";
                }
            }
            catch (Exception ex)
            {
                html = null;
            }
            return html;
        }

    }
}
