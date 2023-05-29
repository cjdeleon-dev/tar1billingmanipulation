function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#mnudbsettings').hide();
    } else {
        $('#mnudbsettings').show();
    }

}

function loadPage() {
    loadAllCbos();
    hideunneccessaryoptions();
}

function checkAcct() {
    var acctnum = $('#txtAccountNo').val();

    if (acctnum.trim() == "") {
        swal("Invalid!", "Account Number is not valid.", "warning");
        $('#txtAccountNo').val("");
    }
    else {
        $.ajax({
            url: "/MemCons/GetAccountDetails?accountNum=" + acctnum,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                if (result.ConsumerId == null)
                    swal("Not Exist!", "Account Number is not exist.", "warning");
                else {
                    $('#txtAccountNo').attr('disabled', 'disabled');
                    $('#txtName').val(result.Name);
                    $('#txtStatus').val(result.Status);
                    $('#txtStatusId').val(result.StatusId);
                    $('#txtAddress').val(result.Address);
                    $('#txtPoleId').val(result.PoleId);
                    $('#cboConsumerType').val(result.ConsumerTypeId);
                    $('#txtMemberOR').val(result.MemberOR);
                    $('#dtpMemberDate').val(result.MemberDate);
                    $('#txtBookNo').val(result.BookNo);
                    $('#txtSeqNo').val(result.SequenceNo);
                    $('#cboArea').val(result.AreaId);
                    $('#cboOffice').val(result.OfficeId);
                    $('#dtpBurial').val(result.ClaimedBurialDate);
                    $('#txtFlatRate').val(result.FlatRate);
                    $('#txtKVARate').val(result.KVALoad);
                    $('#txtFixedDem').val(result.FlatDemand);
                    $('#txtTSFRental').val(result.TSFRental);
                    $('#txtCoreloss').val(result.Coreloss);
                    $('#txtMultiplier').val(result.Multiplier);
                    $('#txtTSFCount').val(result.TSFCount);
                    $('#txtSN').val(result.MeterSerialNo);
                    $('#txtPrevSN').val(result.PrevMeterSerialNo);
                    $('#txtMtrBrand').val(result.MeterBrand);
                    $('#txtMtrAmp').val(result.MeterAmp);
                    $('#txtMtrType').val(result.MeterType);
                    $('#txtCalSeal').val(result.MeterSealNo);
                    $('#txtTermSeal').val(result.MeterSideSealNo);
                    $('#txtDial').val(result.MeterDial);
                    $('#dtpDateIns').val(result.DateInstalled);
                    $('#cboSCFlag').val(result.SCFlag);
                    $('#cboSRFlag').val(result.CLRSR);

                    $('#btnCS').removeAttr('disabled');
                    $('#btnUP').removeAttr('disabled');
                    $('#btnUL').removeAttr('disabled');
                    $('#btnMS').removeAttr('disabled');
                    $('#btnHS').removeAttr('disabled');
                    $('#btnBL').removeAttr('disabled');
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

function loadAllCbos() {
    loadcboConsumerType();
    loadcboArea();
    loadcboOffice();
    //disabled all buttons
    $('#btnCS').attr('disabled', 'disabled');
    $('#btnUP').attr('disabled', 'disabled');
    $('#btnUL').attr('disabled', 'disabled');
    $('#btnMS').attr('disabled', 'disabled');
    $('#btnHS').attr('disabled', 'disabled');
    $('#btnBL').attr('disabled', 'disabled');

    //Load first all cbos
    loadcboModArea();
    loadcboModOffice();
    loadcboModConsumerType();
}

function loadcboConsumerType() {
    $.ajax({
        url: "/MemCons/GetAllTypes/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboConsumerType').empty();
            $('#cboConsumerType').append("<option value=0></option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].ConsumerType;
                var opt = new Option(Desc, result[i].Id);
                $('#cboConsumerType').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadcboArea() {
    $.ajax({
        url: "/MemCons/GetAllAreas/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboArea').empty();
            $('#cboArea').append("<option value=0></option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Area;
                var opt = new Option(Desc, result[i].Id);
                $('#cboArea').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadcboOffice() {
    $.ajax({
        url: "/MemCons/GetAllOffices/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboOffice').empty();
            $('#cboOffice').append("<option value=0></option>");
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

function resetPage() {
    window.location = "/MemCons/MemberConsumer";
}


function showChangeStatModal() {

    var isRecordOfcr = $('#txtIsRecOfcr').val();
    var isTeller = $('#txtIsTeller').val();

    if (isRecordOfcr == "True" && $('#txtStatusId').val() == "D") {
        swal("Sorry", "You do not have the right to change its status from Disconnected to Active.", "warning");
        return false;
    }
        
    if (isTeller == "True" && $('#txtStatusId').val() == "A") {
        swal("Sorry", "You do not have the right to change its status from Active to Disconnected or Apprehended.", "warning");
        return false;
    }

    $('#myChangeStatusModal').modal('show');
    loadcboChangeTo();
    $('#txtChangeFr').val($('#txtStatus').val());
    $('#txtStatusFrom').val($('#txtStatusId').val())

    return true;
}

function loadcboChangeTo() {
    var status = $('#txtStatusId').val();
    $.ajax({
        url: "/MemCons/GetAllStatus?exceptstat=" + status,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboChangeTo').empty();
            //$('#cboChangeTo').val(0);
            $('#cboChangeTo').append("<option value='Q'>Select Status</option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Description;
                var opt = new Option(Desc, result[i].StatusId);
                $('#cboChangeTo').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function isValidEntry() {

    var returnVal = true;

    if ($('#txtStatusFrom').val() == "")
        return false;
    if ($('#cboChangeTo').val() == "" || $('#cboChangeTo').val() == "Q")
        return false;
    if ($('#txtReason').val() == "")
        return false;
    if ($('#dtpActDate').val() == "")
        return false;

    return returnVal;
}

function setStatusByAcctno() {

    if (isValidEntry() == true) {
        var acctnum = $('#txtAccountNo').val();
        var dateentry = "";
        var statfrom = $('#txtStatusFrom').val();
        var changefrom = "";
        var statto = $('#cboChangeTo').val();
        var changeto = "";
        var reason = $('#txtReason').val();
        var entryuser = "";
        var actiondate = $('#dtpActDate').val();
        var dtdread = $('#txtDtdRead').val();

        var dataObj = {
            AccountNo: acctnum,
            DateEntry: dateentry,
            StatusFrom: statfrom,
            ChangeStatusFrom: changefrom,
            StatusTo: statto,
            ChangeStatusTo: changeto,
            Reason: reason,
            EntryUser: entryuser,
            ActionDate: actiondate,
            DTDRead: dtdread
        }

        $.ajax({
            data: JSON.stringify(dataObj),
            url: "/MemCons/SetStatusOfConsumerById/",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.message == "Success") {
                    $('#txtReason').val('');
                    $('#txtDtdRead').val('');
                    $('#myChangeStatusModal').modal('hide');
                    checkAcct();
                    swal('Status Changed!', 'Successfully Changed.', 'success');
                    
                } else {
                    swal('Error', 'Fail to change status.', 'error');
                }
            },
            error: function (errormessage) {
                swal('Error', errormessage.responseText, 'warning');
                alert(errormessage.responseText);
            },
        });
    }
    else {
        swal('Invalid Entry', 'Please fill all required fields.', 'warning');
    }
}

function viewStatusByAcctno() {

    $('#modTxtAcctNo').val($('#txtAccountNo').val());
    $('#modTxtAcctName').val($('#txtName').val().trim());

    var table = $('#statusTable').DataTable();
    table.clear().draw();
    table.destroy();

    $('#statusTable').DataTable({
        "sort": false,
        "searching": false,
        "paging": false,
        "ajax": {
            "url": "/MemCons/LoadStatusLogData?accountno=" + $('#txtAccountNo').val(),
            "type": "GET",
            "datatype": "json",
            "dataSrc": function (obj) {

                if (obj.data == null) {
                    return "";
                } else {
                    return obj.data;
                }

            }
        },
        "columns": [
            { "data": "DateEntry", "autoWidth": true },
            { "data": "ChangeStatusFrom", "autoWidth": true },
            { "data": "ChangeStatusTo", "autoWidth": true },
            { "data": "Reason", "autoWidth": true },
            { "data": "EntryUser", "autoWidth": true },
            { "data": "ActionDate", "autoWidth": true },
            { "data": "DTDRead", "autoWidth": true }
        ]
    });

    $('#myViewStatusLogModal').modal('show');
}

function showUpdateProfileModal() {


    var typ = $('#cboConsumerType').val();
    var area = $('#cboArea').val();
    var offc = $('#cboOffice').val();

    $('#myUpdProfileModal').modal('show');

    $('#txtModName').val($('#txtName').val());
    $('#txtModStatus').val($('#txtStatus').val());
    $('#txtModAddress').val($('#txtAddress').val());
    $('#txtModPoleId').val($('#txtPoleId').val());
    $('#txtModMemberOR').val($('#txtMemberOR').val());
    $('#dtpModMemberDate').val($('#dtpMemberDate').val());
    $('#txtModBookNo').val($('#txtBookNo').val());
    $('#txtModSeqNo').val($('#txtSeqNo').val());
    $('#dtpModBurialDate').val($('#dtpBurial').val())

    if ($('#dtpBurial').val() != "") {
        
        document.getElementById('chkIsClaimed').checked = true;
    }   
    else
        document.getElementById('chkIsClaimed').checked = false;


    $('#cboModConsumerType').val(typ);
    $('#cboModArea').val(area);
    $('#cboModOffice').val(offc);

}

//function closeUpdProfileModal() {
//    $('#myUpdProfileModal').empty();
//    $('#myUpdProfileModal').modal('hide');
//}


function loadcboModArea() {
    $.ajax({
        url: "/MemCons/GetAllAreas/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboModArea').empty();
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Area;
                var opt = new Option(Desc, result[i].Id);
                $('#cboModArea').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadcboModOffice() {
    $.ajax({
        url: "/MemCons/GetAllOffices/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboModOffice').empty();
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Office;
                var opt = new Option(Desc, result[i].Id);
                $('#cboModOffice').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadcboModConsumerType() {
    $.ajax({
        url: "/MemCons/GetAllTypes/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboModConsumerType').empty();
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].ConsumerType;
                var opt = new Option(Desc, result[i].Id);
                $('#cboModConsumerType').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateProfile() {

    var chk;
    chk = document.getElementById("chkIsClaimed").checked;

    if (chk) {
        if ($('#dtpModBurialDate').val() == "")
            return false;
    } else
        $('#dtpModBurialDate').val("");
        
    var objProfile = {
        AccountNo: $('#txtAccountNo').val(),
        AccountName: "",
        AccountStat: "",
        AccountAdd: $('#txtModAddress').val(),
        AccountPoleId: $('#txtModPoleId').val(),
        AccountTypeId: $('#cboModConsumerType').val(),
        AccountType: "",
        MemberOr: $('#txtModMemberOR').val(),
        MemberORDate: $('#dtpModMemberDate').val(),
        BookNo: $('#txtModBookNo').val(),
        SeqNo: $('#txtModSeqNo').val(),
        AreaId: $('#cboModArea').val(),
        Area: "",
        OfficeId: $('#cboModOffice').val(),
        Office: "",
        IsClaimedBurial: chk,
        ClaimedBurialDate: $('#dtpModBurialDate').val(),
        UpdatedBy: "",
    }

    if (objProfile != null) {
        $.ajax({
            type: "POST",
            url: "/MemCons/UpdateMemConsProfile",
            contentType: 'application/json; charset=UTF-8',
            data: JSON.stringify(objProfile),
            dataType: "json",
            success: function (result) {
                if (result) {
                    swal('Success', 'Successfully Updated.', 'success');
                    checkAcct();
                } else
                    swal("Failed", "Failed to update member consumer profile.", "error");
            },
            error: function (errormessage) {
                swal('Error', 'Something went wrong.', 'error');
            }
        });
    }
}

function viewBalance() {
    var acctno = $('#txtAccountNo').val();
    if (acctno != "") {
        $.ajax({
            url: "/MemCons/ViewAccountBalance?accountno=" + acctno,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {

                console.log(result.data);

                $('#tbodybal').empty;

                if ($.trim(result.data) == "" || $.trim(result.data) == undefined) {
                    swal("No Data", "No outstanding balance seen.", "warning");
                } else {
                    var html = '';
                    $.each(result.data, function (key, item) {
                        html += '<tr class="" id="' + item.BillPeriod + '">';
                        html += '<td>' + item.BillPeriod + '</td>';
                        html += '<td style="text-align:right;">' + item.TrxBalance + '</td>';
                        html += '<td style="text-align:right;">' + item.VATBalance + '</td>';
                        html += '<td style="text-align:right;">' + item.Surcharge + '</td>';
                        html += '<td style="text-align:right;">' + item.TotalAmount + '</td>';
                        html += '<td style="text-align:center;">' + item.Months + '</td>';
                        html += '<td style="text-align:right;">' + item.PayAmount + '</td>';
                        html += '</tr>';
                    });
                    $('#tbodybal').html(html);

                    $('#myViewBalanceModal').modal('show');
                }
                    
            },
            error: function (errormessage) {
                swal(errormessage.responseText,"warning");
            }
        });
    }

    
}


function chkBurialOnChange() {
    var chk;
    var burialDate;
    burialDate = $('#dtpBurial').val();

    chk = document.getElementById("chkIsClaimed").checked;
    if (chk) {
        $('#dtpModBurialDate').removeAttr('disabled');
        $('#dtpModBurialDate').val(burialDate);
    }
    else {
        $('#dtpModBurialDate').removeAttr('disabled');
        $('#dtpModBurialDate').attr('disabled', 'disabled');
        $('#dtpModBurialDate').val("");
    }

}