using BusinessAccessLayer.Dto;

namespace BusinessAccessLayer.Services
{
    public interface iPetService
    {
        Task AddPet(PetDto petDto);

        Task<PetDto> GetPetbyId(int petId);

        Task<List<PetDto>> GetPets(FilterPetDto filter);

        Task UpdatePet(PetDto pet, int petId);

        Task DeletePet(int id);

        Task<List<PetDto>> GetMyPets();
    }
}