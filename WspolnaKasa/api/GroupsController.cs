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
    public class GroupsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Groups
        public IQueryable<Group> GetGroups()
        {
            var model = db.Users.Find(User.Identity.GetUserId()).Groups;
            var dto = model.Select(x => new Group
            {
                Expenses = x.Expenses.Select(e => e.ExpenseId).ToList(),
                GroupId = x.GroupId,
                Members = x.Members.Select(e => e.Id).ToList(),
                Name = x.Name,
                Secret = x.Secret,
                Transfers = x.Transfers.Select(e => e.TransferId).ToList()
            }).AsQueryable();
            return dto;
        }

        // GET: api/Groups/5
        [ResponseType(typeof(Group))]
        public IHttpActionResult GetGroup(int id)
        {
            var group = db.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            var dto = new Group
            {
                Expenses = group.Expenses.Select(e => e.ExpenseId).ToList(),
                GroupId = group.GroupId,
                Members = group.Members.Select(e => e.Id).ToList(),
                Name = group.Name,
                Secret = group.Secret,
                Transfers = group.Transfers.Select(e => e.TransferId).ToList()
            };

            return Ok(dto);
        }

        // PUT: api/Groups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGroup(int id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.GroupId)
            {
                return BadRequest();
            }

            var groupModel = db.Groups.Find(id);

            groupModel.Expenses = db.Expenses.Where(e => group.Expenses.Contains(e.ExpenseId)).ToList();
            groupModel.Members = db.Users.Where(u => group.Members.Contains(u.Id)).ToList();
            groupModel.Name = group.Name;
            groupModel.Secret = group.Secret;
            groupModel.Transfers = db.Transfers.Where(t => group.Transfers.Contains(t.TransferId)).ToList();

            db.Entry(groupModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Groups
        [ResponseType(typeof(Group))]
        public IHttpActionResult PostGroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupModel = new DataAccessLayer.Entities.ExpensesDomain.Group();
            groupModel.Expenses = db.Expenses.Where(e => group.Expenses.Contains(e.ExpenseId)).ToList();
            groupModel.Members = db.Users.Where(u => group.Members.Contains(u.Id)).ToList();
            groupModel.Name = group.Name;
            groupModel.Secret = group.Secret;
            groupModel.Transfers = db.Transfers.Where(t => group.Transfers.Contains(t.TransferId)).ToList();

            db.Groups.Add(groupModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = group.GroupId }, group);
        }

        // DELETE: api/Groups/5
        [ResponseType(typeof(Group))]
        public IHttpActionResult DeleteGroup(int id)
        {
            var group = db.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            var dto = new Group
            {
                Expenses = group.Expenses.Select(e => e.ExpenseId).ToList(),
                GroupId = group.GroupId,
                Members = group.Members.Select(e => e.Id).ToList(),
                Name = group.Name,
                Secret = group.Secret,
                Transfers = group.Transfers.Select(e => e.TransferId).ToList()
            };

            db.Groups.Remove(group);
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

        private bool GroupExists(int id)
        {
            return db.Groups.Count(e => e.GroupId == id) > 0;
        }
    }
}