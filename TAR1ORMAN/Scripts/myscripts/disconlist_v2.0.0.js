function loadcboRoutesbyOfficeId(id) {
    $.ajax({
    	url: "/DisconListV2/GetAllRoutesByOfficeId?id=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboRoute').empty();
            $('#cboRoute').val(0);
            $('#cboRoute').append("<option value=0>Select Route</option>");

            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Id + ' - ' + result[i].Route;
                var opt = new Option(Desc, result[i].Id);
                $('#cboRoute').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function loadcboOffices() {
    getCurrentBP();
    hideMainButtons();
    $.ajax({
        url: "/DisconListV2/GetAllOffices/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboOffice').empty();
            $('#cboOffice').val(0);
            $('#cboOffice').append("<option value=0>Select Office</option>");

            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].DisplayText;
                var opt = new Option(Desc, result[i].Id);
                $('#cboOffice').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function resetCbos() {
    $('#cboRoute').empty();
    $('#cboOffice').val(0);
}

function cboOfficeOnChange() {

    if ($('#cboNumOfMo').val() == "0") {
        swal("Invalid", "No selection for parameter 'Number of Months'", "warning");
        $('#cboOffice').val(0);
        return false;
    }

    var ofcid = $('#cboOffice').val();
    loadcboRoutesbyOfficeId(ofcid);
}

function showAddCrewModal() {
    let ofcid = $('#cboOffice').val();
    if (ofcid != 0) {
        $.ajax({
            url: "/DisconListV2/GetAllCrewsByOfficeId?id=" + ofcid,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                $('#chkSelUnselall').prop('checked', false);
                $('#tbodyCrew').empty;
                var html = '';
                $.each(result, function (key, item) {
                    html += '<tr class="" id="' + item.Id + '">';
                    html += '<td id="' + item.Id + '" class="text-center"><input type="checkbox" class="select-item checkbox" name="select-item" value="' + item.Id + '" /></td>';
                    html += '<td>' + item.Id + '</td>';
                    html += '<td>' + item.Name + '</td>';
                    html += '</tr>';
                });
                $('#tbodyCrew').html(html);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });

        $('#modalAddCrew').modal('show');

    } else {
        swal("Invalid", "Please select office.", "warning");
        return false;
    }
        
}

function selUnselAllCrews() {

    var isselected = $('#chkSelUnselall').is(":checked");

    $targetCheckboxes = $('#chkSelUnselall').closest('table').find('[name=select-item]');

    $targetCheckboxes.each(function () {

        // Set checkbox check/uncheck 
        // according to 'select all' status
        this.checked = isselected;
    });
}

function getAllSelectedCrews() {
    var html = '';
    //var arrayData = new Array();
    var isselected = $('#chkSelUnselall').is(":checked");
    var rowSelCnt = $("#tblCrews input[type=checkbox]:checked").length;

    if (isselected)
        rowSelCnt -= 1;

    $("#tblCrews input[type=checkbox]:checked").each(function () {
        var row = $(this).closest("tr")[0];
        if (row.cells[1].innerHTML != "Id") {
            html += '<tr>';
            html += '<td>' + row.cells[1].innerHTML + '</td>';
            html += '<td>' + row.cells[2].innerHTML + '</td>';
            html += '</tr>';
        }
    });

    $('#tbodyCrewList').html(html);

    if (rowSelCnt > 0) {
        $('#btnGenList').show();
        $('#btnResetClear').show();

        $('#cboNumOfMo').prop('disabled', true);
        $('#cboStatus').prop('disabled', true);
        $('#cboOffice').prop('disabled', true);
        $('#cboRoute').prop('disabled', true);

    } else {
        swal("Invalid", "There are no selected crew(s).", "warning");

        $('#cboNumOfMo').prop('disabled', false);
        $('#cboStatus').prop('disabled', false);
        $('#cboOffice').prop('disabled', false);
        $('#cboRoute').prop('disabled', false);

        return false;
    }
}

function getCurrentBP() {
    $.ajax({
        url: "/DisconListV2/GetCurrentBillPeriod",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("lblBillPeriod").innerHTML = 'Billing Period: ' + result.Id + ' - ' + result.BillPeriod;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


function hideMainButtons() {
    $('#btnGenList').hide();
    $('#btnSaveList').hide();
    $('#btnResetClear').hide();
}

function generateDisconList() {

    var routeid, /*bperiod, offid,*/ numofmo, dstatid;
    routeid = $('#cboRoute').val();
    //bperiod = document.getElementById('lblBillPeriod').innerText;
    //offid = $('#cboOffice').val();
    numofmo = $('#cboNumOfMo').val();
    dstatid = $('#cboStatus').val();

    //var param = routeid + '_' + bperiod + '_' + offid + '_' + numofmo + '_' + dstatid;
    var param = routeid + '_' + numofmo + '_' + dstatid;


    $('#tblDisconList').DataTable().destroy();

    var table = $('#tblDisconList').DataTable({
        "ajax": {
            "url": "/DisconListV2/GenerateDisconList?ridnomostatid=" + param,
            "type": "GET",
            "datatype": "json",
        },
        "initComplete": function (settings) {
            if (settings["aoData"].length > 0) {
                //when row count is more than 0, show save and export button
                $('#btnSaveList').show();
            }
        },
        "columns": [
            { "data": "AccountNo", "autoWidth": true },
            { "data": "AccountName", "autoWidth": true },
            { "data": "Address", "autoWidth": true },
            { "data": "MeterNo", "autoWidth": true },
            { "data": "FirstBill", "autoWidth": true },
            { "data": "LastBill", "autoWidth": true },
            { "data": "NoOfMonths", "autoWidth": true },
            { "data": "Due", "autoWidth": true },
            { "data": "Remark", "autoWidth": true }

        ]
    });
}

function saveExportDisconList() {
    saveDisconListHeader();
}

function saveDisconListHeader() {
    var routeid, bperiod, offid, numofmo, dstatid;
    routeid = $('#cboRoute').val();
    bperiod = document.getElementById('lblBillPeriod').innerText.split(':')[1].substring(0, 7).trim();
    offid = $('#cboOffice').val();
    numofmo = $('#cboNumOfMo').val();
    dstatid = $('#cboStatus').val();

    var param = routeid + '_' + bperiod + '_' + offid + '_' + numofmo + '_' + dstatid;

    $.ajax({
        url: "/DisconListV2/SaveDisconListHeader?ridbpoidnomostatid=" + param,
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            saveDisconListCrews();
        },
        error: function (errormessage) {
            swal("Error","Unable to save list.","warning");
        }
    });
}

function saveDisconListCrews() {
    var arrayData = new Array();

    //kunin number of rows in table
    var cnt = $('#tblAddedCrews tbody tr').length;
    //kunin lahat ng rows
    var tblrows = $('#tblAddedCrews tbody tr');

    for (var i = 0; i < cnt; i++) {

        var $tds = tblrows[i].cells;

        var crewlistdata = {
            Id: $tds[0].innerText,
            Name: $tds[1].innerText
        };

        arrayData.push(crewlistdata);
    }

    $.ajax({
        url: "/DisconListV2/SaveDisconListCrews/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(arrayData),
        dataType: "json",
        success: function (result) {
            if (result.savemsg != "Successfully Saved.")
                swal("Error", result.savemsg, "warning");
            else {

                $('#frmExport').submit();
                $('#btnGenList').hide();
                $('#btnSaveList').hide();
            }
                
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


function resetPage() {
    window.location = "/DisconListV2/Index";
}