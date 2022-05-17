# DataAccessAPI
API to access items information using different ways of data access: Entity Framework, Dapper, ADO.NET

This API uses the following technologies: C#, ASP.NET WEB API, SQL Server.

The API currently uses Dapper to access data. To switch to a different type of data access, go to the Extensions folder 
and find the ApplicationServiceExtensions class. Here you can comment out line 20 (Dapper implementation), and uncomment
line 19 to get the Entity FrameWork implementation, or uncomment line 21 to get the ADO.NET implementation.


