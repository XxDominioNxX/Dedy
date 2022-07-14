using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Dedy.Models
{
    public class Product
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        [Display(Name = "Наименование")]
        public string Name { get; set; }



        [Display(Name = "Тип")]
        public string Type { get; set; }



        [Display(Name = "Модель")]
        public string Model { get; set; }



        [Display(Name = "Ориентация")]
        public string Orientation { get; set; }



        [Display(Name = "Размер")]
        public string Size { get; set; }



        [Display(Name = "Цвет")]
        public string Color { get; set; }



        [Display(Name = "Описание")]
        public string Description { get; set; }



        [Display(Name = "Артикул")]
        public string Articul { get; set; }


        
        [Range(0, int.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public int Price { get; set; }






        public string ImageId { get; set; } // ссылка на изображение

        public bool HasImage()
        {
            return !String.IsNullOrWhiteSpace(ImageId);
        }
    }
}
