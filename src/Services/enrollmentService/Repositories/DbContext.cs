
using Microsoft.Data.SqlClient;
using System.Data;

namespace enrollmentService.Repositories
{
    public class DbContext
    {
        private string connectionString { get; set; }

        public DbContext()
        {
            connectionString = @"Server=DESKTOP-LQ6C96V\SQLEXPRESS;Database=LanguageCenterDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable data = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
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
                using (SqlConnection connection = new SqlConnection(connectionString))
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
                using (SqlConnection connection = new SqlConnection(connectionString))
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
                using (SqlConnection connection = new SqlConnection(connectionString))
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
    }
}