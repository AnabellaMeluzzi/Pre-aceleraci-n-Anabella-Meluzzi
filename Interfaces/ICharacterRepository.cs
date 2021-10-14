using DisneyAPI.Entities;
using DisneyAPI.ViewModel.Character.Get;
using DisneyAPI.ViewModel.Character.Post;
using DisneyAPI.ViewModel.Character.Put;
using System.Collections.Generic;

namespace DisneyAPI.Interfaces
{
    public interface ICharacterRepository
    {
        Character GetCharacterEntity(int id);

        GetResCharacterDetailVM GetCharacter(int id);

        List<Character> GetAllCharacters();

        List<GetResCharacterVM> GetAllCharactersVM();

        List<GetResFilteredCharacterVM> GetCharacterByFilter(GetReqFilteredCharacterVM model);

        void AddCharacter(PostReqCharacterVM characterVM);

        void EditCharacter(PutReqCharacterVM characterVM);

        void DeleteCharacter(int id);
    }
}
