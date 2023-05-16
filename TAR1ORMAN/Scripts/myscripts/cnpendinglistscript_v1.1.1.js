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
            "url": "/PendingChangeNameList/loadfordata",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "RefID", "autoWidth": true },
            { "data": "AccountNumber", "autoWidth": true },
            { "data": "Name", "autoWidth": true },
            { "data": "AppDate", "autoWidth": true },
            { "data": "Reason", "autoWidth": true },
            { "data": "Remarks", "autoWidth": true }
        ],
        "aoColumnDefs": [
            {
                "aTargets": [6],
                "mData": "RefID",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" onclick="showModalByRefId(\'' + data + '\')"><i class="glyphicon glyphicon-edit"></i> Modify</button>';
                },
                "className": "text-center"
            }
        ]
    });
}

function showModalByRefId(id) {
    console.log('selected id: ' + id);
    $('#txtRefID').val(id);

    $('#myUpdateModal').modal('show');
}

function updateByRefID() {

    let selId = $('#txtRefID').val();
    let ornum = $('#txtNewMemberID').val();
    let ordtp = $('#dtpNewMemberDate').val();

    if (ornum == "" && ordtp == "") {
        swal('Invalid Entry', 'Please fill all required fields.', 'warning');
        return false;
    } else {
        var dataObj = {
            RefId: selId,
            MemberOR: ornum,
            ORDate: ordtp,
            UpdatedBy: ""
        }

        $.ajax({
            data: JSON.stringify(dataObj),
            url: "/PendingChangeNameList/UpdateNewORMember/",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.message == "Success") {
                    swal({
                        title: "Success",
                        text: "Successfully Updated.",
                        type: "success"
                    },
                        function () {
                            location.reload();
                        }
                    );

                } else {
                    swal('Error', result.message, 'error');
                }
            },
            error: function (errormessage) {
                swal('Error', errormessage.responseText, 'warning');
                //alert(errormessage.responseText);
            },
        });
    }


    function closeModalAndRefresh() {
        $('#myUpdateModal').modal('hide');
        window.location = "/PendingChangeNameList/Index";
    }


}