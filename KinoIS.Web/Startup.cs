using KinoIs.Repository;
using KinoIs.Repository.Implementation;
using KinoIs.Repository.Interface;
using KinoIS.Domain;
using KinoIS.Domain.Models;
using KinoIS.Service;
using KinoIS.Service.Implementation;
using KinoIS.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoIS.Web
{
    public class Startup
    {
        private EmailSettings emailService;
        public Startup(IConfiguration configuration)
        {
            emailService = new EmailSettings();
            Configuration = configuration;
            Configuration.GetSection("EmailSettings").Bind(emailService);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<Microsoft.AspNetCore.Identity.IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();


            services.AddScoped(typeof(TicketRepository), typeof(TicketRepositoryImpl)); 
            services.AddScoped(typeof(ShoppingCartRepository), typeof(ShoppingCartRepositoryImpl));
            services.AddScoped(typeof(TicketInShoppingCartRepository), typeof(TicketInShoppingCartRepositoryImpl));
            services.AddScoped(typeof(CinemaUserRepository), typeof(CinemaUserRepositoryImpl));

            services.AddScoped(typeof(OrderRepository), typeof(OrderRepositoryImpl));
            services.AddTransient(typeof(KinoIS.Service.Interface.OrderService), typeof(KinoIS.Service.Implementation.OrderServiceImpl));
            services.AddScoped(typeof(EmailMessageRepository), typeof(EmailMessageRepositoryImpl));
            services.AddScoped(typeof(TicketInOrderRepository), typeof(TicketInOrderRepositoryImpl));

            services.AddScoped<EmailSettings>(es => emailService);
            services.AddScoped<EmailMessageService, EmailMessageServiceImpl>(email => new EmailMessageServiceImpl(emailService));
            services.AddScoped<BackgroundEmailSender, BackgroundEmailSenderImpl>();
            services.AddHostedService<ConsumeScopedHostedService>();

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddTransient<TicketInOrderService, Service.Implementation.TicketInOrderServiceImpl>();
            services.AddTransient<TicketService, Service.Implementation.TicketServiceImpl>();
            services.AddTransient<ShoppingCartService, Service.Implementation.ShoppingCartServiceImpl>();
            services.AddTransient<CinemaUserService, Service.Implementation.CinemaUserServiceImpl>();
            services.AddTransient<TicketInShoppingCartService, Service.Implementation.TicketInShoppingCartServiceImpl>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
