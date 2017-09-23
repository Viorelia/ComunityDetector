using OpenQA.Selenium.PhantomJS;

namespace ComunityDetectorServices
{
    public class WebCrawler
    {
        public void SearchWeb()
        { 
            var driver = new PhantomJSDriver();
            driver.Url = "http://ieeexplore.ieee.org/browse/conferences/title/";
            driver.Navigate();
            //the driver can now provide you with what you need (it will execute the script)
            //get the source of the page
            var source = driver.PageSource;
            //fully navigate the dom
            //var pathElement = driver.FindElementById("some-id");

            System.IO.File.WriteAllText(@"D:\Work\Dizertatie\Aplicatia\results.txt", source);
        }
    }
}
