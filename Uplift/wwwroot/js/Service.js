﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
   dataTable = $("#table").DataTable({
        "ajax": {
            "url": "/admin/Service/GetAll",
            "type": "GET",
            "datatype": "json"
        },

        "columns": [
            { "data": "name", "width": "30%" },
            { "data": "category.name", "width": "20%" },
            { "data": "price", "width": "10%" },
            { "data": "frequency.frequencyCount", "width": "10%"},
            {
                "data": "id",
                "render": function(data) {
                    return `<div class="text-center">
                                <a href="/Admin/Service/Upsert/${data}" class='btn btn-success text-white' style='cursor:pointer; width:100px;'>
                                    <i class='far fa-edit'></i> Edit
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Service/Delete/${data}") class='btn btn-danger text-white' style='cursor:pointer; width:100px;'>
                                    <i class='far fa-trash-alt'></i> Delete
                                </a>
                            </div>
                            `;

                }, "width": "30%"

            }
        ],

        "language": {
            "emptyTable": "No data found."
        },
        "width": "100%"
    });

}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the content!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: "DELETE",
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}