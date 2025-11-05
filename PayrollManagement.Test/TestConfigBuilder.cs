using System.IO;
using Microsoft.Extensions.Configuration;

public static class TestConfigBuilder
{
	public static IConfiguration BuildConfiguration()
	{
		// Solution kök dizinini bul
		var currentDir = Directory.GetCurrentDirectory();
		DirectoryInfo? dir = new DirectoryInfo(currentDir);

		while (dir != null && !dir.GetFiles("*.sln").Any())
		{
			dir = dir.Parent;
		}

		if (dir == null)
			throw new DirectoryNotFoundException("Solution root (.sln) not found.");

		// API projesindeki appsettings.json'u bul
		var configPath = Path.Combine(dir.FullName, "PayrollManagament.API", "appsettings.json");

		if (!File.Exists(configPath))
			throw new FileNotFoundException($"appsettings.json bulunamadı: {configPath}");

		// Config oluştur
		return new ConfigurationBuilder()
			.AddJsonFile(configPath, optional: false, reloadOnChange: true)
			.Build();
	}
}