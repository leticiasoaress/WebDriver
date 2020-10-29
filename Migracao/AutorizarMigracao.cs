using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public AutorizarMigracao(IWebDriver _navegador)
        {
            navegador = _navegador;
            filiadosParaMigrar = new FiliadosParaMigrar();
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(60));
            _base = new Base(_navegador);
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.WriteLine("\n\n Iniciando autorização de migração.\n\n");
            Console.WriteLine("Log das autorizações");
            AcessarTelaAutorizarMigracao();
            BtnAutorizarMigracao();
        }

        private void AcessarTelaAutorizarMigracao()
        {
            navegador.FindElement(By.XPath("//*[@id=\"1\"]/h3")).Click();
            navegador.FindElement(By.XPath("//*[@id=\"1\"]/ul/li/a[@href=\"paginas/filiado/AutorizaMigracao.aspx\"]")).Click();
        }

        private void BtnAutorizarMigracao()
        {
            const string Processo = "Autorizar migração";
            const string XPathPaginacao = "//*[@id=\"ContentPlaceHolder1_gvAutorizaMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr";
            var idTabelaAutorizacao = "\"ContentPlaceHolder1_gvAutorizaMigracao\"";
            
            var filiadosFranquia = filiadosParaMigrar.listaFiliado.Where(m => m.idFranquiaOrigem == 46).ToList();
            var quantidadePagina = 1;

            if (_base.VerificarPaginacao(idTabelaAutorizacao))
            {
                var paginacao = navegador.FindElements(By.XPath(XPathPaginacao));
                quantidadePagina = paginacao.Count();
            }

            for (int i = 0; i < filiadosFranquia.Count(); i++)
            {
                var (filiado, classe) = _base.PercorrerPaginacao(filiadosFranquia[i].documento, quantidadePagina, idTabelaAutorizacao);
                if (filiado != null)
                {
                    var btnAutorizar = "//*[@id="+ idTabelaAutorizacao + "]/tbody/tr[@class=" + classe + "]/td/input[@title=\"Autorizar\"]";
                    filiado.FindElement(By.XPath(btnAutorizar)).Click();
                    
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("MyMessageBox1_MessageBoxInterface")));                    
                    var retorno = navegador.FindElement(By.Id("MyMessageBox1_MessageBoxInterface")).Text;
                    
                    _base.GravarLog(filiadosFranquia[i].documento, retorno, Processo);
                }
                _base.AcessarPaginaPrincipal();
                AcessarTelaAutorizarMigracao();
            }
        }
    }
}