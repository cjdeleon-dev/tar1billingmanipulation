function loadAllData() {

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    var fileName = "READINGREMARKS";

    $.ajax({
        type: 'GET',
        url: "/ReadingRemark/GetAllReadingRemarks",
        mimeType: 'json',
        success: function (data) {
            if (data != null) {
                $('#myTable1').DataTable({
                    "data": data.data,
                    "bLengthChange": false,
                    "pageLength": 15,
                    "dom": "Bfrtip",
                    "bUseRendered": false,
                    "buttons": [
                        {
                            extend: "excel",
                            filename: fileName,
                            text: "EXPORT TO EXCEL",
                        }
                    ],
                    "columns": [
                        { "data": "ConsumerId", "autoWidth": true },
                        { "data": "Name", "autoWidth": true },
                        { "data": "Address", "autoWidth": true },
                        { "data": "MeterSerialNo", "autoWidth": true },
                        { "data": "ErrText", "autoWidth": true },
                        { "data": "Remark", "autoWidth": true }
                    ],
                    "initComplete": function (settings, json) {
                        
                        document.body.style.cursor = 'default';
                        $('#modalLoading').modal('hide');

                    }
                });
            } else {
                document.body.style.cursor = 'default';
                $('#modalLoading').modal('hide');
                alert("No data to be displayed.");
            }

        },
        error: function () {
            //document.body.style.cursor = 'progress';
            //$('#modalLoading').modal('show');
            //document.getElementById("spintext").innerHTML = "LOADING...";
        }
    });
}