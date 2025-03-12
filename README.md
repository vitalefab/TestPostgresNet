
postgres version 17.4
https://www.enterprisedb.com/downloads/postgres-postgresql-downloads

aggiunta migration se mancano tabelle vengono create  in automatico

usata questa per creare model e dbcontext da database già pronto
---
dotnet ef dbcontext scaffold "Host=localhost;Database=testdb;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -c AppDbContext --force

CREATE DATABASE testdb;

\c TestDb;

CREATE TABLE Persons (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE Addresses (
    Id SERIAL PRIMARY KEY,
    Street VARCHAR(200) NOT NULL,
    City VARCHAR(100) NOT NULL,
    PersonId INT NOT NULL,
    CONSTRAINT fk_person FOREIGN KEY (PersonId) REFERENCES Persons(Id) ON DELETE CASCADE
);



