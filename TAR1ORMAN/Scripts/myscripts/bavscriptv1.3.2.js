function checkAcct() {
    var acctnum = $('#txtAccountNo').val();

    if (acctnum.trim() == "")
        swal("Invalid!", "Account Number is not valid.", "warning");
    else {
        $.ajax({
            url: "/BurialApplicationValidation/GetAccountDetails?acctnum=" + acctnum,
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
                    html += '    <div style="background-color:darkgrey;text-align:center;"><strong>DECEASED MEMBER-COSUMER\'S INFORMATION</strong ></div > ';
                    html += '    <div class="form-group col-sm-5" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtAcctName">Account Name:</label> ';
                    html += '        <input type="text" class="form-control" id="txtAcctName" readonly="readonly" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-6" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtAddress">Address:</label> ';
                    html += '        <input type="text" class="form-control" id="txtAddress" readonly="readonly" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '</div> ';
                    html += '<div class="row"> ';
                    html += '    <div class="form-group col-sm-4" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtMemberId">Member OR #:</label> ';
                    html += '        <input type="text" class="form-control" id="txtMemberORNo" readonly="readonly" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtORDate">OR DATE:</label> ';
                    html += '        <input type="date" class="form-control" id="txtORDate" readonly="readonly" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-4" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtMeterNo">Meter S/N:</label> ';
                    html += '        <input type="text" class="form-control" id="txtMeterNo" readonly="readonly" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '</div> ';
                    html += '<div class="row"> ';
                    html += '    <div class="form-group col-lg-2"> ';
                    html += '        <label for="dtpDateOfDeath">Date of Death<b style="color:red;">*</b>:</label> ';
                    html += '        <input type="date" class="form-control" id="dtpDateOfDeath" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-lg-10"> ';
                    html += '        <label for="txtCauseOfDeath">Cause of Death<b style="color:red;">*</b>:</label> ';
                    html += '        <input type="text" class="form-control" id="txtCauseOfDeath" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '</div> ';
                    html += '<br /> ';
                    html += '<div class="row"> ';
                    html += '    <div style="background-color:darkgrey;text-align:center;"><strong>CLAIMANT\'S INFORMATION</strong ></div > ';
                    html += '    <div class="form-group col-sm-3" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtClaimName">Claimant\'s Name<b style="color:red;">*</b>:</label> ';
                    html += '        <input type="text" class="form-control" id="txtClaimName" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-4" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtCAddress">Claimant\'s Address<b style="color:red;">*</b>:</label > ';
                    html += '        <input type="text" class="form-control" id="txtCAddress" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtRelation">Rel. to Deceased<b style="color:red;">*</b>:</label> ';
                    html += '        <input type="text" class="form-control" id="txtRelation" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '    <div class="form-group col-sm-2" style="padding:0;margin:7px;"> ';
                    html += '        <label for="txtContact">Telephone/Mobile No.<b style="color:red;">*</b>:</label> ';
                    html += '        <input type="text" class="form-control" id="txtContact" style="max-width:100%;" /> ';
                    html += '    </div> ';
                    html += '</div> ';
                    html += '<br /> ';
                    html += '<div class="row"> ';
                    html += '    <div class="col-lg-offset-2 col-lg-8"> ';
                    html += '        <table id="myTable1" class="table-responsive" border="1" style="max-width:100%;"> ';
                    html += '            <tbody> ';
                    html += '                <tr> ';
                    html += '                    <td style="width:15%;text-align:center;"><strong>Valid</strong></td> ';
                    html += '                    <td style="width:150%;padding-left:5px;padding-right:5px;"><strong>Findings</strong></td> ';
                    html += '                </tr> ';
                    html += '                <tr> ';
                    html += '                    <td style="width:15%;text-align:center;"> ';
                    html += '                        <img class="center" src="../../Content/images/questionMark.png" style="width:45px;height:45px;" id="qu1" /> ';
                    html += '                        <img class="center" src="../../Content/images/ajax-loader.gif" id="wait1" /> ';
                    html += '                        <img class="center" src="../../Content/images/checkMark.png" style="width:35px;height:35px;" id="check1" /> ';
                    html += '                        <img class="center" src="../../Content/images/wrongaMark.png" style="width:35px;height:35px;" id="wrong1" /> ';
                    html += '                    </td> ';
                    html += '                    <td style="width:150%;padding-left:5px;padding-right:5px;"> ';
                    html += '                                            No unsettled or outstanding obligation at the time of filling of application. ';
                    html += '                    </td> ';
                    html += '                </tr> ';
                    html += '                <tr> ';
                    html += '                    <td style="width:15%;text-align:center;"> ';
                    html += '                        <img class="center" src="../../Content/images/questionMark.png" style="width:45px;height:45px;" id="qu2" /> ';
                    html += '                        <img class="center" src="../../Content/images/ajax-loader.gif" id="wait2" /> ';
                    html += '                        <img class="center" src="../../Content/images/checkMark.png" style="width:35px;height:35px;" id="check2" /> ';
                    html += '                        <img class="center" src="../../Content/images/wrongaMark.png" style="width:35px;height:35px;" id="wrong2" /> ';
                    html += '                    </td> ';
                    html += '                    <td style="width:150%;padding-left:5px;padding-right:5px;"> ';
                    html += '                                            Has never been apprehended of electric pilferage by the Cooperative. ';
                    html += '                    </td> ';
                    html += '                </tr> ';
                    html += '                <tr> ';
                    html += '                    <td style="width:15%;text-align:center;"> ';
                    html += '                        <img class="center" src="../../Content/images/questionMark.png" style="width:45px;height:45px;" id="qu3" /> ';
                    html += '                        <img class="center" src="../../Content/images/ajax-loader.gif" id="wait3" /> ';
                    html += '                        <img class="center" src="../../Content/images/checkMark.png" style="width:35px;height:35px;" id="check3" /> ';
                    html += '                        <img class="center" src="../../Content/images/wrongaMark.png" style="width:35px;height:35px;" id="wrong3" /> ';
                    html += '                    </td> ';
                    html += '                    <td style="width:150%;padding-left:5px;padding-right:5px;"> ';
                    html += '                                            Average consumption not over 150KWh for the last (1) one year. ';
                    html += '                    </td> ';
                    html += '                </tr> ';
                    html += '                <tr> ';
                    html += '                    <td style="width:15%;text-align:center;"> ';
                    html += '                        <img class="center" src="../../Content/images/questionMark.png" style="width:45px;height:45px;" id="qu4" /> ';
                    html += '                        <img class="center" src="../../Content/images/ajax-loader.gif" id="wait4" /> ';
                    html += '                        <img class="center" src="../../Content/images/checkMark.png" style="width:35px;height:35px;" id="check4" /> ';
                    html += '                        <img class="center" src="../../Content/images/wrongaMark.png" style="width:35px;height:35px;" id="wrong4" /> ';
                    html += '                    </td> ';
                    html += '                    <td style="width:150%;padding-left:5px;padding-right:5px;"> ';
                    html += '                                            Account has not claimed burial. ';
                    html += '                    </td> ';
                    html += '                </tr> ';
                    html += '            </tbody> ';
                    html += '        </table> ';
                    html += '    </div> ';
                    html += '</div> ';
                    html += '<div class="row" id="divButtons"> ';
                    html += '    <div class="col-md-offset-4 col-md-4"> ';
                    html += '        <center> ';
                    html += '            <table> ';
                    html += '                <tr> ';
                    html += '                    <td style="padding:5px;"> ';
                    html += '                        <button class="btn btn-primary" id="btnPrint" onclick="printResult()"><i class="glyphicon glyphicon-print"></i> Save and Print</button> ';
                    html += '                    </td> ';
                    html += '                    <td style="padding:5px;"> ';
                    html += '                        <button class="btn btn-default" id="btnReset" onclick="resetPage()"><i class="glyphicon glyphicon-refresh"></i> Try Another Account</button> ';
                    html += '                    </td> ';
                    html += '                </tr> ';
                    html += '            </table> ';
                    html += '        </center> ';
                    html += '    </div> ';
                    html += '</div> ';

                    $('#divAcctDetails').append(html);

                    $('#txtAcctName').val(result.AccountName);
                    $('#txtAddress').val(result.Address);
                    $('#txtMemberORNo').val(result.MemberId);
                    if (result.ORDate != "")
                        $('#txtORDate').val(result.ORDate);
                    else
                        $('#txtORDate').val();
                    $('#txtMeterNo').val(result.MeterNo);
                    $('#txtAccountNo').attr('readonly', 'readonly');
                    $('#btnCheckAcct').hide();

                    $('#qu1').hide();
                    $('#check1').hide();
                    $('#wait1').show();
                    $('#wrong1').hide();

                    $('#qu2').hide();
                    $('#check2').hide();
                    $('#wait2').show();
                    $('#wrong2').hide();

                    $('#qu3').hide();
                    $('#check3').hide();
                    $('#wait3').show();
                    $('#wrong3').hide();

                    $('#qu4').hide();
                    $('#check4').hide();
                    $('#wait4').show();
                    $('#wrong4').hide();

                    verifyUnsettled();
                    verifyApprehended();
                    verifyConsumption();
                    verifyNotClaimed();

                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
    return false;
}

//function saveVerifyAcct() {

//    if ($('#dtpDateOfDeath').val() !== '' && $('#txtCauseOfDeath').val() !== ''
//        && $('#txtClaimName').val() !== '' && $('#txtCAddress').val() !== ''
//        && $('#txtRelation').val() !== '' && $('#txtContact').val()) {

//        //$('#qu1').hide();
//        //$('#check1').hide();
//        //$('#wait1').show();
//        //$('#wrong1').hide();

//        //$('#qu2').hide();
//        //$('#check2').hide();
//        //$('#wait2').show();
//        //$('#wrong2').hide();

//        //$('#qu3').hide();
//        //$('#check3').hide();
//        //$('#wait3').show();
//        //$('#wrong3').hide();

//        //$('#qu4').hide();
//        //$('#check4').hide();
//        //$('#wait4').show();
//        //$('#wrong4').hide();

//        //verifyUnsettled();
//        //verifyApprehended();
//        //verifyConsumption();
//        //verifyNotClaimed();

//        //$('#btnVerify').hide();

//        //saveResultAndApplication();
//    //} else {
//    //    swal("Error", "All fields with (*) asterisk are required.", "warning");
//    //}
//}

function verifyUnsettled() {
    var acctnum = $('#txtAccountNo').val();

    $.ajax({
        url: "/BurialApplicationValidation/NoUnsettledObligation?acctnum=" + acctnum,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#qu1').hide();
                $('#check1').show();
                $('#wait1').hide();
                $('#wrong1').hide();
            } else {
                $('#qu1').hide();
                $('#check1').hide();
                $('#wait1').hide();
                $('#wrong1').show();
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function verifyApprehended() {
    var acctnum = $('#txtAccountNo').val();

    $.ajax({
        url: "/BurialApplicationValidation/NoApprehended?acctnum=" + acctnum,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#qu2').hide();
                $('#check2').show();
                $('#wait2').hide();
                $('#wrong2').hide();
            } else {
                $('#qu2').hide();
                $('#check2').hide();
                $('#wait2').hide();
                $('#wrong2').show();
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function verifyConsumption() {
    var acctnum = $('#txtAccountNo').val();

    $.ajax({
        url: "/BurialApplicationValidation/InAvgConsumption?acctnum=" + acctnum,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#qu3').hide();
                $('#check3').show();
                $('#wait3').hide();
                $('#wrong3').hide();
            } else {
                $('#qu3').hide();
                $('#check3').hide();
                $('#wait3').hide();
                $('#wrong3').show();
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function verifyNotClaimed() {

    var acctnum = $('#txtAccountNo').val();

    $.ajax({
        url: "/BurialApplicationValidation/NotClaimed?acctnum=" + acctnum,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result == true) {
                $('#qu4').hide();
                $('#check4').show();
                $('#wait4').hide();
                $('#wrong4').hide();
            } else {
                $('#qu4').hide();
                $('#check4').hide();
                $('#wait4').hide();
                $('#wrong4').show();
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


function resetPage() {
    window.location = "/BurialApplicationValidation/Index";
}

function printResult() {

    if ($('#dtpDateOfDeath').val() !== '' && $('#txtCauseOfDeath').val() !== ''
        && $('#txtClaimName').val() !== '' && $('#txtCAddress').val() !== ''
        && $('#txtRelation').val() !== '' && $('#txtContact').val()) {

        saveResultAndApplication();

    }
    else {
        swal("Error", "All fields with (*) asterisk are required.", "warning");
    }
}

function saveResultAndApplication() {

    var acctnum = $('#txtAccountNo').val();

    //document.body.style.cursor = 'progress';
    //waitingDialog.show('Saving Results...');

    $.ajax({
        url: "/BurialApplicationValidation/SaveBurialResult?acctnum=" + acctnum,
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (result == true) {
                //document.body.style.cursor = 'default';
                //waitingDialog.hide();
                //Save Application Form
                saveApplicationForm();
            } else
                //document.body.style.cursor = 'default';
                //waitingDialog.hide();
                swal("Error", "Cannont save the result.", "warning");
        },
        error: function (errormessage) {
            //document.body.style.cursor = 'default';
            //waitingDialog.hide();
            alert(errormessage.responseText);
        }
    });

}

function saveApplicationForm() {

//document.body.style.cursor = 'progress';
//waitingDialog.show('Saving Application...');

var objdata = {
    Id: parseInt(0),
    BurialHeaderId: parseInt(0),
    MCDateOfDeath: $('#dtpDateOfDeath').val(),
    MCCauseOfDeath: $('#txtCauseOfDeath').val(),
    ClaimantName: $('#txtClaimName').val(),
    ClaimantAddress: $('#txtCAddress').val(),
    Relationship: $('#txtRelation').val(),
    ContactNum: $('#txtContact').val(),
    ScreenedBy:'',
    ScreenedByPos: '',
    RecommendedBy: '',
    RecommendedByPos: '',
    ApprovedBy: '',
    ApprovedByPos: ''
};

if (objdata != null) {
    $.ajax({
        type: "POST",
        url: "/BurialApplicationValidation/SaveBurialApplication/",
        contentType: 'application/json; charset=UTF-8',
        data: JSON.stringify(objdata),
        dataType: "json",
        success: function (result) {
            if (result == true) {
                //document.body.style.cursor = 'default';
                //waitingDialog.hide();
                swal("Success", "Data has been saved.", "success");
                //printing
                var parent = $('embed#bvrpdf').parent();
                var newElement = '<embed src="/BurialApplicationValidation/PrintBurialApplicationResult"  width="100%" height="800" type="application/pdf" id="bvrpdf">';

                $('embed#bvrpdf').remove();
                parent.append(newElement);

                $('#myRptModal').modal('show');

                $('#btnPrint').hide();

            } else
                //document.body.style.cursor = 'default';
                //waitingDialog.hide();
                swal("Error", "Can't save the data.", "warning");
        },
        error: function (errormessage) {
            //document.body.style.cursor = 'default';
            //waitingDialog.hide();
            alert(errormessage.responseText);
        }
    });
}
}


    var waitingDialog = waitingDialog || (function ($) {
        'use strict';

        // Creating modal dialog's DOM
        var $dialog = $(
            '<div class="modal fade bd-example-modal-sm" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
            '<div class="modal-dialog modal-sm">' +
            '<div class="modal-content">' +
            '<div class="modal-header"><h5 style="margin:0;"></h5></div>' +
            '<div class="modal-body">' +
            '<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
            '</div>' +
            '</div></div></div>');

        return {
            /**
             * Opens our dialog
             * @param message Custom message
             * @param options Custom options:
             * 				  options.dialogSize - bootstrap postfix for dialog size, e.g. "sm", "m";
             * 				  options.progressType - bootstrap postfix for progress bar type, e.g. "success", "warning".
             */
            show: function (message, options) {
                // Assigning defaults
                if (typeof options === 'undefined') {
                    options = {};
                }
                if (typeof message === 'undefined') {
                    message = 'Loading';
                }
                var settings = $.extend({
                    dialogSize: 'sm',
                    progressType: '',
                    onHide: null // This callback runs after the dialog was hidden
                }, options);

                // Configuring dialog
                $dialog.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + settings.dialogSize);
                $dialog.find('.progress-bar').attr('class', 'progress-bar');
                if (settings.progressType) {
                    $dialog.find('.progress-bar').addClass('progress-bar-' + settings.progressType);
                }
                $dialog.find('h5').text(message);
                // Adding callbacks
                if (typeof settings.onHide === 'function') {
                    $dialog.off('hidden.bs.modal').on('hidden.bs.modal', function (e) {
                        settings.onHide.call($dialog);
                    });
                }
                // Opening dialog
                $dialog.modal();
            },
            /**
             * Closes dialog
             */
            hide: function () {
                $dialog.modal('hide');
            }
        };

    })(jQuery);