using Ynov_WorkShare_Server.Interfaces;
using Ynov_WorkShare_Server.Services;

namespace Ynov_WorkShare_Server.Extensions;

public static class ServiceExtension
{
    public static void RegisterAppServices(this IServiceCollection services) 
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChannelService, ChannelService>();
        services.AddScoped<IUserChannelService, UserChannelService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IFileService, FileService>();
    }
}