using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Migracao
{
    public class SolicitarMigracao
    {
        private readonly IWebDriver _navegador;

        public SolicitarMigracao(IWebDriver navegador)
        {
            Console.Clear();
            Console.WriteLine("Iniciando pedido de migração.");
            _navegador = navegador;
        }

        public void AcessarTelaSolicitarMigracao()
        {
            _navegador.FindElement(By.XPath("//*[@id=\"1\"]/h3")).Click();
            _navegador.FindElement(By.XPath("//*[@id=\"1\"]/ul/li[6]/a")).Click();

            var cpfInformado = PesquisarMigracao();
            var retorno = RealizarPedidoMigracao();
            GravarLog(cpfInformado, retorno);
        }

        public string PesquisarMigracao()
        {
            _navegador.FindElement(By.Id("ContentPlaceHolder1_txbCpf")).SendKeys("12183934670");
            _navegador.FindElement(By.Id("ContentPlaceHolder1_btnPesquisar")).Click();
            return _navegador.FindElement(By.XPath("//*[@id=\"ContentPlaceHolder1_gvInfoFiliados\"]/tbody/tr[2]/td[3]")).Text;
        }

        public string RealizarPedidoMigracao()
        {
            _navegador.FindElement(By.Id("ContentPlaceHolder1_gvInfoFiliados_cbSeleciona_0")).Click();
            _navegador.FindElement(By.Id("ContentPlaceHolder1_gvInfoFiliados_btnAdd")).Click();
            return _navegador.FindElement(By.XPath("//*[@id=\"MyMessageBox1_MessageBoxInterface\"]/p")).Text;
        }

        public void GravarLog(string cpf, string retorno)
        {
            Console.WriteLine($@"CPF: {cpf} --> Retorno: {retorno}");     
        }
    }
}