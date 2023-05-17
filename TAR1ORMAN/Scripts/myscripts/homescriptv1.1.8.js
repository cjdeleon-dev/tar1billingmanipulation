function hideunneccessaryoptions() {
    if ($('#txtCurrentRole').val() == "NONADMIN") {
        $('#oradddiv').hide();
        $('#orsubdiv').hide();
        $('#orprefdiv').hide();
        $('#lstburial').show();
        $('#lstsc').show();
        $('#writeoff').hide();
        $('#empco').hide();
        $('#mnudbsettings').hide();
        if ($('#txtUserIdLogged').val() == '0340' || $('#txtUserIdLogged').val() == '0304' || $('#txtUserIdLogged').val() == '8001'
            || $('#txtUserIdLogged').val() == '0907' || $('#txtUserIdLogged').val() == '2323' || $('#txtUserIdLogged').val() == '0810'
            || $('#txtUserIdLogged').val() == '0313' || $('#txtUserIdLogged').val() == '1521' || $('#txtUserIdLogged').val() == '0335'
            || $('#txtUserIdLogged').val() == '4455' || $('#txtUserIdLogged').val() == '1415' || $('#txtUserIdLogged').val() == '9050'
            || $('#txtUserIdLogged').val() == '9051' || $('#txtUserIdLogged').val() == '9052' || $('#txtUserIdLogged').val() == '9053'
            || $('#txtUserIdLogged').val() == '9054' || $('#txtUserIdLogged').val() == '9055' || $('#txtUserIdLogged').val() == '9056'
            || $('#txtUserIdLogged').val() == '9057' || $('#txtUserIdLogged').val() == '9058' || $('#txtUserIdLogged').val() == '9059'
            || $('#txtUserIdLogged').val() == '0825' || $('#txtUserIdLogged').val() == '0804') {
            $('#soa').show();
            $('#tor').show();
        }
        else {
            $('#soa').hide();
            $('#tor').hide();
        }
            

    } else {
        switch ($('#txtUserIdLogged').val()) {
            case "0000":
            case "001":
            case "0418":
            case "1001":
            case "1020":
            case "1235":
            case "2141":
            case "0825":
                $('#oradddiv').show();
                $('#orsubdiv').show();
                $('#orprefdiv').show();
                $('#lstburial').show();
                $('#lstsc').show();
                $('#writeoff').show();
                $('#empco').show();
                $('#mnudbsettings').show();
                $('#soa').show();
                $('#tor').show();
                break;
            default:
                $('#oradddiv').hide();
                $('#orsubdiv').hide();
                $('#orprefdiv').hide();
                $('#lstburial').show();
                $('#lstsc').show();
                $('#ecpay').show();
                $('#empco').show();
                $('#mnudbsettings').hide();
                $('#soa').show();
                $('#tor').show();
                break;
        }
    }
    
}