using DisneyAPI.Interfaces;
using DisneyAPI.ViewModel.Character.Get;
using DisneyAPI.ViewModel.Character.Post;
using DisneyAPI.ViewModel.Character.Put;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DisneyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CharacterController : ControllerBase
    {
        //This comes from the Startup (dependency injection)
        private readonly ICharacterRepository _characterRepository;
        public CharacterController(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        [HttpGet]
        [Route("character/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var character = _characterRepository.GetCharacter(id);

                return Ok(character);
            }
            catch (Exception ex)
            {
                if (ex.Message is "The character is not in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("characters")]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var characters = _characterRepository.GetAllCharactersVM();

            if (characters is null) return NoContent();

            return Ok(characters);
        }

        [HttpGet]
        [Route("characters-filtered")]
        public IActionResult GetFiltered([FromQuery] GetReqFilteredCharacterVM model)
        {
            try
            {
                var character = _characterRepository.GetCharacterByFilter(model);
                return Ok(character);
            }
            catch (Exception ex)
            {
                if (ex.Message is "The character is not in the database." ||
                    ex.Message is "There is no characters that matches your petition.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(PostReqCharacterVM characterVM)
        {
            try
            {
                _characterRepository.AddCharacter(characterVM);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message is "The character is already in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(PutReqCharacterVM characterVM)
        {
            try
            {
                _characterRepository.EditCharacter(characterVM);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message is "The character is not in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _characterRepository.DeleteCharacter(id);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message is "The character is not in the database.") return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }
    }
}
