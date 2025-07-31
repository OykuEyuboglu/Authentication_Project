namespace authentication_project.Common
{
    public class Result
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; } = [];
        public string Message => string.Join("\n*", Messages).Trim();
        //setter ekle, messages.add(value) yapsın
        public DateTime? TimeStamp { get; set; } = DateTime.UtcNow;
        public int? Count { get; set; }
    }
    public class Result<T> : Result
    {
        public T? Data { get; set; }
    }
}
