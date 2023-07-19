using Microsoft.JSInterop;
using System;

namespace BlazorChess.Client.Services;

public class AlertService
{
	private readonly IJSRuntime _jsRuntime;

	public AlertService(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	public ValueTask ShowAsync(string message)
	{
		return _jsRuntime.InvokeVoidAsync("alert", message);
	}
}
