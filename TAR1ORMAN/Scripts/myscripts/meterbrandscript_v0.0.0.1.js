function loadAllData() {

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    var fileName = "MeterBrands";

    $.ajax({
        type: 'GET',
        url: "/MeterBrand/GetAllMeterBrands",
        mimeType: 'json',
        success: function (data) {
            if (data != null) {
                $('#myTable1').DataTable({
                    "data": data.data,
                    "bLengthChange": false,
                    "pageLength": 10,
                    "dom": "Bfrtip",
                    "buttons": [
                        {
                            extend: "excel",
                            filename: fileName,
                            text: "EXPORT TO EXCEL",
                            exportOptions: {
                                columns: [0, 1, 2],
                                orthogonal: 'export'
                            }
                        }
                    ],
                    "columns": [
                        { "data": "Id", "autoWidth": true },
                        { "data": "MeterBrand", "autoWidth": true },
                        { "data": "IsActive", "width":10 },
                        { "data": "Id" },
                    ],
                    "initComplete": function (settings, json) {

                        document.body.style.cursor = 'default';
                        $('#modalLoading').modal('hide');

                    },
                    "columnDefs": [{
                        "targets": 2,
                        "className": 'dt-body-center',
                        "render": function (data, type, full) {
                            //if (data===true)
                            //    return '<input type="checkbox" checked style="pointer-events:none;" />';
                            //else
                            //    return '<input type="checkbox" style="pointer-events:none;" />';

                            if (type === 'export') {
                                var rslt = (data === true ? 'Yes' : 'No');
                                return rslt;
                            } else {
                                if (data===true)
                                    return '<input type="checkbox" checked style="pointer-events:none;" />';
                                else
                                    return '<input type="checkbox" style="pointer-events:none;" />';
                            }
                        }
                    }, {
                        "targets": 3,
                        "className": 'dt-body-center',
                        "render": function (data, type, full) {
                            return '<button class="btn btn-primary" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                                'onclick="showListTypes(' + full.Id + ',\'' + full.MeterBrand  + '\',' + full.IsActive + ')">SHOW TYPES</button>'
                                
                        }
                    }],
                    
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

function showListTypes(bid, brand, isactive) {
    //empty table first
    var table = $('#myTypesTable').DataTable();

    //clear datatable
    table.clear().draw();

    //destroy datatable
    table.destroy();

    $('#myTListModal').modal('show');

    $('#txtId').val(bid);
    $('#txtMeterBrand').val(brand);

    if (isactive)
        $('#chkIsActive').prop('checked', 'checked');

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    $.ajax({
        type: 'GET',
        url: "/MeterBrand/GetAllMeterTypesByBrandId?brandid="+ bid,
        mimeType: 'json',
        success: function (data) {
            if (data != null) {
                $('#myTypesTable').DataTable({
                    "data": data.data,
                    "bLengthChange": false,
                    "bFilter": false,
                    "pageLength": 5,
                    "columns": [
                        { "data": "Id", "autoWidth": true },
                        { "data": "MeterType", "autoWidth": true },
                        { "data": "IsActive", "autoWidth": true }
                    ],
                    "initComplete": function (settings, json) {

                        document.body.style.cursor = 'default';
                        $('#modalLoading').modal('hide');

                    },
                    "columnDefs": [{
                        "targets": 2,
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
            document.body.style.cursor = 'progress';
            $('#modalLoading').modal('show');
            document.getElementById("spintext").innerHTML = "LOADING...";
        }
    });
}

