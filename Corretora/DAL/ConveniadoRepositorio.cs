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
    public class ConveniadoRepositorio
    {
        //retorna a lista de corretoras para prenchimento do combobox 
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

        public static void InserirConveniado(ConveniadoVO conveniado)
        {
            SqlParameter spNome = new SqlParameter("@nome", SqlDbType.VarChar, 100) { Value = conveniado.Nome };
            SqlParameter spNascimento = new SqlParameter("@nascimento", SqlDbType.DateTime) { Value = conveniado.Nascimento };
            SqlParameter spIdCorretora = new SqlParameter("@Idcorretora", SqlDbType.Int, 100) { Value = conveniado.IdCorretora };

            using (BancoDados bd = new BancoDados())
            {
                bd.NonQuery("SP_INCLUSAO_CONVENIADO", CommandType.StoredProcedure, spNome, spNascimento, spIdCorretora);
            }
        }

        public static IList<ConveniadoVO> ListarConveniado()
        {

             List<ConveniadoVO> listaConveniado = new List<ConveniadoVO>();

            using (BancoDados bd = new BancoDados())
            {
                using (SqlDataReader reader = bd.Reader("SP_LISTAR_CONVENIADO", CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        listaConveniado.Add(new ConveniadoVO()
                        {
                            Id = Convert.ToInt32(reader.GetValue(0)),
                            Nome = Convert.ToString(reader.GetValue(1)),
                            IdCorretora = Convert.ToInt32(reader.GetValue(3)),
                            Nascimento = Convert.ToDateTime(reader.GetValue(2))

                        });
                    }
                }
            }
            return listaConveniado;
        }

        public static void ExcluirConveniado(ConveniadoVO conveniado)
        {
            SqlParameter spId = new SqlParameter("@id", SqlDbType.Int) { Value = conveniado.Id };

            using (BancoDados bd = new BancoDados())
            {
                bd.NonQuery("SP_EXCLUIR_CONVENIADO", CommandType.StoredProcedure, spId);
            }
        }

        public static void AlterarConveniado(ConveniadoVO conveniado)
        {
            SqlParameter spId = new SqlParameter("@idCorretora", SqlDbType.Int) { Value = conveniado.IdCorretora };
            SqlParameter spNome = new SqlParameter("@nome", SqlDbType.VarChar, 100) { Value = conveniado.Nome };
            SqlParameter spPercentual = new SqlParameter("@nascimento", SqlDbType.DateTime) { Value = conveniado.Nascimento };

            using (BancoDados bd = new BancoDados())
            {
                bd.NonQuery("SP_ALTERAR_CONVENIADO", CommandType.StoredProcedure, spId, spNome, spPercentual);
            }
        }

    }
}
