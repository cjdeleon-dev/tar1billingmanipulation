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
                    html += '<hr id="topHLine" />';

                    html += '<div class="row">';
                    html += '    <div style="background-color:darkgrey;text-align:center;"><strong>EXISTING MEMBER-CONSUMER\'S INFORMATION</strong ></div > ';
                    html += '    <div class="form-group col-sm-5" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtAcctName">Account Name:</label> <label for="chkDied" style="color:red;">Died:</label> ';
                    html += '        <input type="checkbox"  id="chkDied" onchange="chkDiedOnChange()" /> ';
                    html += '        <input type="text" class="form-control" id="txtAcctName" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-6" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtAddress">Address:</label> ';
                    html += '        <input type="text" class="form-control" id="txtAddress" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '</div> ';

                    html += '<div class="row"> ';
                    html += '    <div class="form-group col-sm-4" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtMemberId">Member OR #:</label> ';
                    html += '        <input type="text" class="form-control" id="txtMemberORNo" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtORDate">OR DATE:</label> ';
                    html += '        <input type="date" class="form-control" id="txtORDate" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtMeterNo">Meter S/N:</label> ';
                    html += '        <input type="text" class="form-control" id="txtMeterNo" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtSeqNo">Seq. No.:</label> ';
                    html += '        <input type="text" class="form-control" id="txtSeqNo" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '</div> ';
                    html += '<hr /> ';
                    html += '<div class="row"> ';
                    html += '    <div class="form-group col-sm-6" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtNewName">New Name:</label> ';
                    html += '        <input type="text" class="form-control" id="txtNewName" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '</div> ';

                    html += '<div class="row"> ';
                    //html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    //html += '        <label for="txtMemberNewORNo">New Member OR #:</label> ';
                    //html += '        <input type="text" class="form-control" id="txtMemberNewORNo" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    //html += '    </div> ';
                    //html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    //html += '        <label for="dtpNewORDate">New OR DATE:</label> ';
                    //html += '        <input type="date" class="form-control" id="dtpNewORDate" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    //html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="dtpBirthday">Birthday:</label> ';
                    html += '        <input type="date" class="form-control" id="dtpBirthday" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtContactNo">Contact No.:</label> ';
                    html += '        <input type="text" class="form-control" id="txtContactNo" placeholder="09XXXXXXXXX" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" onkeypress="isNumber(event)" maxlength="11" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtRelationship">Relationship:</label> ';
                    html += '        <input type="text" class="form-control" id="txtRelationship" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-3" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtReason">Reason For Change Name:</label> ';
                    html += '        <input type="text" class="form-control" id="txtReason" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-3" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtRemarks">Remarks:</label> ';
                    //html += '        <input type="text" class="form-control" id="txtRemarks" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '        <select class="select form-control" id="cboRemarks"> ';
                    html += '            <option value="0" selected>--SELECT REMARK--</option> ';
                    html += '            <option value="1">WITH DEATH CERTIFICATE</option> ';
                    html += '            <option value="2">WITH AUTHORIZATION LETTER</option> ';
                    html += '            <option value="3">WITH DEED OF SALE</option> ';
                    html += '            <option value="2">WITH LETTER OF REQUEST</option> ';
                    html += '        </select> ';
                    html += '    </div> ';
                    html += '</div> ';

                    html += '<div class="row"> ';
                    html += '    <div class="form-group col-sm-4" style="padding:0;margin:7px;"> ';
                    html += '        <input type="radio" id="optradio_oldmem" name="changenametype" /><br />';
                    html += '        <label for="optradio_oldmem" id="oldmem">For Withdrawal Of Membership (OLD MEMBER)</label> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-4" style="padding:0;margin:7px;"> ';
                    html += '        <input type="radio" id="optradio_newmem" name="changenametype" /><br />';
                    html += '        <label for="optradio_newmem" id="newmem">For Withdrawal Of Membership (NEW MEMBER)</label> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-3" style="padding:0;margin:7px;"> ';
                    html += '        <input type="radio" id="optradio_ret" name="changenametype" /><br /> ';
                    html += '        <label for="optradio_ret" id="newmem">For Retention Of Membership</label> ';
                    html += '    </div> ';
                    html += '</div> ';

                    html += '<div class="row"> ';
                    html += '    <div class="form-group" style="padding:0;margin:7px;"> ';
                    html += '        <div class="col-sm-offset-4 col-sm-4"> ';
                    html += '           <input type="button" class="btn btn-primary" id="btnSave" name="btnSave" value="SAVE AND PREVIEW" onclick="saveAndPrev()" />';
                    html += '        </div> ';
                    html += '    </div> ';
                    html += '</div> ';
                    
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

function chkDiedOnChange() {
    var isdied = document.getElementById("chkDied").checked;
    if (isdied) {
        $('#optradio_newmem').attr('checked', 'checked');
        $('#optradio_oldmem').attr('disabled', 'disabled');
        $('#optradio_ret').attr('disabled', 'disabled');
    } else {
        $('#optradio_newmem').removeAttr('checked');
        $('#optradio_oldmem').removeAttr('disabled');
        $('#optradio_ret').removeAttr('disabled');
    }
}

function saveAndPrev() {
    var chkold, chknew, chkret, isdied;

    chkold = document.getElementById("optradio_oldmem").checked;
    chknew = document.getElementById("optradio_newmem").checked;
    chkret = document.getElementById("optradio_ret").checked;
    isdied = document.getElementById("chkDied").checked;

    if ($('#txtMemberORNo').val() == '') {
        swal('Invalid', 'Please input Member OR No.', 'warning');
        return;
    }

    if ($('#txtORDate').val() == '') {
        swal('Invalid', 'Please input Member OR Date.', 'warning');
        return;
    }

    if ($('#txtReason').val() == '') {
        swal('Invalid', 'Please input Reason.', 'warning');
        return;
    }

    if ($('#cboRemarks').val() == 0) {
        swal('Invalid', 'Please select remark.', 'warning');
        return;
    }

    var objcna = {
        Id: 0,
        ApplicationDate: "",
        AccountNo: $('#txtAccountNo').val(),
        AccountName: $('#txtAcctName').val(),
        Address: "",
        MemberId: $('#txtMemberORNo').val(),
        MemberDate: $('#txtORDate').val(),
        SequenceNo: "",
        IsDied: isdied,
        NewName: $('#txtNewName').val(),
        NewMemberId: "",
        NewMemberDate: "",
        Birthday: $("#dtpBirthday").val(),
        ContactNo: $('#txtContactNo').val(),
        Relationship: $('#txtRelationship').val(),
        Reason: $('#txtReason').val(),
        ForWithdrawOld: chkold,
        ForWithdrawNew: chknew,
        ForRetention: chkret,
        Remarks: $('#cboRemarks option:selected').text(),
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
                swal('Success', 'Successfully Saved', 'success');

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