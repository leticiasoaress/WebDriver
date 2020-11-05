namespace Migracao
{
    public class FiliadosParaMigrar
    {
        public readonly DadosFiliado[] listaFiliado;

        public FiliadosParaMigrar()
        {
            listaFiliado = new DadosFiliado[]
            {
                new DadosFiliado(_documento: "19829108", _idRegionalOrigem: 1, _idFranquiaOrigem: 2, _idFormaPgtoPai: 5301129, _idFormaPgto: 5301130,   _uc: "02197697")
            };
        }
    }
}