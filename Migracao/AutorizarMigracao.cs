using OpenQA.Selenium;
using System;

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
            foreach (var filiado in filiadosParaMigrar.listaFiliado)
            {
                AcessarTelaAutorizarMigracao();
                //PesquisarMigracao(filiado.documento);
                //var retorno = RealizarPedidoMigracao();
                //GravarLog(filiado.documento, retorno);
            }
        }

        private void AcessarTelaAutorizarMigracao()
        {
            navegador.FindElement(By.XPath("//*[@id=\"1\"]/h3")).Click();
            navegador.FindElement(By.XPath("//*[@id=\"1\"]/ul/li/a[@href=\"paginas/filiado/AutorizaMigracao.aspx\"]")).Click();
        }
    }
}
