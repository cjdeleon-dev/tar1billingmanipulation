﻿function loadPage() {
    $('#btnReset').hide();
}

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
            $('#divResult').empty();
            if (result.data.length > 0) {

                var html = '';
                html += '<div class="col-lg-12" style="max-height:300px;overflow: scroll;position: relative;"> ';
                html += '<table class="table table-bordered" id="tblQualified"> ';
                html += '   <thead> ';
                html += '       <tr> ';
                html += '           <th style="text-align:center;">ENTRY ID</th> ';
                html += '           <th style="text-align:center;">HOUSE HOLD ID</th> ';
                html += '           <th style="text-align:center;">NAME</th> ';
                html += '           <th style="text-align:center;">ADDRESS</th> ';
                html += '           <th style="text-align:center;">GENDER</th> ';
                html += '           <th style="text-align:center;">BIRTHDAY</th> ';
                html += '           <th></th> ';
                html += '       </tr> ';
                html += '   </thead> ';
                html += '   <tbody id="tbodyQualified"></tbody> ';
                html += '</table ></div >';

                $('#divResult').append(html);
                appendToTableBody(result.data);
            }
            else {
               
                var html = '';
                html += '<div class="col-lg-offset-3 col-lg-6 text-center">';
                html += '   <h4 class="text-capitalized">No record(s) found.</h4>'
                html += '   <button class="btn btn-danger" id="btnQMEApp" onclick="showQMEApp()">PROCEED TO QME APPLICATION</button>';
                html += '</div>'

                $('#divResult').append(html);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function appendToTableBody(data) {
    var html = '';
    $.each(data, function (key, item) {
        html += '<tr class="" id="' + item.EntryId + '">';
        html += '<td id="' + item.EntryId + '" class="text-center">' + item.EntryId + '</td>';
        html += '<td id="' + item.HH_Id + '" class="text-center">' + item.HH_Id + '</td>';
        html += '<td>' + item.Name + '</td>';
        html += '<td>' + item.Address + '</td>';
        html += '<td>' + item.Gender + '</td>';
        html += '<td>' + item.Birthday + '</td>';
        html += '<td><input type="button" class="btn btn-danger" value="APPLY NOW >>" onclick="showFourPsApp('+ item.EntryId +')" /></td>';
        html += '</tr>';
    });
    $('#tbodyQualified').html(html);
}

function showQMEApp() {
    alert("QME Application");
}


function showFourPsApp(entryId) {
    $('#btnVerify').hide();
    $('#btnReset').show();

    $('#divResult').empty();
    $('#txtlname').attr('disabled', true);
    $('#txtfname').attr('disabled', true);
    $('#txtmname').attr('disabled', true);

    appendToFPSdiv();

    //Four Ps Detail By EntryId
    $.ajax({
        url: "/FourPsApplication/GetFourPsDetailByEntryId?entryId=" + entryId,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            console.log(result);
            $.each(result, function (key, item) {
                $('#txtsurname').val(item.Surname);
                $('#txtfirstname').val(item.Givenname);
                $('#txtmiddlename').val(item.Middlename);
                $('#txtbarangay').val(item.Brgyname);
                $('#txtcitymun').val(item.Cityname);
                $('#txtprovince').val(item.Provname);
                $('#txtregion').val("REGION III");
                $('#dtpbirthdate').val(item.Birthday);
                $('#txthouseholdno').val(item.HH_Id);
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function appendToFPSdiv()
{
    let html = '';

    $('#frmFPS').empty();

    html += '<div class="row"> ';
    html += '    <div class="col-lg-offset-4 col-lg-2 text-center"> ';
    html += '        <input type="text" class="form-control" id="txtaccountno" placeholder="Enter account number" /> ';
    html += '    </div> ';
    html += '    <div class="col-lg-offset-2"> ';
    html += '        <button class="btn btn-warning" id="btnsetacctno" onclick="setAcctNo()"><i class="glyphicon glyphicon-star"></i> Set Account</button> ';
    html += '        <button class="btn btn-info" id="btnsrcacctno" onclick="showSearchAcct()"><i class="glyphicon glyphicon-eye-open"></i> Search Account</button> ';
    html += '    </div> ';
    html += '</div><br /> ';
    html += '<div class="row"> ';
    html += '    <div class="text-center bg bg-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>Name of Applicant</strong></span></div> ';
    html += '    <div class="col-lg-3"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Surname (Apilyedo)</span> ';
    html += '            <input class="form-control" type="text" id="txtsurname" placeholder="Enter surname" /> ';
    html += '        </div> ';
    html += '    </div> '; 
    html += '    <div class="col-lg-3"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>First Name (Pangalan)</span> ';
    html += '            <input class="form-control" type="text" id="txtfirstname" placeholder="Enter first name" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-3"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Middle Name (Gitnang Pangalan)</span> ';
    html += '            <input class="form-control" type="text" id="txtmiddlename" placeholder="Enter middle name" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-3"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Maiden Name (if applicable)</span> ';
    html += '            <input class="form-control" type="text" id="txtmaidenname" placeholder="Enter maiden name" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '</div> ';
    html += '<div class="row"> ';
    html += '    <div class="text-center bg bg-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>address</strong></span></div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>House No./Zone/Purok/Sitio</span> ';
    html += '            <input class="form-control" type="text" id="txthouseno" placeholder="Enter House No./Zone/Purok/Sitio" style="max-width:100%;" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Street</span> ';
    html += '            <input class="form-control" type="text" id="txtstreet" placeholder="Enter Street" style="max-width:100%;" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Barangay</span> ';
    html += '            <input class="form-control" type="text" id="txtbarangay" placeholder="Enter barangay" style="max-width:100%;" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '</div> ';
    html += '<div class="row"> ';
    html += '    <div class="col-lg-3"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>City/Municipality</span> ';
    html += '            <input class="form-control" type="text" id="txtcitymun" placeholder="Enter City/Municipality" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-3"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Province</span> ';
    html += '            <input class="form-control" type="text" id="txtprovince" placeholder="Enter Province" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-3"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Region</span> ';
    html += '            <input class="form-control" type="text" id="txtregion" placeholder="Enter Region" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-3"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Postal</span> ';
    html += '            <input class="form-control" type="text" id="txtpostal" placeholder="Enter Postal" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '</div> ';
    html += '<div class="row"> ';
    html += '    <div class="text-center bg bg-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>date of birth / marital status / contact</strong></span></div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Date of Birth</span> ';
    html += '            <input class="form-control" type="date" id="dtpbirthdate" style="max-width:100%;" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Marital Status</span> ';
    html += '            <select class="form-control" style="max-width:100%;"> ';
    html += '                <option disabled selected>Select Marital Status</option> ';
    html += '                <option value="single" id="optSngl">Single</option> ';
    html += '                <option value="married" id="optMrrd">Married</option> ';
    html += '                <option value="widowed" id="optWdwd">Widowed</option> ';
    html += '                <option value="divorced" id=optDvrcd>Divorced</option> ';
    html += '                <option value="separated" id="optSprtd">Separated</option> ';
    html += '            </select> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Contact Number</span> ';
    html += '            <div class="input-group input-group-multi"> ';
    html += '                <div class="input-group-addon">+63</div> ';
    html += '                <div class="col-xs-12"><input type="text" class="form-control" style="max-width:100%;" placeholder="9999999999" id="txtcontactno" maxlength="10" /></div> ';
    html += '            </div> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '</div> ';
    html += '<div class="row" style="padding-bottom:15px;"> ';
    html += '    <div class="text-center bg bg-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>ownership</strong></span></div> ';
    html += '    <br /> ';
    html += '    <div class="form-group"> ';
    html += '        <div class="col-lg-2"> ';
    html += '            <div class="text-center" style="padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:35px;border-radius:4px;"> ';
    html += '                <input type="radio" id="rbowned" name="ownership" style="vertical-align:middle; margin:0px;" onclick="rbothersOnChange()" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">owned</span> ';
    html += '            </div> ';
    html += '        </div> ';
    html += '        <div class="col-lg-2"> ';
    html += '            <div class="text-center" style="padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:35px;border-radius:4px;"> ';
    html += '                <input type="radio" id="rbrented" name="ownership" style="vertical-align:middle; margin:0px;" onclick="rbothersOnChange()" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">rented</span> ';
    html += '            </div> ';
    html += '        </div> ';
    html += '        <div class="col-lg-8"> ';
    html += '            <div class="input-group input-group-multi"> ';
    html += '                <div class="input-group-addon" style="background-color:transparent !important;"> ';
    html += '                    <input type="radio" id="rbothers" name="ownership" style="vertical-align:middle; margin:0px;" onclick="rbothersOnChange()" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">others</span> ';
    html += '                </div> ';
    html += '                <div class="col-xs-12"><input type="text" class="form-control" style="max-width:100%;" id="txtothers" placeholder="if others, please specify" disabled /></div> ';
    html += '            </div> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '</div> ';
    html += '<div class="row"> ';
    html += '    <div class="text-center bg bg-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>4ps household number (from dswd)</strong></span></div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>4Ps Household No.</span> ';
    html += '            <input class="form-control" type="text" id="txthouseholdno" placeholder="Enter 4Ps Household No." style="max-width:100%;" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>Valid ID</span> ';
    html += '            <input class="form-control" type="text" id="txtvalidid" placeholder="Enter Valid ID" style="max-width:100%;" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="form-group"> ';
    html += '            <span>ID No.</span> ';
    html += '            <input class="form-control" type="text" id="txtvalididno" placeholder="Enter ID No." style="max-width:100%;" /> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '</div> ';
    html += '<div class="row" style="padding-bottom:15px;"> ';
    html += '    <div class="text-center bg bg-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>documentary requirements checklist / other supporting documents</strong></span></div> ';
    html += '    <br /> ';
    html += '    <div class="col-lg-6"> ';
    html += '        <div class="text-left" style="padding-left:10px;padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:35px;border-radius:4px;"> ';
    html += '            <input type="checkbox" id="chkduly" style="vertical-align:middle; margin:0px;" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">Duly accomplished application form</span> ';
    html += '        </div> ';
    html += '        <div class="text-left" style="padding-left:10px;padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:35px;border-radius:4px;"> ';
    html += '            <input type="checkbox" id="chkeb" style="vertical-align:middle; margin:0px;" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">Most-recent electricity bill for the service being applied for</span> ';
    html += '        </div> ';
    html += '        <div class="text-left" style="padding-left:10px;padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:60px;border-radius:4px;"> ';
    html += '            <input type="checkbox" id="chkvalid" style="vertical-align:middle; margin:0px;" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">Valid government-issued ID containing the signature and &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;address of the consumer</span> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-6"> ';
    html += '        <div class="text-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>If electric service not registered under the name of applicant:</strong></span></div> ';
    html += '        <div class="text-left" style="padding-left:10px;padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:35px;border-radius:4px;"> ';
    html += '            <input type="checkbox" id="chkproof" style="vertical-align:middle; margin:0px;" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">Proof of Residence</span> ';
    html += '        </div> ';
    html += '        <div class="text-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>If application filed through a representative:</strong></span></div> ';
    html += '        <div class="text-left" style="padding-left:10px;padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:35px;border-radius:4px;"> ';
    html += '            <input type="checkbox" id="chkauth" style="vertical-align:middle; margin:0px;" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">letter of authority</span> ';
    html += '        </div> ';
    html += '        <div class="text-left" style="padding-left:10px;padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:60px;border-radius:4px;"> ';
    html += '            <input type="checkbox" id="chkvalidrep" style="vertical-align:middle; margin:0px;" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">Valid government-issued ID (with signature) of the &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;representative</span> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '</div> ';
    html += '<div class="row" style="padding-bottom:15px;"> ';
    html += '    <div class="text-center bg bg-info" style="vertical-align:middle;"><span class="text-uppercase"><strong>evaluation</strong></span></div> ';
    html += '    <br /> ';
    html += '    <div class="col-lg-2"> ';
    html += '        <div class="text-center" style="padding-top:5px;border-style:solid;border-color:#dcdcdc;border-width:1px;height:35px;border-radius:4px;"> ';
    html += '            <input type="radio" id="rbapproved" name="evaluation" style="vertical-align:middle; margin:0px;" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">approved</span> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-6"> ';
    html += '        <div class="input-group input-group-multi"> ';
    html += '            <div class="input-group-addon" style="background-color:transparent !important;"> ';
    html += '                <input type="radio" id="rbdisapproved" name="evaluation" style="vertical-align:middle; margin:0px;" /><span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">disapproved</span> ';
    html += '            </div> ';
    html += '            <div class="col-xs-12"><input type="text" class="form-control" style="max-width:100%;" id="txtdisapprovereason" placeholder="Reasons for disapproval" disabled /></div> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '    <div class="col-lg-4"> ';
    html += '        <div class="input-group input-group-multi"> ';
    html += '            <div class="input-group-addon"> ';
    html += '                <span class="text-uppercase" style="vertical-align:middle;margin:0px;padding-left:5px;">DSWD validity period</span> ';
    html += '            </div> ';
    html += '            <div class="col-xs-10"><input type="date" class="form-control" style="max-width:100%;" id="dtpvalidity" /></div> ';
    html += '        </div> ';
    html += '    </div> ';
    html += '</div> ';
    html += '<div class="row"> ';
    html += '    <div class="col-lg-offset-4 col-lg-4 text-center"> ';
    html += '         <button class="btn btn-primary" id="btnSave" onclick="savepreview()"><i class="glyphicon glyphicon-save"></i> Save and Preview</button>';  
    html += '    </div> ';
    html += '</div>'

    $('#frmFPS').html(html);
}

function reloadPage() {
    window.location = "/FourPsApplication/FourPsVerify";
}

function showSearchAcct() {
    $('#modalSearchAcct').modal("show");
}

function displayAccounts() {
    let srcacctno = $('#txtsrcacctno').val();
    let srcacctname = $('#txtsrcacctname').val();
    let srcaddress = $('#txtsrcaddress').val();

    if (srcacctno == '' && srcacctname == '' && srcaddress == '') {
        swal("Invalid", "Please enter valid parameters.", "warning");
        return false;
    }

    let srcparam = srcacctno + '~' + srcacctname + '~' + srcaddress;

    $.ajax({
        url: "/FourPsApplication/CheckAccounts?searchparams=" + srcparam,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#tbodyAccounts').empty();
            if (result.data.length > 0) {
                var html = '';

                html += '<table class="table table-condensed table-striped table-bordered" id="tblAccounts"> ';
                html += '    <thead> ';
                html += '        <tr> ';
                html += '            <th class="text-center" scope="col">Account No</th> ';
                html += '            <th class="text-center" scope="col">Name</th> ';
                html += '            <th class="text-center" scope="col">Address</th> ';
                html += '            <th class="text-center" scope="col"></th> ';
                html += '        </tr> ';
                html += '    </thead> ';
                html += '    <tbody id="tbodyAccounts"></tbody> ';
                html += '</table> ';

                $('#divSeachResult').append(html);

                appendToSearchTableBody(result.data);
            }
            else {
                var html = '';
                html += '<div class="col-lg-offset-3 col-lg-6 text-center">';
                html += '   <h4 class="text-capitalized">No account(s) found.</h4>'
                html += '</div>'

                $('#divResult').append(html);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function appendToSearchTableBody(data) {
    var html = '';
    $.each(data, function (key, item) {
        html += '<tr class="" id="' + item.AccountNo + '">';
        html += '<td id="' + item.AccountNo + '" class="text-center">' + item.AccountNo + '</td>';
        html += '<td id="' + item.AccountName + '" class="text-center">' + item.AccountName + '</td>';
        html += '<td>' + item.Address + '</td>';
        html += '<td><input type="button" class="btn btn-danger" value="SET THIS ACCOUNT" onclick="setThisAccount(' + item.AccountNo.toString() + ')" /></td>';
        html += '</tr>';
    });

    $('#tbodyAccounts').html(html);
}


function setThisAccount(acctnumber) {

    var stractno = ("0" + acctnumber).substr(-10);
    $('#txtaccountno').val(stractno);

    $('#txtsrcacctno').val("");
    $('#txtsrcacctname').val("");
    $('#txtsrcaddress').val("");
    $('#divSeachResult').empty();

    $('#modalSearchAcct').modal("hide");
}

function setAccount() {
    //check the account if existing and active
}


function rbothersOnChange() {
    if ($("#rbothers").prop('checked', true)) {
        $('#txtothers').prop('disabled', false);
       
    }
    else {

        $('#txtothers').val("");
        $('#txtothers').prop('disabled', true);
    }
        
}

function rbdisapprovedOnChange() {

}