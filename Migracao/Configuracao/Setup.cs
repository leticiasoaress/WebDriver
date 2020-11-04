using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;


namespace Migracao
{
    public class Setup
    {
        private IWebDriver driver;
        private Login login;
        private ConfigurarMigracao configurarMigracao;

        public Setup(string url)
        {
            driver = new ChromeDriver();
            driver.Url = url;
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            login = new Login(driver);
            configurarMigracao = new ConfigurarMigracao(driver);
        }

        public void ConfigurarOrdemExecucao()
        {
            Console.Clear();
            InformarAcesso();
            login.SelecionarRegional(1);
            login.SelecionarFranquia(7);
            configurarMigracao.ConfigurarOrdemExecucao();
        }

        public void InformarAcesso()
        {
            Console.WriteLine("Gentileza informar o acesso ao sistema");
            Console.Write("Login: ");
            var _login = Console.ReadLine();
            Console.Write("Senha: ");
            var _senha = Console.ReadLine();

            login.ConfigurarLogin(_login, _senha);
        }

        public void FinalizarProcesso()
        {
            Console.WriteLine("\n\n Finalizando aplicação! \n Pressiona qualquer tecla para sair....");
            Console.ReadKey();
            driver.Quit();
        }
    }
}