	
CREATE PROCEDURE [dbo].[CheckBalance]
(
   @AccountNumber BigInt,
   @TotalBalance decimal(18,8) OUTPUT
)
AS
BEGIN
   
    SET NOCOUNT ON

    SELECT @TotalBalance = SUM(Amount) 
	FROM UserWalletTransaction
	WHERE UserWalletAccountNumber = @AccountNumber
    
END