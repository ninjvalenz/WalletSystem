
CREATE PROCEDURE [dbo].[WithdrawMoney]
(
   @AccountNumber bigInt,
   @Amount decimal(18,8)
)
AS
BEGIN
  
    SET NOCOUNT ON

	DECLARE @HasSufficientBal bit = 0
	DECLARE @TotalBalance decimal(18,0) = 0
	DECLARE @EndBalance decimal(18,0) = 0 

	EXEC dbo.CheckBalance @AccountNumber, @TotalBalance OUTPUT

	IF (@TotalBalance >= @Amount)
	BEGIN
		
		SET @HasSufficientBal = 1
		SET @EndBalance = COALESCE(@TotalBalance, 0) - @Amount

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
			   2, --Transactiontype withdraw
			   @Amount * -1,
			   GETDATE(),
			   @EndBalance

		UPDATE UserWalletAccount
		SET Balance = @EndBalance
		WHERE AccountNumber = @AccountNumber

	END

	SELECT @HasSufficientBal as IsSuccess, @EndBalance as EndBalance
   
END