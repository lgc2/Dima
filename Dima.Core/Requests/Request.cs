using System.Text.Json.Serialization;

namespace Dima.Core.Requests;

public abstract class Request
{
	[JsonIgnore]
	public string UserId { get; set; } = string.Empty;
}
