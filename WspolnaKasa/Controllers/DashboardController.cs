using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WspolnaKasa.App_GlobalResources;
using WspolnaKasa.Models.Dashboard;
using WspolnaKasa.Models.Dashboard.Group;
using Domain.Services;
using Domain.Exceptions;

namespace WspolnaKasa.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IGroupService _groupService;
        private ITransactionService _transactionService;

        public DashboardController(IGroupService groupService, ITransactionService transactionService)
        {
            _groupService = groupService;
            _transactionService = transactionService;
        }

        public ActionResult Index()
        {
            var groups = _groupService
                .GetAllGroups(User.Identity.GetUserId())
                .Select(m => new GroupInListViewModel { GroupId = m.GroupId, Name = m.Name });
            ViewBag.GroupId = new SelectList(groups, "GroupId", "Name");
            return View("Index");
        }

        public PartialViewResult _GroupsSidebar()
        {
            return PartialView(
                _groupService
                    .GetAllGroups(User.Identity.GetUserId())
                    .Select(m => new GroupInListViewModel
                    {
                        GroupId = m.GroupId,
                        Name = m.Name,
                        Summary = _transactionService
                            .GetSummaryForUser(User.Identity.GetUserId(), m.GroupId)
                            .Select(s => new SingleSettlementViewModel
                            {
                                Amount = Math.Round(s.Amount, 2),
                                UserId = s.User.Id,
                                UserName = s.User.DisplayName
                            })
                            .Where(s => s.Amount != 0)
                    }));
        }

        public PartialViewResult _SummarySidebar()
        {
            var settlements = _transactionService
                .GetSummaryForUser(User.Identity.GetUserId())
                .Select(s => new SingleSettlementViewModel
                {
                    Amount = Math.Round(s.Amount, 2),
                    UserName = s.User.DisplayName
                });
            return PartialView(
                new SummaryViewModel
                {
                    Settlements = settlements,
                    TotalAmount = settlements.Sum(x => x.Amount)
                });
        }

        public PartialViewResult _Expenses(int id = -1)
        {
            if (id == -1)
            {
                return PartialView(
                    _transactionService
                        .GetAllExpenses(User.Identity.GetUserId())
                        .Select(m => new ExpenseViewModel
                        {
                            Amount = Math.Round(m.Amount, 2),
                            Date = m.Date,
                            Description = m.Description,
                            ExpenseId = m.ExpenseId,
                            Group = m.Group.Name,
                            GroupId = m.GroupId,
                            User = m.UserPaying.DisplayName,
                            Editable = User.Identity.GetUserId() == m.UserPayingId,
                        }));
            }
            else
            {
                return PartialView(
                    _transactionService
                        .GetAllExpenses(User.Identity.GetUserId(), id)
                        .Select(m => new ExpenseViewModel
                        {
                            Amount = Math.Round(m.Amount, 2),
                            Date = m.Date,
                            Description = m.Description,
                            ExpenseId = m.ExpenseId,
                            Group = m.Group.Name,
                            User = m.UserPaying.DisplayName,
                            Editable = User.Identity.GetUserId() == m.UserPayingId,
                        }));
            }
        }

        public PartialViewResult _Transfers(int id = -1)
        {
            if (id == -1)
            {
                return PartialView(
                            _transactionService
                                .GetAllSentAndReceivedTransfers(User.Identity.GetUserId())
                                .Select(m => new TransferViewModel
                                {
                                    TransferId = m.TransferId,
                                    Amount = Math.Round(m.Amount, 2),
                                    Date = m.Date,
                                    Description = m.Description,
                                    UserFrom = m.Sender.DisplayName,
                                    UserTo = m.Receiver.DisplayName,
                                    Group = m.Group.Name,
                                    Editable = User.Identity.GetUserId() == m.SenderId
                                }));
            }
            else
            {
                return PartialView(
                            _transactionService
                                .GetAllSentAndReceivedTransfers(User.Identity.GetUserId(), id)
                                .Select(m => new TransferViewModel
                                {
                                    TransferId = m.TransferId,
                                    Amount = Math.Round(m.Amount, 2),
                                    Date = m.Date,
                                    Description = m.Description,
                                    UserFrom = m.Sender.DisplayName,
                                    UserTo = m.Receiver.DisplayName,
                                    Group = m.Group.Name,
                                    Editable = User.Identity.GetUserId() == m.SenderId
                                }));
            }
        }

        public PartialViewResult _MembersSelectByGroup(int id)
        {
            var groupMembers = _groupService.GetGroup(id).Members.Select(x => new GroupMemberViewModel { Id = x.Id, Name = x.DisplayName });
            return PartialView("_MembersSelect", groupMembers);
        }

        public PartialViewResult _MembersSelectByExpense(int id)
        {
            var expense = _transactionService.GetExpense(id);
            var members = expense.Participants.Select(x => new GroupMemberViewModel { Id = x.Id, Name = x.DisplayName, Selected = true }).ToList();
            foreach (var member in expense.Group.Members)
            {
                if (!members.Any(x => x.Id == member.Id))
                {
                    members.Add(new GroupMemberViewModel { Id = member.Id, Name = member.DisplayName, Selected = false });
                }
            }
            return PartialView("_MembersSelect", members);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJoinGroup(CreateJoinGroupViewModel model, string CreateOrJoin)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            try
            {
                switch (CreateOrJoin)
                {
                    case "Create":
                        _groupService.CreateGroup(model.Name, model.Secret, User.Identity.GetUserId());
                        break;
                    case "Join":
                        _groupService.JoinGroup(model.Name, model.Secret, User.Identity.GetUserId());
                        break;
                }
                return RedirectToAction("Index");
            }
            catch (GroupAlreadyExistsException)
            {
                ModelState.AddModelError("", Translations.Dashboard_GroupAlreadyExists);
                return Index();
            }
            catch (GroupNotFoundException)
            {
                ModelState.AddModelError("", Translations.Dashboard_GroupDoesNotExist);
                return Index();
            }
            catch (WrongGroupPasswordException)
            {
                ModelState.AddModelError("", Translations.Dashboard_IncorrectGroupSecret);
                return Index();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup(EditGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            try
            {
                _groupService.EditGroup(model.GroupId, model.NewName);
                return RedirectToAction("Index");
            }
            catch (GroupAlreadyExistsException)
            {
                ModelState.AddModelError("", Translations.Dashboard_GroupAlreadyExists);
                return Index();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveGroup(RemoveGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            try
            {
                _groupService.RemoveGroup(model.GroupId, model.Secret);
                return RedirectToAction("Index");
            }
            catch (WrongGroupPasswordException)
            {
                ModelState.AddModelError("", Translations.Dashboard_IncorrectGroupSecret);
                return Index();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddExpense(ExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            if (model.Participants == null || !model.Participants.Any())
            {
                ModelState.AddModelError("", Translations.Dashboard_AtLeastOneParticipant);
                return Index();
            }

            _transactionService.AddExpense(
                User.Identity.GetUserId(),
                model.GroupId,
                model.Description,
                model.Date,
                Math.Round(model.Amount, 2),
                model.Participants);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditExpense(ExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            if (model.Participants == null || !model.Participants.Any())
            {
                ModelState.AddModelError("", Translations.Dashboard_AtLeastOneParticipant);
                return Index();
            }

            try
            {
            _transactionService.EditExpense(
                User.Identity.GetUserId(),
                model.ExpenseId,
                model.GroupId,
                model.Description,
                model.Date,
                Math.Round(model.Amount, 2),
                model.Participants);
                return RedirectToAction("Index");
            }
            catch (CannotEditOtherUsersExpensesException)
            {
                ModelState.AddModelError("", Translations.Dashboard_CannotEditOthersExpenses);
                return Index();
            }
            catch(ExpenseParticipantsMustBeGroupMembersExecption)
            {
                ModelState.AddModelError("", Translations.Dashboard_ExpenseParticipantsMustBeGroupMembers);
                return Index();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveExpense(RemoveExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            try
            {
                _transactionService.RemoveExpense(User.Identity.GetUserId(), model.ExpenseId);
            }
            catch(CannotEditOtherUsersExpensesException)
            {
                ModelState.AddModelError("", Translations.Dashboard_CannotRemoveOthersExpenses);
                return Index();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTransfer(TransferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            _transactionService.AddTransfer(
                User.Identity.GetUserId(),
                model.UserToId,
                model.GroupId,
                model.Description,
                model.Date,
                Math.Round(model.Amount, 2));
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveTransfer(RemoveTransferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            try
            {
                _transactionService.RemoveTransfer(User.Identity.GetUserId(), model.TransferId);
            }
            catch(CannotEditOtherUsersTransfersException)
            {
                ModelState.AddModelError("", Translations.Dashboard_CannotRemoveOthersTransfers);
                return Index();
            }

            return RedirectToAction("Index");
        }
    }
}