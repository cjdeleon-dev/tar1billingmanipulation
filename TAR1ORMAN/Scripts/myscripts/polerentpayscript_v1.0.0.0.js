function emptyTable() {
    var table = $('#tblPoleRentPayment').DataTable();

    //clear datatable
    table.clear().draw();

    //destroy datatable
    table.destroy();
}

function getPoleRentalPayments() {

    emptyTable();

    var seldatefr = $('#dtpDateFr').val();
    var seldateto = $('#dtpDateTo').val();

    if (seldateto == '' || seldatefr == '') {
        alert('Please select Date Range to process.');
        return false;
    }

    var fileName = "PRPAYMENTS_OF_" + seldatefr + "_" + seldateto;

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    $.ajax({
        type: 'GET',
        url: "/PoleRentalPaymentList/GetPoleRentPaymentByDateRange?dateFr=" + seldatefr + "&dateTo=" + seldateto,
        mimeType: 'json',
        success: function (data) {
            
            $('#tblPoleRentPayment').DataTable({
                "data": data.data,
                "pageLength": 25,
                "dom": "Bfrtip",
                "buttons": [
                    {
                        extend: "excel",
                        filename: fileName,
                        text: "EXPORT TO EXCEL",
                    }
                ],
                "initComplete": function (settings, json) {
                    document.body.style.cursor = 'default';
                    $('#modalLoading').modal('hide');
                },
                "columns": [
                    { "data": "ORNumber", "autoWidth": true },
                    { "data": "Payee", "autoWidth": true },
                    { "data": "Address", "autoWidth": true },
                    { "data": "TrxDate", "autoWidth": true },
                    { "data": "Amount", "autoWidth": true },
                    { "data": "VAT", "autoWidth": true },
                    { "data": "TotalAmount", "autoWidth": true }
                ]
            });
        },
        error: function () {
            document.body.style.cursor = 'progress';
            $('#modalLoading').modal('show');
            document.getElementById("spintext").innerHTML = "LOADING...";

            alert('Fail!');
        }
    });

}
