function hidecontrols() {
    $('#lblTrxStatus').hide();
    $('#txtTrxStatus').hide();
}


async function txtORNumberKeyDown(evt) {
    if (evt.key === "Enter") {

        await evt.preventDefault();

        var ornum = $('#txtORNumber').val().trim();

        if (ornum != '') {
            //perform GET operation
            document.body.style.cursor = 'progress';
            $('#modalLoading').modal('show');
            document.getElementById("spintext").innerHTML = "LOADING...";

            await getPaymentByORNumber(ornum);

            document.body.style.cursor = 'default';
            $('#modalLoading').modal('hide');

            
        } else {
            swal("Missing", "OR Number is required.", "warning");
        }

        
    }

    if (evt.key === "Escape") {
        await evt.preventDefault();

        if (ornum != '') {
            //perform clear operation
            clearfields();
        } 
    }
}


function clearfields() {
    $('#txtORNumber').val("");
    $('#txtTrxStatus').val("");
    $('#txtTrxStatus').hide();
    $('#lblTrxStatus').hide();
    $('#txtOffice').val("");
    $('#txtConsumerId').val("");
    $('#txtConsType').val("");
    $('#txtConsStatus').val("");
    $('#txtPayee').val("");
    $('#txtTIN').val("");
    $('#txtAddress').val("");
    $('#txtCheckNo').val("");
    $('#txtBank').val("");
    $('#txtTrxDate').val("");
    $('#txtMode').val("");
    $('#txtAmountDue').val("0.00");
    $('#txtAmountTendered').val("0.00");
    $('#txtChange').val("0.00");
    $('#lblPostedBy').html("<i>POSTED BY:</i>");

    $('#tblTrxDetail tbody').empty();
    $('#tblCharge tbody').empty();
}

async function getPaymentByORNumber(ornumber) {
    $.ajax({
        url: "/TOR/GetPaymentByOrnumber?ornum=" + ornumber,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: await function (result) {

            clearfields();

            if (result.PaymentHeader.ORNumber == null) {

                swal("NOT EXIST", "OR Number is not exist.", "warning");
                return false;

            } else {
                
                if (result.PaymentDetails.length > 0) {
                    //HEADER BEGINS HERE
                    $('#txtORNumber').val(result.PaymentHeader.ORNumber);
                    $('#txtTrxStatus').val(result.PaymentHeader.TrxStatus);
                    if (result.PaymentHeader.TrxStatus != "") {
                        $('#txtTrxStatus').show();
                        $('#lblTrxStatus').show();
                    }
                    $('#txtOffice').val(result.PaymentHeader.Office);
                    $('#txtConsumerId').val(result.PaymentHeader.ConsumerId);
                    $('#txtConsType').val(result.PaymentHeader.ConsumerType);
                    $('#txtConsStatus').val(result.PaymentHeader.ConsumerStatus);
                    $('#txtPayee').val(result.PaymentHeader.Payee);
                    $('#txtTIN').val(result.PaymentHeader.TIN);
                    $('#txtAddress').val(result.PaymentHeader.Address);
                    $('#txtCheckNo').val(result.PaymentHeader.CheckNumber);
                    $('#txtBank').val(result.PaymentHeader.Bank);
                    $('#txtCheckName').val(result.PaymentHeader.CheckName);
                    $('#txtTrxDate').val(result.PaymentHeader.TrxDate);
                    $('#txtMode').val(result.PaymentHeader.ModeOfPayment);
                    $('#txtAmountDue').val(result.PaymentHeader.TotalAmount);
                    $('#txtAmountTendered').val(result.PaymentHeader.Tendered);
                    if (result.PaymentHeader.Change == "0")
                        $('#txtChange').val("0.00");
                    else
                        $('#txtChange').val(result.PaymentHeader.Change);
                    $('#lblPostedBy').html("<i>POSTED BY: " + result.PaymentHeader.EntryUser + " " + result.PaymentHeader.EntryDate + "</i>");
                    //HEADER ENDS HERE
                    //DETAILS BEGINS HERE
                    $('#tblBodyDetails').empty();
                    var html = '';
                    $.each(result.PaymentDetails, function (key, item) {
                        html += '<tr>';
                        html += '<td style="font-weight:bold;color:blue;">' + item.TrxId + '</td>';
                        html += '<td style="font-weight:bold;color:blue;">' + item.TrxDesc + '</td>';
                        html += '<td class="text-right" style="font-weight:bold;color:blue;">' + item.TrxAmount + '</td>';
                        html += '<td class="text-right" style="font-weight:bold;color:blue;">' + item.VAT + '</td>';
                        html += '<td class="text-right" style="font-weight:bold;color:blue;">' + item.Amount + '</td>';
                        html += '</tr>';
                    });
                    $('#tblBodyDetails').html(html);
                    //DETAILS ENDS HERE
                    //APPLIES BEGINS HERE
                    $('#myTblChargeBody').empty();
                    var htmlapp = '';
                    $.each(result.PaymentApplies, function (key, item) {
                        htmlapp += '<tr>';
                        if (item.BillingDate == "TOTAL AR") {
                            htmlapp += '<td class="text-right" colspan="3" style="font-weight:bold;color:red;">' + item.BillingDate + '</td>';

                            htmlapp += '<td class="text-right" style="font-weight:bold;color:red;">' + item.Amount + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;color:red;">' + item.VAT + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;color:red;">' + item.Surcharge + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;color:red;">' + item.Total + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;color:red;">' + item.SCDisc + '</td>';
                        } else {
                            htmlapp += '<td style="font-weight:bold;">' + item.BillingDate + '</td>';
                            htmlapp += '<td style="font-weight:bold;">' + item.Remarks + '</td>';
                            htmlapp += '<td style="font-weight:bold;">' + item.DueDate + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;">' + item.Amount + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;">' + item.VAT + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;">' + item.Surcharge + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;">' + item.Total + '</td>';
                            htmlapp += '<td class="text-right" style="font-weight:bold;">' + item.SCDisc + '</td>';
                        }
                        
                        htmlapp += '</tr>';
                    });

                    $('#myTblChargeBody').html(htmlapp);
                    //APPLIES ENDS HERE
                    //CHECK IF SYSTEM ERROR STARTS HERE
                    if (result.PaymentHeader.Address != "") {

                        $.ajax({
                            url: "/TOR/GetComparisonPaymentToMasterByORNumber?ornum=" + ornumber,
                            type: "GET",
                            contentType: "application/json;charset=UTF-8",
                            dataType: "json",
                            success: function (resultd) {
                                if (resultd.RealConsumerId != resultd.FakeConsumerId) {

                                    if ($('#txtUserIdLogged').val() != '001' && $('#txtUserIdLogged').val() != '0418'
                                        && $('#txtUserIdLogged').val() != '1001' && $('#txtUserIdLogged').val() != '1020'
                                        && $('#txtUserIdLogged').val() != '2141') {

                                        swal("SYSTEM ERROR", "Mismatch Payment Consumer ID and Master Consumer ID\n\n" +
                                            "Please contact your IT System Administrator to update this transaction.", "warning");

                                    } else {

                                        swal({
                                            title: "SYSTEM ERROR",
                                            text: "Mismatch Payment Consumer ID and Master Consumer ID\n\n" +
                                                "Do you want to correct Payment Consumer ID with " + resultd.RealConsumerId + "  to update this transaction?",
                                            type: "warning",
                                            showCancelButton: true,
                                            confirmButtonColor: '#DD6B55',
                                            confirmButtonText: 'Confirm',
                                            cancelButtonText: "No",
                                            closeOnConfirm: false,
                                            closeOnCancel: true
                                        },
                                            function (resp) {
                                                if (resp) {
                                                    $.ajax({
                                                        url: "/TOR/UpdatePaymentConsumerIdByORNumber?ornum=" + ornumber + "&newconsumerid=" + resultd.RealConsumerId,
                                                        type: "POST",
                                                        contentType: "application/json;charset=UTF-8",
                                                        dataType: "json",
                                                        success: function (response) {
                                                            if (response.data == "SUCCESS") {
                                                                $('#txtConsumerId').val(resultd.RealConsumerId);
                                                                //swal("\nSUCCESS", "Payment Consumer ID has been updated.", "success");

                                                                swal({
                                                                    title: "\nSUCCESS",
                                                                    text: "Payment Consumer ID has been updated.",
                                                                    type: "success",
                                                                    closeOnConfirm: true
                                                                },function () {
                                                                    clearfields();
                                                                    //$('#txtORNumber').focus();
                                                                    window.location = "/TOR/Index";
                                                                });

                                                            }
                                                            else {
                                                                swal("ERROR", response.data, error);
                                                            }
                                                        },
                                                        error: function (errormessage) {
                                                            alert(errormessage.responseText);
                                                        }
                                                    });

                                                }
                                            }
                                        );

                                    }

                                }
                            },
                            error: function (errormessage) {
                                alert(errormessage.responseText);
                            }
                        });

                    }
                    //CHECK IF SYSTEM ERROR ENDS HERE
                } else {
                    
                    swal("SYNTAX ERROR", "No details seen for this OR Number.", "warning");
                    return false;
                }
            }
        },
        error: await function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}