﻿using OpenAI.API.Dtos;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OpenAI.API.Services;

public class PromptService(IConfiguration configuration) : IPromptService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> TriggerOpenAI(string prompt)
    {
        var apiKey = _configuration.GetValue<string>("OpenAISetting:APIKey");
        var baseUrl = _configuration.GetValue<string>("OpenAISetting:BaseUrl");

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var request = new OpenAIRequestDto
        {
            Model = "gpt-3.5-turbo",
            Messages = new List<OpenAIMessageRequestDto>{
                    new OpenAIMessageRequestDto
                    {
                        Role = "user",
                        Content = prompt
                    }
                },
            MaxTokens = 100
        };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(baseUrl, content);
        var resjson = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = JsonSerializer.Deserialize<OpenAIErrorResponseDto>(resjson);
            throw new System.Exception(errorResponse.Error.Message);
        }
        var data = JsonSerializer.Deserialize<OpenAIResponseDto>(resjson);
        var responseText = data.choices[0].message.content;

        return responseText;
    }
}
