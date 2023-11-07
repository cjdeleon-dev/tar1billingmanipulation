function loaddata() {
    if (!hasRecords) {
        alert('No record(s) to display.');
    } else {
        $('#myTable1').DataTable({
            "ajax": {
                "url": "/ChangeNameListForBODRes/loaddata",
                "type": "GET",
                "datatype": "json"
            },
            "columns": [
                { "data": "Id", "autoWidth": true },
                { "data": "ApplicationDate", "autoWidth": true },
                { "data": "NameOld", "autoWidth": true },
                { "data": "Address", "autoWidth": true },
                { "data": "ORNoOld", "autoWidth": true },
                { "data": "ORDateOld", "autoWidth": true },
                { "data": "NameNew", "autoWidth": true },
                { "data": "ORNoNew", "autoWidth": true },
                { "data": "ORDateNew", "autoWidth": true },
                { "data": "AccountNo", "autoWidth": true },
                { "data": "Remarks", "autoWidth": true }
            ]
        });
    }
}

function hasRecords() {
    var resval = false;


    return resval;
}