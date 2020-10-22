﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migracao
{
    public class Login
    {
        private IWebDriver navegador;

        public string login { get; private set; }

        public string senha { get; private set; }

        private const int IdRegional = 1000;

        private const int IdFranquiaDestino = 46;

        public Login(IWebDriver _navegador)
        {
            navegador = _navegador;
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
            navegador.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia"));
                dropdown.FindElement(By.XPath("//select/option[@value=" + IdRegional + "]")).Click();
            }
        }

        public void SelecionarFranquia()
        {
            navegador.FindElement(By.Id("ContentPlaceHolder1_ddlFranquia")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_ddlSubFranquia"));
                dropdown.FindElement(By.XPath("//select/option[@value="+ IdFranquiaDestino +"]")).Click();             
            }
            navegador.FindElement(By.Id("ContentPlaceHolder1_btnConfirmar")).Click();
        }
    }
}