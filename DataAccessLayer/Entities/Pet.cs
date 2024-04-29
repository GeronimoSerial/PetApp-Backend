
namespace DataAccessLayer.Entities
{



    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } //Dog or Cat, Perro o gato
        public DateTime Birthday { get; set; }
        public string Breed { get; set; }
        public string ImageUrl {get; set;}
        
        
        
        public string Sex { get; set; } //F or M 

        //[ForeignKey("UserId")]

        public int? UserId {get; set;}
        public User? User {get; set;}





    }
}
