# How to include Fastenshtein in Microsoft SQL Server (SQLCLR)

We will create Fastenshtein as a CLR Scalar-Valued Function within SQL Server. This will allow the fast Levenshtein implementation to be used within SQL Server.

1. To enable CLR integration for the server:
   ```sql
   sp_configure 'clr enabled', 1
   RECONFIGURE
   ```
2. Beginning with SQL Server 2017 (14.x). Either configure [CLR strict security](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/clr-strict-security?view=sql-server-ver15) or run the below to disable it:
   ```sql
   EXEC sp_configure 'show advanced options', 1;
   RECONFIGURE;

   EXEC sp_configure 'clr strict security', 0;
   RECONFIGURE;
   ```

3. To load Fastenshtein onto the server, you must use the .Net framework version 4.6.2. This can be done in two ways:

   - Using assembly bits. Download "Fastenshtein SQL Assembly Hex" from the [lastest release](https://github.com/DanHarltey/Fastenshtein/releases/latest). Unzip the file and copy the full contents of the "Fastenshtein_net462.hex" file into the below:

     ```sql
     CREATE ASSEMBLY FastenshteinAssembly
     FROM 0x{contents of Fastenshtein_net462.hex}
     WITH PERMISSION_SET = SAFE;
     ```

   - Local path or network location to the assembly. Place the Fastenshtein.dll in a directory that the SQL Server instance has access to. To create the assembly (dll) either:

     * Compile the project “Fastenshtein” in Release config.

      OR

      * Download the pre-compiled dll from [nuget](https://www.nuget.org/api/v2/package/Fastenshtein/) unzip the package and use the dll in \lib\net462 folder.

      ```sql
      CREATE ASSEMBLY FastenshteinAssembly FROM 'C:\Fastenshtein.dll' WITH PERMISSION_SET = SAFE
      ```

4. Then create the function
   ```sql
   CREATE FUNCTION [Levenshtein](@value1 [nvarchar](MAX), @value2 [nvarchar](MAX))
   RETURNS [int]
   AS 
   EXTERNAL NAME [FastenshteinAssembly].[Fastenshtein.Levenshtein].[Distance]
   GO
   ```

5. It is now ready to be used: 
   ```sql
   -- Usage
   DECLARE @retVal AS INTEGER
   SELECT @retVal = [dbo].[Levenshtein]('Test','test')
   SELECT @retVal
   ```