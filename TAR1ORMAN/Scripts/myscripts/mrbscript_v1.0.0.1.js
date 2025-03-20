function getLogsByDate() {

    emptyTable();

    var seldate = $('#dtpDate').val();

    if (seldate == '') {
        alert('Please select Date to process.');
        return false;
    }
        

    var fileName = "MRBLOG_OF_" + seldate;

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";


    $.ajax({
        type: 'GET',
        url: "/MRBLogs/loaddata?seldate=" + seldate,
        mimeType: 'json',
        success: function (data) {

            $('#tblMRBLogs').DataTable({
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
                    { "data": "Name", "autoWidth": true },
                    { "data": "RouteId", "autoWidth": true },
                    { "data": "Start", "autoWidth": true },
                    { "data": "End", "autoWidth": true },
                    { "data": "Total", "autoWidth": true }
                ]
            });
        },
        error: function () {
            document.body.style.cursor = 'progress';
            $('#modalLoading').modal('show');
            document.getElementById("spintext").innerHTML = "LOADING...";
        }
    });

    //$('#tblMRBLogs').DataTable({
    //    ajax: {
    //        url: "/MRBLogs/loaddata?seldate=" + seldate,
    //        type: "GET",
    //        datatype: "json"
    //    },
    //    pageLength: 25,
    //    dom: "Bfrtip",
    //    buttons: [
    //        {
    //            extend: "excel",
    //            filename: fileName,
    //            text: "EXPORT TO EXCEL",
    //        }
    //    ],
    //    initComplete: function (settings, json) {
    //        document.body.style.cursor = 'default';
    //        $('#modalLoading').modal('hide');
    //    },
    //    columns: [
    //        { "data": "Name", "autoWidth": true },
    //        { "data": "RouteId", "autoWidth": true },
    //        { "data": "Start", "autoWidth": true },
    //        { "data": "End", "autoWidth": true },
    //        { "data": "Total", "autoWidth": true }
    //    ]
    //});
}

function emptyTable() {
    var table = $('#tblMRBLogs').DataTable();

    //clear datatable
    table.clear().draw();

    //destroy datatable
    table.destroy();
}