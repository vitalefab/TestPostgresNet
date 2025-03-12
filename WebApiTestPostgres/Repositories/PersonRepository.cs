using System;
using System.Collections.Generic;
using WebApiTestPostgres.Models;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;


namespace WebApiTestPostgres.Repositories;

public class PersonRepository
{
    private readonly DapperContext _context;

    public PersonRepository(DapperContext context)
    {
        _context = context;
    }

    //public async Task AddPersonAsync(Person person)
    //{
    //    var query = "INSERT INTO Persons (Name) VALUES (@Name) RETURNING Id";
    //    using var connection = _context.CreateConnection();
    //    int personId = await connection.ExecuteScalarAsync<int>(query, new { person.Name });

    //    foreach (var address in person.Addresses)
    //    {
    //        var addressQuery = "INSERT INTO Addresses (Street, City, PersonId) VALUES (@Street, @City, @PersonId)";
    //        await connection.ExecuteAsync(addressQuery, new { address.Street, address.City, PersonId = personId });
    //    }
    //}

    public async Task AddPersonAsync(Person person)
    {
        using var connection = _context.CreateConnection();
        connection.Open(); // Apri la connessione

        using var transaction = connection.BeginTransaction(); // Inizia una transazione per l'atomicità

        try
        {
            // Inserisci la persona e ottieni l'ID generato
            var query = "INSERT INTO \"Person\" (\"Name\") VALUES (@Name) RETURNING \"Id\""; //Usa i nomi delle colonne corretti
            int personId = await connection.ExecuteScalarAsync<int>(query, new { person.Name }, transaction: transaction);

            // Inserisci gli indirizzi associati alla persona
            foreach (var address in person.Addresses)
            {
                var addressQuery = "INSERT INTO \"Addresses\" (\"Street\", \"City\", \"PersonId\") VALUES (@Street, @City, @PersonId)";
                await connection.ExecuteAsync(addressQuery, new { address.Street, address.City, PersonId = personId }, transaction: transaction);
            }

            transaction.Commit(); // Conferma la transazione se tutto è andato a buon fine
        }
        catch (Exception ex)
        {
            transaction.Rollback(); // Annulla la transazione in caso di errore
            throw; // Rilancia l'eccezione per gestirla a livello superiore
        }
        finally
        {
            if (connection.State == ConnectionState.Open) //Chiudi connessione
            {
                connection.Close();
            }
        }
    }
}