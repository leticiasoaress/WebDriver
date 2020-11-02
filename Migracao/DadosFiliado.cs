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
        
        public int idFormaPgtoPai { get; set; }
        
        public int idFormaPgto { get; set; }

        public int uc { get; set; }

        public DadosFiliado(string _documento, int _idRegionalOrigem, int _idFranquiaOrigem, int _idFormaPgtoPai, int _idFormaPgto, int _uc)
        {
            documento = _documento;
            idFranquiaOrigem = _idFranquiaOrigem;
            idRegionalOrigem = _idRegionalOrigem;
            idFormaPgtoPai = _idFormaPgtoPai;
            idFormaPgto = _idFormaPgto;
            uc = _uc;
        }
    }
}