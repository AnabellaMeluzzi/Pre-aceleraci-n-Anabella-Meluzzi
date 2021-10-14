using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModel.Character.Put
{
    public class PutResCharacterVM
    {
        public string Image { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Weight { get; set; }

        public string Story { get; set; }

        public int? Movie { get; set; }
    }
}
