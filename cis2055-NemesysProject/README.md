# CIS21055 - Nemesy Web App

Here is a small guide on how to setup the solution
The application was built using ASP .NET Core MVC Entity Framework, Database utilized was with SQL Server & SQL Management Studios. 
 1. Appsettings.json
Upon cloning the repository create a new appsettings.json file and within it write   

`{
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "AllowedHosts": "*",
      "ConnectionStrings": {
        "NemesysContext": "Data Source=[Enter your SQL Server Name];Database=[Enter a Database Name];Initial Catalog=cis2055-nemesys;Integrated Security=True;"
      }
    }`

 2. Open Package Manager Console and enter the command:
 `Update-Database`

3. You are now able to running the web application!

To login with the pre-initialized accounts use either:
 - Email: reporter@gmail.com , investigator@gmail.com
 -  Password: Reporter123!. Investigator123!

OR you can register ones yourself as you interact with the web application.
