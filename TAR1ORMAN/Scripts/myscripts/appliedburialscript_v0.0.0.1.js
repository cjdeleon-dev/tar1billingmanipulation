function loaddata() {
    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    $.ajax({
        type: 'GET',
        url: "/AppliedBurial/GetAllAppliedBurial",
        mimeType: 'json',
        success: function (data) {
            if (data != null) {
                $('#myTable1').DataTable({
                    "data": data.data,
                    "bLengthChange": false,
                    "pageLength": 15,
                    "columns": [
                        { "data": "Id", "autoWidth": true },
                        { "data": "ConsumerId", "autoWidth": true },
                        { "data": "Name", "autoWidth": true },
                        { "data": "Address", "autoWidth": true },
                        { "data": "AppValDate", "autoWidth": true }
                    ],
                    "initComplete": function (settings, json) {

                        document.body.style.cursor = 'default';
                        $('#modalLoading').modal('hide');

                    },
                    "columnDefs": [{
                        "targets": 5,
                        "className": 'dt-body-center',
                        "render": function (data, type, full) {
                            return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + full.Id + '" ' +
                                'onclick="showModalByRefId(' + full.Id + ')">' +
                                '<i class="glyphicon glyphicon-print"></i></button>';
                        }
                    }]

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

function showModalByRefId(id) {
    var parent = $('embed#bvrpdf').parent();
    var newElement = '<embed src="/AppliedBurial/PrintBurialApplicationResult?headerid=' + id + '"  width="100%" height="800" type="application/pdf" id="bvrpdf">';

    $('embed#bvrpdf').remove();
    parent.append(newElement);

    $('#myRptModal').modal('show');
}