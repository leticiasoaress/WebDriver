using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
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

        public StreamWriter GerarTxt()
        {
            string nomeArquivo = $"migracaoColombia-{DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", ".")}";
            string CaminhoArquivo = $"C:\\Users\\12183934670\\Desktop\\MigracaoColombia\\{nomeArquivo}.txt";

            return File.CreateText(CaminhoArquivo);
        }
        
        public void GravarLog(StreamWriter arquivo, string mensagem)
        {
            // Escreve no arquivo de texto
            arquivo.WriteLine(mensagem);

            // Escreve no console
            Console.WriteLine(mensagem);
        }

        public void FecharTxt(StreamWriter arquivo)
        {
            arquivo.Close();
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