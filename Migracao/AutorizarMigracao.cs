using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
                dropdown.FindElement(By.XPath("//select/option[@value=6]")).Click();
            }

            navegador.FindElement(By.Id("ddlAno")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ddlAno"));
                dropdown.FindElement(By.XPath("//select/option[@value=2018]")).Click();
            }

            navegador.FindElement(By.Id("ContentPlaceHolder1_btnPesquisar")).Click();

            if (VerificarPaginacao())
            {
                var paginacao = navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr"));
                var classRowStyleDinamico = navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"RowStyle-Dinamico\"]"));
                var classAlternatingRowStyleDinamico = navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"AlternatingRowStyle-Dinamico\"]"));

                var filiadosFranquia = filiadosParaMigrar.listaFiliado.Where(m => m.idFranquiaOrigem == 46).ToList();
                IWebElement elemento = null;

                for (int i = 0; i < filiadosFranquia.Count(); i++)
                {
                    elemento = LocalizarDocumentoNaTela(filiadosFranquia[i].documento, classRowStyleDinamico, "RowStyle-Dinamico");
                    if (elemento == null)
                    {
                        elemento = LocalizarDocumentoNaTela(filiadosFranquia[i].documento, classAlternatingRowStyleDinamico, "AlternatingRowStyle-Dinamico");
                    }
                    
                    if(elemento != null)
                    {
                        Console.WriteLine(elemento.Text);
                    }
                    else
                    {
                        var proximaPagina = 2;
                        var mudarPagina = navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr/td/a")).Where(m=>m.Text == proximaPagina.ToString()).FirstOrDefault();
                        mudarPagina.Click();
                    }
                }
            }
        }

        private IWebElement LocalizarDocumentoNaTela(string documento, ReadOnlyCollection<IWebElement> classRow, string nomeClass)
        {
            try 
            {
                return navegador.FindElements(By.XPath("//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class="+ nomeClass +"]")).Where(m => m.Text.Contains(documento)).FirstOrDefault();
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
