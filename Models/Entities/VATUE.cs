﻿namespace TaxDashboard.Models.Entities;

public class VATUE : Entity
{
    public Client Client { get; set; } = default!;
    public DateOnly ContextDate { get; set; }
    public DateTime ChangedDateTime { get; set; }
}