## Project Overview

**FourtitudeAspNet** is a transaction processing ASP.NET Core Web API built with .NET 8. It provides secure transaction validation with partner authentication, digital signature verification, and intelligent discount calculation based on complex business rules.

## Key Features

### 1. **Transaction Processing API**
- **Endpoint:** `POST /api/submittrxmessage`
- Validates incoming transaction requests from partners
- Implements multi-layer security with authentication and signature verification
- Returns transaction response with calculated discounts

### 2. **Partner Authentication**
- Base64-encoded password verification
- Partner validation against SQLite database
- Secure credential management

### 3. **Digital Signature Validation**
- **Endpoint:** `POST /api/GetSig`
- SHA-256 based signature generation and validation
- Timestamp-based security (±5 minutes tolerance)
- Prevents replay attacks and tampering

### 4. **Dynamic Discount Calculation**
- **Base Discount Tiers:**
  - < 200 MYR: 0%
  - 200-500 MYR: 5%
  - 501-800 MYR: 7%
  - 801-1200 MYR: 10%
  - > 1200 MYR: 15%

- **Conditional Discounts:**
  - Amount > 500 MYR AND is prime number: +8%
  - Amount > 900 MYR AND ends in 5: +10%
  - Maximum cap: 20%

### 5. **Request/Response Logging Middleware**
- Logs all incoming requests and responses
- Uses Log4Net for comprehensive logging
- Helps with debugging and audit trails

### 6. **Data Persistence**
- Entity Framework Core with SQLite
- Automatic database migration on startup
- Partner and transaction data storage

## API Endpoints

### Transaction Submission
```
POST /api/submittrxmessage
Content-Type: application/json

Request Body:
{
  "partnerkey": "string",
  "partnerrefno": "string",
  "partnerpassword": "base64_encoded_password",
  "totalamount": number (in cents),
  "timestamp": "ISO8601_format",
  "sig": "base64_signature",
  "items": [
    {
      "partneritemref": "string",
      "name": "string",
      "qty": number (max 5),
      "unitprice": number (in cents)
    }
  ]
}

Success Response (200):
{
  "result": 1,
  "totalAmount": number,
  "totalDiscount": number,
  "finalAmount": number
}

Error Response (200):
{
  "result": 0,
  "resultMessage": "error_description"
}
```

### Signature Generation
```
POST /api/GetSig
Content-Type: application/json

Request Body:
{
  "timestamp": "ISO8601_format",
  "partnerkey": "string",
  "partnerrefno": "string",
  "totalamount": number,
  "partnerpassword": "base64_encoded_password"
}

Response:
{
  "signature": "base64_encoded_signature"
}
```

## Building and Running

### Building the project

```bash
dotnet build
```

### Running the project

```bash
dotnet run --launch-profile http
```

The application will start, and you can access the Swagger UI at `http://localhost:5143/swagger`.

### Running with Docker

```bash
docker build -t fourtitudeaspnet .
docker run -p 8080:8080 -p 8081:8081 fourtitudeaspnet
```

## Interview Testing Brief

### Test Scenarios

#### 1. **Successful Transaction with Discount**
- **Objective:** Validate end-to-end transaction processing with discount calculation
- **Test Data:**
  - Amount: 60000 cents (600 MYR)
  - Partner: Valid partner in database
  - Expected: 5% base discount applied
  
#### 2. **Authentication Failure**
- **Objective:** Verify partner validation rejects invalid credentials
- **Test Data:**
  - Invalid partner key or wrong base64 password
  - Expected: "Access Denied!" response

#### 3. **Signature Validation Failure**
- **Objective:** Ensure signature tampering is detected
- **Test Data:**
  - Correct request but modified signature
  - Expected: "Access Denied!" response

#### 4. **Expired Timestamp**
- **Objective:** Test timestamp validation (±5 minutes)
- **Test Data:**
  - Timestamp older than 5 minutes
  - Expected: "Expired." error message

#### 5. **Complex Discount Calculation**
- **Objective:** Test conditional discount rules
- **Test Case A - Prime Number Discount:**
  - Amount: 50300 cents (503 MYR - prime number)
  - Expected: 5% (base) + 8% (prime conditional) = 13% total
  
- **Test Case B - Ends in Five Discount:**
  - Amount: 100015 cents (1000.15 MYR rounds to 1000, ends in 5 when considering integer)
  - Expected: 10% (base) + 10% (ends in 5) = 20% total (capped)

#### 6. **Item Quantity Validation**
- **Objective:** Ensure quantity does not exceed 5 per item
- **Test Data:**
  - Item with qty: 6
  - Expected: "Quantity must not exceed 5" error

#### 7. **Amount Mismatch Validation**
- **Objective:** Verify calculated total matches provided total
- **Test Data:**
  - Items: [qty=2, price=100], [qty=3, price=100]
  - Total provided: 60000 (incorrect)
  - Expected: "Invalid Total Amount." error

#### 8. **Required Fields Validation**
- **Objective:** Test all required field validations
- **Test Data:** Missing one of: partnerkey, partnerpassword, partnerrefno, totalamount, timestamp, sig
- **Expected:** Appropriate error message for each missing field

### Architecture Components

| Component | Purpose | Location |
|-----------|---------|----------|
| **TransactionController** | Handles HTTP requests | `Controllers/TransactionController.cs` |
| **TransactionValidationService** | Orchestrates validation logic | `Services/TransactionValidationService.cs` |
| **PartnerService** | Partner authentication | `Services/PartnerService.cs` |
| **SignatureService** | SHA-256 signature verification | `Services/SignatureService.cs` |
| **DiscountService** | Complex discount calculations | `Services/DiscountService.cs` |
| **FourtitudeDbContext** | EF Core database context | `Data/FourtitudeDbContext.cs` |
| **RequestResponseLoggingMiddleware** | Request/response logging | `Middleware/RequestResponseLoggingMiddleware.cs` |

## Development Conventions

*   **API Endpoints:** API endpoints are implemented as controllers in the `Controllers` directory.
*   **Business Logic:** Services contain business logic and are registered in `Program.cs` with dependency injection.
*   **Models:** Request/response models are in the `Models` directory with JSON serialization attributes.
* **Interfaces:** Service contracts defined in the `Interface` directory for loose coupling.
*   **Configuration:** Application settings are stored in `appsettings.json` and environment-specific files.
*   **Database:** SQLite with Entity Framework Core, migrations stored in `Migrations` directory.
*   **Logging:** Configured with Log4Net in `Configuration/Log4NetConfiguration.cs`.
*   **Dependencies:** NuGet packages are managed in the `.csproj` file.
* **Startup:** The application is configured and started in `Program.cs`.

## Configuration

Update `appsettings.json` for database connection and application settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=fourtitude.db"
  }
}
```

## Dependencies

- ASP.NET Core (.NET 8)
- Entity Framework Core
- SQLite
- Log4Net
- Swagger/OpenAPI

## Logging

Logs are configured via Log4Net and can be found in the `logs` directory. The middleware captures all HTTP requests and responses for auditing and debugging purposes.
