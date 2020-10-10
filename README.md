# Fastenshtein
[![NuGet](https://img.shields.io/nuget/v/Fastenshtein.svg)](https://www.nuget.org/packages/Fastenshtein/) [![Build status](https://img.shields.io/appveyor/ci/DanHarltey/fastenshtein/master.svg?label=appveyor)](https://ci.appveyor.com/project/DanHarltey/fastenshtein/branch/master) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

The fastest .Net Levenshtein around.

Fastenshtein is an optimized and unit tested Levenshtein implementation. It is optimized for speed and memory usage.

From the included brenchmarking tests comparing random words of 3 to 20 random chars to other Nuget Levenshtein implementations.

|                Method |        Mean |    StdDev | Scaled | Scaled-StdDev |     Gen 0 | Allocated |
|---------------------- |------------ |---------- |------- |-------------- |---------- |---------- |
|          Fastenshtein |  16.2006 ms | 0.0069 ms |   1.00 |          0.00 |         - |  20.48 kB |
|    FastenshteinStatic |  17.2029 ms | 0.0234 ms |   1.06 |          0.00 |         - |   2.81 MB |
|      StringSimilarity |  24.1955 ms | 0.0280 ms |   1.49 |          0.00 |  329.1667 |   5.87 MB |
|              NinjaNye |  35.9226 ms | 0.0152 ms |   2.22 |          0.00 | 6337.5000 |  44.21 MB |
| TNXStringManipulation |  45.4600 ms | 0.0065 ms |   2.81 |          0.00 | 3329.1667 |  24.63 MB |
|   MinimumEditDistance | 207.9967 ms | 0.0893 ms |  12.84 |          0.01 | 3404.1667 |  25.59 MB |

## Usage

```cs
int levenshteinDistance = Fastenshtein.Levenshtein.Distance("value1", "value2");
```
Alternative method for comparing one item against many (quicker due to less memory allocation, not thread safe)
```cs
Fastenshtein.Levenshtein lev = new Fastenshtein.Levenshtein("value1");
foreach (var item in new []{ "value2", "value3", "value4"})
{
	int levenshteinDistance = lev.DistanceFrom(item);
}
```
### How to include Fastenshtein in Microsoft SQL Server (SQLCLR)

We will create Fastenshtein as a CLR Scalar-Valued Function within SQL Server. This will allow the fast Levenshtein implementationto be used within SQL Server.

1. To enable CLR integration for the server:
```sql
sp_configure 'clr enabled', 1
RECONFIGURE
```
2. Beginning with SQL Server 2017 (14.x), either configure [CLR strict security](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/clr-strict-security?view=sql-server-ver15) or run the below to disable it.
```sql
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;

EXEC sp_configure 'clr strict security', 0;
RECONFIGURE;
```

3. Place the Fastenshtein.dll on the same computer as the SQL Server instance in a directory that the SQL Server instance has access to. You must use the .Net framework version 4 of Fastenshtein. To create the assembly (dll) either:

* Compile the project “FastenshteinFramework” in Release config in Visual Studio.

OR

* Download the pre-compiled dll from [nuget](https://www.nuget.org/api/v2/package/Fastenshtein/) unzip the package and use the dll in \lib\net40-client folder.

4. Create the assembly
```sql
CREATE ASSEMBLY FastenshteinAssembly FROM 'C:\Fastenshtein.dll' WITH PERMISSION_SET = SAFE
```

5. Then create the function
```sql
CREATE FUNCTION [Levenshtein](@value1 [nvarchar](MAX), @value2 [nvarchar](MAX))
RETURNS [int]
AS 
EXTERNAL NAME [FastenshteinAssembly].[Fastenshtein.Levenshtein].[Distance]
GO
```

6. It is now ready to be used: 
```sql
-- Usage
DECLARE @retVal as integer
select @retVal = [dbo].[Levenshtein]('Test','test')
Select @retVal
```