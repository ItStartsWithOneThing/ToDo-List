
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.DataBase.Seed;

namespace ToDo_List.Models.DataBase.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.TaskCards)
                .WithOne(x => x.User);

            builder.HasMany(x => x.RefreshSessions)
                .WithOne(x => x.User);

            builder.HasData(UserSeedData.GetData());
        }
    }
}
