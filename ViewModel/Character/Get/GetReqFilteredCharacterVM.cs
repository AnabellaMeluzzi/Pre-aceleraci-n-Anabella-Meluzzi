using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModel.Character.Get
{
    public class GetReqFilteredCharacterVM
    {
        [Required]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string? Movie { get; set; }
    }
}
