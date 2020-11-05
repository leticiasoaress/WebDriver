using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;
using SupportUI = OpenQA.Selenium.Support.UI;

namespace Migracao
{
    public class ConfigurarMigracao
    {
        private FiliadosParaMigrar filiadosParaMigrar;
        private SolicitarMigracao solicitarMigracao;
        private MigrarFiliado migrarFiliado;

        public ConfigurarMigracao(IWebDriver _navegador)
        {
            filiadosParaMigrar = new FiliadosParaMigrar();
            solicitarMigracao = new SolicitarMigracao(_navegador);
            migrarFiliado = new MigrarFiliado(_navegador);
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.Clear();
            Console.WriteLine("Iniciando processo de migração.\n");
            Console.WriteLine("Processo iniciado: " + DateTime.Now);

            int contador = 1;
            int quantidadePedido = filiadosParaMigrar.listaFiliado.Count();

            foreach (var filiado in filiadosParaMigrar.listaFiliado)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n\nPedido {contador} / {quantidadePedido}");
                Console.ResetColor();
                var retorno = solicitarMigracao.SolicitarMigracaoFranquia(filiado.documento);
                if (retorno == "Filiado autorizado com sucesso.")
                {
                    migrarFiliado.MigrarFiliadoFranquia(filiado);
                }
                
                contador++;
                Thread.Sleep(5000);
            }
            Console.WriteLine("\nProcesso finalizado: " + DateTime.Now);
        }
    }
}
