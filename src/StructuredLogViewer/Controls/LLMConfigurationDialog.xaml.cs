using System;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using StructuredLogViewer.Dialogs;
using StructuredLogViewer.LLM;
using StructuredLogViewer.LLM.GitHubCopilot.Authentication;
using StructuredLogViewer.LLM.GitHubCopilot.Client;
using StructuredLogViewer.LLM.GitHubCopilot.Configuration;
using StructuredLogViewer.LLM.GitHubCopilot.Models;

namespace StructuredLogViewer.Controls
{
    public partial class LLMConfigurationDialog : Window
    {
        private bool isApiKeyVisible = false;

        public string Endpoint { get; private set; }
        public string Model { get; private set; }
        public string ApiKey { get; private set; }
        public bool AutoSendOnEnter { get; private set; }
        public bool AgentMode { get; private set; }

        public LLMConfigurationDialog(LLMConfiguration currentConfig)
        {
            InitializeComponent();

            // Pre-populate with current configuration
            if (currentConfig != null)
            {
                endpointTextBox.Text = currentConfig.Endpoint ?? "";
                
                // Always start with editable textbox
                modelComboBox.IsEditable = true;
                modelComboBox.Text = currentConfig.ModelName ?? "";
                
                if (!string.IsNullOrWhiteSpace(currentConfig.ApiKey))
                {
                    apiKeyPasswordBox.Password = currentConfig.ApiKey;
                    apiKeyTextBox.Text = currentConfig.ApiKey;
                }
                
                autoSendOnEnterCheckBox.IsChecked = currentConfig.AutoSendOnEnter;
                agentModeCheckBox.IsChecked = currentConfig.AgentMode;
            }
            else
            {
                // Default to editable textbox
                modelComboBox.IsEditable = true;
            }

            // Focus on first empty field
            Loaded += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(endpointTextBox.Text))
                    endpointTextBox.Focus();
                else if (string.IsNullOrWhiteSpace(modelComboBox.Text))
                    modelComboBox.Focus();
                else
                    apiKeyPasswordBox.Focus();
            };
        }

        private bool IsGitHubCopilotEndpoint(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                return false;
                
            return endpoint.Contains("githubcopilot.com", StringComparison.OrdinalIgnoreCase) ||
                   endpoint.Equals("github-copilot", StringComparison.OrdinalIgnoreCase) ||
                   endpoint.Equals("copilot", StringComparison.OrdinalIgnoreCase);
        }

        private void ToggleApiKeyVisibility_Click(object sender, RoutedEventArgs e)
        {
            isApiKeyVisible = !isApiKeyVisible;

            if (isApiKeyVisible)
            {
                // Show plain text
                apiKeyTextBox.Text = apiKeyPasswordBox.Password;
                apiKeyPasswordBox.Visibility = Visibility.Collapsed;
                apiKeyTextBox.Visibility = Visibility.Visible;
                toggleApiKeyButton.Content = "🙈";
            }
            else
            {
                // Show password box
                apiKeyPasswordBox.Password = apiKeyTextBox.Text;
                apiKeyTextBox.Visibility = Visibility.Collapsed;
                apiKeyPasswordBox.Visibility = Visibility.Visible;
                toggleApiKeyButton.Content = "👁";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            Endpoint = endpointTextBox.Text?.Trim();
            Model = modelComboBox.IsEditable ? modelComboBox.Text?.Trim() : (modelComboBox.SelectedItem as string)?.Trim();
            ApiKey = isApiKeyVisible ? apiKeyTextBox.Text?.Trim() : apiKeyPasswordBox.Password?.Trim();
            AutoSendOnEnter = autoSendOnEnterCheckBox.IsChecked ?? true;
            AgentMode = agentModeCheckBox.IsChecked ?? true;

            if (string.IsNullOrWhiteSpace(Endpoint))
            {
                MessageBox.Show("Please enter an endpoint URL.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                endpointTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(Model))
            {
                MessageBox.Show("Please enter or select a model/deployment name.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                modelComboBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ApiKey))
            {
                // Check if it's GitHub Copilot - API key is optional for OAuth flow
                var isGitHubCopilot = Endpoint?.Contains("github", StringComparison.OrdinalIgnoreCase) == true ||
                                      Endpoint?.Equals("github-copilot", StringComparison.OrdinalIgnoreCase) == true;
                
                if (!isGitHubCopilot)
                {
                    MessageBox.Show("Please enter an API key.", "Validation Error", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    if (isApiKeyVisible)
                        apiKeyTextBox.Focus();
                    else
                        apiKeyPasswordBox.Focus();
                    return;
                }
            }

            DialogResult = true;
        }

        private async void GitHubLoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                githubLoginButton.IsEnabled = false;
                githubLoginButton.Content = "⏳ Authenticating...";

                GitHubDeviceCodeDialog deviceDialog = null;

                // Create device code callback
                Action<string, string> deviceCodeCallback = (userCode, verificationUrl) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        deviceDialog = new GitHubDeviceCodeDialog(userCode, verificationUrl);
                        deviceDialog.Owner = this;
                        deviceDialog.Show();
                    });
                };

                // Start authentication
                var authenticator = new GitHubDeviceFlowAuthenticator(deviceCodeCallback);
                var githubToken = await authenticator.AuthenticateAsync();

                // Close device dialog on success
                if (deviceDialog != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        deviceDialog.CloseWithSuccess();
                    });
                }

                // Set the token in the API key field
                if (isApiKeyVisible)
                {
                    apiKeyTextBox.Text = githubToken;
                }
                else
                {
                    apiKeyPasswordBox.Password = githubToken;
                }

                // Try to fetch models from GitHub Copilot API
                githubLoginButton.Content = "⏳ Loading models...";
                bool modelsLoaded = await TryLoadGitHubCopilotModelsAsync(githubToken);
                
                if (modelsLoaded)
                {
                    githubLoginButton.Content = "✓ Logged In";
                }
                else
                {
                    // If models couldn't be loaded, keep textbox editable
                    modelComboBox.IsEditable = true;
                    if (string.IsNullOrWhiteSpace(modelComboBox.Text))
                    {
                        modelComboBox.Text = "claude-sonnet-4.5";
                    }
                    githubLoginButton.Content = "✓ Logged In";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"GitHub authentication failed:\n\n{ex.Message}",
                    "Authentication Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                githubLoginButton.Content = "🔑 GitHub Login";
                githubLoginButton.IsEnabled = true;
            }
        }

        private async Task<bool> TryLoadGitHubCopilotModelsAsync(string githubToken)
        {
            try
            {
                // Create token provider to get Copilot token
                var tokenProvider = new GitHubCopilotTokenProvider(githubToken, CopilotAccountType.Individual);
                var copilotToken = await tokenProvider.GetCopilotTokenAsync();
                
                // Create HTTP client
                using var httpClient = new GitHubCopilotHttpClient(tokenProvider, null);
                
                // Fetch models from API
                var request = httpClient.CreateRequest(HttpMethod.Get, "models");
                var response = await httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"Models endpoint returned: {response.StatusCode}");
                    return false;
                }
                
                var json = await response.Content.ReadAsStringAsync();
                var modelsResponse = System.Text.Json.JsonSerializer.Deserialize<ModelsResponse>(json);
                
                if (modelsResponse?.Data == null || modelsResponse.Data.Count == 0)
                {
                    return false;
                }
                
                // Filter models: only include those enabled by policy and model picker
                var availableModels = modelsResponse.Data
                    .Where(m => m.ModelPickerEnabled && 
                               m.Policy != null && 
                               m.Policy.State == "enabled")
                    .ToList();
                
                if (availableModels.Count == 0)
                {
                    return false;
                }
                
                // Successfully fetched models - populate dropdown
                Dispatcher.Invoke(() =>
                {
                    modelComboBox.Items.Clear();
                    
                    foreach (var model in availableModels)
                    {
                        modelComboBox.Items.Add(model.Id);
                    }
                    
                    // Select default model (prefer Claude Sonnet 4.5)
                    if (modelComboBox.Items.Contains("claude-sonnet-4.5"))
                    {
                        modelComboBox.SelectedItem = "claude-sonnet-4.5";
                    }
                    else if (modelComboBox.Items.Count > 0)
                    {
                        modelComboBox.SelectedIndex = 0;
                    }
                    
                    modelComboBox.IsEditable = false;
                });
                
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load GitHub Copilot models: {ex.Message}");
                return false;
            }
        }
    }
}
