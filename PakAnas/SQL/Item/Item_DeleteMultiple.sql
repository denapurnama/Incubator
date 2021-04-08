execute @RetVal = [sp_ItemDeleteMultiple]
		@@ro_v_err_mesg				= @ErrMesg output,
		@@ri_t_item_code				= @TblOfVarchar