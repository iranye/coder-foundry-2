<Query Kind="Program" />

void Main()
{
	LinqSelect();
	// Regex();
	// FizzBuzz();
	// NullCoalescing();
	// StringFormatting();
}

class Book
{
	// TODO: Implement Cloneable and walkthrough blog: https://weblogs.asp.net/bleroy/linq-lambdas
	string Title { get; set; }
	string Author { get; set; }
}

void LinqSelect()
{
	string[] fruits = { "apple", "banana", "mango", "orange",
					  "passionfruit", "grape" };

	var query = fruits.Select((fruit, index) => new { index, str = fruit.Substring(0, index) });
	foreach (var obj in query)
	{
		Console.WriteLine("{0}", obj);
	}
	Console.WriteLine();

	var query2 = fruits.Select((fruit, index) => new { index, str = fruit + index });
	foreach (var obj in query2)
	{
		Console.WriteLine("{0}", obj);
	}
}

void Regex()
{
	string pattern = @"^[0-9].*";
	Regex rg = new Regex(pattern);
	// Long string
	var authors = new[] { "Mahesh Chand", "9Raj Kumar", "Mike Gold", "Allen O'Neill", "Marshal Troll", "2-pac" };
	foreach (var author in authors)
	{
		if (rg.IsMatch(author))
		{
			Console.WriteLine(author);
		}		
	}
}

void FizzBuzz()
{
	int fizz = 3;
	int buzz = 5;
	int max = 20;
	var results = GetFizzBuzz(fizz, buzz, max);
	results.Dump();
}

string[] GetFizzBuzz(int fizz, int buzz, int max)
{
	var ret = new string[max];

	for (int ind = 1; ind <= max; ind++)
	{
		var fizzBuzz = string.Empty;
		if (ind % fizz == 0)
		{
			fizzBuzz = "fizz";
		}
		if (ind % buzz == 0)
		{
			fizzBuzz += "buzz";
		}
		if (String.IsNullOrEmpty(fizzBuzz))
		{
			fizzBuzz = ind.ToString();
		}
		ret[ind - 1] = fizzBuzz;
	}

	return ret;
}

void NullCoalescing()
{
	List<int> numbers = null;
	int? a = null;

	Console.WriteLine((numbers is null)); // expected: true
										  // if numbers is null, initialize it. Then, add 5 to numbers
	(numbers ??= new List<int>()).Add(5);
	Console.WriteLine(string.Join(" ", numbers));  // output: 5
	Console.WriteLine((numbers is null)); // expected: false        


	Console.WriteLine((a is null)); // expected: true
	Console.WriteLine((a ?? 3)); // expected: 3 since a is still null 
								 // if a is null then assign 0 to a and add a to the list
	numbers.Add(a ??= 0);
	Console.WriteLine((a is null)); // expected: false        
	Console.WriteLine(string.Join(" ", numbers));  // output: 5 0
	Console.WriteLine(a);  // output: 0

	a = null;
	a ??= 2;
	numbers.Add(a ??= 1);
	Console.WriteLine((a is null)); // expected: false        
	Console.WriteLine(string.Join(" ", numbers));  // output: 5 0
	Console.WriteLine(a);  // output: 0

}

void StringFormatting()
{
	var fmt = "{0:D}";
	var toPrint = string.Format(fmt, DateTime.Now);
	toPrint.Dump();
}

