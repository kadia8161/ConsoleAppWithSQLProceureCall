Create PROCEDURE [dbo].[prcProcessOrder]
AS
BEGIN
     
    create table #tmpRejectedOrd (OrderId int)
    create table #tmpRejectedOrdDtl (DetailId int)

    insert into #tmpRejectedOrd  
    select OrderId     
    from NewOrders ord 
    where 
    (
    (not exists(select 1 from Customers C where C.CustomerId = ord.CustomerId and c.StatusCode = 'ACTIVE'))
    OR
    (Len(ord.Zipcode) <> 5)
    OR
    (lower(ord.Zipcode) COLLATE Latin1_General_CS_AS <> UPPER(ord.Zipcode) COLLATE Latin1_General_CS_AS)
    )

    INSERT into #tmpRejectedOrdDtl 
    select dtl.DetailId from NewOrderDetails dtl
    inner join NewOrders as ord
    on ord.OrderId = dtl.OrderId
    inner join Customers c on c.CustomerId = ord.CustomerId 
    and ( dtl.Quantity = 0 OR dtl.Quantity < c.MinProdQuantity Or dtl.Price = 0)
    union 
    select dtl.DetailId from NewOrderDetails dtl
    inner join #tmpRejectedOrd ro on ro.OrderId = dtl.OrderId 
    
    update NewOrders
    set StatusCode = 'REJECTED',
    TotalAmount = 0,
    StatusDescription = 'Order Rejected' ,
    ModifiedDate = getdate(),
    ModifiedBy = 'ADMIN'
    from #tmpRejectedOrd t
    where t.OrderId = NewOrders.OrderId

    update NewOrderDetails 
    set StatusCode = 'REJECTED',
    StatusDescription = 'Order Rejected' ,
    ModifiedDate = getdate(),
    ModifiedBy = 'ADMIN'
    from #tmpRejectedOrdDtl rd 
    where rd.DetailId = NewOrderDetails.DetailId 

    UPDATE NewOrders
    set TotalAmount = DetailOrder.TotalAmount 
    from 
    (select OrderId,Sum(dtl.Price) as TotalAmount
    from NewOrderDetails dtl 
    where dtl.StatusCode <> 'REJECTED'
    group by dtl.OrderId
    ) DetailOrder      
    where DetailOrder.OrderId = NewOrders.OrderId 
    and NewOrders.StatusCode <> 'REJECTED'  
    
    
    drop table #tmpRejectedOrd
    drop table #tmpRejectedOrdDtl
end


GO
