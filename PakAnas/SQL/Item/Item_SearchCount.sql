select count(1)
from tb_item as a
where 1=1
		and (nullif(@ItemCode,'') is null or a.item_code = @ItemCode)
		and (nullif(@ItemName,'') is null or a.item_name like '%'+@ItemName+'%')
		and (nullif(@ItemUm,'') is null or a.item_um like '%' + @ItemUm+'%')