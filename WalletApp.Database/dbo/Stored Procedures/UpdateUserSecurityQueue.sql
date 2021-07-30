
CREATE PROCEDURE [dbo].[UpdateUserSecurityQueue] 
	@QueueId bigint, @QueueStatusId int, @Message varchar(max) = ''
AS
BEGIN
	
	SET NOCOUNT ON;

	UPDATE UserSecurityQueue
	SET QueueStatusId = @QueueStatusId,
	    [Message] = @Message
	WHERE QueueId = @QueueId
  
END