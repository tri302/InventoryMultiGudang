$(document).ready(function () {
  const $modal = $("#modalAddrolle");
  const $input = $("#roleNameInput");
  const $errorDiv = $("#roleNameInputError");

  // 1. Bersihkan form saat modal pertama kali dibuka
  $modal.on("show.bs.modal", function () {
    $input.val("");
    $input.removeClass("is-invalid-custom is-valid-custom");
    $errorDiv.hide();
  });

  // 2. Live validation saat user mengetik (Merah jika kosong, Hijau jika terisi)
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

  // 3. Validasi saat tombol submit diklik
  $("#btnSaveRole").on("click", function () {
    const val = $input.val().trim();

    if (val === "") {
      $input.removeClass("is-valid-custom").addClass("is-invalid-custom");
      $errorDiv.show();
      toastr.warning("Role name wajib diisi!", "Peringatan", { timeOut: 3000 });
      return;
    }

    // Kirim via AJAX jika valid
    $.ajax({
      url: "/RolesPermissions/CreateRole",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify({ roleName: val }),
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
