function verify4PsDetails() {
    let vlname = $('#txtlname').val();
    let vfname = $('#txtfname').val();
    let vmname = $('#txtmname').val();

    if (vlname == '' && vfname == '' && vmname == '') {
        swal("Invalid", "No parameters to be verified.", "warning");
        return false;
    }

    let vname = vlname + '~' + vfname + '~' + vmname;

    $.ajax({
        url: "/FourPsApplication/VerifyFourPsDetail?name=" + vname,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            console.log(result);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}