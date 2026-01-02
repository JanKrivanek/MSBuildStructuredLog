# GitHub Copilot Integration for MSBuild Structured Log Viewer

This folder contains a clean, minimal implementation for integrating GitHub Copilot as an LLM provider.

## Overview

The GitHub Copilot integration provides:
- OAuth Device Code Flow authentication
- Automatic token management and refresh (in-memory only)
- IChatClient implementation compatible with Microsoft.Extensions.AI
- Support for streaming responses and function calling
- Integrated with ResilientChatClient for automatic retry logic

## Architecture

### Authentication Layer (`Authentication/`)
- **GitHubDeviceFlowAuthenticator**: Implements OAuth Device Code Flow for GitHub authentication
- **GitHubCopilotTokenProvider**: Manages token exchange and automatic refresh

### Client Layer (`Client/`)
- **GitHubCopilotHttpClient**: Minimal HTTP client with GitHub Copilot-specific headers
- **GitHubCopilotChatCompletionsClient**: Chat completions API wrapper with streaming support
- **GitHubCopilotChatClient**: IChatClient implementation that bridges to Microsoft.Extensions.AI

### Configuration Layer (`Configuration/`)
- **GitHubCopilotConfiguration**: Simple configuration with environment variable support
- **GitHubCopilotClientBuilder**: Builder pattern for creating configured IChatClient instances

### Models (`Models/`)
- **DeviceCodeResponse**: OAuth device code flow response
- **CopilotToken**: Copilot token with expiration metadata
- **ChatCompletionModels**: Request/response models for chat completions API
- **ModelInfo**: Model information
- **UsageInfo**: Usage and quota information

## Usage

### Environment Variables

```bash
# Minimal setup - device flow will be triggered
set LLM_ENDPOINT=github-copilot
set LLM_MODEL=gpt-4

# Or provide a GitHub token to skip device flow
set LLM_ENDPOINT=github-copilot
set LLM_API_KEY=ghp_yourGitHubTokenHere
set LLM_MODEL=gpt-4
```

### Programmatic Usage

```csharp
var config = new GitHubCopilotConfiguration
{
    DefaultModel = "gpt-4",
    AccountType = CopilotAccountType.Individual
};

var client = await new GitHubCopilotClientBuilder()
    .WithConfiguration(config)
    .WithDeviceCodeCallback((userCode, verificationUrl) =>
    {
        // Show UI for authentication
        Console.WriteLine($"Go to {verificationUrl} and enter code: {userCode}");
    })
    .BuildAsync();

// Use the client
var response = await client.CompleteAsync(
    new[] { new ChatMessage(ChatRole.User, "Hello!") });
```

## Authentication Flow

### Device Code Flow (Recommended)
1. Application requests device code from GitHub
2. User is shown a verification URL and user code
3. User opens URL in browser and enters code
4. Application polls GitHub for authorization
5. GitHub token is exchanged for Copilot token
6. Tokens auto-refresh during session (not persisted)

### Direct Token (Alternative)
Provide token via `GITHUB_COPILOT_TOKEN` or `LLM_API_KEY` to skip device flow.

## Token Management

- Copilot tokens are automatically refreshed 60 seconds before expiration
- Tokens are kept in memory for the session only
- No persistent storage - tokens are transient
- Re-authentication required each session

## Design Decisions

### Why No Token Persistence?
- **Simplicity**: Eliminates platform-specific credential management code
- **Security**: Reduces attack surface - no tokens at rest
- **Maintenance**: No need to manage encrypted storage on multiple platforms
- **GitHub Design**: Copilot tokens are designed to be short-lived and refreshable

### Why ResilientChatClient Wrapper?
- Retry logic is handled by ResilientChatClient at the application level
- Consistent retry behavior across all LLM providers
- Eliminates duplicate retry code in GitHubCopilotHttpClient

## Supported Features

- ✅ Non-streaming responses
- ✅ Streaming responses
- ✅ System, user, and assistant messages
- ✅ Temperature, top_p, max_tokens parameters
- ✅ Multiple models (gpt-4, gpt-4o, gpt-3.5-turbo, claude-3.5-sonnet, o1, o1-mini)
- ✅ Function calling with automatic invocation
- ✅ Automatic retry via ResilientChatClient
- ✅ Rate limit handling (429)
- ✅ Context overflow truncation

## Troubleshooting

### "No GitHub token available"
- Set `LLM_API_KEY` or `GITHUB_COPILOT_TOKEN` environment variable, OR
- Ensure device code callback is configured in the UI

### "Payment required (402)"
- Your GitHub account needs an active Copilot subscription
- Subscribe at https://github.com/features/copilot

### "Rate limit exceeded (429)"
- Automatic retry with exponential backoff is enabled via ResilientChatClient
- Check your usage quotas if this persists

## Dependencies

- Microsoft.Extensions.AI - IChatClient abstraction
- System.Text.Json - JSON serialization

## References

- [GitHub Copilot API Documentation](https://docs.github.com/en/copilot)
- [OAuth Device Flow](https://docs.github.com/en/developers/apps/building-oauth-apps/authorizing-oauth-apps#device-flow)
- [Microsoft.Extensions.AI](https://github.com/dotnet/extensions)
