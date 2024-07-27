document.addEventListener("DOMContentLoaded", function () {
    var currentPageSize = @(ViewBag.PageSize); // Get current page size from ViewBag
    var pageSizeDropdown = document.getElementById("pageSize");
    // Set selected option based on current page size
    for (var i = 0; i < pageSizeDropdown.options.length; i++) {
        if (pageSizeDropdown.options[i].value === currentPageSize.toString()) {
            pageSizeDropdown.selectedIndex = i;
            break;
        }
    }
});
function changePageSize(select) {
    var pageSize = select.value;
    var currentPage = @(ViewBag.CurrentPage);
    window.location.href = "/Phonebook/Index?page=" + currentPage + "&pageSize=" + pageSize;
}