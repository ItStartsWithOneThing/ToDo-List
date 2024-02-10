
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDo_List.Models.DataBase.Entities;

namespace ToDo_List.Models.DataBase.Configuration
{
    public class TaskCardConfiguration : IEntityTypeConfiguration<TaskCard>
    {
        public void Configure(EntityTypeBuilder<TaskCard> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.EditedDate)
                    .HasColumnType("timestamp without time zone");

        }
    }
}
