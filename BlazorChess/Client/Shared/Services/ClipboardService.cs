using Microsoft.JSInterop;

namespace BlazorChess.Client.Services;

public class ClipboardService
{
	private readonly IJSRuntime _jsRuntime;

	public ClipboardService(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	public ValueTask<string> ReadAsync()
	{
		return _jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
	}

	public ValueTask WriteAsync(string text)
	{
		return _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
	}
}
