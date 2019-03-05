using System;
using System.Data;
using System.Data.SqlClient;

namespace SisCorDAL
{
    public class BancoDados : IDisposable
    {
        protected SqlConnection _conexao;
        private SqlDataReader _reader;

        protected string _servidor = "DESKTOP-5N3USB3";
        protected const string _usuario = "sa";
        protected const string _senha = "uxoria";
        protected const string _database = "Mesha";
        protected const int _maxPoolSize = 5000;

        public BancoDados()
        {
                _conexao = new SqlConnection(
                            string.Format(@"Server={0};UID={1};PWD={2};Database={3};MAX POOL SIZE={4}",
                                _servidor,
                                _usuario,
                                _senha,
                                _database,
                                _maxPoolSize
                            )
                        );
        }

        public BancoDados(string servidor, string usuario, string senha, string database, int maxPoolSize)
        {
            _conexao = new SqlConnection(
                            string.Format(@"Server={0};UID={1};PWD={2};Database={3};MAX POOL SIZE={4}",
                                servidor,
                                usuario,
                                senha,
                                database,
                                maxPoolSize
                            )
                        );
        }

        private bool Conectar()
        {
            if (_conexao != null)
            {
                switch (_conexao.State)
                {
                    case ConnectionState.Open:
                        return true;

                    case ConnectionState.Closed:
                        _conexao.Open();
                        return true;

                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void Desconectar()
        {
            if (_conexao != null && _conexao.State == ConnectionState.Open)
            {
                _conexao.Close();
            }
        }

        /// <summary>
        /// Executar Reader
        /// </summary>
        /// <param name="textoComando"></param>
        /// <param name="tipoComando"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public SqlDataReader Reader(string textoComando, CommandType tipoComando, params SqlParameter[] parametros)
        {
            _reader = null;

            try
            {
                using (SqlCommand comandoSQL = new SqlCommand() { CommandText = textoComando, CommandType = tipoComando })
                {
                    foreach (SqlParameter parametro in parametros)
                    {
                        if (parametro != null)
                        {
                            comandoSQL.Parameters.Add(parametro);
                        }
                    }

                    if (Conectar())
                    {
                        comandoSQL.Connection = _conexao;

                        _reader = comandoSQL.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _reader;
        }

        /// <summary>
        /// Executar ExecuteNonQuery
        /// </summary>
        /// <param name="textoComando"></param>
        /// <param name="tipoComando"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public int NonQuery(string textoComando, CommandType tipoComando, params SqlParameter[] parametros)
        {
            int retorno = -1;

            try
            {
                using (SqlCommand comandoSQL = new SqlCommand() { CommandText = textoComando, CommandType = tipoComando })
                {
                    foreach (SqlParameter parametro in parametros)
                    {
                        if (parametro != null)
                        {
                            comandoSQL.Parameters.Add(parametro);
                        }
                    }

                    if (Conectar())
                    {
                        comandoSQL.Connection = _conexao;

                        retorno = comandoSQL.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Desconectar();
            }

            return retorno;
        }


        /// <summary>
        /// Executar comando Scalar.
        /// </summary>
        /// <param name="textoComando"></param>
        /// <param name="tipoComando"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public object Scalar(string textoComando, CommandType tipoComando, params SqlParameter[] parametros)
        {
            object retorno = null;

            try
            {
                using (SqlCommand comandoSQL = new SqlCommand() { CommandText = textoComando, CommandType = tipoComando })
                {
                    foreach (SqlParameter parametro in parametros)
                    {
                        if (parametro != null)
                        {
                            comandoSQL.Parameters.Add(parametro);
                        }
                    }

                    if (Conectar())
                    {
                        comandoSQL.Connection = _conexao;

                        retorno = comandoSQL.ExecuteScalar();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Desconectar();
            }

            return retorno;
        }

        // 
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                Desconectar();

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _conexao = null;
                _servidor = null;
                _reader = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~BancoDados()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public static class ExtensoesSQLDataReader
    {
        /// <summary>
        /// Converte uma VarChar/NVarChar(SQL Server) para string(C#). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static string VarCharParaString(this SqlDataReader sr, string nomeColuna)
        {
            string retorno = String.Empty;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetString(sr.GetOrdinal(nomeColuna));
                }
                else
                {
                    retorno = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar VarCharParaString [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte uma float(SQL Server) para double(C#). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static double DoubleParaFloat(this SqlDataReader sr, string nomeColuna)
        {
            double retorno ;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetDouble(sr.GetOrdinal(nomeColuna));
                }
                else
                {
                    retorno = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar DoubleParafloat [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte uma Bit(SQL Server) para bool?(C# - nullable). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static bool? BitParaBoolean(this SqlDataReader sr, string nomeColuna)
        {
            bool? retorno = null;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetBoolean(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar BitParaBoolean [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte uma Bit(SQL Server) para bool(C#). Retorna "valorPadrao" caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <param name="valorPadrao"></param>
        /// <returns></returns>
        public static bool BitParaBooleanNN(this SqlDataReader sr, string nomeColuna, bool valorPadrao = false)
        {
            bool retorno = valorPadrao;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetBoolean(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar BitParaBoolean [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um BigInt(SQL Server) para long?(C# - nullable). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static long? BigIntParaLong(this SqlDataReader sr, string nomeColuna)
        {
            long? retorno = null;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetInt64(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar BigIntParaLong [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um BigInt(SQL Server) para long(C#). Retorna "valorPadrao" caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <param name="valorPadrao"></param>
        /// <returns></returns>
        public static long BigIntParaLongNN(this SqlDataReader sr, string nomeColuna, long valorPadrao = 0)
        {
            long retorno = valorPadrao;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetInt64(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar BigIntParaLong [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um Int(SQL Server) para int?(C# - nullable). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static int? IntParaInt(this SqlDataReader sr, string nomeColuna)
        {
            int? retorno = null;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetInt32(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar IntParaInt [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um Int(SQL Server) para int(C#). Retorna "valorPadrao" caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <param name="valorPadrao"></param>
        /// <returns></returns>
        public static int IntParaIntNN(this SqlDataReader sr, string nomeColuna, int valorPadrao = 0)
        {
            int retorno = valorPadrao;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetInt32(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar IntParaInt [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um SmallInt(SQL Server) para short?(C# - nullable). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static short? SmallIntParaShort(this SqlDataReader sr, string nomeColuna)
        {
            short? retorno = null;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetInt16(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar SmallIntParaShort [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um SmallInt(SQL Server) para short(C#). Retorna "valorPadrao" caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <param name="valorPadrao"></param>
        /// <returns></returns>
        public static short SmallIntParaShortNN(this SqlDataReader sr, string nomeColuna, short valorPadrao = 0)
        {
            short retorno = 0;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetInt16(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar SmallIntParaShort [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um TinyInt(SQL Server) para byte?(C# - nullable). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static byte? TinyIntParaByte(this SqlDataReader sr, string nomeColuna)
        {
            byte? retorno = null;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetByte(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar TinyIntParaByte [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um TinyInt(SQL Server) para byte(C#). Retorna "valorPadrao" caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <param name="valorPadrao"></param>
        /// <returns></returns>
        public static byte TinyIntParaByteNN(this SqlDataReader sr, string nomeColuna, byte valorPadrao = 0)
        {
            byte retorno = valorPadrao;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetByte(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar TinyIntParaByte [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um DateTime(SQL Server) para DateTime?(C# - nullable). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static DateTime? DateTimeParaDateTime(this SqlDataReader sr, string nomeColuna)
        {
            DateTime? retorno = null;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetDateTime(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar DateTimeParaDateTime [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um DateTime(SQL Server) para DateTime(C#). Retorna DateTime.Now caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static DateTime DateTimeParaDateTimeNN(this SqlDataReader sr, string nomeColuna)
        {
            DateTime retorno = DateTime.Now;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetDateTime(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar DateTimeParaDateTime [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um Decimal(SQL Server) para Decimal(C#). Retorna null caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <returns></returns>
        public static decimal? DecimalParaDecimal(this SqlDataReader sr, string nomeColuna)
        {
            decimal? retorno = null;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetDecimal(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar DateTimeParaDateTime [{0}].", ex.Message));
            }

            return retorno;
        }

        /// <summary>
        /// Converte um Decimal(SQL Server) para Decimal(C#). Retorna "valorPadrao" caso a coluna seja DBNull.
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="nomeColuna"></param>
        /// <param name="valorPadrao"></param>
        /// <returns></returns>
        public static decimal DecimalParaDecimalNN(this SqlDataReader sr, string nomeColuna, decimal valorPadrao = 0.0m)
        {
            decimal retorno = valorPadrao;

            try
            {
                if (!sr.IsDBNull(sr.GetOrdinal(nomeColuna)))
                {
                    retorno = sr.GetDecimal(sr.GetOrdinal(nomeColuna));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro ao executar DateTimeParaDateTime [{0}].", ex.Message));
            }

            return retorno;
        }
    }
}
