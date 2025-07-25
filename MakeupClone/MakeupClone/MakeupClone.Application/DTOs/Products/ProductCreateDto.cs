﻿namespace MakeupClone.Application.DTOs.Products;

public class ProductCreateDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string ImageUrl { get; set; }

    public Guid CategoryId { get; set; }

    public Guid BrandId { get; set; }
}