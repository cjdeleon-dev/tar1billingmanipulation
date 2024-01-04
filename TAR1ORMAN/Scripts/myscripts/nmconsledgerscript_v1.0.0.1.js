function loaddata() {
    initloadTableBody();
}


function getConsumerDetails(acctno) {

}

function initloadTableBody() {
    var acctno = $('#txtAccountNo').val();
    var fileName = "consumersledger_" + acctno;

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    $('#myTable1').DataTable({
        ajax: {
            "url": "/NetMetering/LoadLedgerByAccountNo?actno=" + acctno,
            "type": "GET",
            "datatype": "json"
        },
        paging: false,
        scrollCollapse: true,
        scrollY: '400px',
        order: [[1, 'asc']],
        pageLength: -1,
        searching: false,
        info: false,
        dom: 'lrt',
        initComplete: function (settings, json) {
            document.body.style.cursor = 'default';
            $('#modalLoading').modal('hide');

            $('#txtTotalMonths').val("Total Month(s): " + json.data[0]["Months"]);
            $('#txtTotalTrxBalance').val("PHP " + json.data[0]["TotalTrxBalance"]);
            $('#txtTotalVatBalance').val("PHP " + json.data[0]["TotalVatBalance"]);
        },
        columns: [
            { "data": "TrxSeqId", "autoWidth": true },
            { "data": "TrxDate", "autoWidth": true },
            { "data": "Trx", "autoWidth": true },
            { "data": "Period", "autoWidth": true },
            { "data": "Prev", "autoWidth": true },
            { "data": "Curr", "autoWidth": true },
            { "data": "KWh", "autoWidth": true },
            { "data": "DMU", "autoWidth": true },
            { "data": "TrxAmount", "autoWidth": true },
            { "data": "TrxBalance", "autoWidth": true },
            { "data": "VAT", "autoWidth": true },
            { "data": "VATBalance", "autoWidth": true },
            { "data": "TotalTrxBalance", "visible": false },
            { "data": "TotalVatBalance", "visible": false },
            { "data": "Months", "visible": false }
        ],
        aoColumnDefs: [
            {
                "aTargets": [15],
                "mData": "TrxSeqId",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                        'onclick="showLedgerByAcctNo(\'' + data + '\')">' +
                        '<i class="glyphicon glyphicon-tasks"></i> VIEW</button> ';
                },
                "className": "text-center"
            }
        ]
    }); 
}