# K.A.T.E (Keys Are The Enemy)
# AccessPointManagement API

This repository contains an API for managing access points. The API provides several features such as creating, reading, updating, and deleting access points, handling user access, user registration, and generating access reports.

## Features

- **Access Point Management**: Add, update, delete, and retrieve access points.
- **User Management**: Handle user registration, authentication, access levels, and report access permissions.
- **Access Event Reporting**: Generate reports of access events based on user and access point.

## Technologies Used

- .NET 6
- Entity Framework Core
- MariaDB
- JWT Auth
- xUnit for testing

## API Endpoints

**AuthController**
- DEMO-ONLY-get-all-users
- sign-up
- sign-in

**AccessPointController** (Authenticated users only)
- DEMO-ONLY-get-access-point-ids: Allows Anonymous
- get-access-point-history: Restricted to users with role 'Admin'
- open-access-point: Accessible to 'Admin' and 'User' roles
- add-access-point: Restricted to users with role 'Admin'
- delete-access-point: Restricted to users with role 'Admin'
- update-access-point: Restricted to users with role 'Admin'

## Getting Started

**Clone the Repository**

On your command line:

    git clone https://github.com/DieGrysWolf/K.A.T.E..git

**Update Database Connection**

    Modify the connection string in the `appsettings.json` file to match your database configuration.

**Run The Migrations**

    dotnet ef database update --project Infrastructure --startup-project API

## Running Tests

The required NuGet packages have been added to execute the tests via Visual Studio's Test Explorer

Core services are tested for positive and negative results
