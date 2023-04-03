const { stringify } = require("querystring");

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
                    html += '    <div style="background-color:darkgrey;text-align:center;"><strong>EXISTING MEMBER-COSUMER\'S INFORMATION</strong ></div > ';
                    html += '    <div class="form-group col-sm-5" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtAcctName">Account Name:</label> ';
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
                    html += '        <input type="text" class="form-control" id="txtMemberORNo" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtORDate">OR DATE:</label> ';
                    html += '        <input type="text" class="form-control" id="txtORDate" readonly="readonly" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
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
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtMemberNewORNo">New Member OR #:</label> ';
                    html += '        <input type="text" class="form-control" id="txtMemberNewORNo" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="dtpNewORDate">New OR DATE:</label> ';
                    html += '        <input type="date" class="form-control" id="dtpNewORDate" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="dtpBirthday">Birthday:</label> ';
                    html += '        <input type="date" class="form-control" id="dtpBirthday" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtContactNo">Contact No.:</label> ';
                    html += '        <input type="text" class="form-control" id="txtContactNo" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtRelationship">Relationship:</label> ';
                    html += '        <input type="text" class="form-control" id="txtRelationship" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-6" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtReason">Reason For Change Name:</label> ';
                    html += '        <input type="text" class="form-control" id="txtReason" style="max-width:inherit;border-style:dashed;border-color:#9b9b9b;background-color:transparent;" /> ';
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
                    $('#txtMemberId').val(result.MemberId);
                    $('#txtORDate').val(result.ORDate);
                    $('#txtMeterNo').val(result.MeterNo);
                    $('#txtSeqNo').val(result.SeqNo);

                    $('#txtAcctName').attr('readonly', 'readonly');
                    $('#txtAddress').attr('readonly', 'readonly');
                    $('#txtMemberId').attr('readonly', 'readonly');
                    $('#txtORDate').attr('readonly', 'readonly');
                    $('#txtMeterNo').attr('readonly', 'readonly');
                    $('#txtSeqNo').attr('readonly', 'readonly');
                    $('#txtAccountNo').attr('readonly', 'readonly');
                    $('#btnCheckAcct').hide();
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

function saveAndPrev() {
    var chkold, chknew, chkret;

    chkold = document.getElementById("optradio_oldmem").checked;
    chknew = document.getElementById("optradio_newmem").checked;
    chkret = document.getElementById("optradio_ret").checked;

    var objcna = {
        Id: 0,
        ApplicationDate: "",
        AccountNo: $('#txtAccountNo').val(),
        AccountName: "",
        Address: "",
        MemberId: "",
        MemberDate: "",
        SequenceNo: "",
        NewName: $('#txtNewName').val(),
        NewMemberId: $('#txtMemberNewORNo').val(),
        NewMemberDate: $('#dtpNewORDate').val(),
        Birthday: $("#dtpBirthday").val(),
        ContactNo: $('#txtContactNo').val(),
        Relationship: $('#txtRelationship').val(),
        Reason: $('#txtReason').val(),
        ForWithdrawOld: chkold,
        ForWithdrawNew: chknew,
        ForRetention: chkret,
        Remarks: "",
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