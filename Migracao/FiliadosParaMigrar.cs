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
                
            };

            listaFranquia = new (int, int)[]
            {
                (1,2)
            };
        }
    }
}