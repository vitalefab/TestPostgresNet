﻿using System;
using System.Collections.Generic;

namespace WebApiTestPostgres.Models;

public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
