namespace authentication_project.DTOs.Card
{
    public class CreateTaskCardDTO
    {
        public string? Barcode { get; set; }
        public int? TypeId { get; set; }
        public int? StatusId { get; set; }
        public string? Description { get; set; }
        public string? TailNo { get; set; }
        public bool? IsCritical { get; set; }
        public string CreateUser { get; set; }
    }
}
