using ProfileEditor.Models;

namespace ProfileEditor.Services.Persons
{
    public interface IPersonsService
    {
        Task<IEnumerable<PersonVm>> GetPersonsAsync();
        Task<PersonVm?> GetPersonAsync(Guid id);
        Task<PersonVm> CreatePersonAsync(PersonVm person);
        Task<PersonVm> UpdatePersonAsync(PersonVm person);
        Task DeletePersonAsync(Guid id);
    }
}
