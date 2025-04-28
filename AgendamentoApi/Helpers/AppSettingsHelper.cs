namespace AgendamentoApi.Helpers;

public class AppSettingsHelper : IAppSettingsHelper
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AppSettingsHelper> _logger;

    public AppSettingsHelper(IConfiguration configuration, ILogger<AppSettingsHelper> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    public string GetRequiredSetting(string key)
    {
        var value = _configuration.GetValue<string>(key);
        if (string.IsNullOrEmpty(value))
        {
            _logger.LogError($"Missing required setting {key}");
            throw new Exception($"Missing required setting {key}");
        }
        
        return value;
    }
}