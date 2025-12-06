using System;
using System.Collections.Generic;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SpeedSolution.Models
{
    [Table("users")]
    public class User : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string SecondName { get; set; }

        [Column("phone")]
        public string Mobile { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("gender")]
        public string Gender { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("referral_source")]
        public string ReferralSource { get; set; }

        [Column("created_at")]
        public DateTime Created_At { get; set; }

        [Reference(typeof(Booking))]
        public List<Booking> Bookings { get; set; }
    }
}
