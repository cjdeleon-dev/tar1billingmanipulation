

function selectOnlyThis(id) {
    if (id == "chkAdd") {
        if ($("#chkAdd").prop('checked')) {
            //uncheck
            $("#chkSub").prop('checked', false);
            $('#txtAddValue').prop('disabled', false);
            $('#txtSubValue').prop('disabled', true);
            $('#txtSubValue').val("");
        } else {
            $('#txtAddValue').prop('disabled', true);
            $('#txtAddValue').val("");
        }
    } else {
        if ($("#chkSub").prop('checked')) {
            //uncheck
            $("#chkAdd").prop('checked', false);
            $('#txtAddValue').prop('disabled', true);
            $('#txtSubValue').prop('disabled', false);
            $('#txtAddValue').val("");
        } else {
            $('#txtSubValue').prop('disabled', true);
            $('#txtSubValue').val("");
        }
    }
}

function previewdata() {

    //validation
    if ($('#txtOrFrom').val().trim() == "" || $('#txtOrTo').val().trim() == "" || $('#txtPrefix').val().trim() == "") {
        alert('Invalid values.');
        return;
    }

    //check if what operator to be used.
    var optr = 0;
    var optrvalue = 0;

    if (parseInt($('#txtAddValue').val().trim()) > 0 && $('#txtSubValue').val().trim() == "") {
        optr = 1;
        optrvalue = parseInt($('#txtAddValue').val().trim());
    } else if ($('#txtAddValue').val().trim() == "" && parseInt($('#txtSubValue').val().trim()) > 0) {
        optr = 2;
        optrvalue = parseInt($('#txtSubValue').val().trim())
    }

    var ORObj = {
        ORNumberFrom: $('#txtOrFrom').val(),
        ORNumberTo: $('#txtOrTo').val(),
        NewPrefix: $('#txtPrefix').val(),
        UsedOperator: parseInt(optr),
        Addend: optr == 1 ? optrvalue : 0,
        Subtrahend: optr == 2 ? optrvalue : 0
    };

    //show waiting gif
    $("#waiting").show();
    //remove label tag
    $('.execmsg').find("label").remove();

    //disabled buttons and textboxes
    $('#btnPreview').addClass("disabled");
    $('#btnProcess').addClass('disabled');
    $('#btnBack').addClass("disabled");
    $('#txtOrFrom').prop('disabled', true);
    $('#txtOrTo').prop('disabled', true);
    $('#txtPrefix').prop('disabled', true);

    $.ajax({
        data: JSON.stringify(ORObj),
        url: "/Processing/PreviewNewPrefixORRange/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            var rowctr = 0;
            $.each(result, function (key, item) {
                html += '<tr style="background-color: white;">';
                html += '<td>' + item.ORNumber + '</td>';
                html += '<td>' + item.NewORNumber + '</td>';
                html += '<td>' + item.AccountNumber + '</td>';
                html += '<td>' + item.Payee + '</td>';
                html += '<td>' + item.TransactionDate + '</td>';
                html += '</tr>';
                rowctr += 1;
            });
            $('.tbody').html(html);

            //hide waiting gif
            $("#waiting").hide();

            //enable buttons and textboxes
            $('#btnPreview').removeClass('disabled');
            if (rowctr > 0) {
                $('#btnProcess').removeClass('disabled');
            }
            $('#btnBack').removeClass('disabled');
            $('#txtOrFrom').prop('disabled', false);
            $('#txtOrTo').prop('disabled', false);
            $('#txtPrefix').prop('disabled', false);

            var msg = "Executed successfully. " + rowctr + " row(s) returned.";

            //add label tag and set the forecolor to red.
            $('.execmsg').html('<label style="color: red;">' + msg + '</label>');

        },
        error: function (errormessage) {
            $("#waiting").hide();
            //enable buttons and textboxes
            $('#btnPreview').removeClass('disabled');
            $('#btnProcess').removeClass('disabled');

            $('#btnBack').removeClass('disabled');
            $('#txtOrFrom').prop('disabled', false);
            $('#txtOrTo').prop('disabled', false);
            $('#txtPrefix').prop('disabled', false);
            alert(errormessage.responseText);
        },
    });
}

function processdata() {
    //validation
    if ($('#txtOrFrom').val().trim() == "" || $('#txtOrTo').val().trim() == "" || $('#txtPrefix').val().trim() == "") {
        alert('Invalid values.');
        return;
    }

    //check if what operator to be used.
    var optr = 0;
    var optrvalue = 0;

    if (parseInt($('#txtAddValue').val().trim()) > 0 && $('#txtSubValue').val().trim() == "") {
        optr = 1;
        optrvalue = parseInt($('#txtAddValue').val().trim());
    } else if ($('#txtAddValue').val().trim() == "" && parseInt($('#txtSubValue').val().trim()) > 0) {
        optr = 2;
        optrvalue = parseInt($('#txtSubValue').val().trim())
    }

    var ORObj = {
        ORNumberFrom: $('#txtOrFrom').val(),
        ORNumberTo: $('#txtOrTo').val(),
        NewPrefix: $('#txtPrefix').val(),
        UsedOperator: parseInt(optr),
        Addend: optr == 1 ? optrvalue : 0,
        Subtrahend: optr == 2 ? optrvalue : 0
    };

    //show waiting gif
    $("#waiting").show();
    //remove label tag
    $('.execmsg').find("label").remove();

    //disabled buttons and textboxes
    $('#btnPreview').addClass("disabled");
    $('#btnProcess').addClass('disabled');
    $('#btnBack').addClass("disabled");
    $('#txtOrFrom').prop('disabled', true);
    $('#txtOrTo').prop('disabled', true);
    $('#txtPrefix').prop('disabled', true);

    $.ajax({
        data: JSON.stringify(ORObj),
        url: "/Processing/ProcessNewPrefixORRange/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            var msg = result.errMessage;

            //hide waiting gif
            $("#waiting").hide();

            //enable buttons and textboxes
            if (result.isSuccess) {
                $('#btnProcess').addClass('disabled');
            } else {
                $('#btnProcess').removeClass('disabled');
            }
            $('#btnPreview').removeClass('disabled');
            $('#btnBack').removeClass('disabled');
            $('#txtOrFrom').prop('disabled', false);
            $('#txtOrTo').prop('disabled', false);
            $('#txtPrefix').prop('disabled', false);

            
 
            //add label tag and set the forecolor to red.
            $('.execmsg').html('<label style="color: red;">' + msg + '</label>');

        },
        error: function (errormessage) {
            $("#waiting").hide();
            //enable buttons and textboxes
            $('#btnPreview').removeClass('disabled');
            $('#btnProcess').removeClass('disabled');

            $('#btnBack').removeClass('disabled');
            $('#txtOrFrom').prop('disabled', false);
            $('#txtOrTo').prop('disabled', false);
            $('#txtPrefix').prop('disabled', false);
            alert(errormessage.responseText);
        },
    });
}