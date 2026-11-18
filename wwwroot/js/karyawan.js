let currentPage = 1;
let searchTimeout = null;

// Gunakan Delegated Event untuk Tombol Tambah Karyawan agar pasti merespons
$(document).on("click", "#btnTambahKaryawan", function (e) {
  e.preventDefault();
  $("#modalTambahKaryawan").modal("show");
});

$(document).ready(function () {
  // ==========================================
  // VALIDASI MANUAL (Garis Merah Bootstrap)
  // ==========================================
  function validateFormManual($form) {
    let isValid = true;
    $form
      .find(
        "input[data-val-required], textarea[data-val-required], select[data-val-required]",
      )
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

  // Otomatis hapus garis merah saat user mengetik atau mengubah nilai field/select
  $(document).on("input change", "input, textarea, select", function () {
    $(this).removeClass("is-invalid");
    const fieldName = $(this).attr("name");
    if (fieldName) {
      const $form = $(this).closest("form");
      $form.find(`span[data-valmsg-for="${fieldName}"]`).text("").hide();
    }
  });

  // Reset Form & Validation State saat Modal Ditutup
  $("#modalTambahKaryawan, #modalEditKaryawan").on(
    "hidden.bs.modal",
    function () {
      const $form = $(this).find("form");
      if ($form.length) {
        $form[0].reset();
        $form.find(".is-invalid").removeClass("is-invalid");
        $form.find("span[data-valmsg-for]").text("").hide();
        // Reset Select2 jika ada
        $form.find(".select2-dept-ajax").val(null).trigger("change");
      }
    },
  );

  loadKaryawanData(currentPage);

  // Live Search Ajax dengan debounce
  $("#searchInput").on("keyup input", function () {
    clearTimeout(searchTimeout);
    searchTimeout = setTimeout(function () {
      currentPage = 1;
      loadKaryawanData(currentPage);
    }, 300);
  });

  // Filter Ajax
  $("#filterDepartment, #filterStatus").on("change", function () {
    currentPage = 1;
    loadKaryawanData(currentPage);
  });

  // Helper untuk re-parse validasi unobtrusive secara aman
  function parseFormValidation($form) {
    if ($.validator && $.validator.unobtrusive) {
      $form.removeData("validator").removeData("unobtrusiveValidation");
      $.validator.unobtrusive.parse($form);
    }
  }

  // ==========================================
  // FUNGSI INISIALISASI SELECT2 AJAX
  // ==========================================
  function setupDepartmentSelect2($select, $modal) {
    if ($select.data("select2")) {
      $select.select2("destroy");
    }

    $select.select2({
      theme: "bootstrap-5",
      width: "100%",
      dropdownParent: $modal,
      placeholder: "Pilih Departemen...",
      allowClear: true,
      minimumInputLength: 0,
      ajax: {
        url: "/Karyawan/GetDepartmentsSearch",
        type: "GET",
        dataType: "json",
        delay: 300,
        cache: false,
        data: function (params) {
          return {
            q: params.term || "",
          };
        },
        processResults: function (data) {
          return {
            results: data.results || [],
          };
        },
      },
    });
  }

  // Submit Create Form via Ajax
  $("#formTambahKaryawan").on("submit", function (e) {
    e.preventDefault();
    let $form = $(this);

    if (!validateFormManual($form)) {
      toastr.warning("Mohon lengkapi field wajib yang belum diisi!");
      return false;
    }

    if (!validateImageFile("#createUrlFotoFile", "#errorCreateFoto")) {
      toastr.warning(
        "Validasi foto gagal. Periksa kembali file yang diunggah.",
      );
      return false;
    }

    let formData = new FormData(this);
    $.ajax({
      url: "/Karyawan/Create",
      type: "POST",
      data: formData,
      contentType: false,
      processData: false,
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalTambahKaryawan").modal("hide");
          $form[0].reset();
          $("#createDepartmentId").val(null).trigger("change");
          loadKaryawanData(1);
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem.");
      },
    });
  });

  // Fetch Detail data Karyawan untuk Edit Modal
  $(document).on("click", ".btn-edit", function () {
    let id = $(this).data("id");
    let $form = $("#formEditKaryawan");

    $form
      .find(".field-validation-error")
      .text("")
      .addClass("field-validation-valid")
      .removeClass("field-validation-error");
    $("#errorEditFoto").text("").hide();
    $("#editUrlFotoFile").val(""); // reset file input

    $.ajax({
      url: "/Karyawan/GetById",
      type: "GET",
      data: { id: id },
      success: function (res) {
        if (res.success) {
          let d = res.data;
          $("#editKaryawanId").val(d.id);
          $("#editNik").val(d.nik);
          $("#editNamaLengkap").val(d.namaLengkap);
          $("#editJabatan").val(d.jabatan);
          $("#editEmail").val(d.email);
          $("#editNoTelepon").val(d.noTelepon);
          $("#editStatus").val(d.status);

          if (d.departmentId) {
            let deptText = d.departmentName
              ? d.departmentName
              : "Departemen Terpilih";
            let option = new Option(deptText, d.departmentId, true, true);
            $("#editDepartmentId").empty().append(option).trigger("change");
          } else {
            $("#editDepartmentId").val(null).trigger("change");
          }

          parseFormValidation($form);
          $("#modalEditKaryawan").modal("show");
        } else {
          toastr.error(res.message);
        }
      },
    });
  });

  // Submit Edit Form via Ajax
  $("#formEditKaryawan").on("submit", function (e) {
    e.preventDefault();
    let $form = $(this);

    if (!validateFormManual($form)) {
      toastr.warning("Mohon lengkapi field wajib yang belum diisi!");
      return false;
    }

    if (!validateImageFile("#editUrlFotoFile", "#errorEditFoto")) {
      toastr.warning(
        "Validasi foto gagal. Periksa kembali file yang diunggah.",
      );
      return false;
    }

    let formData = new FormData(this);
    $.ajax({
      url: "/Karyawan/Edit",
      type: "POST",
      data: formData,
      contentType: false,
      processData: false,
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalEditKaryawan").modal("hide");
          loadKaryawanData(currentPage);
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem.");
      },
    });
  });

  // Trigger Modal Hapus
  $(document).on("click", ".btn-delete", function () {
    let id = $(this).data("id");
    $("#deleteKaryawanId").val(id);
    $("#modalHapusKaryawan").modal("show");
  });

  // Confirm Delete via Ajax
  $("#btnConfirmDelete").on("click", function () {
    let id = $("#deleteKaryawanId").val();
    $.ajax({
      url: "/Karyawan/Delete",
      type: "POST",
      data: { id: id },
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalHapusKaryawan").modal("hide");
          loadKaryawanData(currentPage);
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem.");
      },
    });
  });

  // Inisialisasi saat modal Create & Edit dibuka
  $("#modalTambahKaryawan").on("show.bs.modal", function () {
    const $select = $("#createDepartmentId");
    $select.empty().append('<option value=""></option>');
    setupDepartmentSelect2($select, $(this));
  });

  $("#modalEditKaryawan").on("show.bs.modal", function () {
    setupDepartmentSelect2($("#editDepartmentId"), $(this));
  });
});

// Load Ajax Data
function loadKaryawanData(page) {
  currentPage = page || 1;
  let search = $("#searchInput").val() || "";
  let dept = $("#filterDepartment").val() || "";
  let status = $("#filterStatus").val() || "";

  $.ajax({
    url: "/Karyawan/GetData",
    type: "GET",
    data: {
      searchTerm: search,
      departmentId: dept,
      status: status,
      pageNumber: currentPage,
      pageSize: 5,
    },
    success: function (res) {
      if (res && res.success && res.data) {
        renderTable(res.data.items);
        renderPagination(res.data);
      } else {
        renderTable([]);
        renderPagination({ totalItems: 0 });
      }
    },
    error: function (xhr, status, error) {
      console.error("Ajax Error:", error);
      renderTable([]);
    },
  });
}

// Render Tabel Karyawan
function renderTable(items) {
  let html = "";

  if (!items || items.length === 0) {
    html = `
            <tr>
                <td colspan="5" class="text-center py-5 text-muted fw-semibold">
                    <i class="bi bi-inbox fs-2 d-block mb-2"></i>
                    Belum ada data untuk ditampilkan !
                </td>
            </tr>`;
  } else {
    $.each(items, function (i, item) {
      let isAktif = String(item.status).toLowerCase() === "aktif";
      let statusBadge = isAktif
        ? '<span class="badge-aktif px-3 py-2">Aktif</span>'
        : '<span class="badge bg-secondary px-3 py-2">Non-Aktif</span>';

      let rawFoto =
        item.urlFoto || item.UrlFoto || item.fotoUrl || item.FotoUrl;

      let fotoUrl = "/images/no-image.png";
      if (rawFoto && typeof rawFoto === "string" && rawFoto.trim() !== "") {
        let cleanPath = rawFoto.trim().replace(/^\/+/, "");
        if (!cleanPath.toLowerCase().startsWith("images/")) {
          cleanPath = "images/" + cleanPath;
        }
        fotoUrl = "/" + cleanPath;
      }

      html += `
                <tr class="border-bottom">
                    <td class="ps-4 py-3">
                        <div class="d-flex align-items-center gap-3">
                            <div class="flex-shrink-0" style="width: 40px; height: 40px; border-radius: 8px; overflow: hidden; background-color: #f1f5f9; display: flex; align-items: center; justify-content: center;">
                                <img src="${fotoUrl}" alt="Foto ${item.namaLengkap}" style="width: 100%; height: 100%; object-fit: cover;" onerror="this.onerror=null;this.src='/images/no-image.png';" />
                            </div>
                            <div>
                                <div class="fw-bold text-indigo-dark mb-0.5 fs-6">${item.namaLengkap}</div>
                                <div class="small text-muted fw-semibold">${item.nik}</div>
                            </div>
                        </div>
                    </td>
                    <td class="py-3 text-secondary small">
                        <div class="d-flex align-items-center gap-2 fw-medium text-dark">
                            <i class="bi bi-building text-muted"></i> ${item.departmentName}
                        </div>
                    </td>
                    <td class="small py-3">
                        <div class="d-flex flex-column gap-1 text-secondary">
                            <div><i class="bi bi-envelope text-muted me-1"></i>${item.email}</div>
                            <div><i class="bi bi-telephone text-muted me-1"></i>${item.noTelepon}</div>
                        </div>
                    </td>                    
                    <td class="text-center">
                        ${statusBadge}
                    </td>
                    <td class="text-center pe-4">
                        <div class="d-flex justify-content-center gap-1">
                            <button class="btn btn-sm btn-action-light text-primary border-0 btn-edit" data-id="${item.id}" title="Ubah">
                                <i class="bi bi-pencil"></i>
                            </button>
                            <button class="btn btn-sm btn-action-light text-danger border-0 btn-delete" data-id="${item.id}" title="Hapus">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>
                    </td>
                </tr>`;
    });
  }
  $("#karyawanTableBody").html(html);
}

// Render Pagination Bootstrap (Dibatasi maksimal 5 tombol ke kiri & 5 ke kanan)
function renderPagination(pagedData) {
  let container = $("#paginationContainer");
  let info = $("#paginationInfo");

  if (!pagedData || pagedData.totalItems === 0) {
    info.html("");
    container.html("");
    return;
  }

  let start = (pagedData.pageNumber - 1) * pagedData.pageSize + 1;
  let end = Math.min(
    pagedData.pageNumber * pagedData.pageSize,
    pagedData.totalItems,
  );

  info.html(
    `Menampilkan <b>${start}</b> - <b>${end}</b> dari <b>${pagedData.totalItems}</b> data`,
  );

  let html = '<ul class="pagination pagination-sm mb-0 justify-content-end">';

  let prevDisabled = pagedData.pageNumber === 1 ? "disabled" : "";
  html += `<li class="page-item ${prevDisabled}"><a class="page-link" href="javascript:void(0)" onclick="loadKaryawanData(${
    pagedData.pageNumber - 1
  })">Previous</a></li>`;

  // Hitung batasan 5 ke kiri dan 5 ke kanan dari halaman aktif
  let startPage = Math.max(1, pagedData.pageNumber - 5);
  let endPage = Math.min(pagedData.totalPages, pagedData.pageNumber + 5);

  // Tampilkan halaman 1 dan indikator "..." jika rentang awal jauh dari halaman 1
  if (startPage > 1) {
    html += `<li class="page-item"><a class="page-link" href="javascript:void(0)" onclick="loadKaryawanData(1)">1</a></li>`;
    if (startPage > 2) {
      html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
    }
  }

  // Render nomor halaman dalam rentang limit
  for (let i = startPage; i <= endPage; i++) {
    let active = i === pagedData.pageNumber ? "active" : "";
    html += `<li class="page-item ${active}"><a class="page-link" href="javascript:void(0)" onclick="loadKaryawanData(${i})">${i}</a></li>`;
  }

  // Tampilkan halaman terakhir dan indikator "..." jika rentang akhir jauh dari totalPages
  if (endPage < pagedData.totalPages) {
    if (endPage < pagedData.totalPages - 1) {
      html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
    }
    html += `<li class="page-item"><a class="page-link" href="javascript:void(0)" onclick="loadKaryawanData(${pagedData.totalPages})">${pagedData.totalPages}</a></li>`;
  }

  let nextDisabled =
    pagedData.pageNumber >= pagedData.totalPages ? "disabled" : "";
  html += `<li class="page-item ${nextDisabled}"><a class="page-link" href="javascript:void(0)" onclick="loadKaryawanData(${
    pagedData.pageNumber + 1
  })">Next</a></li>`;

  html += "</ul>";
  container.html(html);
}

// Helper Validasi File Gambar (Max 2MB & Ekstensi Valid)
function validateImageFile(fileInputId, errorSpanId) {
  let fileInput = $(fileInputId)[0];
  let $errorSpan = $(errorSpanId);
  $errorSpan.text("").hide();

  if (fileInput && fileInput.files && fileInput.files[0]) {
    let file = fileInput.files[0];
    let fileSize = file.size;
    let fileName = file.name;
    let allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;

    if (!allowedExtensions.exec(fileName)) {
      $errorSpan
        .text(
          "Hanya file gambar dengan format .png, .jpg, atau .jpeg yang diizinkan!",
        )
        .show();
      return false;
    }

    if (fileSize > 2 * 1024 * 1024) {
      $errorSpan.text("Ukuran file gambar maksimal adalah 2MB!").show();
      return false;
    }
  }
  return true;
}

// Validasi jQuery saat nilai Select2 berubah
$(document).on("change", ".select2-dept-ajax", function () {
  if ($(this).valid) {
    $(this).valid();
  }
});
