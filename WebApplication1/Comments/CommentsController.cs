using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.News;

namespace WebApplication1.Comments
{
    [ApiController]
    [Route("comments")]
    public class CommentsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        public CommentsController(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// Выводит список комментариев
        /// </summary>
        /// <returns>Список новостей</returns>
        /// <response code="200">Список новостей.</response>
        /// <response code="404">Ничего нет.</response>

        [HttpGet]
        [ProducesResponseType(typeof(List<CommentsDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Index([FromQuery] int? newsId)
        {
            if (newsId == null)
            {
                return BadRequest();
            }
            if ((await _context.News.Where(n => n.Id == newsId).FirstOrDefaultAsync()) == null)
            {
                return NotFound();
            }
            var items = await _context.Comments.Where(c => c.NewsId == newsId).Include(newsId => newsId.Author).OrderByDescending(n => n.CreatedAt).Take(100).ToListAsync();
            return Ok(_mapper.Map<CommentsDTO[]>(items));
        }

        /// <summary>
        /// Выводит комментарий по id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Новость по id</response>
        /// <response code="404">Ничего нет.</response>
        /// 
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommentsEntity), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var item = await _context.Comments.Include(n => n.Author).FirstOrDefaultAsync(n => n.Id == id);

            return Ok(_mapper.Map<CommentsDTO>(item));   //#

            //return item != null ? Ok(item) : NotFound();
        }

        /// <summary>
        /// Добавление комментария
        /// </summary>
        /// <param name="item">Публикуемая статья</param>
        /// <response code="201">Новость опубликована</response>
        /// <response code="400">Некорректный запрос</response>
        [HttpPost]
        [ProducesResponseType(typeof(CommentsEntity), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CommentCreateDTO item)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity?.Name == null) return Forbid();

                int userId = int.Parse(User.Identity.Name);

                var created = new CommentsEntity()
                {
                    Content = item.Content,
                    NewsId = item.NewsId,
                    AuthorId = userId,
                };

                _context.Add(created);
                await _context.SaveChangesAsync();
                //поиск автора в списке юзеров
                created.Author = await _context.Users.FirstAsync(u => u.Id == created.AuthorId);

                return CreatedAtAction(nameof(Details), new { id = created.Id }, created);
            }
            return BadRequest();
        }
        /// <summary>
        /// Изменение комментария
        /// </summary>
        /// <param name="id">ID новости</param>
        /// <param name="item">Редактиремая статья</param>
        /// <response code="202">Новость изменена</response>
        /// <response code="400">Некорректный запрос</response>
        /// <response code="404">Запись не найдена</response>
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(CommentsEntity), 202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Edit(int id, [FromBody] CommentCreateDTO item)
        {
            if (User.Identity?.Name == null) return Forbid();

            int userId = int.Parse(User.Identity.Name);
            if (_context.News == null)
            {
                return Problem("Entity set 'DatabaseContext.news'  is null.");
            }
            if (ModelState.IsValid)
            {
                if (item.Content == null && item.NewsId == null)
                {
                    return BadRequest();
                }

                try
                {
                    var editable = await _context.Comments.Include(n => n.Author).FirstAsync(n => n.Id == id);

                    if (editable == null)
                    {
                        return NotFound();

                    }

                    editable.NewsId = item.NewsId;
                    editable.Content = item.Content;

                    _context.Update(editable);
                    await _context.SaveChangesAsync();

                    return Accepted(editable);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await _context.News.AnyAsync(n => n.Id == id)))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return BadRequest();
        }

        /// <summary>
        /// Удаление комментария
        /// </summary>
        /// <param name="id">ID записи</param>
        /// <response code="202">Новость удалена</response>
        /// <response code="404">Запись не найдена</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'DatabaseContext.news'  is null.");
            }
            var item = await _context.Comments.FindAsync(id);
            if (item != null)
            {
                _context.Comments.Remove(item);
                await _context.SaveChangesAsync();
                return Accepted();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
