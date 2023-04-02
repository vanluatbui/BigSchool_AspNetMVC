namespace BigSchool_THBuoi4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Followings
    {
        [Key]
        [Column(Order = 0)]
        public string FollowerId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string FolloweeId { get; set; }
    }
}
