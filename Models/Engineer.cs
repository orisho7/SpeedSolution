using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SpeedSolution.Models
{
    [Table("engineers")]
    public class Engineer : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }
        
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("degree")]
        public string Degree { get; set; }

        [Column("specialization")]
        public string Specialization { get; set; }
        
        [Column("schedule")]
        public string Schedule { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("rating")]
        public decimal? Rating { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("created_at")]
        public DateTime Created_At { get; set; }
    }
}
