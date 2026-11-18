$(document).ready(function () {
  let currentPage = 1;
  const pageSize = 5;

  // Load Data awal
  loadSupplierData(currentPage);

  // Search & Filter
  let searchTimer;
  $("#searchInput").on("keyup", function () {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function () {
      currentPage = 1;
      loadSupplierData(currentPage);
    }, 400);
  });

  $("#statusFilter").on("change", function () {
    currentPage = 1;
    loadSupplierData(currentPage);
  });

  // Load Data Ajax
  function loadSupplierData(page) {
    const search = $("#searchInput").val() || "";
    const status = $("#statusFilter").val() || "";

    $.ajax({
      url: "/Supplier/GetData",
      type: "GET",
      data: { search: search, status: status, page: page, pageSize: pageSize },
      success: function (res) {
        renderTable(res.items);
        renderPagination(res);
      },
      error: function () {
        toastr.error("Gagal mengambil data dari server!");
      },
    });
  }

  function renderTable(items) {
    const tbody = $("#supplierTableBody");
    tbody.empty();

    if (!items || items.length === 0) {
      tbody.append(`
                <tr>
                    <td colspan="5" class="text-center py-5 text-muted">
                        <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                        Belum ada data untuk ditampilkan !
                    </td>
                </tr>
            `);
      return;
    }

    items.forEach((item) => {
      const statusBadge = item.isAktif
        ? '<span class="badge-aktif px-3 py-2">Aktif</span>'
        : '<span class="badge bg-secondary px-3 py-2">Non-Aktif</span>';

      const row = `
                <tr class="border-bottom">
                    <td class="ps-4 py-3">
                        <div class="d-flex align-items-center gap-3">
                            <div class="icon-box flex-shrink-0" style="background-color: #ffe4e6; color: #f43f5e;">
                                <i class="bi bi-shop"></i>
                            </div>
                            <div>
                                <div class="fw-bold text-indigo-dark mb-0.5 fs-6">${item.namaSupplier}</div>
                                <div class="d-flex align-items-center gap-2 small">
                                    <span class="text-muted fw-semibold">${item.kodeSupplier || "-"}</span>
                                    <span class="text-muted">&#8226;</span>
                                    <span class="text-muted text-truncate" style="max-width: 220px;">${item.catatan || "-"}</span>
                                </div>
                            </div>
                        </div>
                    </td>
                    <td class="small py-3">
                        <div class="d-flex flex-column gap-1 text-secondary">
                            <div><i class="bi bi-envelope text-muted me-1.5"></i>${item.email}</div>
                            <div><i class="bi bi-telephone text-muted me-1.5"></i>${item.telepon}</div>
                            <div class="text-muted text-truncate" style="max-width: 250px;"><i class="bi bi-geo-alt me-1.5"></i>${item.alamat}</div>
                        </div>
                    </td>
                    <td class="small py-3">
                        <div class="fw-semibold text-indigo-dark mb-1"><i class="bi bi-person text-muted me-1.5"></i>${item.namaPic}</div>
                        <div class="text-muted">${item.whatsappPic}</div>
                    </td>
                    <td class="text-center">${statusBadge}</td>
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
                </tr>
            `;
      tbody.append(row);
    });
  }

  function renderPagination(res) {
    const container = $("#paginationContainer");
    container.empty();

    if (res.totalItems === 0) {
      $("#paginationInfo").text("Menampilkan 0 data");
      return;
    }

    const start = (res.currentPage - 1) * res.pageSize + 1;
    const end = Math.min(res.currentPage * res.pageSize, res.totalItems);
    $("#paginationInfo").text(
      `Menampilkan ${start} - ${end} dari ${res.totalItems} data`,
    );

    // Tombol Previous
    container.append(`
        <li class="page-item ${res.currentPage === 1 ? "disabled" : ""}">
            <a class="page-link page-prev" href="#" data-page="${res.currentPage - 1}">Previous</a>
        </li>
    `);

    // Hitung batasan 5 ke kiri dan 5 ke kanan dari halaman aktif
    let startPage = Math.max(1, res.currentPage - 5);
    let endPage = Math.min(res.totalPages, res.currentPage + 5);

    // Tampilkan halaman 1 dan indikator "..." jika rentang awal jauh dari halaman 1
    if (startPage > 1) {
      container.append(`
        <li class="page-item">
            <a class="page-link page-num" href="#" data-page="1">1</a>
        </li>
      `);
      if (startPage > 2) {
        container.append(`
          <li class="page-item disabled">
              <span class="page-link">...</span>
          </li>
        `);
      }
    }

    // Render nomor halaman dalam rentang limit
    for (let i = startPage; i <= endPage; i++) {
      container.append(`
        <li class="page-item ${i === res.currentPage ? "active" : ""}">
            <a class="page-link page-num" href="#" data-page="${i}">${i}</a>
        </li>
      `);
    }

    // Tampilkan halaman terakhir dan indikator "..." jika rentang akhir jauh dari totalPages
    if (endPage < res.totalPages) {
      if (endPage < res.totalPages - 1) {
        container.append(`
          <li class="page-item disabled">
              <span class="page-link">...</span>
          </li>
        `);
      }
      container.append(`
        <li class="page-item">
            <a class="page-link page-num" href="#" data-page="${res.totalPages}">${res.totalPages}</a>
        </li>
      `);
    }

    // Tombol Next
    container.append(`
        <li class="page-item ${res.currentPage === res.totalPages ? "disabled" : ""}">
            <a class="page-link page-next" href="#" data-page="${res.currentPage + 1}">Next</a>
        </li>
    `);
  }

  $(document).on("click", ".page-link", function (e) {
    e.preventDefault();
    const page = $(this).data("page");
    if (page && page !== currentPage) {
      currentPage = page;
      loadSupplierData(currentPage);
    }
  });

  // ==========================================
  // VALIDASI form create dan edit
  // ==========================================
  function validateFormManual($form) {
    let isValid = true;
    $form
      .find("input[data-val-required], textarea[data-val-required]")
      .each(function () {
        const val = $(this).val() ? $(this).val().trim() : "";
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

  // Otomatis hapus garis merah saat diketik/diisi
  $(document).on("input change", "input, textarea", function () {
    $(this).removeClass("is-invalid");
    const fieldName = $(this).attr("name");
    if (fieldName) {
      $(`span[data-valmsg-for="${fieldName}"]`).text("").hide();
    }
  });

  // --- CREATE SUPPLIER ---
  function submitCreateSupplier() {
    const $form = $("#formTambahSupplier");

    if (!validateFormManual($form)) {
      toastr.warning("Mohon lengkapi field wajib yang belum diisi!");
      return false;
    }

    const data = {
      KodeSupplier: $("#createKodeSupplier").val(),
      NamaSupplier: $("#createNamaSupplier").val(),
      Email: $("#createEmail").val(),
      Telepon: $("#createTelepon").val(),
      NamaPic: $("#createNamaPic").val(),
      WhatsappPic: $("#createWhatsappPic").val(),
      Alamat: $("#createAlamat").val(),
      Catatan: $("#createCatatan").val(),
      IsAktif: $("#checkSupplierAktif").is(":checked"),
    };

    $.ajax({
      url: "/Supplier/Create",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(data),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message || "Supplier berhasil disimpan!");
          $("#modalTambahSupplier").modal("hide");
          resetForm("#formTambahSupplier");
          loadSupplierData(1);
        } else {
          toastr.warning(res.message || "Gagal menyimpan data.");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan saat menyimpan data!");
      },
    });
  }

  $(document).on("click", "#btnSimpanSupplier", function (e) {
    e.preventDefault();
    submitCreateSupplier();
  });

  $("#formTambahSupplier").on("submit", function (e) {
    e.preventDefault();
    submitCreateSupplier();
  });

  // --- EDIT SUPPLIER ---
  $(document).on("click", ".btn-edit", function () {
    const id = $(this).data("id");
    resetForm("#formEditSupplier");

    $.get("/Supplier/GetById/" + id, function (res) {
      if (res.success) {
        const item = res.data;
        $("#editId").val(item.id);
        $("#editKodeSupplier").val(item.kodeSupplier);
        $("#editNamaSupplier").val(item.namaSupplier);
        $("#editEmail").val(item.email);
        $("#editTelepon").val(item.telepon);
        $("#editNamaPic").val(item.namaPic);
        $("#editWhatsappPic").val(item.whatsappPic);
        $("#editAlamat").val(item.alamat);
        $("#editCatatan").val(item.catatan);
        $("#checkEditSupplierAktif").prop("checked", item.isAktif);

        $("#modalEditSupplier").modal("show");
      } else {
        toastr.error(res.message);
      }
    });
  });

  function submitUpdateSupplier() {
    const $form = $("#formEditSupplier");

    if (!validateFormManual($form)) {
      toastr.warning("Mohon lengkapi field wajib yang belum diisi!");
      return false;
    }

    const data = {
      Id: $("#editId").val(),
      KodeSupplier: $("#editKodeSupplier").val(),
      NamaSupplier: $("#editNamaSupplier").val(),
      Email: $("#editEmail").val(),
      Telepon: $("#editTelepon").val(),
      NamaPic: $("#editNamaPic").val(),
      WhatsappPic: $("#editWhatsappPic").val(),
      Alamat: $("#editAlamat").val(),
      Catatan: $("#editCatatan").val(),
      IsAktif: $("#checkEditSupplierAktif").is(":checked"),
    };

    $.ajax({
      url: "/Supplier/Edit",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(data),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message || "Data berhasil diperbarui!");
          $("#modalEditSupplier").modal("hide");
          loadSupplierData(currentPage);
        } else {
          toastr.warning(res.message || "Gagal memperbarui data.");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan saat memperbarui data!");
      },
    });
  }

  $(document).on("click", "#btnUpdateSupplier", function (e) {
    e.preventDefault();
    submitUpdateSupplier();
  });

  $("#formEditSupplier").on("submit", function (e) {
    e.preventDefault();
    submitUpdateSupplier();
  });

  // --- DELETE SUPPLIER ---
  $(document).on("click", ".btn-delete", function () {
    const id = $(this).data("id");
    $("#deleteId").val(id);
    $("#modalHapusSupplier").modal("show");
  });

  $("#btnConfirmDelete").on("click", function () {
    const id = $("#deleteId").val();
    $.post("/Supplier/Delete/" + id, function (res) {
      if (res.success) {
        toastr.success(res.message);
        $("#modalHapusSupplier").modal("hide");
        loadSupplierData(currentPage);
      } else {
        toastr.error(res.message);
      }
    });
  });

  // Reset Modal Form & Validation
  $("#modalTambahSupplier, #modalEditSupplier").on(
    "hidden.bs.modal",
    function () {
      const $form = $(this).find("form");
      if ($form.length) {
        $form[0].reset();
        $form.find(".is-invalid").removeClass("is-invalid");
        $form.find("span[data-valmsg-for]").text("").hide();
      }
    },
  );

  function resetForm(selector) {
    const $form = $(selector);
    if ($form.length > 0) {
      $form[0].reset();
      $form.find(".is-invalid").removeClass("is-invalid");
      $form.find("span[data-valmsg-for]").text("").hide();
    }
  }
});
