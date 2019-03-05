using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Modelos;

namespace SisCorDAL
{
    public class CorretoraRepositorio
    {
        public static void InserirCorretora(string nome, double perc) {

            SqlParameter spNome = new SqlParameter("@nome", SqlDbType.VarChar,100) { Value = nome };
            SqlParameter spPercentual = new SqlParameter("@perc", SqlDbType.Float) { Value = perc };
          

            using (BancoDados bd = new BancoDados())
            {
                bd.NonQuery("SP_INCLUSAO_CORRETORA", CommandType.StoredProcedure, spNome, spPercentual);
            }

            


        }

        public static IList<CorretoraVO> ListarCorretora() {

            List<CorretoraVO> listaCorretoras = new List<CorretoraVO>();

            using (BancoDados bd = new BancoDados())
            {
                using (SqlDataReader reader = bd.Reader("SP_LISTAR_CORRETORAS", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        listaCorretoras.Add(new CorretoraVO()
                        {
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
