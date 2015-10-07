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
    public class TransfersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Transfers
        public IQueryable<Transfer> GetTransfers()
        {
            var userId = User.Identity.GetUserId();
            var model = db.Users.Find(userId).TransfersSent.Union(db.Transfers.Where(x => x.ReceiverId == userId));
            var dto = model.Select(m => new Transfer
            {
                Amount = m.Amount,
                ApplicationUserId = m.ApplicationUserId,
                Date = m.Date,
                Description = m.Description,
                GroupId = m.GroupId,
                ReceiverId = m.ReceiverId,
                TransferId = m.TransferId
            }).AsQueryable();
            return dto;
        }

        // GET: api/Transfers/5
        [ResponseType(typeof(Transfer))]
        public IHttpActionResult GetTransfer(int id)
        {
            var transfer = db.Transfers.Find(id);
            if (transfer == null)
            {
                return NotFound();
            }

            var dto = new Transfer
            {
                Amount = transfer.Amount,
                ApplicationUserId = transfer.ApplicationUserId,
                Date = transfer.Date,
                Description = transfer.Description,
                GroupId = transfer.GroupId,
                ReceiverId = transfer.ReceiverId,
                TransferId = transfer.TransferId
            };
            return Ok(dto);
        }

        // PUT: api/Transfers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransfer(int id, Transfer transfer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transfer.TransferId)
            {
                return BadRequest();
            }

            var transferModel = db.Transfers.Find(id);

            transferModel.Amount = transfer.Amount;
            if (transferModel.ApplicationUserId != transfer.ApplicationUserId)
            {
                transferModel.ApplicationUserId = transfer.ApplicationUserId;
                transferModel.ApplicationUser = db.Users.Find(transferModel.ApplicationUserId);
            }
            transferModel.Date = transfer.Date;
            transferModel.Description = transfer.Description;
            if (transferModel.GroupId != transfer.GroupId)
            {
                transferModel.GroupId = transfer.GroupId;
                transferModel.Group = db.Groups.Find(transferModel.GroupId);
            }
            transferModel.ReceiverId = transfer.ReceiverId;

            db.Entry(transferModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferExists(id))
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

        // POST: api/Transfers
        [ResponseType(typeof(Transfer))]
        public IHttpActionResult PostTransfer(Transfer transfer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transferModel = new DataAccessLayer.Entities.ExpensesDomain.Transfer();
            transferModel.Amount = transfer.Amount;
            transferModel.ApplicationUserId = transfer.ApplicationUserId;
            transferModel.ApplicationUser = db.Users.Find(transferModel.ApplicationUserId);
            transferModel.Date = transfer.Date;
            transferModel.Description = transfer.Description;
            transferModel.GroupId = transfer.GroupId;
            transferModel.Group = db.Groups.Find(transferModel.GroupId);
            transferModel.ReceiverId = transfer.ReceiverId;

            db.Transfers.Add(transferModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transfer.TransferId }, transfer);
        }

        // DELETE: api/Transfers/5
        [ResponseType(typeof(Transfer))]
        public IHttpActionResult DeleteTransfer(int id)
        {
            var transfer = db.Transfers.Find(id);
            if (transfer == null)
            {
                return NotFound();
            }

            var dto = new Transfer
            {
                Amount = transfer.Amount,
                ApplicationUserId = transfer.ApplicationUserId,
                Date = transfer.Date,
                Description = transfer.Description,
                GroupId = transfer.GroupId,
                ReceiverId = transfer.ReceiverId,
                TransferId = transfer.TransferId
            };

            db.Transfers.Remove(transfer);
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

        private bool TransferExists(int id)
        {
            return db.Transfers.Count(e => e.TransferId == id) > 0;
        }
    }
}