/**
 * File Pengelola CRUD & AJAX Kategori
 * Tempat: wwwroot/js/kategori.js
 */

$(document).ready(function () {
  let currentPage = 1;
  const pageSize = 10;
  let selectedDeleteId = null;

  // Load data awal saat halaman pertama kali dibuka
  loadKategoriTableData(currentPage);

  // ==========================================
  // 1. FILTER & SEARCH (DEBOUNCE INPUT)
  // ==========================================
  let searchTimer;
  $("#filterSearchKategori").on("keyup", function () {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function () {
      currentPage = 1;
      loadKategoriTableData(currentPage);
    }, 400);
  });

  // ==========================================
  // 2. FETCH & RENDER TABLE (AJAX & PAGINATION)
  // ==========================================
  function loadKategoriTableData(page) {
    const search = $("#filterSearchKategori").val();

    $.ajax({
      url: "/Kategori/GetData",
      type: "GET",
      data: { search: search, page: page, pageSize: pageSize },
      success: function (res) {
        if (res.success) {
          renderTable(res.data.items, res.data.pageNumber, res.data.pageSize);
          renderPagination(res.data);
          $("#totalKategoriCount").text(res.data.totalItems);
        } else {
          toastr.error("Gagal memuat data kategori!");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem saat memuat data kategori.");
      },
    });
  }

  // Render HTML Baris Tabel dengan Fallback jika data kosong
  function renderTable(items, pageNumber, pageSize) {
    const tbody = $("#kategoriTableBody");
    tbody.empty();

    // Fallback jika data masih kosong
    if (!items || items.length === 0) {
      tbody.append(`
                <tr>
                    <td colspan="4" class="text-center py-5 text-muted">
                        <i class="bi bi-inbox fs-1 d-block mb-2 text-secondary"></i>
                        <span class="fw-semibold">Belum ada data untuk ditampilkan !</span>
                    </td>
                </tr>
            `);
      return;
    }

    let no = (pageNumber - 1) * pageSize + 1;
    items.forEach((item) => {
      const tr = `
                <tr class="border-bottom">
                    <td class="ps-4 py-3 fw-medium text-muted">${no++}</td>
                    <td class="py-3">
                        <span class="fw-bold text-indigo-dark fs-6">${escapeHtml(item.namaKategori)}</span>
                    </td>
                    <td class="py-3 text-secondary">
                        ${item.deskripsi ? escapeHtml(item.deskripsi) : '<em class="text-muted small">- Tidak ada deskripsi -</em>'}
                    </td>
                    <td class="text-center pe-4">
                        <div class="d-flex justify-content-center gap-2">
                            <button class="btn btn-sm btn-light text-primary border-0 btn-edit-kategori" data-id="${item.id}" title="Edit Kategori">
                                <i class="bi bi-pencil"></i>
                            </button>
                            <button class="btn btn-sm btn-light text-danger border-0 btn-delete-kategori" data-id="${item.id}" title="Hapus Kategori">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>
                    </td>
                </tr>
            `;
      tbody.append(tr);
    });
  }

  // Render Komponen Pagination Bootstrap
  function renderPagination(data) {
    const nav = $("#paginationNavKategori");
    nav.empty();

    if (data.totalPages <= 1) {
      $("#paginationInfoKategori").text(`Menampilkan ${data.totalItems} data`);
      return;
    }

    const startItem = (data.pageNumber - 1) * data.pageSize + 1;
    const endItem = Math.min(data.pageNumber * data.pageSize, data.totalItems);
    $("#paginationInfoKategori").text(
      `Menampilkan ${startItem}-${endItem} dari ${data.totalItems} data`,
    );

    // Tombol Previous
    nav.append(`
            <li class="page-item ${data.pageNumber === 1 ? "disabled" : ""}">
                <a class="page-link" href="#" data-page="${data.pageNumber - 1}">Previous</a>
            </li>
        `);
    let startPage = Math.max(1, data.pageNumber - 5);
    let endPage = Math.min(data.totalPages, data.pageNumber + 5);
    if (startPage > 1) {
      nav.append(`
                <li class="page-item">
                    <a class="page-link" href="#" data-page="1">1</a>
                </li>
            `);
      if (startPage > 2) {
        nav.append(`
                    <li class="page-item disabled">
                        <span class="page-link">...</span>
                    </li>
                `);
      }
    }

    // Render nomor halaman dalam rentang limit
    for (let i = startPage; i <= endPage; i++) {
      nav.append(`
                <li class="page-item ${i === data.pageNumber ? "active" : ""}">
                    <a class="page-link" href="#" data-page="${i}">${i}</a>
                </li>
            `);
    }
    if (endPage < data.totalPages) {
      if (endPage < data.totalPages - 1) {
        nav.append(`
                    <li class="page-item disabled">
                        <span class="page-link">...</span>
                    </li>
                `);
      }
      nav.append(`
                <li class="page-item">
                    <a class="page-link" href="#" data-page="${data.totalPages}">${data.totalPages}</a>
                </li>
            `);
    }

    // Tombol Next
    nav.append(`
            <li class="page-item ${data.pageNumber === data.totalPages ? "disabled" : ""}">
                <a class="page-link" href="#" data-page="${data.pageNumber + 1}">Next</a>
            </li>
        `);
  }

  // Dynamic Event Click Pagination
  $(document).on("click", "#paginationNavKategori .page-link", function (e) {
    e.preventDefault();
    const page = $(this).data("page");
    if (page && page !== currentPage) {
      currentPage = page;
      loadKategoriTableData(currentPage);
    }
  });

  // ==========================================
  // 3. RE-BIND UNOBTRUSIVE VALIDATION (SAFE FALLBACK)
  // ==========================================
  function rebindValidation($form) {
    if (
      typeof $.validator !== "undefined" &&
      typeof $.validator.unobtrusive !== "undefined"
    ) {
      $form.removeData("validator").removeData("unobtrusiveValidation");
      $.validator.unobtrusive.parse($form);
    }
  }

  // Fallback Manual Validator jika unobtrusive gagal/tidak ter-load
  function validateFormManual($form) {
    let isValid = true;
    $form
      .find("input[data-val-required], textarea[data-val-required]")
      .each(function () {
        const val = $(this).val().trim();
        const fieldName = $(this).attr("name");
        const $errorSpan = $form.find(`span[data-valmsg-for="${fieldName}"]`);

        if (val === "") {
          isValid = false;
          $(this).addClass("is-invalid");
          if ($errorSpan.length) {
            $errorSpan.text("Field ini wajib diisi!").show();
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

  // Remove inline error highlight saat diketik
  $(document).on("input", "input, textarea", function () {
    $(this).removeClass("is-invalid");
  });

  // Reset Form & Validation State saat Modal Ditutup
  $("#modalTambahKategori, #modalEditKategori").on(
    "hidden.bs.modal",
    function () {
      const $form = $(this).find("form");
      $form[0].reset();
      $form.find(".is-invalid").removeClass("is-invalid");
      $form.find("span[data-valmsg-for]").text("");
    },
  );

  // Trigger re-bind validation saat modal dibuka
  $("#modalTambahKategori").on("shown.bs.modal", function () {
    rebindValidation($("#formTambahKategori"));
  });

  $("#modalEditKategori").on("shown.bs.modal", function () {
    rebindValidation($("#formEditKategori"));
  });

  // ==========================================
  // 4. CREATE KATEGORI (PROSES SIMPAN)
  // ==========================================
  $("#formTambahKategori").on("submit", function (e) {
    e.preventDefault();
    const $form = $(this);

    rebindValidation($form);

    // Jalankan Validasi Unobtrusive / Manual Fallback
    if ($form.valid && !$form.valid()) {
      toastr.warning("Mohon lengkapi field wajib yang belum diisi!");
      return false;
    } else if (!validateFormManual($form)) {
      toastr.warning("Mohon lengkapi field wajib yang belum diisi!");
      return false;
    }

    const formData = $form.serialize();

    $.ajax({
      url: "/Kategori/Create",
      type: "POST",
      data: formData,
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalTambahKategori").modal("hide");

          // Langsung reload data ke index
          currentPage = 1;
          loadKategoriTableData(currentPage);
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Gagal memproses permintaan!");
      },
    });
  });

  // ==========================================
  // 5. EDIT KATEGORI (FETCH & UPDATE)
  // ==========================================
  $(document).on("click", ".btn-edit-kategori", function () {
    const id = $(this).data("id");

    $.ajax({
      url: `/Kategori/GetById?id=${id}`,
      type: "GET",
      success: function (res) {
        if (res.success && res.data) {
          const data = res.data;
          $("#edit_Id").val(data.id);
          $("#edit_NamaKategori").val(data.namaKategori || "");
          $("#edit_Deskripsi").val(data.deskripsi || "");

          $("#modalEditKategori").modal("show");
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Gagal mengambil detail kategori!");
      },
    });
  });

  $("#formEditKategori").on("submit", function (e) {
    e.preventDefault();
    const $form = $(this);

    rebindValidation($form);

    if ($form.valid && !$form.valid()) {
      toastr.warning("Mohon lengkapi field wajib!");
      return false;
    } else if (!validateFormManual($form)) {
      toastr.warning("Mohon lengkapi field wajib!");
      return false;
    }

    const formData = $form.serialize();

    $.ajax({
      url: "/Kategori/Edit",
      type: "POST",
      data: formData,
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalEditKategori").modal("hide");

          // Langsung reload data ke index
          loadKategoriTableData(currentPage);
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Gagal memperbarui data!");
      },
    });
  });

  // ==========================================
  // 6. DELETE KATEGORI (CONFIRMATION & ACTION)
  // ==========================================
  $(document).on("click", ".btn-delete-kategori", function () {
    selectedDeleteId = $(this).data("id");
    $("#modalHapusKategori").modal("show");
  });

  $("#btnConfirmDeleteKategori").on("click", function () {
    if (!selectedDeleteId) return;

    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
      url: "/Kategori/Delete",
      type: "POST",
      data: {
        id: selectedDeleteId,
        __RequestVerificationToken: token,
      },
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalHapusKategori").modal("hide");

          // Reload table & penyesuaian halaman
          loadKategoriTableData(currentPage);
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Gagal menghapus data kategori!");
      },
    });
  });

  // ==========================================
  // 7. HELPER SANITIZE HTML STRING
  // ==========================================
  function escapeHtml(text) {
    if (!text) return "";
    return text
      .toString()
      .replace(/&/g, "&amp;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;")
      .replace(/"/g, "&quot;")
      .replace(/'/g, "&#039;");
  }
});
