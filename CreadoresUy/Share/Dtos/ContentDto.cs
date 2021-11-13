using Share.Entities;
using Share.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Share.Dtos
{
    public class ContentDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int IdCreator { get; set; }
        public string NickName { get; set; }


        public DateTime AddedDate { get; set; }
        public bool Draft { get; set; }
        public DateTime DatePublish { get; set; }
        public bool Public { get; set; }
        public string Compositor { get; set; }
        public string Link { get; set; }
        public string Img { get; set; }
        public TipoContent Type { get; set; }



        public ICollection<int> Plans { get; set; }
        public ICollection<TagDto> Tags { get; set; }


        public void ReduceContent()
        {
            Description = Description.Substring(0, 500);
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
            if (Draft == null)
            {
                Draft = false;
            }
            if (Public == null)
            {
                Public = false;
            }
            if (Compositor == null)
            {
                Compositor = "";
            }
            if (Link == null)
            {
                Link = "";
            }

            if (Img == null)
            {
                Img = "";
            }
            if (Plans == null)
            {
                Plans =new  Collection<int>();
            }
            if (Tags == null)
            {
                Tags = new Collection<TagDto>();
            }

        }
    }
}
