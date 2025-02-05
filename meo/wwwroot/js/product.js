var dataTable;

$(document).ready(function () {
    LoadDataTable();
});
////datatables.net/manual/data/
function LoadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/GetAll' },
        "columns": [
            { data: 'title', "width": "15%" },
            { data: 'author', "width": "15%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'category.categoryName', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                           <a href="/admin/Product/UpSert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
                           <a onclick="Delete('/admin/Product/Delete/${data}')" class="btn btn-danger mx-2"><i class="bi bi-trash3-fill"></i> Delete</a>
                    </div>`;
                },
                "width": "25%"
            },
        ]
    });
}

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
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload(null,false);
                    toastr.success(data.message);
                   
                   
                }
            });
        }
    });
}
