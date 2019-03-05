using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace SisCorDAL
{
    public class Contexto
    {
        private readonly SqlConnection minhaConexao = null;

        //Conexao com o banco de dados
        public Contexto()
        {
            //instrução que recebe a conectionstring e realiza a conexao com o banco
            minhaConexao = new SqlConnection(ConfigurationManager.ConnectionStrings["stringConexao"].ConnectionString);
            minhaConexao.Open();

           


        }
    }
}
