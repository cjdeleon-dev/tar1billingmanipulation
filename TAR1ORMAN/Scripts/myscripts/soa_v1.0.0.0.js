function processdata() {

    let route = $('#cboRoute').val();
    let bp = $('#cboBPeriod').val();

    if (route != "0" && bp != "0") {
        var parent = $('embed#soapdf').parent();
        var newElement = '<embed src="/SOA/SOAReportView?rptParams=' + route + '_' + bp + '"  width="100%" height="800" type="application/pdf" id="soapdf">';

        $('embed#soapdf').remove();
        parent.append(newElement);

        $('#myRptModal').modal('show');
    }

    else {
        alert("Please select a valid Route and Billing Period.");
    }
    
}

function getallroutes() {
    $.ajax({
        url: "/SOA/getAllRoutes/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboRoute').empty();
            $('#cboRoute').val(0);
            $('#cboRoute').append("<option value=0>Select Route</option>");
            for (var i = 0; i < result.length; i++) {
                var Desc = result[i].Route;
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

function getallbillperiods() {
    $.ajax({
        url: "/SOA/getAllBillPeriods/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboBPeriod').empty();
            $('#cboBPeriod').val(0);
            $('#cboBPeriod').append("<option value=0>Select Bill Period</option>");
            for (var i = 0; i < result.length; i++) {
                /*var Desc = result[i].Description;*/
                /*var opt = new Option(Desc, result[i].Id);*/
                $('#cboBPeriod').append($("<option></option>").val(result[i].Id).html(result[i].BillPeriod));
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function loadPage() {
    getallroutes();
    getallbillperiods();
}