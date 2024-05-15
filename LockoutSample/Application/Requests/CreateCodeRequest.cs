using System.ComponentModel.DataAnnotations;

namespace LockoutSample.Application.Requests
{
    public class CreateCodeRequest
    {
        [Required, Range(0, int.MaxValue)]
        public int UserId { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Code { get; set; }
    }
}
