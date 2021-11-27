using Share.Entities;
using Share.Enums;
using System;
using System.Collections.Generic;

namespace Share.Dtos
{
    public class CreatorDto
    {
        public int IdUser {  get; set; } //Para valiadar usr
        public String Category1 {  get; set; }
        public String Category2 { get; set; }
        public string CreatorName { get; set; } //Datos creador
        public string NickName { get; set; }
        public string ContentDescription { get; set; }
        public string Biography {  get; set;}  
        public string YoutubeLink { get; set; }
        public string CreatorImage {  get; set; }
        public string CoverImage { get; set; }
        public DtoPagoCreador InfoPago {  get; set; }  

    }
}
