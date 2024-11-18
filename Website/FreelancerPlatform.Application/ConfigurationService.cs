using FluentValidation;
using FreelancerPlatform.Application.Abstraction;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.ServiceImplementions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IFreelancerService, FreelancerService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISkillService, SkillService>();
            services.AddTransient<IPotientSerivice, PotientService>();
            services.AddTransient<IJobService, JobService>();
            services.AddTransient<IFavoriteJobService, FavoriteJobService>();
            services.AddTransient<IApplyService, ApplyService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IContractService, ContractService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ISavePostService, SavePostService>();
            services.AddTransient<ILikePostService, LikePostService>();
            services.AddTransient<ISystemManagementService, SystemManagementService>();
            services.AddTransient<ILikeCommentService, LikeCommentService>();
            /*services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IApplyService, ApplyService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IContractService, ContractService>();
            services.AddTransient<IJobService, JobService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IRecruiterService, RecruiterService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IRequirementServiceByFreelancerService, RequirementServiceByFreelancerService>();
            services.AddTransient<IRequirementServiceByRecruiterService, RequirementServiceByRecruiterService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IServiceForFreelancerService, ServiceForFreelancerService>();
            services.AddTransient<IServiceForRecruiterService, ServiceForRecruiterService>();
            services.AddTransient<ISystemManagementService, SystemManagementService>();
			services.AddTransient<IPotientSerivice, PotientService>();
			services.AddTransient<IFavoriteJobService, FavoriteJobService>();*/

            return services;
        }
    }
}
