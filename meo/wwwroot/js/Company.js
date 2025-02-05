var dataTable;

$(document).ready(function () {
    LoadDataTable();
});

// Function to load DataTable
function LoadDataTable() {
    dataTable = $('#myCompany').DataTable({
        "ajax": { url: '/admin/Company/GetAll' },
        "columns": [
            { data: 'companyName', "width": "15%" },
            { data: 'companyStreetAddress', "width": "15%" },
            { data: 'companyCity', "width": "15%" },
            { data: 'companyState', "width": "10%" },
            { data: 'companyPostalCode', "width": "10%" },
            {
                data: 'companyID',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                               <a href="/admin/Company/UpSert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
                               <a onclick="Delete('/admin/Company/Delete/${data}')" class="btn btn-danger mx-2"><i class="bi bi-trash3-fill"></i> Delete</a>
                            </div>`;
                },
                "width": "25%"
            },
        ]
    });
}

// Function to handle delete operation
function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE', // Fixed: Ensure 'DELETE' is a string
                success: function (data) {
                    dataTable.ajax.reload(null, false); // Reload the DataTable without resetting the paging
                    Swal.fire({
                        title: "Deleted!",
                        text: "Delete Success!",
                        icon: "success"
                    });
                },
                error: function () {
                    Swal.fire({
                        title: "Error!",
                        text: "An error occurred while deleting the record.",
                        icon: "error"
                    });
                }
            });
        }
    });
}
