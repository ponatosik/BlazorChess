using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorChess.Client.Services;

public class FullscreenService
{
	private readonly IJSRuntime _jsRuntime;

	public FullscreenService(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	public ValueTask OpenFullscreen(ElementReference elem)
	{
		return _jsRuntime.InvokeVoidAsync("Element.prototype.requestFullscreen.call", elem);
	}

	public ValueTask<bool> CloseFullscreen()
	{
		return _jsRuntime.InvokeAsync<bool>("document.exitFullscreen");
	}

	public ValueTask<ElementReference?> FullScreenElement()
	{
		return _jsRuntime.InvokeAsync<ElementReference?>("document.fullscreenElement");
	}

	public async ValueTask<bool> IsInFullscreen()
	{
		return await FullScreenElement() is not null;
	}
}
