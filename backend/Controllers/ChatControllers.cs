using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly GeminiService _gemini;

    public ChatController(GeminiService gemini)
    {
        _gemini = gemini;
    }

    [HttpPost]
    public async Task<ChatResponse> Post(ChatRequest req)
    {
        var reply = await _gemini.AskAsync(req.Message);
        return new ChatResponse
        {
            Reply = reply
        };
    }
}