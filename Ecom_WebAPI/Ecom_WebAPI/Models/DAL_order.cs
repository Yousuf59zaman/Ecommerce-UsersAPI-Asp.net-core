using System.Data.SqlClient;
using System.Collections.Generic;


namespace Ecom_WebAPI.Models
{
    public class DAL_order
    {
        public int PlaceOrder(Order order, List<OrderDetail> orderItems, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert the order
                        string insertOrderQuery = "INSERT INTO Orders (UserId, OrderDate, TotalAmount) OUTPUT INSERTED.OrderId VALUES (@UserId, @OrderDate, @TotalAmount)";
                        using (SqlCommand command = new SqlCommand(insertOrderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", order.UserId);
                            command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                            command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                            int orderId = (int)command.ExecuteScalar();
                            order.OrderId = orderId;
                        }

                        // Insert order items
                        foreach (var item in orderItems)
                        {
                            string insertItemQuery = "INSERT INTO OrderDetail (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                            using (SqlCommand command = new SqlCommand(insertItemQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@OrderId", order.OrderId);
                                command.Parameters.AddWithValue("@ProductId", item.ProductId);
                                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                                command.Parameters.AddWithValue("@Price", item.Price);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return order.OrderId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return -1; // Indicate failure
                    }
                }
            
        }

    }

        public List<Order> GetOrdersByUserId(int userId,string _connectionString)
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT OrderId, UserId, OrderDate, TotalAmount FROM Orders WHERE UserId = @UserId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                UserId = (int)reader["UserId"],
                                OrderDate = (DateTime)reader["OrderDate"],
                                TotalAmount = (decimal)reader["TotalAmount"]
                            });
                        }
                    }
                }
            }
            return orders;
        }


    }
}
