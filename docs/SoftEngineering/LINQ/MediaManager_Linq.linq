<Query Kind="Program">
  <NuGetReference>Npgsql</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Npgsql</Namespace>
</Query>

async Task Main()
{
	var connString = $"Host=127.0.0.1;Port=5666;Database=test;Username=linqpad;Password=password";

	await using var conn = new NpgsqlConnection(connString);
	await conn.OpenAsync();

	await using (var cmd = new NpgsqlCommand("select \"Title\" from \"Volumes\"", conn))
	await using (var reader = await cmd.ExecuteReaderAsync())
		while (await reader.ReadAsync())
			Console.WriteLine(reader.GetString(0));

	await using (var cmd = new NpgsqlCommand("select \"Created\" from \"Volumes\"", conn))
	await using (var reader = await cmd.ExecuteReaderAsync())
		while (await reader.ReadAsync())
			Console.WriteLine(reader.GetDateTime(0));
}
