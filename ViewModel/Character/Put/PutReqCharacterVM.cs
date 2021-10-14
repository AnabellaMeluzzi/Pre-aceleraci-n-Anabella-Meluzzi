using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.ViewModel.Character.Put
{
    public class PutReqCharacterVM
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Weight { get; set; }

        public string Story { get; set; }

        public int? MovieID { get; set; }
    }
}
