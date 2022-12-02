using GunlukHisseSenediRaporu.API.Models;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Investing_Helper.Services
{
    public interface IInvestingJobs
    {
        Task GetStockPrice();
    }
    public class InvestingJobs : IInvestingJobs
    {
        private IMailService _mailService;
        public InvestingJobs(IMailService mailService)
        {
            _mailService = mailService;
        }
        public async Task GetStockPrice()
        {
            string Google_Finans = "https://www.google.com/finance/quote/VESBE:IST?sa=X&ved=2ahUKEwjIkKHR39P7AhWfSPEDHYluBA4Q3ecFegQINRAi";
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Google_Finans);
            List<Hisse> hisses = new()
                {
                    new Hisse
                    {
                        OncekiKapanis = driver.FindElement(By.CssSelector("#yDmH0d > c-wiz > div > div.e1AOyf > div > main > div.Gfxi4 > div.HKO5Mb > div > div.eYanAe > div:nth-child(2) > div")).Text,
                        Kapanis = driver.FindElement(By.CssSelector("#yDmH0d > c-wiz > div > div.e1AOyf > div > main > div.Gfxi4 > div.yWOrNb > div.VfPpkd-WsjYwc.VfPpkd-WsjYwc-OWXEXe-INsAgc.KC1dQ.Usd1Ac.AaN0Dd.QZMA8b > c-wiz > div > div:nth-child(1) > div > div.rPF6Lc > div > div:nth-child(1) > div > span > div > div")).Text,
                        GunlukAralik = driver.FindElement(By.CssSelector("#yDmH0d > c-wiz > div > div.e1AOyf > div > main > div.Gfxi4 > div.HKO5Mb > div > div.eYanAe > div:nth-child(3) > div")).Text,
                        YillikAralik = driver.FindElement(By.CssSelector("#yDmH0d > c-wiz > div > div.e1AOyf > div > main > div.Gfxi4 > div.HKO5Mb > div > div.eYanAe > div:nth-child(4) > div")).Text,
                        FKOrani = driver.FindElement(By.CssSelector("#yDmH0d > c-wiz > div > div.e1AOyf > div > main > div.Gfxi4 > div.HKO5Mb > div > div.eYanAe > div:nth-child(7) > div")).Text,
                        PiyasaDegeriTl = driver.FindElement(By.CssSelector("#yDmH0d > c-wiz > div > div.e1AOyf > div > main > div.Gfxi4 > div.HKO5Mb > div > div.eYanAe > div:nth-child(5) > div")).Text,
                        TemettuGetirisi = driver.FindElement(By.CssSelector("#yDmH0d > c-wiz > div > div.e1AOyf > div > main > div.Gfxi4 > div.HKO5Mb > div > div.eYanAe > div:nth-child(8) > div")).Text
                    }
                };
            _mailService.SendEmail(hisses);
            driver.Quit();
        }
    }
}
