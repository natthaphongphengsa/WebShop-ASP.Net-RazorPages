// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function CancelClicked(id) {
    if (id == 1) {
        var r = confirm("All of your work will not save. Do you still want to cancel?");
        if (r == true) {
            return window.location.href = "/Identity/Admin/Management/CategoryList";
        }
    } else {
        var r = confirm("All of your work will not save. Do you still want to cancel?");
        if (r == true) {
            return window.location.href = "/Identity/Admin/AdminPage";
        }
    }
}