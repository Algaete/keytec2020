



$(document).ready(function () {
	$(document).on('click', '#reinicio', function () {
		var id = $(this).val();
		var first = $('#sn' + id).text();
		

		
		$('#sn').val(first);
		
	});
});

function llenarSnModal(snValue) {
	document.getElementById("sn").value = snValue;
}