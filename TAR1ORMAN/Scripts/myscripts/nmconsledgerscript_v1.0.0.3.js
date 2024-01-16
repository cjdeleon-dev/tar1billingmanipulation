
function loaddata() {
    getConsumerDetails();
    initloadTableBody();
    var element = document.getElementById('myTable1');
    element.scrollTop = element.scrollHeight;
    document.getElementById('btnNSave').disabled = true;
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

    var table = $('#myTable1').DataTable({
        ajax: {
            "url": "/NetMetering/LoadLedgerByAccountNo?actno=" + acctno,
            "type": "GET",
            "datatype": "json"
        },
        paging: false,
        scrollCollapse: true,
        scrollY: '500px',
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

            this.a
            
        },
        columns: [
            { "data": "TrxSeqId", "visible": false },
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

function saveNewBill() {
    let acctno = $('#txtAccountNo').val();
    let netimp = parseInt($('#txtNCurrImp').val()) - parseInt($('#txtNPrevImp').val());
    let netexp = parseInt($('#txtNCurrExp').val()) - parseInt($('#txtNPrevExp').val());

    var obj = {
        ConsumerId: acctno,
        PrevImp: $('#txtNPrevImp').val(),
        CurrImp: $('#txtNCurrImp').val(),
        NetImp: netimp,
        PrevExp: $('#txtNPrevExp').val(),
        CurrExp: $('#txtNCurrExp').val(),
        NetExp: netexp,
        PrevRec: $('#txtNPrevRec').val(),
        CurrRec: $('#txtNCurrRec').val(),
        Demand: $('#txtNDemand').val(),
        TrxDate: $('#txtNTrxDate').val(),
        BillPeriod: $('#txtNBillPeriod').val(),
        EntryUser:""
    }

    $.ajax({
        url: "/NetMetering/SaveNewNetMeteringBill/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (result) {
            if (result.data != null) {
                if (result.data == true) {
                    alert("The bill has been successfully posted.");
                    window.location = "/NetMetering/SetAccountLedger?accountNo=" + acctno;
                }                    
                else
                    alert("Unable to post the bill.");
            }
            else {
                alert("Sorry, something went wrong.");
            }

        },
        error: function (errormessage) {

            alert(errormessage.responseText);
        }
    });

}

function getCurrentBillPeriod() {
    $.ajax({
        url: "/NetMetering/GetCurrentBillPeriod/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result.data!="") {
                $('#txtNBillPeriod').val(result.data);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getDemandByAccountNo() {
    var actno = $('#txtAccountNo').val();
    $.ajax({
        url: "/NetMetering/GetDemandByAccountNo?acctno=" + actno,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#txtNDemand').val(result.data);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getPrevReadingByAccountNo() {
    var actno = $('#txtAccountNo').val();

    $.ajax({
        url: "/NetMetering/GetPrevReadingByAccountNo?acctno=" + actno,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result.data != null) {
                $('#txtNPrevImp').val(result.data.PrevImp);
                $('#txtNPrevExp').val(result.data.PrevExp);
                $('#txtNPrevRec').val(result.data.PrevRec);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function showNewBill() {

    //clear all controls first.
    clearAllControls();

    let today = new Date().toISOString().slice(0, 10)

    $('#newTransactionModal').modal('show');
    $('#txtNAcctNo').val($('#txtAccountNo').val());
    getCurrentBillPeriod();
    getDemandByAccountNo();
    getPrevReadingByAccountNo();
    $('#txtNTrxDate').val(today);
}

function processBill() {
    let netimp = parseInt($('#txtNCurrImp').val()) - parseInt($('#txtNPrevImp').val());
    let netexp = parseInt($('#txtNCurrExp').val()) - parseInt($('#txtNPrevExp').val());
    let acctno = $('#txtAccountNo').val();

    if (netimp < 0) {
        alert("Invalid input of previous and current import values.");
        return false;
    }

    if (netexp < 0) {
        alert("Invalid input of previous and current export values.");
        return false;
    }

    var obj = {
        ConsumerId: acctno,
        BillPeriod: $('#txtNBillPeriod').val(),
        NetImport: netimp,
        NetExport: netexp,
        ActualDemand: $('#txtNDemand').val()
    }

    $.ajax({
        url: "/NetMetering/ProcessBill/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(obj),
        dataType: "json",
        success: function (result) {
            if (result.data != null) {
                $('#txtNEnergyAmt').val(numberWithCommas(result.data.EnergyAmount));
                $('#txtNDemandAmt').val(numberWithCommas(result.data.DemandAmount));
                $('#txtNBillAmt').val(numberWithCommas(result.data.BillAmount));
                $('#txtNBVatAmt').val(numberWithCommas(result.data.VATAmount));
                $('#txtNTotalAmt').val(numberWithCommas(result.data.TotalAmount));
                $('#txtNNetBillAmt').val(numberWithCommas(result.data.NetBillAmount));
                $('#txtNNetVatAmt').val(numberWithCommas(result.data.VATAmount));
                $('#txtNCurrBillAmt').val(numberWithCommas(result.data.TotalCurrentBill));

                document.getElementById('btnNSave').disabled = false;
            }
            else {
                alert("Sorry unable to calculate details.");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

    return true;
}

function clearAllControls() {
    $('#txtNAcctNo').val("");
    $('#txtNBillPeriod').val("");
    $('#txtNTrxDate').val("");
    $('#txtNPrevImp').val("");
    $('#txtNCurrImp').val("");
    $('#txtNPrevExp').val("");
    $('#txtNCurrExp').val("");
    $('#txtNPrevRec').val("");
    $('#txtNCurrRec').val("");
    $('#txtNDemand').val("");

    $('#txtNEnergyAmt').val("");
    $('#txtNDemandAmt').val("");
    $('#txtNBillAmt').val("");
    $('#txtNBVatAmt').val("");
    $('#txtNTotalAmt').val("");

    $('#txtNNetBillAmt').val("");
    $('#txtNNetVatAmt').val("");
    $('#txtNCurrBillAmt').val("");
}

function resetEntry() {
    clearAllControls();

    let today = new Date().toISOString().slice(0, 10)

    $('#txtNAcctNo').val($('#txtAccountNo').val());
    getCurrentBillPeriod();
    getDemandByAccountNo();
    getPrevReadingByAccountNo();
    $('#txtNTrxDate').val(today);
}

function rebuildAccount() {
    var acctno = $('#txtAccountNo').val();

    $.ajax({
        url: "/NetMetering/RebuildByAccountNo/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify({ acctno }),
        dataType: "json",
        success: function (result) {
            if (result.data != null) {
                if (result.data = true) {
                    alert('Rebuilding account has been done.');
                    window.location = "/NetMetering/SetAccountLedger?accountNo=" + acctno;
                }
                else
                    alert('Rebuilding is not possible.');
            }
            else {
                alert("Sorry unable to rebuild account's ledger.");
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}