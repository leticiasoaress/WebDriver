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
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(40));
            _base = new Base(_navegador);
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.Clear();
            Console.WriteLine("Iniciando pedido de migração.\n\n");
            Console.WriteLine("Log dos documentos");

            var listaFiliado = filiadosParaMigrar.listaFiliado.ToList();

            for (int i = 0; i < listaFiliado.Count(); i++)
            {
                if (i == (listaFiliado.Count / 2))
                {
                    Thread.Sleep(10000);
                }
                AcessarTelaSolicitarMigracao();
                PesquisarMigracao(listaFiliado[i].documento);
                var log = $"{i + 1}: {listaFiliado[i].documento}";
                var retorno = RealizarPedidoMigracao();
                _base.GravarLog(log, retorno);
                AcessarPaginaPrincipal();
            }
        }

        private void AcessarPaginaPrincipal()
        {
            const string Url = "//*[@id=\"0\"]/ul/li/a[@href=\"../../PrincipalMensagens.aspx\"]";

            navegador.FindElement(By.XPath("//*[@id=\"0\"]/h3")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(Url)));
            navegador.FindElement(By.XPath(Url)).Click();
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