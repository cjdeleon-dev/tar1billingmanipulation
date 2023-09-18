function hideunneccessaryoptions() {
	if ($('#txtCurrentRole').val() == "NONADMIN") {
		$('#mnudbsettings').hide();
	} else {
		$('#mnudbsettings').show();
	}

}

function loadList() {
	hideunneccessaryoptions();
	$('#myTable1').DataTable({
		"ajax": {
			"url": "/ApprovalChangeNameList/loadfordata",
			"type": "GET",
			"datatype": "json"
		},
		"columns": [
            { "data": "RefID", "autoWidth": true },
            { "data": "AccountNumber", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "AppDate", "autoWidth": true }
		],
		"aoColumnDefs": [
            {
            	"aTargets": [4],
            	"mData": "RefID",
            	"mRender": function (data, type, full) {

            		return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" onclick="approvedApplication(\'' + data + '\')"><i class="glyphicon glyphicon-check"></i> Approved</button>';
            	},
            	"className": "text-center"
            }
		]
	});
}

function approvedApplication(id){
	$.ajax({
		url: "/ApprovalChangeNameList/ApprovedApplicant?refid="+ id,
		type: "POST",
		contentType: "application/json;charset=UTF-8",
		dataType: "json",
		success: function (result) {
			//window.location = "/ApprovalChangeNameList/Index";
			if (result) {
				//swal('Success', 'Updated Consumers\' Master File.', 'success');
				swal({
					title: "Success!",
					text: "Updated Consumers\' Master File.",
					type: "success"
				}, function(){ 
					location.reload();
				});
			} else {
				swal('Failed', 'Fail to approve this account.', 'warning');
			}
		},
		error: function (errormessage) {
			alert(errormessage.responseText);
		}
	});
}
