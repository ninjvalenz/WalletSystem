
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