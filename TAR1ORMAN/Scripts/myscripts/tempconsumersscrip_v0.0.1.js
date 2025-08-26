function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadData() {
    //$('#myTable1').DataTable({
    //    "ajax": {
    //        "url": "/TempConsumers/loadfordata",
    //        "type": "GET",
    //        "datatype": "json"
    //    },
    //    "columns": [
    //        { "data": "AccountNo", "autoWidth": true },
    //        { "data": "Name", "autoWidth": true },
    //        { "data": "Address", "autoWidth": true },
    //        { "data": "MeterNo", "autoWidth": true },
    //        { "data": "PoleNo", "autoWidth": true },
    //        { "data": "TrxBalance", "autoWidth": true },
    //        { "data": "VATBalance", "autoWidth": true }
    //    ],
    //    //aoColumnDefs: [
    //    //    {
    //    //        "aTargets": [5],
    //    //        "mData": "AccountNo",
    //    //        "mRender": function (data, type, full) {

    //    //            return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
    //    //                'onclick="showSCDetailsByAcctNo(\'' + data + '\')">' +
    //    //                '<i class="glyphicon glyphicon-edit"></i> VIEW ID NO.</button> ';
    //    //        },
    //    //        "className": "text-center"
    //    //    }
    //    //]
    //});

    const today = new Date();
    const yyyy = today.getFullYear();
    let mm = today.getMonth() + 1; // Months start at 0!
    let dd = today.getDate();

    if (dd < 10) dd = '0' + dd;
    if (mm < 10) mm = '0' + mm;

    var fileName = "TEMPCONSUMERS_ASOF_" + mm+dd+yyyy;

    $.ajax({
        type: 'GET',
        url: "/TempConsumers/loadfordata",
        mimeType: 'json',
        success: function (data) {

            $('#myTable1').DataTable({
                "data": data.data,
                "pageLength": 15,
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
                    { "data": "AccountNo", "autoWidth": true },
                    { "data": "Name", "autoWidth": true },
                    { "data": "Address", "autoWidth": true },
                    { "data": "MeterNo", "autoWidth": true },
                    { "data": "PoleNo", "autoWidth": true },
                    { "data": "TrxBalance", "autoWidth": true },
                    { "data": "VATBalance", "autoWidth": true },
                    { "data": "Status", "autoWidth": true },
                    { "data": "DateInstalled", "autoWidth": true },
                ],
            });
        },
        error: function () {
            document.body.style.cursor = 'progress';
            $('#modalLoading').modal('show');
            document.getElementById("spintext").innerHTML = "LOADING...";

            alert('Fail!');
        }
    });
}

function loadProcess() {
    hideunneccessaryoptions();
    loadData();
}