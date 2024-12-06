using Dapper;
using ECommerce.Connection;
using ECommerce.Contracts;
using ECommerce.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class OrderRepository: IOrderRepository
{
    private readonly DapperContext context;
    public OrderRepository(DapperContext context)
    {
        this.context = context;
    }

    public async Task<Order> GetMostRecentOrderAsync(Request request)
    {
        using (IDbConnection connection = context.CreateConnection())
        {
            connection.Open();
            try
            {
                //Verify email and customerId match
               var customerQuery = "SELECT CUSTOMERID as CustomerId,FIRSTNAME as FirstName,LASTNAME as LastName,EMAIL as Email FROM CUSTOMERS WHERE CUSTOMERID = @CustomerId AND EMAIL = @Email";
                Customer customer = await connection.QueryFirstOrDefaultAsync<Customer>(customerQuery, new { CustomerId = request.CustomerId, Email = request.MailId });

                if (customer == null)
                {
                    return null;
                }

                var orderQuery = @"SELECT TOP 1 O.ORDERID AS OrderNumber, O.ORDERDATE, O.DELIVERYEXPECTED, 
                             C.HOUSENO + ' ' + C.STREET + ', ' + C.TOWN + ', ' + C.POSTCODE AS DeliveryAddress,o.CONTAINSGIFT
                      FROM ORDERS O
                      JOIN CUSTOMERS C ON O.CUSTOMERID = C.CUSTOMERID
                      WHERE C.CUSTOMERID = @CustomerId
                      ORDER BY O.ORDERDATE DESC";

             

                var order = await connection.QuerySingleOrDefaultAsync<Order>(orderQuery, new { CustomerId = request.CustomerId });

                if (order == null)
                {
                    return new Order { OrderItems = new List<OrderItem>() }; // No orders, return empty order
                }

                // Get order items for the most recent order
                string orderItemsQuery = @"
            SELECT p.PRODUCTNAME AS Product, oi.QUANTITY Quantity,  (oi.PRICE/oi.QUANTITY) as PriceEach
            FROM ORDERITEMS oi
            JOIN PRODUCTS p ON p.PRODUCTID = oi.PRODUCTID
            WHERE oi.ORDERID = @OrderId";


                List<OrderItem> orderItems = (List<OrderItem>)await connection.QueryAsync<OrderItem>(orderItemsQuery, new { OrderId = order.OrderNumber });


                // Replace "Gift" for products in gift orders
              
                    if (order.ContainsGift)
                    {
                        orderItems.ForEach(item => item.Product = "Gift");
                    }
                   
                

                order.OrderItems = orderItems.ToList();
                return order;
            }
            catch (Exception ex)
            {
                throw;  
            }
        }
    }

}
