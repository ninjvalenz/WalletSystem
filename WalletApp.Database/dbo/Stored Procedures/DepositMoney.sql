
CREATE PROCEDURE [dbo].[DepositMoney]
(
   @AccountNumber bigInt,
   @Amount decimal(18,8)
)
AS
BEGIN
   
    SET NOCOUNT ON

	DECLARE @TotalBalance decimal(18,8) = 0

	EXEC dbo.CheckBalance @AccountNumber, @TotalBalance OUTPUT

	INSERT INTO UserWalletTransaction
		(
			UserWalletAccountNumber,
			FromToAccountNumber,
			TransactionTypeId,
			Amount,
			TransactionDate,
			EndBalance
		)
		SELECT @AccountNumber,
		       null,
			   1,
			   @Amount,
			   GETDATE(),
			   COALESCE(@TotalBalance, 0) + @Amount
   
END