using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.Models;

namespace MoodleScraper
{
    public class DriverManager
    {
        

        public ChromeDriver? LastBuildedDriver { get; private set; }
        public ProxyServer? LastBuildedProxyServer { get; private set; }
        public DriverManager()
        {

        }

        public IWebDriver CreateDriver(string proxyAddress = "127.0.0.1",int proxyPort= 4443,bool headless = false)
        {
            if (LastBuildedProxyServer == null) CreateProxy(proxyPort);
            Proxy proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.SslProxy = $"{proxyAddress}:{proxyPort}";
            ChromeOptions options = new ChromeOptions() { Proxy = proxy, AcceptInsecureCertificates = true };
            if (headless)
            {
                options.AddArgument("--headless");
            }


            
            LastBuildedDriver = new ChromeDriver(options);
            


            LastBuildedDriver.Manage().Network.StartMonitoring().Wait();
            LastBuildedDriver.Manage().Network.NetworkRequestSent += (e, s) => Console.WriteLine(e);
            return LastBuildedDriver;
        }

        public ProxyServer CreateProxy(int port)
        {
            ProxyServer proxy = new ProxyServer();
            proxy.EnableHttp2 = true;
            proxy.CertificateManager.CertificateEngine = Titanium.Web.Proxy.Network.CertificateEngine.DefaultWindows;
            var ok = proxy.CertificateManager.CreateRootCertificate();
            proxy.CertificateManager.TrustRootCertificate(true);
            ExplicitProxyEndPoint model = new ExplicitProxyEndPoint(System.Net.IPAddress.Any, port, true)
            {
                GenericCertificate = proxy.CertificateManager.RootCertificate
            };

            proxy.AddEndPoint(model);
            LastBuildedProxyServer = proxy;
            return LastBuildedProxyServer;
        }


    }
}
