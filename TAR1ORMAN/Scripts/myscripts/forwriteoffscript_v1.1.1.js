function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadDataTable1() {

    var yearOf = $("#txtYear").val();
    var areaOf = $('#txtArea').val();

    $('#myTable1').dataTable().fnDestroy();

    $('#myTable1').DataTable({
        "ajax": {
            "bDestroy": true,
            "url": "/ForWriteOff/loadfordata?yr=" + parseInt(yearOf) +"&area=" + areaOf,
            "type": "GET",
            "datatype": "json"
        }, 
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "AccountName", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "StatusId", "autoWidth": true },
            { "data": "TypeId", "autoWidth": true },
            { "data": "TrxBalance", "autoWidth": true },
            { "data": "VATBalance", "autoWidth": true },
            { "data": "TotalBalance", "autoWidth": true }
        ]
    });


}

function loadPage() {
    hideunneccessaryoptions();
    loadcboYear();
    loadcboArea();
}

function processdata() {
    loadDataTable1();
}


function loadcboYear() {
    $.ajax({
        url: "/ForWriteOff/GetAllYearOf/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboYear').empty();
            $('#cboYear').val(0);
            $('#cboYear').append("<option value=0>Select Year</option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].YearOf;
                var opt = new Option(Desc, result[i].YearOf);
                $('#cboYear').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function loadcboArea() {
    $.ajax({
        url: "/ForWriteOff/GetAllAreaOf/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboArea').empty();
            $('#cboArea').val(0);
            $('#cboArea').append("<option value=0>Select Area</option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Area;
                var opt = new Option(Desc, result[i].Area);
                $('#cboArea').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function cboYearOnChange() {
    $("#txtYear").val(document.getElementById("cboYear").value);
}

function cboAreaOnChange() {
    $("#txtArea").val(document.getElementById("cboArea").value);

}