function loaddata() {
    getConsumerDetails();
    initloadTableBody();
}


function getConsumerDetails() {
    var acctno = $('#txtAccountNo').val();
    $.ajax({
        url: "/NetMetering/GetAccountDetails?accountNo=" + acctno,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                $('#txtName').val(result.data.AccountName);
                $('#txtAddress').val(result.data.Address);
                $('#txtStatus').val(result.data.Status);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function initloadTableBody() {
    var acctno = $('#txtAccountNo').val();

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
        //language: {
        //    "decimal": ".",
        //    "thousands": ","
        //},
        createdRow: function (row, data, dataIndex) {
            if (data.isBalance === true) {
                $(row).css('color', 'red');
                $(row).css('font-weight', 'bold');
            }
        },
        dom: 'lrt',
        initComplete: function (settings, json) {
            document.body.style.cursor = 'default';
            $('#modalLoading').modal('hide');

            var mons = json.data[0]["Months"].toString();
            var trxbal = json.data[0]["TotalTrxBalance"].toString();
            var vatbal = json.data[0]["TotalVatBalance"].toString();

            $('#txtTotalMonths').val("Total Month(s): " + mons);
            $('#txtTotalTrxBalance').val("PHP " + numberWithCommas(trxbal));
            $('#txtTotalVatBalance').val("PHP " + numberWithCommas(vatbal));
            
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
            { "data": "Months", "visible": false },
            { "data": "isBalance", "visible": false }
        ],
        aoColumnDefs: [
            {
                "aTargets": [16],
                "mData": "TrxSeqId",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                        'onclick="showLedgerByAcctNo(\'' + data + '\')">' +
                        '<i class="glyphicon glyphicon-tasks"></i> VIEW</button> ';
                },
                "className": "text-center"
            },
            {
                "aTargets": [8, 9, 10, 11],
                "mRender": function (data, type, full) {
                    return 'PHP ' + parseFloat(data).toLocaleString('en-US');
                }
            }
        ]
    });

}