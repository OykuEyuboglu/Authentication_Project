namespace authentication_project.Models.CardModels
{
    public class TaskCardModel
    {
        public int Id { get; set; }
        public string? Barcode { get; set; }
        public int? TypeId { get; set; }
        public int? StatusId { get; set; }
        public string? Description { get; set; }
        public string? TailNo { get; set; }
        public bool? IsCritical { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; } = null!;
        public string? UpdateUser { get; set; }
        public bool IsActive { get; set; }
    }
}
