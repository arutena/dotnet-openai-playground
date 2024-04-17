namespace OpenAI.API.Services;

public interface IPromptService
{
    Task<string> TriggerOpenAI(string prompt);
}
