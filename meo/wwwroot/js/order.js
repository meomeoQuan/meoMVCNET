var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inprocess")) {
        LoadDataTable("inprocess");
    } else if (url.includes("completed")) {
        LoadDataTable("completed");
    } else if (url.includes("pending")) {
        LoadDataTable("pending");
    } else if (url.includes("approved")) {
        LoadDataTable("approved");
    } else {
        LoadDataTable("all");
    }
});




////datatables.net/manual/data/
function LoadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/GetAll?status=' + status },
        "columns": [
            { data: 'applicationUserId', "width": "15%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "10%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'orderHeaderId',
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                           <a href="/admin/order/Details?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
                          
                    </div>`;
                },
                "width": "25%"
            },
        ]
    });
}