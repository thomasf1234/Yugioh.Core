using OpenQA.Selenium;
using System;
using System.Text.RegularExpressions;

namespace Yugioh.Sync.Konami.Pages
{
    public class ProductPage
    {
        public int SetSize { get; set; }
        public ProductPage(IWebDriver driver)
        {
            string setSizeText = driver.FindElement(By.XPath("//div[@class='page_num_title']")).Text;
            SetSize = Convert.ToInt32(Regex.Match(setSizeText, @"\d+").Value);
        }
    }
}
