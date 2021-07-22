# WalletSystem

https://localhost:44369/api/user/register

request body:
{
    "Login": "jvalenzona",
    "Password": "P@ssw0rd"
}

success payload:
{
    "accountNumber": 151007544471,
    "message": null,
    "infoMessage": null,
    "isSuccess": true
}

https://localhost:44369/api/wallet/deposit

request body:
{
    "AccountNumber": 151007544471,
    "Amount": 2500
}

success payload:
{
    "message": null,
    "infoMessage": "Success! End balance is now 2500.00000000",
    "isSuccess": true
}

https://localhost:44369/api/wallet/withdraw

request body:
{
    "AccountNumber": 151007544471,
    "Amount": 100
}

success payload:
{
    "message": null,
    "infoMessage": "Success! End balance is now 2600.00000000",
    "isSuccess": true
}

https://localhost:44369/api/wallet/transfer

request body:
{
    "AccountNumber": 151007544471,
    "ToAccountNumber": 588737369455,
    "Amount": 100
}

success payload:
{
    "message": null,
    "infoMessage": "Success! End balance is now 2300.00000000",
    "isSuccess": true
}

https://localhost:44369/api/wallet/historyall

request body:
{
    "AccountNumber": 151007544471,
    "Offset": 0
}


success payload:
{
    "transactionHistoryViewModels": [
        {
            "transactionType": "Deposit",
            "transactionAmount": 2500.00000000,
            "fromToAccountNumber": 0,
            "transactionDate": "7/22/2021",
            "transactionEndBalance": 2500.00000000,
            "message": null,
            "infoMessage": null,
            "isSuccess": true
        },
        {
            "transactionType": "Withdraw",
            "transactionAmount": -100.00000000,
            "fromToAccountNumber": 0,
            "transactionDate": "7/22/2021",
            "transactionEndBalance": 2400.00000000,
            "message": null,
            "infoMessage": null,
            "isSuccess": true
        },
        {
            "transactionType": "Transfer",
            "transactionAmount": -100.00000000,
            "fromToAccountNumber": 588737369455,
            "transactionDate": "7/22/2021",
            "transactionEndBalance": 2300.00000000,
            "message": null,
            "infoMessage": null,
            "isSuccess": true
        }
    ],
    "message": null,
    "infoMessage": null,
    "isSuccess": true
}


https://localhost:44369/api/wallet/historybyrange

request body:
{
    "AccountNumber": 191117534371,
    "FromDate": "2021-07-21",
    "ToDate": "2021-07-22"
}

success payload:
{
    "transactionHistoryViewModels": [
        {
            "transactionType": "Deposit",
            "transactionAmount": 500.00000000,
            "fromToAccountNumber": 0,
            "transactionDate": "7/21/2021",
            "transactionEndBalance": 500.00000000,
            "message": null,
            "infoMessage": null,
            "isSuccess": true
        },
        {
            "transactionType": "Transfer",
            "transactionAmount": -100.00000000,
            "fromToAccountNumber": 688787369955,
            "transactionDate": "7/21/2021",
            "transactionEndBalance": 400.00000000,
            "message": null,
            "infoMessage": null,
            "isSuccess": true
        },
        {
            "transactionType": "Withdraw",
            "transactionAmount": -200.00000000,
            "fromToAccountNumber": 0,
            "transactionDate": "7/22/2021",
            "transactionEndBalance": 200.00000000,
            "message": null,
            "infoMessage": null,
            "isSuccess": true
        },
        
    ],
    "message": null,
    "infoMessage": null,
    "isSuccess": true
}

success payload for end of line:
{
    "transactionHistoryViewModels": null,
    "message": null,
    "infoMessage": "No more records to be display",
    "isSuccess": true
}