select count(1)
from tb_po as a
where 1=1
		and (nullif(@Po,'') is null or a.po_no = @Po)
		and (nullif(@Pr,'') is null or a.pr_no = @Pr)
		and (nullif(@Supplier,'') is null or a.supplier_code like '%'+@Supplier+'%')
		and (nullif(@PoDate,'') is null or a.po_date = convert(date, @PoDate, 120))
