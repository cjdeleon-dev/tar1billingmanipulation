
var isMemberWithNonExistAcct = false;

function initializeData() {
    loadTowns();
    loadTypes();
    loadMunicipalities();
    loadConsumerTypes();
}
function invisibleModalButtons(optproc) {
    switch (optproc) {
        case 'view','update','cancel':
            $('#btnCancel').hide();
            $('#btnUpdate').hide();
            $('#btnEdit').show();
            $('#btnClose').show();
            $('#btnAddAccount').hide();

            $('#txtBName').removeAttr('disabled');
            $('#txtLName').removeAttr('disabled');
            $('#txtFName').removeAttr('disabled');
            $('#txtMName').removeAttr('disabled');
            $('#txtSuffix').removeAttr('disabled');
            $('#txtBName').prop('disabled', true);
            $('#txtLName').prop('disabled', true);
            $('#txtFName').prop('disabled', true);
            $('#txtMName').prop('disabled', true);
            $('#txtSuffix').prop('disabled', true);

            $('#cboType').removeAttr('disabled');
            $('#cboType').prop('disabled', true);
            $('#txtMemberId').removeAttr('disabled');
            $('#txtMemberId').prop('disabled', true);
            $('#dtpMemberDate').removeAttr('disabled');
            $('#dtpMemberDate').prop('disabled', true);

            $('#tbodyAccts').empty();

            displayDetailsById($('#txtId').val());

            break;
        case 'edit':
            $('#btnCancel').show();
            $('#btnUpdate').show();
            $('#btnEdit').hide();
            $('#btnClose').hide();
            $('#btnAddAccount').show();

            if ($('#cboType').val() <= 2 && $('#cboType').val() > 0) {
                
                $('#txtLName').removeAttr('disabled');
                $('#txtFName').removeAttr('disabled');
                $('#txtMName').removeAttr('disabled');
                $('#txtSuffix').removeAttr('disabled');

                $('#txtBName').removeAttr('disabled');
                $('#txtBName').prop('disabled', true);

            } else {
                $('#txtBName').removeAttr('disabled');

                $('#txtLName').removeAttr('disabled');
                $('#txtFName').removeAttr('disabled');
                $('#txtMName').removeAttr('disabled');
                $('#txtSuffix').removeAttr('disabled');
                $('#txtLName').prop('disabled', true);
                $('#txtFName').prop('disabled', true);
                $('#txtMName').prop('disabled', true);
                $('#txtSuffix').prop('disabled', true);
            }
            
            $('#cboType').removeAttr('disabled');
            $('#txtMemberId').removeAttr('disabled');
            $('#dtpMemberDate').removeAttr('disabled');

            break;
        default:
            $('#btnCancel').hide();
            $('#btnUpdate').hide();
            $('#btnEdit').show();
            $('#btnClose').show();
            $('#btnAddAccount').hide();

            $('#txtBName').removeAttr('disabled');
            $('#txtLName').removeAttr('disabled');
            $('#txtFName').removeAttr('disabled');
            $('#txtMName').removeAttr('disabled');
            $('#txtSuffix').removeAttr('disabled');
            $('#txtBName').prop('disabled', true);
            $('#txtLName').prop('disabled', true);
            $('#txtFName').prop('disabled', true);
            $('#txtMName').prop('disabled', true);
            $('#txtSuffix').prop('disabled', true);

            $('#cboType').removeAttr('disabled');
            $('#cboType').prop('disabled', true);
            $('#txtMemberId').removeAttr('disabled');
            $('#txtMemberId').prop('disabled', true);
            $('#dtpMemberDate').removeAttr('disabled');
            $('#dtpMemberDate').prop('disabled', true);
            break;
    }
    
}

function loadTowns() {
    $.ajax({
        url: "/Members/GetAllTowns/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboTown').empty();
            $('#cboTown').append("<option value=0>PLEASE SELECT MUNICIPALITY</option>");
            for (var i = 0; i < result.data.length; i++) {
                var Desc = '(' + result.data[i].Id + ') - ' +result.data[i].Town;
                var opt = new Option(Desc, result.data[i].Id);
                $('#cboTown').append(opt);
            }
            $('#cboTown').append("<option value=8>SELECT ALL</option>");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadTypes() {
    $.ajax({
        url: "/Members/GetAllTypes/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboType').empty();
            $('#cboNewType').empty();
            $('#cboENewType').empty();

            $('#cboType').append("<option value=0>PLEASE SELECT TYPE</option>");
            $('#cboNewType').append("<option value=0>PLEASE SELECT TYPE</option>");
            $('#cboENewType').append("<option value=0>PLEASE SELECT TYPE</option>");

            for (var i = 0; i < result.data.length; i++) {
                let Desc = result.data[i].MemberType;
                let opt = new Option(Desc, result.data[i].Id);

                $('#cboType').append(opt);
                
            }

            for (var j = 0; j < result.data.length; j++) {
                let Desc = result.data[j].MemberType;
                let opt = new Option(Desc, result.data[j].Id);

                $('#cboNewType').append(opt);
            }

            for (var j = 0; j < result.data.length; j++) {
                let Desc = result.data[j].MemberType;
                let opt = new Option(Desc, result.data[j].Id);

                $('#cboENewType').append(opt);
            }


        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadClasses() {
    $.ajax({
        url: "/Members/GetAllClasses/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboClass').empty();
            $('#cboClass').append("<option value=0>PLEASE SELECT CLASS</option>");
            for (var i = 0; i < result.data.length; i++) {
                var Desc = result.data[i].MemberClass;
                var opt = new Option(Desc, result.data[i].Id);
                $('#cboClass').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadMunicipalities() {
    $.ajax({
        url: "/Members/GetAllMunicipalities/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboNewMunicipality').empty();
            $('#cboENewMunicipality').empty();

            $('#cboNewMunicipality').append("<option value=0>SELECT MUNICIPALITY</option>");
            $('#cboENewMunicipality').append("<option value=0>SELECT MUNICIPALITY</option>");

            for (var i = 0; i < result.data.length; i++) {
                let Desc = result.data[i].Municipality + ", " + result.data[i].Province;
                let opt = new Option(Desc, result.data[i].Id);

                $('#cboNewMunicipality').append(opt);
            }
            for (var j = 0; j < result.data.length; j++) {
                let Desc = result.data[j].Municipality + ", " + result.data[j].Province;
                let opt = new Option(Desc, result.data[j].Id);

                $('#cboENewMunicipality').append(opt);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadBarangays(municipalityid, elmnt) {
    $.ajax({
        url: "/Members/GetAllBarangaysByMunicipalityId?id=" + municipalityid,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            if (elmnt === "N") {
                $('#cboNewBarangay').empty();
                $('#cboNewBarangay').append("<option value=0>SELECT BARANGAY</option>");
                for (var i = 0; i < result.data.length; i++) {
                    let Desc = result.data[i].Barangay;
                    let opt = new Option(Desc, result.data[i].Id);
                    $('#cboNewBarangay').append(opt);
                }
            } else {
                $('#cboENewBarangay').empty();
                $('#cboENewBarangay').append("<option value=0>SELECT BARANGAY</option>");
                for (var i = 0; i < result.data.length; i++) {
                    let Desc = result.data[i].Barangay;
                    let opt = new Option(Desc, result.data[i].Id);
                    $('#cboENewBarangay').append(opt);
                }
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function loadConsumerTypes(){
    $.ajax({
        url: "/Members/GetAllConsumerTypes/",
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#cboNewMemberConsType').empty();
            $('#cboNewMemberConsType').append("<option value=0>SELECT CONSUMER TYPE</option>");
            for (var i = 0; i < result.data.length; i++) {
                var Desc = result.data[i].ConsumerType;
                var opt = new Option(Desc, result.data[i].Id);
                $('#cboNewMemberConsType').append(opt);
            }

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function getAllMembers() {

    //invisibleModalButtons
    invisibleModalButtons('view');
    //load Member Types
    loadTypes();

    document.body.style.cursor = 'progress';
    $('#modalLoading').modal('show');
    document.getElementById("spintext").innerHTML = "LOADING...";

    var townid = $('#cboTown').val();

    emptyTable();

    $('#myTable1').DataTable({
        ajax: {
            "url": "/Members/LoadData?twnid=" + townid,
            "type": "GET",
            "datatype": "json"
        },
        pageLength: 50,
        initComplete: function (settings, json) {
            document.body.style.cursor = 'default';
            $('#modalLoading').modal('hide');
        },
        columns: [
            { "data": "Id", "autoWidth": true },
            { "data": "BusinessName", "autoWidth": true },
            { "data": "MemberType", "autoWidth": true },
            { "data": "MemberId", "autoWidth": true },
            { "data": "MemberDate", "autoWidth": true },
            { "data": "Barangay", "autoWidth": true },
            { "data": "Municipality", "autoWidth": true }
        ],
        aoColumnDefs: [
            {
                "width": "100px",
                "aTargets": [7],
                "mData": "Id",
                "mRender": function (data, type, full) {

                    return '<button class="btn btn-info btn-sm" style="font-size:smaller;" href="#" id="vw_' + data + '" ' +
                        'onclick="showDetailsById(\'' + data + '\')">' +
                        '<i class="glyphicon glyphicon-list-alt"></i></button> ';
                },
                "className": "text-center"
            }
        ]
    });
}

function emptyTable() {
    var table = $('#myTable1').DataTable();

    //clear datatable
    table.clear().draw();

    //destroy datatable
    table.destroy();
}

function showDetailsById(id) {
    $('#myViewDetailsModal').modal('show');

    displayDetailsById(id);
}

function displayDetailsById(id) {
    $.ajax({
        url: "/Members/GetMemberById?id=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var data = result.data;
            $('#txtId').val(id);
            $('#txtBName').val(data.BusinessName);
            $('#txtLName').val(data.LastName);
            $('#txtFName').val(data.FirstName);
            $('#txtMName').val(data.MiddleName);
            $('#txtSuffix').val(data.Suffix);
            $('#cboType').val(data.MemberTypeId);
            $('#txtMemberId').val(data.MemberId.toString());
            $('#dtpMemberDate').val(data.MemberDate);
            $('#txtBarangay').val(data.Barangay);
            $('#txtTown').val(data.Municipality);

            $('#tbodyAccts').empty();
            //load all accounts of member to table
            showAccountsById(id);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function showAccountsById(memid) {
    $.ajax({
        url: "/Members/GetAllMemberAccountsById?memberid=" + memid,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#tbodyAccts').empty();
            var data = result.data;

            if (data.length > 0) {
                for (var i = 0; i <= data.length - 1; i++){
                    let acctno = data[i].AccountNo.toString();
                    let address = data[i].Address;
                    let isprimary = data[i].IsPrimary;

                    var html = '';
                    html += '<tr id="mytr_' + acctno + '">';
                    html += '<td class="hidden">OLD</td>';
                    html += '<td class="actno">' + acctno + '</td>';
                    html += '<td>' + address + '</td>';
                    html += '<td style="text-align:center;"><input type="radio" class="select-item checkbox" name="chkprimary" id="myidis_' + acctno + '" /></td>';
                    html += '<td style="text-align:center;" id="tdbtns"><button class="btn btn-primary" id="btn_' + acctno + '" onclick="deleteAccount(' + acctno + ')"><i class="glyphicon glyphicon-trash"></i></button></td>';
                    html += '</tr>';

                    $('#tbodyAccts').append(html);

                    if (isprimary)
                        $('#myidis_' + acctno).prop('checked', 'checked');
                }

                disableTable(true);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function disableTable(bl) {
    var inputs = document.getElementById('myTableAccts').getElementsByTagName('input');
    var btns = document.getElementById('myTableAccts').getElementsByTagName('button');
    for (var i = 0; i < inputs.length; ++i) 
        inputs[i].disabled = bl;

    for (var j = 0; j < btns.length; j++)
        btns[j].disabled = bl;
        
}

function editDetails(idnum) {
    invisibleModalButtons('edit');

    disableTable(false);

    //swal('Note!', 'Please follow the correct format of JOINT and SINGLE type:' +
    //    '\n<Last Name>[comma][space]<First Name>[space]{-[space]<Joint Name>[space]}<Middle Initial>[period]' +
    //    '{[space]<Suffix(JR,SR,I,II,etc..)>}',
    //    'warning');
}

function cancelChanges() {
    invisibleModalButtons('cancel');
    showAccountsById($('#txtId').val());
}

function updateDetails() {
    //update details and accounts
    saveMemberDetails();
    invisibleModalButtons('update');
    //update table.
    getAllMembers();
}

function showAddAccountModal() {
    $('#myAddAccountModal').modal('show');
    $('#txtsrcAcctNo').val('');

    document.getElementById("hAcctNo").innerHTML = "";
    document.getElementById("pName").innerHTML = "";
    document.getElementById("pAddress").innerHTML = "";
}

function searchAccount() {
    let acctno = $('#txtsrcAcctNo').val();

    if (acctno.trim() == '') {
        swal('No Value', 'Account Number field is required.', 'warning');
        return false;
    }

    $.ajax({
        url: "/Members/GetNameAddressByAccountNo?acctno=" + acctno,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            var data = result.data;
            if (data == null) {
                swal('No Account Found', 'The Account Number is not exist.', 'warning');
                document.getElementById("hAcctNo").innerHTML = "";
                document.getElementById("pName").innerHTML = "";
                document.getElementById("pAddress").innerHTML = "";
                $('#btnAddAcct').removeAttr('disabled');
                $('#btnAddAcct').prop('disabled', true);

            } else {
                document.getElementById("hAcctNo").innerHTML = data.AccountNo;
                document.getElementById("pName").innerHTML = data.AccountName;
                document.getElementById("pAddress").innerHTML = data.Address;
                $('#btnAddAcct').removeAttr('disabled');
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function addAccount() {

    let selacctno = document.getElementById("hAcctNo").innerText;
    let seladdress = document.getElementById("pAddress").innerText;

    var html = '';
    html += '<tr id="mytr_' + selacctno + '">';
    html += '<td class="hidden">NEW</td>';
    html += '<td class="actno">' + selacctno + '</td>';
    html += '<td>' + seladdress + '</td>';
    html += '<td style="text-align:center;"><input type="radio" class="select-item checkbox" name="chkprimary" id="myidis_' + selacctno + '" /></td>';
    html += '<td style="text-align:center;" id="tdbtns"><button class="btn btn-primary" onclick="deleteAccount(' + selacctno + ')"><i class="glyphicon glyphicon-trash"></i></button></td>';
    html += '</tr>';

    $('#tbodyAccts').append(html);
}

function deleteAccount(acctno) {

    if (acctno.toString().length == 9)
        acctno = "0" + acctno.toString();

    var Row = document.getElementById("mytr_" + acctno);
    var Cells = Row.getElementsByTagName("td");

    if (Cells[0].innerText == "NEW") {
        //remove full row automatically
        Row.remove();
    } else {
        //go to the first cell of the current row and set DEL as innerHtml
        Cells[0].innerHTML = "DEL"
        //hide only the full row
        Row.style.display = 'none';
    }
}

function saveAccounts() {
    
    var oTable = document.getElementById('myTableAccts');
    var oRowsLen = oTable.rows.length;

    var primaryacct = '';

    if (oRowsLen > 0) {
        //to check if there are old account(s) to be deleted.
        const delAccts = new Array();

        for (var i = 1; i <= oRowsLen - 1; i++) {

            var oRowDel = oTable.rows[i];
            var selacctno = oRowDel.cells[1].innerText;
            if (oRowDel.cells[0].innerText == 'DEL') {
                var objDelAccts = {
                    Id: 0,
                    MemberId: parseInt($('#txtId').val()),
                    AccountNo: selacctno,
                    IsPrimary: false
                };
                delAccts.push(objDelAccts);
            }
        }

        //to check if there are new account(s) inserted to table.
        const newAccts = new Array();

        for (var j = 1; j <= oRowsLen - 1; j++) {

            var oRowNew = oTable.rows[j];
            var selacctno = oRowNew.cells[1].innerText;
            var ischeck = document.getElementById('myidis_' + selacctno).checked;

            if (oRowNew.cells[0].innerText == 'NEW') {
                var objNewAccts = {
                    Id: 0,
                    MemberId: parseInt($('#txtId').val()),
                    AccountNo: selacctno,
                    IsPrimary: ischeck
                };

                newAccts.push(objNewAccts);
            }
        }

        //to check set accountno as primary
        for (var j = 1; j <= oRowsLen - 1; j++) {

            var oRowNew = oTable.rows[j];
            var selacctno = oRowNew.cells[1].innerText;
            var ischeck = document.getElementById('myidis_' + selacctno).checked;

            if (ischeck)
                primaryacct = selacctno;
        }

        //console.log(newAccts);
        //if there are deleted accounts, then delete it to the database.
        if (delAccts.length > 0) {
            $.ajax({
                url: "/Members/DeleteAccounts/",
                type: "POST",
                contentType: "application/json;charset=UTF-8",
                data: JSON.stringify(delAccts),
                dataType: "json",
                success: function (result) {
                    if (result) {
                        console.log('success deletion of account(s).');
                    }
                },
                error: function (errormessage) {
                    swal('Error', 'Something went wrong.', 'error');
                }
            });
        }

        //if there are new accounts, then insert it to the database.
        if (newAccts.length > 0) {
            $.ajax({
                url: "/Members/InsertNewAccounts/",
                type: "POST",
                contentType: "application/json;charset=UTF-8",
                data: JSON.stringify(newAccts),
                dataType: "json",
                success: function (result) {
                    if (result) {
                       console.log('Success insertion of account(s).')
                    }
                },
                error: function (errormessage) {
                    swal('Error', 'Something went wrong.', 'error');
                }
            });
        }

        if (primaryacct != '') {
            $.ajax({
                url: "/Members/UpdateAccountAsPrimary?memid=" + parseInt($('#txtId').val()) + "&acctno=" + primaryacct,
                type: "POST",
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function (result) {
                    if (result) {
                        console.log('Success updating primary account number.')
                    }
                },
                error: function (errormessage) {
                    swal('Error', 'Something went wrong.', 'error');
                }
            });
        }
    }


}

function saveMemberDetails() {
    if (isValidDetails()) {
        var objMember = {
            Id: parseInt($('#txtId').val()),
            IsBusiness: $('#cboType').val() == 3 ? true : false,
            BusinessName: $('#txtBName').val(),
            LastName: $('#txtLName').val(),
            FirstName: $('#txtFName').val(),
            MiddleName: $('#txtMName').val(),
            Suffix: $('#txtSuffix').val(),
            MemberTypeId: $('#cboType').val(),
            MemberType: "",
            MemberId: $('#txtMemberId').val(),
            MemberDate: $('#dtpMemberDate').val(),
            Barangay: $('#txtBarangay').val(),
            Municipality: $('#txtTown').val()
        }

        $.ajax({
            url: "/Members/UpdateMemberDetails/",
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            data: JSON.stringify(objMember),
            dataType: "json",
            success: function (result) {
                if (result == 'Updated Successfully.') {
                    saveAccounts();
                }
            },
            error: function (errormessage) {
                swal('Error', 'Something went wrong.', 'error');
            }
        });
    }
    
}

function cboTypeOnChange() {
    if ($('#cboType').val() == 3) {
        $('#txtBName').removeAttr('disabled');

        $('#txtLName').removeAttr('disabled');
        $('#txtLName').prop('disabled', true);
        $('#txtFName').removeAttr('disabled');
        $('#txtFName').prop('disabled', true);
        $('#txtMName').removeAttr('disabled');
        $('#txtMName').prop('disabled', true);
        $('#txtSuffix').removeAttr('disabled');
        $('#txtSuffix').prop('disabled', true);

    } else {
        //disable businessname field.
        $('#txtBName').removeAttr('disabled');
        $('#txtBName').prop('disabled', true);

        //enable Name Fields.
        $('#txtLName').removeAttr('disabled');
        $('#txtFName').removeAttr('disabled');
        $('#txtMName').removeAttr('disabled');
        $('#txtSuffix').removeAttr('disabled');

    }
}

function cboNewTypeOnChange() {

    var ebn = document.getElementById("divNewBName");
    var enn = document.getElementById("divNewName");

    $('#txtNewId').val("AUTONUMBER");

    if (parseInt($('#cboNewType').val()) > 0) {

        var typeid = parseInt($('#cboNewType').val());

        switch (typeid) {
            case 1:
            case 2:
                ebn.classList.add("hidden");
                enn.classList.remove("hidden");
                break;
            case 3:
                ebn.classList.remove("hidden");
                enn.classList.add("hidden");
                break;
            default:
                ebn.classList.remove("hidden");
                enn.classList.remove("hidden");
                break;
        }
    }
    
}

function cboENewTypeOnChange() {

    var ebn = document.getElementById("divENewBName");
    var enn = document.getElementById("divENewName");

    $('#txtENewId').val("AUTONUMBER");

    if (parseInt($('#cboENewType').val()) > 0) {

        var typeid = parseInt($('#cboENewType').val());

        switch (typeid) {
            case 1:
            case 2:
                ebn.classList.add("hidden");
                enn.classList.remove("hidden");
                break;
            case 3:
                ebn.classList.remove("hidden");
                enn.classList.add("hidden");
                break;
            default:
                ebn.classList.remove("hidden");
                enn.classList.remove("hidden");
                break;
        }
    }

}

function isValidDetails() {
    if ($('#cboType').val() == 1 || $('#cboType').val() == 2) {
        if ($('#txtLName').val().trim() == "" || $('#txtFName').val().trim() == "" || $('#txtMName').val().trim() == "") {
            swal("Invalid", "Last Name, First Name, and Middle Name fields are required.", "warning");
            return false;
        }
        else
            return true;
    } else {
        if ($('#txtBName').val().trim() == "") {
            alert("Invalid Business Name");
            return false;
        }
        else
            return true;
    }
}
function addNewMember() {

    isMemberWithNonExistAcct = true;

    $('#myNewMemberModal').modal("show");
}

function addNewMemberAcctExist() {

    isMemberWithNonExistAcct = false;

    $('#myENewMemberModal').modal("show");
}

function cboNewMunicipalityOnChange() {
    var muniid = $('#cboNewMunicipality').val();

    loadBarangays(muniid, "N");
}

function cboENewMunicipalityOnChange() {
    var muniid = $('#cboENewMunicipality').val();

    loadBarangays(muniid,"E");
}

function saveMemberAndAccount() {

    var memtypeid = 0;
    var isbn = false;
    var businessname, firstname, lastname, middlename, suffix, brgy, muni, ofcid, memid, memdate, acctno, constype;

    if (isMemberWithNonExistAcct)
        memtypeid = $('#cboNewType').val()
    else
        memtypeid = $('#cboENewType').val();

    if (memtypeid == 0) {
        swal('Invalid', 'There are no selected member type.', 'warning');
        return;
    }

    var nbrgy = document.getElementById("cboNewBarangay");
    var ebrgy = document.getElementById("cboENewBarangay");

    var nmuni = document.getElementById("cboNewMunicipality");
    var emuni = document.getElementById("cboENewMunicipality");

    if (memtypeid > 2)
        isbn = true;

    if (isMemberWithNonExistAcct) {
        if (isbn) {
            businessname = $('#txtNewBName').val();
            firstname = "";
            lastname = "";
            middlename = "";
            suffix = "";
        } else {
            businessname = "";
            firstname = $('#txtNewFName').val();
            lastname = $('#txtNewLName').val();
            middlename = $('#txtNewMName').val();
            suffix = $('#txtNewSuffix').val();
        }
        brgy = nbrgy.options[nbrgy.selectedIndex].text;
        muni = nmuni.options[nmuni.selectedIndex].text;
        ofcid = getOfficeId(muni);
        memid = $('#txtNewMemberId').val();
        memdate = $('#dtpNewMemberDate').val();
        acctno = $('#txtNewMemberAcctNo').val();
        constype = $('#cboNewMemberConsType').val();
    }
    else {
        if (isbn) {
            businessname = $('#txtENewBName').val();
            firstname = "";
            lastname = "";
            middlename = "";
            suffix = "";
        } else {
            businessname = "";
            firstname = $('#txtENewFName').val();
            lastname = $('#txtENewLName').val();
            middlename = $('#txtENewMName').val();
            suffix = $('#txtENewSuffix').val();
        }
        brgy = ebrgy.options[ebrgy.selectedIndex].text;
        muni = emuni.options[emuni.selectedIndex].text;
        ofcid = getOfficeId(muni);
        memid = $('#txtENewMemberId').val();
        memdate = $('#dtpENewMemberDate').val();
        acctno = $('#txtENewMemberAccount').val();
        constype = ""
    }
        
    var objModel = {
        IsBusiness:isbn,
        LName:lastname,
        FName:firstname,
        MName:middlename,
        Suffix:suffix,
        BusinessName:businessname,
        MemberTypeId:memtypeid,
        Barangay:brgy,
        Municipality:muni,
        OfficeId:ofcid,
        MemberId:memid,
        MemberDate:memdate,
        MadeBy:"",
        AccountNo:acctno,
        ConsumerType:constype,
        IsExistAccount: !isMemberWithNonExistAcct
    }

    $.ajax({
        url: "/Members/SaveNewMemberWithAccount/",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        data: JSON.stringify(objModel),
        dataType: "json",
        success: function (result) {
            if (result) {
                window.location = "/Members/Index/";
            }
        },
        error: function (errormessage) {
            swal('Error', 'Something went wrong.', 'error');
        }
    });
}

function getOfficeId(muniprov) {
    var officeid = 0;
    var mp = muniprov.split(",");

    if (mp[0].trim() == "CAMILING" || mp[0].trim() == "MAYANTOC" || mp[0].trim() == "SAN CLEMENTE") {
        officeid = 1;
    }

    if (mp[0].trim() == "CUYAPO" || mp[0].trim() == "NAMPICUAN" || mp[0].trim() == "ANAO") {
        officeid = 2;
    }

    if (mp[0].trim() == "GERONA") {
        officeid = 3;
    }

    if (mp[0].trim() == "MONCADA" || mp[0].trim() == "SAN MANUEL") {
        officeid = 4;
    }

    if (mp[0].trim() == "PANIQUI" || mp[0].trim() == "RAMOS") {
        officeid = 5;
    }

    if (mp[0].trim() == "SANTA IGNACIA" || mp[0].trim() == "SAN JOSE") {
        officeid = 6;
    }

    if (mp[0].trim() == "VICTORIA" || mp[0].trim() == "PURA") {
        officeid = 7;
    }

    return officeid;
}