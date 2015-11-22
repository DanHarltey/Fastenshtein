# Fastenshtein
The fastest .Net Levenshtein around.

I got tired of seeing slow Levenshtein implementations, think of the CPU cycles we could be saving! 
You owe it to your end user to use this.

Under the [MIT license](LICENSE) also available as [NuGet package](https://www.nuget.org/packages/Fastenshtein/).

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


Find this useful? Let me know to make me happy.