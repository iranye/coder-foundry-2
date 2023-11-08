<Query Kind="Program">
  <Connection>
    <ID>420b162b-f922-4b50-91cc-0313fe659f60</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>XBISQL842</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>TEMP_FCHHOH_EH_DEV</Database>
    <NoPluralization>true</NoPluralization>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
  <Namespace>Microsoft.VisualBasic</Namespace>
  <Namespace>Microsoft.VisualBasic.FileIO</Namespace>
</Query>


// SEE ALSO: C:\src\dev\coder-foundry-2\docs\SoftEngineering\LINQ\GENERAL-01.linq
void Main()
{
	// CsvRead();
	// DateTimeHaha();
	// FileListings();
	// NullCoalescing();
	StringFormatting();
	// GetPayorInfo();
	// PayorToggleSetting();
	// GetTransactions();
	// GetVisitInfo();
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
	
	var x = 1;
	toPrint = x.ToString("");
	toPrint = toPrint.PadLeft(2, '0');
	toPrint.Dump();
}

// copy /y "C:\Users\inye\OneDrive - Homecare Homebase, LLC\Dev\SoftEngineering\LINQ\GENERAL-01.linq" C:\src\dev\coder-foundry-2\docs\SoftEngineering\LINQ