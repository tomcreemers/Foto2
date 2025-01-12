using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Controls;
using Plugin.LocalNotification;


namespace Foto2.ViewModels
{
    public class AIChatPageViewModel : BaseViewModel
    {
        private const string OpenAIApiKey = "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"; // Vervang dit door een veilige opslagmethode
        private const string OpenAIApiUrl = "https://api.openai.com/v1/chat/completions";

        public ObservableCollection<ChatMessage> Messages { get; set; }
        public ICommand SendPromptCommand { get; }

        public AIChatPageViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            SendPromptCommand = new Command<string>(async (prompt) => await HandlePrompt(prompt));
        }

        private async Task HandlePrompt(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Messages.Add(new ChatMessage { Text = "Please enter a prompt!", IsUser = false });
                });
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(new ChatMessage { Text = prompt, IsUser = true });
            });

            string aiResponse = await FetchAIResponse(prompt);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(new ChatMessage { Text = aiResponse, IsUser = false });

                // Trigger phone vibration
                Vibration.Vibrate(TimeSpan.FromMilliseconds(200));

                // Trigger a notification (optional customization)
                App.Current.MainPage.DisplayAlert("Picture Helper", "AI has responded!", "OK");
            });
        }

        private async Task<string> FetchAIResponse(string prompt)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {OpenAIApiKey}");

                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                        new { role = "system", content = "You are the Picture Helper AI. Always answer in 10 words or less." },
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 50,
                    temperature = 0.7
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(OpenAIApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return $"Error: {response.StatusCode} - {errorContent}";
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<OpenAIResponse>(responseBody, options);

                var aiResponse = result?.Choices?[0]?.Message?.Content?.Trim() ?? "AI returned no response.";

                // Trigger Local Notification
                ShowLocalNotification(aiResponse);

                return aiResponse;
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
        }


        private void ShowLocalNotification(string message)
        {
            var notification = new NotificationRequest
{
    NotificationId = new Random().Next(1000, 9999), // Unique ID for the notification
    Title = "AI Chat Update",
    Description = "The Picture Helper AI has responded!",
    Schedule = new NotificationRequestSchedule // Schedule for immediate notification
    {
        NotifyTime = DateTime.Now.AddSeconds(5), // Schedule for 5 second in the future
    }
};

// Display the notification
LocalNotificationCenter.Current.Show(notification);

// Log the notification
Console.WriteLine($"Notification sent: {notification.Description}");
        }

        private class OpenAIResponse
        {
            [JsonPropertyName("choices")]
            public List<OpenAIChoice> Choices { get; set; }
        }

        private class OpenAIChoice
        {
            [JsonPropertyName("message")]
            public OpenAIMessage Message { get; set; }
        }

        private class OpenAIMessage
        {
            [JsonPropertyName("content")]
            public string Content { get; set; }
        }
    }

    public class ChatMessage
    {
        public string Text { get; set; }
        public bool IsUser { get; set; }
    }
}
