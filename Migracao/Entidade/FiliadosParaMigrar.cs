namespace Migracao
{
    public class FiliadosParaMigrar
    {
        public readonly DadosFiliado[] listaFiliado;

        public FiliadosParaMigrar()
        {
            listaFiliado = new DadosFiliado[]
            {
                new DadosFiliado(_documento: "1030206", _idRegionalOrigem: 1, _idFranquiaOrigem: 2, _idFormaPgtoPai: 3200, _idFormaPgto: 22, _uc: "0"),
                new DadosFiliado(_documento: "581573", _idRegionalOrigem: 1, _idFranquiaOrigem: 2, _idFormaPgtoPai: 5301129, _idFormaPgto: 5301130, _uc: "35083009")
            };
        }
    }
}