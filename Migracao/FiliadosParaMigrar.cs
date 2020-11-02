namespace Migracao
{
    public class FiliadosParaMigrar
    {
        public readonly DadosFiliado[] listaFiliado;
        public readonly (int idRegional, int idFranquia)[] listaFranquia;

        public FiliadosParaMigrar()
        {
            listaFiliado = new DadosFiliado[]
            {
               new DadosFiliado(_documento: "1126424399", _idRegionalOrigem: 1, _idFranquiaOrigem: 2, _idFormaPgtoPai:3200,    _idFormaPgto: 22,     _uc:0)
            };

            listaFranquia = new (int, int)[]
            {
                (1,2)
            };
        }
    }
}