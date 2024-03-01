
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase.Configuration
{
    public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
    {
        public void Configure(EntityTypeBuilder<RefreshSession> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ExpiresIn)
                .HasColumnType("timestamp without time zone");

            builder.Property(x => x.CreatedAt)
                .HasColumnType("timestamp without time zone");

            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshSessions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
