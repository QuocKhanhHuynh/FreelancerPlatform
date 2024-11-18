using FreelancerPlatform._Admin;
using FreelancerPlatform.Application;
using FreelancerPlatform.Infratructure.Entityframework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddApplicationService();
builder.Services.AddPresentService();
builder.Services.AddInfrastructureEntityFrameworkService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "index",
    pattern: "trang-chu",
    defaults: new { controller = "Home", action = "Index" }
);

app.MapControllerRoute(
    name: "login",
    pattern: "dang-nhap",
    defaults: new { controller = "Acount", action = "Login" }
);

app.MapControllerRoute(
    name: "logout",
    pattern: "dang-xuat",
    defaults: new { controller = "Acount", action = "Login" }
);

app.MapControllerRoute(
    name: "admin",
    pattern: "quan-tri-vien",
    defaults: new { controller = "Acount", action = "GetAdmin" }
);

app.MapControllerRoute(
    name: "createAdmin",
    pattern: "them-tai-khoan",
    defaults: new { controller = "Acount", action = "CreateAdmin" }
);

app.MapControllerRoute(
    name: "freelancer",
    pattern: "freelancer",
    defaults: new { controller = "Acount", action = "GetFreelancer" }
);

app.MapControllerRoute(
    name: "category",
    pattern: "linh-vuc",
    defaults: new { controller = "Category", action = "GetCategory" }
);

app.MapControllerRoute(
    name: "createCategory",
    pattern: "them-linh-vuc",
    defaults: new { controller = "Category", action = "CreateCategory" }
);

app.MapControllerRoute(
    name: "createCategory",
    pattern: "sua-linh-vuc/{id}",
    defaults: new { controller = "Category", action = "UpdateCategory" }
);


app.MapControllerRoute(
    name: "skill",
    pattern: "ky-nang",
    defaults: new { controller = "Category", action = "GetSkill" }
);

app.MapControllerRoute(
    name: "createSkill",
    pattern: "them-ky-nang",
    defaults: new { controller = "Category", action = "CreateSkill" }
);

app.MapControllerRoute(
    name: "updateSkill",
    pattern: "sua-ky-nang/{id}",
    defaults: new { controller = "Category", action = "UpdateSkill" }
);

app.MapControllerRoute(
    name: "deleteSkill",
    pattern: "xoa-ky-nang/{id}",
    defaults: new { controller = "Category", action = "DeleteSkill" }
);

app.MapControllerRoute(
    name: "report",
    pattern: "bao-cao",
    defaults: new { controller = "Report", action = "GetReport" }
);

app.MapControllerRoute(
    name: "transaction",
    pattern: "giao-dich",
    defaults: new { controller = "Transaction", action = "GetTransaction" }
);

app.MapControllerRoute(
    name: "detailTransaction",
    pattern: "chi-tiet-giao-dich/{id}",
    defaults: new { controller = "Transaction", action = "GetTransactionDetail" }
);

app.MapControllerRoute(
    name: "verifyPayment",
    pattern: "xac-thuc-thanh-toan",
    defaults: new { controller = "Acount", action = "GetVerifyPayment" }
);

app.MapControllerRoute(
    name: "verifyPaymentDetail",
    pattern: "chi-tiet-xac-thuc-thanh-toan/{id}",
    defaults: new { controller = "Acount", action = "GetVerifyPaymentDetail" }
);





app.Run();
