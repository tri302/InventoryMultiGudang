/**
 * assignments.js - Module CRUD & Interaction Penugasan Aset
 */

let selectedItems = [];

$(document).ready(function () {
  if ($("#tbodyPenugasan").length > 0) {
    initIndexPage();
  }
  if ($("#formRegistrasiPenugasan").length > 0) {
    initCreatePage();
  }
  if ($("#formTransferAset").length > 0) {
    initTransferPage();
  }

  const activeClasses =
    "border-red-600 bg-red-50 text-red-700 dark:bg-red-950/20 dark:border-red-500 dark:text-red-400";
  const inactiveClasses =
    "border-slate-200 bg-white text-slate-600 hover:border-slate-300 dark:bg-slate-800 dark:border-slate-700 dark:text-slate-300";

  const typeLabels = {
    1: "Penerima (Karyawan)",
    2: "Penerima (Department)",
    3: "Penerima (Customer)",
  };

  function setTipePenerima(selectedType) {
    $("#TipePenerimaVal").val(selectedType);

    $("[data-type]").each(function () {
      const btn = $(this);
      const btnType = btn.attr("data-type");

      if (btnType === selectedType.toString()) {
        btn.removeClass(inactiveClasses).addClass(activeClasses);
      } else {
        btn.removeClass(activeClasses).addClass(inactiveClasses);
      }
    });

    if (typeLabels[selectedType]) {
      $("#lblPenerima").text(typeLabels[selectedType]);
    }

    if ($("#TargetPenerimaId").hasClass("select2-hidden-accessible")) {
      $("#TargetPenerimaId").val(null).empty().trigger("change");
    }
  }

  $(document).on("click", "[data-type]", function () {
    const selectedType = $(this).attr("data-type");
    setTipePenerima(selectedType);
  });

  const initialType = $("#TipePenerimaVal").val() || "1";
  setTipePenerima(initialType);
});

/* ==========================================================================
   HALAMAN INDEX (LIST DATA)
   ========================================================================== */
function initIndexPage() {
  loadPenugasanData(1);

  let timerSearch;
  $("#txtSearch").on("keyup input", function () {
    clearTimeout(timerSearch);
    timerSearch = setTimeout(function () {
      loadPenugasanData(1);
    }, 300);
  });

  $("#ddlStatus").on("change", function () {
    loadPenugasanData(1);
  });
}
function loadPenugasanData(page) {
  const search = $("#txtSearch").val() || "";
  const status = $("#ddlStatus").val() || "";
  const pageSize = 5;
  const $tbody = $("#tbodyPenugasan");

  $tbody.html(
    '<tr><td colspan="5" class="text-center py-4 text-muted"><div class="spinner-border spinner-border-sm me-2"></div> Memuat data penugasan...</td></tr>',
  );

  $.ajax({
    url: "/PenugasanAset/GetPaged",
    type: "GET",
    data: { search: search, status: status, page: page, pageSize: pageSize },
    dataType: "json",
    success: function (res) {
      $tbody.empty();
      let items = [];
      let currentPage = page;
      let totalItems = 0;
      let currentPageSize = pageSize;

      if (res.data && res.data.items) {
        items = res.data.items;
        currentPage = res.data.pageIndex || page;
        totalItems = res.data.totalItems || 0;
        currentPageSize = res.data.pageSize || pageSize;
      } else if (Array.isArray(res.items)) {
        items = res.items;
        currentPage = res.pageIndex || page;
        totalItems = res.totalItems || items.length;
        currentPageSize = res.pageSize || pageSize;
      } else if (Array.isArray(res)) {
        items = res;
        totalItems = items.length;
      }

      if (items.length === 0) {
        $tbody.html(
          '<tr><td colspan="5" class="text-center py-4 text-muted">Data penugasan aset tidak ditemukan.</td></tr>',
        );
        $("#paginationInfo").html(
          "Menampilkan <strong>0</strong> - <strong>0</strong> dari <strong>0</strong> data",
        );
        $("#paginationContainer ul").empty();
        return;
      }

      $.each(items, function (i, item) {
        const tglSerah = item.tanggalSerahTerima
          ? item.tanggalSerahTerima.split("T")[0]
          : "-";
        const tglKembali = item.tanggalPengembalian
          ? item.tanggalPengembalian.split("T")[0]
          : null;

        let displayTanggal = tglSerah;
        if (tglKembali) {
          displayTanggal = `${tglSerah} <span class="text-muted mx-1">&rarr;</span> <span class="text-success fw-semibold">${tglKembali}</span>`;
        }

        // Badge Enum StatusPenugasan
        let statusBadge = "";
        const st = parseInt(item.status);
        if (st === 1)
          statusBadge =
            '<span class="badge bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2 py-1">Menunggu Persetujuan</span>';
        else if (st === 2)
          statusBadge =
            '<span class="badge bg-primary-subtle text-primary border border-primary-subtle px-2 py-1">Dalam Penugasan</span>';
        else if (st === 3)
          statusBadge =
            '<span class="badge bg-success-subtle text-success border border-success-subtle px-2 py-1">Dikembalikan</span>';
        else if (st === 4)
          statusBadge =
            '<span class="badge bg-danger-subtle text-danger border border-danger-subtle px-2 py-1">Ditolak</span>';
        else if (st === 5)
          statusBadge =
            '<span class="badge bg-info-subtle text-info-emphasis border border-info-subtle px-2 py-1">Transfer Aset Langsung</span>';

        let infoSiklus = item.keterangan
          ? `<div class="text-muted small mt-0.5" style="font-size:0.72rem;">${item.keterangan}</div>`
          : "";
        const itemId = item.id || item.Id;

        $tbody.append(`
                    <tr class="border-bottom">
                        <td class="ps-4 py-3 fw-bold text-indigo-dark">${item.kodePenugasan || "-"}</td>
                        <td class="py-3">
                            <div class="fw-semibold text-indigo-dark">${item.namaTarget || "-"}</div>
                            <small class="text-muted">${item.tipeTargetStr || "-"}</small>
                            ${infoSiklus}
                        </td>
                        <td class="py-3 text-secondary">${displayTanggal}</td>
                        <td class="py-3 text-center">${statusBadge}</td>
                        <td class="py-3 text-center pe-4">
                            <a href="/PenugasanAset/Details/${itemId}" class="btn btn-sm btn-outline-secondary" title="Detail">
                                <i class="bi bi-eye"></i>
                            </a>
                        </td>
                    </tr>
                `);
      });

      const startItem = (currentPage - 1) * currentPageSize + 1;
      const endItem = Math.min(currentPage * currentPageSize, totalItems);
      $("#paginationInfo").html(
        `Menampilkan <strong>${startItem}</strong> - <strong>${endItem}</strong> dari <strong>${totalItems}</strong> data`,
      );

      // Render tombol pagination
      const totalPages = Math.ceil(totalItems / currentPageSize);
      renderPagination(currentPage, totalPages);
    },
    error: function () {
      $tbody.html(
        '<tr><td colspan="5" class="text-center py-4 text-danger">Gagal memuat data dari server.</td></tr>',
      );
      $("#paginationInfo").html(
        "Menampilkan <strong>0</strong> - <strong>0</strong> dari <strong>0</strong> data",
      );
      $("#paginationContainer ul").empty();
    },
  });
}

function renderPagination(current, totalPages) {
  const $pagination = $("#paginationContainer ul");
  $pagination.empty();

  if (totalPages <= 1) return;

  // 1. Tombol Previous
  const prevDisabled = current <= 1 ? "disabled" : "";
  const prevStyle =
    current <= 1 ? 'style="background-color: #e9ecef; color: #6c757d;"' : "";
  $pagination.append(`
        <li class="page-item ${prevDisabled}">
            <button type="button" class="page-link" ${prevStyle} ${current > 1 ? `onclick="loadPenugasanData(${current - 1})"` : "disabled"}>Previous</button>
        </li>
    `);

  // Hitung batasan 5 ke kiri dan 5 ke kanan dari halaman aktif
  let startPage = Math.max(1, current - 5);
  let endPage = Math.min(totalPages, current + 5);

  // Tampilkan halaman 1 dan indikator "..." jika rentang awal jauh dari halaman 1
  if (startPage > 1) {
    $pagination.append(`
      <li class="page-item">
          <button type="button" class="page-link" onclick="loadPenugasanData(1)">1</button>
      </li>
    `);
    if (startPage > 2) {
      $pagination.append(`
        <li class="page-item disabled">
            <span class="page-link">...</span>
        </li>
      `);
    }
  }

  // Render nomor halaman dalam rentang limit (Max 5 Kiri & 5 Kanan)
  for (let i = startPage; i <= endPage; i++) {
    const activeClass = i === current ? "active" : "";
    $pagination.append(`
            <li class="page-item ${activeClass}">
                <button type="button" class="page-link" onclick="loadPenugasanData(${i})">${i}</button>
            </li>
        `);
  }

  // Tampilkan halaman terakhir dan indikator "..." jika rentang akhir jauh dari totalPages
  if (endPage < totalPages) {
    if (endPage < totalPages - 1) {
      $pagination.append(`
        <li class="page-item disabled">
            <span class="page-link">...</span>
        </li>
      `);
    }
    $pagination.append(`
      <li class="page-item">
          <button type="button" class="page-link" onclick="loadPenugasanData(${totalPages})">${totalPages}</button>
      </li>
    `);
  }

  // 3. Tombol Next
  const nextDisabled = current >= totalPages ? "disabled" : "";
  const nextStyle =
    current >= totalPages
      ? 'style="background-color: #e9ecef; color: #6c757d;"'
      : "";
  $pagination.append(`
        <li class="page-item ${nextDisabled}">
            <button type="button" class="page-link" ${nextStyle} ${current < totalPages ? `onclick="loadPenugasanData(${current + 1})"` : "disabled"}>Next</button>
        </li>
    `);
}

/* ==========================================================================
   HALAMAN FORM TRANSFER ASET
   ========================================================================== */
function initTransferPage() {
  initPenerimaSelect2();

  // Inisialisasi Select2 dengan AJAX Live Search untuk Aset Aktif
  $("#selectProduk").select2({
    theme: "bootstrap-5",
    placeholder: "-- Pilih Aset Aktiv --",
    allowClear: true,
    ajax: {
      url: "/PenugasanAset/GetAssignedAssets",
      dataType: "json",
      delay: 300,
      data: function (params) {
        return {
          search: params.term || "", // Kirim keyword pencarian
        };
      },
      processResults: function (data) {
        var list = data.success && data.data ? data.data : [];
        return {
          results: $.map(list, function (item) {
            return {
              id: item.produkItemId,
              text: item.displayText,
            };
          }),
        };
      },
      cache: true,
    },
  });

  // Event saat dropdown aset berubah
  $("#selectProduk").on("change", function () {
    const id = $(this).val();
    if (!id) {
      // Reset kembali ke string kosong jika tidak dipilih
      $("#detailNamaProduk").text("-");
      $("#detailSkuSn").text("SKU: - | SN: -");
      $("#detailGudang").text("Gudang Asal: -");
      $("#detailBadgeTipe")
        .text("-")
        .attr(
          "class",
          "px-1.5 py-0.5 text-[9px] font-bold rounded uppercase border",
        );
      $("#detailNamaPemegang").text("-");
      $("#detailStatusAset").text("-");
      return;
    }

    $.ajax({
      url: "/PenugasanAset/GetAssignedAssetDetail",
      type: "GET",
      data: { id: id },
      success: function (res) {
        if (res.success && res.data) {
          const d = res.data;

          // Mengisi masing-masing elemen ID di HTML Anda
          $("#detailNamaProduk").text(d.namaBarang || d.namaProduk || "-");
          $("#detailSkuSn").text(
            `SKU: ${d.sku || "-"} | SN: ${d.serialNumber || "-"}`,
          );
          $("#detailGudang").text(`Gudang Asal: ${d.gudangAsal || "-"}`);
          $("#detailNamaPemegang").text(
            d.pemegangSaatIni || d.namaPemegang || "-",
          );
          $("#detailStatusAset").text(d.statusAsetStr || d.statusAset || "-");

          // Mengisi badge tipe pemegang
          const tipeStr = d.tipePemegangStr || d.tipePemegangText || "-";
          const $badge = $("#detailBadgeTipe").text(tipeStr);

          const tipeVal = parseInt(d.tipePemegang) || 1;
          if (tipeVal === 1) {
            $badge.attr(
              "class",
              "px-1.5 py-0.5 bg-red-50 dark:bg-red-950/20 text-red-600 dark:text-red-400 border border-red-100 dark:border-red-900/30 text-[9px] font-bold rounded uppercase",
            );
          } else if (tipeVal === 2) {
            $badge.attr(
              "class",
              "px-1.5 py-0.5 bg-red-50 dark:bg-red-950/20 text-red-600 dark:text-red-400 border border-red-100 dark:border-red-900/30 text-[9px] font-bold rounded uppercase",
            );
          } else {
            $badge.attr(
              "class",
              "px-1.5 py-0.5 bg-red-50 dark:bg-red-950/20 text-red-600 dark:text-red-400 border border-red-100 dark:border-red-900/30 text-[9px] font-bold rounded uppercase",
            );
          }
        } else {
          toastr.error(res.message || "Gagal mengambil detail aset.");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan koneksi server.");
      },
    });
  });

  // Form Submit Handler untuk Transfer Aset
  $("#formTransferAset").on("submit", function (e) {
    e.preventDefault();
    const $form = $(this);

    $.ajax({
      url: $form.attr("action"),
      type: "POST",
      data: $form.serialize(),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message || "Transfer aset berhasil disimpan!");
          setTimeout(
            () => (window.location.href = "/PenugasanAset/Index"),
            1000,
          );
        } else {
          toastr.error(res.message || "Terjadi kesalahan.");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan koneksi server.");
      },
    });
  });
}

/* ==========================================================================
   HALAMAN FORM CREATE / EDIT
   ========================================================================== */
function initCreatePage() {
  initPenerimaSelect2();

  let timerScan;
  $("#txtScanSerial").on("keyup input change", function (e) {
    if (e.type === "keyup" && e.key !== "Enter") {
      clearTimeout(timerScan);
      timerScan = setTimeout(function () {
        processScanSerial();
      }, 500);
    } else if (e.key === "Enter" || e.type === "change") {
      e.preventDefault();
      clearTimeout(timerScan);
      processScanSerial();
    }
  });

  $("#btnTambahScan").on("click", function () {
    processScanSerial();
  });

  $("#modalPilihAset").on("show.bs.modal", function () {
    loadModalAsetList("");
  });

  let timerModalSearch;
  $("#txtSearchModalAset").on("keyup input", function () {
    clearTimeout(timerModalSearch);
    const query = $(this).val();
    timerModalSearch = setTimeout(function () {
      loadModalAsetList(query);
    }, 300);
  });

  $(document).on("click", ".list-item-aset", function () {
    const itemData = {
      id: $(this).data("id"),
      namaBarang: $(this).data("nama"),
      serialNumber: $(this).data("sn"),
      sku: $(this).data("sku"),
      lokasiAwal: $(this).data("lokasi"),
      kuantitas: 1,
      kondisiSerah: "Good",
    };
    addAsetToList(itemData);
    const modalEl = document.getElementById("modalPilihAset");
    const modalInstance = bootstrap.Modal.getInstance(modalEl);
    if (modalInstance) modalInstance.hide();
  });

  $("#formRegistrasiPenugasan").on("submit", function (e) {
    e.preventDefault();
    if (selectedItems.length === 0) {
      toastr.warning("Silakan tambahkan minimal 1 item aset yang ditugaskan.");
      return false;
    }

    const formData = new FormData(this);
    selectedItems.forEach((item, idx) => {
      formData.append(`Details[${idx}].ProdukItemId`, item.id);
      formData.append(`Details[${idx}].NamaBarang`, item.namaBarang);
      formData.append(`Details[${idx}].SerialNumber`, item.serialNumber);
      formData.append(`Details[${idx}].LokasiAwal`, item.lokasiAwal);
      formData.append(`Details[${idx}].Kuantitas`, item.kuantitas);
      formData.append(`Details[${idx}].KondisiSerah`, item.kondisiSerah);
    });

    $.ajax({
      url: $(this).attr("action") || "/PenugasanAset/Create",
      type: "POST",
      data: formData,
      contentType: false,
      processData: false,
      success: function (res) {
        if (res.success) {
          toastr.success(res.message || "Penugasan aset berhasil disimpan!");
          setTimeout(
            () => (window.location.href = "/PenugasanAset/Index"),
            1000,
          );
        } else {
          toastr.error(res.message || "Terjadi kesalahan saat menyimpan data.");
        }
      },
      error: () => toastr.error("Terjadi kesalahan koneksi server."),
    });
  });
}

function initPenerimaSelect2() {
  $("#TargetPenerimaId").select2({
    theme: "bootstrap-5",
    placeholder: "Pilih Penerima...",
    allowClear: true,
    ajax: {
      url: "/PenugasanAset/GetPenerimaOptions",
      dataType: "json",
      delay: 300,
      data: function (params) {
        return {
          tipe: $("#TipePenerimaVal").val() || 1,
          search: params.term || "",
        };
      },
      processResults: function (data) {
        var list = Array.isArray(data) ? data : data.results || data.data || [];
        return {
          results: $.map(list, function (item) {
            return {
              id: item.id,
              text: item.text,
            };
          }),
        };
      },
      cache: true,
    },
  });
}

function processScanSerial() {
  const sn = $("#txtScanSerial").val().trim();
  if (!sn) return;

  const isExist = selectedItems.some(
    (x) => x.serialNumber.toLowerCase() === sn.toLowerCase(),
  );
  if (isExist) {
    toastr.warning(
      `Aset dengan Serial Number '${sn}' sudah dimasukkan ke dalam daftar!`,
    );
    $("#txtScanSerial").val("");
    return;
  }

  $.ajax({
    url: "/PenugasanAset/GetAssetBySerial",
    type: "GET",
    data: { serialNumber: sn },
    dataType: "json",
    success: function (res) {
      if (res.success && res.data) {
        addAsetToList(res.data);
        $("#txtScanSerial").val("");
        toastr.success(`Aset '${res.data.namaBarang}' berhasil ditambahkan.`);
      } else {
        toastr.error(res.message || "Aset tidak ditemukan di database.");
      }
    },
    error: function () {
      toastr.error("Gagal menghubungi server untuk mencari aset.");
    },
  });
}

function loadModalAsetList(searchQuery) {
  const $container = $("#containerModalAsetList");
  $container.html(
    '<div class="text-center py-4 text-muted"><div class="spinner-border spinner-border-sm me-2"></div> Memuat data aset...</div>',
  );

  $.ajax({
    url: "/PenugasanAset/GetAvailableAssets",
    type: "GET",
    data: { search: searchQuery },
    dataType: "json",
    success: function (res) {
      $container.empty();
      if (res.success && res.data.length > 0) {
        $.each(res.data, function (i, item) {
          $container.append(`
                        <div class="card p-3 border border-light-subtle cursor-pointer list-item-aset" 
                             data-id="${item.id}"
                             data-nama="${item.namaBarang}"
                             data-sku="${item.sku}"
                             data-sn="${item.serialNumber}"
                             data-lokasi="${item.lokasiAwal}"
                             style="border-radius: 8px; transition: background 0.2s;">
                            <div class="d-flex justify-content-between align-items-start gap-2">
                                <div>
                                    <div class="d-flex align-items-center gap-2 mb-1.5">
                                        <span class="badge bg-indigo-light text-primary fw-semibold" style="font-size: 0.7rem; padding: 3px 6px; border-radius: 4px;">${item.tipe}</span>
                                        <span class="fw-bold text-indigo-dark small">${item.namaBarang}</span>
                                    </div>
                                    <div class="text-muted font-monospace" style="font-size: 0.75rem;">
                                        ${item.sku} <span class="mx-1 text-light-dark">|</span> SN: <span class="text-danger fw-semibold">${item.serialNumber}</span>
                                    </div>
                                </div>
                                <div class="text-end flex-shrink-0">
                                    <div class="fw-semibold text-indigo-dark" style="font-size: 0.8rem;">${item.lokasiAwal}</div>
                                    <div class="text-muted small" style="font-size: 0.75rem;">Rak: ${item.rak}</div>
                                </div>
                            </div>
                        </div>
                    `);
        });
      } else {
        $container.html(
          '<div class="text-center py-4 text-muted small">Tidak ada aset yang tersedia/ditemukan.</div>',
        );
      }
    },
    error: function () {
      $container.html(
        '<div class="text-center py-4 text-danger small">Gagal memuat data dari server.</div>',
      );
    },
  });
}

function addAsetToList(item) {
  const isExist = selectedItems.some(
    (x) => x.serialNumber.toLowerCase() === item.serialNumber.toLowerCase(),
  );
  if (isExist) {
    toastr.warning(`Serial Number '${item.serialNumber}' sudah ada di tabel.`);
    return;
  }

  selectedItems.push(item);
  renderSelectedItemsTable();
}

function removeItemDetail(index) {
  selectedItems.splice(index, 1);
  renderSelectedItemsTable();
}

function renderSelectedItemsTable() {
  const $tbody = $("#tbodySelectedAset");
  $tbody.empty();

  $("#lblItemCount").text(selectedItems.length);

  if (selectedItems.length === 0) {
    $("#emptyStateDetail").show();
    return;
  }

  $("#emptyStateDetail").hide();

  $.each(selectedItems, function (idx, item) {
    $tbody.append(`
            <tr class="border-bottom">
                <td class="py-3">
                    <div class="fw-bold text-indigo-dark small">${item.namaBarang}</div>
                    <div class="font-monospace text-muted" style="font-size: 0.7rem;">SN: <span class="text-danger fw-semibold">${item.serialNumber}</span></div>
                </td>
                <td class="py-3 small text-muted text-truncate" style="max-width: 150px;">${item.lokasiAwal}</td>
                <td class="py-3">
                    <input type="number" class="form-control form-control-sm text-center" style="width: 65px;" value="${item.kuantitas}" min="1" onchange="selectedItems[${idx}].kuantitas = parseInt(this.value) || 1">
                </td>
                <td class="py-3">
                    <select class="form-select form-select-sm text-dark fw-medium" style="width: 110px; font-size: 0.8rem;" onchange="selectedItems[${idx}].kondisiSerah = this.value">
                        <option value="Good" ${item.kondisiSerah === "Good" ? "selected" : ""}>Good</option>
                        <option value="Brand New" ${item.kondisiSerah === "Brand New" ? "selected" : ""}>Brand New</option>
                        <option value="Fair" ${item.kondisiSerah === "Fair" ? "selected" : ""}>Fair</option>
                    </select>
                </td>
                <td class="py-3 text-center">
                    <button type="button" class="btn btn-danger btn-sm d-inline-flex align-items-center justify-content-center p-2 shadow-sm" style="width: 32px; height: 32px; border-radius: 6px;" onclick="removeItemDetail(${idx})" title="Hapus Aset">
                        <i class="bi bi-trash text-white" style="font-size: 0.85rem;"></i>
                    </button>
                </td>
            </tr>
        `);
  });
}

$(document).ready(function () {
  $(document).on("submit", "#formPengembalian", function (e) {
    e.preventDefault();
    const $form = $(this);
    const actionUrl = $form.attr("action");

    $.ajax({
      url: actionUrl,
      type: "POST",
      data: $form.serialize(),
      success: function (res) {
        if (res.success || res.status === "ok") {
          if (typeof toastr !== "undefined")
            toastr.success(res.message || "Pengembalian berhasil disimpan.");
          setTimeout(function () {
            window.location.reload();
          }, 800);
        } else {
          if (typeof toastr !== "undefined")
            toastr.error(res.message || "Gagal menyimpan pengembalian.");
        }
      },
      error: function () {
        if (typeof toastr !== "undefined")
          toastr.error("Terjadi kesalahan server.");
      },
    });
  });
});

$(document).ready(function () {
  const token = $('input[name="__RequestVerificationToken"]').val();

  // 1. Handler Tombol Setujui Penugasan
  $(document).on("click", "#btnSetujuiPenugasan", function () {
    const id = $(this).data("id");

    // Toastr Konfirmasi
    toastr.warning(
      "<br/><button type='button' id='btnConfirmSetuju' class='btn btn-primary btn-sm mt-2'>Ya, Setujui</button>",
      "Apakah Anda yakin ingin menyetujui penugasan ini?",
      {
        closeButton: true,
        allowHtml: true,
        timeOut: 0,
        extendedTimeOut: 0,
        onShown: function () {
          $("#btnConfirmSetuju")
            .off("click")
            .on("click", function () {
              toastr.clear();

              // AJAX Setujui
              $.ajax({
                url: "/PenugasanAset/ApprovePenugasan",
                type: "POST",
                data: {
                  id: id,
                  __RequestVerificationToken: token,
                },
                success: function (res) {
                  if (res.success) {
                    toastr.success(
                      res.message || "Berhasil menyetujui penugasan.",
                    );
                    setTimeout(function () {
                      window.location.reload();
                    }, 800);
                  } else {
                    toastr.error(res.message || "Terjadi kesalahan.");
                  }
                },
                error: function () {
                  toastr.error("Gagal terhubung ke server.");
                },
              });
            });
        },
      },
    );
  });

  // 2. Handler Tombol Tolak Penugasan
  $(document).on("click", "#btnTolakPenugasan", function () {
    const id = $(this).data("id");
    const token = $('input[name="__RequestVerificationToken"]').val();
    // Toastr Konfirmasi
    toastr.warning(
      "<br/><button type='button' id='btnConfirmTolak' class='btn btn-danger btn-sm mt-2'>Ya, Tolak</button>",
      "Apakah Anda yakin ingin menolak penugasan ini?",
      {
        closeButton: true,
        allowHtml: true,
        timeOut: 0,
        extendedTimeOut: 0,
        onShown: function () {
          $("#btnConfirmTolak")
            .off("click")
            .on("click", function () {
              toastr.clear();

              // AJAX Tolak
              $.ajax({
                url: "/PenugasanAset/RejectPenugasan",
                type: "POST",
                data: {
                  id: id,
                  __RequestVerificationToken: token,
                },
                success: function (res) {
                  if (res.success) {
                    toastr.success(
                      res.message || "Berhasil menolak penugasan.",
                    );
                    setTimeout(function () {
                      window.location.reload();
                    }, 800);
                  } else {
                    toastr.error(res.message || "Terjadi kesalahan.");
                  }
                },
                error: function () {
                  toastr.error("Gagal terhubung ke server.");
                },
              });
            });
        },
      },
    );
  });
});
// Variabel untuk menyimpan state tab aktif di modal
let currentModalFilter = "semua";

// Saat modal pertama kali dibuka, reset ke tab "Semua"
$("#modalPilihAset").on("show.bs.modal", function () {
  currentModalFilter = "semua";

  // Sinkronisasi class active secara manual pada tombol tab
  $(".tab-filter-btn")
    .removeClass(
      "active fw-bold text-danger border-bottom border-danger border-3",
    )
    .addClass("text-muted fw-medium");
  $("#semua-tab")
    .addClass("active fw-bold text-danger border-bottom border-danger border-3")
    .removeClass("text-muted fw-medium");

  $("#txtSearchModalAset").val("");
  loadModalAsetList("", currentModalFilter);
});

// Event ketika tab diklik (Semua / Asset / Inventory)
$("#asetTab").on("click", 'button[data-bs-toggle="tab"]', function (e) {
  e.preventDefault();

  // Hapus highlight dari semua tombol tab
  $(".tab-filter-btn")
    .removeClass(
      "active fw-bold text-danger border-bottom border-danger border-3",
    )
    .addClass("text-muted fw-medium");

  // Berikan highlight warna merah hanya pada tab yang diklik
  $(this)
    .addClass("active fw-bold text-danger border-bottom border-danger border-3")
    .removeClass("text-muted fw-medium");

  currentModalFilter = $(this).attr("data-filter") || "semua";
  const searchQuery = $("#txtSearchModalAset").val();
  loadModalAsetList(searchQuery, currentModalFilter);
});

// Event saat mengetik di search modal
let timerModalSearch;
$("#txtSearchModalAset").on("keyup input", function () {
  clearTimeout(timerModalSearch);
  const query = $(this).val();
  timerModalSearch = setTimeout(function () {
    loadModalAsetList(query, currentModalFilter);
  }, 300);
});

// Fungsi memuat data modal dengan parameter type filter
function loadModalAsetList(searchQuery, typeFilter) {
  const $container = $("#containerModalAsetList");
  $container.html(
    '<div class="text-center py-4 text-muted"><div class="spinner-border spinner-border-sm me-2"></div> Memuat data aset...</div>',
  );

  $.ajax({
    url: "/PenugasanAset/GetAvailableAssets",
    type: "GET",
    data: { search: searchQuery, type: typeFilter },
    dataType: "json",
    success: function (res) {
      $container.empty();
      if (res.success && res.data && res.data.length > 0) {
        $.each(res.data, function (i, item) {
          $container.append(`
                        <div class="card p-3 border border-light-subtle cursor-pointer list-item-aset" 
                             data-id="${item.id}"
                             data-nama="${item.namaBarang}"
                             data-sku="${item.sku}"
                             data-sn="${item.serialNumber}"
                             data-lokasi="${item.lokasiAwal}"
                             style="border-radius: 8px; transition: background 0.2s;">
                            <div class="d-flex justify-content-between align-items-start gap-2">
                                <div>
                                    <div class="d-flex align-items-center gap-2 mb-1.5">
                                        <span class="badge bg-indigo-light text-primary fw-semibold" style="font-size: 0.7rem; padding: 3px 6px; border-radius: 4px;">${item.tipe}</span>
                                        <span class="fw-bold text-indigo-dark small">${item.namaBarang}</span>
                                    </div>
                                    <div class="text-muted font-monospace" style="font-size: 0.75rem;">
                                        ${item.sku} <span class="mx-1 text-light-dark">|</span> SN: <span class="text-danger fw-semibold">${item.serialNumber}</span>
                                    </div>
                                </div>
                                <div class="text-end flex-shrink-0">
                                    <div class="fw-semibold text-indigo-dark" style="font-size: 0.8rem;">${item.lokasiAwal}</div>
                                    <div class="text-muted small" style="font-size: 0.75rem;">Rak: ${item.rak}</div>
                                </div>
                            </div>
                        </div>
                    `);
        });
      } else {
        $container.html(
          '<div class="text-center py-4 text-muted small">Tidak ada aset yang tersedia/ditemukan.</div>',
        );
      }
    },
    error: function () {
      $container.html(
        '<div class="text-center py-4 text-danger small">Gagal memuat data dari server.</div>',
      );
    },
  });
}
