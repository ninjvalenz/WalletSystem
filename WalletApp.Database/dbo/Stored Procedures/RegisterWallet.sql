
CREATE PROCEDURE [dbo].[RegisterWallet]
(
    @UserSecurityId uniqueidentifier
)
AS
BEGIN
   
    SET NOCOUNT ON

   DECLARE @NewAccountNumber varchar(12) = null
   DECLARE @GeneratedTable TABLE(AccountNumber BigInt)

   INSERT INTO UserWalletAccount
   (
	  AccountNumber,
	  UserSecurityId,
	  RegisteredDate,
	  Balance
	)
	OUTPUT inserted.AccountNumber INTO @GeneratedTable
	SELECT REPLACE(STR(CAST(CAST(NEWID() AS binary(5)) AS bigint),12),0,0),
	       @UserSecurityId,
		   GETDATE(),
		   0

	SELECT AccountNumber FROM @GeneratedTable

	
END