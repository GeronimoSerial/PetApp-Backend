using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Infrastucture.Dto
{
    public class AddPetDto
    {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public DateTime Birthday { get; set; }
            public string Breed { get; set; }
            public IFormFile Image { get; set; }

    }
}