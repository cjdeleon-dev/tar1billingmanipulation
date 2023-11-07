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
            { "data": "Status", "autoWidth": true }
        ],
        "aoColumnDefs": [
            {
                "aTargets": [9],
                "mData": "Id",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                                'onclick="showModalByRefId(\'' + data + '|' + full.AccountNo + '|' + full.OldMemberId + '|' + full.OldMemberDate + '\')">' +
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
    console.log(sdata[2]);
    console.log(sdata[3]);

    if (sdata[2].trim() == '' || sdata[3] == '') {
        $('#myUpdateModal').modal('show');

        $('#txtRefID').val(sdata[0]);
        $('#txtAccountNo').val(sdata[1]);
        $('#txtMemberID').val(sdata[2].trim());

    } else {
        swal({
            title: "Ooopppssss!",
            text: "Cannot update Member ID and Member Date when they already have values.",
            type: "warning"
        });
        //swal('Oooppps!','Cannot update Member ID and Member Date when they already have values.','warning');
    }
    
}

function updateByRefID(id){

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