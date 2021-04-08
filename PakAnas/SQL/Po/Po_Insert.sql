exec	@RetVal						= [sp_PoInsert]
		@@ro_v_err_mesg				= @ErrMesg output,
		@@ri_v_user_id				= @UserId,
		@@ri_v_po_no				= @Po,
		@@ri_v_pr_no				= @Pr,
		@@ri_d_po_date				= @PoDate,
		@@ri_v_supplier_code		= @Supplier,
		@@ri_t_po_dtl				= @TableApprovalFlow