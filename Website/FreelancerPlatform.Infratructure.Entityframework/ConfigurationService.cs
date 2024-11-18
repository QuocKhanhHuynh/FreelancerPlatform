using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Infratructure.Entityframework.Implementions.Repository;
using FreelancerPlatform.Infratructure.Entityframework.Implementions.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Infratructure.Entityframework
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddInfrastructureEntityFrameworkService(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IApplyRepository, ApplyRepository>();
            services.AddTransient<ISkillRepository, SkillRepository>();
            services.AddTransient<IFreelancerSkilRepository, FreelancerSkillRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IFavoriteJobRepository, FavoriteJobRepository>();
            services.AddTransient<IFreelancerRepository, FreelancerRepository>();
            services.AddTransient<IFreelancerCategoryRepository, FreelancerCategoryRepository>();
            services.AddTransient<IJobRepository, JobRepository>();
            services.AddTransient<IOfferRepository, OfferRepository>();
            services.AddTransient<IPermissionRepository, PermissionRepository>();
            services.AddTransient<IPotientialFreelancerRepository, PotientialFreelancerRepository>();
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IRequirementServiceByFreelancerRepository, RequirementServiceByFreelancerRepository>();
            services.AddTransient<IRolePermissionRepository, RolePermissionRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IServiceForFreelancerRepository, ServiceForFreelancerRepository>();
            services.AddTransient<ISystemManagementRepository, SystemManagementRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ISystemManagementRoleRepository, SystemManagementRoleRepository>();
            services.AddTransient<IJobSkillRepository, JobSkillRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<IHubChatRepository, HubChatRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ISavePostRepository, SavePostRepository>();
            services.AddTransient<ILikePostRepository, LikePostRepository>();
            services.AddTransient<ILikeCommentRepository, LikeCommentRepository>();
            services.AddTransient<IRecentViewRepository, RecentViewRepository>();

            return services;
        }
    }
}
