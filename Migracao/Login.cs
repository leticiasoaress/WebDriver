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

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("txbLogin")));

            navegador.FindElement(By.Id("txbLogin")).SendKeys(_login);
            navegador.FindElement(By.Id("txbSenha")).SendKeys(_senha + Keys.Enter);
        }

        public void SelecionarRegional(int idRegional)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_ddlSubFranquia")));

            navegador.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia"));
                dropdown.FindElement(By.XPath("//select/option[@value=" + idRegional + "]")).Click();
            }
        }

        public void SelecionarFranquia(int idFranquia)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_ddlFranquia")));

            navegador.FindElement(By.Id("ContentPlaceHolder1_ddlFranquia")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_ddlFranquia"));              
                dropdown.FindElement(By.XPath("//select/option[@value="+ idFranquia + "]")).Click();             
            }
            navegador.FindElement(By.Id("ContentPlaceHolder1_btnConfirmar")).Click();
        }

        public void AlterarFranquia(int idRegional, int idFranquia)
        {
            navegador.FindElement(By.XPath("//*[@id=\"btSair\"]")).Click();
            ConfigurarLogin(login, senha);
            SelecionarRegional(idRegional);
            SelecionarFranquia(idFranquia);
        }
    }
}