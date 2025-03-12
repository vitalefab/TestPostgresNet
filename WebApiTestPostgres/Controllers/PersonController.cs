using Microsoft.AspNetCore.Mvc;
using WebApiTestPostgres.Models;
using WebApiTestPostgres.Services;

namespace WebApiTestPostgres.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonService _service;

    public PersonController(PersonService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] CreatePersonDto personDto)
    {
        var person = new Person
        {
            Name = personDto.Name,
            Addresses = personDto.Addresses.Select(a => new Address
            {
                Street = a.Street,
                City = a.City
            }).ToList()
        };

        await _service.AddPersonAsync(person);
        return Ok();
    }

}


public class CreatePersonDto
{
    public string Name { get; set; } = null!;
    public List<CreateAddressDto> Addresses { get; set; } = new List<CreateAddressDto>();
}

public class CreateAddressDto
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
}
