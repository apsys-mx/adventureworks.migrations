# AdventureWorks Database 

For developers building data access libraries, components or any piece or code reading or writing in a database, it's useful to have a database schema with the most common conditions found in a real-worl application.
One of the sample database used by developers to achieve this is the _AdventureWorks_ sample database for SQLServer


>This database sample an other databases can be found in the link below

>https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/downloading-sample-databases

This project is an effort for having a common database schema for other database provider just than SqlServer

> This project created only the database schema, without data.

> This project was build using the .NET Core 3.1

## Installing

1. Clone the repository in you local development folder
2. Run `dotnet build src/apsys.adventureworks.migrations.sln`
3. Create a database in your database instance with name 'AdventureWorks'. _This name is optional. Just take care that the name matches with the database's name used in the connection string passed in the step 5_
4. Open a console and change the location to the build result folder. Depending of your confuguration this folder can be like `src/apsys.adventureworks.migrations/bin/debug/netcoreapp3.1/`
5. On the build result folder execute `apsys.adventureworks.migrations.exe /cnn:"YourSqlServerDatabaseConnectionStringHere" /provider:"YouProviderName"`

> YouProviderName must be replaced with the name of the database provider. Use _sqlserver_ for Microsoft SqlServer. Use _mysql_ for MySql 

>YourSqlServerDatabaseConnectionStringHere must be replaced with the connection string to your database. For Microsoft SqlServer can be like _Server=localhost\SQLEXPRESS;Database=AdventureWorks;Trusted_Connection=True;_


## Related project 

#### Migrator.NET / FluentMigration

https://github.com/fluentmigrator/fluentmigrator