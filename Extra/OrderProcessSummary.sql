select COUNT(ord.OrderId) OrderProcessed,
    Sum(case when ord.StatusCode <> 'REJECTED' then 1 else 0 end) 'AcceptedOrder',
    Sum(case when ord.StatusCode = 'REJECTED' then 1 else 0 end) 'RejectedOrder'
    from 
    NewOrders ord

    select ord.OrderId,count(dtl.DetailId) Items,ord.TotalAmount,ord.StatusCode from NewOrders ord
    inner join NewOrderDetails dtl on dtl.OrderId = ord.OrderId 
    and dtl.StatusCode <> 'REJECTED' 
    where ord.StatusCode <> 'REJECTED' 
    group by ord.OrderId,ord.TotalAmount,ord.StatusCode

    select ord.OrderId,count(dtl.DetailId) Items,ord.TotalAmount,ord.StatusCode from NewOrders ord
    inner join NewOrderDetails dtl on dtl.OrderId = ord.OrderId 
    and dtl.StatusCode = 'REJECTED' 
    where ord.StatusCode = 'REJECTED' 
    group by ord.OrderId,ord.TotalAmount,ord.StatusCode
