﻿using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Report
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string Label { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Result? Result { get; set; }

    public virtual Student Student { get; set; } = null!;
}