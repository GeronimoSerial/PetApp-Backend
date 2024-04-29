using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SeedRandomData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string[] petNames = new string[] {
                "Firulais", "Pelusa", "Sultán", "Coco", "Manchas", "Loki", "Rambo", "Nala", "Tobby", "Chispa",
                "Duquesa", "Ringo", "Rocko", "Simba", "Sasha", "Canela", "Lobo", "Muñeca", "Bambi", "Ruffo",
                "Princesa", "Rocky", "Trueno", "Nube", "Bigotes", "Fiona", "Chucky", "Pepe", "Kira", "Titan",
                "Arturito", "Churro", "Bruna", "Lucifer", "Rex", "Pancho", "Lulu", "Campeón", "Fily", "Michu",
                "Bomber", "Chocolate", "Blanquita", "Estrella", "Gordo", "Laika", "Reina", "Bombon", "Tango",
                "Limón", "Sombra", "Mili", "Maia", "Nemo", "Lana", "Pulgui", "Tobi", "Bruno", "Frida", "Chucho",
                "Moco", "Muffin", "Sugar", "Negra", "Principo", "Maya", "Niebla", "Orión", "Lila", "Akira", "Oso",
                "Perla", "Nayla", "Valentina", "Chispa", "Hachi", "Osito", "Juancho", "Cloe", "Cupido", "Panda",
                "Pinto", "Loba", "Piqui", "Loki", "Tarzan", "Rayo", "Vicho", "Huarac", "Ambar", "Lucas", "Azul",
                "Kiara", "Miel", "Marley", "Muñeca", "Lenteja", "Sammy", "Tiny", "Pelusita", "Ojitos", "Sparky"
                };
            string[] petTypes = new string[] {"Dog", "Cat"};
            string[] dogBreeds = new string[] {
                "Labrador Retriever", "Pastor Alemán", "Bulldog", "Beagle", "Boxer", "Rottweiler", "Poodle", "Chihuahua",
                "Dóberman", "Bulldog Francés", "Cocker Spaniel", "Caniche Toy", "Golden Retriever", "Schnauzer Miniatura",
                "Shih Tzu", "Yorkshire Terrier", "Bóxer", "Chow Chow", "Dálmata", "Maltés", "Basset Hound", "Husky Siberiano",
                "Mastín Napolitano", "San Bernardo", "Bullmastiff", "Akita", "Sabueso", "Salchicha", "Border Collie", "Pug",
                "Boston Terrier", "Shar Pei", "Staffordshire Bull Terrier", "Crestado Chino", "Ovejero Alemán", "Xoloitzcuintle",
                "Lhasa Apso", "Mastín Inglés", "Galgo Español", "Jack Russell Terrier", "Bóxer", "Schnauzer Gigante", "Bobtail",
                "Samoyedo", "Alaskan Malamute", "Weimaraner", "Gran Danés", "Dogo Argentino", "Pitbull Terrier"
                };
            string[] catBreeds = new string[] { 
                "Siamés", "Persa", "Maine Coon", "Bengalí", "Esfinge", "Ragdoll", "Exótico de Pelo Corto", "British Shorthair",
                "Birmano", "Sphynx", "Azul Ruso", "Munchkin", "Savannah", "Scottish Fold", "Snowshoe", "Selkirk Rex",
                "Oriental", "Balinés", "Somalí", "Tonkinés", "Korat", "American Curl", "Himalayo", "Burmés", "Habana",
                "Chartreux", "Cymric", "Devon Rex", "Cornish Rex", "Blanco Alemán", "Safari", "Kurilian Bobtail", "Manx",
                "Bombay", "Bosque de Noruega", "Sagrado de Birmania", "Peterbald", "Pixie-bob", "Van Turco", "NebeluNG",
                "Laperm", "Abisinio", "Australiano", "Maracaibo", "Toyger", "Bobtail Japonés", "Javalina", "Chausie", "Caracat",
                "California Spangled"
                };
           string[] petSexs = new string[] {"Male", "Female"};

            for (int i = 0; i < 100; i++)
            {
                Random random = new Random();
                var petName = petNames[random.Next(petNames.Length)];
                var petType = petTypes[random.Next(petTypes.Length)];
                var petBreed = petType == "Dog" ? dogBreeds[random.Next(dogBreeds.Length)] : catBreeds[random.Next(catBreeds.Length)];
                var petSex = petSexs[random.Next(petSexs.Length)];
                var petBirthday = DateTime.Now.AddYears(-2);
                var petImageUrl = Guid.NewGuid().ToString() + ".jpg";
                var petUserId = i + 1;

              

                migrationBuilder.InsertData(
                    table: "Pets",
                    columns: new[] {"Id", "Name", "Description", "Type", "Birthday", "Breed", "ImageUrl", "Sex", "UserId"},
                    values: new object[] { i + 7, petName, $"Description Pet {i}", petType, petBirthday, petBreed, petImageUrl, petSex, petUserId}
                );
            }


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
