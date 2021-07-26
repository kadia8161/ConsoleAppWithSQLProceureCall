 
select Year(b.RunningDate) ForYear,datename(mm,b.RunningDate) ForMonth,
sum(case when c.StatusCode = 'ACTIVE' then 1 else 0 end) ActiveCustomer,
sum(case when c.StatusCode <> 'ACTIVE' then 1 else 0 end) InActiveCustomer,
isnull(Count(ord.OrderId),0) TotalOrder,
isnull(Count(dtl.Price),0) TotalAmount 
from 
(
select Min(CreateDate) StartDate,Max(CreateDate) EndDate from 
Customers
) a
cross apply
(
select rn,MONTH(DATEADD(MM,(tmp.rn-1),a.StartDate)) as RunningMonth ,
Year(DATEADD(MM,(tmp.rn-1),a.StartDate)) as RunningYear,
convert(datetime,('01/' + convert(varchar(2),MONTH(DATEADD(MM,(tmp.rn-1),a.StartDate))) + '/' + convert(varchar(4),Year(DATEADD(MM,(tmp.rn-1),a.StartDate)))),103) RunningDate
from  (select ROW_NUMBER() over (order by object_id) rn from sys.columns)
tmp 
where a.EndDate >= DATEADD(MM,(tmp.rn-1),a.StartDate)
) b 
inner join 
 Customers c
on 
--Month(c.CreateDate) <= b.RunningMonth and Year(c.CreateDate) <= b.RunningYear 
c.CreateDate <= b.RunningDate  
left join NewOrders ord on ord.CustomerId = c.CustomerId
and Month(ord.SubmitDate) = b.RunningMonth and Year(ord.SubmitDate) = b.RunningYear  and ord.StatusCode <> 'REJECTED'
left join NewOrderDetails dtl on dtl.OrderId = ord.OrderId  and dtl.StatusCode <> 'REJECTED'
group by b.RunningDate
Order by b.RunningDate

 
