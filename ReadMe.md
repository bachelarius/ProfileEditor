# Profile Editor

## Brief

A .NET Core MVC app to allow a user to log in and display a list of Persons

A Person must contain the following information:

- First Name
- Last Name
- Email Address
- Phone Number
- Date of Birth
- Gender
- Image Upload

## Running instructions

The application is lauched using the `docker-compose.yml` file, or the Docker Compose project within Visual Studio 2022.

It has two dependencies, both running within docker:
- MSSQL for storing the authorization and Person records
- Azure Blob storage (locally running within Azurite)
