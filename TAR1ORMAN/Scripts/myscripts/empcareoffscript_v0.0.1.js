function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadDataTable1() {
    var fileName = "TAR1EMPCAREOF";

    $.ajax({
        type: 'GET',
        url: "/EmpPowerBill/loadfordata",
        mimeType: 'json',
        success: function (data) {
            $('#myTable1').DataTable({
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
                "columns": [
                    { "data": "EmployeeName", "autoWidth": true },
                    { "data": "AccountNo", "autoWidth": true },
                    { "data": "AccountName", "autoWidth": true },
                    { "data": "NumOfMonths", "autoWidth": true },
                    { "data": "PowerBill", "autoWidth": true },
                    { "data": "VAT", "autoWidth": true },
                    { "data": "Surcharge", "autoWidth": true },
                    { "data": "Total", "autoWidth": true },
                    { "data": "Status", "autoWidth": true }
                ]
            });
        },
        error: function () {
            alert('Fail!');
        }
    });

   
}

function loadProcess() {
    hideunneccessaryoptions();
    loadDataTable1();
}