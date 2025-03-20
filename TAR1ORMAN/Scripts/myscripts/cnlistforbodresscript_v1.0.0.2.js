
function emptyTable() {
    var table = $('#myTable1').DataTable();

    //clear datatable
    table.clear().draw();

    //destroy datatable
    table.destroy();
}
function loaddata() {

    emptyTable();

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    var dtfr = $('#dtpFrom').val();
    var dtto = $('#dtpTo').val();
    var dts = dtfr + "~" + dtto;

    $.ajax({
        url: "/ChangeNameListForBODRes/checkifhasrecords?daterange=" + dts,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result) {
                var fileName = "CHANGENAMEAPP_" + dtfr.replace(/\-/g, "") + "_TO_" + dtto.replace(/\-/g, "");

                $('#myTable1').DataTable({
                    ajax: {
                        "url": "/ChangeNameListForBODRes/loaddata?daterange=" + dts,
                        "type": "GET",
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
                        { "data": "Remarks", "autoWidth": true },
                        { "data": "RptRemark", "autoWidth": true },
                        { "data": "DateApproved", "autoWidth": true }
                    ]
                });
            } else {
                document.body.style.cursor = 'default';
                $('#modalLoading').modal('hide');
                alert('There are no data to load.');
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}
