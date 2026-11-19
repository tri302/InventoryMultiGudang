$(document).ready(function () {
  const $modal = $("#modalAddPermission");
  const $input = $("#permissionNameInput");
  const $categorySelect = $("#permissionCategoryInput");
  const $errorDiv = $("#permNameInputError");

  // 1. Reset form saat modal dibuka
  $modal.on("show.bs.modal", function () {
    $input.val("");
    $categorySelect.val("General"); // Reset ke default
    $input.removeClass("is-invalid-custom is-valid-custom");
    $errorDiv.hide();
  });

  // 2. Live validation input nama
  $input.on("input", function () {
    const val = $(this).val().trim();
    if (val !== "") {
      $(this).removeClass("is-invalid-custom").addClass("is-valid-custom");
      $errorDiv.hide();
    } else {
      $(this).removeClass("is-valid-custom").addClass("is-invalid-custom");
      $errorDiv.show();
    }
  });

  // 3. Submit data ke Controller via AJAX
  $("#btnSavePermission").on("click", function () {
    const permName = $input.val().trim();
    const category = $categorySelect.val();
    const type = $("#permissionTypeInput").val();

    if (permName === "") {
      $input.removeClass("is-valid-custom").addClass("is-invalid-custom");
      $errorDiv.show();
      toastr.warning("Permission name wajib diisi!", "Peringatan", {
        timeOut: 3000,
      });
      return;
    }

    $.ajax({
      url: "/RolesPermissions/CreatePermission",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify({
        permissionName: permName,
        category: category,
        type: type
      }),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message, "Berhasil", { timeOut: 2000 });
          setTimeout(() => location.reload(), 1000);
        } else {
          toastr.error(res.message, "Gagal", { timeOut: 3000 });
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan pada server.", "Error", {
          timeOut: 3000,
        });
      },
    });
  });
});

//Delete Permission menggunakan SweetAlert2
$(document).on("click", ".btn-delete-perm", function () {
  const id = $(this).data("id");

  Swal.fire({
    title: "Apakah Anda yakin?",
    text: "Permission yang dihapus tidak dapat dikembalikan!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#dc3545",
    cancelButtonColor: "#6c75d7",
    confirmButtonText: "Ya, Hapus!",
    position: "top",
    cancelButtonText: "Batal",
  }).then((result) => {
    if (result.isConfirmed) {
      $.ajax({
        url: "/RolesPermissions/DeletePermission",
        type: "POST",
        data: { id: id },
        success: function (res) {
          if (res.success) {
            toastr.success(res.message, "Berhasil", { timeOut: 1500 });
            
            // Beri jeda sedikit agar toastr sempat terlihat, lalu reload/redirect otomatis
            setTimeout(function () {
              window.location.href = '/RolesPermissions/PermissionsManager';
            }, 1000);

          } else {
            toastr.error(res.message, "Error", { timeOut: 3000 });
          }
        },
        error: function () {
          toastr.error("Terjadi kesalahan koneksi server.", "Error", {
            timeOut: 3000,
          });
        },
      });
    }
  });
});