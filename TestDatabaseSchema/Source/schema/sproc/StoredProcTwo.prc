IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoredProcTwo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[StoredProcTwo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[StoredProcTwo] 
	@ParamOne int
AS
BEGIN
	EXEC StoredProcOne @ParamOne
END


GO

