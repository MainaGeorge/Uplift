$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $("#table").DataTable({
        "ajax": {
            "url": "/admin/category/GetAll",
            "type": "GET",
            "datatype": "json"
        },

        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "displayOrder", "width": "20%" },
            {
                "data": "id",
                "render": function(data) {
                    return `<div class="text-center">
                        <a href="/Admin/Category/Upsert/${data}"
                        class="btn btn-success text-white" style="cursor: pointer; width: 100px">
                            <i class="far fa-edit">Edit</i>
                        </a>
                        &nbsp;
                        <a onclick=Delete({data}") class="btn btn-danger text-white" style="cursor: pointer; width: 100px">
                            <i class="far fa-trash-alt">Edit</i>
                        </a>
                    </div >`;

                }, "width": "30%"

            }
        ],

        "language": {
            "emptyTable": "No data found."
        },
        "width": "100%"
    });
}