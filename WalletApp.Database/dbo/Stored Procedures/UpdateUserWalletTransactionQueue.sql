
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