using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Threading;
using SupportUI = OpenQA.Selenium.Support.UI;

namespace Migracao
{
    public class Base
    {
        private readonly IWebDriver navegador;
        private SupportUI.WebDriverWait wait;

        public Base(IWebDriver _navegador)
        {
            navegador = _navegador;
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(10));
        }

        public string[] VerificarPaginacao(string idTabelaMigracao)
        {
            try
            {
                var xPath = "//*[@id="+idTabelaMigracao+"]/tbody/tr[@class=\"paginacao-dinamico\"]";
                var paginacao = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
                return paginacao.Text.Split(" ");
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public (IWebElement, string) LocalizarDocumentoTela(string documento, string idTabelaMigracao)
        {
            IWebElement filiado = null;
            string classe = "";

            (filiado, classe) = LocalizarDocumentoPorClass(documento, "\"RowStyle-Dinamico\"", idTabelaMigracao);
            if (filiado == null)
            {
                (filiado, classe) = LocalizarDocumentoPorClass(documento, "\"AlternatingRowStyle-Dinamico\"", idTabelaMigracao);
            }
            return (filiado, classe);
        }

        public (IWebElement, string) LocalizarDocumentoPorClass(string documento, string nomeClass, string idTabelaMigracao)
        {
            try
            {
                var xPath = "//*[@id=" + idTabelaMigracao + "]/tbody/tr[@class=" + nomeClass + "]";
                wait.Until(ExpectedConditions.ElementExists(By.XPath(xPath)));
                return (navegador.FindElements(By.XPath(xPath)).Where(m => m.Text.Contains(documento)).FirstOrDefault(), nomeClass);
            }
            catch
            {
                return (null, null);
            }
        }

        public void GravarLog(string retorno)
        {
            Console.WriteLine(retorno);
        }

        public void AcessarPaginaPrincipal()
        {
            const string Url = "//*[@id=\"0\"]/ul/li/a[@href=\"../../PrincipalMensagens.aspx\"]";

            navegador.FindElement(By.XPath("//*[@id=\"0\"]/h3")).Click();
            var paginaPrincipal = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(Url)));
            paginaPrincipal.Click();
        }
    }
}