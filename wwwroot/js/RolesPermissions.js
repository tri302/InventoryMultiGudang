$(document).ready(function () {
  // Toggle Permission via AJAX
  $(".chk-permission").on("change", function () {
    const roleId = $(this).data("role-id");
    const permName = $(this).data("perm-name");
    const isChecked = $(this).is(":checked");

    $.ajax({
      url: "/RolesPermissions/TogglePermission",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify({
        roleId: roleId,
        permissionName: permName,
        isChecked: isChecked,
      }),
      success: function (res) {
        toastr.success(res.message, "Success", { timeOut: 3000 });
        if (!res.success) {
          toastr.error(res.message, "Error", { timeOut: 3000 });
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan koneksi server.", "Error", { timeOut: 3000 });
      },
    });
  });
});
