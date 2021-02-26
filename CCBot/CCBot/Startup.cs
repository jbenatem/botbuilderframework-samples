// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.11.1

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using CCBot.Bots;
using CCBot.Domain.Interfaces;
using CCBot.Core;
using CCBot.Answer;
using Microsoft.Bot.Builder.AI.QnA;
using CCBot.Core.CognitiveServices;
using Microsoft.Bot.Builder.AI.Luis;
using CCBot.Dialog.Dialogs;

namespace CCBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();

            // Create the User state. (Used in this bot's Dialog implementation.)
            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();

            // The MainDialog that will be run by the bot.
            services.AddSingleton<MainDialog>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, EchoBot>();
            services.AddTransient<IOrchestratorService, OrchestratorService>();
            services.AddTransient<ILuisService, LuisService>();
            services.AddTransient<IQnAService, QnAService>();
            services.AddTransient<IAnswersService, AnswersService>();
            services.AddTransient<IAnswersFactory, AnswersFactory>();

            services.AddSingleton(new QnAMakerEndpoint
            {
                KnowledgeBaseId = Configuration["QnAKnowledgebaseId"],
                EndpointKey = Configuration["QnAEndpointKey"],
                Host = Configuration["QnAEndpointHostName"]
            });

            services.AddSingleton(new LuisApplication
            {
                ApplicationId = Configuration["LuisAppId"],
                EndpointKey = Configuration["LuisAPIKey"],
                Endpoint = "https://" + Configuration["LuisAPIHostName"]
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
