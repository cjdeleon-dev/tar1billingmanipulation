function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function processdata() {
    let enddate = $('#dtpDateEnd').val();
    let isEB = document.getElementById("chkIsEB").checked;
    let fileName = "";

    emptyTable();

    if (enddate == "") {
        alert("No selected date to process.");
        return false;
    }

    if (isEB)
        fileName = "AgingByDate_" + enddate + "TRX";
    else
        fileName = "AgingByDate_" + enddate + "VAT";

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    $('#myTable1').DataTable({
        ajax: {
            "url": "/ProcessJ/GetData?sdate=" + enddate + "&isEB=" + isEB,
            "type": "GET",
            "contentType": "application/json;charset=UTF-8",
            "datatype": "json"
        },
        pageLength: 25,
        dom: "Bfrtip",
        buttons: [{
            extend: "excel",
            filename: fileName,
            text: "EXPORT TO EXCEL",

        }],
        initComplete: function (settings, json) {
            document.body.style.cursor = 'default';
            $('#modalLoading').modal('hide');
        },
        columns: [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "ConsumerType", "autoWidth": true },
            { "data": "Status", "autoWidth": true },
            { "data": "Days30", "autoWidth": true },
            { "data": "Days60", "autoWidth": true },
            { "data": "Days90", "autoWidth": true },
            { "data": "Days180", "autoWidth": true },
            { "data": "Days240", "autoWidth": true },
            { "data": "Days360", "autoWidth": true },
            { "data": "AboveDays365", "autoWidth": true }
        ]
    });

}

function emptyTable() {
    var table = $('#myTable1').DataTable();

    //clear datatable
    table.clear().draw();

    //destroy datatable
    table.destroy();
}