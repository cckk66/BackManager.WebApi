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

            //����ڶ��ַ������������ԣ��ǵ��±�app������

            services.AddCors(c =>
            {
                //һ��������ַ���
                c.AddPolicy("LimitRequests", policy =>
                {
                    // ֧�ֶ�������˿ڣ�ע��˿ںź�Ҫ��/б�ˣ�����localhost:8000/���Ǵ��
                    // ע�⣬http://127.0.0.1:1818 �� http://localhost:1818 �ǲ�һ���ģ�����д����
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

            #region ����ͬ����ȡ
            // �������  Synchronous operations are disallowed. Call WriteAsync or set AllowSynchronousIO to true instead
            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.AllowSynchronousIO = true; //����ͬ����ȡ
            //});
            #endregion
            #region ���ظ��ύ

            services.AddFormRepeatSubmitIntercept(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.FormUniqueIdentification = "BmForm";
                options.FormRepeatSubmitReturnValue = JsonConvert.SerializeObject(ApiResult<string>.Error($"��ǰ���ظ��ύ"));

            });
            #endregion
            //services.AddControllers();

            // ���߽�Controller���뵽Services��
            services.AddControllersWithViews(options =>
            {
                //������֤������
                options.Filters.Add<DataValidationActionFilter>();
                options.Filters.Add<DataValidationActionFilter>();

            }).AddControllersAsServices().AddNewtonsoftJson(options =>
            {

                //JSON����ĸСд���
                options.SerializerSettings.ContractResolver =
                    new Newtonsoft.Json.Serialization.DefaultContractResolver();

                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });

            #region swagger
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                //swagger�п��������ʱ���Ƿ���Ҫ��url������BmForm
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
                code first   ���Ǿ�������vs2017����->Nuget���������->������������̨
                PM >
                ִ���������
                enable - migrations
                add - migration foo
                update - database
                ��������foo�����Ǹ�ҪǨ�Ƶ����ݽ��������ʶ����
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
        //ע��AOP
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<MyAutofacModule>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //dotnet publish -c Release -f netcoreapp2.2 -o  ./test ���� -c ģʽ -f ģ���� -o ָ���ļ���
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

            //���ظ��ύ
            //app.UseFormRepeatSubmitIntercept();
            app.UseCors("LimitRequests");//�� CORS �м����ӵ� web Ӧ�ó��������, �������������

            app.UseMiddleware<MyErrorMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SysHub>("/chathub");
                endpoints.MapControllers();
            });
        }
    }


}
