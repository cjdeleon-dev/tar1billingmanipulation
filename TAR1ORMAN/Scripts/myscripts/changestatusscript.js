

function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadPage() {
    hideunneccessaryoptions();
    $('#optradio_acttodtd').prop('checked', true);
    loadActive();
}

function loadDataTableActive() {

    $('#myTable1').DataTable({
        "ajax": {
            "url": "/ChangeStatus/loadfordata?status=" + 1,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "AccountName", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "MeterNo", "autoWidth": true },
            { "data": "PoleId", "autoWidth": true },
            { "data": "Status", "autoWidth": true }

        ],
        "aoColumnDefs": [
            {
                "aTargets": [6],
                "mData": "AccountNo",
                "mRender": function (data, type, full) {

                    var acctno = data.toString();
                    var acctname = full['AccountName'].toString();

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw' + acctno + '" onclick="viewStatusByAcctno(\'' + acctno + '\',\'' + acctname + '\')"><i class="glyphicon glyphicon-eye-open"></i> View</button> | ' +
                        '<button class="btn btn-warning" style="font-size:smaller;" href="#" id="ch' + acctno + '" onclick="showChangeStatusModal(\'' + acctno + '\',\'' + 0 + '\',\'' + acctname + '\')"><i class="glyphicon glyphicon-remove-circle"></i> Change Status</button>';
                },
                "className": "text-center"
            }
        ]
    });
}

function loadDataTableDiscon() {

    $('#myTable1').DataTable({
        "ajax": {
            "url": "/ChangeStatus/loadfordata?status=" + 0,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "AccountName", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "MeterNo", "autoWidth": true },
            { "data": "PoleId", "autoWidth": true },
            { "data": "Status", "autoWidth": true }

        ],
        "aoColumnDefs": [
            {
                "aTargets": [6],
                "mData": "AccountNo",
                "mRender": function (data, type, full) {
                    var acctno = data.toString();
                    var acctname = full['AccountName'].toString();

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw' + acctno + '" onclick="viewStatusByAcctno(\'' + acctno + '\',\'' + acctname + '\')"><i class="glyphicon glyphicon-eye-open"></i> View</button> | ' +
                        '<button class="btn btn-warning" style="font-size:smaller;" href="#" id="ch' + acctno + '" onclick="showChangeStatusModal(\'' + acctno + '\',\'' + 1 + '\',\'' + acctname + '\')"><i class="glyphicon glyphicon-ok-circle"></i> Change Status</button>';
                },
                "className": "text-center"
            }
        ]
    });
}

function setStatusByAcctno() {

    if (isValidEntry()==true) {
        var acctnum = $('#txtAcctNo').val();
        var dateentry = "";
        var statfrom = $('#txtStatusFrom').val();
        var changefrom = "";
        var statto = $('#txtStatusTo').val();
        var changeto = "";
        var reason = $('#txtReason').val();
        var entryuser = "";
        var actiondate = $('#dtpActDate').val();
        var dtdread = $('#dtdRead').val();

        var dataObj = {
            AccountNo: acctnum,
            DateEntry: dateentry,
            StatusFrom: statfrom,
            ChangeStatusFrom: changefrom,
            StatusTo: statto,
            ChangeStatusTo: changeto,
            Reason: reason,
            EntryUser: entryuser,
            ActionDate: actiondate,
            DTDRead: dtdread
        }

        $.ajax({
            data: JSON.stringify(dataObj),
            url: "/ChangeStatus/SetStatusOfConsumerById/",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                swal("Status Changed!", "Successfully Changed.", "success");
                selbtn = $('#ch' + acctnum);
                selbtn.prop('disabled', true);
                clearChangeStatusForm();
                $('#myChangeStatusModal').modal('hide');
            },
            error: function (errormessage) {
                swal("Error", errormessage.responseText, "warning");
                alert(errormessage.responseText);
            },
        });
    }
    else {
        swal("Invalid Entry", "Please fill all required fields.", "warning");
    }
}

function isValidEntry() {

    var returnVal = true;

    if ($('#txtStatusFrom').val() == "")
        return false;
    if ($('#txtStatusTo').val() == "" || $('#txtStatusTo').val() == "Q")
        return false;
    if ($('#txtReason').val() == "")
        return false;
    if ($('#dtpActDate').val() == "")
        return false;

    return returnVal;
}

function loadActive() {
    var table = $('#myTable1').DataTable();
    table.clear().draw();
    table.destroy();
    loadcboChangeTo('A');
    loadDataTableActive();
}

function loadDiscon() {
    var table = $('#myTable1').DataTable();
    table.clear().draw();
    table.destroy();
    loadcboChangeTo('D');
    loadDataTableDiscon();
}

function viewStatusByAcctno(acctnum, acctname ) {

    $('#modTxtAcctNo').val(acctnum);
    $('#modTxtAcctName').val(acctname);

    var table = $('#statusTable').DataTable();
    table.clear().draw();
    table.destroy();

    $('#statusTable').DataTable({
        "sort": false,
        "searching": false,
        "paging": false,
        "ajax": {
            "url": "/ChangeStatus/LoadStatusLogData?accountno=" + acctnum,
            "type": "GET",
            "datatype": "json",
            "dataSrc": function (obj) {

                if (obj.data == null) {
                    return "";
                } else {
                    return obj.data;
                }
                
            }
        },
        "columns": [
            { "data": "DateEntry", "autoWidth": true },
            { "data": "ChangeStatusFrom", "autoWidth": true },
            { "data": "ChangeStatusTo", "autoWidth": true },
            { "data": "Reason", "autoWidth": true },
            { "data": "EntryUser", "autoWidth": true },
            { "data": "ActionDate", "autoWidth": true },
            { "data": "DTDRead", "autoWidth": true }
        ]
    });

    $('#myViewModal').modal('show');
}

function showChangeStatusModal(acctnum, changestatto, acctname) {
    $('#txtAcctNo').val(acctnum);
    $('#txtAcctName').val(acctname);
    if (changestatto == 0) { //active to disconnect
        $('#txtChangeFr').val("Active");
        $('#txtStatusFrom').val("A");
    }
    else {
        $('#txtChangeFr').val("Disconnected");
        $('#txtStatusFrom').val("D");
    }

    $('#myChangeStatusModal').modal('show');
}


function loadcboChangeTo(status) {
    $.ajax({
        url: "/ChangeStatus/GetAllStatus?exceptstat=" + status,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboChangeTo').empty();
            //$('#cboChangeTo').val(0);
            $('#cboChangeTo').append("<option value='Q'>Select Status</option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Description;
                var opt = new Option(Desc, result[i].StatusId);
                $('#cboChangeTo').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function clearChangeStatusForm() {
    $('#txtChangeFr').val("");
    $('#cboChangeTO').val("Q");
    $('#dtpActDate').val("");
    $('#txtReason').val("");
    $('#txtDtdRead').val("");
}

function cboChangeToOnChange() {
    var setVal = $('#cboChangeTo').val();
    $('#txtStatusTo').val(setVal);
}