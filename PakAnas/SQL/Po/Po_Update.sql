﻿exec	@RetVal						= [sp_ApprovalUpdate]
		@@ro_v_err_mesg				= @ErrMesg output,
		@@ri_v_user_id				= @UserId,
		@@ri_v_approval_id			= @ApprovalId,
		@@ri_d_date_from			= @DateFrom,
		@@ri_d_date_to				= @DateTo,
		@@ri_v_function_id			= @FunctionId,
		@@ri_v_approv_name			= @ApprovName,
		@@ri_t_approval_dtl			= @TableOfApprovalFlow