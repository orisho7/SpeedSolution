using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SpeedSolution.Models
{
    [Table("bookings")]
    public class Booking : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid User_Id { get; set; }

        [Column("engineer_id")]
        public Guid Engineer_Id { get; set; }
        
        [Column("booking_date")]
        public DateTime Date { get; set; }

        [Column("booking_hour")]
        public string Time { get; set; }

        [Column("engineer_name")]
        public string EngineerName { get; set; }

        [Column("payment_method")]
        public string Payment_Method { get; set; }

        [Column("payment_status")]
        public string Payment_Status { get; set; }

        [Column("created_at")]
        public DateTime Created_At { get; set; }
    }
}
