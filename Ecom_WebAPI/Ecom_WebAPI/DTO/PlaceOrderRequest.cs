using Ecom_WebAPI.Models;
using System.Collections.Generic;


namespace Ecom_WebAPI.DTO
{
    public class PlaceOrderRequest
    {
        public Order Order { get; set; }
        public List<OrderDetail> OrderItems { get; set; }
    }
}
