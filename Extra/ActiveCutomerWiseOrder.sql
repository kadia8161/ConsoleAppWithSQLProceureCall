select c.CustomerId,c.CustomerName,count(ord.OrderId) CustomerWiseNoOfOrder,count(dtl.DetailId) CustomerWiseTotlaItem,Sum(ord.TotalAmount) as  CustomerWiseTotal
from Customers c
inner join NewOrders ord on ord.CustomerId = c.CustomerId 
inner join NewOrderDetails dtl on dtl.OrderId = ord.OrderId  
where C.StatusCode = 'ACTIVE'
and ord.StatusCode = 'NEW'
and dtl.StatusCode <> 'REJECTED'
group by c.CustomerId,c.CustomerName
