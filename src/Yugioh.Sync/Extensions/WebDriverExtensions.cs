using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Threading;

namespace Yugioh.Sync.Extensions
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var stopWatch = new Stopwatch();

                stopWatch.Start();
                while (stopWatch.Elapsed.TotalSeconds < timeoutInSeconds)
                {
                    try
                    {
                        var element = driver.FindElement(by);

                        if (element != null)
                        {
                            return element;
                        }
                    }
                    catch (NoSuchElementException) { }

                    Thread.Sleep(500);
                }
            }

            return driver.FindElement(by);
        }

        // WebDriverWait not working
        /*public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }*/
    }
}


