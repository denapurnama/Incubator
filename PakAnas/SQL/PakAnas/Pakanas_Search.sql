select * from 
(
	select ROW_NUMBER() over(order by org_code asc) [r]
	,org_code
	,org_name
	,create_by
	,create_on
	,update_by
	,update_on
	from
		tb_org
		where 1=1
	and ((@org_code is null or @org_code = '') or  (org_code LIKE '%' + @org_code+ '%'))
	and ((@org_name is null or @org_name = '') or  (org_name LIKE '%' + @org_name+ '%'))

			
)a
where 1=1 and a.r between @recordPerPage and @currentpage