
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