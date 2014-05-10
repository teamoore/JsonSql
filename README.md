JSON (Javascript Object Notation) is a data interchange format that's easy to read both computers and humans. JsonSql library helps to get data from SQL Server as a json formatted. Just add jsonsql.dll into your .Net application as a reference and then [disco](http://youtu.be/UkSPUDpe0U8?t=10s)

Features
========

JsonSql is very simple to use. Give the connection information and procedure name (or t-sql query) to execute. 
Then it returns json formatted result.

Example
=======

    JsonSqlManager mg = new JsonSqlManager();
    mg.ConnectionString = "Server=myServerAddress;Database=myDBase;User Id=myUsername;Password=***;";
    mg.SqlToJson("select * from Table", System.Data.CommandType.Text);
    Console.WriteLine(mg.JsonResult);

Result:

    [
        {
            "UserId": 1,
            "Email": "abc@gmail.com",
            "Name": "timur",
            "Password": "92929",
            "UserLevel": null,
            "CityId": null,
            "Website": null,
            "Telephone": null,
            "CreatedDate": "26.09.2013 14:54:45",
            "Avatar": "user_demo.jpg",
            "IsActive": null
        }
    ]
