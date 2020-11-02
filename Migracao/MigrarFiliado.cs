using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using SupportUI = OpenQA.Selenium.Support.UI;

namespace Migracao
{
    public class MigrarFiliado
    {
        private readonly IWebDriver navegador;
        private FiliadosParaMigrar filiadosParaMigrar;
        private Base _base;
        private SupportUI.WebDriverWait wait;

        public MigrarFiliado(IWebDriver _navegador)
        {
            navegador = _navegador;
            _base = new Base(_navegador);
            filiadosParaMigrar = new FiliadosParaMigrar();
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(60));
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.WriteLine("\n\n Iniciando a migração dos filiados.\n\n");
            Console.WriteLine("Log das migrações");
            AcessarTelaFinalizarMigracao();
            VerificarPedidosMigracao();
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

            navegador.FindElement(By.Id("ContentPlaceHolder1_dtAnoMes_ddlMeses")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_dtAnoMes_ddlMeses"));
                dropdown.FindElement(By.XPath("//select/option[@value=10]")).Click();
            }

            navegador.FindElement(By.Id("ContentPlaceHolder1_dtAnoMes_ddlAno")).Click();
            {
                var dropdown = navegador.FindElement(By.Id("ContentPlaceHolder1_dtAnoMes_ddlAno"));
                dropdown.FindElement(By.XPath("//select/option[@value=2020]")).Click();
            }

            navegador.FindElement(By.Id("ContentPlaceHolder1_btnPesquisar")).Click();
        }

        private void VerificarPedidosMigracao()
        {
            const string XPathPaginacao = "//*[@id=\"ContentPlaceHolder1_gvMigracao\"]/tbody/tr[@class=\"paginacao-dinamico\"]/td/table/tbody/tr";
            var idTabelaMigracao = "\"ContentPlaceHolder1_gvMigracao\"";
            var quantidadePagina = 1;
            var filiadosFranquia = filiadosParaMigrar.listaFiliado.ToList();

            if (_base.VerificarPaginacao(idTabelaMigracao))
            {
                var paginacao = navegador.FindElements(By.XPath(XPathPaginacao));
                quantidadePagina = paginacao.Count();
            }

            for (int i = 0; i < filiadosFranquia.Count(); i++)
            {
                var (elementFiliado, classe) = _base.PercorrerPaginacao(filiadosFranquia[i].documento, quantidadePagina, idTabelaMigracao);
                if (elementFiliado != null)
                {
                    BtnConfirmarMigracao(filiadosFranquia[i], elementFiliado, idTabelaMigracao, classe);
                }
                _base.AcessarPaginaPrincipal();
                AcessarTelaFinalizarMigracao();
            }
        }

        private void BtnConfirmarMigracao(DadosFiliado filiado, IWebElement elementFiliado, string tabela, string classe)
        {
            var btnMigrar = "//*[@id=" + tabela + "]/tbody/tr[@class=" + classe + "]/td/input[@title=\"Migrar Cliente\"]";
            elementFiliado.FindElement(By.XPath(btnMigrar)).Click();


            var formaPagamentoPai = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"ContentPlaceHolder1_ucFormasPagto_ddlFormaPagto\"]")));
            formaPagamentoPai.Click();
            formaPagamentoPai.FindElement(By.XPath("//select/option[@value=" + filiado.idFormaPgtoPai + "]")).Click();

            var formaPagamento = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"ContentPlaceHolder1_ucFormasPagto_PagtoEnergia_ddlInstituicao\"]")));
            formaPagamento.Click();
            formaPagamento.FindElement(By.XPath("//select/option[@value=" + filiado.idFormaPgto + "]")).Click();

            if (filiado.idFormaPgtoPai == 5301129)
            {
                var uc = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"ContentPlaceHolder1_ucFormasPagto_PagtoEnergia_txbUC\"]")));
                uc.Click();
                uc.FindElement(By.XPath("//*[@id=\"ContentPlaceHolder1_ucFormasPagto_PagtoEnergia_txbUC\"]")).SendKeys(filiado.uc.ToString());
            }

            var migrar = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_btnMigrar")));
            migrar.Click();

            var messageBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("MyMessageBox1_MessageBoxInterface")));
            var retorno = messageBox.Text;

            _base.GravarLog(filiado.documento, retorno);
        }
    }
}