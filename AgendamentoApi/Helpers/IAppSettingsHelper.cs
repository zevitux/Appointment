namespace AgendamentoApi.Helpers;

public interface IAppSettingsHelper
{
    string GetRequiredSetting(string key);
}