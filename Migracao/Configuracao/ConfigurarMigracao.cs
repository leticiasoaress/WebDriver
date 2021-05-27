using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;

namespace Migracao
{
    public class ConfigurarMigracao
    {
        private FiliadosParaMigrar filiadosParaMigrar;
        private SolicitarMigracao solicitarMigracao;
        private MigrarFiliado migrarFiliado;
        private Base _base;

        public ConfigurarMigracao(IWebDriver _navegador)
        {
            filiadosParaMigrar = new FiliadosParaMigrar();
            solicitarMigracao = new SolicitarMigracao(_navegador);
            migrarFiliado = new MigrarFiliado(_navegador);
            _base = new Base(_navegador);
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.Clear();
           
            var arquivo = _base.GerarTxt();
            _base.GravarLog(arquivo, "Iniciando processo de migração.\n");
            _base.GravarLog(arquivo, $"Processo iniciado: {DateTime.Now} \n");

            int contador = 1;
            int quantidadePedido = filiadosParaMigrar.listaFiliado.Count();

            foreach (var filiado in filiadosParaMigrar.listaFiliado)
            {
                _base.GravarLog(arquivo, $"\nPedido {contador} / {quantidadePedido}");

                var retorno = solicitarMigracao.SolicitarMigracaoFranquia(filiado.documento, arquivo);
                if (retorno == "Filiado autorizado com sucesso.")
                {
                    migrarFiliado.MigrarFiliadoFranquia(filiado, arquivo);
                }
                
                contador++;
                Thread.Sleep(5000);
            }
            _base.GravarLog(arquivo, $"\nProcesso finalizado: {DateTime.Now} \n\n");
            _base.FecharTxt(arquivo);
        }
    }
}