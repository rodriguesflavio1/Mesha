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
        public static void InserirCorretora(CorretoraVO corretora)
        {

            SqlParameter spNome = new SqlParameter("@nome", SqlDbType.VarChar,100) { Value = corretora.Nome };
            SqlParameter spPercentual = new SqlParameter("@perc", SqlDbType.Float) { Value = corretora.Percentual};
          
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
                            Id = Convert.ToInt32(reader.GetValue(0)),
                            Nome = reader.VarCharParaString("nome"),
                            Percentual = reader.DoubleParaFloat("percentual")
                                                      
                        });
                    }
                }
            }
            return listaCorretoras;
        }

        public static void AlterarCorretora(CorretoraVO corretora)
        {
            SqlParameter spId = new SqlParameter("@id", SqlDbType.Int) { Value = corretora.Id };
            SqlParameter spNome = new SqlParameter("@nome", SqlDbType.VarChar, 100) { Value = corretora.Nome };
            SqlParameter spPercentual = new SqlParameter("@perc", SqlDbType.Float) { Value = corretora.Percentual };

            using (BancoDados bd = new BancoDados())
            {
                bd.NonQuery("SP_ALTERAR_CORRETORA", CommandType.StoredProcedure, spId,spNome, spPercentual);
            }
        }

        public static void ExcluirCorretora(CorretoraVO corretora)
        {
            SqlParameter spId = new SqlParameter("@id", SqlDbType.Int) { Value = corretora.Id};

            using (BancoDados bd = new BancoDados())
            {
                bd.NonQuery("SP_EXCLUIR_CORRETORA", CommandType.StoredProcedure, spId);
            }
        }
    }
}
