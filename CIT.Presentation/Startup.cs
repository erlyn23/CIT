using CIT.BusinessLogic.Contracts;
using CIT.BusinessLogic.Services;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.DbContexts;
using CIT.DataAccess.Repositories;
using CIT.Dtos.Profiles;
using CIT.Presentation.Filters;
using CIT.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Presentation
{
    public class Startup
    {
        private readonly string _myCors = "CITCors";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            string connectionString = Configuration.GetConnectionString("CITConnection");
            services.AddDbContext<CentroInversionesTecnocorpDbContext>(builder =>
            {
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddAutoMapper(typeof(DtosProfiles));

            var secretKey = Encoding.ASCII.GetBytes(Configuration["SecretKey"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearer=>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = true;
                bearer.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };
            });

            services.AddCors(builder => 
            {
                builder.AddPolicy(_myCors, policy =>
                {
                    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });

            //Dependency Injections
            services.AddScoped<AuthFilter>();
            services.AddScoped<ExceptionFilter>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();
            services.AddScoped<IEntitesInfoRepository, EntitiesInfoRepository>();
            services.AddScoped<IEntitiesInfoService, EntitiesInfoService>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IPageService, PagesService>();
            services.AddScoped<IOperationRepository, OperationRepository>();
            services.AddScoped<IOperationService, OperationService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IUserAddressService, UserAddressService>();
            services.AddScoped<ILenderRoleRepository, LenderRoleRepository>();
            services.AddScoped<ILenderAddressRepository, LenderAddressRepository>();
            services.AddScoped<ILenderBusinessRepository, LenderBusinessRepository>();
            services.AddScoped<ILenderBusinessService, LenderBusinessService>();
            services.AddScoped<ILenderRoleService, LenderRoleService>();
            services.AddScoped<ILenderAddressService, LenderAddressService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IVehicleAssignmentRepository, VehicleAssignmentRepository>();
            services.AddScoped<IVehicleAssignmentService, VehicleAssignmentService>();
            services.AddScoped<ILoanService,LoanService>();
            services.AddScoped<ILoanRepository,LoanRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUsersLenderBusinessesRepository, UsersLenderBusinessesRepository>();
            services.AddTransient<TokenCreator>();
            services.AddTransient<EmailTools>();
            services.AddTransient<AccountTools>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseCors(_myCors);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Index}/{id?}");
            });
        }
    }
}
