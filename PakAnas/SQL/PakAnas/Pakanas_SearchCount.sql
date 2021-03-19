select COUNT(1) from tb_org
	where 1=1
	and ((@org_code is null or @org_code = '') or  (org_code LIKE '%' + @org_code+ '%'))
	and ((@org_name is null or @org_name = '') or  (org_name LIKE '%' + @org_name+ '%'))
