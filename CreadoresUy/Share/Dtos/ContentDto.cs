using Share.Entities;
using Share.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Share.Dtos
{
    public class ContentDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int IdCreator { get; set; }
        public string NickName { get; set; }

        public string CreatorImage { get; set; }

        public DateTime AddedDate { get; set; }
        public bool IsPublic { get; set; }
        public string Dato { get; set; }
        public bool Draft { get; set; }
        public DateTime PublishDate { get; set; }
        public TipoContent Type { get; set; }



        public ICollection<int> Plans { get; set; }
        public ICollection<TagDto> Tags { get; set; }

        public void ReduceContent()
        {
            int tam = 500;
            if(Description.Length < tam) { tam = Description.Length; }
            Description = Description.Substring(0, tam);
        }
        public void NoNulls()
        {
            if (NickName == null) NickName = "";
            if (Title == null)
            {
                Title = "";
            }
            if (Description == null)
            {
                Description = "";
            }
            if (IsPublic == null)
            {
                IsPublic = false;
            }
            if (Plans == null)
            {
                Plans =new  Collection<int>();
            }
            if (Tags == null)
            {
                Tags = new Collection<TagDto>();
            }

            if (CreatorImage == null)
            {
                CreatorImage = "";
            }

            if (Dato == null)
            {
                Dato = string.Empty;
            }

        }
    }
}
