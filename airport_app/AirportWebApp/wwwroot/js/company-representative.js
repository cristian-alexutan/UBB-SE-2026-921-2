$(function () {

    /* Add modal */
    $('#openAddModal').on('click', function () {
        $('#addModal').addClass('open');
    });

    if ($('.validation-summary-errors').length > 0) {
        $('#addModal').addClass('open');
    }

    $('#cancelAdd').on('click', function () {
        $('#addModal').removeClass('open');
    });

    $('#addModal').on('click', function (event) {
        if ($(event.target).is('#addModal')) {
            $('#addModal').removeClass('open');
        }
    });

    /* Recurrent toggle */
    $('#recurrentCheck').on('change', function () {
        if ($(this).is(':checked')) {
            $('#recurrent-section').addClass('show');
            $('#singleDateWrap').hide();
        } else {
            $('#recurrent-section').removeClass('show');
            $('#singleDateWrap').show();
        }
    });

    /* Recurrence type -> custom days */
    $('#recurrenceType').on('change', function () {
        var isCustom = $(this).val() === 'Custom';
        $('#custom-days-wrap').toggleClass('show', isCustom);
        $('#customDaysText').prop('required', isCustom);
        if (!isCustom) {
            $('#customDaysText').val('');
        } else if (!$('#customDaysText').val()) {
            $('#customDaysText').val('1');
        }
    });

    $('#recurrenceType').trigger('change');

    /* Delete modal */
    $(document).on('click', '.btn-delete', function () {
        var flightId = $(this).data('id');
        var flightNumber = $(this).data('num');
        $('#deleteFlightId').val(flightId);
        $('#deleteConfirmText').text('Are you sure you want to delete flight ' + flightNumber + '?');
        $('#deleteModal').addClass('open');
    });

    $('#cancelDelete').on('click', function () {
        $('#deleteModal').removeClass('open');
    });

    $('#deleteModal').on('click', function (event) {
        if ($(event.target).is('#deleteModal')) {
            $('#deleteModal').removeClass('open');
        }
    });

    /* Live search (debounced) */
    var searchTimer;
    $('#searchInput').on('input', function () {
        clearTimeout(searchTimer);
        searchTimer = setTimeout(function () {
            $('#searchForm').submit();
        }, 420);
    });

    /* Time input: zero-pad on blur */
    $('.time-part').on('blur', function () {
        var numericValue = parseInt($(this).val(), 10);
        if (!isNaN(numericValue)) {
            $(this).val(String(numericValue).padStart(2, '0'));
        }
    });

    /* Compute hidden DepartureOffset / ArrivalOffset */
    $('#addFlightForm').on('submit', function () {
        function toMinutes(hourElement, minuteElement, amPmElement) {
            var hour = parseInt($(hourElement).val(), 10) || 0;
            var minute = parseInt($(minuteElement).val(), 10) || 0;
            var amPm = $(amPmElement).val();
            if (amPm === 'AM' && hour === 12) hour = 0;
            if (amPm === 'PM' && hour !== 12) hour += 12;
            return hour * 60 + minute;
        }

        $('<input>').attr({ type: 'hidden', name: 'DepartureOffsetMinutes', value: toMinutes('#depHour', '#depMin', '#depAmPm') }).appendTo(this);
        $('<input>').attr({ type: 'hidden', name: 'ArrivalOffsetMinutes', value: toMinutes('#arrHour', '#arrMin', '#arrAmPm') }).appendTo(this);
    });

});
