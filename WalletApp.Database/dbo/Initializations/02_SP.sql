USE [WalletApp]
GO
/****** Object:  StoredProcedure [dbo].[WithdrawMoney]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[WithdrawMoney]
GO
/****** Object:  StoredProcedure [dbo].[ViewTransactionHistoryByRange]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[ViewTransactionHistoryByRange]
GO
/****** Object:  StoredProcedure [dbo].[ViewTransactionHistoryAll]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[ViewTransactionHistoryAll]
GO
/****** Object:  StoredProcedure [dbo].[TransferMoney]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[TransferMoney]
GO
/****** Object:  StoredProcedure [dbo].[RegisterWallet]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[RegisterWallet]
GO
/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[RegisterUser]
GO
/****** Object:  StoredProcedure [dbo].[DepositMoney]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[DepositMoney]
GO
/****** Object:  StoredProcedure [dbo].[CheckBalance]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[CheckBalance]
GO
/****** Object:  StoredProcedure [dbo].[AuthenticateLogin]    Script Date: 7/23/2021 1:07:03 AM ******/
DROP PROCEDURE IF EXISTS [dbo].[AuthenticateLogin]
GO
/****** Object:  StoredProcedure [dbo].[AuthenticateLogin]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AuthenticateLogin]
(
   @Login varchar(100),
   @Password varchar(50)
)
AS
BEGIN
 
    SET NOCOUNT ON

	SELECT UserSecu.Id, UserSecu.[Login], AccountNumber
	FROM UserSecurity UserSecu
	INNER JOIN UserWalletAccount Wallet ON UserSecu.Id = Wallet.UserSecurityId
	WHERE [Login] = @Login AND [Password] COLLATE Latin1_General_CS_AS = @Password
    
END
GO
/****** Object:  StoredProcedure [dbo].[CheckBalance]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	
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
GO
/****** Object:  StoredProcedure [dbo].[DepositMoney]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
GO
/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RegisterUser]
(
    @Login varchar(100), @Password varchar(50)
)
AS
BEGIN
  
    SET NOCOUNT ON

	DECLARE @UserSecuID uniqueidentifier = null

	IF NOT EXISTS(SELECT Id FROM UserSecurity WHERE [Login] = @Login)
	BEGIN

	    SET @UserSecuID = NEWID()

		INSERT INTO UserSecurity(Id, [Login], [Password], CreatedDate)
		SELECT @UserSecuID, @Login, @Password, GETDATE()

	END

	SELECT @UserSecuID as UserSecurityID
   
END
GO
/****** Object:  StoredProcedure [dbo].[RegisterWallet]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[RegisterWallet]
(
    @UserSecurityId uniqueidentifier
)
AS
BEGIN
   
    SET NOCOUNT ON

   DECLARE @NewAccountNumber varchar(12) = null
   DECLARE @GeneratedTable TABLE(AccountNumber BigInt)

   INSERT INTO UserWalletAccount
   (
	  AccountNumber,
	  UserSecurityId,
	  RegisteredDate,
	  Balance
	)
	OUTPUT inserted.AccountNumber INTO @GeneratedTable
	SELECT REPLACE(STR(CAST(CAST(NEWID() AS binary(5)) AS bigint),12),0,0),
	       @UserSecurityId,
		   GETDATE(),
		   0

	SELECT AccountNumber FROM @GeneratedTable

	
END
GO
/****** Object:  StoredProcedure [dbo].[TransferMoney]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
GO
/****** Object:  StoredProcedure [dbo].[ViewTransactionHistoryAll]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ViewTransactionHistoryAll]
(
    @AccountNumber BigInt,
	@Offset int
)
AS
BEGIN
  
    SET NOCOUNT ON

	SELECT TransacType.Type as TransactionType,
	       Amount,
		   FromToAccountNumber,
		   CAST(TransactionDate AS Date ) as TransactionDate,
		   EndBalance
	FROM UserWalletTransaction Transac
	INNER JOIN TransactionType TransacType ON Transac.TransactionTypeId = TransacType.Id
	WHERE UserWalletAccountNumber = @AccountNumber
	ORDER BY TransactionDate desc
	OFFSET @Offset ROWS
	FETCH NEXT 5 ROWS ONLY;

    
END
GO
/****** Object:  StoredProcedure [dbo].[ViewTransactionHistoryByRange]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ViewTransactionHistoryByRange]
(
    @AccountNumber BigInt,
	@FromDate datetime,
	@ToDate datetime
)
AS
BEGIN
  
    SET NOCOUNT ON

	SELECT TransacType.Type as TransactionType,
	       Amount,
		   FromToAccountNumber,
		   CAST(TransactionDate AS Date ) as TransactionDate,
		   EndBalance
	FROM UserWalletTransaction Transac
	INNER JOIN TransactionType TransacType ON Transac.TransactionTypeId = TransacType.Id
	WHERE UserWalletAccountNumber = @AccountNumber AND 
	      CAST(TransactionDate AS Date) >= @FromDate AND 
		  CAST(TransactionDate AS Date) <= @ToDate
	ORDER BY TransactionDate desc

    
END
GO
/****** Object:  StoredProcedure [dbo].[WithdrawMoney]    Script Date: 7/23/2021 1:07:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
GO
