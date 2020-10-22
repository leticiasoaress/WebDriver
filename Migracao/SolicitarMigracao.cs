using OpenQA.Selenium;
using System;

namespace Migracao
{
    public class SolicitarMigracao
    {
        private readonly IWebDriver _navegador;
        private FiliadosParaMigrar filiadosParaMigrar;


        public SolicitarMigracao(IWebDriver navegador)
        {            
            _navegador = navegador;
            filiadosParaMigrar = new FiliadosParaMigrar();
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
            } 
        }

        private void AcessarTelaSolicitarMigracao()
        {
            _navegador.FindElement(By.XPath("//*[@id=\"1\"]/h3")).Click();
            _navegador.FindElement(By.XPath("//*[@id=\"1\"]/ul/li[8]/a")).Click();
        }

        private void PesquisarMigracao(string documento)
        {
            _navegador.FindElement(By.Id("ContentPlaceHolder1_txbCpf")).SendKeys(documento);
            _navegador.FindElement(By.Id("ContentPlaceHolder1_btnPesquisar")).Click();
        }

        private string RealizarPedidoMigracao()
        {
            _navegador.FindElement(By.Id("ContentPlaceHolder1_gvInfoFiliados_cbSeleciona_0")).Click();
            _navegador.FindElement(By.Id("ContentPlaceHolder1_gvInfoFiliados_btnAdd")).Click();
            return _navegador.FindElement(By.XPath("//*[@id=\"MyMessageBox1_MessageBoxInterface\"]/p")).Text;
        }

        private void GravarLog(string documento, string retorno)
        {          
            Console.WriteLine($@"Documento: {documento} --> Retorno: {retorno}");     
        }
    }
}