﻿$(document).ready(function () {
    groupsContainerInitialize();
    expensesContainerInitialize();
    modalsInitialize();
});

function groupsContainerInitialize() {

    $('.groupInList').click(function () {
        if ($(this).parent().parent().next().hasClass('in')) {
            $('#btnEditGroup').addClass('disabled');
            $('#btnRemoveGroup').addClass('disabled');
        }
        else {
            $('#btnEditGroup').removeClass('disabled');
            $('#btnRemoveGroup').removeClass('disabled');

            // Hidden fields in forms populated.
            var selectedId = $(this).parent().prev().val();
            $('#selectedId').val(selectedId);
            $('#groupIdEditModal').val(selectedId);
            $('#groupIdRemoveModal').val(selectedId);
        }
    });

    $('#btnClearFilterGroup').click(function () {
        $('#btnEditGroup').addClass('disabled');
        $('#btnRemoveGroup').addClass('disabled');

        $('.panel-collapse.collapse').collapse('hide');

        $.ajax({
            cache: false,
            url: '/Dashboard/_Expenses',
            beforeSend: function () {
                $('#loadingIndicator').modal('show');
            },
            success: function (result) {
                $('#expensesContainer').html(result);
                expensesContainerInitialize();
            }
        });

        $.ajax({
            cache: false,
            url: '/Dashboard/_Transfers',
            success: function (result) {
                $('#transfersContainer').html(result);
            },
            complete: function () {
                $('#loadingIndicator').modal('hide');
            }
        });
    });

    $('#groups a.minus').click(function () {
        var selectedUserId = $(this).find('input.userId').val();
        var selectedGroupId = $(this).parent().parent().attr('id');
        var predefinedAmount = $(this).find('input.amount').val().substr(1);

        $('#userIdSettleModal').val(selectedUserId);
        $('#groupIdSettleModal').val(selectedGroupId);
        $('#amountAddTransferModal').val(predefinedAmount);
        $('#addTransferModal').modal('show');
    });
}

function expensesContainerInitialize() {

    $('#expensesContainer tr').click(function () {
        var editable = $(this).find('input.editable').val() == 'True';
        if (editable) {
            $('#expensesContainer tr').removeClass('active');
            $(this).addClass('active');
            $('#btnEditExpense').removeClass('disabled');
            $('#btnRemoveExpense').removeClass('disabled');
            var selectedExpenseId = $(this).find('input.expenseId').val();
            $('#selectedExpenseId').val(selectedExpenseId);
        }
    });

    $('#btnEditExpense').click(function () {
        $.ajax({
            cache: false,
            url: '/Dashboard/_MembersSelectByExpense/' + $('#selectedExpenseId').val(),
            success: function (result) {
                $('#participantsEditExpenseModal').html(result);
            }
        });

        var row = $('#expensesContainer tr.active');
        var expenseId = $('#selectedExpenseId').val();
        var groupId = $(row).find('input.groupId').val();
        var description = $(row).find('td.description').text();
        var date = $(row).find('td.date').text();
        var amount = $(row).find('td.amount').text().split(' ')[0];

        $('#expenseIdEditExpenseModal').val(expenseId);
        $('#groupSelectEditModal').val(groupId);
        $('#descriptionEditExpenseModal').val(description);
        $('#dateEditExpenseModal').val(date);
        $('#amountEditExpenseModal').val(amount);
    });

    $('#btnRemoveExpense').click(function () {
        var expenseId = $('#selectedExpenseId').val();
        $('#expenseIdRemoveModal').val(expenseId);
    });
}

function modalsInitialize() {

    $('#groupSelectAddModal').change(function () {
        $.ajax({
            cache: false,
            url: '/Dashboard/_MembersSelectByGroup/' + $(this).val(),
            success: function (result) {
                $('#participantsAddExpenseModal').html(result);
                $('#participantsAddExpenseModal').find('option').prop('selected', 'selected');
            }
        });
    });

    $('#groupSelectEditModal').change(function () {
        $.ajax({
            cache: false,
            url: '/Dashboard/_MembersSelectByGroup/' + $(this).val(),
            success: function (result) {
                $('#participantsEditExpenseModal').html(result);
            }
        });
    });
}

function filterByGroup(groupId) {    
    $.ajax({
        cache: false,
        url: '/Dashboard/_Expenses/' + groupId,
        beforeSend: function () {
            $('#loadingIndicator').modal('show');
        },
        success: function (result) {
            $('#expensesContainer').html(result);
            expensesContainerInitialize();
        }
    });

    $.ajax({
        cache: false,
        url: '/Dashboard/_Transfers/' + groupId,
        success: function (result) {
            $('#transfersContainer').html(result);
        },
        complete: function () {
            $('#loadingIndicator').modal('hide');
        }
    });
};