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
    public class AnswerController : ControllerBase
    {
        private readonly QuizContext _context;
        public AnswerController(QuizContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDTO>>> GetAnswers()
        {
            return await _context.Answers.Select(x => ItemToDTO(x)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerDTO>> GetAnswer(long id)
        {
            var ans = await _context.Answers.FindAsync(id);
            if (ans == null) return NotFound();
            return Ok(ItemToDTO(ans));
        }

        [HttpPost]
        public async Task<ActionResult<AnswerDTO>> PostAnswer(AnswerDTO ansdto)
        {
            if (!_context.Questions.Any(q => q.Id == ansdto.QuestId)) return BadRequest();
            var answer = new Answer
            {
                answer = ansdto.answer,
                QuestId = ansdto.QuestId,
                Question = _context.Questions.FirstOrDefault()
            };

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return Ok(ItemToDTO(answer));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAnswer(long id, AnswerDTO answerdto)
        {
            var ans = _context.Answers.FirstOrDefault(x => x.Id == id);
            if (ans != null) return BadRequest();

            ans.answer = answerdto.answer;
            ans.QuestId = answerdto.QuestId;

            if (!_context.Questions.Any(q => q.Id == ans.QuestId)) return BadRequest();
            ans.Question = _context.Questions.FirstOrDefault(q => q.Id == ans.QuestId);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AnswerExists(id))
            {
                if (!AnswerExists(id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAnswer(long id)
        {
            var ans = await _context.Answers.FindAsync(id);
            if (ans == null) return NotFound();
            _context.Answers.Remove(ans);
            await _context.SaveChangesAsync();

            return Ok(ans);
        }

        private bool AnswerExists(long id)
        {
            return _context.Answers.Any(u => u.Id == id);
        }
        private static AnswerDTO ItemToDTO(Answer answer) => new AnswerDTO
        {
            answer = answer.answer,
            QuestId = answer.QuestId

        };
    }
}
