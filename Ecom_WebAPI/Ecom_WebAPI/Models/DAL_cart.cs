using System.Data.SqlClient;

namespace Ecom_WebAPI.Models
{
    public class DAL_cart
    {
        public bool AddToCart(CartItem cartItem, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO CartItem (UserId, ProductId, Quantity) VALUES (@UserId, @ProductId, @Quantity)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", cartItem.UserId);
                    command.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
                    command.Parameters.AddWithValue("@Quantity", cartItem.Quantity);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }

        public List<CartItem> GetCartItems(int userId, string _connectionString)
        {
            List<CartItem> cartItems = new List<CartItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT CartItemId, UserId, ProductId, Quantity FROM CartItem WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartItems.Add(new CartItem
                            {
                                CartItemId = (int)reader["CartItemId"],
                                UserId = (int)reader["UserId"],
                                ProductId = (int)reader["ProductId"],
                                Quantity = (int)reader["Quantity"]
                            });
                        }
                    }
                }
            }

            return cartItems;
        }

        public bool RemoveFromCart(int cartItemId, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM CartItem WHERE CartItemId = @CartItemId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CartItemId", cartItemId);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
        }



    }
}
