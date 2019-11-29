using Autofac;
using BackManager.Domain;
using BackManager.Utility;
using BackManager.Utility.Filter;
using BackManager.Utility.Middleware;
using BackManager.Utility.Middleware.ErrorMiddleware;
using BackManager.Utility.Tool.Swagger;
using BackManager.WebApi.Signal;
using BackManager.WebApi.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnitOfWork;

namespace BackManager.WebApi
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;


            StaticConstraint.Init(s => configuration[s]);

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region CORS

            //跨域第二种方法，声明策略，记得下边app中配置

            services.AddCors(c =>
            {
                //一般采用这种方法
                c.AddPolicy("LimitRequests", policy =>
                {
                    // 支持多个域名端口，注意端口号后不要带/斜杆：比如localhost:8000/，是错的
                    // 注意，http://127.0.0.1:1818 和 http://localhost:1818 是不一样的，尽量写两个
                    policy
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();
                });
            });

            #endregion
            #region redis
            services.AddDistributedServiceStackRedisCache(options =>
            {
                Configuration.GetSection("redis").Bind(options);
                if (!IPAddress.TryParse(options.Host, out IPAddress ip))
                {
                    options.Host = Dns.GetHostAddressesAsync(options.Host)
                    .Result.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork).ToString();
                }
            });
            #endregion

            #region 允许同步读取
            // 解决错误  Synchronous operations are disallowed. Call WriteAsync or set AllowSynchronousIO to true instead
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.AllowSynchronousIO = true; //允许同步读取
            //});
            #endregion
            #region 表单重复提交

            services.AddFormRepeatSubmitIntercept(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.FormUniqueIdentification = "BmForm";
                options.FormRepeatSubmitReturnValue = JsonConvert.SerializeObject(ApiResult<string>.Error($"当前表单重复提交"));

            });
            #endregion
            //services.AddControllers();

            // 或者将Controller加入到Services中
            services.AddControllersWithViews(options =>
            {
                //数据验证过滤器
                options.Filters.Add<DataValidationActionFilter>();
                options.Filters.Add<DataValidationActionFilter>();

            }).AddControllersAsServices().AddNewtonsoftJson(options =>
            {

                //JSON首字母小写解决
                options.SerializerSettings.ContractResolver =
                    new Newtonsoft.Json.Serialization.DefaultContractResolver();

                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            #region swagger
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                //swagger中控制请求的时候发是否需要在url中增加BmForm
                options.OperationFilter<AddAuthTokenHeaderParameter>();

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                //Determine base path for the application.  
                //Set the comments path for the swagger json and ui.  
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BackManager.Common.DtoModel.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "BackManager.WebApi.xml"));


            });
            #endregion

            services.AddDbContext<UnitOfWorkDbContext>(options =>
            {
                /*
                code first   ，那就是利用vs2017工具->Nuget程序包管理->程序包管理控制台
                PM >
                执行如下命令：
                enable - migrations
                add - migration foo
                update - database
                关于名字foo，就是给要迁移的数据进行起个标识名字
                */
                if (Configuration.GetConnectionString("DbType").ToEnum<DbType>() == DbType.SqlServer)
                {
                    options.UseSqlServer(Configuration.GetConnectionString("SqlServerConneceftionString"));
                }
                else
                {
                    options.UseMySql(Configuration.GetConnectionString("MySqlConnectionString"));
                }
            });
            services.AddSignalR();
            //services.AddDbContext<UnitOfWorkDbContext>(options =>
            //   options.UseMySql("Data Source=localhost;port=3306;Initial Catalog=magicadmin;uid=root;password=123456;")
            //   );
        }
        //注册AOP
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<MyAutofacModule>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //dotnet publish -c Release -f netcoreapp2.2 -o  ./test 发布 -c 模式 -f 模板框架 -o 指定文件夹
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            #region swagger
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            #endregion
            app.UseRouting();

            app.UseAuthorization();

            //表单重复提交
            //app.UseFormRepeatSubmitIntercept();
            app.UseCors("LimitRequests");//将 CORS 中间件添加到 web 应用程序管线中, 以允许跨域请求。

            app.UseMiddleware<MyErrorMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SysHub>("/chathub");
                endpoints.MapControllers();
            });
        }
    }


}
