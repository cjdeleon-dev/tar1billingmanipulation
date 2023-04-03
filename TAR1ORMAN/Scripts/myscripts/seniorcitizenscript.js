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
            "url": "/SeniorCitizen/loadfordata",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "AppliedDate", "autoWidth": true },
            { "data": "ExpiryDate", "autoWidth": true }
        ]
    });
}

function loadProcess() {
    hideunneccessaryoptions();
    loadDataTable1();
}