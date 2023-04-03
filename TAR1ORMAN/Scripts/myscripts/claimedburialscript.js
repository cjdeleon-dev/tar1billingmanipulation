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
            "url": "/ClaimedBurial/loadfordata",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "ClaimedDate", "autoWidth": true }
        ]
    });
}

function loadProcess() {
    hideunneccessaryoptions();
    loadDataTable1();
}