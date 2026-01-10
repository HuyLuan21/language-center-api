using Microsoft.Data.SqlClient;
using System.Data;

namespace scheduleService.Repositories
{
    public class DbContext
    {
        private readonly string _connectionString;

        public DbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
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

        public DataTable ExecuteQueryWithParameters(string query, Dictionary<string, object> parameters)
        {
            DataTable data = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the command
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }

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

        public int ExecuteNonQueryWithParameters(string query, Dictionary<string, object> parameters)
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
                                foreach (var param in parameters)
                                {
                                    command.Parameters.AddWithValue(param.Key, param.Value);
                                }
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
    }
}
