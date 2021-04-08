select * from (
	select ROW_NUMBER() OVER(ORDER BY a.item_code) r,
		   a.item_code,
		   a.item_name,
		   a.item_um,
		   a.create_by [created_by],
		   a.create_on [created_dt],
		   dbo.fn_FormatDateTime(a.create_on) [created_dt_str],
		   a.update_by [changed_by],
		   a.update_on [changed_dt],
		   dbo.fn_FormatDateTime(a.update_on) [changed_dt_str]
	from tb_item as a
	where 1=1
		and (nullif(@ItemCode,'') is null or a.item_code = @ItemCode)
		and (nullif(@ItemName,'') is null or a.item_name like '%'+ @ItemName+'%')
		and (nullif(@ItemUm,'') is null or a.item_um like '%' + @ItemUm+'%')

		) x
where x.r between cast(@RowStart as varchar) and cast(@RowEnd as varchar) 