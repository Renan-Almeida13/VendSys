# VendSys - DEX Processing System

System for processing DEX files from vending machines, developed with ASP.NET Core and Angular.

## 🚀 Technologies

- **Backend:**
  - ASP.NET Core 8.0
  - SQL Server
  - Minimal APIs
  - Basic Authentication
  - Swagger/OpenAPI

- **Frontend:**
  - Angular 17
  - TypeScript
  - SCSS

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [SQL Server](https://www.microsoft.com/sql-server)
- [Angular CLI](https://cli.angular.io/)

## 🔧 Setup

### Database

1. In SQL Server Management Studio, run the database creation script:
   ```sql
   NayaxVendSys.Api/Scripts/CreateDatabase.sql
   ```

2. Update the connection string in `NayaxVendSys.Api/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YourServer;Database=NayaxVendSys;User Id=YourUser;Password=YourPassword;TrustServerCertificate=True;"
     }
   }
   ```

### Backend (ASP.NET Core)

1. Navigate to the API project folder:
   ```bash
   cd NayaxVendSys.Api
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Run the project:
   ```bash
   dotnet run
   ```

The backend will be available at `https://localhost:44316`

### Frontend (Angular)

1. Navigate to the Web project folder:
   ```bash
   cd NayaxVendSys.Web
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Run the project:
   ```bash
   ng serve
   ```

The frontend will be available at `http://localhost:4200`

## 🔒 Authentication

The system uses basic authentication with the following credentials:
- **Username:** vendsys
- **Password:** NFsZGmHAGWJSZ#RuvdiV

## 📁 Project Structure

### Backend

- `NayaxVendSys.Api/`
  - `Authentication/` - Basic authentication configuration
  - `Endpoints/` - API endpoints definition
  - `Models/` - Model classes
  - `Services/` - DEX processing and database access services
  - `Scripts/` - SQL scripts for database creation

### Frontend

- `NayaxVendSys.Web/`
  - `src/app/` - Angular components and services
  - `src/assets/` - Sample DEX files
  - `src/environments/` - Environment configurations

## 📊 Database

The system uses two main tables:

- **DexMeter**
  - Stores general machine information
  - Fields: Id, Machine, DexDateTime, MachineSerialNumber, ValueOfPaidVends

- **DexLaneMeter**
  - Stores product sales information
  - Fields: Id, DexMeterId, ProductIdentifier, Price, NumberOfVends, ValueOfPaidSales

## 🔍 Swagger

API documentation is available through Swagger UI at:
```
https://localhost:44316/swagger
```

## 📝 Features

1. **DEX File Processing**
   - Reading DEX files from two machines (A and B)
   - Extracting relevant information
   - Structured database storage

2. **Web Interface**
   - Simplified DEX file upload
   - Visual processing feedback
   - Modern responsive design

3. **RESTful API**
   - Secure endpoints with authentication
   - Data validation
   - Swagger documentation

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details. 