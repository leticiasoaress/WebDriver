using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Threading;
using SupportUI = OpenQA.Selenium.Support.UI;

namespace Migracao
{
    public class SolicitarMigracao
    {
        private readonly IWebDriver navegador;
        private FiliadosParaMigrar filiadosParaMigrar;
        private SupportUI.WebDriverWait wait;
        private Base _base;

        public SolicitarMigracao(IWebDriver _navegador)
        {
            navegador = _navegador;
            filiadosParaMigrar = new FiliadosParaMigrar();
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(60));
            _base = new Base(_navegador);
        }

        public string SolicitarMigracaoFranquia(string documento)
        {
            AcessarTelaSolicitarMigracao();
            PesquisarMigracao(documento);
            var retorno = RealizarPedidoMigracao();
            var log = $"\nDocumento: {documento} \nSolicitar migração \nRetorno: {retorno}";
            _base.GravarLog(log);
            _base.AcessarPaginaPrincipal();
            return retorno;
        }

        private void AcessarTelaSolicitarMigracao()
        {
            const string Url = "//*[@id=\"1\"]/ul/li/a[@href=\"paginas/filiado/SolicitaMigracao.aspx\"]";

            navegador.FindElement(By.XPath("//*[@id=\"1\"]/h3")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(Url)));
            navegador.FindElement(By.XPath(Url)).Click();
        }

        private void PesquisarMigracao(string documento)
        {
            navegador.FindElement(By.Id("ContentPlaceHolder1_txbCpf")).SendKeys(documento);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_btnPesquisar")));
            navegador.FindElement(By.Id("ContentPlaceHolder1_btnPesquisar")).Click();
        }

        private string RealizarPedidoMigracao()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("ContentPlaceHolder1_gvInfoFiliados_cbSeleciona_0")));
            navegador.FindElement(By.Id("ContentPlaceHolder1_gvInfoFiliados_cbSeleciona_0")).Click();
            navegador.FindElement(By.Id("ContentPlaceHolder1_gvInfoFiliados_btnAdd")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("MyMessageBox1_MessageBoxInterface")));
            return navegador.FindElement(By.Id("MyMessageBox1_MessageBoxInterface")).Text;
        }
    }
}