
CREATE PROCEDURE [dbo].[RegisterUser]
(
    @Login varchar(100), @Password varchar(50)
)
AS
BEGIN
  
    SET NOCOUNT ON

	DECLARE @UserSecuID uniqueidentifier = null

	IF NOT EXISTS(SELECT Id FROM UserSecurity WHERE [Login] = @Login)
	BEGIN

	    SET @UserSecuID = NEWID()

		INSERT INTO UserSecurity(Id, [Login], [Password], CreatedDate)
		SELECT @UserSecuID, @Login, @Password, GETDATE()

	END

	SELECT @UserSecuID as UserSecurityID
   
END