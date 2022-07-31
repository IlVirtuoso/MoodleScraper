using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MoodleScraper
{
    public class DriverManager
    {
        

        public ChromeDriver? LastBuildedDriver { get; private set; }
        public DriverManager()
        {

        }

        public IWebDriver CreateDriver(bool headless = false)
        {
            
            ChromeOptions options = new ChromeOptions() {  AcceptInsecureCertificates = true };
            if (headless)
            {
                options.AddArgument("--headless");
            }

            LastBuildedDriver = new ChromeDriver(options);
           
            return LastBuildedDriver;
        }

   


    }
}
