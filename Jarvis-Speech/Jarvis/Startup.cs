// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.12.2

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Jarvis.Bots;
using Jarvis.Dialogs;
using Jarvis.Domain.Interfaces;
using Jarvis.Answers;
using Jarvis.Cognitive;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.AI.Luis;

namespace Jarvis
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

            services.AddTransient<IAnswersService, AnswersService>();
            services.AddTransient<IAnswersFactory, AnswersFactory>();
            services.AddTransient<ILuisService, LuisService>();
            services.AddTransient<IQnAService, QnAService>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, DialogAndWelcomeBot<MainDialog>>();

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
