using HtmlAgilityPack;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Reflection.Metadata;
using System.Xml.Linq;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace Scrap;

static class Program
{
    private const string _url = @"https://www.indlaegssedler.dk/Search/Search/SearchAlpha/";
    private static string[] Alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

    public static async Task DownloadTestBrowser()
    {
        await new BrowserFetcher().DownloadAsync();
    }

    public static async Task<IPage> NavigateToUrl(string url, string letter, bool headless = true)
    {
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = headless});

        var page = await browser.NewPageAsync();

        await page.GoToAsync(url + letter);

        return page; 
    }
     
    public static async Task<IEnumerable<string>> GetTabelContent(this IPage page)
    {
        List<string> result = new List<string>();
        var h1title = await page.EvaluateFunctionAsync<string>("() => document.querySelector('h1').textContent.trim()");
        var h2title = await page.EvaluateFunctionAsync<string>("() => document.querySelector('h2').textContent.trim()");
        var company = await page.EvaluateFunctionAsync<string>("() => document.querySelector('div.SpaceBtm.col333 h3').textContent.trim()");
        

        result.Add(h1title);
        result.Add(h2title);
        result.Add(company);
        

        var sectionHeaders = await page.EvaluateFunctionAsync<string[]>("() => Array.from(document.querySelectorAll('div.glob-content-header-openclose-wrapper')).map(div => div.textContent.trim())");
        string jsonarray = JsonConvert.SerializeObject(sectionHeaders);


        result.Add(jsonarray);
        return result;
    }
    public static async Task<IEnumerable<string>> GetAllLinks(this IPage page)
    {
        List<string> result = new List<string>();
        var searchLinks = await page.XPathAsync("//a[contains(@class, 'glob-search_link')]");
        foreach (var Link in searchLinks)
        {
            var link = await Link.EvaluateFunctionAsync<string>("el => el.getAttribute('href')");
            result.Add(link.Trim());
        }

        return result;
    }
    
    private static IEnumerable<string[]> CreateArray(IEnumerable<string> navne, IEnumerable<string> FormOgStyrke, IEnumerable<string> virksomhed, IEnumerable<string> indlaegseddel)
    {
        var combinedData = navne.Zip(FormOgStyrke, (a1, a2) => new { a1, a2 })
                                .Zip(virksomhed, (pair, a3) => new { pair.a1, pair.a2, a3 })
                                .Zip(indlaegseddel, (pair, a4) => new { pair.a1, pair.a2, pair.a3,a4 })
                                .Select(pair => new[] { pair.a1, pair.a2, pair.a3, pair.a4});

        return combinedData;
    }

    public static void InsertDataIntoSQLDatabase(IEnumerable<string> data)
    {
        using (var connection = new SqlConnection("Server=tcp:indlaegsseddel.database.windows.net,1433;Initial Catalog=IndlaegsseddelDB;User ID=indlaegsseddel_admin;Password=ThisShouldBeGoodEnough123#;"))
        {
            connection.Open();

                var command = new SqlCommand("INSERT INTO Indlaegsseddel (Navn, FormOgStyrke, Virksomhed, Indlaegsseddel) VALUES (@Navn, @FormOgStyrke, @Virksomhed, @Indlaegsseddel)", connection);
                command.Parameters.AddWithValue("@Navn", data.ToArray()[0]);
                command.Parameters.AddWithValue("@FormOgStyrke", data.ToArray()[1]);
                command.Parameters.AddWithValue("@Virksomhed", data.ToArray()[2]);
                command.Parameters.AddWithValue("@Indlaegsseddel", data.ToArray()[3]);
                command.ExecuteNonQuery();
            
        }
    }
    static async Task Main(string[] args)
    {
        await DownloadTestBrowser();
        foreach (var letter in Alphabet)
        {
            using (var page = await NavigateToUrl(_url, letter, headless: false))
            {
                var links = await page.GetAllLinks();

                var d = links.Where((item, index) => index % 20 == 0).ToList();

                if (d.Count() > 1)
                {
                    foreach (var link in d)
                    {
                        var absoluteUrl = new Uri(new Uri("https://www.indlaegssedler.dk"), link).ToString();
                        await page.GoToAsync(absoluteUrl);
                        var records = await page.GetTabelContent();

                        InsertDataIntoSQLDatabase(records);
                    }
                }
                else if (links.Count() >0)
                {
                    var link = links.First();
                    var absoluteUrl = new Uri(new Uri("https://www.indlaegssedler.dk"), link).ToString();
                    await page.GoToAsync(absoluteUrl);
                    var records = await page.GetTabelContent();

                    InsertDataIntoSQLDatabase(records);

                    link = links.Last();
                    absoluteUrl = new Uri(new Uri("https://www.indlaegssedler.dk"), link).ToString();
                    await page.GoToAsync(absoluteUrl);
                    records = await page.GetTabelContent();

                    InsertDataIntoSQLDatabase(records);

                }
            }
        }
        return; 
    }
}
