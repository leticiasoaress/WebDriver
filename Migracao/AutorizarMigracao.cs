using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using SupportUI = OpenQA.Selenium.Support.UI;

namespace Migracao
{
    public class AutorizarMigracao
    {
        private readonly IWebDriver navegador;
        private FiliadosParaMigrar filiadosParaMigrar;
        private SupportUI.WebDriverWait wait;
        private Base _base;
        private Login login;

        public AutorizarMigracao(IWebDriver _navegador, Login _login)
        {
            navegador = _navegador;
            filiadosParaMigrar = new FiliadosParaMigrar();
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(60));
            _base = new Base(_navegador);
            login = _login;
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.WriteLine("\n\n Iniciando autorização de migração.\n\n");
            Console.WriteLine("Log das autorizações");
            AcessarTelaAutorizarMigracao();
            ProcessarAutorizarMigracao();
        }

        private void AcessarTelaAutorizarMigracao()
        {
            navegador.FindElement(By.XPath("//*[@id=\"1\"]/h3")).Click();
            navegador.FindElement(By.XPath("//*[@id=\"1\"]/ul/li/a[@href=\"paginas/filiado/AutorizaMigracao.aspx\"]")).Click();
        }

        private void ProcessarAutorizarMigracao()
        {        
            foreach (var franquia in filiadosParaMigrar.listaFranquia)
            {              
                login.AlterarFranquia(franquia.idRegional, franquia.idFranquia);
                AcessarTelaAutorizarMigracao();
                VerificarPedidosMigracao(franquia.idFranquia);
            }
        }

        private void VerificarPedidosMigracao(int idFranquia)
        {
            const string XPathPaginacao = "//*[@id=\"ContentPlaceHolder1_gvAutorizaMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr";
            var idTabelaAutorizacao = "\"ContentPlaceHolder1_gvAutorizaMigracao\"";
            var quantidadePagina = 1;
            var filiadosFranquia = filiadosParaMigrar.listaFiliado.Where(m => m.idFranquiaOrigem == idFranquia).ToList();

            if (_base.VerificarPaginacao(idTabelaAutorizacao))
            {
                var paginacao = navegador.FindElements(By.XPath(XPathPaginacao));
                quantidadePagina = paginacao.Count();
            }

            for (int i = 0; i < filiadosFranquia.Count(); i++)
            {
                var (elementFiliado, classe) = _base.PercorrerPaginacao(filiadosFranquia[i].documento, quantidadePagina, idTabelaAutorizacao);
                if (elementFiliado != null)
                {
                    BtnAutorizarMigracao(filiadosFranquia[i].documento, elementFiliado, idTabelaAutorizacao, classe);
                }
                _base.AcessarPaginaPrincipal();
                AcessarTelaAutorizarMigracao();
            }
        }

        private void BtnAutorizarMigracao(string documento, IWebElement elementFiliado, string tabela, string classe)
        {
            var btnAutorizar = "//*[@id=" + tabela + "]/tbody/tr[@class=" + classe + "]/td/input[@title=\"Autorizar\"]";
            elementFiliado.FindElement(By.XPath(btnAutorizar)).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("MyMessageBox1_MessageBoxInterface")));
            var retorno = navegador.FindElement(By.Id("MyMessageBox1_MessageBoxInterface")).Text;
            
            _base.GravarLog(documento, retorno);
        }
    }
}