function loaddata() {
    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    $.ajax({
        type: 'GET',
        url: "/FourPsAppliedList/GetAppliedList",
        mimeType: 'json',
        success: function (data) {
            if (data != null) {
                $('#myTable1').DataTable({
                    "data": data.data,
                    "bLengthChange": false,
                    "pageLength": 15,
                    "columns": [
                        { "data": "Id", "autoWidth": true },
                        { "data": "DateApplied", "autoWidth": true },
                        { "data": "IsQualified", "autoWidth": true },
                        { "data": "Surname", "autoWidth": true },
                        { "data": "Givenname", "autoWidth": true },
                        { "data": "Middlename", "autoWidth": true },
                        { "data": "Address", "autoWidth": true }
                    ],
                    "initComplete": function (settings, json) {

                        document.body.style.cursor = 'default';
                        $('#modalLoading').modal('hide');

                    },
                    "columnDefs": [
                    {
                        "targets": 2,
                        "className": 'dt-body-center',
                        "render": function (data, type, full) {
                            if (data === true)
                                return '<input type="checkbox" checked style="pointer-events:none;" />';
                            else
                                return '<input type="checkbox" style="pointer-events:none;" />';
                        }
                    },
                    {
                        "targets": 7,
                        "className": 'dt-body-center',
                        "render": function (data, type, full) {
                            return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + full.Id + '" ' +
                                'onclick="showModalByRefId(' + full.Id + ')">' +
                                '<i class="glyphicon glyphicon-print"></i></button>';
                        }
                    }
                    ]

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

function showModalByRefId(refId) {
    //preview report form for printing
    var parent = $('embed#fourpsqmepdf').parent();
    var newElement = '<embed src="/FourPsAppliedList/PreviewQualifiedLifelinerReport?rptid=' + refId + '"  width="100%" height="800" type="application/pdf" id="fourpsqmepdf">';

    $('embed#fourpsqmepdf').remove();
    parent.append(newElement);

    $('#myRptFormModal').modal('show');
}