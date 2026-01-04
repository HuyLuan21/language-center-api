using Microsoft.Data.SqlClient;
using System.Data;


namespace examService.Repositories
{
    public class DbContext
    {
        private string _connectionString;

        public DbContext()
        {
            _connectionString = "Server=Lias;Database=LanguageCenterDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable data = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(data);
                        }
                    }

                    connection.Close();
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return data;
        }

        public int ExecuteNonQuery(string query)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            {
                                rowsAffected = command.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                        catch (SqlException)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return rowsAffected;
        }

        public int ExecuteNonQuery(string commandText, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.CommandType = commandType;

                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        rowsAffected = command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return rowsAffected;
        }

        public object ExecuteScalar(string query)
        {
            object? result = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        result = command.ExecuteScalar();
                    }

                    connection.Close();
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return result ?? new object();
        }

        // Thêm hàm này vào class DbContext của bạn
        public object? ExecuteScalar(string commandText, SqlParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            object? result = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(commandText, connection))
                    {
                        command.CommandType = commandType;

                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        result = command.ExecuteScalar();
                    }

                    connection.Close();
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return result;
        }
    }
}