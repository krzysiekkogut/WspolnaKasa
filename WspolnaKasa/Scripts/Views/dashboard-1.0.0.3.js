$(document).ready(function () {
    groupsContainerInitialize();
    expensesContainerInitialize();
    transfersContainerInitialize();
    modalsInitialize();
});

function groupsContainerInitialize() {

    $('.groupInList').click(function () {
        if ($(this).parent().parent().next().hasClass('in')) {
            $('#btnEditGroup').addClass('disabled');
            $('#btnEditGroup').attr('disabled', 'disabled');
            $('#btnRemoveGroup').addClass('disabled');
            $('#btnRemoveGroup').attr('disabled', 'disabled');
        }
        else {
            $('#btnEditGroup').removeClass('disabled');
            $('#btnEditGroup').removeAttr('disabled');
            $('#btnRemoveGroup').removeClass('disabled');
            $('#btnRemoveGroup').removeAttr('disabled');

            // Hidden fields in forms populated.
            var selectedId = $(this).parent().prev().val();
            $('#selectedId').val(selectedId);
            $('#groupIdEditModal').val(selectedId);
            $('#groupIdRemoveModal').val(selectedId);
        }
    });

    $('#btnClearFilterGroup').click(function () {
        $('#btnEditGroup').addClass('disabled');
        $('#btnEditGroup').attr('disabled', 'disabled');
        $('#btnRemoveGroup').addClass('disabled');
        $('#btnRemoveGroup').attr('disabled', 'disabled');

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
};

function expensesContainerInitialize() {

    $('#expensesContainer tr').click(function () {
        var editable = $(this).find('input.editable').val() == 'True';
        if (editable) {
            $('#expensesContainer tr').removeClass('active');
            $(this).addClass('active');
            $('#btnEditExpense').removeClass('disabled');
            $('#btnEditExpense').removeAttr('disabled');
            $('#btnRemoveExpense').removeClass('disabled');
            $('#btnRemoveExpense').removeAttr('disabled');
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
};

function modalsInitialize() {

    $('#groupSelectAddModal').change(function () {
        loadGroupParticipantsToOptionList($(this).val(), '#participantsAddExpenseModal', false);
    });
    loadGroupParticipantsToOptionList($('#groupSelectAddModal').val(), '#participantsAddExpenseModal', false);

    $('#groupSelectEditModal').change(function () {
        loadGroupParticipantsToOptionList($(this).val(), '#participantsEditExpenseModal', true);
    });
};

function loadGroupParticipantsToOptionList(groupId, optionListId, editMode) {
    if (groupId) {
        $.ajax({
            cache: false,
            url: '/Dashboard/_MembersSelectByGroup/' + groupId,
            success: function (result) {
                $(optionListId).html(result);
                if (!editMode) {
                    $(optionListId).find('option').prop('selected', 'selected');
                }
            }
        });
    }
};

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

function transfersContainerInitialize() {
    $('#transfersContainer tr').click(function () {
        var editable = $(this).find('input.editable').val() == 'True';
        if (editable) {
            $('#btnRemoveTransfer').removeClass('disabled');
            $('#btnRemoveTransfer').removeAttr('disabled');
            var selectedTransferId = $(this).find('input.transferId').val();
            $('#selectedTransferId').val(selectedTransferId);
        }
    });

    $('#btnRemoveTransfer').click(function () {
        var expenseId = $('#selectedTransferId').val();
        $('#transferIdRemoveModal').val(expenseId);
    });
};