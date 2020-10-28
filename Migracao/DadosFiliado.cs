using System;
using System.Collections.Generic;
using System.Text;

namespace Migracao
{
    public class DadosFiliado
    {
        public string documento { get; set; }

        public int idFranquiaOrigem { get; set; }

        public int idRegionalOrigem { get; set; }

        public DadosFiliado(string _documento, int _idRegionalOrigem, int _idFranquia)
        {
            documento = _documento;
            idFranquiaOrigem = _idFranquia;
            idRegionalOrigem = _idRegionalOrigem;
        }
    }
}
