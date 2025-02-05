

var dataTable;

$(document).ready(function () {
    LoadDataTable();
});

// Function to load DataTable
function LoadDataTable() {
    dataTable = $('#myUser').DataTable({
        "ajax": { url: '/admin/User/GetAll' },
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'company.companyName', "width": "10%" },
            { data: 'role', "width": "10%" },
            {
                data: { id: "companyID", lockoutEnd: "lockoutEnd"},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                           <div class="text-center">

                              <a onclick=LockUnLock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer ; width:100px;">
                                       <i class="bi bi-lock-fill"></i> lock
                             </a>
                             <a class="btn btn-danger text-white" style="cursor:pointer ; width:150px">
                                   <i class="bi bi-pencil-square"></i> Permission
                             </a>
                           </div>
                        `
                    }
                    else {
                        return `
                         <div class="text-center">
                            <a onclick=LockUnLock('${data.id}') class="btn btn-success text-white" style="cursor:pointer ; width:100px;">
                                       <i class="bi bi-unlock-fill"></i> unlock
                             </a>
                             <a class="btn btn-danger text-white" style="cursor:pointer ; width:150px">
                                   <i class="bi bi-pencil-square"></i> Permission
                             </a>
                           </div>
                        `
                    }
                  
                   
                },
                "width": "25%"
            },
        ]
    });
}


function LockUnLock(id) {
    $.ajax({
        type: "POST",
        url: '/admin/user/LockUnlock', 
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                dataTable.ajax.reload(null, false);
                toastr.success(data.message);
            }
        }

    });
}


