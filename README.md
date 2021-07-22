# WalletSystem

After cloning the code, here are the steps on running the project locally:

1. Create local DB named "WalletApp"
2. Run all the scripts from WalletApp.Database/Initializations. Please take note of the numbering sequence.
3. Set the project WalletApp.Api as the startup project.
4. Run the project. 
5. Using postman (or any other preferred tool), register an account: 
    https://yourlocalhost/api/user/register

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

6. After registering, login using your credentials:
    https://yourlocalhost/api/user/authenticate

    request body:
    {
        "Username": "jvalenzona",
        "Password": "P@ssw0rd"
    }

    success payload:
    {
        "message": "You are now logged in using login name jvalenzona. Wallet account(s): 151007544471",
        "isSuccess": true,
        "token": "A very long string. Copy this and use this to other api calls."
    }

7. Get the "token" received after authenticating. From the postman's Authorization's tab, select "Bearer Token" type and set the token value. 
This is essential for all the api calls.


Below are the other api calls you can use:


1. https://yourlocalhost/api/wallet/deposit

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

2. https://yourlocalhost/api/wallet/withdraw

    request body:
    {
        "AccountNumber": 151007544471,
        "Amount": 100s
    }

    success payload:
    {
        "message": null,
        "infoMessage": "Success! End balance is now 2600.00000000",
        "isSuccess": true
    }

3. https://yourlocalhost/api/wallet/transfer

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

4. https://yourlocalhost/api/wallet/historyall

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


5. https://yourlocalhost/api/wallet/historybyrange

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