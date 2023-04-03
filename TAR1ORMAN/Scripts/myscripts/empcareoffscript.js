function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadDataTable1() {
    $('#myTable1').DataTable({
        "ajax": {
            "url": "/EmpPowerBill/loadfordata",
            "type": "GET",
            "datatype": "json"
        },
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
}

function loadProcess() {
    hideunneccessaryoptions();
    loadDataTable1();
}