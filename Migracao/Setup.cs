using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Migracao
{
    public class Setup
    {
        private IWebDriver driver;

        public IWebDriver ConfigurarNavegador()
        {
            driver = new ChromeDriver();
            driver.Url = "http://treinamento.sistematodos.com.br:82/CTN/Login.aspx";
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return driver;
        }

        public void RealizarLogin()
        {
            const string Login = "";
            const string Senha = "";

            driver.FindElement(By.Id("txbLogin")).SendKeys(Login);
            driver.FindElement(By.Id("txbSenha")).SendKeys(Senha + Keys.Enter);
        }

        public void SelecionarRegional()
        {
            driver.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia")).Click();
            {
                var dropdown = driver.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia"));
                dropdown.FindElement(By.XPath("//*[@id=\"ContentPlaceHolder1_ddlSubFranquia\"]/option[2]")).Click();
            }
        }

        public void SelecionarFranquia()
        {
            driver.FindElement(By.Id("ContentPlaceHolder1_ddlFranquia")).Click();
            {
                var dropdown = driver.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia"));
                dropdown.FindElement(By.XPath("//*[@id=\"ContentPlaceHolder1_ddlFranquia\"]/option[33]")).Click();
            }
            driver.FindElement(By.Id("ContentPlaceHolder1_btnConfirmar")).Click();
        }

        public void FinalizarProcesso()
        {
            Console.WriteLine("Finalizando aplicação! \n Pressiona qualquer tecla para continuar....");
            Console.ReadKey();
            driver.Quit();
        }
    }
}