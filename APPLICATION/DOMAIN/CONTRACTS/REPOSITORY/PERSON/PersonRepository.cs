﻿using APPLICATION.DOMAIN.DTOS.CONFIGURATION;
using APPLICATION.DOMAIN.DTOS.REQUEST.PEOPLE;
using APPLICATION.DOMAIN.DTOS.REQUEST.PERSON;
using APPLICATION.DOMAIN.ENTITY.PERSON;
using APPLICATION.DOMAIN.UTILS.PERSON;
using APPLICATION.INFRAESTRUTURE.CONTEXTO;
using APPLICATION.INFRAESTRUTURE.REPOSITORY.PERSON;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace APPLICATION.DOMAIN.CONTRACTS.REPOSITORY.PERSON;

public class PersonRepository : IPersonRepository
{
    private readonly Contexto _contexto;

    private readonly IOptions<AppSettings> _appSettings;

    public PersonRepository(IOptions<AppSettings> appSettings, Contexto contexto)
    {
        _contexto = contexto;

        _appSettings = appSettings;
    }

    public async Task<(bool success, Person person)> Create(PersonFastRequest personFastRequest, Guid userId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(Create)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando usuário no banco de dados.\n");

            // Add user in database.
            var entityEntry = await _contexto.Persons.AddAsync(personFastRequest.ToEntity(userId)); await _contexto.SaveChangesAsync();

            // return true value and person.
            return (true, entityEntry.Entity);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // retun false and a null value.
            return (false, null);
        }
    }

    public async Task<(bool success, Person person)> Get(Guid personId)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(Create)}\n");

        try
        {
            // Get person for Id.
            var person = await _contexto.Persons
                // Include user in Person.
                .Include(person => person.User)
                // Include list of contacts in Person. 
                .Include(person => person.Contacts)
                // Include list of professions in Person.
                .Include(person => person.Professions)
                // Return one Person when Id queal a pernsoId.
                .FirstOrDefaultAsync(person => person.Id.Equals(personId));

            // Return person.
            return (true, person);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // Return null value.
            return (false, null);
        }
    }

    public async Task<(bool success, Person person)> CompleteRegister(PersonFullRequest personFullRequest)
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
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // return false and null value.
            return (false, null);
        }
    }

    public async Task<(bool success, byte[] image)> ProfileImage(Person person, byte[] image)
    {
        Log.Information($"[LOG INFORMATION] - SET TITLE {nameof(PersonRepository)} - METHOD {nameof(ProfileImage)}\n");

        try
        {
            Log.Information($"[LOG INFORMATION] - Adicionando imagem no banco.\n");

            // Set image in person. / Update Person with image. / Save the changes.
            person.Image = image; _contexto.Persons.Update(person); await _contexto.SaveChangesAsync();

            // Return true and person value.
            return (true, person.Image);
        }
        catch (Exception exception)
        {
            Log.Error($"[LOG ERROR] - {exception.InnerException} - {exception.Message}\n");

            // return false and null value.
            return (false, null);
        }
    }
}