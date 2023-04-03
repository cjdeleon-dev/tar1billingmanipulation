function processdata() {

    let dtFr = $('#dtpDateFr').val();
    let dtTo = $('#dtpDateTo').val();
    let ofc = $('#cboOffice').val();

    console.log(dtFr);
    console.log(dtTo);
    console.log(ofc);

    var parent = $('embed#torpdf').parent();
    var newElement = '<embed src="/TORtoPDF/TORReportView?rptParams=' + dtFr + '_' + dtTo + '_' + ofc + '"  width="100%" height="800" type="application/pdf" id="torpdf">';

    $('embed#torpdf').remove();
    parent.append(newElement);

    $('#myRptModal').modal('show');
}

function loadPage() {
    loadOffices();
}

function loadOffices() {
    $.ajax({
        url: "/TORtoPDF/GetAllOffices/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboOffice').empty();
            //$('#cboChangeTo').val(0);
            $('#cboOffice').append("<option value=0>Select Office</option>");
            for (var i = 0; i < result.length; i++) {
                var office = result[i].Office;
                var opt = new Option(office, result[i].Id);
                $('#cboOffice').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}
