using Microsoft.EntityFrameworkCore;
using ProfileEditor.Data;
using ProfileEditor.Models;

namespace ProfileEditor.Services.Persons {
    public class PersonsDbService : IPersonsService {
        private readonly ApplicationDbContext _context;
        public PersonsDbService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<PersonVm> CreatePersonAsync(PersonVm person) {
            var asEntry = person.toEf();
            await _context.Persons.AddAsync(asEntry);
            await _context.SaveChangesAsync();
            return PersonVm.fromEf(asEntry);
        }

        public async Task DeletePersonAsync(Guid id) {
            var person = await _context.Persons.FindAsync(id);
            if (person != null) {
                _context.Remove(person);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PersonVm?> GetPersonAsync(Guid id) {
            var person = await _context.Persons.FindAsync(id);
            return person == null ? null : PersonVm.fromEf(person);
        }

        public async Task<IEnumerable<PersonVm>> GetPersonsAsync() {
            var persons = await _context.Persons.ToListAsync();
            return persons.Select(p => PersonVm.fromEf(p));
        }

        public async Task<PersonVm> UpdatePersonAsync(PersonVm person) {
            var entity = await _context.Persons.FindAsync(person.Id);
            if (entity == null) {
                return await CreatePersonAsync(person);
            }

            entity = person.UpdateEf(entity);

            _context.Update(entity);
            await _context.SaveChangesAsync();

            return PersonVm.fromEf(entity);
        }
    }
}
