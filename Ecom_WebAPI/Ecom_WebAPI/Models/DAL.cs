using System.Data.SqlClient;

namespace Ecom_WebAPI.Models
{
    public class DAL
    {
        public bool RegisterUser(User user, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (Email, Password, FullName) VALUES (@Email, @Password, @FullName)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password); // Remember to hash the password in a real application
                    command.Parameters.AddWithValue("@FullName", user.FullName);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }

        public User AuthenticateUser(string email, string password, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT UserId, Email, FullName FROM Users WHERE Email = @Email AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password); // In real application, compare hashed passwords

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = (int)reader["UserId"],
                                Email = reader["Email"].ToString(),
                                FullName = reader["FullName"].ToString()
                                // Do not return the password
                            };
                        }
                    }
                }
            }

            return null; // Authentication failed
        }


        public User GetUserById(int userId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT UserId, Email, FullName FROM Users WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = (int)reader["UserId"],
                                Email = reader["Email"].ToString(),
                                FullName = reader["FullName"].ToString()
                            };
                        }
                    }
                }
            }

            return null; // User not found
        }


        public bool UpdateUser(User user,string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users SET Email = @Email, FullName = @FullName WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@FullName", user.FullName);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }


    }
}



