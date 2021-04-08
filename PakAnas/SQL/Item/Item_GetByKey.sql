select 
		   a.item_code,
		   a.item_name, 
		   a.item_um,
		   a.update_by [changed_by],
		   a.update_on [changed_dt]
		   
	from tb_item as a 
where 1=1
	and a.item_code = @ItemId