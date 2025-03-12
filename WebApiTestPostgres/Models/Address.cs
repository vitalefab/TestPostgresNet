using System;
using System.Collections.Generic;

namespace WebApiTestPostgres.Models;

public partial class Address
{
    public int Id { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public int PersonId { get; set; }

    public virtual Person? Person { get; set; } = null!;
}
