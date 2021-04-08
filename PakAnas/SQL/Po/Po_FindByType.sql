SELECT po_no
    ,pr_no
	,po_date
    ,supplier_code
    ,create_by
    ,create_on
FROM dbo.tb_po
where supplier_code = @SystemType
