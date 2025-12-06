using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string FirstNameArabic { get; set; }
    [Required]
    public string LastNameArabic { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Gender { get; set; } // Male/Female
    [Required]
    public string City { get; set; }
    [Required]
    public string HowDidYouHear { get; set; } // Dropdown
    public string MobileNumber { get; set; } // For verification
    // Password validation: Handled in registration view
}