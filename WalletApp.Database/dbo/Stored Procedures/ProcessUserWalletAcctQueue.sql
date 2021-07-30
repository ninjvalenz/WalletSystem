
CREATE PROCEDURE [dbo].[ProcessUserWalletAcctQueue] 
	
AS
BEGIN
	
	SET NOCOUNT ON
	DECLARE @QueueTbl TABLE(queueId bigint)

	UPDATE TOP (100) UserWalletAccountQueue   
	SET QueueStatusId = 2  
	OUTPUT INSERTED.QueueId
	INTO @QueueTbl
	WHERE QueueStatusId = 1
	
	SELECT tbl.queueId, 
		   UserSecurityId
	FROM  @QueueTbl tbl
	INNER JOIN UserWalletAccountQueue ON tbl.queueId = UserWalletAccountQueue.QueueId
	

END