namespace Dima.E2ETests.Config;

public class TestSettings
{
	public string[] Args { get; set; }
	public float Timeout { get; set; } = 30.0f;
	public bool Headless { get; set; } = true;
	public float SlowMo { get; set; }
	public DriverType DriverType { get; set; } = DriverType.Chrome;
	public string ApplicationUrl { get; set; }
	public string ApplicationApiUrl { get; set; }
	public string DbConnectionString { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
}

public enum DriverType
{
	Chromium,
	Firefox,
	Edge,
	Chrome,
	Webkit
}
