function loadPage() {
    getCurrentBP();
    loadcboOffices();
    loadcboFinanceHeads();
    $('#cboRoute').attr('disabled', 'disabled');
    $('#btnSavePrev').attr('disabled', 'disabled');
}

function getCurrentBP() {
    $.ajax({
        url: "/SumofDisconReport/GetCurrentBillPeriod",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            document.getElementById("lblBillPeriod").innerHTML = 'Period:\n' + result.BillPeriod;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadcboOffices() {
    $.ajax({
        url: "/SumofDisconReport/GetAllOffices/",
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

function loadcboFinanceHeads() {
    $.ajax({
        url: "/SumofDisconReport/GetAllFinanceHead/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboCheckedBy').empty();
            $('#cboCheckedBy').val(0);
            $('#cboCheckedBy').append("<option value=0>Select Finance Head</option>");

            for (var i = 0; i < result.length; i++) {
                var FinHead = result[i].Name;
                var opt = new Option(FinHead, result[i].UserId);
                $('#cboCheckedBy').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function cboOfficeOnChange() {

    var ofcid = $('#cboOffice').val();
    if (ofcid != 0) {
        $('#cboRoute').removeAttr('disabled');
        loadcboRoutesbyOfficeId(ofcid);
    }
    else {
        $('#cboRoute').removeAttr('disabled');
        $('#cboRoute').attr('disbaled', 'disabled');
    }
        
        
}

function loadcboRoutesbyOfficeId(id) {
    $.ajax({
        url: "/SumofDisconReport/GetAllRoutesByOfficeId?id=" + id,
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

function resetPage() {
    window.location = "/SumOfDisconReport/SumDisconRpt";
}

function showAddCrewModal() {
    let ofcid = $('#cboOffice').val();
    if (ofcid != 0) {
        $.ajax({
            url: "/SumofDisconReport/GetAllCrewsByOfficeId?id=" + ofcid,
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

function getAllSelectedCrews(){
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
            html += '<td class="text-center"><button class="btn btn-danger" onclick="deleteCrewRow(this)"><i class="glyphicon glyphicon-trash"></i></button></td>';
            html += '</tr>';
        }
    });


    $('#tbodyCrewList').html(html);

    if (rowSelCnt > 0) {
        $('#cboOffice').prop('disabled', true);
        $('#cboRoute').prop('disabled', true);

    } else {
        swal("Invalid", "There are no selected crew(s).", "warning");

        $('#cboOffice').prop('disabled', false);
        $('#cboRoute').prop('disabled', false);

        return false;
    }
}

function deleteCrewRow(r) {
    var i = r.parentNode.parentNode.rowIndex;
    document.getElementById('tblAddedCrews').deleteRow(i);
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

function showAddDTDConsumersModal() {
    let routeid = $('#cboRoute').val();
    let dtddate = $('#dtpDisconDate').val();
    if (routeid != 0 && dtddate != "") {
        let dtddaterouteid = dtddate + '_' + routeid;
        $.ajax({
            url: "/SumofDisconReport/GetAllDTDConsumerByRouteId?dtddaterouteid=" + dtddaterouteid,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                $('#chkSelUnselDTDall').prop('checked', false);
                $('#tbodyDTD').empty;
                if (result.length > 0) {
                    var html = '';
                    $.each(result, function (key, item) {
                        html += '<tr class="" id="' + item.AccountNo + '">';
                        html += '<td id="' + item.AccountNo + '" class="text-center"><input type="checkbox" class="select-item checkbox" name="select-item" value="' + item.AccountNo + '" /></td>';
                        html += '<td>' + item.AccountNo + '</td>';
                        html += '<td>' + item.AccountName + '</td>';
                        html += '<td>' + item.AccountType + '</td>';
                        html += '<td>' + item.Reason + '</td>';
                        html += '<td>' + item.LastReading + '</td>';
                        html += '<td>' + item.EncoderId + '</td>';
                        html += '<td>' + item.Encoder + '</td>';
                        html += '<td>' + item.ActDate + '</td>';
                        html += '</tr>';
                    });
                    $('#tbodyDTD').html(html);
                } else {
                    swal("No Records", "No Data Records to Display.", "warning")
                }
                
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });

        $('#modalAddDTDCons').modal('show');

    } else {
        swal("Invalid", "Please fill all required parameters", "warning");
        return false;
    }
}

function getAllSelectedDTDCons() {
    var html = '';
    //var arrayData = new Array();
    var isselected = $('#chkSelUnselDTDall').is(":checked");
    var rowSelCnt = $("#tblDTDs input[type=checkbox]:checked").length;

    if (isselected)
        rowSelCnt -= 1;

    //Account Number
    //Account Name
    //Type
    //Reason
    //Last Reading
    //Encoder
    //Actual Date

    $("#tblDTDs input[type=checkbox]:checked").each(function () {
        var row = $(this).closest("tr")[0];
        if (row.cells[1].innerHTML != "Account No")
        //Header is not included.
        {
            html += '<tr>';
            html += '<td>' + row.cells[1].innerHTML + '</td>';
            html += '<td>' + row.cells[2].innerHTML + '</td>';
            html += '<td>' + row.cells[3].innerHTML + '</td>';
            html += '<td>' + row.cells[4].innerHTML + '</td>';
            html += '<td>' + row.cells[5].innerHTML + '</td>';
            html += '<td>' + row.cells[6].innerHTML + '</td>';
            html += '<td>' + row.cells[7].innerHTML + '</td>';
            html += '<td>' + row.cells[8].innerHTML + '</td>';
            html += '</tr>';
        }
    });


    $('#tbodyDTDConsList').html(html);

    if (rowSelCnt > 0) {
        //$('#btnGenList').show();
        //$('#btnResetClear').show();

        $('#cboOffice').prop('disabled', true);
        $('#cboRoute').prop('disabled', true);

    } else {
        swal("Invalid", "There are no selected DTD Consumer(s).", "warning");

        //$('#cboNumOfMo').prop('disabled', false);
        //$('#cboStatus').prop('disabled', false);
        //$('#cboOffice').prop('disabled', false);
        //$('#cboRoute').prop('disabled', false);

        return false;
    }
}

function selUnselAllDTDCons() {

    var isselected = $('#chkSelUnselDTDall').is(":checked");

    $targetCheckboxes = $('#chkSelUnselDTDall').closest('table').find('[name=select-item]');

    $targetCheckboxes.each(function () {

        // Set checkbox check/uncheck 
        // according to 'select all' status
        this.checked = isselected;
    });
}

function cboCheckedbyOnChange() {
    var sel = $('#cboCheckedBy').val();
    if (sel != 0)
        $('#btnSavePrev').removeAttr('disabled');
    else {
        $('#btnSavePrev').removeAttr('disabled');
        $('#btnSavePrev').attr('disabled', 'disabled');
    }
}

function savePrevSumDTDRpt() {
    var objhdr = {
        Id: 0,
        DateTimeGenerate: "",
        GenerateUserId: "0000",
        CheckByUserId: $('#cboCheckedBy').val(),
        CheckBy: "",
        NotedByUserId: $('#cboNotedBy').val(),
        NotedBy: "",
        RouteId: $('#cboRoute').val(),
        OfficeId: $('#cboOffice').val(),
        ActionDate: $('#dtpDisconDate').val()
    }

    if (objhdr != null) {
        $.ajax({
            type: "POST",
            url: "/SumOfDisconReport/InsertNewSumDTDRptHeader",
            contentType: 'application/json; charset=UTF-8',
            data: JSON.stringify(objhdr),
            dataType: "json",
            success: function (result) {
                if (result) {
                    //get the max header id of the generator by userid
                    $.ajax({
                        url: "/SumofDisconReport/GetHeaderIdOfNewInsertedSumDTDRpt/",
                        type: "GET",
                        contentType: "application/json;charset=UTF-8",
                        dataType: "json",
                        success: function (result) {
                            var headerid = result;
                            saveSumDTDRptDetailAndCrews(headerid);
                        },
                        error: function (errormessage) {
                            swal('Error', 'Something went wrong.', 'error');
                        }
                    });

                } else
                    swal("Failed", "Failed to save new Summary of Disconnection Report.", "error");
            },
            error: function (errormessage) {
                swal('Error','Something went wrong.','error');
            }
        });
    }
}

function saveSumDTDRptDetailAndCrews(hdrid) {
    //kunin number of rows in table
    var cnt = $('#tblAddedCrews tbody tr').length;
    //kunin lahat ng rows
    var tblrows = $('#tblAddedCrews tbody tr');

    var arrayData = new Array();

    //dito iyon magloloop per item (details)
    for (var i = 0; i < cnt; i++) {

        var $tds = tblrows[i].cells;

        var objCrews = {
            Id: 0,
            SumDTDRptHeaderId: parseInt(hdrid),
            DisconCrewId: parseInt($tds[0].innerText)
        };

        arrayData.push(objCrews);
    }

    $.ajax({
        url: "/SumOfDisconReport/InsertNewSumDTDRptCrews/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(arrayData),
        dataType: "json",
        success: function (result) {
            if (result) {
                //save details
                //kunin number of rows in table
                var cnt2 = $('#tblAddedDTDCons tbody tr').length;
                //kunin lahat ng rows
                var tbl2rows = $('#tblAddedDTDCons tbody tr');

                var arrayData2 = new Array();

                //dito iyon magloloop per item (details)
                for (var j = 0; j < cnt2; j++) {

                    var $tds2 = tbl2rows[j].cells;

                    var objDetails = {
                        Id: 0,
                        SumDTDRptHeaderId: parseInt(hdrid),
                        ConsumerId: $tds2[0].innerText,
                        Reason: $tds2[3].innerText,
                        LastReading: $tds2[4].innerText,
                        EncodeUserId: $tds2[5].innerText
                    };

                    arrayData2.push(objDetails);
                }

                $.ajax({
                    url: "/SumOfDisconReport/InsertNewSumDTDRptDetails/",
                    type: "POST",
                    contentType: "application/json;charset=UTF-8",
                    data: JSON.stringify(arrayData2),
                    dataType: "json",
                    success: function (finalresult) {
                        if (finalresult) {
                            //preview report
                            swal("Success", "Data has been saved.", "success");
                            //printing
                            var parent = $('embed#sodrpdf').parent();
                            var newElement = '<embed src="/SumOfDisconReport/PrintGeneratedSummaryOfDisconReport"  width="100%" height="800" type="application/pdf" id="sodrpdf">';

                            $('embed#sodrpdf').remove();
                            parent.append(newElement);

                            $('#myRptModal').modal('show');
                        }
                    },
                    error: function (errormessage) {
                        swal('Error', 'Something went wrong.', 'error');
                    }
                });

            } else 
                swal("Failed", "Failed to save new Summary of Disconnection Report.", "error");
        },
        error: function (errormessage) {
            swal('Error', 'Something went wrong.', 'error');
        }
    });
}