using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class CreatGenerDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
