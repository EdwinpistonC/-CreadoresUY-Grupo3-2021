using Share.Entities;
using Share.Enums;
using System.Collections.Generic;

namespace Share.Dtos
{
    public class CreatorDto
    {
        public int IdUser {  get; set; } //Para valiadar usr
        public TipoCategory Category1 {  get; set; }
        public TipoCategory Category2 { get; set; }
        public string CreatorName { get; set; } //Datos creador
        public string NickName { get; set; }
        public string CreatorDescription { get; set; }
        public string YoutubeLink { get; set; }
        public string WelcomeMsg { get; set; }
        public ICollection<BasePlanDto> Plans { get; set; } 

    }
}
