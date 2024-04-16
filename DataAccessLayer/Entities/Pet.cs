
namespace DataAccessLayer.Entities{



    public class Pet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Birth { get; set; }


        
        public int? UserId {get; set;}

    
        public User? User {get; set;}

    }
}
