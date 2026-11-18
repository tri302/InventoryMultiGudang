$(document).ready(function () {
  $(".btn-delete-perm").on("click", function () {
    const id = $(this).data("id");
    if (confirm("Yakin ingin menghapus permission ini?")) {
      $.ajax({
        url: "/RolesPermissions/DeletePermission",
        type: "POST",
        data: { id: id },
        success: function (res) {
          if (res.success) {
            toastr.success(res.message, "Success", { timeOut: 3000 });
            location.reload();
          } else {
            toastr.error(res.message, "Error", { timeOut: 3000 });
          }
        },
      });
    }
  });
});
