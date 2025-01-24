# Fastenshtein
[![NuGet](https://img.shields.io/nuget/v/Fastenshtein.svg)](https://www.nuget.org/packages/Fastenshtein/) [![GitHub action build](https://github.com/DanHarltey/Fastenshtein/actions/workflows/main-build.yml/badge.svg?branch=master)](https://github.com/DanHarltey/Fastenshtein/actions/workflows/main-build.yml) [![AppVeyor Build](https://ci.appveyor.com/api/projects/status/my7qghoen4pofb3h?svg=true)](https://ci.appveyor.com/project/DanHarltey/fastenshtein) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE) [![Unit test coverage](https://coveralls.io/repos/github/DanHarltey/Fastenshtein/badge.svg?branch=master)](https://coveralls.io/github/DanHarltey/Fastenshtein?branch=master)

One of the fastest .Net Levenshtein projects around.

Fastenshtein is an optimized and fully unit tested Levenshtein implementation. It is optimized for speed and memory usage.

From the included brenchmarking tests comparing random words of 3 to 20 random chars to other Nuget Levenshtein implementations.

| Method                  | Mean     | Ratio | Rank | Gen0     | Allocated  | Alloc Ratio |
|------------------------ |---------:|------:|-----:|---------:|-----------:|------------:|
| Fastenshtein            | 1.077 ms |  1.00 |    1 |        - |     6345 B |       1.000 |
| FastenshteinStatic      | 1.122 ms |  1.04 |    2 |   3.9063 |   265441 B |      41.835 |
| NinjaNye                | 1.899 ms |  1.76 |    4 |  76.1719 |  4274593 B |     673.695 |
| StringSimilarity        | 2.899 ms |  2.69 |    5 |   7.8125 |   543770 B |      85.701 |
| FuzzyStringsNetStandard | 7.351 ms |  6.81 |    6 | 414.0625 | 22967283 B |   3,619.745 |

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
2. Beginning with SQL Server 2017 (14.x). Either configure [CLR strict security](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/clr-strict-security?view=sql-server-ver15) or run the below to disable it.
```sql
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;

EXEC sp_configure 'clr strict security', 0;
RECONFIGURE;
```

3. Place the Fastenshtein.dll on the same computer as the SQL Server instance in a directory that the SQL Server instance has access to. You must use the .Net framework version 4.6.2 of Fastenshtein. To create the assembly (dll) either:

* Compile the project “Fastenshtein” in Release config in Visual Studio.

OR

* Download the pre-compiled dll from [nuget](https://www.nuget.org/api/v2/package/Fastenshtein/) unzip the package and use the dll in \lib\net462 folder.

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