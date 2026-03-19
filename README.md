# Fastenshtein
[![NuGet](https://img.shields.io/nuget/v/Fastenshtein.svg)](https://www.nuget.org/packages/Fastenshtein/) [![GitHub action build](https://github.com/DanHarltey/Fastenshtein/actions/workflows/main-build.yml/badge.svg?branch=master)](https://github.com/DanHarltey/Fastenshtein/actions/workflows/main-build.yml) [![AppVeyor Build](https://ci.appveyor.com/api/projects/status/my7qghoen4pofb3h?svg=true)](https://ci.appveyor.com/project/DanHarltey/fastenshtein) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE) [![Unit test coverage](https://coveralls.io/repos/github/DanHarltey/Fastenshtein/badge.svg?branch=master)](https://coveralls.io/github/DanHarltey/Fastenshtein?branch=master)

One of the fastest .Net Levenshtein projects around.

Fastenshtein is an optimized and fully unit tested Levenshtein implementation. It is optimized for speed and memory usage.

From the included benchmarking tests comparing random words of 3 to 20 random chars to other Nuget Levenshtein implementations.

| Method                    | Mean       | Ratio | Rank | Gen0      | Allocated  | Alloc Ratio |
|-------------------------- |-----------:|------:|-----:|----------:|-----------:|------------:|
| Fastenshtein              |   896.9 μs |  1.00 |    1 |         - |     6344 B |        1.00 |
| FastenshteinValue         |   886.6 μs |  0.99 |    1 |         - |     4424 B |        0.70 |
| FastenshteinValueAdvanced |   897.7 μs |  1.00 |    1 |         - |          - |        0.00 |
| FastenshteinStatic        | 1,005.3 μs |  1.12 |    2 |   31.2500 |   265440 B |       41.84 |
| FastenshteinStaticSpan    |   996.7 μs |  1.11 |    2 |   31.2500 |   265440 B |       41.84 |
| FuzzySharp                | 1,375.3 μs |  1.53 |    3 |   80.0781 |   677088 B |      106.73 |
| NinjaNye                  | 1,463.3 μs |  1.63 |    4 |  509.7656 |  4274592 B |      673.80 |
| StringSimilarity          | 2,046.7 μs |  2.28 |    5 |   62.5000 |   543744 B |       85.71 |
| FuzzyStringsNetStandard   | 6,577.7 μs |  7.33 |    6 | 2742.1875 | 22967280 B |    3,620.32 |

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

## Advanced fetures
  - [Autocomplete / Type-ahead / Prefix](docs/auto-complete-levenshtein.md)
  - [How to include Fastenshtein in Microsoft SQL Server (SQLCLR)](docs/fastenshtein-in-ms-sql-server.md)
