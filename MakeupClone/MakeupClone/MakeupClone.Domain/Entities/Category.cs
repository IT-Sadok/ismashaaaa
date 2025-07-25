﻿namespace MakeupClone.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<Product> Products { get; set; }
}