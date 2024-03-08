//const { stringify } = require("querystring");

function checkAcct() {
    var acctnum = $('#txtAccountNo').val();

    if (acctnum.trim() == "")
        swal("Invalid!", "Account Number is not valid.", "warning");
    else {
        $.ajax({
            url: "/ChangeNameApplication/GetAccountDetails?acctnum=" + acctnum,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                if (result.AccountNo == null)
                    swal("Not Exist!", "Account Number is not exist.", "warning");
                else {
                    var html = '';
                    html += '<div class="row">';
                    html += '    <div style="background-color:darkgrey;text-align:center;"><strong>TYPE OF CHANGE NAME</strong></div>';
                    html += '    <div class="col-lg-6" style="padding:0;margin:7px;">';
                    html += '        <input type="radio" id="optradio_withdraw" name="changenametype" onchange="optwithdrawonchange()" />';
                    html += '        <label for="optradio_withdraw" id="withdraw">FOR FULL WITHDRAWAL OF MEMBERSHIP</label>';
                    html += '        <div class="row">';
                    html += '            <div class="col-sm-12">';
                    html += '                <input type="radio" id="optdeath" name="withdrawtype" style="margin-left:2em;" disabled="disabled" onchange="optdeathonchange()" />&nbsp;<label for="optdeath">DEATH OF MEMBER</label><br />';
                    html += '                <input type="radio" id="optwaive" name="withdrawtype" style="margin-left:2em;" disabled="disabled" onchange="optwaiveonchange()" />&nbsp;<label for="optwaver">WAIVE OF MEMBERSHIP</label>';
                    html += '            </div>';
                    html += '        </div>';
                    html += '    </div>';
                    html += '    <div class="col-lg-5" style="padding:0;margin:7px;">';
                    html += '        <input type="radio" id="optradio_retention" name="changenametype" onchange="optretentiononchange()" />';
                    html += '        <label for="optradio_retention" id="retention">FOR RETENTION OF MEMBERSHIP</label>';
                    html += '        <div class="row">';
                    html += '            <div class="col-sm-12">';
                    html += '                <input type="radio" id="optjoint" name="retentiontype" style="margin-left:2em;" disabled="disabled" />&nbsp;<label for="optdeath">JOINT ACCOUNT</label><br />';
                    html += '                <input type="radio" id="optexist" name="retentiontype" style="margin-left:2em;" disabled="disabled" />&nbsp;<label for="optwaver">WITH DIFF. EXISTING ACCOUNT</label>';
                    html += '            </div>';
                    html += '        </div>';
                    html += '    </div>';
                    html += '</div>';
                    html += '<hr />';
                    html += '<div class="row">';
                    html += '    <div style="background-color:darkgrey;text-align:center;"><strong>EXISTING MEMBER-CONSUMER\'S INFORMATION</strong></div>';
                    html += '    <div class="form-group col-sm-5" style="padding:0;margin:7px;">';
                    html += '        <label for="txtAcctName">Account Name:</label>';
                    html += '        <input type="text" class="form-control" id="txtAcctName" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '    <div class="form-group col-sm-6" style="padding:0;margin:7px;">';
                    html += '        <label for="txtAddress">Address:</label>';
                    html += '        <input type="text" class="form-control" id="txtAddress" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '</div>';
                    html += '<div class="row">';
                    html += '    <div class="form-group col-sm-4" style="padding:0;margin:7px;">';
                    html += '        <label for="txtMemberId">Member OR #:</label>';
                    html += '        <input type="text" class="form-control" id="txtMemberORNo" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;">';
                    html += '        <label for="txtORDate">OR DATE:</label>';
                    html += '        <input type="date" class="form-control" id="txtORDate" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;">';
                    html += '        <label for="txtMeterNo">Meter S/N:</label>';
                    html += '        <input type="text" class="form-control" id="txtMeterNo" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;">';
                    html += '        <label for="txtSeqNo">Seq. No.:</label>';
                    html += '        <input type="text" class="form-control" id="txtSeqNo" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '</div>';
                    html += '<hr />';
                    html += '<div class="row">';
                    html += '    <div class="form-group col-sm-6" style="padding:0;margin:7px;">';
                    html += '        <label for="txtNewName">New Name:</label>';
                    html += '        <input type="text" class="form-control" id="txtNewName" maxlength="30" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '</div>';
                    html += '<div class="row">';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;">';
                    html += '        <label for="dtpBirthday">Birthday:</label>';
                    html += '        <input type="date" class="form-control" id="dtpBirthday" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;">';
                    html += '        <label for="txtContactNo">Contact No.:</label>';
                    html += '        <input type="text" class="form-control" id="txtContactNo" placeholder="09XXXXXXXXX" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" onkeypress="isNumber(event)" maxlength="11" />';
                    html += '    </div>';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;">';
                    html += '        <label for="txtRelationship">Relationship to the Owner:</label>';
                    html += '        <input type="text" class="form-control" id="txtRelationship" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '    <div class="form-group col-sm-3" style="padding:0;margin:7px;">';
                    html += '        <label for="txtReason">Reason For Change Name:</label>';
                    html += '        <input type="text" class="form-control" id="txtReason" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" />';
                    html += '    </div>';
                    html += '</div>';
                    html += '<hr />';
                    html += '<div class="row">';
                    html += '    <div class="col-lg-12">';
                    html += '        <strong>SUBMITTED DOCUMENT(S):</strong>';
                    html += '        <div>';
                    html += '            <input type="checkbox" id="chkdeathcert" name="docsub" style="margin-left:2em;" />&nbsp;<label for="chkdeathcert">DEATH CERTIFICATE</label>';
                    html += '            <input type="checkbox" id="chkauthletter" name="docsub" style="margin-left:2em;" />&nbsp;<label for="chkauthletter">AUTHORIZATION LETTER</label>';
                    html += '            <input type="checkbox" id="chkdos" name="docsub" style="margin-left:2em;" />&nbsp;<label for="chkdos">DEED OF SALE</label>';
                    html += '            <input type="checkbox" id="chklor" name="docsub" style="margin-left:2em;" />&nbsp;<label for="chklor">LETTER OF REQUEST</label>';
                    html += '            <input type="checkbox" id="chkother" name="docsub" style="margin-left:2em;" onchange="chkotheronchange()" />&nbsp;<label for="chkother">OTHER: </label>';
                    html += '            <input type="text" id="txtOther" name="docsub" disabled="disabled" />';
                    html += '        </div>';
                    html += '    </div>';
                    html += '</div>';
                    html += '<hr />';
                    html += '<div class="row">';
                    html += '    <div class="col-lg-12">';
                    html += '        <strong>REPORT REMARK:</strong>';
                    html += '        <div>';
                    html += '            <input type="radio" id="optretention" name="remarkstype" style="margin-left:2em;" />&nbsp;<label for="optretention">FOR RETENTION</label>';
                    html += '            <input type="radio" id="optdeletion" name="remarkstype" style="margin-left:2em;" />&nbsp;<label for="optdeletion">FOR DELETION</label>';
                    html += '        </div>';
                    html += '    </div>';
                    html += '</div>';
                    html += '<hr />';
                    html += '<div class="row">';
                    html += '    <div class="form-group" style="padding:0;margin:7px;">';
                    html += '        <div class="col-sm-offset-4 col-sm-4">';
                    html += '            <input type="button" class="btn btn-primary" id="btnSave" name="btnSave" value="SAVE AND PREVIEW" onclick="saveAndPrev()" />';
                    html += '        </div>';
                    html += '    </div>';
                    html += '</div>';
                    
                    $('#divExistingAcctDetails').append(html);
                    $('#txtAcctName').val(result.AccountName);
                    $('#txtAddress').val(result.Address);
                    $('#txtMemberORNo').val(result.MemberId);
                    $('#txtORDate').val(result.ORDate);
                    $('#txtMeterNo').val(result.MeterNo);
                    $('#txtSeqNo').val(result.SeqNo);

                    $('#txtAcctName').attr('readonly', 'readonly');
                    $('#txtAddress').attr('readonly', 'readonly');
                    //$('#txtMemberId').attr('readonly', 'readonly');
                    //$('#txtORDate').attr('readonly', 'readonly');
                    $('#txtMeterNo').attr('readonly', 'readonly');
                    $('#txtSeqNo').attr('readonly', 'readonly');
                    $('#txtAccountNo').attr('readonly', 'readonly');

                    $('#divVerify').hide();
                    $('#btnMenu').hide();
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    return false;
}

function resetPage() {
    window.location = "/ChangeNameApplication/Index";
}

//function chkDiedOnChange() {
//    var isdied = document.getElementById("chkDied").checked;
//    if (isdied) {
//        $('#optradio_newmem').attr('checked', 'checked');
//        $('#optradio_oldmem').attr('disabled', 'disabled');
//        $('#optradio_ret').attr('disabled', 'disabled');
//    } else {
//        $('#optradio_newmem').removeAttr('checked');
//        $('#optradio_oldmem').removeAttr('disabled');
//        $('#optradio_ret').removeAttr('disabled');
//    }
//}
function validateinputs() {
    let cntype = 0;
    let docsub = 0;

    if (document.getElementById("optdeath").checked)
        cntype = 1;
    if (document.getElementById("optwaive").checked)
        cntype = 2;
    if (document.getElementById("optjoint").checked)
        cntype = 3;
    if (document.getElementById("optexist").checked)
        cntype = 4;

    if (cntype == 0)
        return false;

    if (document.getElementById("chkdeathcert").checked)
        docsub += 1;
    if (document.getElementById("chkauthletter").checked)
        docsub += 1;
    if (document.getElementById("chkdos").checked)
        docsub += 1;
    if (document.getElementById("chklor").checked)
        docsub += 1;
    if (document.getElementById("chkother").checked)
        docsub += 1;

    if (docsub == 0)
        return false;

    return true;
                            
}

function saveAndPrev() {
    //VALIDATION FIRST

    if (!validateinputs) {
        alert("Please fill up all required fields.");
        return;
    }
        

    //var chkold, chknew, chkret, isdied;

    //chkold = document.getElementById("optradio_oldmem").checked;
    //chknew = document.getElementById("optradio_newmem").checked;
    //chkret = document.getElementById("optradio_ret").checked;
    //isdied = document.getElementById("chkDied").checked;

    var chkdeathcert, chkauthletter, chkdos, chklor, chkother;
    var reprem, othertext;
    var typeid = 0;

    chkdeathcert = document.getElementById("chkdeathcert").checked;
    chkauthletter = document.getElementById("chkauthletter").checked;
    chkdos = document.getElementById("chkdos").checked;
    chklor = document.getElementById("chklor").checked;
    chkother = document.getElementById("chkother").checked;

    if (document.getElementById("optdeath").checked)
        typeid = 1;
    if (document.getElementById("optwaive").checked)
        typeid = 2;
    if (document.getElementById("optjoint").checked)
        typeid = 3;
    if (document.getElementById("optexist").checked)
        typeid = 4;

    if (chkother)
        othertext = $('#txtOther').val();

    if (document.getElementById("optretention").checked)
        reprem = "R"

    if (document.getElementById("optdeletion").checked)
        reprem = "D"

    if ($('#txtMemberORNo').val() == '') {
        swal('\nInvalid', 'Please input Member OR No.', 'warning');
        return;
    }

    if ($('#txtORDate').val() == '') {
        swal('\nInvalid', 'Please input Member OR Date.', 'warning');
        return;
    }

    if ($('#txtReason').val() == '') {
        swal('\nInvalid', 'Please input Reason.', 'warning');
        return;
    }

    //if ($('#cboRemarks').val() == 0) {
    //    swal('Invalid', 'Please select remark.', 'warning');
    //    return;
    //}

    var isDiedMem;

    if (reprem == "D")
        isDiedMem = true;
    else
        isDiedMem = false;

    var objcna = {
        Id: 0,
        ApplicationDate: "",
        AccountNo: $('#txtAccountNo').val(),
        AccountName: $('#txtAcctName').val(),
        Address: "",
        MemberId: $('#txtMemberORNo').val(),
        MemberDate: $('#txtORDate').val(),
        SequenceNo: "",
        IsDied: isDiedMem,
        NewName: $('#txtNewName').val(),
        NewMemberId: "",
        NewMemberDate: "",
        Birthday: $("#dtpBirthday").val(),
        ContactNo: $('#txtContactNo').val(),
        Relationship: $('#txtRelationship').val(),
        Reason: $('#txtReason').val(),
        ChangeNameTypeId: typeid,
        ChangeNameDesc: "",
        IsRemDeathCert: chkdeathcert,
        IsRemAuthLetter: chkauthletter,
        IsRemDeedOfSale: chkdos,
        IsRemLetterOfReq: chklor,
        IsRemOther: chkother,
        RemOtherText: othertext,
        RptRemark: reprem,
        //ForWithdrawOld: chkold,
        //ForWithdrawNew: chknew,
        //ForRetention: chkret,
        /*Remarks: $('#cboRemarks option:selected').text(),*/
        MadeById: ""
    }

    $.ajax({
        url: "/ChangeNameApplication/InsertNewApplicant",
        type: "POST",
        data: JSON.stringify(objcna),
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result) {
                //preview report
                swal('\nSuccess', 'Successfully Saved', 'success');

                //printing
                var parent = $('embed#cnpdf').parent();
                var newElement = '<embed src="/ChangeNameApplication/PreviewChangeNameApplicationReport"  width="100%" height="800" type="application/pdf" id="cnpdf">';

                $('embed#cnpdf').remove();
                parent.append(newElement);

                $('#myRptModal').modal('show');
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}

function showPendingList() {
    window.location = "/PendingChangeNameList/Index";
}

function showApprovalList() {
    window.location = "/ApprovalChangeNameList/Index";
}

function showBODResList() {
    window.location = "/ChangeNameListForBODRes/Index";
}

function showAppliedList() {
    window.location = "/ChangeNameAppliedList/Index";
    return;
}



function isNumber(evt) {
    var ch = String.fromCharCode(evt.which);
    if (!(/[0-9]/.test(ch))) {
        evt.preventDefault();
    }
    if (evt.keyCode === 13) {
        evt.preventDefault();
    }
}

function optwithdrawonchange() {
    var isChecked = $('#optradio_withdraw').is(':checked');
    if (isChecked) {
        $('#optdeath').removeAttr('disabled');
        $('#optwaive').removeAttr('disabled');

        $('#optjoint').prop('checked', false);
        $('#optexist').prop('checked', false);

        $('#optjoint').removeAttr('disabled');
        $('#optexist').removeAttr('disabled');

        $('#optjoint').attr('disabled', true);
        $('#optexist').attr('disabled', true);

    }
}

function optretentiononchange() {
    var isChecked = $('#optradio_retention').is(':checked');
    if (isChecked) {
        $('#optjoint').removeAttr('disabled');
        $('#optexist').removeAttr('disabled');

        $('#optdeath').prop('checked', false);
        $('#optwaive').prop('checked', false);

        $('#optdeath').removeAttr('disabled');
        $('#optwaive').removeAttr('disabled');

        $('#optdeath').attr('disabled', true);
        $('#optwaive').attr('disabled', true);

        $('#optdeletion').prop('checked', false);
        $('#optretention').prop('checked', true);

    }
}

function optdeathonchange() {
    var isChecked = $('#optdeath').is(':checked');
    if (isChecked) {
        $('#optdeletion').prop('checked', true);
        $('#optretention').prop('checked', false);
    }
}

function optwaiveonchange() {
    var isChecked = $('#optwaive').is(':checked');
    if (isChecked) {
        $('#optdeletion').prop('checked', false);
        $('#optretention').prop('checked', true);
    }
}

function chkotheronchange() {
    if ($('#chkother').is(':checked') === true) {
        $('#txtOther').removeAttr('disabled');
        $('#txtOther').focus();
    }
    else {
        $('#txtOther').val("");
        $('#txtOther').removeAttr('disabled');
        $('#txtOther').attr('disabled',true);
    }
}