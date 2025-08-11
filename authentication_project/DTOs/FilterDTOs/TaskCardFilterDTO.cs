namespace authentication_project.DTOs.FilterDTOs
{
    public class TaskCardFilterDTO
    {
        public int id { get; set; }

        public string? barcode { get; set; }
        public int? typeId { get; set; }
        public int? statusId { get; set; }
        public string? description { get; set; }
        public string? tailNo { get; set; }
        public bool? isCritical { get; set; }
        public string createUser { get; set; }
        public string? updateUser { get; set; }
    }
}
