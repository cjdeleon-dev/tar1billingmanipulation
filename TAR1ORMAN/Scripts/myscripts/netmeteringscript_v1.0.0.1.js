function loaddata() {

    //var fileName = "S_RECAP_CONSUMER_CLASS_BASED_" + billp;

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
         //dom: 'Bfrtip',
         //buttons: [{
         //    extend: 'excel',
         //    filename: fileName,
         //    text: 'EXPORT TO EXCEL'
         //}],
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