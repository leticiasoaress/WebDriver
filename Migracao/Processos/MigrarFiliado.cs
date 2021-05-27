using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Threading;
using SupportUI = OpenQA.Selenium.Support.UI;

namespace Migracao
{
    public class MigrarFiliado
    {
        private Base _base;
        private SupportUI.WebDriverWait wait;

        public MigrarFiliado(IWebDriver _navegador)
        {
            _base = new Base(_navegador);
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(35));
        }

        public void MigrarFiliadoFranquia(DadosFiliado filiado, StreamWriter arquivo)
        {
            Thread.Sleep(3000);
            AcessarTelaMigrarFiliado();
            VerificarPedidosMigracao(filiado, arquivo);
            _base.AcessarPaginaPrincipal();
        }

        private void AcessarTelaMigrarFiliado()
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
                ddlMeses.FindElement(By.XPath("//select/option[@value=05]")).Click();
            }

            var ddlAno = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_dtAnoMes_ddlAno")));
            ddlAno.Click();
            {
                ddlAno.FindElement(By.XPath("//select/option[@value=2021]")).Click();
            }

            var btnPesquisar = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_btnPesquisar")));
            btnPesquisar.Click();
        }

        private void VerificarPedidosMigracao(DadosFiliado filiado, StreamWriter arquivo)
        {
            var idTabelaMigracao = "\"ContentPlaceHolder1_gvMigracao\"";

            Thread.Sleep(2000);

            var (elementFiliado, classe) = _base.LocalizarDocumentoTela(filiado.documento, idTabelaMigracao);

            if (elementFiliado != null)
            {
                BtnConfirmarMigracao(filiado, elementFiliado, idTabelaMigracao, classe, arquivo);
            }
            else
            {
                _base.GravarLog(arquivo, $"\nMigrar filiado \nErro: Falha ao tentar localizar o filiado.");
            }
        }

        private void BtnConfirmarMigracao(DadosFiliado filiado, IWebElement elementFiliado, string tabela, string classe, StreamWriter arquivo)
        {
            try
            {
                var btnMigrar = "//*[@id=" + tabela + "]/tbody/tr[@class=" + classe + "]/td/input[@title=\"Migrar Cliente\"]";
                elementFiliado.FindElement(By.XPath(btnMigrar)).Click();

                PreencherFormaPagamento(filiado);

                var migrar = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_btnMigrar")));
                migrar.Click();

                Thread.Sleep(2000);

                var messageBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("MyMessageBox1_MessageBoxInterface")));
                var retorno = messageBox.Text;
                messageBox = null;

                _base.GravarLog(arquivo, $"\nMigrar filiado \nRetorno: {retorno}");
            }
            catch (Exception ex)
            {
                _base.GravarLog(arquivo, $"\nMigrar filiado \nErro --> {ex.Message}");
            }
        }

        private void PreencherFormaPagamento(DadosFiliado filiado)
        {
            var formaPagamentoPai = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"ContentPlaceHolder1_ucFormasPagto_ddlFormaPagto\"]")));
            formaPagamentoPai.Click();
            formaPagamentoPai.FindElement(By.XPath("//select/option[@value=" + filiado.idFormaPgtoPai + "]")).Click();

            var xPathFormaPgto = VerificarXPathFormaPagameto(filiado.idFormaPgtoPai);
            if (!string.IsNullOrEmpty(xPathFormaPgto))
            {
                var formaPagamento = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPathFormaPgto)));
                formaPagamento.Click();
                formaPagamento.FindElement(By.XPath("//select/option[@value=" + filiado.idFormaPgto + "]")).Click();

                if (filiado.idFormaPgtoPai == 5301129) //Concessionaria
                {
                    var uc = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"ContentPlaceHolder1_ucFormasPagto_PagtoEnergia_txbUC\"]")));
                    uc.Click();
                    uc.FindElement(By.XPath("//*[@id=\"ContentPlaceHolder1_ucFormasPagto_PagtoEnergia_txbUC\"]")).SendKeys(filiado.uc);
                }
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