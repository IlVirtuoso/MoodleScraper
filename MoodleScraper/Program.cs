using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.DevTools;
using Titanium;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.EventArguments;
using System.Security.Cryptography.X509Certificates;
using MoodleScraper.Strategies;
using System.Diagnostics;
using Xabe.FFmpeg;
namespace MoodleScraper
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Directory.SetCurrentDirectory(@"F:\MoodleSnapshot\");
            Console.WriteLine("Hello, World!");
            var anchor = StrategyBooker.Instance.Start();
            Downloader.Instance.Start();
            new LoginStrategy().Wait();
            new AvrcpStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2354");
            Downloader.Instance.WaitDownloads();
            anchor.Join();
        }





      


    }
}