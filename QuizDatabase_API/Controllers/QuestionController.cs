using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizDatabase_API.DTO;
using QuizDatabase_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizDatabase_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly QuizContext _context;
        public QuestionController(QuizContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetQuestions()
        {
            return await _context.Questions.Select(x=>ItemToDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(long id)
        {
            var quest = await _context.Questions.FindAsync(id);
            if (quest == null) return NotFound();
            return Ok(ItemToDTO(quest));
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDTO>> PostQuestion(QuestionDTO questdto)
        {
            var quest = new Question
            {
                question = questdto.question,
                quest_answer = questdto.quest_answer
            };
            _context.Questions.Add(quest);
            await _context.SaveChangesAsync();
            return Ok(ItemToDTO(quest));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateQuestion(long id, QuestionDTO questdto)
        {
            var quest = _context.Questions.FirstOrDefault(x => x.Id == id);
            if (quest != null) return BadRequest();

            quest.question = questdto.question;
            quest.quest_answer = questdto.quest_answer;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!QuestionExists(id))
            {
                if (!QuestionExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteQuestion(long id)
        {
            var quest = await _context.Questions.FindAsync(id);
            if (quest == null) return NotFound();
            _context.Questions.Remove(quest);
            await _context.SaveChangesAsync();

            return Ok(quest);
        }

        private bool QuestionExists(long id)
        {
            return _context.Answers.Any(u => u.Id == id);
        }
        private static QuestionDTO ItemToDTO(Question quest) => new QuestionDTO
        {
            question = quest.question,
            quest_answer = quest.quest_answer
            
        };
    }
}
