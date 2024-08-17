//var dtable;
//$(document).ready(function () {
//    dtable = $('#myTable').DataTable({
//        "ajax": {
//            "url": "/Admin/Product/AllProducts",

//            success: function (data) {
//                console.log(data);
//            }
//        },
//            "columns": [
//                { "data": "name" },           // Maps to 'Product Name'
//                { "data": "description" },    // Maps to 'Product Description'
//                { "data": "price" },          // Maps to 'Price'

//            ]
//        });
//});

$(document).ready(function () {
    $.ajax({
        url: '/Admin/Product/AllProducts',
        method: 'GET',
        success: function (response) {
            var rows = '';
            $.each(response.data, function (index, product) {
                rows += '<tr>';
                rows += '<td>' + product.name + '</td>';
                rows += '<td>' + product.description + '</td>';
                rows += '<td>' + product.price + '</td>';
                rows += '</tr>';
            });
            $('#myTable tbody').html(rows);
        }
    });
});