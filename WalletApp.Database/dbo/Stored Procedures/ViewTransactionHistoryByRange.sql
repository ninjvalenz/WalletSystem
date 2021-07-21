
CREATE PROCEDURE [dbo].[ViewTransactionHistoryByRange]
(
    @AccountNumber BigInt,
	@FromDate datetime,
	@ToDate datetime
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
	WHERE UserWalletAccountNumber = @AccountNumber AND 
	      CAST(TransactionDate AS Date) >= @FromDate AND 
		  CAST(TransactionDate AS Date) <= @ToDate
	ORDER BY TransactionDate desc

    
END