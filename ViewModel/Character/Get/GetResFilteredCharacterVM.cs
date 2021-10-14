using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DisneyAPI.Entities;

namespace DisneyAPI.ViewModel.Character.Get
{
    public class GetResFilteredCharacterVM
    {
        public string Image { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Weight { get; set; }

        public string? MovieTitle { get; set; }
    }
}
