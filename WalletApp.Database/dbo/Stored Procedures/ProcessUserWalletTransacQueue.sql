
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