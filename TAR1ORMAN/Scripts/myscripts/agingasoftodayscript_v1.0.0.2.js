function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadPage() {
    //hide unneccessary options
    hideunneccessaryoptions();
    //load types
    loadtypes();
    //load towns
    loadtowns();
    //get current date
    getcurrentdate();
}

function loadtypes() {
    $.ajax({
        url: "/agingasoftoday/GetAllConsumerTypes/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboType').empty();
            $('#cboType').val(0);
            $('#cboType').append("<option value=0>Select Type</option>");
            $("#txtType").val(0);
            for (var i = 0; i < result.data.length; i++) {
                var Desc = result.data[i].ConsumerType;
                var opt = new Option(Desc, result.data[i].Id);
                $('#cboType').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function loadtowns() {
    $.ajax({
        url: "/agingasoftoday/GetAllTowns/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboTown').empty();
            $('#cboTown').val(0);
            $('#cboTown').append("<option value=0>Select Town</option>");
            $("#txtType").val(0);
            for (var i = 0; i < result.data.length; i++) {
                var Desc = result.data[i].Town;
                var opt = new Option(Desc, result.data[i].Id);
                $('#cboTown').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function getcurrentdate() {
    $.ajax({
        url: "/agingasoftoday/GetCurrentDate/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("txtCurrDate").value = result;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function cboTypeOnChange() {
    $('#txtType').val($('#cboType').val());
}

function cboTownOnChange() {
    $('#txtTown').val($('#cboTown').val())
}

function emptyTable() {
    var table = $('#myTable1').DataTable();

    //clear datatable
    table.clear().draw();

    //destroy datatable
    table.destroy();
}

function processdata() {
    var typeid = $('#txtType').val();
    var townid = $('#txtTown').val();
    var currdate = $('#txtCurrDate').val();

    if (typeid == "" || typeid == "0") {
        alert("There are no selected type.");
        return false;
    }
    
    if (townid == "" || townid == "0") {
        alert("There are no selected town.");
        return false;
    }

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    emptyTable();

    var strparam = townid + "~" + typeid;

    $.ajax({
        url: "/agingasoftoday/CheckIfHasData?townid=" + townid + "&typeid=" + typeid,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (response) { //check first if it has atleast one record.

            var fileName = "AgingAsOfToday_" + currdate + "-" + townid + "_" + typeid;

            if (response) {

                $('#myTable1').DataTable({
                    ajax: {
                        "url": "/AgingAsOfToday/GetData?townidtypeid=" + strparam,
                        "type": "GET",
                        "contentType": "application/json;charset=UTF-8",
                        "datatype": "json"
                    },
                    pageLength: 25,
                    dom: "Bfrtip",
                    buttons: [{
                        extend: "excel",
                        filename: fileName,
                        text: "EXPORT TO EXCEL",

                    }],
                    initComplete: function (settings, json) {
                        document.body.style.cursor = 'default';
                        $('#modalLoading').modal('hide');
                    },
                    columns: [
                        { "data": "TownId", "autoWidth": true },
                        { "data": "AccountNo", "autoWidth": true },
                        { "data": "Name", "autoWidth": true },
                        { "data": "Address", "autoWidth": true },
                        { "data": "Status", "autoWidth": true },
                        { "data": "Days30", "autoWidth": true },
                        { "data": "Days60", "autoWidth": true },
                        { "data": "Days90", "autoWidth": true },
                        { "data": "Days120", "autoWidth": true },
                        { "data": "Above120", "autoWidth": true }
                    ]
                });
                
            } else {
                document.body.style.cursor = 'default';
                $('#modalLoading').modal('hide');
                alert('There are no data to load.');
            }
            
        },
        error: function (errormessage) {
            document.body.style.cursor = 'default';
            $('#modalLoading').modal('hide');
            alert(errormessage.responseText);
        }
    });

}

