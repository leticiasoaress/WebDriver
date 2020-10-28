using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Migracao
{
    public class MigrarFiliado
    {
        private readonly IWebDriver navegador;
        private FiliadosParaMigrar filiadosParaMigrar;

        public MigrarFiliado(IWebDriver _navegador)
        {
            navegador = _navegador;
            filiadosParaMigrar = new FiliadosParaMigrar();
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.WriteLine("\n\n Iniciando a migração dos filiados.\n\n");
            Console.WriteLine("Log das migrações");
            AcessarTelaFinalizarMigracao();
            //foreach (var filiado in filiadosParaMigrar.listaFiliado)
            //{
            //    AcessarTelaAutorizarMigracao();
            //    //PesquisarMigracao(filiado.documento);
            //    //var retorno = RealizarPedidoMigracao();
            //    //GravarLog(filiado.documento, retorno);
            //}
        }

        private void AcessarTelaFinalizarMigracao()
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
                dropdown.FindElement(By.XPath("//select/option[@value=6]")).Click();
            }

            navegador.FindElement(By.Id("ddlAno")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ddlAno"));
                dropdown.FindElement(By.XPath("//select/option[@value=2018]")).Click();
            }

            navegador.FindElement(By.Id("ContentPlaceHolder1_btnPesquisar")).Click();

            var classRowStyleDinamico = navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"RowStyle-Dinamico\"]"));
            var classAlternatingRowStyleDinamico = navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"AlternatingRowStyle-Dinamico\"]"));
            var filiadosFranquia = filiadosParaMigrar.listaFiliado.Where(m => m.idFranquiaOrigem == 46).ToList();

            if (VerificarPaginacao())
            {
                var paginacao = navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr"));

                for (int i = 0; i < filiadosFranquia.Count(); i++)
                {
                    var filiado = PercorrerPaginacao(filiadosFranquia[i].documento, classRowStyleDinamico, classAlternatingRowStyleDinamico, paginacao);
                    if (filiado != null)
                    {

                    }
                }
            }
        }

        private IWebElement PercorrerPaginacao(string documento, ReadOnlyCollection<IWebElement> classRow, ReadOnlyCollection<IWebElement> classAlternating, ReadOnlyCollection<IWebElement> paginacao)
        {
            IWebElement elemento = null;

            for (int j = 1; j <= paginacao.Count; j++)
            {
                elemento = LocalizarDocumentoNaTela(documento, classRow, "RowStyle-Dinamico");
                if (elemento == null)
                {
                    elemento = LocalizarDocumentoNaTela(documento, classAlternating, "AlternatingRowStyle-Dinamico");
                }

                if (elemento != null)
                {
                    return elemento;
                }
                else
                {
                    var proximaPagina = j + 1;
                    var mudarPagina = navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr/td/a")).Where(m => m.Text == proximaPagina.ToString()).FirstOrDefault();
                    mudarPagina.Click();
                }
            }
            return null;
        }

        private IWebElement LocalizarDocumentoNaTela(string documento, ReadOnlyCollection<IWebElement> classRow, string nomeClass)
        {
            try
            {
                return navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=" + nomeClass + "]")).Where(m => m.Text.Contains(documento)).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        private bool VerificarPaginacao()
        {
            try
            {
                navegador.FindElement(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]"));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}