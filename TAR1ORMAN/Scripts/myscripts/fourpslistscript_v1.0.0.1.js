function loaddata() {

    $('#myTable1').DataTable({
        ajax: {
            "url": "/FourPsAppliedList/GetAppliedList/",
            "type": "GET",
            "datatype": "json"
        },
        pageLength: 25,
        dom: 'Bfrtip',
        initComplete: function (settings, json) {
            document.body.style.cursor = 'default';
            $('#modalLoading').modal('hide');
        },
        columns: [
            { "data": "Id", "autoWidth": true },
            { "data": "DateApplied", "autoWidth": true },
            { "data": "IsQualified", "autoWidth": true },
            { "data": "Surname", "autoWidth": true },
            { "data": "Givenname", "autoWidth": true },
            { "data": "Middlename", "autoWidth": true },
            { "data": "Address", "autoWidth": true }
        ],
        "aoColumnDefs": [
            {
                "aTargets": [7],
                "mData": "Id",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                        'onclick="showModalByRefId(' + data + ')">' +
                        '<i class="glyphicon glyphicon-eye-open"></i> VIEW</button> ';
                },
                "className": "text-center"
            }
        ]
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