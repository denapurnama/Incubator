execute @RetVal = [sp_orgDeleteMultiple]
		@@ro_v_err_mesg				= @ErrMesg output,
		@@ri_t_org					= @TblOfVarchar