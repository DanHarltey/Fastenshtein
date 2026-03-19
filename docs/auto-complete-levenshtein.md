# Autocomplete / Type-ahead / Prefix

Fastenshtein also includes AutoCompleteLevenshtein. A static class for efficient autocomplete-style matching without using substrings. This is useful when your user is entering partial input and you want to match it against full candidate values.

The method avoids using substrings and associated memory allocations.

## What it does

The first parameter is the incomplete value you want to match. The second parameter is the full candidate value to compare against.

```
   int distance = Fastenshtein.AutoCompleteLevenshtein.Distance("Benidorm", "Benidorm, Spain");
   // distance is 0
```

When the first parameter is smaller than the second, it calculates the Levenshtein distance as if the second parameter is the same length.

## Why it exists

It avoids cases where comparing a short/partial input against a long candidate produces a large distance that doesn’t reflect how close the start of the candidate matches what the user typed.

```
   // Autocomplete-style comparisons (user input first, candidate second)
   int prefixDistance = Fastenshtein.AutoCompleteLevenshtein.Distance("Bendorm", "Benidorm, Spain"); 
   // distance is 2
   
   // This differs from full-string Levenshtein distance.
   int fullDistance = Fastenshtein.Levenshtein.Distance("Bendorm", "Benidorm, Spain");
   // much larger, distance is 8
```

## Important note / gotcha

Because it shortens the candidate to the input length, you can get results that differ from full edit-distance expectations.
For example, "tst" vs "test" will be compared as "tst" vs "tes" (candidate is shortened to 3 chars),
which yields a distance of 2 rather than 1.

## Example: ranking suggestions

```
   var input = "Bendorm";
   var candidates = new[]
   {
       "Benidorm, Spain",
       "Berlin, Germany",
       "Bendigo, Australia",
       "London, UK"
   };   

   var bestMatches = candidates
       .Select(c => new { Candidate = c, Distance = Fastenshtein.AutoCompleteLevenshtein.Distance(input, c) })
       .OrderBy(x => x.Distance);

   foreach (var match in bestMatches)
   {
       Console.WriteLine($"{match.Distance} - {match.Candidate}");
   }
```