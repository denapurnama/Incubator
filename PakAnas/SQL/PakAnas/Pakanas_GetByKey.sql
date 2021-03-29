select	org_code,
		org_name

		from tb_org where 1=1 
		and org_code = convert(varchar(max), @org_Code)