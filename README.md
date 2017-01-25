# Fastenshtein
[![NuGet](https://img.shields.io/nuget/v/Fastenshtein.svg)](https://www.nuget.org/packages/Fastenshtein/) [![Build status](https://img.shields.io/appveyor/ci/DanHarltey/fastenshtein/master.svg?label=appveyor)](https://ci.appveyor.com/project/DanHarltey/fastenshtein/branch/master) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

The fastest .Net Levenshtein around.

Fastenshtein is an optimized and unit tested Levenshtein implementation. It is optimized for speed and memory usage.

From the included brenchmarking tests comparing random words of 3 to 20 random chars to other Nuget Levenshtein implementations.

                Method |        Mean |    StdDev | Scaled | Scaled-StdDev |     Gen 0 | Allocated |
---------------------- |------------ |---------- |------- |-------------- |---------- |---------- |
          Fastenshtein |  16.2006 ms | 0.0069 ms |   1.00 |          0.00 |         - |  20.48 kB |
    FastenshteinStatic |  17.2029 ms | 0.0234 ms |   1.06 |          0.00 |         - |   2.81 MB |
      StringSimilarity |  24.1955 ms | 0.0280 ms |   1.49 |          0.00 |  329.1667 |   5.87 MB |
              NinjaNye |  35.9226 ms | 0.0152 ms |   2.22 |          0.00 | 6337.5000 |  44.21 MB |
 TNXStringManipulation |  45.4600 ms | 0.0065 ms |   2.81 |          0.00 | 3329.1667 |  24.63 MB |
   MinimumEditDistance | 207.9967 ms | 0.0893 ms |  12.84 |          0.01 | 3404.1667 |  25.59 MB |

## Usage

```cs
int levenshteinDistance = Fastenshtein.Levenshtein.Distance("value1", "value2");
```
Alternative method for comparing one item against many (quicker due to less memory allocation)
```cs
Fastenshtein.Levenshtein lev = new Fastenshtein.Levenshtein("value1");
foreach (var item in new []{ "value2", "value3", "value4"})
{
	int levenshteinDistance = lev.Distance(item);
}
```
### SQL Server Hosting (SQLCLR)
If you are crazy enough to want to do this. Place the Fastenshtein.dll on the SQL Servers hard drive and do the following…
```sql
CREATE ASSEMBLY Fastenshtein from 'C:\Program Files\Microsoft SQL Server\MSSQL11.DEV\MSSQL\Binn\Fastenshtein.dll' WITH PERMISSION_SET = SAFE

CREATE FUNCTION [dbo].[Levenshtein](@s [nvarchar](4000), @t [nvarchar](4000))
RETURNS [int] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [Fastenshtein].[Fastenshtein.Levenshtein].[Distance]
GO

-- Usage
DECLARE @retVal as integer
select @retVal = [dbo].[Levenshtein]('Test','test')
Select @retVal
```