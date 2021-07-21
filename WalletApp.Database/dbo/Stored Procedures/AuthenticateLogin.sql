
CREATE PROCEDURE [dbo].[AuthenticateLogin]
(
   @Login varchar(100),
   @Password varchar(50)
)
AS
BEGIN
 
    SET NOCOUNT ON

	SELECT UserSecu.Id, UserSecu.[Login], AccountNumber
	FROM UserSecurity UserSecu
	INNER JOIN UserWalletAccount Wallet ON UserSecu.Id = Wallet.UserSecurityId
	WHERE [Login] = @Login AND [Password] COLLATE Latin1_General_CS_AS = @Password
    
END