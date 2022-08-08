using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.DevTools;

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
            Downloader.DryRun = true; //remove
            Downloader.Instance.Start();


            new LoginStrategy();
            new AvrcpStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2354");
            //new SCPDStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2390");
            //new SIMMODStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2240");
            //new SecurityStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2292");
            //new VCPStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2416");
            //new RetiStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2308"); 
            //new ATSDStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2284");
            //new ElivaStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2360");
            //new MTNStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2250");
            //new ModelGraphicStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2206");
            //new SDRVStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2294");
            //new AAUTStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2328");
            //new BDMStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2410");
            //new BioInfStrategy("https://informatica.i-learn.unito.it/course/view.php?id=2336");


            Downloader.Instance.WaitDownloads();
            anchor.Join();
        }





      


    }
}