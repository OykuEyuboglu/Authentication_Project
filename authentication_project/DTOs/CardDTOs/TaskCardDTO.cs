namespace authentication_project.DTOs.CardDTOs
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
    }

}
