## Setup and Build

This projects is designed to be built using Visual Studio 2019 (though any .net build environment should work) with the "ASP.NET and Web Development" workflow. 


* Visual Studio 2019 Community Edition can be downloaded from Microsoft.
* Open the `src/Metrc.SampleProject.sln` solution file in Visual Studio.
* Create an empty Sql Database with the name 'Testdb'
* Edit the `appsettings.json` file to connect to the  Database. If using Visual Studios built in local db the connection string should look something like 
 ```Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Testdb;Integrated Security=true;```
* Run the project and navigate to the "About" page. Click the button to run migration to build the database schema and demo data.
	* Clean Migration button will reset the database.

