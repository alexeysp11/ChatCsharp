using WokflowLib.Authentication.AuthBL;
using WokflowLib.Authentication.Models.ConfigParameters;
using WokflowLib.Authentication.Models.NetworkParameters;

namespace Chat.AuthenticationService.AuthenticationServiceBL;

/// <summary>
/// Chat authentication resolver.
/// </summary>
public class ChatAuthResolver
{
    /// <summary>
    /// Authentication resolver from WokflowLib.
    /// </summary>
    private IAuthResolver AuthResolver { get; set; }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ChatAuthResolver()
    {
        var configHelper = new ConfigHelper();
        var settings = new AuthResolverSettings
        {
            CheckUCConfig = configHelper.GetUCConfigs(),
            AuthDBSettings = configHelper.GetAuthDBSettings(usersTableName: "chat_users")
        };
        AuthResolver = new AuthResolverDB(settings);
    }

    /// <summary>
    /// 
    /// </summary>
    public UserCreationResult AddUser(UserCredentials request)
    {
        return AuthResolver.AddUser(request);
    }

    /// <summary>
    /// 
    /// </summary>
    public VUCResponse VerifyUserCredentials(UserCredentials request)
    {
        return AuthResolver.VerifyUserCredentials(request);
    }
}
