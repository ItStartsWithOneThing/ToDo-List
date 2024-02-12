
using ToDo_List.Models.DataBase.Entities;
using ToDo_List.Models.Enums;

namespace ToDo_List.Models.DataBase.Seed
{
    public static class TaskCardSeedData
    {
        public static IEnumerable<TaskCard> GetData()
        {
            string basicText = "Lorem Ipsum is simply dummy text of the printing and typesetting" +
                " industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, " +
                "when an unknown printer took a galley of type and scrambled it to make a type specimen book." +
                " It has survived not only five centuries, but also the leap into electronic typesetting, remaining" +
                " essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing" +
                " Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including" +
                " versions of Lorem Ipsum.";

            var cards = new List<TaskCard>()
            {
                new ()
                {
                    Id = Guid.NewGuid(),
                    Title = "Dinner",
                    Text = "Order some pizza",
                    EditedDate = DateTime.Parse("2024-02-05 14:30:00"),
                    Completed = false,
                    BackgroundColor = "#bde0fe",
                    Priority = Priority.Medium
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Title = "Rent",
                    Text = "Pay rent for electricity and water. Also send electricity meter readings.",
                    EditedDate = DateTime.Parse("2024-02-10 09:17:00"),
                    Completed = false,
                    BackgroundColor = "#ffb5a7",
                    Priority = Priority.High
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Title = "Movies",
                    Text = "Movies that I should watch this winter: Harry Potter (all parts), Lord of the ring + Hobbit (all parts), ",
                    EditedDate = DateTime.Parse("2024-01-02 10:30:00"),
                    Completed = false,
                    BackgroundColor = "white",
                    Priority = Priority.Low
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Title = "Basic text for filling",
                    Text = basicText,
                    EditedDate = DateTime.Parse("2024-02-05 14:30:00"),
                    Completed = false,
                    BackgroundColor = "#d0f4de",
                    Priority = Priority.Medium
                }
            };

            return cards;
        }
    }
}
