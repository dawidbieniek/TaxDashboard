﻿using LifeManagers.Data;

namespace TaxDashboard.Data.Entities;

public class JPKV7 : Entity
{
    public Client Client { get; set; } = default!;
    public DateOnly ContextDate { get; set; }
    public DateTime ChangedDateTime { get; set; }
}