using ApiCore.JsonFilter;
using ApiCore.Utils;
using AspNet.Security.OAuth.Validation;
using LogCore.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using PluginCore.Basic;
using SchoolServer.DefaultService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchoolServer
{
    public class Startup
    {
        public IConfiguration config { get; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            config = configuration;
        }
        private PluginCoreContextImpl applicationContext = null;
        public void ConfigureServices(IServiceCollection services)
        {
            LogLevels logLevel = LogLevels.Info;
            int maxDays = 7;
            IConfigurationSection logConfig = config.GetSection("Log");
            string maxFileSize = "10MB";
            if (logConfig != null)
            {
                Enum.TryParse(logConfig["Level"] ?? "", out logLevel);
                int.TryParse(logConfig["SaveDays"], out maxDays);
                maxFileSize = logConfig["MaxFileSize"];
                if (string.IsNullOrEmpty(maxFileSize))
                {
                    maxFileSize = "10MB";
                }
            }
            string logFolder = Path.Combine(AppContext.BaseDirectory, "Logs");
            LoggerManager.InitLogger(new LogConfig
            {
                LogBaseDir = logFolder,
                MaxFileSize = maxFileSize,
                LogLevels = logLevel,
                IsAsync = true,
                LogFileTemplate = LogFileTemplates.PerDayDirAndLogger,
                LogContentTemplate = LogLayoutTemplates.SimpleLayout
            });
            LoggerManager.SetLoggerAboveLevels(logLevel);
            LoggerManager.StartClear(maxDays, logFolder, LoggerManager.GetLogger("clear"));

            services.AddSingleton(config as IConfigurationRoot);
            ////services.AddMvc().AddViewComponentsAsServices();
            services.AddControllersWithViews().AddJsonOptions(options => {
                //设置时间格式
                options.JsonSerializerOptions.Converters.Add(new DateJsonConverter("yyyy-MM-dd HH:mm:ss", DateTimeZoneHandling.Local));
                //不使用驼峰样式的key
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                //不使用驼峰样式的key
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //获取或设置要在转义字符串时使用的编码器
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }).AddNewtonsoftJson(options => {
                //options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                //options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            }); ;
            services.AddCors();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddJwtBearer(OAuthValidationDefaults.AuthenticationScheme, options =>
            {
                options.Authority = config["AuthUrl"];
                options.RequireHttpsMetadata = false;
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnAuthenticationFailed = (context) =>
                    {
                        var msg = context.Exception;
                        //throw msg;
                        return Task.CompletedTask;
                    }
                };
                //注册应用的AppName值
                options.Audience = config["Audience"];
            });
            //services.AddAuthorization();
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", option =>
            //    {
            //        option.Authority = config["AuthUrl"];
            //        option.RequireHttpsMetadata = false;
            //        option.Audience = config["Audience"];
            //    });

            applicationContext = services.AddPlugin(options =>
            {
                options.ConnectionString = config["ConnectionStrings:DefaultConnection"];
                options.AuthUrl = config["AuthUrl"];
                options.ClientID = config["ClientID"];
                options.ClientSecret = config["ClientSecret"];
            });

            var apppart = services.FirstOrDefault(x => x.ServiceType == typeof(ApplicationPartManager))?.ImplementationInstance;
            var assemblys = new List<Assembly>() { Assembly.GetExecutingAssembly() };
            if (apppart != null)
            {
                ApplicationPartManager apm = apppart as ApplicationPartManager;
                //所有附件程序集
                PluginCoreContextImpl ac = PluginCoreContext.Current as PluginCoreContextImpl;
                ac.AdditionalAssembly.ForEach((a) =>
                {
                    assemblys.Add(a);
                    apm.ApplicationParts.Add(new AssemblyPart(a));
                });
            }
            // 注入日志http
            services.AddUserDefined();

           
            services.AddSwaggerGen(c =>
            {
                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoreWebApi", Version = "v1" });
                //按Http类型排序
                c.OrderActionsBy(o => o.GroupName);
                
                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var pluginPath = Path.Combine(basePath, "Plugin");
                c.CustomSchemaIds(a => a.FullName);
                //找到下面的所有xml文件
                DirectoryInfo dir = new DirectoryInfo(pluginPath);
                FileInfo[] fil = dir.GetFiles();
                ////List<string> pluginsXMLPath = new List<string>();
                foreach (var item in fil)
                {
                    //获取xml文件
                    if (item.Name.EndsWith("Plugin.xml"))
                        c.IncludeXmlComments(item.FullName);
                }

                var securityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter the JWT Bearer token in format: Bearer {token}",
                    Name = "authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }

                };
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securityScheme, new List<string>() }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
            })
            .AddMvcCore()
            .AddApiExplorer();
            services.AddAutoMapper(assemblys);

            // 在AutoMapper之后
            applicationContext.Init().Wait();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new LoggerProvider());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreWebApi");
                options.RoutePrefix = string.Empty;
            });
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                //options.AllowAnyOrigin();
                options.SetIsOriginAllowed(c => true);
                options.AllowCredentials();
            });


            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Check}/{action=Index}/{id?}");
            });

            //app.UseMvcWithDefaultRoute();
            if (applicationContext != null)
            {
                applicationContext.ApplicationBuilder = app;
            }
        }
    }
}
