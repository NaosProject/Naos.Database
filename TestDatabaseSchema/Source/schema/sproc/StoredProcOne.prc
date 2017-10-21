IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoredProcOne]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[StoredProcOne]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE StoredProcOne 
	@ParamOne int
AS
BEGIN
	SELECT @ParamOne
END


GO

