# Data Favorites Extractor

Small C# console app that reads lines from `Favorites_NOTES.txt` and prints distinct left-side substrings (text before the first `_`).

Usage:

```bash
dotnet run --project DataFavoritesExtractor.csproj -- [path/to/Favorites_NOTES.txt]
```

If no path is provided it will read `Favorites_NOTES.txt` from the current working directory.
