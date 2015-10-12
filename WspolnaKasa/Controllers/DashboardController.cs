﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccessLayer;
using ExpensesDomain.Exceptions;
using ExpensesDomain.Services;
using Microsoft.AspNet.Identity;
using WspolnaKasa.App_GlobalResources;
using WspolnaKasa.Models.Dashboard;
using WspolnaKasa.Models.Dashboard.Group;

namespace WspolnaKasa.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private IGroupService _groupService;
        private ITransactionService _transactionService;
        private IApplicationUserRepository _userRepository;

        public DashboardController(IGroupService groupService, ITransactionService transactionService, IApplicationUserRepository userRepository)
        {
            _groupService = groupService;
            _transactionService = transactionService;
            _userRepository = userRepository;
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
                                UserId = s.UserId,
                                UserName = _userRepository.GetUser(s.UserId).DisplayName
                            })
                    }));
        }

        public PartialViewResult _SummarySidebar()
        {
            var settlements = _transactionService
                .GetSummaryForUser(User.Identity.GetUserId())
                .Select(s => new SingleSettlementViewModel
                {
                    Amount = Math.Round(s.Amount, 2),
                    UserName = _userRepository.GetUser(s.UserId).DisplayName
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
                            User = _userRepository.GetUser(m.UserPayingId).DisplayName,
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
                            User = _userRepository.GetUser(m.UserPayingId).DisplayName,
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
                                    Amount = Math.Round(m.Amount, 2),
                                    Date = m.Date,
                                    Description = m.Description,
                                    UserFrom = m.ApplicationUser.DisplayName,
                                    UserTo = _userRepository.GetUser(m.ReceiverId).DisplayName,
                                    Group = m.Group.Name
                                }));
            }
            else
            {
                return PartialView(
                            _transactionService
                                .GetAllSentAndReceivedTransfers(User.Identity.GetUserId(), id)
                                .Select(m => new TransferViewModel
                                {
                                    Amount = Math.Round(m.Amount, 2),
                                    Date = m.Date,
                                    Description = m.Description,
                                    UserFrom = m.ApplicationUserId,
                                    UserTo = m.ReceiverId,
                                    Group = m.Group.Name
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
                if (members.Any(x => x.Id == member.Id) == false)
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
            switch (CreateOrJoin)
            {
                case "Create":
                    var result = _groupService.CreateGroup(model.Name, model.Secret, User.Identity.GetUserId());
                    if (!result)
                    {
                        ModelState.AddModelError("", Translations.Dashboard_GroupAlreadyExists);
                        return Index();
                    }
                    break;
                case "Join":
                    try
                    {
                        _groupService.JoinGroup(model.Name, model.Secret, User.Identity.GetUserId());
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
                    break;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup(EditGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }
            var result = _groupService.EditGroup(model.GroupId, model.NewName);
            if (!result)
            {
                ModelState.AddModelError("", Translations.Dashboard_GroupAlreadyExists);
                return Index();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveGroup(RemoveGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }
            var result = _groupService.RemoveGroup(model.GroupId, model.Secret);
            if (!result)
            {
                ModelState.AddModelError("", Translations.Dashboard_IncorrectGroupSecret);
                return Index();
            }

            return RedirectToAction("Index");
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

            var result = _transactionService.EditExpense(
                User.Identity.GetUserId(),
                model.ExpenseId,
                model.GroupId,
                model.Description,
                model.Date,
                Math.Round(model.Amount, 2),
                model.Participants);
            if (!result)
            {
                ModelState.AddModelError("", Translations.Dashboard_CannotEditOthersExpenses);
                return Index();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveExpense(RemoveExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Index();
            }

            var result = _transactionService.RemoveExpense(User.Identity.GetUserId(), model.ExpenseId);
            if (!result)
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
    }
}