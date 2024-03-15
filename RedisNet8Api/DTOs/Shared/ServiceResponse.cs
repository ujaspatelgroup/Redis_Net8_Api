namespace RedisNet8Api.DTOs.Shared
{
    public class ServiceResponse<T>
    {
        public T? data { get; set; }

        public bool Success { get; set; } = true;

        public string? Message { get; set; }
    }
}
