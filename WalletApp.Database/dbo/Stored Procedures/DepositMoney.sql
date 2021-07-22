
CREATE PROCEDURE [dbo].[DepositMoney]
(
   @AccountNumber bigInt,
   @Amount decimal(18,8)
)
AS
BEGIN
   
    SET NOCOUNT ON

	DECLARE @TotalBalance decimal(18,8) = 0
	DECLARE @EndBalance decimal(18,8) = 0

	EXEC dbo.CheckBalance @AccountNumber, @TotalBalance OUTPUT

	SET @EndBalance = COALESCE(@TotalBalance, 0) + @Amount

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
			   @EndBalance

	UPDATE UserWalletAccount
	SET Balance = @EndBalance
	WHERE AccountNumber = @AccountNumber

	SELECT @EndBalance as EndBalance
   
END