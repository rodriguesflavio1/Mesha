using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisCorDAL;
using Modelos;
namespace BLL
{
    public class ConveniadoBLL
    {
        public void InserirConveniado(ConveniadoVO conveniado)
        {
            CorretoraRepositorio.InserirCorretora(conveniado);
        }

        public IList<CorretoraVO> ListarCorretora()
        {
            return CorretoraRepositorio.ListarCorretora();
        }
    }
}
