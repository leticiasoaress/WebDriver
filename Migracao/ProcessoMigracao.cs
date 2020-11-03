using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using SupportUI = OpenQA.Selenium.Support.UI;

namespace Migracao
{
    public class ProcessoMigracao
    {
        private readonly IWebDriver navegador;
        private FiliadosParaMigrar filiadosParaMigrar;
        private Base _base;
        private SupportUI.WebDriverWait wait;
        private SolicitarMigracao solicitarMigracao;
        private AutorizarMigracao autorizarMigracao;
        private MigrarFiliado migrarFiliado;

        public ProcessoMigracao(IWebDriver _navegador)
        {
            navegador = _navegador;
            _base = new Base(_navegador);
            filiadosParaMigrar = new FiliadosParaMigrar();
            solicitarMigracao = new SolicitarMigracao(_navegador);
            //autorizarMigracao = new AutorizarMigracao(_navegador);
            migrarFiliado = new MigrarFiliado(_navegador);
            wait = new SupportUI.WebDriverWait(_navegador, TimeSpan.FromSeconds(60));
        }

        public void ConfigurarOrdemExecucao()
        {
            foreach (var filiado in filiadosParaMigrar.listaFiliado)
            {
                solicitarMigracao.ConfigurarOrdemExecucao(filiado.documento);
            }
        }
    }
}
