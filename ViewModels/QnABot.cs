using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModels
{
    public class QnABot
    {
        //static void Main(string[] args)
        //{
        //    Console.OutputEncoding = System.Text.Encoding.UTF8;

        //    var words = "....";
        //    Console.Write("請輸入要問的問題:");
        //    //讓用戶輸入一段話
        //    words = Console.ReadLine();
        //    //翻譯成日文、英文、韓文
        //    var ret = CallQna(words);
        //    //ret[0].translations[0].text
        //    Console.Write($"結果: {ret.answers[0].answer}");
        //}

        //static dynamic CallQna(string msg)
        //{
        //    HttpClient client = new HttpClient();
        //    string endpoint = "https://vaccinationqnamaker.azurewebsites.net/qnamaker/knowledgebases/0d22a768-9839-4f40-a22b-d78b4c7a2691/generateAnswer";

        //    // Request headers.
        //    client.DefaultRequestHeaders.Add(
        //        "Authorization", "EndpointKey d4663526-2df8-45d8-860b-3fa00d05567d");

        //    var JsonString = "{\"question\":\"" + msg + "\"}";
        //    var content =
        //       new StringContent(JsonString, System.Text.Encoding.UTF8, "application/json");
        //    //呼叫
        //    var response = client.PostAsync(endpoint, content).Result;
        //    //取得回傳
        //    var JSON = response.Content.ReadAsStringAsync().Result;
        //    //轉型
        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(JSON);
        //}
    }
}
