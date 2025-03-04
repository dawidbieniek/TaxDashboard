﻿using LifeManagers.Data;

using TaxDashboard.Data.Enums;

namespace TaxDashboard.Data.Entities;

public class InvoiceCount : Entity
{
    public Client Client { get; set; } = default!;
    public InvoiceType Type { get; set; } = InvoiceType.Sell;
    public DateOnly Date { get; set; }
    public int Amount { get; set; }
}