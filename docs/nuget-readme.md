## About

Fastenshtein is one of the fastest .Net Levenshtein projects around. Levenshtein calculates the shortest possible distance between two strings. Producing a count of the number of insertions, deletions and substitutions to make one string into another.

Fastenshtein is an optimized and fully unit tested Levenshtein implementation. It is optimized for speed and memory usage.

## How to Use

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