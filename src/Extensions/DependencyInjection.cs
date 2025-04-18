/*
* Author: Steve Bang
* History:
* - [2025-04-19] - Created by mrsteve.bang@gmail.com
*/

using Microsoft.EntityFrameworkCore;
using Steve.ManagerHero.UserService.Infrastructure;
using Steve.ManagerHero.UserService.Infrastructure.Repository;
using FluentValidation;
using Steve.ManagerHero.Application.Features.Users.Commands;
using Steve.ManagerHero.UserService.Application.Auth;
using Steve.ManagerHero.UserService.Infrastructure.Services;

namespace Steve.ManagerHero.UserService.Extensions;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddCoreServices(this IHostApplicationBuilder builder)
    {
        // Add project settings
        builder.AddProjectSettings();

        // Add http context
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IIdentityService, IdentityService>();

        builder.Services.AddDbContext<UserAppContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        );

        builder.EnrichNpgsqlDbContext<UserAppContext>();

        // Add the migration service. When the application starts, it will check if the database is up to date. 
        // If not, it will run the migration to update the database.
        builder.Services.AddMigration<UserAppContext>();

        // Add the MediatR services
        builder.Services.AddMediatR(config =>
        {
            // Register all the handlers from the current assembly
            config.RegisterServicesFromAssemblyContaining<Program>();

            // Register the ValidationBehavior
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        // Register validator
        builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();

        // Register smtp setting
        builder.AddSmtpSettings();

        // Register repositories
        builder.AddRepositories();

        return builder;
    }

    public static IHostApplicationBuilder AddRepositories(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ISessionRepository, SessionRepository>();

        return builder;
    }

    public static IHostApplicationBuilder AddSmtpSettings(this IHostApplicationBuilder builder)
    {
        var smtpSetting = builder.Configuration.GetSection("SMTP");
        if (!smtpSetting.Exists())
        {
            throw new NotImplementedException("smtp section is missing in the appsettings.json file.");
        }

        var smtpSettingValue = smtpSetting.Get<SmtpSettings>();

        if (smtpSettingValue == null)
        {
            throw new NotImplementedException("smtp section is missing in the appsettings.json file.");
        }
        builder.Services.AddSingleton(smtpSettingValue);

        builder.Services.AddScoped<IEmailService, EmailService>();

        return builder;
    }

    public static IHostApplicationBuilder AddProjectSettings(this IHostApplicationBuilder builder)
    {
        var projectSetting = builder.Configuration.GetSection(nameof(ProjectSettings));
        if (!projectSetting.Exists())
        {
            throw new NotImplementedException("ProjectSettings section is missing in the appsettings.json file.");
        }

        var projectSettingValue = projectSetting.Get<ProjectSettings>();

        if (projectSettingValue == null)
        {
            throw new NotImplementedException("ProjectSettings section is missing in the appsettings.json file.");
        }
        builder.Services.AddSingleton(projectSettingValue);

        return builder;
    }

}