
CREATE PROCEDURE [dbo].[ViewTransactionHistoryAll]
(
    @AccountNumber BigInt,
	@Offset int
)
AS
BEGIN
  
    SET NOCOUNT ON

	SELECT TransacType.Type as TransactionType,
	       Amount,
		   FromToAccountNumber,
		   CAST(TransactionDate AS Date ) as TransactionDate,
		   EndBalance
	FROM UserWalletTransaction Transac
	INNER JOIN TransactionType TransacType ON Transac.TransactionTypeId = TransacType.Id
	WHERE UserWalletAccountNumber = @AccountNumber
	ORDER BY TransactionDate desc
	OFFSET @Offset ROWS
	FETCH NEXT 5 ROWS ONLY;

    
END