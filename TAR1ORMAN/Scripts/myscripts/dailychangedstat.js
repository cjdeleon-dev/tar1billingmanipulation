function iniatializePage() {
    loadcboStatus();
    loadcurrentDate();
}

function loadcboStatus() {
    $('#cboStatus').append("<option value=0>Select Status</option>");
    $('#cboStatus').append("<option value=1>Active</option>");
    $('#cboStatus').append("<option value=2>Disconnected</option>");
}

function loadcurrentDate() {
    var now = new Date();

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    $('#dtpDate').val(today);
}

function processdata() {

    let datestat = $('#cboStatus').val() + '_' + $('#dtpDate').val();

    var parent = $('embed#csrpdf').parent();
    var newElement = '<embed src="/DailyChangedStatus/ViewChangedStatusReport?rptParams=' + datestat + '"  width="100%" height="800" type="application/pdf" id="csrpdf">';

    $('embed#csrpdf').remove();
    parent.append(newElement);

    $('#myRptModal').modal('show');
}