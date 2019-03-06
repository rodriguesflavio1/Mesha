using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisCorDAL;
using Modelos;

namespace SisCorBLL
{
    public class CorretoraBLL
    {
        public void InserirCorretora(string _nome,double _perc)
        {
           CorretoraRepositorio.InserirCorretora(_nome, _perc);
        }

        public IList<CorretoraVO> ListarCorretora()
        {
            return CorretoraRepositorio.ListarCorretora();
        }

        public void AlterarCorretora(CorretoraVO corretora)
        {
            CorretoraRepositorio.AlterarCorretora(corretora);
        }

        public void ExcluirCorretora(CorretoraVO corretora)
        {
            CorretoraRepositorio.ExcluirCorretora(corretora);
        }
    }
}
