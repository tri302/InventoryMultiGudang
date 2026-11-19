$(document).ready(function () {
  // 1. Inisialisasi Filter Kategori di Halaman Utama
  $("#filterKategori").select2({
    theme: "bootstrap-5",
    placeholder: "Semua Kategori",
    allowClear: true,
    width: "100%",
    ajax: {
      url: "/Produk/GetDropdownKategori",
      dataType: "json",
      delay: 300,
      data: function (params) {
        return { q: params.term || "" };
      },
      processResults: function (data) {
        return {
          results: (data.results || []).map(function (item) {
            return { id: item.id, text: item.text };
          })
        };
      },
      cache: true
    },
    minimumInputLength: 0
  });

  // Real-time Search untuk Input Produk (#filterSearch)
  let searchTimer;
  $("#filterSearch").on("keyup input", function () {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(function () {
      currentPage = 1;
      loadTableData(currentPage);
    }, 400);
  });

  $("#filterKategori").on("change", function () {
    currentPage = 1;
    loadTableData(currentPage);
  });

  // Real-time clear error untuk input biasa / select standar
  $(document).on(
    "input change",
    "#formTambahProduk input, #formTambahProduk select, #formTambahProduk textarea, #formEditProduk input, #formEditProduk select, #formEditProduk textarea",
    function () {
      const $field = $(this);
      const name = $field.attr("name");
      if ($field.val()) {
        $field.removeClass("is-invalid");
        $(`#error-create-${name}, #error-edit-${name}`).text("");
      }
    },
  );

  // Toastr Configurations
  toastr.options = {
    closeButton: true,
    progressBar: true,
    positionClass: "toast-top-right",
    timeOut: "3000",
  };

  // EVENT: Klik tombol "Lihat" Serial Number
  $(document).on("click", ".btn-view-sn", function () {
    const produkId = $(this).data("id");
    const tbody = $("#snTableBody");

    tbody.html(`
        <tr>
            <td colspan="4" class="text-center py-4 text-muted">
                <div class="spinner-border spinner-border-sm me-2 text-primary" role="status"></div>
                Memuat data serial number...
            </td>
        </tr>
    `);

    $("#modalRiwayatProduk").modal("show");

    $.ajax({
      url: `/Produk/GetSerialNumbersByProduk?produkId=${produkId}`,
      type: "GET",
      success: function (res) {
        if (res.success) {
          $("#snModalSubTitle").text(`${res.namaProduk} (${res.sku})`);
          renderSNTable(res.items);
        } else {
          tbody.html(`<tr><td colspan="4" class="text-center py-3 text-danger">${res.message}</td></tr>`);
        }
      },
      error: function () {
        tbody.html(`<tr><td colspan="4" class="text-center py-3 text-danger">Gagal memuat data dari server.</td></tr>`);
      },
    });
  });

  function renderSNTable(items) {
    const tbody = $("#snTableBody");
    tbody.empty();

    if (!items || items.length === 0) {
      tbody.append(`
            <tr>
                <td colspan="5" class="text-center py-4 text-muted">
                    <i class="bi bi-qr-code fs-3 d-block mb-1"></i>
                    Belum ada data Serial Number untuk produk ini.
                </td>
            </tr>
        `);
      updateSNBadges([]);
      return;
    }

    items.forEach((item, index) => {
      let badgeClass = "bg-secondary text-white";
      const statusLower = (item.status || "").toLowerCase();
      let displayStatus = item.status;

      if (statusLower === "dalam penugasan" || statusLower === "assigned") {
        displayStatus = "Assigned";
        badgeClass = "bg-primary text-white";
      } else if (statusLower === "dipinjam" || statusLower === "loaned") {
        displayStatus = "Loaned";
        badgeClass = "bg-warning text-dark";
      } else if (statusLower === "in stock" || statusLower === "tersedia") {
        displayStatus = "In Stock";
        badgeClass = "bg-success text-white";
      } else if (statusLower === "repair" || statusLower === "perbaikan") {
        displayStatus = "Repair";
        badgeClass = "bg-danger text-white";
      } else if (statusLower === "lost" || statusLower === "hilang") {
        displayStatus = "Lost";
        badgeClass = "bg-dark text-white";
      }

      const tr = `
            <tr>
                <td class="ps-3 fw-bold text-muted">${index + 1}</td>
                <td class="fw-bold text-indigo-dark">${escapeHtml(item.serialNumber)}</td>
                <td class="small text-muted">
                    <i class="bi bi-geo-alt me-1"></i>${escapeHtml(item.namaGudang)} / ${escapeHtml(item.namaRak)}
                </td>
                <td class="text-center">
                    <span class="badge ${badgeClass} px-2 py-1 fs-8">${escapeHtml(displayStatus)}</span>
                </td>
                <td class="text-center">
                    <a href="/Produk/PrintQrLabel?produkItemId=${item.id}" target="_blank" class="btn btn-sm btn-outline-secondary py-0 px-2" title="Print Label QR">
                        <i class="bi bi-printer"></i>
                    </a>
                </td>
            </tr>
        `;
      tbody.append(tr);
    });

    updateSNBadges(items);
  }

  function updateSNBadges(items) {
    const total = items.length;
    const countInStock = items.filter(x => (x.status || "").toLowerCase() === "in stock" || (x.status || "").toLowerCase() === "tersedia").length;
    const countAssigned = items.filter(x => (x.status || "").toLowerCase() === "assigned" || (x.status || "").toLowerCase().includes("penugasan")).length;
    const countLoaned = items.filter(x => (x.status || "").toLowerCase() === "loaned" || (x.status || "").toLowerCase() === "dipinjam").length;
    const countRepair = items.filter(x => (x.status || "").toLowerCase() === "repair" || (x.status || "").toLowerCase() === "perbaikan").length;
    const countLost = items.filter(x => (x.status || "").toLowerCase() === "lost" || (x.status || "").toLowerCase() === "hilang").length;

    $("#sumTotal").text(`Total: ${total} Unit`);
    $("#sumInStock").text(`${countInStock} In Stock`);
    $("#sumAssigned").text(`${countAssigned} Assigned`);
    $("#sumLoaned").text(`${countLoaned} Loaned`);
    $("#sumRepair").text(`${countRepair} Repair`);
    $("#sumLost").text(`${countLost} Lost`);
  }

  let currentPage = 1;
  const pageSize = 10;
  let selectedDeleteId = null;

  function formatRupiah(angka) {
    if (angka === undefined || angka === null || angka === "") return "";
    let numberString = angka.toString().replace(/[^,\d]/g, "");
    let split = numberString.split(",");
    let sisa = split[0].length % 3;
    let rupiah = split[0].substr(0, sisa);
    let ribuan = split[0].substr(sisa).match(/\d{3}/gi);

    if (ribuan) {
      let separator = sisa ? "." : "";
      rupiah += separator + ribuan.join(".");
    }

    return split[1] !== undefined ? rupiah + "," + split[1] : rupiah;
  }

  function cleanRupiah(rupiahString) {
    if (!rupiahString) return "0";
    return rupiahString.toString().replace(/\./g, "").replace(",", ".");
  }

  $(document).on("keyup input", ".input-rupiah", function () {
    $(this).val(formatRupiah($(this).val()));
  });

  // ==========================================
  // EVENT SAAT MODAL DIBUKA (SHOWN)
  // ==========================================
  $("#modalTambahProduk").on("shown.bs.modal", function () {
    initSelect2Kategori($("#addKategori"), "#modalTambahProduk");
    initSelect2Uom($("#addUomId"), "#modalTambahProduk");
    setupGudangRakDropdown($("#addGudang"), $("#addRak"), "#modalTambahProduk");
  });

  $("#modalEditProduk").on("shown.bs.modal", function () {
    initSelect2Kategori($("#edit_Kategori"), "#modalEditProduk");
    initSelect2Uom($("#editUomId"), "#modalEditProduk");
    setupGudangRakDropdown($("#editGudang"), $("#editRak"), "#modalEditProduk");
  });

  // EVENT SAAT MODAL DITUTUP (HIDDEN) - Reset Form & Clear Error Tanpa Destroy Paksa
  $("#modalTambahProduk").on("hidden.bs.modal", function () {
    $("#formTambahProduk")[0].reset();
    clearFormErrors("create");
    $("#addGudang, #addRak, #addKategori, #addUomId").val(null).trigger("change");
  });

  $("#modalEditProduk").on("hidden.bs.modal", function () {
    $("#formEditProduk")[0].reset();
    clearFormErrors("edit");
    $("#editGudang, #editRak, #edit_Kategori, #editUomId").val(null).trigger("change");
  });

  loadTableData(currentPage);

  function loadTableData(page) {
    const search = $("#filterSearch").val();
    const kategori = $("#filterKategori").val();

    $.ajax({
      url: "/Produk/GetData",
      type: "GET",
      data: { search: search, kategori: kategori, page: page, pageSize: pageSize },
      success: function (res) {
        if (res.success) {
          renderTable(res.data.items);
          renderPagination(res.data);
          $("#totalDataCount").text(res.data.totalItems);
        } else {
          toastr.error("Gagal memuat data!");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem saat memuat data.");
      },
    });
  }

  function renderTable(items) {
    const tbody = $("#produkTableBody");
    tbody.empty();

    if (!items || items.length === 0) {
      tbody.append(`
                <tr>
                    <td colspan="7" class="text-center py-5 text-muted">
                        <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                        <span class="fw-semibold">Belum ada data untuk ditampilkan !</span>
                    </td>
                </tr>
            `);
      return;
    }

    items.forEach((item) => {
      const isSnTracked = item.pelacakanBarang && item.pelacakanBarang.includes("Pelacakan Serial Number (SN)");
      const pelacakanBadgeHtml = isSnTracked
        ? `
        <span class="text-[9px] px-1.5 py-0.5 rounded bg-purple-50 border border-purple-250 text-purple-700 font-bold uppercase inline-flex items-center gap-1">            
            Pelacakan Serial Number (SN)
            <button class="ml-1 px-1 py-0.2 bg-purple-200 hover:bg-purple-300 text-purple-800 rounded text-[8px] font-extrabold cursor-pointer border-none uppercase transition-colors btn-view-sn" data-id="${item.id}">
                Lihat
            </button>
        </span>
      `
        : `
        <span class="text-[9px] px-1.5 py-0.5 rounded bg-slate-100 border border-slate-200 text-slate-600 font-bold uppercase inline-flex items-center gap-1">
            Tanpa Serial Number (Bulk)
        </span>
      `;

      const hargaBeliFormatted = new Intl.NumberFormat("id-ID", { style: "currency", currency: "IDR", maximumFractionDigits: 0 }).format(item.hargaBeli);
      const hargaPerolehanFormatted = new Intl.NumberFormat("id-ID", { style: "currency", currency: "IDR", maximumFractionDigits: 0 }).format(item.hargaPerolehan);
      const isAsset = item.tipeBarang && item.tipeBarang.includes("Asset");

      const tr = `
                <tr class="border-bottom">
                    <td class="ps-4 py-3">
                        <div class="d-flex align-items-center gap-3">
                            <div class="icon-box flex-shrink-0" style="background-color: #fff1f2; color: #f43f5e;">
                                <i class="bi bi-box-seam-fill"></i>
                            </div>
                            <div>
                                <div class="fw-bold text-indigo-dark mb-1 fs-6">${escapeHtml(item.namaProduk)}</div>
                                <div class="d-flex flex-wrap align-items-center gap-2" style="font-size: 0.72rem;">
                                    <span class="text-muted fw-medium">${escapeHtml(item.sku)}</span>
                                    <span class="text-muted">&#8226;</span>
                                    <span class="badge text-uppercase ${isAsset ? "text-indigo bg-light" : "text-success bg-light"} px-2 py-1 fw-bold">${escapeHtml(item.tipeBarang)}</span>                                                                        
                                    ${pelacakanBadgeHtml}                                    
                                </div>
                            </div>
                        </div>
                    </td>
                    <td>
                        <span class="badge text-indigo-dark bg-light px-3 py-2 fw-semibold d-inline-flex align-items-center gap-1" style="border-radius: 20px;">
                            <i class="bi bi-tag text-muted"></i> ${escapeHtml(item.kategori)}
                        </span>
                    </td>
                    <td class="text-end pe-4 fw-medium text-secondary">${hargaBeliFormatted}</td>
                    <td class="text-end pe-4 fw-bold text-indigo-dark">${hargaPerolehanFormatted}</td>
                    <td class="text-center">
                        <span class="badge bg-light text-secondary border px-2 py-1 fw-semibold">${escapeHtml(item.satuan)}</span>
                    </td>
                    <td class="text-center">
                        <span class="badge-aktif d-inline-block px-3 py-2 fs-7">${item.stok || 0} Unit</span>
                    </td>
                    <td class="text-center pe-4">
                        <div class="d-flex justify-content-center gap-2">
                            <button class="btn btn-sm btn-light text-primary border-0 btn-edit" data-id="${item.id}" title="Ubah"><i class="bi bi-pencil"></i></button>
                            <button class="btn btn-sm btn-light text-danger border-0 btn-delete" data-id="${item.id}" title="Hapus"><i class="bi bi-trash"></i></button>
                        </div>
                    </td>
                </tr>
            `;
      tbody.append(tr);
    });
  }

  function renderPagination(data) {
    const nav = $("#paginationNav");
    nav.empty();

    if (data.totalPages <= 1) {
      $("#paginationInfo").text(`Menampilkan ${data.totalItems} data`);
      return;
    }

    const startItem = (data.pageNumber - 1) * data.pageSize + 1;
    const endItem = Math.min(data.pageNumber * data.pageSize, data.totalItems);
    $("#paginationInfo").text(`Menampilkan ${startItem}-${endItem} dari ${data.totalItems} data`);

    // Tombol Previous
    nav.append(`
      <li class="page-item ${data.pageNumber === 1 ? "disabled" : ""}">
        <a class="page-link" href="#" data-page="${data.pageNumber - 1}">Previous</a>
      </li>
    `);

    // Hitung batasan 5 ke kiri dan 5 ke kanan dari halaman aktif
    let startPage = Math.max(1, data.pageNumber - 5);
    let endPage = Math.min(data.totalPages, data.pageNumber + 5);

    // Tampilkan halaman 1 dan indikator "..." jika rentang awal jauh dari halaman 1
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

    // Tampilkan halaman terakhir dan indikator "..." jika rentang akhir jauh dari totalPages
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

  $(document).on("click", "#paginationNav .page-link", function (e) {
    e.preventDefault();
    const page = $(this).data("page");
    if (page && page !== currentPage) {
      currentPage = page;
      loadTableData(currentPage);
    }
  });

  // CREATE SUBMIT
  $("#formTambahProduk").on("submit", function (e) {
    e.preventDefault();
    clearFormErrors("create");

    let formData = $(this).serializeArray().map((item) => {
      if (item.name === "HargaBeli" || item.name === "HargaPerolehan") {
        item.value = cleanRupiah(item.value);
      }
      return item;
    });

    const token = $('input[name="__RequestVerificationToken"]').val();
    if (token) formData.push({ name: "__RequestVerificationToken", value: token });

    $.ajax({
      url: "/Produk/Create",
      type: "POST",
      data: $.param(formData),
      success: function (response) {
        if (response.success) {
          toastr.success(response.message, "success");
          $("#modalTambahProduk").modal("hide");
          $("#formTambahProduk")[0].reset();
          loadTableData(currentPage);
        } else {
          toastr.error(response.message || "Validasi gagal!");
          if (response.errors) {
            $.each(response.errors, function (key, messages) {
              let $input = $(`[name="${key}"]`);
              let $errorSpan = $(`#error-create-${key}`);
              if ($input.hasClass("select2-hidden-accessible")) {
                $input.next(".select2-container").find(".select2-selection").addClass("is-invalid border-danger");
              } else {
                $input.addClass("is-invalid");
              }
              if ($errorSpan.length) $errorSpan.text(messages[0]);
            });
          }
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem saat menyimpan data.");
      },
    });
  });

  // EDIT GET BY ID
  $(document).on("click", ".btn-edit", function () {
    const id = $(this).data("id");

    $.ajax({
      url: `/Produk/GetById?id=${id}`,
      type: "GET",
      success: function (res) {
        if (res.success && res.data) {
          const data = res.data;

          $("#edit_Id").val(data.id);
          $("#edit_Sku").val(data.sku || "");
          $("#edit_NamaProduk").val(data.namaProduk || "");
          $("#edit_PelacakanBarang").val(data.pelacakanBarang || "");

          // Gudang
          if (data.gudangId && data.namaGudang) {
            $("#editGudang").empty().append(new Option(data.namaGudang, data.gudangId, true, true)).trigger("change");
          } else {
            $("#editGudang").val(null).trigger("change");
          }

          // Rak
          const isVirtual = data.namaGudang && data.namaGudang.toLowerCase().includes("gudang transit virtual");
          if (isVirtual || !data.rakId) {
            $("#editRak").val(null).trigger("change").prop("disabled", true);
          } else if (data.rakId && data.namaRak) {
            $("#editRak").prop("disabled", false).empty().append(new Option(data.namaRak, data.rakId, true, true)).trigger("change");
          } else {
            $("#editRak").val(null).trigger("change");
          }

          // Kategori
          if (data.kategori) {
            $("#edit_Kategori").empty().append(new Option(data.kategori, data.kategori, true, true)).trigger("change");
          } else {
            $("#edit_Kategori").val(null).trigger("change");
          }

          // UOM (Satuan)
          if (data.uomId) {
            const uomText = data.namaUom || data.satuan || data.uomId;
            $("#editUomId").empty().append(new Option(uomText, data.uomId, true, true)).trigger("change");
          } else {
            $("#editUomId").val(null).trigger("change");
          }

          $("#edit_Merek").val(data.merek || "");
          $("#edit_Stok").val(data.stok || 0);
          $("#edit_TipeBarang").val(data.tipeBarang || "Inventory (Komersial)");
          $("#edit_HargaBeli").val(formatRupiah(data.hargaBeli || 0));
          $("#edit_HargaPerolehan").val(formatRupiah(data.hargaPerolehan || 0));
          $("#edit_Deskripsi").val(data.deskripsi || "");

          $("#modalEditProduk").modal("show");
        } else {
          toastr.error(res.message);
        }
      },
    });
  });

  // EDIT SUBMIT - Menggunakan Event Delegation agar aman dari un-wireup
  $(document).on("submit", "#formEditProduk", function (e) {
    e.preventDefault();
    clearFormErrors("edit");

    let formData = $(this).serializeArray().map((item) => {
      if (item.name === "HargaBeli" || item.name === "HargaPerolehan") {
        item.value = cleanRupiah(item.value);
      }
      return item;
    });

    const token = $('input[name="__RequestVerificationToken"]').val();
    if (token) formData.push({ name: "__RequestVerificationToken", value: token });

    $.ajax({
      url: "/Produk/Edit",
      type: "POST",
      data: $.param(formData),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalEditProduk").modal("hide");
          loadTableData(currentPage);
        } else {
          toastr.error(res.message || "Validasi gagal!");
          if (res.errors) {
            $.each(res.errors, function (key, messages) {
              let $input = $(`#formEditProduk [name="${key}"]`);
              let $errorSpan = $(`#error-edit-${key}`);
              if ($input.hasClass("select2-hidden-accessible")) {
                $input.next(".select2-container").find(".select2-selection").addClass("is-invalid border-danger");
              } else {
                $input.addClass("is-invalid");
              }
              if ($errorSpan.length) $errorSpan.text(messages[0]);
            });
          }
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem saat memperbarui data.");
      },
    });
  });

  // DELETE
  $(document).on("click", ".btn-delete", function () {
    selectedDeleteId = $(this).data("id");
    $("#modalHapusProduk").modal("show");
  });

  $(document).on("click", "#modalHapusProduk .btn-delete-modal", function () {
    if (!selectedDeleteId) return;

    $.ajax({
      url: "/Produk/Delete",
      type: "POST",
      data: {
        id: selectedDeleteId,
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
      },
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalHapusProduk").modal("hide");
          loadTableData(currentPage);
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Gagal menghapus data!");
      },
    });
  });

  function escapeHtml(text) {
    if (!text) return "";
    return text.toString().replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&#039;");
  }
});

// ==========================================
// INISIALISASI SELECT2 KATEGORI (5 data awal + Live Search)
// ==========================================
function initSelect2Kategori($element, parentModalId) {
  if ($element.hasClass("select2-hidden-accessible")) {
    $element.select2("destroy");
  }

  $element.select2({
    theme: "bootstrap-5",
    dropdownParent: $(parentModalId),
    placeholder: "Pilih atau cari kategori...",
    allowClear: true,
    width: "100%",
    ajax: {
      url: "/Produk/GetDropdownKategori",
      dataType: "json",
      delay: 300,
      data: function (params) {
        return { q: params.term || "" };
      },
      processResults: function (data) {
        return {
          results: (data.results || []).map(function (item) {
            return { id: item.id, text: item.text };
          })
        };
      },
      cache: true
    },
    minimumInputLength: 0
  });
}

// ==========================================
// INISIALISASI SELECT2 UOM (5 data awal + Live Search)
// ==========================================
function initSelect2Uom($element, parentModalId) {
  if ($element.hasClass("select2-hidden-accessible")) {
    $element.select2("destroy");
  }

  $element.select2({
    theme: "bootstrap-5",
    dropdownParent: $(parentModalId),
    placeholder: "Pilih Satuan...",
    allowClear: true,
    width: "100%",
    ajax: {
      url: "/Produk/GetDropdownUom",
      dataType: "json",
      delay: 300,
      data: function (params) {
        return { q: params.term || "" };
      },
      processResults: function (data) {
        return {
          results: (data.results || []).map(function (item) {
            return { id: item.id, text: item.text };
          })
        };
      },
      cache: true
    },
    minimumInputLength: 0
  });
}

// ==========================================
// SETUP GUDANG & RAK DROPDOWN
// ==========================================
function setupGudangRakDropdown($gudangEl, $rakEl, parentModalId) {
  $gudangEl.off("change").on("change", function () {
    const gudangId = $(this).val();
    const selectedText = $gudangEl.select2("data")[0] ? $gudangEl.select2("data")[0].text : "";

    $rakEl.val(null).trigger("change");

    if (selectedText.toLowerCase().includes("gudang transit virtual")) {
      $rakEl.prop("disabled", true);
      $rakEl.val(null).trigger("change");
      const modalPrefix = parentModalId.includes("Edit") ? "edit" : "create";
      $(`#error-${modalPrefix}-RakId`).text("");
      $rakEl.next(".select2-container").find(".select2-selection").removeClass("is-invalid border-danger");
    } else if (gudangId) {
      $rakEl.prop("disabled", false);
    } else {
      $rakEl.prop("disabled", true);
    }
  });

  if ($gudangEl.hasClass("select2-hidden-accessible")) $gudangEl.select2("destroy");
  if ($rakEl.hasClass("select2-hidden-accessible")) $rakEl.select2("destroy");

  $gudangEl.select2({
    theme: "bootstrap-5",
    dropdownParent: $(parentModalId),
    placeholder: "Pilih Gudang...",
    allowClear: true,
    ajax: {
      url: "/Produk/GetDropdownGudang",
      dataType: "json",
      delay: 300,
      data: function (params) {
        return { q: params.term, page: params.page || 1 };
      },
      processResults: function (data) {
        var items = data.results || data;
        return {
          results: items.map(function (item) {
            return { id: item.id, text: item.text };
          }),
        };
      },
    },
  });

  $rakEl.select2({
    theme: "bootstrap-5",
    dropdownParent: $(parentModalId),
    placeholder: "Pilih Rak...",
    allowClear: true,
    disabled: false,
    ajax: {
      url: "/Produk/GetDropdownRakByGudang",
      dataType: "json",
      delay: 300,
      data: function (params) {
        return { gudangId: $gudangEl.val(), q: params.term };
      },
      processResults: function (data) {
        var list = data.results || data;
        return {
          results: (Array.isArray(list) ? list : []).map(function (item) {
            return {
              id: item.id,
              text: item.text || item.namaRak || "Rak tanpa nama",
              kode: item.kode || "",
              namaRak: item.namaRak || "",
              area: item.area || "-",
              binNo: item.binNo || "-",
              deskripsi: item.deskripsi || "",
            };
          }),
        };
      },
    },
    templateResult: formatRakOption,
    templateSelection: formatRakSelection,
  });

  function formatRakOption(item) {
    if (item.loading || !item.id) return item.text;
    var namaDisplay = item.namaRak || item.text;
    var kodeDisplay = item.kode ? `<span class="badge bg-primary me-1">${item.kode}</span>` : "";
    var areaDisplay = `<span class="text-muted small ms-1">(Area ${item.area} - Bin ${item.binNo})</span>`;

    return $(`
            <div class="py-1">
                <div class="d-flex align-items-center justify-content-between">
                    <div>
                        ${kodeDisplay}
                        <strong class="text-dark">${namaDisplay}</strong>
                        ${areaDisplay}
                    </div>
                </div>
                ${item.deskripsi ? `<div class="text-secondary small mt-1" style="font-size: 0.8rem; line-height: 1.2;"><i class="bi bi-info-circle me-1 text-primary"></i><em>${item.deskripsi}</em></div>` : ""}
            </div>
        `);
  }

  function formatRakSelection(item) {
    if (!item.id) return item.text;
    var label = item.namaRak || item.text;
    return item.kode ? `[${item.kode}] ${label}` : label;
  }
}

function clearFormErrors(prefix) {
  const $form = $(`#form${prefix === "create" ? "Tambah" : "Edit"}Produk`);
  $form.find(".error-msg").text("");
  $form.find(".form-control, .form-select").removeClass("is-invalid");
  $form.find(".select2-selection").removeClass("is-invalid border-danger");
}