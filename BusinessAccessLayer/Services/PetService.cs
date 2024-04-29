using AutoMapper;
using BusinessAccessLayer.Dto;
using DataAccessLayer.DatabaseContext;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusinessAccessLayer.Services
{
    // Service for handling operations related to pets
    public class PetService : iPetService
    {
        private readonly BootcampDbContext _db;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        // Constructor for initializing PetService with required dependencies
        public PetService(BootcampDbContext bootcampDbContext, IMapper mapper, ICurrentUserService currentUserService)
        {
            _db = bootcampDbContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        // Add a new pet to the database
        public async Task AddPet(PetDto petDto)
        {
            var userId = _currentUserService.UserId;
            var Username = _currentUserService.Username;

            if (userId == 0 || Username == string.Empty)
            {
                throw new Exception("User required");
            }
            var pet = new Pet
            {
                Name = petDto.Name,
                Birthday = petDto.Birthday,
                Description = petDto.Description,
                Type = petDto.Type,
                Breed = petDto.Breed,
                UserId = petDto.UserId,
                ImageUrl = petDto.ImageUrl

            };

            _db.Pets.Add(pet);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving changes in pet service AddPet method", ex);
            }
        }

        // Delete a pet from the database by ID
        public async Task DeletePet(int id)
        {
            try
            {
                var pet = await _db.Pets.FirstOrDefaultAsync(x => x.Id == id);
                if (pet == null) throw new Exception("Pet not found");
                _db.Pets.Remove(pet);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting pet in pet service DeletePet method", ex);
            }
        }

        // Update a pet in the database with new information
        public async Task UpdatePet(PetDto petDto, int petId)
        {
            try
            {
                var pet = await _db.Pets.FirstOrDefaultAsync(x => x.Id == petId);

                if (pet == null) throw new Exception("Pet not found");

                pet.Name = petDto.Name;
                pet.Birthday = petDto.Birthday;
                pet.Description = petDto.Description;
                pet.Type = petDto.Type;
                pet.Breed = petDto.Breed;

                _db.Pets.Update(pet);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating pet in pet service UpdatePet method", ex);
            }
        }

        // Get a pet by its ID from the database
        public async Task<PetDto> GetPetbyId(int petId)
        {
            if (petId == 0) throw new Exception("Pet Id required");

            try
            {
                var pet = await _db.Pets.FirstOrDefaultAsync(x => x.Id == petId);
                var petDto = new PetDto();

                if (pet != null)
                    petDto = new PetDto
                    {
                        Name = pet.Name,
                        Description = pet.Description,
                        Birthday = pet.Birthday,
                        Type = pet.Type,
                        Breed = pet.Breed,
                        UserId = pet.UserId,
                        ImageUrl = pet.ImageUrl

                    };
                var serviceResult = new ServiceResult<PetDto>()
                {
                    IsSuccess = pet != null,
                    Message = pet == null ? "Pet not found" : "",
                    Data = petDto
                };

                return petDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Get a list of all pets from the database
        public async Task<List<PetDto>> GetPets(FilterPetDto filter)
        {
            try
            {
                var pets = await _db.Pets
                    .Where(p => 
                        (string.IsNullOrEmpty(filter.Type) || (p.Type == filter.Type))
                        && 
                        (string.IsNullOrEmpty(filter.Breed) || (p.Breed == filter.Breed))
                        &&
                        (string.IsNullOrEmpty(filter.Sex) || (p.Sex == filter.Sex))
                    )
                    //Add Pagination
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();
                var petDtos = new List<PetDto>();

                // Mapping pets to PetDto using AutoMapper
                petDtos = _mapper.Map<List<PetDto>>(pets);
                return petDtos;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Add a method GetMyPets that returns the pets of the logged-in user.
        public async Task<List<PetDto>> GetMyPets()
        {   
            try
            {
                var userId = _currentUserService.UserId;
                //filter pets by user id
                var pets = await _db.Pets.Where(x => x.UserId == userId).ToListAsync();
                var petDtos = new List<PetDto>();

                // Mapping pets to PetDto using AutoMapper
                petDtos = _mapper.Map<List<PetDto>>(pets);
                
                var serviceResult = new ServiceResult<List<PetDto>>()
                {
                    IsSuccess = pets != null,
                    Message = pets == null ? "No pets found" : "Pets found",
                    Data = petDtos
                };

                return petDtos;

            }
            
            catch (Exception ex)
            {
                throw new Exception("Error getting pets in pet service GetMyPets method", ex);
            }
        }


    }
}
