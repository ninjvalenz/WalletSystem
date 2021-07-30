
CREATE PROCEDURE [dbo].[InsertIntoUserWalletTransacQueue] 
	@UserWalletAccountNumber bigint,
    @FromToAccountNumber bigint,
	@TransactionTypeId int,
	@Amount decimal(18, 8)
AS
BEGIN

	SET NOCOUNT ON;
	
	INSERT INTO UserWalletTransactionQueue(
		QueueId,
		UserWalletAccountNumber,
		FromToAccountNumber,
		TransactionTypeId,
		Amount,
		CreatedDate,
		QueueStatusId)

		SELECT REPLACE(STR(CAST(CAST(NEWID() AS binary(5)) AS bigint),5),0,0),
			   @UserWalletAccountNumber,
		       @FromToAccountNumber,
			   @TransactionTypeId,
			   @Amount, 
			   GETDATE(),
			   1
END