function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadDataTable1() {

    var stat = $("#txtStatus").val();
    var town = $("#txtTown").val();
    var top = $("#txtTop").val();

    $('#myTable1').DataTable().destroy();

    $('#myTable1').DataTable({
        "ajax": {
            "url": "/TopActiveDisconByTown/loadfordata?stattowntop=" + stat + "_" + town + "_" + top,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNumber", "autoWidth": true },
            { "data": "Amount", "autoWidth": true },
            { "data": "VAT", "autoWidth": true },
            { "data": "NumOfMonths", "autoWidth": true },
            { "data": "ConsumerType", "autoWidth": true },
            { "data": "AccountName", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "MeterNumber", "autoWidth": true },
            { "data": "PoleNumber", "autoWidth": true },
            { "data": "Town", "autoWidth": true }
            
        ]
    });
}

function loadPage() {
    hideunneccessaryoptions();
    loadcboStatus();
    loadcboTown();
}

function processdata() {
    
    loadDataTable1();
}


function exportData() {

    var statustown = $("#txtStatus").val() + "_" + $("#txtTown").val();

    $.ajax({
        data: { stattown: statustown },
        url: "/TopActiveDisconByTown/ExportToExcel/",
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

function loadcboStatus() {
    $.ajax({
        url: "/TopActiveDisconByTown/GetStatus/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboStatus').empty();
            $('#cboStatus').val(0);
            $('#cboStatus').append("<option value=0>Select Status</option>");
            $("#txtStatus").val(0);
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].CStatus;
                var opt = new Option(Desc, result[i].CStatus);
                $('#cboStatus').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function loadcboTown() {
    $.ajax({
        url: "/TopActiveDisconByTown/GetTown/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboTown').empty();
            $('#cboTown').val(0);
            $('#cboTown').append("<option value=0>Select Town</option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].CTown;
                var opt = new Option(Desc, result[i].CTown);
                $('#cboTown').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function cboStatusOnChange() {
    $("#txtStatus").val(document.getElementById("cboStatus").value);
}

function cboTownOnChange() {
    $("#txtTown").val(document.getElementById("cboTown").value);
}

function cboTopOnChange() {
    $("#txtTop").val(document.getElementById("cboTop").value);
}