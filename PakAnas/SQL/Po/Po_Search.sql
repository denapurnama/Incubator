select * from (
		SELECT
			ROW_NUMBER() OVER (ORDER BY a.[po_no] DESC) AS ROW_NO	
			,a.po_no
			,a.pr_no
			,a.po_date
			,dbo.fn_FormatDate(a.po_date) [PO_DATE_STR]
			,a.supplier_code
		    ,a.create_by [created_by]
		    ,a.create_on [created_dt]
		    ,dbo.fn_FormatDateTime(a.create_on) [created_dt_str]
		    ,a.update_by [changed_by]
		    ,a.update_on [changed_dt]
		    ,dbo.fn_FormatDateTime(a.update_on) [changed_dt_str]
	FROM [dbo].[tb_po] a
	where 1=1
		and (nullif(@Po,'') is null or a.po_no = @Po)
		and (nullif(@Pr,'') is null or a.pr_no = @Pr)
		and (nullif(@Supplier,'') is null or a.supplier_code like '%'+@Supplier+'%')
		and (nullif(@PoDate,'') is null or a.po_date = convert(date, @PoDate, 120))
			
		) tb
where 1 = 1
	and [ROW_NO] between cast(@RowStart as varchar) and cast(@RowEnd as varchar)
order by [ROW_NO] DESC