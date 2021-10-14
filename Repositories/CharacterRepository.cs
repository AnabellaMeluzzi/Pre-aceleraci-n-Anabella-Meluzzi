using DisneyAPI.Context;
using DisneyAPI.Entities;
using DisneyAPI.Interfaces;
using DisneyAPI.Repositories;
using DisneyAPI.ViewModel.Character.Get;
using DisneyAPI.ViewModel.Character.Post;
using DisneyAPI.ViewModel.Character.Put;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public class CharacterRepository : BaseRepository<Character, DisneyContext>, ICharacterRepository
    {
        public CharacterRepository(DisneyContext dbContext) : base(dbContext)
        {
        }


        public Character GetCharacterEntity(int id)
        {
            return DbSet.ToList().FirstOrDefault(x => x.Id == id);
        }

        public GetResCharacterDetailVM GetCharacter(int id)
        {
            var character = DbSet.Include(x => x.Movie).ToList().FirstOrDefault(x => x.Id == id);

            if(character is default(Character)) throw new Exception("The character is not in the database.");

            string movieName = CheckMovieName(character);

            GetResCharacterDetailVM characterVM = new GetResCharacterDetailVM
            {
                Image = character.Image,
                Name = character.Name,
                Age = character.Age,
                Weight = character.Weight,
                Story = character.Story,
                MovieName = movieName
            };

            return characterVM;
        }        

        public List<Character> GetAllCharacters()
        {
            return DbSet.Include(x => x.Movie).ToList();
        }

        public List<GetResCharacterVM> GetAllCharactersVM()
        {
            var characterList = GetAllCharacters();

            List<GetResCharacterVM> charactersVM = new List<GetResCharacterVM>();
            foreach(var character in characterList)
            {
                charactersVM.Add(new GetResCharacterVM
                {
                    Image = character.Image,
                    Name = character.Name
                });
            }
            return (charactersVM);
        }

        public List<GetResFilteredCharacterVM> GetCharacterByFilter(GetReqFilteredCharacterVM model)
        {
            var characterInDB = DbSet.Include(x => x.Movie).ToList().Where(g => g.Name.Contains(model.Name)).ToList();
            try
            {
                if (model.Age is not null) characterInDB = characterInDB.Where(x => x.Age == model.Age).ToList();

                if (model.Movie is not null) characterInDB = characterInDB.Where(x => x.Movie.Title.Contains(model.Movie)).ToList();

                if (characterInDB.Count is 0) throw new Exception("There is no characters that matches your petition.");

                List <GetResFilteredCharacterVM> charactersVM = new List<GetResFilteredCharacterVM>();
                foreach(var character in characterInDB)
                {
                    string movieName = CheckMovieName(character);

                    charactersVM.Add(new GetResFilteredCharacterVM
                    {
                        Image = character.Image,
                        Name = character.Name,
                        Age = character.Age,
                        Weight = character.Weight,
                        MovieTitle = movieName
                    });

                }
                return (charactersVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCharacter(PostReqCharacterVM characterVM)
        {
            var characterInDB = GetAllCharacters().FirstOrDefault(x => x.Name == characterVM.Name);
            if (characterInDB is not default(Character)) throw new Exception("The character is already in the database.");

            Movie movie = new Movie();
            movie = (characterVM.MovieID is not null) ? _dbContext.Movies.FirstOrDefault(m => m.Id == characterVM.MovieID)
                                                      : null;

            try
            {
                Character character = new Character
                {
                    Name = characterVM.Name,
                    Image = characterVM.Name,
                    Age = characterVM.Age,
                    Weight = characterVM.Weight,
                    Story = characterVM.Story,
                    Movie = movie
                };

                Add(character);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EditCharacter(PutReqCharacterVM characterVM)
        {
            var characterInDB = DbSet.Include(x => x.Movie).ToList().FirstOrDefault(x => x.Id == characterVM.Id);
            if (characterInDB is default(Character)) throw new Exception("The character is not in the database.");

            Movie movie = new Movie();
            movie = (characterVM.MovieID is not null) ? _dbContext.Movies.FirstOrDefault(m => m.Id == characterVM.MovieID)
                                                      : null;
            try
            {
                characterInDB.Name = characterVM.Name;
                characterInDB.Age = characterVM.Age;
                characterInDB.Story = characterVM.Story;
                characterInDB.Image = characterVM.Image;
                characterInDB.Weight = characterVM.Weight;
                characterInDB.Movie = movie;

                Put(characterInDB);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCharacter(int id)
        {
            var characterInDB = GetCharacterEntity(id);
            if (characterInDB is default(Character)) throw new Exception("The character is not in the database.");

            try
            {
                Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Return the name of the movie associated to the character.
        /// If null, returns: "No movie asociated"
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private string CheckMovieName(Character character)
        {
            if (character.Movie is null) return "No movie associated";

            return character.Movie.Title;
        }
    }
}
