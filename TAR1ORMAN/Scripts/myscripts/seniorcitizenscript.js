function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadDataTable1() {
    $('#myTable1').DataTable({
        "ajax": {
            "url": "/SeniorCitizen/loadfordata",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "AppliedDate", "autoWidth": true },
            { "data": "ExpiryDate", "autoWidth": true }
        ],
        aoColumnDefs: [
            {
                "aTargets": [5],
                "mData": "AccountNo",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                        'onclick="showSCDetailsByAcctNo(\'' + data + '\')">' +
                        '<i class="glyphicon glyphicon-edit"></i> VIEW ID NO.</button> ';
                },
                "className": "text-center"
            }
        ]
    });
}

function loadProcess() {
    hideunneccessaryoptions();
    loadDataTable1();
}

function showSCDetailsByAcctNo(acctno) {
    $('#mySCDetailModal').modal("show");
    $('#txtAcctNo').val(acctno);
    getSetAcctSCDetailsByAcctNo();
}

function getSetAcctSCDetailsByAcctNo() {
    $.ajax({
        url: "/SeniorCitizen/GetAccountDetails?accountNum=" + $('#txtAcctNo').val(),
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var cons = result;
            if (cons != null) {
                
                $('#txtName').val(cons.Name);
                $('#txtSCID').val(cons.SCID);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateSCDetailsOfAccount() {
    var acctno = $('#txtAcctNo').val();
    var scidno = $('#txtSCID').val();

    $.ajax({
        url: "/SeniorCitizen/UpdateSCIDOfAccountNo?accountNum=" + acctno + "&scidno=" + scidno,
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result == "Success")
                swal("\nUpdated", "Successfully updated.", "success");
            else
                swal("\nFailed", "Failed to update data.", "warning");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}