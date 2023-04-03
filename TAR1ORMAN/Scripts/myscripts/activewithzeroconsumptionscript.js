function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadPage() {
    hideunneccessaryoptions();
    loadDataTable();
}

function loadDataTable() {
    $('#myTable1').DataTable({
        "ajax": {
            "url": "/ActiveWithZeroConsumption/loadfordata",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "AccountName", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "MeterNo", "autoWidth": true },
            { "data": "PoleId", "autoWidth": true },
            { "data": "Type", "autoWidth": true },
            { "data": "ReadKWH", "autoWidth": true }

        ]
    });
}





