using Infrastructure.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public virtual DbSet<FileModel> FileModel { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options: options) { }
	}
}
