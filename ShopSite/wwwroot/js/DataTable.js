$(document).ready(function () {
    $('#myTable').DataTable({
        "scrollY": "350px",
        "scrollX": true,
        "scroller": true,
        "scrollCollapse": true,
        "paging": true,
        "searching": false,
        "info": false,
        "language": {
            "lengthMenu": "_MENU_"
        }
    })
})
