# Transaction API Test

## Test Request (Based on Task Example)

### Valid Request Example
```json
{
  "partnerkey": "FG-00001",
"partnerrefno": "FG-000001",
  "partnerpassword": "RkFLRVBBU1NXT1JEMTIzNA==",
  "totalamount": 1000,
  "items": [
    {
      "partneritemref": "ITEM001",
      "name": "Test Item",
   "qty": 2,
      "unitprice": 500
    }
  ],
  "timestamp": "2024-08-15T02:11:22.0000000Z",
  "sig": "MDE3ZTBkODg4ZDNhYzU0ZDBlZWRmNmU2NmUyOWRhZWU4Y2M1NzQ1OTIzZGRjYTc1ZGNjOTkwYzg2MWJlMDExMw=="
}
```

### Expected Response
```json
{
  "result": 1,
  "totalamount": 1000,
  "totaldiscount": 0,
  "finalamount": 1000
}
```

## Testing Endpoints

POST `https://localhost:7xxx/api/submittrxmessage`

## Test Cases to Verify

1. ? Valid transaction with correct signature
2. ? Invalid partner key
3. ? Invalid partner password
4. ? Invalid signature
5. ? Negative total amount
6. ? Item quantity > 5
7. ? Empty required fields