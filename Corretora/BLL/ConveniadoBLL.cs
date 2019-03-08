using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SisCorDAL;
using Modelos;


namespace SisCorBLL
{
    public class ConveniadoBLL
    {
        public void InserirConveniado(ConveniadoVO conveniado)
        {
            ConveniadoRepositorio.InserirConveniado(conveniado);
        }

        //Metodo para carregar combobox da Corretora
        public IList<CorretoraVO> ListarCorretora()
        {
            return CorretoraRepositorio.ListarCorretora();
        }

        public IList<ConveniadoVO> ListarConveniado()
        {
            return ConveniadoRepositorio.ListarConveniado();
        }

        public void ExcluirConveniado(ConveniadoVO conveniado)
        {
            ConveniadoRepositorio.ExcluirConveniado(conveniado);
        }

        public void AlterarConveniado(ConveniadoVO conveniado)
        {
            ConveniadoRepositorio.AlterarConveniado(conveniado);
        }
    }
}
