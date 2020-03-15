using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public virtual DbSet<FileModel> FileModel { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options: options) { }
	}
}
