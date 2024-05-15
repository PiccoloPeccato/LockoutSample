namespace LockoutSample.Domain.Entities
{
    public class LockCode
    {
        public int UserId { get; set; }
        public int Code { get; set; }
        public byte Attempts { get; set; }
        public DateTime? LockoutEnd { get; set; }
    }
}
