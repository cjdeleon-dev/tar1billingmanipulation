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
                html += '           <th style="text-align:center;">ID</th> ';
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
        html += '<td><input type="button" class="btn btn-danger" value="APPLICATION >>" onclick="showFourPsApp('+ item.EntryId +')" /></td>';
        html += '</tr>';
    });
    $('#tbodyQualified').html(html);
}

function showQMEApp() {
    alert("QME Application");
}


function showFourPsApp(id) {
    alert("Selected Entry ID: " + id);
}