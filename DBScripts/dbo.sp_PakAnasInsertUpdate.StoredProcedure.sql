USE [db_logistic]
GO
/****** Object:  StoredProcedure [dbo].[sp_PakAnasInsertUpdate]    Script Date: 19/03/2021 13:47:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_PakAnasInsertUpdate]
	-- Add the parameters for the stored procedure here
	@ro_v_err_mesg	varchar(2000) output,
	@ri_v_User_id	varchar(30),
	@ri_v_Org_Code	varchar(20),
	@ri_v_Org_Name	varchar(30),
	@ri_v_action	varchar(20)
AS
BEGIN
		SET NOCOUNT ON;

	-- common variable
	declare
		@l_n_process_status	smallint = 0,
		@l_n_return_value smallint = 0,
		@l_v_log_mesg varchar(max)
END
GO
