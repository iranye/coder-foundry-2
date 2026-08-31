using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static int Main(string[] args)
    {
        string path = args.Length > 0 ? args[0] : "Favorites_NOTES.txt";

        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"File not found: {path}");
            return 2;
        }

        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var raw in File.ReadLines(path))
        {
            if (string.IsNullOrWhiteSpace(raw))
                continue;

            var line = raw.Trim();
            var left = line.Split(new[] { '_' }, 2)[0];
            if (!string.IsNullOrEmpty(left))
                set.Add(left);
        }

        foreach (var item in set.OrderBy(s => s, StringComparer.OrdinalIgnoreCase))
            Console.WriteLine(item);

        return 0;
    }
}
