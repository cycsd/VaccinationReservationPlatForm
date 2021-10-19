using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace VaccinationReservationPlatForm.ViewModel
{
    public class NewsViewModel
    {
        public class NewsData
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
        public List<NewsData> newsList { get; set; }

    }
}
