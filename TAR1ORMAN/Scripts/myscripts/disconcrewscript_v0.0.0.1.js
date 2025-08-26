function loadAllData() {
    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    $.ajax({
        type: 'GET',
        url: "/DisconCrew/GetAllDisconCrews",
        mimeType: 'json',
        success: function (data) {
            if (data != null) {
                $('#myTable1').DataTable({
                    "data": data.data,
                    "bLengthChange": false,
                    "pageLength": 10,
                    "columns": [
                        { "data": "Id", "autoWidth": true },
                        { "data": "Name", "autoWidth": true },
                        { "data": "Office", "autoWidth": true },
                        { "data": "IsActive", "width": 10 }
                    ],
                    "initComplete": function (settings, json) {

                        document.body.style.cursor = 'default';
                        $('#modalLoading').modal('hide');

                    },
                    "columnDefs": [{
                        "targets": 3,
                        "className": 'dt-body-center',
                        "render": function (data, type, full) {

                            if (data === true)
                                return '<input type="checkbox" checked style="pointer-events:none;" />';
                            else
                                return '<input type="checkbox" style="pointer-events:none;" />';
                        }
                    }]

                });
            } else {
                document.body.style.cursor = 'default';
                $('#modalLoading').modal('hide');
                alert("No data to be displayed.");
            }

        },
        error: function () {
            //document.body.style.cursor = 'progress';
            //$('#modalLoading').modal('show');
            //document.getElementById("spintext").innerHTML = "LOADING...";
        }
    });
}

function showModal() {
    $('#txtCrewName').val("");
    loadCboOffice();
    $('#myAddNewCrewModal').modal('show');
}

function loadCboOffice() {
    $.ajax({
        url: "/DisconCrew/GetAllOffices/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboOffice').empty();
            $('#cboOffice').append("<option value=0>SELECT OFFICE</option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Office;
                var opt = new Option(Desc, result[i].Id);
                $('#cboOffice').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function addNewDisconCrew() {
    var crewname = $('#txtCrewName').val();
    var officeid = $('#cboOffice').val();

    if (crewname != "" && officeid != 0) {
        var obj = {
            Id: 0,
            Name: crewname,
            OfficeId: officeid,
            Office: "",
            IsActive: true
        }

        $.ajax({
            data: JSON.stringify(obj),
            url: "/DisconCrew/InsertNewDisconCrew/",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                console.log(result.data);
                if (result.data.IsSucceed) {
                   
                    swal('\nADDED CREW', result.data.ResultMessage, 'success');
                    loadAllData();
                    var oTable = $('#myTable1').dataTable();
                    oTable.fnPageChange('last');
                } else {
                    swal('\nERROR', result.data.ResultMessage, 'error');
                }
                 
                $('#myAddNewCrewModal').modal('hide');
                
            },
            error: function (errormessage) {
                swal('Error', errormessage.responseText, 'warning');
                alert(errormessage.responseText);
            },
        });

    } else {
        swal("\nMISSING DETAILS", "ALL FIELDS ARE REQUIRED.", "warning");
    }
}