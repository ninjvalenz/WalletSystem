USE [WalletApp]
GO
/****** Object:  StoredProcedure [dbo].[WithdrawMoney]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[WithdrawMoney]
GO
/****** Object:  StoredProcedure [dbo].[ViewTransactionHistoryByRange]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[ViewTransactionHistoryByRange]
GO
/****** Object:  StoredProcedure [dbo].[ViewTransactionHistoryAll]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[ViewTransactionHistoryAll]
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserWalletTransactionQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[UpdateUserWalletTransactionQueue]
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserSecurityQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[UpdateUserSecurityQueue]
GO
/****** Object:  StoredProcedure [dbo].[TransferMoney]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[TransferMoney]
GO
/****** Object:  StoredProcedure [dbo].[Transfer_Deduct]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[Transfer_Deduct]
GO
/****** Object:  StoredProcedure [dbo].[RegisterWallet]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[RegisterWallet]
GO
/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[RegisterUser]
GO
/****** Object:  StoredProcedure [dbo].[ProcessUserWalletTransacQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[ProcessUserWalletTransacQueue]
GO
/****** Object:  StoredProcedure [dbo].[ProcessUserSecurityQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[ProcessUserSecurityQueue]
GO
/****** Object:  StoredProcedure [dbo].[InsertToUserSecurityQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[InsertToUserSecurityQueue]
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoUserWalletTransacQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[InsertIntoUserWalletTransacQueue]
GO
/****** Object:  StoredProcedure [dbo].[DepositMoney]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[DepositMoney]
GO
/****** Object:  StoredProcedure [dbo].[CheckBalance]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[CheckBalance]
GO
/****** Object:  StoredProcedure [dbo].[AuthenticateLogin]    Script Date: 8/3/2021 6:02:44 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[AuthenticateLogin]
GO
/****** Object:  StoredProcedure [dbo].[AuthenticateLogin]    Script Date: 8/3/2021 6:02:44 PM ******/
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
/****** Object:  StoredProcedure [dbo].[CheckBalance]    Script Date: 8/3/2021 6:02:44 PM ******/
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
/****** Object:  StoredProcedure [dbo].[DepositMoney]    Script Date: 8/3/2021 6:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
GO
/****** Object:  StoredProcedure [dbo].[InsertIntoUserWalletTransacQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertIntoUserWalletTransacQueue] 
	@UserWalletAccountNumber bigint,
    @FromToAccountNumber bigint = NULL,
	@TransactionTypeId int,
	@Amount decimal(18, 8)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @GeneratedTable TABLE(QueueId bigInt)
	INSERT INTO UserWalletTransactionQueue(
		QueueId,
		UserWalletAccountNumber,
		FromToAccountNumber,
		TransactionTypeId,
		Amount,
		CreatedDate,
		QueueStatusId)
	OUTPUT inserted.QueueId INTO @GeneratedTable
		SELECT REPLACE(STR(CAST(CAST(NEWID() AS binary(5)) AS bigint),12),0,0),
			   @UserWalletAccountNumber,
		       @FromToAccountNumber,
			   @TransactionTypeId,
			   @Amount, 
			   GETDATE(),
			   1

	    SELECT CAST(QueueId as bigint) as QueueId FROM @GeneratedTable 
END
GO
/****** Object:  StoredProcedure [dbo].[InsertToUserSecurityQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertToUserSecurityQueue] 
	@Login varchar(100), @Password varchar(50)
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @GeneratedTable TABLE(QueueId bigInt)
	INSERT INTO UserSecurityQueue(QueueId, [Login], [Password], CreatedDate, QueueStatusId)
	OUTPUT inserted.QueueId INTO @GeneratedTable
	SELECT REPLACE(STR(CAST(CAST(NEWID() AS binary(5)) AS bigint),12),0,0), @Login, @Password, GETDATE(), 1

	SELECT CAST(QueueId as bigint) as QueueId FROM @GeneratedTable 

	
   
END
GO
/****** Object:  StoredProcedure [dbo].[ProcessUserSecurityQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ProcessUserSecurityQueue]

AS
BEGIN
  
    SET NOCOUNT ON

	DECLARE @QueueTbl TABLE(queueId bigint)

	UPDATE TOP (100) UserSecurityQueue   
	SET QueueStatusId = 2  
	OUTPUT INSERTED.QueueId
	INTO @QueueTbl
	WHERE QueueStatusId = 1
	
	SELECT tbl.queueId, 
		   [Login],
		   [Password]
	FROM  @QueueTbl tbl
	INNER JOIN UserSecurityQueue ON tbl.queueId = UserSecurityQueue.QueueId
	ORDER BY UserSecurityQueue.CreatedDate
   
END
GO
/****** Object:  StoredProcedure [dbo].[ProcessUserWalletTransacQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ProcessUserWalletTransacQueue] 
	
AS
BEGIN
	
	SET NOCOUNT ON
	DECLARE @QueueTbl TABLE(queueId bigint)

	UPDATE TOP (100) UserWalletTransactionQueue   
	SET QueueStatusId = 2  
	OUTPUT INSERTED.QueueId
	INTO @QueueTbl
	WHERE QueueStatusId = 1
	
	SELECT tbl.queueId, 
		   UserWalletAccountNumber,
		   FromToAccountNumber,
		   TransactionTypeId,
		   Amount
	FROM  @QueueTbl tbl
	INNER JOIN UserWalletTransactionQueue ON tbl.queueId = UserWalletTransactionQueue.QueueId
	ORDER BY UserWalletTransactionQueue.CreatedDate

END
GO
/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 8/3/2021 6:02:44 PM ******/
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
/****** Object:  StoredProcedure [dbo].[RegisterWallet]    Script Date: 8/3/2021 6:02:44 PM ******/
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
/****** Object:  StoredProcedure [dbo].[Transfer_Deduct]    Script Date: 8/3/2021 6:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
	    SET @IsExisting = 1
		EXEC dbo.CheckBalance @AccountNumber, @TotalBalanceFromAcct OUTPUT

		IF(@TotalBalanceFromAcct >= @Amount)
		BEGIN
		
			DECLARE @ToUserEndBalance decimal(18,8) = 0

			SET @FromUserEndBalance = COALESCE(@TotalBalanceFromAcct, 0) - @Amount
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

	
		END
	END
	SELECT @HasSufficientBal as IsSuccess, @IsExisting as IsExisting, @FromUserEndBalance as EndBalance
END
GO
/****** Object:  StoredProcedure [dbo].[TransferMoney]    Script Date: 8/3/2021 6:02:44 PM ******/
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
/****** Object:  StoredProcedure [dbo].[UpdateUserSecurityQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateUserSecurityQueue] 
	@QueueId bigint, 
	@QueueStatusId int, 
	@Message varchar(max) = '',
	@RegisteredUserId uniqueidentifier = NULL,
	@RegisteredWalletAcctNo bigInt = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	UPDATE UserSecurityQueue
	SET QueueStatusId = @QueueStatusId,
	    [Message] = @Message,
		RegisteredUserId = @RegisteredUserId,
		RegisteredWalletAcctNo = @RegisteredWalletAcctNo
	WHERE QueueId = @QueueId
  
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateUserWalletTransactionQueue]    Script Date: 8/3/2021 6:02:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[UpdateUserWalletTransactionQueue] 
	@QueueId bigint, 
	@QueueStatusId int, 
	@Message varchar(max) = ''
	
AS
BEGIN
	
	SET NOCOUNT ON;

	UPDATE UserWalletTransactionQueue
	SET QueueStatusId = @QueueStatusId,
	    [Message] = @Message
	WHERE QueueId = @QueueId
  
END
GO
/****** Object:  StoredProcedure [dbo].[ViewTransactionHistoryAll]    Script Date: 8/3/2021 6:02:44 PM ******/
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
/****** Object:  StoredProcedure [dbo].[ViewTransactionHistoryByRange]    Script Date: 8/3/2021 6:02:44 PM ******/
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
/****** Object:  StoredProcedure [dbo].[WithdrawMoney]    Script Date: 8/3/2021 6:02:44 PM ******/
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
