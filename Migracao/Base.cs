using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public bool VerificarPaginacao(string idTabelaMigracao)
        {
            try
            {
                var xPath = "//*[@id="+idTabelaMigracao+"]/tbody/tr[@class=\"paginacao-dinamico\"]";
                navegador.FindElement(By.XPath(xPath));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public (IWebElement, string) PercorrerPaginacao(string documento, int quantidadePagina, string idTabelaMigracao)
        {
            IWebElement filiado = null;
            string classe = "";
            string XPath = "//*[@id=" + idTabelaMigracao + "]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr/td/a";

            for (int j = 1; j <= quantidadePagina; j++)
            {
                (filiado, classe) = LocalizarDocumentoTela(documento, idTabelaMigracao);
 
                if (filiado != null)
                {
                    return (filiado, classe);
                }
                else if (quantidadePagina >= 1)
                {
                    var proximaPagina = j + 1;
                    var mudarPagina = navegador.FindElements(By.XPath(XPath)).Where(m => m.Text == proximaPagina.ToString()).FirstOrDefault();
                    mudarPagina.Click();
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

        public void GravarLog(string documento, string retorno, string processo)
        {
            Console.WriteLine(processo);
            Console.WriteLine($@"Documento: {documento} --> Retorno: {retorno}");
        }
    }
}