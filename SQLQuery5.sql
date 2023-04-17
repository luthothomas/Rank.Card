USE [C:\USERS\LUSINDISOT\SOURCE\REPOS\RANK.CARD\APP_DATA\DATABASE1.MDF]
GO

DECLARE	@return_value Int

EXEC	@return_value = [dbo].[sp_GetCardList]
		@Id = null,
		@CardNumber = ''

SELECT	@return_value as 'Return Value'

GO
