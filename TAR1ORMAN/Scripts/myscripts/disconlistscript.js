
function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadPage() {
    hideunneccessaryoptions();
    loadcboRoute();

    var today = new Date();

    var yy = today.getFullYear();
    var mm = today.getMonth() + 1;
    var dd = today.getDate();

    yy = checkTime(yy);
    mm = checkTime(mm);
    dd = checkTime(dd);

    //console.log(mm + "-" + dd + "-" + yy);

    $('#txtDate').val(yy + "-" + mm + "-" + dd);

    $('#txtNumMonths').val("0");
    $('#txtStatus').val("0");
    $('#txtRoute').val("0");
}

function checkTime(i) {
    if (i < 10) { i = "0" + i };  // add zero in front of numbers < 10
    return i;
}

function loadcboRoute() {
    $.ajax({
        url: "/DisconList/getAllRoutes/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboRoute').empty();
            $('#cboRoute').val(0);
            $('#cboRoute').append("<option value=0>All Routes</option>");

            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Id + ' - ' + result[i].Route;
                var opt = new Option(Desc, result[i].Id);
                $('#cboRoute').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function processdata() {

    loadDataTable1();
}

function loadDataTable1() {
    var nomonth = $("#txtNumMonths").val();
    var stat = $("#txtStatus").val();
    var route = $("#txtRoute").val();

    console.log(nomonth + "_" + stat + "_" + route);

    $('#myTable1').DataTable().destroy();

    $('#myTable1').DataTable({
        "ajax": {
            "url": "/DisconList/loadfordata?nomonthstatroute=" + nomonth + "_" + stat+ "_" + route,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "AccountName", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "MeterNo", "autoWidth": true },
            { "data": "FirstBill", "autoWidth": true },
            { "data": "LastBill", "autoWidth": true },
            { "data": "NoOfMonths", "autoWidth": true },
            { "data": "Due", "autoWidth": true },
            { "data": "Remark", "autoWidth": true }

        ]
    });
}



function cboStatusOnChange() {
    $("#txtStatus").val(document.getElementById("cboStatus").value);
}

function cboNumOfMoChange() {
    $("#txtNumMonths").val(document.getElementById("cboNumOfMo").value);
}

function cboRouteOnChange() {
    $("#txtRoute").val(document.getElementById("cboRoute").value);
}