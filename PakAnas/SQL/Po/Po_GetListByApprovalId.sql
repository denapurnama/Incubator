
SELECT af.po_no
	  ,af.pr_no
	  ,af.SEQ
	  ,af.request_qty
	  ,af.unit_price
FROM 
	[tb_po_dtl] af
WHERE 
	1 = 1
    AND af.po_no = @ApprovalId