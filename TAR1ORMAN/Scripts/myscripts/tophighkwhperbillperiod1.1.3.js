function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadDataTable1() {

    var bperiod = $("#txtBillPeriod").val();
    var top = $("#txtTop").val();

    $('#myTable1').DataTable().destroy();

    $('#myTable1').DataTable({
        "ajax": {
            "url": "/TopHighKwh/loadfordata?billperiodtop=" + bperiod + "_" + top,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "PoleId", "autoWidth": true },
            { "data": "MeterNo", "autoWidth": true },
            { "data": "KwH", "autoWidth": true },
            { "data": "Amount", "autoWidth": true },
        ]
    }).order([6,'desc']);
}

function loadPage() {
    hideunneccessaryoptions();
    loadcboBillPeriod();
}

function processdata() {
    
    loadDataTable1();
}


function exportData() {

    var statustown = $("#txtStatus").val() + "_" + $("#txtTown").val();

    $.ajax({
        data: { stattown: statustown },
        url: "/TopHighKwh/ExportToExcel/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            alert("Successfully Exported");
        },
        error: function (errormessage) {
            alert("Error in Exporting");
        },
    });

}


function loadcboBillPeriod() {
    $.ajax({
        url: "/TopHighKwh/GetAllBillPeriod/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboBillPeriod').empty();
            $('#cboBillPeriod').val(0);
            $('#cboBillPeriod').append("<option value=0>Select BillPeriod</option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].BillPeriod;
                var opt = new Option(Desc, result[i].Id);
                $('#cboBillPeriod').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function cboBPOnChange() {
    $("#txtBillPeriod").val(document.getElementById("cboBillPeriod").value);
}

function cboTopOnChange() {
    $("#txtTop").val(document.getElementById("cboTop").value);
}