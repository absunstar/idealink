
$(document).ready(function () {
	

$('#tourbookdeparturedate').attr('disabled', 'disabled');
$('#tourbookdeparturedate').addClass('disabled');
$("#tourbookarrivaldate").on('dp.change',function () { 
$('#tourbookdeparturedate').removeAttr('disabled', 'disabled');
$('#tourbookdeparturedate').removeClass('disabled');
});
$('#tourbookarrivaldate').datetimepicker({
format: 'dddd, DD MMMM YYYY'
});
$('#tourbookarrivaldate').data("DateTimePicker").minDate(moment().add(7,'days'));
$('#tourbookdeparturedate').datetimepicker({
useCurrent: false,
format: 'dddd, DD MMMM YYYY'
});
$("#tourbookarrivaldate").on("dp.change", function (e) {
$('#tourbookdeparturedate').data("DateTimePicker").minDate(e.date);
});
$("#tourbookdeparturedate").on("dp.change", function (e) {
$('#tourbookarrivaldate').data("DateTimePicker").maxDate(e.date);
});


});

