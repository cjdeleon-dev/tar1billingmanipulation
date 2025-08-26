function loaddata() {

    var fileName = "NETMETERING_LIST";

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

     $('#myTable1').DataTable({
        ajax: {
            "url": "/NetMetering/LoadData",
            "type": "GET",
            "datatype": "json"
         },
         pageLength: 25,
         dom: 'Bfrtip',
         buttons: [{
             extend: 'excel',
             filename: fileName,
             text: 'EXPORT TO EXCEL'
         }],
         initComplete: function (settings, json) {
             document.body.style.cursor = 'default';
             $('#modalLoading').modal('hide');
         },
         columns: [
             { "data": "AccountNo", "autoWidth": true },
             { "data": "Name", "autoWidth": true },
             { "data": "Address", "autoWidth": true },
             { "data": "Type", "autoWidth": true },
             { "data": "Status", "autoWidth": true },
             { "data": "MeterNo", "autoWidth": true }
         ],
		 aoColumnDefs: [
			 {
			 	"aTargets": [6],
			 	"mData": "AccountNo",
			 	"mRender": function (data, type, full) {

			 		return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                        'onclick="showLedgerByAcctNo(\'' + data + '\')">' +
                                '<i class="glyphicon glyphicon-edit"></i> VIEW LEDGER</button> ';
			 	},
			 	"className": "text-center"
			 }
		 ]
    }); 
}

function showLedgerByAcctNo(actno) {
    window.location = "/NetMetering/SetAccountLedger?accountNo=" + actno;
}

function showAddModal() {
    $('#txtAcctNo').val('');
    $('#txtName').val('');
    $('#txtAddress').val('');
    $('#txtPoleId').val('');
    $('#txtMeterNo').val('');

    $('#btnAddNow').attr('disabled', 'disabled');

    $('#addNetMeteringModal').modal('show');
}

function displayDetails() {

    var acctno = $('#txtAcctNo').val();

    if (acctno.trim() == "") {
        alert('Please specify ACCOUNT NO to be searched.');
        return false;
    }

    $.ajax({
        url: "/NetMetering/GetAccountDetails?accountNo=" + acctno,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result.data != null) {
                $('#txtName').val(result.data.AccountName);
                $('#txtAddress').val(result.data.Address);
                $('#txtPoleId').val(result.data.PoleId);
                $('#txtMeterNo').val(result.data.MeterNo);

                $('#btnAddNow').removeAttr('disabled');
            } else {
                alert('Account Number is not exist.');
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function addAccountAsNetMetering() {

    var objData = {
        Id: 0,
        AccountNo: $('#txtAcctNo').val(),
        AccountName: null,
        Address: null,
        MeterNo: null,
        SeqNo: null,
        PoleId: null,
        Status: null,
        MemberId: null,
        ORDate: null,
    }

    $.ajax({
        url: "/NetMetering/AddAccountAsNetMetering/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: JSON.stringify(objData),
        success: function (result) {
            if (result) {
                alert('The account has been added to Net Metering List.');
                $('#addNetMeteringModal').modal('hide');
                window.location = "/NetMetering/NetMeteringList";
            }
            else
                alert('An error occured. Cannot be added.');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}