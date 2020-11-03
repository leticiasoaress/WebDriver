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
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(40));
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

        public (IWebElement, string) PercorrerPaginacao(string documento, string[] paginacao, string idTabelaMigracao)
        {
            IWebElement filiado = null;
            IWebElement mudarPagina = null;
            
            string classe = "";
            string XPathPaginacao = "//*[@id=" + idTabelaMigracao + "]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr/td/a";
            var quantidadePagina = 1;
            int cont = 1;

            if (paginacao.Count() > 0)
            {
                quantidadePagina = paginacao.Count();
            }

            for (; cont <= quantidadePagina; cont++)
            {
                (filiado, classe) = LocalizarDocumentoTela(documento, idTabelaMigracao);

                if (filiado == null && quantidadePagina > 1)
                {
                    if (paginacao[cont] == "...")
                    {
                        mudarPagina = navegador.FindElements(By.XPath(XPathPaginacao)).Where(m => m.Text == "...").FirstOrDefault();
                        Thread.Sleep(2000);
                        mudarPagina.Click();
                        Thread.Sleep(2000);
                        paginacao = VerificarPaginacao(idTabelaMigracao);
                        quantidadePagina = paginacao.Count();
                        cont = 0;
                    }
                    else
                    {
                        var proximaPagina = paginacao[cont];
                        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(XPathPaginacao)));
                        mudarPagina = navegador.FindElements(By.XPath(XPathPaginacao)).Where(m => m.Text == proximaPagina.ToString()).FirstOrDefault();
                        mudarPagina.Click();
                        
                        (filiado, classe) = LocalizarDocumentoTela(documento, idTabelaMigracao); 
                    }
                }

                if (filiado != null)
                {
                    return (filiado, classe);
                }
            }    
            return (null, classe);
        }

        private (IWebElement, string) LocalizarDocumentoTela(string documento, string idTabelaMigracao)
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
                return (navegador.FindElements(By.XPath(xPath)).Where(m => m.Text.Contains(documento)).FirstOrDefault(), nomeClass);
            }
            catch
            {
                return (null, null);
            }
        }

        public void GravarLog(string documento, string retorno)
        {
            Console.WriteLine($@"Documento: {documento} --> Retorno: {retorno}" + "\n\n");
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