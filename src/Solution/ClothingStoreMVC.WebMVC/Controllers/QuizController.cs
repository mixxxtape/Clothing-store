using ClothingStoreMVC.Domain.Entities.QuizAggregates;
using ClothingStoreMVC.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreMVC.WebMVC.Controllers
{
    public class QuizController : Controller
    {
        private readonly ClothingStoreContext _context;

        public QuizController(ClothingStoreContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                        .ThenInclude(a => a.Styles)
                            .ThenInclude(s => s.Style)
                .FirstOrDefaultAsync();

            return View(quiz);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int quizId, Dictionary<int, int> answers)
        {
            var identityUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUserId);
            if (user == null) return RedirectToAction("Index", "Home");

            if (!answers.Any())
                return RedirectToAction(nameof(Index));

            var styleScores = new Dictionary<int, int>();

            foreach (var (questionId, answerId) in answers)
            {
                var answer = await _context.Answers
                    .Include(a => a.Styles)
                    .FirstOrDefaultAsync(a => a.Id == answerId && a.QuestionId == questionId);

                if (answer == null) continue;

                var alreadyAnswered = await _context.UserAnswers
                    .AnyAsync(ua => ua.UserId == user.Id && ua.QuestionId == questionId);

                if (!alreadyAnswered)
                {
                    _context.UserAnswers.Add(new UserAnswer
                    {
                        UserId = user.Id,
                        QuestionId = questionId,
                        AnswerId = answerId
                    });
                }

                foreach (var answerStyle in answer.Styles)
                {
                    if (!styleScores.ContainsKey(answerStyle.StyleId))
                        styleScores[answerStyle.StyleId] = 0;
                    styleScores[answerStyle.StyleId]++;
                }
            }

            await _context.SaveChangesAsync();

            if (!styleScores.Any())
                return RedirectToAction(nameof(Index));

            var winnerStyleId = styleScores
                .OrderByDescending(s => s.Value)
                .First().Key;

            _context.Results.Add(new Result
            {
                UserId = user.Id,
                QuizId = quizId,
                StyleId = winnerStyleId,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Result), new { styleId = winnerStyleId });
        }

        [Authorize(Roles = "user")]
        public async Task<IActionResult> Result(int styleId)
        {
            var style = await _context.Styles.FirstOrDefaultAsync(s => s.Id == styleId);
            if (style == null) return NotFound();
            return View(style);
        }


        //quiz management
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Manage()
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                        .ThenInclude(a => a.Styles)
                            .ThenInclude(s => s.Style)
                .FirstOrDefaultAsync();
            return View(quiz);
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateQuiz() => View();

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuiz([Bind("Name,Description")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(quiz);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditQuiz(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();
            return View(quiz);
        }

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuiz(int id, [Bind("Id,Name,Description")] Quiz quiz)
        {
            if (id != quiz.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(quiz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(quiz);
        }



        //question management

        [Authorize(Roles = "admin")]
        public IActionResult CreateQuestion(int quizId)
        {
            ViewBag.QuizId = quizId;
            return View();
        }

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(int quizId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ViewBag.QuizId = quizId;
                ModelState.AddModelError("", "Text is required");
                return View();
            }

            var question = new Question { Text = text, QuizId = quizId };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null) return NotFound();
            return View(question);
        }

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(int id, [Bind("Id,Text,QuizId")] Question question)
        {
            if (id != question.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage));
            }
            return View(question);
        }

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Manage));
        }




        //answer management

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAnswer(int questionId)
        {
            ViewBag.QuestionId = questionId;
            ViewBag.Styles = await _context.Styles.ToListAsync();
            return View();
        }

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnswer(int questionId, string text, int[] selectedStyles)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ViewBag.QuestionId = questionId;
                ViewBag.Styles = await _context.Styles.ToListAsync();
                ModelState.AddModelError("", "Answer text is required");
                return View();
            }

            var answer = new Answer { Text = text, QuestionId = questionId };

            foreach (var styleId in selectedStyles)
                answer.Styles.Add(new AnswerStyle { StyleId = styleId });

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditAnswer(int id)
        {
            var answer = await _context.Answers
                .Include(a => a.Styles)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (answer == null) return NotFound();
            ViewBag.Styles = await _context.Styles.ToListAsync();
            ViewBag.SelectedStyles = answer.Styles.Select(s => s.StyleId).ToList();
            return View(answer);
        }

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnswer(int id, string text, int[] selectedStyles)
        {
            var answer = await _context.Answers
                .Include(a => a.Styles)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (answer == null) return NotFound();

            answer.Text = text;
            answer.Styles.Clear();
            foreach (var styleId in selectedStyles)
                answer.Styles.Add(new AnswerStyle { StyleId = styleId });

            _context.Update(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        [HttpPost, Authorize(Roles = "admin"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Manage));
        }
    }
}