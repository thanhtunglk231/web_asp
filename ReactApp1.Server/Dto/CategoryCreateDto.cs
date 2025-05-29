namespace ReactApp1.Server.Dto
{
    public class CategoryCreateDto
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
