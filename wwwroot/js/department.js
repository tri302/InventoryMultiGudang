$(document).ready(function () {
  // ==========================================
  // VALIDASI MANUAL (Garis Merah Bootstrap)
  // ==========================================
  function validateFormManual($form) {
    let isValid = true;
    $form
      .find("input[data-val-required], textarea[data-val-required]")
      .each(function () {
        const val = $(this).val() ? $(this).val().toString().trim() : "";
        const fieldName = $(this).attr("name");
        const customMsg =
          $(this).attr("data-val-required") || "Field ini wajib diisi!";
        const $errorSpan = $form.find(`span[data-valmsg-for="${fieldName}"]`);

        if (val === "") {
          isValid = false;
          $(this).addClass("is-invalid");
          if ($errorSpan.length) {
            $errorSpan.text(customMsg).show();
          }
        } else {
          $(this).removeClass("is-invalid");
          if ($errorSpan.length) {
            $errorSpan.text("").hide();
          }
        }
      });
    return isValid;
  }

  // Otomatis hapus garis merah saat user mengetik / mengisi field
  $(document).on(
    "input change",
    "#formTambahDepartemen input, #formTambahDepartemen textarea, #formEditDepartemen input, #formEditDepartemen textarea",
    function () {
      $(this).removeClass("is-invalid");
      const fieldName = $(this).attr("name");
      if (fieldName) {
        const $form = $(this).closest("form");
        $form.find(`span[data-valmsg-for="${fieldName}"]`).text("").hide();
      }
    },
  );

  let currentPage = 1;
  let searchTimer;

  // CSRF Token Setup
  const getRequestHeader = () => {
    return {
      RequestVerificationToken: $(
        'input[name="__RequestVerificationToken"]',
      ).val(),
    };
  };

  // Load Data awal
  loadTableData();

  // 1. Live Search dengan Debounce (Ajax tanpa reload)
  $("#searchInput").on("keyup", function () {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function () {
      currentPage = 1;
      loadTableData();
    }, 300);
  });

  // 2. Pagination Click (Ajax tanpa reload)
  $(document).on("click", ".page-nav", function (e) {
    e.preventDefault();
    const page = $(this).data("page");
    if (page && page !== currentPage) {
      currentPage = page;
      loadTableData();
    }
  });

  // Load Table Function
  function loadTableData() {
    const search = $("#searchInput").val();
    $.ajax({
      url: "/Department/GetTableData",
      type: "GET",
      data: { search: search, page: currentPage },
      success: function (html) {
        $("#tableContainer").html(html);
      },
      error: function () {
        if (typeof toastr !== "undefined")
          toastr.error("Gagal memuat data tabel!");
      },
    });
  }

  // Safe Re-parse jQuery Unobtrusive Validation
  function rebindValidation(formId) {
    const $form = $(formId);
    if (typeof $ !== "undefined" && $.validator && $.validator.unobtrusive) {
      $form.removeData("validator").removeData("unobtrusiveValidation");
      $.validator.unobtrusive.parse($form);
    }
  }

  // Modal Create Trigger (DI SINI LOKASI YANG BENAR)
  $(document).on("click", "#btnTambahDepartemen", function () {
    const modalEl = document.getElementById("modalTambahDepartemen");
    if (modalEl) {
      const modalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);
      modalInstance.show();
    }
  });

  // Modal Create Reset
  $("#modalTambahDepartemen").on("show.bs.modal", function () {
    const $form = $("#formTambahDepartemen");
    if ($form.length > 0) {
      $form[0].reset();
      $form.find(".field-validation-error").text("");
      rebindValidation("#formTambahDepartemen");
    }
  });

  // Submit Create Ajax
  $("#formTambahDepartemen").on("submit", function (e) {
    e.preventDefault();
    const $form = $(this);

    if (!validateFormManual($form)) {
      toastr.warning("Mohon lengkapi field wajib yang belum diisi!");
      return false;
    }

    $.ajax({
      url: "/Department/Create",
      type: "POST",
      data: $form.serialize(),
      headers: getRequestHeader(),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          const modalEl = document.getElementById("modalTambahDepartemen");
          const modalInstance = bootstrap.Modal.getInstance(modalEl);
          if (modalInstance) modalInstance.hide();
          loadTableData();
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem!");
      },
    });
  });

  // Open Edit Modal & Fetch Data
  $(document).on("click", ".btn-edit", function () {
    const id = $(this).data("id");
    $.ajax({
      url: "/Department/GetById",
      type: "GET",
      data: { id: id },
      success: function (res) {
        if (res.success) {
          $("#Edit_Id").val(res.data.id);
          $("#Edit_Code").val(res.data.code);
          $("#Edit_Name").val(res.data.name);
          $("#Edit_Description").val(res.data.description);

          $("#formEditDepartemen").find(".field-validation-error").text("");
          rebindValidation("#formEditDepartemen");

          const modalEl = document.getElementById("modalEditDepartemen");
          const modalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);
          modalInstance.show();
        } else {
          toastr.error(res.message);
        }
      },
    });
  });

  // Submit Edit Ajax
  $("#formEditDepartemen").on("submit", function (e) {
    e.preventDefault();
    const $form = $(this);

    if (!validateFormManual($form)) {
      toastr.warning("Mohon lengkapi field wajib yang belum diisi!");
      return false;
    }

    $.ajax({
      url: "/Department/Edit",
      type: "POST",
      data: $form.serialize(),
      headers: getRequestHeader(),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          const modalEl = document.getElementById("modalEditDepartemen");
          const modalInstance = bootstrap.Modal.getInstance(modalEl);
          if (modalInstance) modalInstance.hide();
          loadTableData();
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan pada sistem!");
      },
    });
  });

  // Open Delete Modal
  $(document).on("click", ".btn-delete", function () {
    const id = $(this).data("id");
    const name = $(this).data("name");

    $("#Delete_Id").val(id);
    $("#deleteDepartmentName").text(`"${name}"`);

    const modalEl = document.getElementById("modalHapusDepartemen");
    const modalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);
    modalInstance.show();
  });

  // Submit Delete Ajax
  $("#btnConfirmDelete").on("click", function () {
    const id = $("#Delete_Id").val();

    $.ajax({
      url: "/Department/Delete",
      type: "POST",
      data: { id: id },
      headers: getRequestHeader(),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          const modalEl = document.getElementById("modalHapusDepartemen");
          const modalInstance = bootstrap.Modal.getInstance(modalEl);
          if (modalInstance) modalInstance.hide();
          loadTableData();
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Gagal menghapus data!");
      },
    });
  });

  // HELPER: Auto-Format Rupiah (16500000 -> 16.500.000)
  $(document).on("keyup input", ".currency-input", function () {
    let value = $(this)
      .val()
      .replace(/[^,\d]/g, "")
      .toString();
    let split = value.split(",");
    let sisa = split[0].length % 3;
    let rupiah = split[0].substr(0, sisa);
    let ribuan = split[0].substr(sisa).match(/\d{3}/gi);

    if (ribuan) {
      let separator = sisa ? "." : "";
      rupiah += separator + ribuan.join(".");
    }

    rupiah = split[1] !== undefined ? rupiah + "," + split[1] : rupiah;
    $(this).val(rupiah);
  });
});
