select 
		   a.po_no,
		   a.pr_no, 
		   a.po_date,
		   a.supplier_code,
		   a.update_by [changed_by],
		   a.update_on [changed_dt]
		   
	from tb_po as a 
where 1=1
	and a.po_no = @ApprovalId
