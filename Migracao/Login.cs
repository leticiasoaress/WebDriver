using OpenQA.Selenium;
using System;
using SupportUI = OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Migracao
{
    public class Login
    {
        private IWebDriver navegador;

        public string login { get; private set; }

        public string senha { get; private set; }

        private const int IdRegional = 1000;

        private const int IdFranquiaDestino = 46;

        private SupportUI.WebDriverWait wait;

        public Login(IWebDriver _navegador)
        {
            navegador = _navegador;
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(40));
        }

        private void SetLogin(string _login)
        {
            login = _login;
        }

        private void SetSenha(string _senha)
        {
            senha = _senha;
        }

        public void ConfigurarLogin(string _login, string _senha)
        {
            SetLogin(_login);
            SetSenha(_senha);

            navegador.FindElement(By.Id("txbLogin")).SendKeys(_login);
            navegador.FindElement(By.Id("txbSenha")).SendKeys(_senha + Keys.Enter);
        }

        public void SelecionarRegional()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_ddlSubFranquia")));

            navegador.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia"));
                dropdown.FindElement(By.XPath("//select/option[@value=" + IdRegional + "]")).Click();
            }
        }

        public void SelecionarFranquia()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_ddlFranquia")));

            navegador.FindElement(By.Id("ContentPlaceHolder1_ddlFranquia")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_ddlFranquia"));              
                dropdown.FindElement(By.XPath("//select/option[@value="+ IdFranquiaDestino +"]")).Click();             
            }
            navegador.FindElement(By.Id("ContentPlaceHolder1_btnConfirmar")).Click();
        }
    }
}