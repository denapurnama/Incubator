execute @RetVal = [sp_OrgInsertUpdate]
		@@ro_v_err_mesg				= @ErrMesg output,
		@@ri_v_User_id				= @userId,
		@@ri_v_Org_Code				= @org_code,
		@@ri_v_Org_Name				= @org_name,
		@@ri_v_action				= @ScreenMode