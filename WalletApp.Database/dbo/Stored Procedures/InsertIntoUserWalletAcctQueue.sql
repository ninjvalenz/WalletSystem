
CREATE PROCEDURE [dbo].[InsertIntoUserWalletAcctQueue] 
	@UserSecurityId uniqueidentifier 
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO UserWalletAccountQueue(QueueId, UserSecurityId, CreatedDate, QueueStatusId)
	SELECT REPLACE(STR(CAST(CAST(NEWID() AS binary(5)) AS bigint),5),0,0), @UserSecurityId, GETDATE(), 1

	SELECT SCOPE_IDENTITY()
   
END