﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class Product
{
    public string ArticleNumber { get; set; } = null!;
    public string Company { get; set; } = null!;
    public string? Description { get; set; }
    public string? Ingress { get; set; }
    public decimal Price { get; set; }
    public string? NutName { get; set; } = null!;
    public decimal NutSize { get; set; }
    public string? BoltName { get; set; } = null!;
    public decimal BoltSize { get; set; }
}
