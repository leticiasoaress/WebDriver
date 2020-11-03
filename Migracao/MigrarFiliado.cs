using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading;
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
            var menuUsuario = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"1\"]/h3")));
            menuUsuario.Click();

            var migracao = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"1\"]/ul/li/a[@href=\"paginas/filiado/Migracao.aspx\"]")));
            migracao.Click();

            var tipoAutorizacao = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_ddlTipoAutorizacao")));
            tipoAutorizacao.Click();
            {
                tipoAutorizacao.FindElement(By.XPath("//select/option[@value=1]")).Click();
            }

            var ddlMeses = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_dtAnoMes_ddlMeses")));
            ddlMeses.Click();
            {
                ddlMeses.FindElement(By.XPath("//select/option[@value=11]")).Click();
            }

            var ddlAno = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_dtAnoMes_ddlAno")));
            ddlAno.Click();
            {
                ddlAno.FindElement(By.XPath("//select/option[@value=2020]")).Click();
            }

            var btnPesquisar = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_btnPesquisar")));
            btnPesquisar.Click();
        }

        private void VerificarPedidosMigracao()
        {
            var idTabelaMigracao = "\"ContentPlaceHolder1_gvMigracao\"";
            var filiadosFranquia = filiadosParaMigrar.listaFiliado.ToList();

            var paginacao = _base.VerificarPaginacao(idTabelaMigracao);

            for (int i = 0; i < filiadosFranquia.Count(); i++)
            {
                var (elementFiliado, classe) = _base.PercorrerPaginacao(filiadosFranquia[i].documento, paginacao, idTabelaMigracao);
                if (elementFiliado != null)
                {
                    BtnConfirmarMigracao(filiadosFranquia[i], elementFiliado, idTabelaMigracao, classe);
                }
                Thread.Sleep(5000);
                _base.AcessarPaginaPrincipal();
                AcessarTelaFinalizarMigracao();
            }
        }

        private void BtnConfirmarMigracao(DadosFiliado filiado, IWebElement elementFiliado, string tabela, string classe)
        {
            try 
            {
                var btnMigrar = "//*[@id=" + tabela + "]/tbody/tr[@class=" + classe + "]/td/input[@title=\"Migrar Cliente\"]";
                elementFiliado.FindElement(By.XPath(btnMigrar)).Click();

                var formaPagamentoPai = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"ContentPlaceHolder1_ucFormasPagto_ddlFormaPagto\"]")));
                formaPagamentoPai.Click();
                formaPagamentoPai.FindElement(By.XPath("//select/option[@value=" + filiado.idFormaPgtoPai + "]")).Click();

                var xPathFormaPgto = VerificarXPathFormaPagameto(filiado.idFormaPgtoPai);
                if (!string.IsNullOrEmpty(xPathFormaPgto))
                {
                    var formaPagamento = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPathFormaPgto)));
                    formaPagamento.Click();
                    formaPagamento.FindElement(By.XPath("//select/option[@value=" + filiado.idFormaPgto + "]")).Click();
                }

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
            catch 
            {
                _base.GravarLog(filiado.documento, "Migração já realizada ou Não encontrado");
            }
        }

        private string VerificarXPathFormaPagameto(int idFormaPgtoPai)
        {
            var xPath = "";
            switch (idFormaPgtoPai)
            {
                case 5301129: //CONCESIONÁRIA ENERGIA
                    xPath = "//*[@id=\"ContentPlaceHolder1_ucFormasPagto_PagtoEnergia_ddlInstituicao\"]";
                    break;
                case 5300079: //CREDIFACIL
                    xPath = "";
                    break;
                case 3200: //PAGO EN LA OFICINA
                    xPath = "//*[@id=\"ContentPlaceHolder1_ucFormasPagto_PagtoDiretoCTN_ddlTipoCTN\"]";
                    break;
                case 5300081: //PAYVALIDA
                    xPath = "//*[@id=\"ContentPlaceHolder1_ucFormasPagto_PagtoPayvalida_ddlTipoPayvalida\"]";
                    break;
                case 5301109: //TARJETA (DEBITO/CREDITO)
                    xPath = "";
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return xPath;
        }
    }
}