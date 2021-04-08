select FUNC.req_org
	,FUNC.pr_no
from tb_pr FUNC
where 1=1
	and (FUNC.req_org = 'ORG001' 
			or FUNC.req_org = 'ORG002' 
			or FUNC.req_org = 'ORG003')