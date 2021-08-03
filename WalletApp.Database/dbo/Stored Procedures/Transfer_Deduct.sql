
CREATE PROCEDURE [dbo].[Transfer_Deduct] 
(
   @AccountNumber bigInt,
   @ToAccountNumber bigInt,
   @Amount decimal(18,8)
)
	
AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @HasSufficientBal bit = 0
	DECLARE @IsExisting bit = 0
	DECLARE @TotalBalanceFromAcct decimal(18,8) = 0
	DECLARE @FromUserEndBalance decimal(18,8) = 0

	IF EXISTS(SELECT UserSecurityId FROM UserWalletAccount WHERE AccountNumber = @AccountNumber) AND
	  EXISTS(SELECT UserSecurityId FROM UserWalletAccount WHERE AccountNumber = @ToAccountNumber)
	BEGIN
		EXEC dbo.CheckBalance @AccountNumber, @TotalBalanceFromAcct OUTPUT

		IF(@TotalBalanceFromAcct >= @Amount)
		BEGIN
		
			DECLARE @ToUserEndBalance decimal(18,8) = 0

			SET @FromUserEndBalance = COALESCE(@TotalBalanceFromAcct, 0) - @Amount
			SET @HasSufficientBal = 1
			SET @IsExisting = 1

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

	
		END
	END
	SELECT @HasSufficientBal as IsSuccess, @IsExisting as IsExisting, @FromUserEndBalance as EndBalance
END