using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace News.WebApplication.Models
{
    public class NoticiaViewModel
    {
        [DisplayName("Código")]
        public int Id { get; set; }

        [DisplayName("Título")]
        public string Titulo { get; set; }

        [DataType(DataType.MultilineText)]        
        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Data de Publicação")]        
        public DateTime DataPublicacao { get; set; }

        [DisplayName("Autor")]
        public string Autor { get; set; }
    }
}
