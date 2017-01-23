# Fastenshtein
[![NuGet](https://img.shields.io/nuget/v/Fastenshtein.svg)](https://www.nuget.org/packages/Fastenshtein/) [![Build status](https://img.shields.io/appveyor/ci/DanHarltey/fastenshtein/master.svg?label=appveyor)](https://ci.appveyor.com/project/DanHarltey/fastenshtein/branch/master) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)

The fastest .Net Levenshtein around.

I got tired of seeing slow Levenshtein implementations, think of the CPU cycles we could be saving! 
You owe it to your end user to use this.

Under the [MIT license](LICENSE) also available as [NuGet package](https://www.nuget.org/packages/Fastenshtein/).

Find this useful? Let me know to make me happy.

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