
CREATE PROCEDURE [dbo].[DepositMoney]
(
   @AccountNumber bigInt,
   @ToAccountNumber bigInt = null,
   @Amount decimal(18,8),
   @TransactionTypeId int
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
		       @ToAccountNumber,
			   @TransactionTypeId,
			   @Amount,
			   GETDATE(),
			   @EndBalance

	UPDATE UserWalletAccount
	SET Balance = @EndBalance
	WHERE AccountNumber = @AccountNumber

	SELECT @EndBalance as EndBalance
   
END