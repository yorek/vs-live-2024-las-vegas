# VS Live 2024 Las Vegas Demos

Website: https://vslive.com/events/las-vegas-2024/home.aspx

## W20 My Application is Fully Concurrent and Async. What About the Database?

You have created a beautiful, concurrent application taking advantage of all the latest and greatest language features. It scales magnifically and it’s the jewel of the cloud. But what about the database? What happens when simultaneous queries are executed against the same data? Using Azure SQL Database or SQL Server, we'll see what happens behind the scenes, how it guarantees concurrency *and* consistency and what options you have to performance *and* scalability.

### You will learn:

Understand what are locks, why they are needed and how they work
How to create applications with databases that can scale as much as needed
About isolation levels and database internals

### Demos

Available in the `./concurrency` folder.

## T13 Schema Management and Evolution to Create Flexible and High-performance Applications

Managing a data model and schema that is not known upfront and that can change over time might seem an impossible task for a relational database. Well, modern relational databases offer plenty of flexibility regarding such complex topic. From metadata-only schema updates to full support for JSON, though hybrid document-relational models, the possibilities are endless. Such a wide option of possibilities opens interesting discussion on where should land the responsibility of managing the schema changes. In the application? In the database? In between? In this session we’ll discuss these scenarios with practical example and an open mind.”

### You will learn:

- New JSON features in Azure SQL and SQL Server
- The pros and cons of managing the schema in the app or in the db
- A third, Hybrid approach

### Demos

The demos are available in their own repositories. The first one uses the Dapper micro-ORM and the second one uses Entity Framework Core.

- https://github.com/azure-samples/azure-sql-db-dynamic-schema
- https://github.com/azure-samples/azure-sql-db-dynamic-schema-ef
