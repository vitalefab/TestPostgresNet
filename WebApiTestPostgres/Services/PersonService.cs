using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApiTestPostgres.Models;
using WebApiTestPostgres.Repositories;

namespace WebApiTestPostgres.Services;

public class PersonService
{
    private readonly AppDbContext _context;
    private readonly PersonRepository _personRepository;
    private readonly IConfiguration _configuration;

    public PersonService(AppDbContext context, PersonRepository personRepository, IConfiguration configuration)
    {
        _context = context;
        _personRepository = personRepository;
        _configuration = configuration;
    }

    //public async Task AddPersonAsync(Person person)
    //{
    //    _context.People.Add(person);
    //    await _context.SaveChangesAsync();
    //}

    public async Task AddPersonAsync(Person person)
    {

        bool useDapper = _configuration.GetValue<bool>("useDapper");

        if (useDapper)
        {
            await _personRepository.AddPersonAsync(person);
        }
        else
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync(); // Salva la persona per ottenere l'Id generato dal DB


            //SALVA GIA' TUTTO CORRETTAMENTE ANCHE GLI INDIRIZZI SENZA PASSARE DA QUA
            // Aggiorna PersonId negli indirizzi se presenti
            //if (person.Addresses != null && person.Addresses.Any())
            //{
            //    foreach (var address in person.Addresses)
            //    {
            //        address.PersonId = person.Id;  //Popola la FK
            //    }

            //    _context.Addresses.AddRange(person.Addresses); // Aggiunge gli indirizzi al context
            //    await _context.SaveChangesAsync(); // Salva gli indirizzi con la FK corretta
            //}
        }

    }
}
