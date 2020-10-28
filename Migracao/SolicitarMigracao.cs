using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using SupportUI = OpenQA.Selenium.Support.UI;

namespace Migracao
{
    public class SolicitarMigracao
    {
        private readonly IWebDriver navegador;
        private FiliadosParaMigrar filiadosParaMigrar;
        private SupportUI.WebDriverWait wait;

        public SolicitarMigracao(IWebDriver _navegador)
        {            
            navegador = _navegador;
            filiadosParaMigrar = new FiliadosParaMigrar();
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(40));
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.Clear();
            Console.WriteLine("Iniciando pedido de migração.\n\n");
            Console.WriteLine("Log dos documentos");
            foreach (var filiado in filiadosParaMigrar.listaFiliado)
            {
                AcessarTelaSolicitarMigracao();
                PesquisarMigracao(filiado.documento);
                var retorno = RealizarPedidoMigracao();
                GravarLog(filiado.documento, retorno);
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
            const string BtnPesquisar = "\"ContentPlaceHolder1_btnPesquisar\"";
            const string CampoCPF = "\"ContentPlaceHolder1_txbCpf\"";
            
            navegador.FindElement(By.Id(CampoCPF)).SendKeys(documento);
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(BtnPesquisar)));
            navegador.FindElement(By.Id(BtnPesquisar)).Click();
        }

        private string RealizarPedidoMigracao()
        {
            const string CheckBox = "\"ContentPlaceHolder1_gvInfoFiliados_cbSeleciona_0\"";
            const string BtnSolicitar = "\"ContentPlaceHolder1_gvInfoFiliados_btnAdd\"";
            const string MessageBox = "\"MyMessageBox1_MessageBoxInterface\"";

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(CheckBox)));
            navegador.FindElement(By.Id(CheckBox)).Click();
            navegador.FindElement(By.Id(BtnSolicitar)).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(MessageBox)));
            return navegador.FindElement(By.Id(MessageBox)).Text;
        }

        private void GravarLog(string documento, string retorno)
        {          
            Console.WriteLine($@"Documento: {documento} --> Retorno: {retorno}");     
        }
    }
}