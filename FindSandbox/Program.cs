using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using EPiServer.Find;
using EPiServer.Find.Api;
using EPiServer.Find.ClientConventions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FindSandbox
{
    class Program
    {


        static void Main(string[] args)
        {
            #region Index data in Find
            //IndexData(); 
            #endregion

            #region 1. Search and filter by integer example
            //var books = GetBooksByYear(2009);
            //Console.WriteLine(books.Count());
            #endregion

            #region 2. Search value from string array example
            var books = SearchBooksByAuthor("Barbara Kingsolver");
            Console.WriteLine(books.Count());
            #endregion

        }


        /// <summary>
        /// Search value from array
        /// </summary>
        static IEnumerable<Book> SearchBooksByAuthor(string author)
        {
            IClient client = Client.CreateFromConfig();
            var r = client.Search<Book>(new SearchRequestBody { Fields = new List<string> { "Authors", "ISBN$$string" } }, x =>
              {
                  Console.WriteLine(x.ToString());
              });


            var books = client.Search<Book>()
                .For(author)
                .InField(x => x.Authors);
                //.Filter(x => x.PublicationYear.GreaterThan(2000));
                //.InAllField()
                
            var result = books.GetResult();
            int total = result.TotalMatching;

            return result;
        }

        /// <summary>
        ///  Filter number example
        /// </summary>
        static IEnumerable<Book> GetBooksByYear(int year)
        {
            IClient client = Client.CreateFromConfig();

            var books = client.Search<Book>()
                .Filter(x => x.PublicationYear.GreaterThan(year));
            var result = books.GetResult();

            int total = result.TotalMatching;
            return result;
        }
        static void IndexData()
        {
            var filePath = Path.GetFullPath("../../books.csv");

            IClient client = Client.CreateFromConfig();

            List<Book> books = new List<Book>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
                books.AddRange(csv.GetRecords<Book>());

            #region Customize Identity naming convention
            // Specify Identifier for the record stored in find https://world.episerver.com/documentation/developer-guides/find/NET-Client-API/Indexing/#Identity
            client.Conventions.ForInstancesOf<Book>().IdIs(x => x.Id);
            #endregion

            client.Index(books);

        }
    }

    public class StringToArrayConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                return text.Split(',').Select(x => x.Trim()).ToArray();
            }

            return Array.Empty<string>();
        }
    }

    public class PublicationYearConverter : DefaultTypeConverter
    {
        /// <summary>
		/// Converts the string to an object.
		/// </summary>
		/// <param name="text">The string to convert to an object.</param>
		/// <param name="row">The <see cref="IReaderRow"/> for the current record.</param>
		/// <param name="memberMapData">The <see cref="MemberMapData"/> for the member being created.</param>
		/// <returns>The object created from the string.</returns>
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            var numberStyle = memberMapData.TypeConverterOptions.NumberStyle ?? NumberStyles.Integer;

            if (int.TryParse(text.Split('.')[0], numberStyle, memberMapData.TypeConverterOptions.CultureInfo, out var i))
            {
                return i;
            }

            return 0;
            //return base.ConvertFromString(text, row, memberMapData);

        }
    }
}
