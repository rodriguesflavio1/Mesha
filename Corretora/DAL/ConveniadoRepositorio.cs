using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Modelos;
using SisCorDAL;

namespace DAL
{
    public class ConveniadoRepositorio
    {
        public static IList<CorretoraVO> ListarCorretora()
        {

            List<CorretoraVO> listaCorretoras = new List<CorretoraVO>();

            using (BancoDados bd = new BancoDados())
            {
                using (SqlDataReader reader = bd.Reader("SP_LISTAR_CORRETORAS", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        listaCorretoras.Add(new CorretoraVO()
                        {
                            Id = Convert.ToInt32(reader.GetValue(0)),
                            Nome = reader.VarCharParaString("nome"),
                            Percentual = reader.DoubleParaFloat("percentual")

                        });
                    }
                }
            }
            return listaCorretoras;
        }
    }
}
