using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Migracao
{
    public class AutorizarMigracao
    {
        private readonly IWebDriver navegador;
        private FiliadosParaMigrar filiadosParaMigrar;

        public AutorizarMigracao(IWebDriver _navegador)
        {
            navegador = _navegador;
            filiadosParaMigrar = new FiliadosParaMigrar();
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.WriteLine("\n\n Iniciando autorização de migração.\n\n");
            Console.WriteLine("Log das autorizações");
            AcessarTelaAutorizarMigracao();
            //foreach (var filiado in filiadosParaMigrar.listaFiliado)
            //{
            //    AcessarTelaAutorizarMigracao();
            //    //PesquisarMigracao(filiado.documento);
            //    //var retorno = RealizarPedidoMigracao();
            //    //GravarLog(filiado.documento, retorno);
            //}
        }

        private void AcessarTelaAutorizarMigracao()
        {
            navegador.FindElement(By.XPath("//*[@id=\"1\"]/h3")).Click();
            navegador.FindElement(By.XPath("//*[@id=\"1\"]/ul/li/a[@href=\"paginas/filiado/Migracao.aspx\"]")).Click();
            navegador.FindElement(By.Id("ContentPlaceHolder1_ddlTipoAutorizacao")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_ddlTipoAutorizacao"));
                dropdown.FindElement(By.XPath("//select/option[@value=1]")).Click();
            }

            navegador.FindElement(By.Id("ddlMeses")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ddlMeses"));
                dropdown.FindElement(By.XPath("//select/option[@value=5]")).Click();
            }

            navegador.FindElement(By.Id("ddlAno")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ddlAno"));
                dropdown.FindElement(By.XPath("//select/option[@value=2018]")).Click();
            }

            navegador.FindElement(By.Id("ContentPlaceHolder1_btnPesquisar")).Click();
            Paginacao();
        }

        private void Paginacao()
        {
            IReadOnlyCollection<IWebElement> elements = (IReadOnlyCollection<IWebElement>)navegador.FindElement(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]"));

            if (elements.Count > 0)
            {
                Console.WriteLine("Tem paginação");
            }
            else
            {
                Console.WriteLine("Não tem paginação");
            }
        }
    }
}
