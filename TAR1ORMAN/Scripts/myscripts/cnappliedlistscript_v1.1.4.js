function loaddata() {
    $('#myTable1').DataTable({
        "ajax": {
            "url": "/ChangeNameAppliedList/loaddata",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Id", "autoWidth": true },
            { "data": "AppDate", "autoWidth": true },
            { "data": "AccountNo", "autoWidth": true },
            { "data": "OldName", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "OldMemberId", "autoWidth": true },
            { "data": "OldMemberDate", "autoWidth": true },
            { "data": "NewName", "autoWidth": true },
            { "data": "Status", "autoWidth": true },
            { "data": "Remark", "autoWidth": true }
        ],
        "aoColumnDefs": [
            {
                "aTargets": [10],
                "mData": "Id",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                        'onclick="showModalByRefId(\'' + data + '|' + full.AccountNo + '|' + full.OldMemberId + '|' + full.OldMemberDate + '|' + full.Remark + '|' + full.NewName + '\')">' +
                                '<i class="glyphicon glyphicon-edit"></i></button> ' +
                           '<button class="btn btn-danger" style="font-size:smaller;" href="#" id="del_' + data + '" onclick="removeByRefId(\'' + data + '\')"><i class="glyphicon glyphicon-remove"></i></button>';
                },
                "className": "text-center"
            }
        ]
    });
}

function showModalByRefId(ddata) {

    var sdata = ddata.split('|');

    $('#myUpdateModal').modal('show');

    $('#txtmodRefID').val(sdata[0]);
    $('#txtmodAccountNo').val(sdata[1]);
    $('#txtmodMemberID').val(sdata[2].trim());
    $('#dtpmodMemberDate').val(sdata[3]);
    $('#txtmodNewName').val(sdata[5].trim());

    if (sdata[4] == "")
        selectItem("--SELECT REMARK--");
    else
        selectItem(sdata[4]);
}

function updateByRefID(){

    //if (isValidUpdate==true) {
    if ($('#txtmodMemberID').val().trim() == "") {
        swal('\nInvalid', 'Please enter Member ID', 'warning');
        return false;
    }

    if ($('#dtpmodMemberDate').val() == "") {
        swal('\nInvalid', 'Please select Member Date', 'warning');
        return false;
    }

    var elt = document.getElementById("cbomodRemarks");
    if (elt.options[elt.selectedIndex].text == "--SELECT REMARK--") {
        swal('\nInvalid', 'Please select remark', 'warning');
        return false;
    }

    var dataObj = {
        RefId: $('#txtmodRefID').val(),
        NewName: $('#txtmodNewName').val(),
        OldMemberId: $('#txtmodMemberID').val(),
        OldMemDate: $('#dtpmodMemberDate').val(),
        Remark: elt.options[elt.selectedIndex].text,
        UpdatedBy: ""
    }

    $.ajax({
        data: JSON.stringify(dataObj),
        url: "/ChangeNameAppliedList/UpdateCNMember/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.message == "Success") {
                swal({
                    title: "\nSuccess",
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

function selectItem(itemToSelect) {
    var dd = document.getElementById('cbomodRemarks');
    for (var i = 0; i < dd.options.length; i++) {
        if (dd.options[i].text === itemToSelect) {
            dd.selectedIndex = i;
            break;
        }
    }
}

function removeByRefId(id) {
    swal({
        title: "Are you sure to delete?",
        text: "You will not be able to recover the selected record!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, remove it!",
        closeOnConfirm: false
    }, function (isConfirm) {
            swal("\nDeleted!", "The selected record has been deleted.", "success");
    });
}
