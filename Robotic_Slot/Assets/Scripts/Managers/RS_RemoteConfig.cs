using UnityEngine;
using Unity.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.VisualScripting;
using Newtonsoft.Json;

public class RS_RemoteConfig : MonoBehaviour
{

    public struct userAttributes
    {

    }

    public struct appAttributes
    {
    }

    public static RS_remoteJson configResponseJson;
    private static TaskCompletionSource<bool> configFetched = new TaskCompletionSource<bool>();

    public static async Task<RS_remoteJson> GetConfigResponseJsonAsync()
    {
        await configFetched.Task;
        return configResponseJson;
    }
    async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    async Task Awake()
    {
        await InitializeRemoteConfigAsync();
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteConfig(ConfigResponse configResponse)
    {
        configResponseJson = JsonConvert.DeserializeObject<RS_remoteJson>( RemoteConfigService.Instance.appConfig.GetJson("Slot_Robotic"));
        configFetched.TrySetResult(true);
    }
}
