
CREATE PROCEDURE [dbo].[TransferMoney]
(
   @AccountNumber bigInt,
   @ToAccountNumber bigInt,
   @Amount decimal(18,8)
)
AS
BEGIN
   
    SET NOCOUNT ON

	DECLARE @HasSufficientBal bit = 0
	DECLARE @TotalBalanceFromAcct decimal(18,8) = 0
	DECLARE @TotalBalanceToAcct decimal(18,8) = 0
	DECLARE @FromUserEndBalance decimal(18,8) = 0

	EXEC dbo.CheckBalance @AccountNumber, @TotalBalanceFromAcct OUTPUT
	EXEC dbo.CheckBalance @ToAccountNumber, @TotalBalanceToAcct OUTPUT

	IF(@TotalBalanceFromAcct >= @Amount)
	BEGIN
		
		DECLARE @ToUserEndBalance decimal(18,8) = 0

		SET @FromUserEndBalance = COALESCE(@TotalBalanceFromAcct, 0) - @Amount
		SET @ToUserEndBalance = COALESCE(@TotalBalanceToAcct, 0) + @Amount
		SET @HasSufficientBal = 1
		--deduct record from current account
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
				   3,
				   @Amount * -1,
				   GETDATE(),
				   @FromUserEndBalance

		UPDATE UserWalletAccount
		SET Balance = @FromUserEndBalance
		WHERE AccountNumber = @AccountNumber

		--addition record to destination account
		INSERT INTO UserWalletTransaction
			(
				UserWalletAccountNumber,
				FromToAccountNumber,
				TransactionTypeId,
				Amount,
				TransactionDate,
				EndBalance
			)
			SELECT @ToAccountNumber,
				   @AccountNumber,
				   1,
				   @Amount,
				   GETDATE(),
				   @ToUserEndBalance

		UPDATE UserWalletAccount
		SET Balance = @ToUserEndBalance
		WHERE AccountNumber = @ToAccountNumber

	END

	SELECT @HasSufficientBal as IsSuccess, @FromUserEndBalance as EndBalance
   
END