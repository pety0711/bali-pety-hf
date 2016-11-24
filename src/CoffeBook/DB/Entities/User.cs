using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Entities
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public string FbMail { get; set; }
        public string FbPassword { get; set; }
        public ICollection<RecipeBook> RecipeBooks { get; set; }
    }
}
