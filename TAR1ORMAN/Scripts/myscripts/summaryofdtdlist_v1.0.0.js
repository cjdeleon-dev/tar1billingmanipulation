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
            "url": "/DisconRptSummaryList/loadfordata",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Id", "autoWidth": true },
            { "data": "DateTimeGenerate", "autoWidth": true },
            { "data": "GenerateUserId", "autoWidth": true },
            { "data": "RouteId", "autoWidth": true },
            { "data": "CheckBy", "autoWidth": true },
            { "data": "NotedBy", "autoWidth": true }
        ],
        "aoColumnDefs": [
            {
                "aTargets": [6],
                "mData": "Id",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" onclick="viewReportById(\'' + data + '\')"><i class="glyphicon glyphicon-eye-open"></i> View</button>';
                },
                "className": "text-center"
            }
        ]
    });
}

function viewReportById(id) {

    console.log(id);

    var parent = $('embed#sodrpdf').parent();
    var newElement = '<embed src="/DisconRptSummaryList/PrintGeneratedSummaryOfDisconReport?headerId=' + id +  '" width="100%" height="800" type="application/pdf" id="sodrpdf">';

    $('embed#sodrpdf').remove();
    parent.append(newElement);

    $('#myRptModal').modal('show');
}