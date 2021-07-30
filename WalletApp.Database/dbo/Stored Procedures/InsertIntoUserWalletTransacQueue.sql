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