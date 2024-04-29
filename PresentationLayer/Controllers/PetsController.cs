using BusinessAccessLayer.Dto;
using BusinessAccessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Infrastucture.Dto;

namespace PresentationLayer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly iPetService _petService;

        public PetsController(iPetService petService)
        {
            _petService = petService;
        }

        //CRUD 

   
        //Get my pets - Endpoint que devuelve las mascotas del usuario logueado
        [HttpGet("GetMyPets")]
        public async Task<ActionResult> GetMyPets()
        {
            try
            {
                var result = await _petService.GetMyPets();
                var serviceResult = new ServiceResult<List<PetDto>>()
                {
                    IsSuccess = result != null,
                    Message = result == null ? "No pets found" : "Pets found",
                    Data = result
                };
                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                throw new ArgumentException();
            }
        }
         
        // Method to retrieve all pets
        [HttpGet("GetPets")]
        public async Task<ActionResult> GetPets([FromQuery]FilterPetDto filter)
        {
            try
            {
                // Call the pet service to get all pets
                var result = await _petService.GetPets(filter);
        
                // Create a service result object to hold the outcome of the operation
                var serviceResult = new ServiceResult<List<PetDto>>()
                {
                    IsSuccess = result != null, // Set IsSuccess based on whether result is not null
                    Message = result == null ? "No pets found" : "Pets found", // Set ErrorMessage based on the presence of result
                    Data = result  // Set the Data property to the retrieved result
                };
        
                // Return the service result as an HTTP response
                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a BadRequest response with the exception message
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPet/{petId}")]
        public async Task<ActionResult> GetPetbyId(int petId)
        {
            try
            {
                var pet = await _petService.GetPetbyId(petId);
                return Ok(pet);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Crear endpoint para devolver imagen de mascota dado el nombre de la imagen
        [HttpGet("GetPetImage/{imageName}")]
        public async Task<ActionResult> GetPetImage(string imageName)
        {
            try
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", imageName);
                if(!System.IO.File.Exists(imagePath))
                {
                    return NotFound("Image not found");
                }
                
                var image = System.IO.File.OpenRead(imagePath);
                return File(image, "image/jpeg");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddPet")]
        public async Task<ActionResult> AddPet([FromForm]AddPetDto petDto)
        {
            //1- Imagen a base de datos: byte[]
            //2- Imagen a servidor: Guardar en carpeta


            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(petDto.Image.FileName);

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);
            
            //Verificar que la carpeta exista (required)
            if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")); 
            }
           
            
            //Guardar imagen en carpeta
            using (var stream = System.IO.File.Create(imagePath)) 
            {
                await petDto.Image.CopyToAsync(stream);
            }



            try
            {
                var pet = new PetDto
                {
                    Name = petDto.Name,
                    Description = petDto.Description,
                    Birthday = petDto.Birthday,
                    Type = petDto.Type,
                    Breed = petDto.Breed,
                    ImageUrl = fileName
                };
                await _petService.AddPet(pet);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePet/{petId}")]
        public async Task<ActionResult> UpdatePet(PetDto pet, int petId)
        {
            try
            {
                await _petService.UpdatePet(pet, petId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePet/{petId}")]
        public async Task<ActionResult> DeletePet(int petId)
        {
            try
            {
                await _petService.DeletePet(petId);
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}