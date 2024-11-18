using FreelancerPlatform.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Infratructure.Entityframework
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RequirementServiceByFreelancer>().HasOne(x => x.Freelancer).WithMany(x => x.RequirementServiceByFreelancers).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<RequirementServiceByFreelancer>().HasOne(x => x.ServiceForFreelancer).WithMany(x => x.RequirementServiceByFreelancers).HasForeignKey(x => x.ServiceForFreelancerId).OnDelete(DeleteBehavior.NoAction);


            builder.Entity<FreelancerCategory>().HasOne(x => x.Freelancer).WithMany(x => x.FreelancerCategories).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<FreelancerCategory>().HasOne(x => x.Category).WithMany(x => x.FreelancerCategories).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<FreelancerCategory>().HasKey(x => new { x.FreelancerId, x.CategoryId });

            builder.Entity<FreelancerCategory>().HasOne(x => x.Freelancer).WithMany(x => x.FreelancerCategories).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<FreelancerCategory>().HasOne(x => x.Category).WithMany(x => x.FreelancerCategories).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<FreelancerCategory>().HasKey(x => new { x.FreelancerId, x.CategoryId });

            builder.Entity<JobSkill>().HasOne(x => x.Job).WithMany(x => x.JobSkills).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<JobSkill>().HasOne(x => x.SKill).WithMany(x => x.JobSkills).HasForeignKey(x => x.SkillId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<JobSkill>().HasKey(x => new { x.JobId, x.SkillId });


            builder.Entity<RolePermission>().HasOne(x => x.Role).WithMany(x => x.RolePermissions).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<RolePermission>().HasOne(x => x.Permission).WithMany(x => x.RolePermissions).HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<RolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });


            builder.Entity<Apply>().HasOne(x => x.Freelancer).WithMany(x => x.Applies).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Apply>().HasOne(x => x.Job).WithMany(x => x.Applies).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Report>().HasOne(x => x.Freelancer).WithMany(x => x.Reports).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FavoriteJob>().HasOne(x => x.Freelancer).WithMany(x => x.FavoriteJobs).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<FavoriteJob>().HasOne(x => x.Job).WithMany(x => x.FavoriteJobs).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<FavoriteJob>().HasKey(x => new { x.FreelancerId, x.JobId });

            builder.Entity<PotentialFreelancer>().HasOne(x => x.Freelancer).WithMany(x => x.PotentialFreelancers).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<PotentialFreelancer>().HasKey(x => x.Id);

            builder.Entity<Offer>().HasOne(x => x.Job).WithMany(x => x.Offers).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);
			builder.Entity<Offer>().HasOne(x => x.Freelancer).WithMany(x => x.Offers).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Job>().HasOne(x => x.Category).WithMany(x => x.Jobs).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);


            builder.Entity<SystemManagementRole>().HasKey(x => new {x.SystemManagementId, x.RoleId});
          //  builder.Entity<SystemManagementRole>().HasOne(x => x.SystemManagement).WithMany(x => x.SystemManagementRoles).HasForeignKey(x => x.SystemManagementId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<SystemManagementRole>().HasOne(x => x.Role).WithMany(x => x.SystemManagementRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.NoAction);

			builder.Entity<FreelancerSkill>().HasKey(x => new { x.FreelancerId, x.SkillId });
			builder.Entity<FreelancerSkill>().HasOne(x => x.SKill).WithMany(x => x.FreelancerSkills).HasForeignKey(x => x.SkillId).OnDelete(DeleteBehavior.NoAction);
			builder.Entity<FreelancerSkill>().HasOne(x => x.Freelancer).WithMany(x => x.FreelancerSkills).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Chat>().HasOne(x => x.HubChat).WithMany(x => x.Chats).HasForeignKey(x => x.HubChatId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Chat>().HasOne(x => x.Freelancer).WithMany(x => x.Chats).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>().HasOne(x => x.Freelancer).WithMany(x => x.Notifications).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Transaction>().HasOne(x => x.Contract).WithMany(x => x.Transactions).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Transaction>().HasOne(x => x.Freelancer).WithMany(x => x.Transactions).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Post>().HasOne(x => x.Freelancer).WithMany(x => x.Posts).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>().HasOne(x => x.Freelancer).WithMany(x => x.Comments).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Comment>().HasOne(x => x.Post).WithMany(x => x.Comments).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SavePost>().HasOne(x => x.Post).WithMany(x => x.SavePosts).HasForeignKey(x => x.PostId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<SavePost>().HasOne(x => x.Freelancer).WithMany(x => x.SavePosts).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<LikeComment>().HasOne(x => x.Comment).WithMany(x => x.LikeComments).HasForeignKey(x => x.CommentId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<LikeComment>().HasOne(x => x.Freelancer).WithMany(x => x.LikeComments).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<RecentView>().HasOne(x => x.Freelancer).WithMany(x => x.RecentViews).HasForeignKey(x => x.FreelancerId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<RecentView>().HasOne(x => x.Job).WithMany(x => x.RecentViews).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<RecentView>().HasKey(x => new {x.FreelancerId, x.JobId});

            // builder.Entity<Contract>().HasOne(x => x.Job).WithMany(x => x.Contracts).HasForeignKey(x => x.JobId).OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(builder);
        }

        public DbSet<Apply> Applies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<FavoriteJob> FavoriteJobs { get; set; }
        public DbSet<Freelancer> Freelancers { get; set; }
        public DbSet<FreelancerCategory> FreelancerCategories { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PotentialFreelancer> PotentialFreelancers { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<RequirementServiceByFreelancer> RequirementServiceByFreelancers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ServiceForFreelancer> ServiceForFreelancers { get; set; }
        public DbSet<SystemManagement> SystemManagements { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SavePost> SavePosts { get; set; }
        public DbSet<LikePost> LikePosts { get; set; }
        public DbSet<LikeComment> LikeComments { get; set; }
    }
}
