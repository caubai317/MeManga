using System.IO;
using AutoMapper;
using MeManga.Core.Business.Caching;
using MeManga.Core.Business.Filters;
using MeManga.Core.Business.IoC;
using MeManga.Core.Business.Models;
using MeManga.Core.Business.Services;
using MeManga.Core.Common.Constants;
using MeManga.Core.Common.Extensions;
using MeManga.Core.DataAccess;
using MeManga.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using MeManga.Core.DataAccess.Repositories.Base;
using MeManga.Core.Common.Helpers;
//using Orient.RMS.Api.Core.Business.Services;
using MeManga.Core.Business.Profiles;
using System.Linq;
using MeManga.Core.Common.Utilities;
using System.Collections.Generic;
using System;
using MeManga.Core.Entities.Enums;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace MeManga
{
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        public static IConfigurationRoot Configuration;

        public static IContainer container { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder2 = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            builder2.AddEnvironmentVariables();
            Configuration = builder2.Build();


            var logPath = Configuration["AppSettings:LoggingPath"] + "Orient-{Date}-" + System.Environment.MachineName + ".txt";
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Warning()
              .WriteTo.RollingFile(logPath, retainedFileCountLimit: 15)
              .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                  builder2 => builder2.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc().AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
              .AddJsonOptions(opt =>
              {
                  opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
              });

            services.AddSingleton(Configuration);
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSerilog(dispose: true);
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddFilter<SerilogLoggerProvider>(null, LogLevel.Trace);
            });
            services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>();

            //Config Automapper map
            Mapper.Initialize(config =>
            {
                config.AddProfile<BookProfile>();
                config.AddProfile<CommentProfile>();
                config.AddProfile<UserProfile>();
                config.AddProfile<ChapterProfile>();
                config.AddProfile<WriterProfile>();
                config.AddProfile<RoleProfile>();
                config.AddProfile<FilePathProfile>();
                config.AddProfile<TypeBookProfile>();
            });

            var conn = Configuration.GetConnectionString("DefaultConnectionString");
            services.AddDbContextPool<MeMangaNetCoreDbContext>(options => options.UseSqlServer(conn));

            //Register Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //Register Service
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISSOAuthService, SSOAuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IChapterService, ChapterService>();
            services.AddScoped<IWriterService, WriterService>();
            services.AddScoped<IFilePathService, FilePathService>();
            services.AddScoped<ITypeBookService, TypeBookService>();


            //Register MemoryCacheManager
            services.AddScoped<ICacheManager, MemoryCacheManager>();

            // Set Service Provider for IoC Helper
            IoCHelper.SetServiceProvider(services.BuildServiceProvider());

            services.AddMvc(option =>
            {
                option.Filters.Add<HandleExceptionFilterAttribute>();
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "MeManga API",
                    Description = "ASP.NET Core API.",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "DO DANG QUANG", Email = "dquangdn@gmail.com", Url = "" },
                });

                c.DescribeAllParametersInCamelCase();
                c.OperationFilter<AccessTokenHeaderParameterOperationFilter>();

                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "MeManga.xml");
                c.IncludeXmlComments(xmlPath);

                
            });

            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);

            var builder = new ContainerBuilder();

            builder.Populate(services);

            container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            // global policy - assign here or on each controller
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddSerilog();
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug(LogLevel.Debug);
            }
            else if (env.IsProduction())
            {
                loggerFactory.AddSerilog();
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug(LogLevel.Warning);
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orient API V1");
            });
            app.UseMvc();

            applicationLifetime.ApplicationStopped.Register(() => container.Dispose());

            // Auto run migration
            RunMigration(app);

            // Initialize Data
            InitDataRole();
            InitUserAdmin();
            InitUser();
            //InitCalendarType();
            //InitQuestionAnswer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        private void RunMigration(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<MeMangaNetCoreDbContext>().Database.Migrate();
            }
        }

        private void InitDataRole()
        {
            var roleRepository = IoCHelper.GetInstance<IRepository<Role>>();

            var roles = new[]
            {
                new Role {
                    Id = RoleConstants.ADMINId,
                    Name = "Admin"
                },
                new Role
                {
                    Id = RoleConstants.TRANSId,
                    Name = "Translator"
                },
                new Role
                {
                    Id = RoleConstants.CLIENTId,
                    Name = "Reader"
                }
            };

            roleRepository.GetDbContext().Roles.AddIfNotExist(x => x.Name, roles);
            roleRepository.GetDbContext().SaveChanges();
        }

        private void InitUserAdmin()
        {
            var userRepository = IoCHelper.GetInstance<IRepository<User>>();
            if (userRepository.GetAll().Count() > 1)
            {
                return;// It's already init
            }

            var user = new User();
            user.Name = "Admin";
            user.Email = "dquangdn@gmail.com";
            user.Mobile = "0702683177";

            var password = "quang123";
            password.GeneratePassword(out string saltKey, out string hashPass);

            user.Password = hashPass;
            user.PasswordSalt = saltKey;

            user.RoleId = RoleConstants.ADMINId;

            var users = new[]
            {
                user
            };

            userRepository.GetDbContext().Users.AddIfNotExist(x => x.Email, users);
            userRepository.GetDbContext().SaveChanges();
        }

        private void InitUser()
        {
            var userRepository = IoCHelper.GetInstance<IRepository<User>>();

            var userTrans = new User[] {
                new User() {
                    Email = "nguyen@gmail.com",
                    Name = "Khanh Nguyen",
                },
                new User() {
                    Email = "thanh@gmail.com",
                    Name = "Thanh Thanh",
                },
                new User() {
                    Email = "phuong@gmail.com",
                    Name = "Phuong Phuong",
                }
            };

            var userReaders = new User[] {
                new User() {
                    Email = "quan@gmail.com",
                    Name = "Quan quan",
                },
                new User() {
                    Email = "huynh@gmail.com",
                    Name = "Huynh Huynh",
                }
            };

            foreach (var user in userTrans)
            {
                user.RoleId = RoleConstants.TRANSId;
            }

            foreach (var user in userReaders)
            {
                user.RoleId = RoleConstants.CLIENTId;
            }

            var users = userTrans.Concat(userReaders).ToArray();

            foreach (var user in users)
            {
                user.Mobile = "0123456789";

                var password = "metruyen@123";
                password.GeneratePassword(out string saltKey, out string hashPass);

                user.Password = hashPass;
                user.PasswordSalt = saltKey;
            }

            userRepository.GetDbContext().Users.AddIfNotExist(x => x.Email, users);
            userRepository.GetDbContext().SaveChanges();
        }
    }
}
