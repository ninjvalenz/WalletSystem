
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

	EXEC dbo.CheckBalance @AccountNumber, @TotalBalanceFromAcct OUTPUT
	EXEC dbo.CheckBalance @ToAccountNumber, @TotalBalanceToAcct OUTPUT

	IF(@TotalBalanceFromAcct >= @Amount)
	BEGIN
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
				   COALESCE(@TotalBalanceFromAcct, 0) - @Amount

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
				   COALESCE(@TotalBalanceToAcct, 0) + @Amount
	END

	SELECT @HasSufficientBal as IsSuccess
   
END