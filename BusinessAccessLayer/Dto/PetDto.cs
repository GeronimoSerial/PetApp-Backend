﻿namespace BusinessAccessLayer.Dto
{
    public class PetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Birthday { get; set; }
        public string Breed { get; set; }
        public string ImageUrl {get; set;}
        public string Sex {get; set;}
        public int? UserId { get; set; }

    }
}