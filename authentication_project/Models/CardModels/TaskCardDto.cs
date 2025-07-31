namespace authentication_project.Models.CardModels
{
    public class TaskCardDto
    {
        public int id { get; set; }
        public string? barcode { get; set; }
        public int? typeId { get; set; }
        public int? statusId { get; set; }
        public string? description { get; set; }
        public string? tailNo { get; set; }
        public bool? isCritical { get; set; }
        public DateTime createDate { get; set; }
        public string createUser { get; set; }
        public DateTime? updateDate { get; set; } = null!;
        public string? updateUser { get; set; }
        public bool isActive { get; set; }
    }
}
