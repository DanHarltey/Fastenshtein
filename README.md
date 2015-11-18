# Fastenshtein
The fastest .Net Levenshtein around.

I got tired of seeing slow Levenshtein implementations, think of the CPU cycles we could be saving! 
You owe it to your end user to use this.

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

It is under the [MIT license](LICENSE).

Find this useful? Let me know to make me happy.