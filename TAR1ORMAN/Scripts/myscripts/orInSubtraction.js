function isNumber(evt) {
    var ch = String.fromCharCode(evt.which);
    if (!(/[0-9]/.test(ch))) {
        evt.preventDefault();
    }
}

function previewdata() {

    //validation
    if ($('#txtOrFrom').val().trim() == "" || $('#txtOrTo').val().trim() == "" || $('#txtSubtrahend').val().trim() == "") {
        alert('Invalid values.');
        return;
    }

    var ORObj = {
        Addend: 0,
        ORNumberFrom: $('#txtOrFrom').val(),
        ORNumberTo: $('#txtOrTo').val(),
        Subtrahend: $('#txtSubtrahend').val()
    };

    //show waiting gif
    $("#waiting").show();
    //remove label tag
    $('.execmsg').find("label").remove();

    //disabled buttons and textboxes
    $('#btnPreview').addClass("disabled");
    $('#btnProcess').addClass("disabled");
    $('#btnBack').addClass("disabled");
    $('#txtOrFrom').prop('disabled', true);
    $('#txtOrTo').prop('disabled', true);
    $('#txtSubtrahend').prop('disabled', true);

    $.ajax({
        data: JSON.stringify(ORObj),
        url: "/Processing/PreviewORRangeInSubtraction/",
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
            $('#btnPreview').removeClass("disabled");
            if (rowctr > 0) {
                $('#btnProcess').removeClass('disabled');
            }
            $('#btnBack').removeClass("disabled");
            $('#txtOrFrom').prop('disabled', false);
            $('#txtOrTo').prop('disabled', false);
            $('#txtSubtrahend').prop('disabled', false);

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
            $('#txtSubtrahend').prop('disabled', false);
            alert(errormessage.responseText);
        },
    });
}

//function prevprocessdata() {
//    //validation
//    if ($('#txtOrFrom').val().trim() == "" || $('#txtOrTo').val().trim() == "" || $('#txtSubtrahend').val().trim() == "") {
//        alert('Invalid values.');
//        return;
//    }

//    var ORObj = {
//        ORNumberFrom: $('#txtOrFrom').val(),
//        ORNumberTo: $('#txtOrTo').val(),
//        Subtrahend: $('#txtSubtrahend').val()
//    };

//    //show waiting gif
//    $("#waiting").show();
//    //remove label tag
//    $('.execmsg').find("label").remove();

//    //disabled buttons and textboxes
//    $('#btnPreview').addClass("disabled");
//    $('#btnProcess').addClass("disabled");
//    $('#btnBack').addClass("disabled");
//    $('#txtOrFrom').prop('disabled', true);
//    $('#txtOrTo').prop('disabled', true);
//    $('#txtSubtrahend').prop('disabled', true);


//    $.ajax({
//        data: JSON.stringify(ORObj),
//        url: "/Processing/PreviewORRangeInSubtraction/",
//        type: "POST",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            var html = '';
//            var rowctr = 0;
//            $.each(result, function (key, item) {
//                html += '<tr style="background-color: white;">';
//                html += '<td>' + item.ORNumber + '</td>';
//                html += '<td>' + item.NewORNumber + '</td>';
//                html += '<td>' + item.AccountNumber + '</td>';
//                html += '<td>' + item.Payee + '</td>';
//                html += '<td>' + item.TransactionDate + '</td>';
//                html += '</tr>';
//                rowctr += 1;
//            });
//            $('.tbody').html(html);


//            var msg = "Executed successfully. " + rowctr + " row(s) updated.";

//            //add label tag and set the forecolor to red.
//            $('.execmsg').html('<label style="color: red;">' + msg + '</label>');

//            if (confirm("Click Ok to continue processing your request.")) {
//                processdata(ORObj);
//            }

//            //hide waiting gif
//            $("#waiting").hide();

//            //enable buttons and textboxes
//            $('#btnPreview').removeClass("disabled");
//            $('#btnProcess').removeClass("disabled");
//            $('#btnBack').removeClass("disabled");
//            $('#txtOrFrom').prop('disabled', false);
//            $('#txtOrTo').prop('disabled', false);
//            $('#txtSubtrahend').prop('disabled', false);

//        },
//        error: function (errormessage) {
//            $("#waiting").hide();
//            alert(errormessage.responseText);
//        },
//    });
//}

function processdata() {

    //validation
    if ($('#txtOrFrom').val().trim() == "" || $('#txtOrTo').val().trim() == "" || $('#txtSubtrahend').val().trim() == "") {
        alert('Invalid values.');
        return;
    }

    var ORObj = {
        ORNumberFrom: $('#txtOrFrom').val(),
        ORNumberTo: $('#txtOrTo').val(),
        Subtrahend: $('#txtSubtrahend').val()
    };

    //show waiting gif
    $("#waiting").show();
    //remove label tag
    $('.execmsg').find("label").remove();

    //disabled buttons and textboxes
    $('#btnPreview').addClass("disabled");
    $('#btnProcess').addClass("disabled");
    $('#btnBack').addClass("disabled");
    $('#txtOrFrom').prop('disabled', true);
    $('#txtOrTo').prop('disabled', true);
    $('#txtSubtrahend').prop('disabled', true);

    $.ajax({
        data: JSON.stringify(ORObj),
        url: "/Processing/ProcessORRangeInSubtraction/",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            //get process message
            var msg = result.errMessage;
            var issuccess = result.isSuccess;

            //add label tag and set the forecolor to red.
            $('.execmsg').html('<label style="color: red;">' + msg + '</label>');

            //hide waiting gif
            $("#waiting").hide();

            //enable buttons and textboxes
            //enable buttons and textboxes
            if (issuccess) {
                $('#btnProcess').addClass("disabled");
            } else {
                $('#btnProcess').removeClass("disabled");
            }
            $('#btnPreview').removeClass("disabled");
            $('#btnBack').removeClass("disabled");
            $('#txtOrFrom').prop('disabled', false);
            $('#txtOrTo').prop('disabled', false);
            $('#txtSubtrahend').prop('disabled', false);
         
        },
        error: function (errormessage) {
            $("#waiting").hide();
            alert(errormessage.responseText);
        },
    });
}