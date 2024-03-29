﻿namespace Bluefish.Blazor.Models;

public enum FilterTypes
{
    [Display(Name = "Equals")]
    Equals,
    [Display(Name = "Does not equal")]
    DoesNotEqual,
    [Display(Name = "Starts with")]
    StartsWith,
    [Display(Name = "Ends with")]
    EndsWith,
    [Display(Name = "Contains")]
    Contains,
    [Display(Name = "Does not contain")]
    DoesNotContain,
    [Display(Name = "In list")]
    In,
    [Display(Name = "Greater than")]
    GreaterThan,
    [Display(Name = "Greater than or equal")]
    GreaterThanOrEqual,
    [Display(Name = "Less than")]
    LessThan,
    [Display(Name = "Less than or equal")]
    LessThanOrEqual,
    [Display(Name = "Range")]
    Range,
    [Display(Name = "Is Null")]
    IsNull,
    [Display(Name = "Is Not Null")]
    IsNotNull,
    [Display(Name = "Is Empty")]
    IsEmpty,
    [Display(Name = "Is Not Empty")]
    IsNotEmpty
}
