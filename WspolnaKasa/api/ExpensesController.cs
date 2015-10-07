using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccessLayer;
using Microsoft.AspNet.Identity;
using WspolnaKasa.api.DTO;

namespace WspolnaKasa.api
{
    [Authorize]
    public class ExpensesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Expenses
        public IQueryable<Expense> GetExpenses()
        {
            var userId = User.Identity.GetUserId();
            var model = db.Users.Find(userId).Expenses.Union(db.Expenses.Where(x => x.UserPayingId == userId));
            var dto = model.Select(x => new Expense
            {
                Amount = x.Amount,
                Date = x.Date,
                Description = x.Description,
                ExpenseId = x.ExpenseId,
                GroupId = x.GroupId,
                Participants = x.Participants.Select(p => p.Id).ToList(),
                UserPayingId = x.UserPayingId
            }).AsQueryable();
            return dto;
        }

        // GET: api/Expenses/5
        [ResponseType(typeof(Expense))]
        public IHttpActionResult GetExpense(int id)
        {
            var expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            var dto = new Expense
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseId = expense.ExpenseId,
                GroupId = expense.GroupId,
                Participants = expense.Participants.Select(p => p.Id).ToList(),
                UserPayingId = expense.UserPayingId
            };

            return Ok(dto);
        }

        // PUT: api/Expenses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExpense(int id, Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expense.ExpenseId)
            {
                return BadRequest();
            }

            var expenseModel = db.Expenses.Find(id);
            expenseModel.Amount = expense.Amount;
            expenseModel.Date = expense.Date;
            expenseModel.Description = expense.Description;
            if (expenseModel.GroupId != expense.GroupId)
            {
                expenseModel.GroupId = expense.GroupId;
                expenseModel.Group = db.Groups.Find(expenseModel.GroupId);
            }
            expenseModel.Participants = db.Users.Where(x => expense.Participants.Contains(x.Id)).ToList();
            expenseModel.UserPayingId = expense.UserPayingId;

            db.Entry(expenseModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Expenses
        [ResponseType(typeof(Expense))]
        public IHttpActionResult PostExpense(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var expenseModel = new DataAccessLayer.Entities.ExpensesDomain.Expense();
            expenseModel.Amount = expense.Amount;
            expenseModel.Date = expense.Date;
            expenseModel.Description = expense.Description;
            expenseModel.GroupId = expense.GroupId;
            expenseModel.Group = db.Groups.Find(expenseModel.GroupId);
            expenseModel.Participants = db.Users.Where(x => expense.Participants.Contains(x.Id)).ToList();
            expenseModel.UserPayingId = expense.UserPayingId;

            db.Expenses.Add(expenseModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = expense.ExpenseId }, expense);
        }

        // DELETE: api/Expenses/5
        [ResponseType(typeof(Expense))]
        public IHttpActionResult DeleteExpense(int id)
        {
            var expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            var dto = new Expense
            {
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                ExpenseId = expense.ExpenseId,
                GroupId = expense.GroupId,
                Participants = expense.Participants.Select(p => p.Id).ToList(),
                UserPayingId = expense.UserPayingId
            };

            db.Expenses.Remove(expense);
            db.SaveChanges();

            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExpenseExists(int id)
        {
            return db.Expenses.Count(e => e.ExpenseId == id) > 0;
        }
    }
}