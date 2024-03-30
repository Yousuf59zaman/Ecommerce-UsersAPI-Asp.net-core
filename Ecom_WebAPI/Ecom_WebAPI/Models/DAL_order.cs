using System.Data.SqlClient;

namespace Ecom_WebAPI.Models
{
    public class DAL_order
    {
        public int PlaceOrder(Order order, List<OrderDetail> orderDetails, string _connectionString)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert the order
                        string orderQuery = "INSERT INTO Orders (UserId, OrderDate, TotalAmount) OUTPUT INSERTED.OrderId VALUES (@UserId, @OrderDate, @TotalAmount)";
                        using (SqlCommand orderCommand = new SqlCommand(orderQuery, connection, transaction))
                        {
                            orderCommand.Parameters.AddWithValue("@UserId", order.UserId);
                            orderCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                            orderCommand.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                            order.OrderId = (int)orderCommand.ExecuteScalar();
                        }

                        // Insert each order detail
                        foreach (var detail in orderDetails)
                        {
                            string detailQuery = "INSERT INTO OrderDetails (OrderId, ProductId, Quantity, Price) VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                            using (SqlCommand detailCommand = new SqlCommand(detailQuery, connection, transaction))
                            {
                                detailCommand.Parameters.AddWithValue("@OrderId", order.OrderId);
                                detailCommand.Parameters.AddWithValue("@ProductId", detail.ProductId);
                                detailCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                                detailCommand.Parameters.AddWithValue("@Price", detail.Price);
                                detailCommand.ExecuteNonQuery();
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

    }
}
