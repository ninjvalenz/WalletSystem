
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