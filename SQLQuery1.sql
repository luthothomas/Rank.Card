CREATE Procedure sp_GetCardById
(@ID int)
AS 
BEGIN
	SELECT [Id],CardHolderName,CardNumber,CardType,DateCreated,DateExpiryYear,DateExpiryMonth
	FROM CARD
	WHERE ID = @ID
END;