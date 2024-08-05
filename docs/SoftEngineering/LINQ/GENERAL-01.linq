<Query Kind="Program">
  <Namespace>Microsoft.VisualBasic</Namespace>
  <Namespace>Microsoft.VisualBasic.FileIO</Namespace>
  <Namespace>System.Collections.Immutable</Namespace>
</Query>


// SEE ALSO: C:\src\dev\coder-foundry-2\docs\SoftEngineering\LINQ\GENERAL-01.linq
void Main()
{
	// CsvRead();
	// DateTimeHaha();
	// CopyFilesToProcess();
	// DeleteProcessedFiles();
	// FileListings();
	// NullCoalescing();
	// StringFormatting();
	// GetPayorInfo();
	// PayorToggleSetting();
	// GetTransactions();
	// GetVisitInfo();
	// PassByReference();
	// StringMassage();
	// TrackProgress();
	// ShowUniqueAndDupes();
	// ActionExample();
	ImmutableExamples();
}

void ImmutableExamples()
{
	var numbers = ImmutableList.Create(1, 2, 6, 1, 8, 40);
	numbers.Dump();
}

void ActionExample()
{
	Action<int> printNumb = (num) => Console.WriteLine(num);
	printNumb(40);
}

void ShowUniqueAndDupes()
{
	var fullList = new List<int>()
	{
		4361335,
		4207867,
		4387229,
		4387229,
		4288668,
		4386235,
		4360902,
		4393522,
		4391823,
		4388996,
		4387293,
		4387745,
		4149245,
		4393766,
		4393693,
		4346628,
		4346628,
		4393975,
		4226661,
		4393959,
		4393961,
		4393961,
		4393959,
		4393646,
		4393874,
		4389093,
		4389093,
		4363814,
		4393553,
		4393553,
		4393862,
		4393862,
		4393862,
		4393538,
		4393541,
		4393695,
		4393693,
		4370981,
		4389210,
		4387293,
		4391846,
		4193926,
		4193926,
		4389212,
		4393639,
		4391972,
		4393617,
		4289523,
		4393575,
		4366118
	};
	fullList.Count.Dump();
	var uniques = fullList.Distinct().ToList();
	uniques.Count.Dump();
	uniques.Dump();
}

void DeleteProcessedFiles()
{
	// Get list of files in AVIs (I:\Media-Track\MoviesAndTv\_Family_Flat\BLUEY\AVIs)
	// Foreach file in ToProcess
	// if exists in AVIs directory, delete it from ToProcess

	var aviFiles = Directory.GetFiles(@"I:\Media-Track\MoviesAndTv\_Family_Flat\BLUEY\AVIs", "*.avi").ToList();
	var aviFilesMassaged = new List<string>();
	foreach (var avi in aviFiles)
	{
		var basename = Path.GetFileNameWithoutExtension(avi);
		aviFilesMassaged.Add(basename);
	}

	var toProcessDir = @"I:\Media-Track\MoviesAndTv\_Family_Flat\BLUEY\ToProcess";
	var matchedAviCount = 0;
	var mkvFiles = Directory.GetFiles(toProcessDir, "*.mkv").ToList();
	foreach (var mkv in mkvFiles)
	{
		var baseMkv = Path.GetFileNameWithoutExtension(mkv);
		if (aviFilesMassaged.Any(a => a == baseMkv))
		{
			matchedAviCount++;
			// UN-COMMENT TO DELETE!!!
			// File.Delete(mkv);
			baseMkv.Dump();
		}
	}
	Console.WriteLine($"Matched {matchedAviCount} AVI(s)");
}

void CopyFilesToProcess()
{
	// Get list of files in AVIs (I:\Media-Track\MoviesAndTv\_Family_Flat\BLUEY\AVIs)
	// Foreach file in Parent Dir
	// if (!avis.Contains(file))
	//   copy to <to-process> directory

	var aviFiles = Directory.GetFiles(@"I:\Media-Track\MoviesAndTv\_Family_Flat\BLUEY\AVIs", "*.avi").ToList();
	var aviFilesMassaged = new List<string>();
	foreach (var avi in aviFiles)
	{
		var basename = Path.GetFileNameWithoutExtension(avi);
		aviFilesMassaged.Add(basename);
	}

	var toProcessDir = @"I:\Media-Track\MoviesAndTv\_Family_Flat\BLUEY\ToProcess";
	var i = 0;
	var matchedAviCount = 0;
	var mkvFiles = Directory.GetFiles(@"I:\Media-Track\MoviesAndTv\_Family_Flat\BLUEY", "*.mkv").ToList();
	foreach (var mkv in mkvFiles)
	{
		var baseMkv = Path.GetFileNameWithoutExtension(mkv);
		if (aviFilesMassaged.Any(a => a == baseMkv))
		{
			matchedAviCount++;
		}
		else
		{
			i++;
			var ext = Path.GetExtension(mkv);
			baseMkv.Dump();
			var targetFile = Path.Combine(toProcessDir, baseMkv + ext);
			File.Copy(mkv, targetFile);
		}
	}
	Console.WriteLine($"Matched {matchedAviCount} AVI(s)");
	Console.WriteLine($"{i} MKV(s) to process");
}

void TrackProgress()
{
	// var init = 0;
	// var countPerHour = 85;
	// while (init < 520)
	// {
	// 	init += countPerHour;
	// 	init.Dump();
	// }
	var init = 520;
	var countPerHour = 85;
	while (init > 0)
	{
		init = init - countPerHour;
		init.Dump();
	}
}

void StringMassage()
{
	var str = "LoremIpsumDeplorumHouseOfTargaryan_0 Failed";
	var shorterStr = str.Substring(0, 18);
	shorterStr.Dump();
	// var newStr = str.Replace(" ", "-");
	// newStr.ToUpper().Dump();
}

class Nested 
{
	public int? NestedId { get; set; }
	public bool IsQualified { get; set; }
	public string Message { get; set; }
}

void PassByReference()
{
	var nested = new Nested{NestedId = 1, IsQualified = true};
	ValidateStuff(nested);
	nested.Dump();
}

void ValidateStuff(Nested nested)
{
	nested.NestedId++;
	nested.IsQualified = !nested.IsQualified;
	nested.Message = "haha";
}

void CsvRead()
{
	var csvFilePath = @"FILE.CSV";
	csvFilePath.Dump();
	using (TextFieldParser parser = new TextFieldParser(csvFilePath))
	{
		parser.TextFieldType = FieldType.Delimited;
		parser.SetDelimiters(",");
		int i = 0;
		while (!parser.EndOfData)
		{
			Console.Write($"[{i++}]: ");
			//Process row
			int j = 0;
			string[] fields = parser.ReadFields();
			foreach (string field in fields)
			{
				Console.Write(field + ", ");
				j++;
			}
			Console.WriteLine();
		}
	}

}

void DateTimeHaha()
{
	DateTime dateFromthePast = DateTime.Now.AddDays(-217);
	dateFromthePast.Dump();
}

void FileListings()
{
	var today = DateTime.Today;
	var todayMidnight = new DateTime(today.Year, today.Month, today.Day);
	// var todayMidnight = new DateTime( today.Date.DateOnly());
	var testDir = @"C:\src\yo-test";
	// var di = new DirectoryInfo(testDir);

	var archiveDir = @"\\dbintsys110\G$\Archive";
	var inboundDropDir = @"\\dbintsys110\G$\InboundDrop";
	var di = new DirectoryInfo(inboundDropDir);
	ListFilesNewThanDate(di, todayMidnight);
}

void ListFilesNewThanDate(DirectoryInfo di, DateTime todayMidnight)
{
	foreach (var fi in di.GetFiles("*.hl7").Take(3).Where(f => f.LastWriteTime > todayMidnight))
	{
		fi.FullName.Dump();
	}

	foreach (var subdir in di.GetDirectories())
	{
		ListFilesNewThanDate(subdir, todayMidnight);
	}
}

void NullCoalescing()
{
	Nested foo = new Nested();
	var hasValue = foo?.NestedId.HasValue;
	hasValue.Dump();
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
	var toPrint = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
	toPrint.Dump();
	return;
	
	var fmt = "{0:D}";
	toPrint = string.Format(fmt, DateTime.Now);
	toPrint.Dump();
	
	var x = 1;
	toPrint = x.ToString("");
	toPrint = toPrint.PadLeft(2, '0');
	toPrint.Dump();
}

// copy /y "C:\Users\inye\OneDrive - Homecare Homebase, LLC\Dev\SoftEngineering\LINQ\GENERAL-01.linq" C:\src\dev\coder-foundry-2\docs\SoftEngineering\LINQ
// copy /y C:\src\dev\coder-foundry-2\docs\SoftEngineering\LINQ\GENERAL-01.linq "C:\Users\inye\OneDrive - Homecare Homebase, LLC\Dev\SoftEngineering\LINQ\"