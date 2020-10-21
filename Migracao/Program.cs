using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace Migracao
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando aplicação");
            
            var setup = new Setup();
            var navegador = setup.ConfigurarNavegador();

            setup.RealizarLogin();
            setup.SelecionarRegional();
            setup.SelecionarFranquia();

            var solicitarMigracao = new SolicitarMigracao(navegador);
            solicitarMigracao.AcessarTelaSolicitarMigracao();

            setup.FinalizarProcesso();
        }
    }
}
