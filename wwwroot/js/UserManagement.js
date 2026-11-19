let deleteUserId = null;
$(document).ready(function () {
  // Inisialisasi Select2 untuk Modal Create
  $(".select2-karyawan").select2({
    theme: "bootstrap-5",
    dropdownParent: $("#modalCreateUser"),
    placeholder: "-- Pilih Karyawan (Opsional) --",
    allowClear: true,
    ajax: {
      url: "/UserManagement/GetKaryawan",
      dataType: "json",
      delay: 250,
      data: function (params) {
        return {
          term: params.term, // Parameter kata kunci ketikan user
        };
      },
      processResults: function (data) {
        return {
          results: data.results,
        };
      },
      cache: true,
    },
  });

  // Inisialisasi Select2 untuk Modal Edit
  $("#editKaryawanId").select2({
    theme: "bootstrap-5",
    dropdownParent: $("#modalEditUser"),
    placeholder: "-- Pilih Karyawan (Opsional) --",
    allowClear: true,
    ajax: {
      url: "/Karyawan/GetKaryawanSelect2",
      dataType: "json",
      delay: 250,
      data: function (params) {
        return { term: params.term };
      },
      processResults: function (data) {
        return { results: data.results };
      },
      cache: true,
    },
  });

  toastr.options = {
    closeButton: true,
    progressBar: true,
    positionClass: "toast-top-right",
    timeOut: "3000",
  };

  loadUsers(1);

  $("#searchInput").on("keyup", function () {
    loadUsers(1);
  });
  $("#roleFilter").on("change", function () {
    loadUsers(1);
  });

  // Live validation untuk menghapus error saat user mengetik
  $("#modalCreateUser input, #modalEditUser input").on(
    "input change",
    function () {
      if ($(this).val().trim() !== "") {
        $(this).removeClass("is-invalid-custom");
        $(this).closest(".mb-3, .mb-3.5, .mb-4").find(".error-text").hide();
      }
    },
  );
  // Hilangkan error merah pada role saat checkbox dicentang
  $(document).on(
    "change",
    "#modalCreateUser input[type=checkbox], #modalEditUser input[type=checkbox]",
    function () {
      let $modal = $(this).closest(".modal");
      if ($modal.find("input[type=checkbox]:checked").length > 0) {
        $modal.find(".p-3.border").removeClass("is-invalid-custom");
        $modal.find(".role-container-error").remove();
      }
    },
  );
});

function loadUsers(page) {
  let search = $("#searchInput").val();
  let role = $("#roleFilter").val();

  $.ajax({
    url: "/UserManagement/GetData",
    type: "GET",
    data: { search: search, role: role, page: page },
    success: function (result) {
      $("#tableContainer").html(result);
    },
  });
}

function resetCreateForm() {
  $("#modalCreateUser form")[0].reset();
  $("#modalCreateUser input").removeClass("is-invalid-custom");
  $("#modalCreateUser .error-text").remove();
}

// Validasi Manual yang Kuat dan Pasti Membaca Input Modal Aktif
function validateForm(modalId) {
  let isValid = true;
  let $modal = $(modalId);

  // Cari input berdasarkan tipe di dalam modal yang sedang dibuka
  let fullName = $modal.find('input[type="text"]').first();
  let email = $modal.find('input[type="email"]').first();
  let password = $modal.find('input[type="password"]').first();

  // Hapus pesan error lama
  $modal.find(".error-text").remove();
  $modal.find(".form-control").removeClass("is-invalid-custom");
  $modal.find(".role-container-error").remove(); // Hapus pesan error role jika ada

  if (!fullName.length || !fullName.val().trim()) {
    setError(fullName, "Full Name wajib diisi!");
    isValid = false;
  }
  if (!email.length || !email.val().trim()) {
    setError(email, "Email Address wajib diisi!");
    isValid = false;
  }
  // Password hanya wajib diisi saat Create
  if (
    modalId === "#modalCreateUser" &&
    (!password.length || !password.val().trim())
  ) {
    setError(password, "Password wajib diisi!");
    isValid = false;
  }

  // Validasi Minimal 1 Role Harus Dicentang
  let checkedRoles = $modal.find("input[type=checkbox]:checked");
  if (checkedRoles.length === 0) {
    let $roleBox = $modal.find(".p-3.border").first();
    $roleBox.addClass("is-invalid-custom");
    $roleBox.after(
      `<div class="error-text text-danger role-container-error" style="font-size:0.75rem; margin-top:4px;">Pilih minimal satu Security Role!</div>`,
    );
    isValid = false;
  }

  if (!isValid) {
    toastr.warning(
      "Mohon lengkapi semua field dan pilih minimal satu role!",
      "Peringatan",
    );
  }
  return isValid;
}

function setError(inputElement, message) {
  if (!inputElement || inputElement.length === 0) return;
  inputElement.addClass("is-invalid-custom");
  let targetGroup = inputElement.closest(".input-group");
  if (targetGroup.length > 0) {
    targetGroup.after(
      `<div class="error-text text-danger" style="font-size:0.75rem; margin-top:4px;">${message}</div>`,
    );
  } else {
    inputElement.after(
      `<div class="error-text text-danger" style="font-size:0.75rem; margin-top:4px;">${message}</div>`,
    );
  }
}

// Tombol Simpan Create
$("#modalCreateUser .btn-save")
  .off("click")
  .on("click", function () {
    if (!validateForm("#modalCreateUser")) return;

    let roles = [];
    $("#modalCreateUser input[type=checkbox]:checked").each(function () {
      let rVal = $(this).val();
      if (rVal) roles.push(rVal);
    });

    let data = {
      FullName: $("#createFullName").val().trim(),
      Email: $("#createEmail").val().trim(),
      Password: $("#createPassword").val(),
      KaryawanId: $("#createKaryawanId").val()
        ? parseInt($("#createKaryawanId").val())
        : null,
      Roles: roles,
    };

    // --- DEBUG DI SINI ---
    //console.log("DATA YANG DIKIRIM KE CREATE:", JSON.stringify(data));

    $.ajax({
      url: "/UserManagement/Create",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(data),
      success: function (response) {
        if (response.success) {
          $("#modalCreateUser").modal("hide");
          loadUsers(1);
          toastr.success(response.message, "Berhasil");
        } else {
          toastr.error(response.message, "Gagal");
        }
      },
      error: function (xhr) {
        toastr.error("Terjadi kesalahan pada server.", "Error");
      },
    });
  });

// Load Data ke Modal Edit
function editUser(id) {
  $.ajax({
    url: "/UserManagement/GetById",
    type: "GET",
    data: { id: id },
    success: function (user) {
      $("#modalEditUser").data("id", user.id);
      $("#editFullName").val(user.fullName);
      $("#editEmail").val(user.email);
      $("#editPassword").val("");
      // --- SET KARYAWAN SELECT2 PADA EDIT ---
      $("#editKaryawanId").val(null).trigger("change");
      if (user.karyawanId) {
        // Buat option baru secara dinamis agar Select2 bisa menampilkan teks karyawannya
        var option = new Option(
          user.karyawanText || `Karyawan ID: ${user.karyawanId}`,
          user.karyawanId,
          true,
          true,
        );
        $("#editKaryawanId").append(option).trigger("change");
      }
      // -------------------------------------
      // Uncheck semua role
      $("#modalEditUser input[type=checkbox]").prop("checked", false);

      // Centang role sesuai data dari backend
      if (user.roles && user.roles.length > 0) {
        $("#modalEditUser input[type=checkbox]").each(function () {
          if (user.roles.includes($(this).val())) {
            $(this).prop("checked", true);
          }
        });
      }

      $("#modalEditUser input").removeClass("is-invalid-custom");
      $("#modalEditUser .error-text").remove();
      $("#modalEditUser").modal("show");
    },
    error: function () {
      toastr.error("Gagal memuat data user.", "Error");
    },
  });
}

// Tombol Simpan Edit
$("#modalEditUser .btn-save")
  .off("click")
  .on("click", function () {
    if (!validateForm("#modalEditUser")) return;

    let id = $("#modalEditUser").data("id");
    let roles = [];
    $("#modalEditUser input[type=checkbox]:checked").each(function () {
      let rVal = $(this).val();
      if (rVal) roles.push(rVal);
    });

    let data = {
      Id: id,
      FullName: $("#editFullName").val().trim(),
      Email: $("#editEmail").val().trim(),
      Password: $("#editPassword").val(),
      KaryawanId: $("#editKaryawanId").val() ? parseInt($("#editKaryawanId").val()) : null,
      Roles: roles,
    };

    // --- DEBUG DI SINI ---
    //console.log("DATA YANG DIKIRIM KE UPDATE:", JSON.stringify(data));

    $.ajax({
      url: "/UserManagement/Update",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(data),
      success: function (response) {
        if (response.success) {
          $("#modalEditUser").modal("hide");
          loadUsers(1);
          toastr.success(response.message, "Berhasil");
        } else {
          toastr.error(response.message, "Gagal");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan pada server.", "Error");
      },
    });
  });

// Konfirmasi Hapus
function deleteConfirm(id) {
  deleteUserId = id;
  $("#modalHapusUser").modal("show");
}

$("#modalHapusUser .btn-delete-modal")
  .off("click")
  .on("click", function () {
    if (!deleteUserId) return;

    $.ajax({
      url: "/UserManagement/Delete",
      type: "POST",
      data: { id: deleteUserId },
      success: function (response) {
        if (response.success) {
          $("#modalHapusUser").modal("hide");
          loadUsers(1);
          toastr.success(response.message, "Berhasil");
        } else {
          toastr.error(response.message, "Gagal");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan pada server.", "Error");
      },
    });
  });
