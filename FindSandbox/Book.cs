using CsvHelper.Configuration.Attributes;
using EPiServer.Find.UnifiedSearch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindSandbox
{
    public class Book
    {
        [Name("id")]
        public int Id { get; set; }

        [Name("isbn")]
        public string ISBN { get; set; }

        [Name("authors")]
        [TypeConverter(typeof(StringToArrayConverter))]
        public string[] Authors { get; set; }

        [Name("original_publication_year")]
        [TypeConverter(typeof(PublicationYearConverter))]
        public int PublicationYear { get; set; }

        [Name("title")]
        public string BookTitle { get; set; }

        [Name("language_code")]
        public string LanguageCode { get; set; }
        
        [Name("average_rating")]
        public double AvgRating { get; set; }

        [Name("image_url")]
        public string ImageUrl { get; set; }

        [Name("small_image_url")]
        public string SmallImageUrl { get; set; }
    }
}
