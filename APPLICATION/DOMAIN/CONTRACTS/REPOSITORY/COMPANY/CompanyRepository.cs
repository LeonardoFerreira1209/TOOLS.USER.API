using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.BASE;
using APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.PERSON;
using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST.COMPANY;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.ENTITY.COMPANY;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.UTILS.EXTENSIONS;
using APPLICATION.INFRAESTRUTURE.CONTEXTO;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.COMPANY;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.COMPANY;

public class CompanyRepository : BaseRepository, ICompanyRepository
{
    private readonly Contexto _contexto;

    private readonly IOptions<AppSettings> _appSettings;

    public CompanyRepository(IOptions<AppSettings> appSettings, Contexto contexto) : base(appSettings)
    {
        _contexto = contexto;

        _appSettings = appSettings;
    }

    #region EF Core
    /// <summary>
    /// Create a Company
    /// </summary>
    /// <param name="personFastRequest"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<(bool success, CompanyEntity company)> Create(CompanyRequest companyRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(CompanyRepository)} - METHOD {nameof(Create)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando empresa no banco de dados.\n");

            // Add company in database.
            var entityEntry = await _contexto.Companies.AddAsync(companyRequest.ToEntity(userId)); await _contexto.SaveChangesAsync();

            // return true value and company.
            return (true, entityEntry.Entity);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun false and a null value.
            return (false, null);
        }
    }

    /// <summary>
    /// Get person for Id
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="withDependencies"></param>
    /// <returns></returns>
    public async Task<(bool success, PersonEntity person)> Get(Guid personId, bool withDependencies)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(Get)}\n");

        try
        {
            // Get person for Id without dependencies.
            if (withDependencies)
            {
                var person = await _contexto.Persons
                            // Include user in Person.
                            .Include(person => person.User)
                            // Include list of contacts in Person. 
                            .Include(person => person.Contacts)
                            // Include company in Person.
                            .Include(person => person.Company)
                            // Return one Person when Id queal a pernsoId.
                            .AsSplitQuery().FirstOrDefaultAsync(person => person.Id.Equals(personId));

                // Return person.
                return (person is not null, person);
            }
            else // Get person for Id with dependencies.
            {
                var person = await _contexto.Persons
                                        .FirstOrDefaultAsync(person => person.Id.Equals(personId));

                // Return person.
                return (person is not null, person);
            }
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Return null value.
            return (false, null);
        }
    }


    /// <summary>
    /// étodo responsável por recuperar o Id de uma pessoa pelo userId.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<(bool success, Guid personId)> GetIdWithUserId(Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(GetIdWithUserId)}\n");

        try
        {
            var personId = _contexto.Persons
                                    .FirstOrDefaultAsync(person => person.UserId.Equals(userId)).Result.Id;
            // Return person.
            return (!Guid.Empty.Equals(personId), await Task.FromResult(personId));
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Returnguid empty.
            return (false, Guid.Empty);
        }
    }

    /// <summary>
    /// Método responsavel por recuperar todas as pessoas.
    /// </summary>
    /// <returns></returns>
    public async Task<(bool success, IEnumerable<PersonEntity> persons)> GetAll(bool withDependencies)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(GetAll)}\n");

        try
        {
            // Get person for Id with dependencies.
            if (withDependencies)
            {
                var persons = await _contexto.Persons
                            // Include user in Person.
                            .Include(person => person.User)
                            // Include list of contacts in Person. 
                            .Include(person => person.Contacts)
                            // Include company in Person.
                            .Include(person => person.Company)
                            // Return an list of persons.
                            .AsSplitQuery().ToListAsync();

                // Return person.
                return (persons is not null, persons);
            }
            else // Get person for Id without dependencies.
            {
                var persons = await _contexto.Persons.ToListAsync();

                // Return person.
                return (persons is not null && persons.Any() is true, persons);
            }
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Return null value.
            return (false, null);
        }
    }

    /// <summary>
    /// Complete register person
    /// </summary>
    /// <param name="personFullRequest"></param>
    /// <returns></returns>
    public async Task<(bool success, PersonEntity person)> CompleteRegister(PersonFullRequest personFullRequest)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(CompleteRegister)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Atualizando dados do usuário.\n");

            // Update person in database.
            var entityEntry = _contexto.Persons.Update(personFullRequest.ToEntity()); await _contexto.SaveChangesAsync();

            // Return true and person value.
            return (true, entityEntry.Entity);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.Message}\n");

            // return false and null value.
            return (false, null);
        }
    }

    /// <summary>
    /// Add image profile in person
    /// </summary>
    /// <param name="person"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    public async Task<(bool success, string imageUri)> ProfileImage(PersonEntity person, string imageUri)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(ProfileImage)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando imagem no banco.\n");

            // Set image in person. / Update Person with image. / Save the changes.
            person.ImageUri = imageUri; _contexto.Persons.Update(person); await _contexto.SaveChangesAsync();

            // Return true and person value.
            return (true, person.ImageUri);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // return false and null value.
            return (false, null);
        }
    }
    #endregion
}
