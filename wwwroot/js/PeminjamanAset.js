/**
 * PeminjamanAset.js - Penanganan Frontend Interaktif, AJAX CRUD, Pagination, & Toastr
 */

let currentPage = 1;
let selectedItems = []; // Variabel global penampung item aset yang dipilih
let currentModalFilter = "semua"; // State tab aktif di modal

$(document).ready(function () {
  initPenerimaSelect2();

  $("#modalPilihAset").on("show.bs.modal", function () {
    $("#semua-tab").tab("show");
    currentModalFilter = "semua";
    $("#txtSearchModalAset").val("");
    loadModalAsetList("", currentModalFilter);
  });

  if (
    $("#tablePeminjamanIndex").length > 0 ||
    $("#tbodyPeminjaman").length > 0
  ) {
    loadIndexData(1);
    initGudangSelect2();
  }
  initPenerimaSelect2();

  // Live search handler tanpa reload page (Index)
  let searchTimer;
  $("#txtCariPeminjaman").on("keyup input", function () {
    clearTimeout(searchTimer);
    searchTimer = setTimeout(() => loadIndexData(1), 300);
  });

  $("#ddlFilterStatus, #CariGudang").on("change", function () {
    loadIndexData(1);
  });

  // Validasi Form Create menggunakan Fallback Unobtrusive
  const $form = $("#formPeminjamanAset");
  if ($form.length > 0) {
    if (
      typeof $.validator !== "undefined" &&
      typeof $.validator.unobtrusive !== "undefined"
    ) {
      $form.removeData("validator").removeData("unobtrusiveValidation");
      $.validator.unobtrusive.parse($form);
    }
  }

  // Event Trigger Scan Barcode/Serial Number
  $("#btnTambahScan").on("click", function () {
    processScanSerial();
  });

  $("#txtScanSerial").on("keypress", function (e) {
    if (e.which === 13) {
      e.preventDefault();
      processScanSerial();
    }
  });
});

// Helper Fungsi Format Tanggal ke dd/mm/yyyy
function formatDateToID(dateString) {
  if (!dateString) return "";
  const datePart = dateString.split("T")[0]; // Ambil bagian YYYY-MM-DD
  const parts = datePart.split("-");
  if (parts.length === 3) {
    return `${parts[2]}/${parts[1]}/${parts[0]}`;
  }
  return dateString;
}

// Fungsi memuat data Index dengan Ajax Pagination dan Fallback data kosong
function loadIndexData(page) {
  currentPage = page;
  const search = $("#txtCariPeminjaman").val() || "";
  const status = $("#ddlFilterStatus").val() || "";
  const gudangId = $("#CariGudang").val() || "";
  const pageSize = 5; // Sesuaikan ukuran halaman Anda
  const $tbody = $("#tbodyPeminjaman");

  $tbody.html(
    '<tr><td colspan="5" class="text-center py-4 text-muted">Memuat data...</td></tr>',
  );

  $.ajax({
    url: "/PeminjamanAset/GetPaged",
    type: "GET",
    data: {
      search: search,
      status: status !== "" ? status : null,
      pageIndex: page,
      pageSize: pageSize,
      gudangId: gudangId !== "" ? gudangId : null,
    },
    success: function (res) {
      $tbody.empty();

      if (res.success && res.data && res.data.length > 0) {
        const totalData = res.totalItems || 0;

        // Hitung rentang item yang sedang ditampilkan (Contoh: 1 - 5 dari 50)
        const startItem =
          totalData === 0 ? 0 : (res.pageIndex - 1) * pageSize + 1;
        const endItem = Math.min(res.pageIndex * pageSize, totalData);

        // Update teks informasi di sebelah kiri bawah tabel
        $("#tableInfoText").html(
          `Menampilkan <b>${startItem}</b> - <b>${endItem}</b> dari <b>${totalData}</b> data`,
        );
      } else {
        $tbody.html(
          '<tr><td colspan="5" class="text-center py-4 text-muted fw-semibold">Belum ada data untuk ditampilkan !</td></tr>',
        );
        $("#tableInfoText").text("Menampilkan 0 - 0 dari 0 data");
        $("#paginationContainer").empty();
        return;
      }

      $.each(res.data, function (i, item) {
        let badgeClass = "bg-secondary";
        let statusText = item.status;

        if (item.status === "Kembali" || item.status === 2) {
          badgeClass = "bg-success text-white";
          statusText = "Kembali";
        } else if (item.status === "Dipinjam" || item.status === 3) {
          badgeClass = "bg-warning text-dark";
          statusText = "Dipinjam";
        } else if (item.status === "Terlambat" || item.status === 1) {
          badgeClass = "bg-danger text-white";
          statusText = "Terlambat";
        }

        let hariTelatHtml = "";
        if (item.jumlahHariTelatPinjam && item.jumlahHariTelatPinjam > 0) {
          hariTelatHtml = `<span class="text-danger d-block">Jumlah Hari Terlambat : ${item.jumlahHariTelatPinjam} Hari</span>`;
        }

        let tanggalPengembalianHtml = "";
        if (item.tanggalPengembalian) {
          const tglKembaliFormatted = formatDateToID(item.tanggalPengembalian);
          const tglKembaliObj = new Date(item.tanggalPengembalian);
          const tglJatuhTempoObj = new Date(item.tanggalJatuhTempo);

          if (tglKembaliObj <= tglJatuhTempoObj) {
            tanggalPengembalianHtml = `<span class="text-success d-block fw-semibold">Tanggal Pengembalian : ${tglKembaliFormatted} (Tepat Waktu)</span>`;
          } else {
            tanggalPengembalianHtml = `<span class="text-danger d-block fw-semibold">Tanggal Pengembalian : ${tglKembaliFormatted} (Terlambat)</span>`;
          }
        }

        const tglPinjamFmt = formatDateToID(item.tanggalPinjam);
        const tglJatuhTempoFmt = formatDateToID(item.tanggalJatuhTempo);

        $tbody.append(`
                    <tr>
                        <td class="ps-4 py-3 fw-bold">${item.kodePeminjaman}</td>
                        <td>
                            ${item.namaPeminjamTamu ? `<span class="text-primary fw-semibold">Tamu: ${item.namaPeminjamTamu}</span>` : "Karyawan Internal"}
                            <br/>
                            <span> ${item.karyawanNama || ""} </span>    
                            <br/>
                            <span> ${item.karyawanNik || ""} </span>
                        </td>
                        <td>
                            ${tglPinjamFmt} → ${tglJatuhTempoFmt}
                            <span class="text-success d-block">Jumlah Hari Pinjam : ${item.jumlahHariPinjam || 0} Hari</span>
                            ${tanggalPengembalianHtml}
                            ${hariTelatHtml}
                        </td>
                        <td><span class="badge ${badgeClass} px-2.5 py-1">${statusText}</span></td>
                        <td class="text-center">
                            <a href="/PeminjamanAset/Details/${item.id}" class="btn btn-sm btn-outline-primary"><i class="bi bi-eye"></i></a>
                        </td>
                    </tr>
                `);
      });

      // Panggil fungsi render pagination
      const totalPages = Math.ceil(res.totalItems / pageSize);
      renderBootstrapPagination(res.pageIndex, totalPages);
    },
    error: function () {
      toastr.error("Gagal memuat data dari server.");
      $tbody.html(
        '<tr><td colspan="5" class="text-center py-4 text-danger">Gagal memuat data.</td></tr>',
      );
    },
  });
}

// Render Pagination Bootstrap bergaya tombol kotak terpisah rapi (Dibatasi 5 ke kiri & 5 ke kanan)
function renderBootstrapPagination(current, totalPages) {
  const $container = $("#paginationContainer");
  $container.empty();

  if (totalPages <= 1) return;

  let html = '<ul class="pagination pagination-sm mb-0">';

  // Tombol Previous
  const prevDisabled = current === 1 ? "disabled" : "";
  html += `<li class="page-item ${prevDisabled}">
                <button class="page-link" ${current > 1 ? `onclick="loadIndexData(${current - 1})"` : ""}>Previous</button>
             </li>`;

  // Hitung batasan 5 ke kiri dan 5 ke kanan dari halaman aktif
  let startPage = Math.max(1, current - 5);
  let endPage = Math.min(totalPages, current + 5);

  // Tampilkan halaman 1 dan indikator "..." jika rentang awal jauh dari halaman 1
  if (startPage > 1) {
    html += `<li class="page-item">
                <button class="page-link" onclick="loadIndexData(1)">1</button>
             </li>`;
    if (startPage > 2) {
      html += `<li class="page-item disabled">
                  <span class="page-link">...</span>
               </li>`;
    }
  }

  // Render nomor halaman dalam rentang limit (Max 5 Kiri & 5 Kanan)
  for (let i = startPage; i <= endPage; i++) {
    const active = i === current ? "active" : "";
    html += `<li class="page-item ${active}">
                <button class="page-link" onclick="loadIndexData(${i})">${i}</button>
             </li>`;
  }

  // Tampilkan halaman terakhir dan indikator "..." jika rentang akhir jauh dari totalPages
  if (endPage < totalPages) {
    if (endPage < totalPages - 1) {
      html += `<li class="page-item disabled">
                  <span class="page-link">...</span>
               </li>`;
    }
    html += `<li class="page-item">
                <button class="page-link" onclick="loadIndexData(${totalPages})">${totalPages}</button>
             </li>`;
  }

  // Tombol Next
  const nextDisabled = current === totalPages ? "disabled" : "";
  html += `<li class="page-item ${nextDisabled}">
                <button class="page-link" ${current < totalPages ? `onclick="loadIndexData(${current + 1})"` : ""}>Next</button>
             </li>`;

  html += "</ul>";
  $container.html(html);
}

// Fungsi Select2 Gudang
function initGudangSelect2() {
  if ($("#CariGudang").length === 0) return;
  $("#CariGudang").select2({
    theme: "bootstrap-5",
    placeholder: "Semua Gudang",
    allowClear: true,
    ajax: {
      url: "/PeminjamanAset/GetGudangOptions",
      dataType: "json",
      delay: 250,
      data: function (params) {
        return { q: params.term };
      },
      processResults: function (data) {
        return { results: data.results };
      },
      cache: true,
    },
  });
}

// Fungsi Select2 Penerima
function initPenerimaSelect2() {
  if ($("#KaryawananId").length === 0) return;

  // Hancurkan instance sebelumnya jika ada untuk menghindari duplikasi render
  if ($("#KaryawananId").hasClass("select2-hidden-accessible")) {
    $("#KaryawananId").select2("destroy");
  }

  $("#KaryawananId").select2({
    theme: "bootstrap-5",
    placeholder: "Pilih Penerima...",
    allowClear: true,
    width: "100%",
    ajax: {
      url: "/PeminjamanAset/GetPenerimaOptions",
      dataType: "json",
      delay: 250,
      data: function (params) {
        return {
          search: params.term || "",
        };
      },
      processResults: function (data) {
        var list = Array.isArray(data) ? data : data.results || data.data || [];
        return {
          results: $.map(list, function (item) {
            return {
              id: item.id || item.Id,
              text: item.text || item.Nama || item.nama,
            };
          }),
        };
      },
      cache: true,
    },
  });
}

// Event Tab Modal Aset
$(document).on(
  "shown.bs.tab",
  '#asetTab button[data-bs-toggle="tab"]',
  function (e) {
    currentModalFilter = $(this).attr("data-filter") || "semua";
    const searchQuery = $("#txtSearchModalAset").val();
    loadModalAsetList(searchQuery, currentModalFilter);
  },
);

// Event saat mengetik di search modal
let timerModalSearch;
$(document).on("keyup input", "#txtSearchModalAset", function () {
  clearTimeout(timerModalSearch);
  const query = $(this).val();
  timerModalSearch = setTimeout(function () {
    loadModalAsetList(query, currentModalFilter);
  }, 300);
});

// Pilih item aset dari modal list
$(document).on("click", ".list-item-aset", function () {
  const itemData = {
    id: $(this).data("id"),
    namaBarang: $(this).data("nama"),
    serialNumber: $(this).data("sn"),
    sku: $(this).data("sku"),
    lokasiAwal: $(this).data("lokasi"),
    gudangId: $(this).data("gudangid"),
    kuantitas: 1,
    kondisiSerah: "Good",
  };
  // Set secara otomatis ke input hidden GudangId di form utama
  if (itemData.gudangId) {
    $("#GudangId").val(itemData.gudangId);
  }
  addAsetToList(itemData);
  const modalEl = document.getElementById("modalPilihAset");
  const modalInstance = bootstrap.Modal.getInstance(modalEl);
  if (modalInstance) modalInstance.hide();
});

// Fungsi memuat data modal dengan parameter type dan search (Sinkronisasi dengan BE)
function loadModalAsetList(searchQuery, typeFilter) {
  const $container = $("#containerModalAsetList");
  if ($container.length === 0) return;

  $container.html(
    '<div class="text-center py-4 text-muted"><div class="spinner-border spinner-border-sm me-2"></div> Memuat data aset...</div>',
  );

  $.ajax({
    url: "/PeminjamanAset/GetAvailableAssets",
    type: "GET",
    data: { search: searchQuery, type: typeFilter || currentModalFilter },
    dataType: "json",
    success: function (res) {
      $container.empty();
      if (res.success && res.data && res.data.length > 0) {
        $.each(res.data, function (i, item) {
          $container.append(`
                        <div class="card p-3 border border-light-subtle cursor-pointer list-item-aset mb-2" 
                             data-id="${item.id}"
                             data-nama="${item.namaBarang}"
                             data-sku="${item.sku}"
                             data-sn="${item.serialNumber}"
                             data-lokasi="${item.lokasiAwal}"
                             data-gudangid="${item.gudangId}"
                             style="border-radius: 8px; transition: background 0.2s;">
                            <div class="d-flex justify-content-between align-items-start gap-2">
                                <div>
                                    <div class="d-flex align-items-center gap-2 mb-1.5">
                                        <span class="badge bg-primary bg-opacity-10 text-primary fw-semibold" style="font-size: 0.7rem; padding: 3px 6px; border-radius: 4px;">${item.tipe}</span>
                                        <span class="fw-bold text-dark small">${item.namaBarang}</span>
                                    </div>
                                    <div class="text-muted font-monospace" style="font-size: 0.75rem;">
                                        ${item.sku} <span class="mx-1">|</span> SN: <span class="text-danger fw-semibold">${item.serialNumber}</span>
                                    </div>
                                </div>
                                <div class="text-end flex-shrink-0">
                                    <div class="fw-semibold text-dark" style="font-size: 0.8rem;">${item.lokasiAwal}</div>
                                    <div class="text-muted small" style="font-size: 0.75rem;">Rak: ${item.rak || "-"}</div>
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

function processScanSerial() {
  const sn = $("#txtScanSerial").val().trim();
  if (!sn) return;

  const isExist = selectedItems.some(
    (x) => x.serialNumber && x.serialNumber.toLowerCase() === sn.toLowerCase(),
  );
  if (isExist) {
    toastr.warning(
      `Aset dengan Serial Number '${sn}' sudah dimasukkan ke dalam daftar!`,
    );
    $("#txtScanSerial").val("");
    return;
  }

  $.ajax({
    url: "/PeminjamanAset/GetAssetBySerial",
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

function addAsetToList(item) {
  const isExist = selectedItems.some(
    (x) =>
      x.serialNumber &&
      item.serialNumber &&
      x.serialNumber.toLowerCase() === item.serialNumber.toLowerCase(),
  );
  if (isExist) {
    toastr.warning(`Serial Number '${item.serialNumber}' sudah ada di tabel.`);
    return;
  }

  selectedItems.push(item);
  // Otomatis set GudangId berdasarkan aset yang dimasukkan (biasanya mengambil dari item pertama)
  if (item.gudangId) {
    $("#GudangId").val(item.gudangId);
  }
  renderSelectedItemsTable();
}

function removeItemDetail(index) {
  selectedItems.splice(index, 1);
  renderSelectedItemsTable();
}

function renderSelectedItemsTable() {
  const $container = $("#tbodySelectedAset"); // Bisa berupa div container list
  if ($container.length === 0) return;

  $container.empty();
  $("#lblItemCount").text(selectedItems.length);

  if (selectedItems.length === 0) {
    $("#emptyStateDetail").show();
    return;
  }

  $("#emptyStateDetail").hide();

  let htmlContent = '<div class="d-flex flex-column gap-2">';

  $.each(selectedItems, function (idx, item) {
    htmlContent += `
            <div class="p-3 border rounded bg-white shadow-sm d-flex align-items-center justify-content-between gap-3">
                <div class="flex-grow-1" style="min-width: 0;">
                    <div class="fw-bold text-dark small text-truncate">${item.namaBarang}</div>
                    <div class="font-monospace text-muted mb-1" style="font-size: 0.7rem;">SN: <span class="text-danger fw-semibold">${item.serialNumber || "-"}</span></div>
                    <div class="text-muted small">Lokasi: <span class="fw-medium text-dark">${item.lokasiAwal || "-"}</span></div>
                </div>
                <div class="d-flex align-items-center gap-2 flex-shrink-0">
                    <div>
                        <label class="form-label text-muted mb-0" style="font-size: 0.65rem;">Qty</label>
                        <input type="number" class="form-control form-control-sm" style="width: 60px;" value="${item.kuantitas || 1}" min="1" onchange="selectedItems[${idx}].kuantitas = parseInt(this.value) || 1">
                    </div>
                    <div>
                        <label class="form-label text-muted mb-0" style="font-size: 0.65rem;">Kondisi</label>
                        <select class="form-select form-select-sm text-dark fw-medium" style="width: 100px;" onchange="selectedItems[${idx}].kondisiSerah = this.value">
                            <option value="Good" ${item.kondisiSerah === "Good" ? "selected" : ""}>Good</option>
                            <option value="Brand New" ${item.kondisiSerah === "Brand New" ? "selected" : ""}>Brand New</option>
                            <option value="Fair" ${item.kondisiSerah === "Fair" ? "selected" : ""}>Fair</option>
                        </select>
                    </div>
                    <div class="pt-3">
                        <button type="button" class="btn btn-sm btn-outline-danger border-0" onclick="removeItemDetail(${idx})" title="Hapus Aset">
                            <i class="bi bi-trash fs-5"></i>
                        </button>
                    </div>
                </div>
            </div>
        `;
  });

  htmlContent += "</div>";
  $container.append(htmlContent);
}

// Submit Form Create Peminjaman
$(document).on("click", "#btnSubmitFormPeminjamanAset", function (e) {
  e.preventDefault();

  const karyawanId = $("#KaryawananId").val();
  if (!karyawanId || karyawanId === "") {
    toastr.error("Pilih Penerima Karyawan!", "Peringatan Validasi");
    $("#KaryawananId").focus();
    return;
  }

  const tanggalPinjam = $("#datepickerTanggalPinjam").val();
  if (!tanggalPinjam) {
    toastr.error("Tanggal pinjam wajib diisi!", "Peringatan Validasi");
    $("#datepickerTanggalPinjam").focus();
    return;
  }

  const tanggalJatuhTempo = $("#datepickerTanggalJatuhTempo").val();
  if (!tanggalJatuhTempo) {
    toastr.error("Tanggal jatuh tempo wajib diisi!", "Peringatan Validasi");
    $("#datepickerTanggalJatuhTempo").focus();
    return;
  }

  if (!selectedItems || selectedItems.length === 0) {
    toastr.error(
      "Silakan tambahkan minimal 1 item aset yang akan dipinjam.",
      "Peringatan Validasi",
    );
    return;
  }

  const formElement = document.getElementById("formPeminjamanAset");
  const formData = new FormData(formElement);

  // Bersihkan/pastikan format detail terkirim secara berurutan agar ModelBinder ASP.NET Core tidak gagal parsing
  selectedItems.forEach((item, idx) => {
    formData.append(`Details[${idx}].ProdukItemId`, item.id);
    formData.append(`Details[${idx}].NamaBarang`, item.namaBarang);
    formData.append(`Details[${idx}].SerialNumber`, item.serialNumber || "");
    formData.append(`Details[${idx}].LokasiAwal`, item.lokasiAwal || "");
    formData.append(`Details[${idx}].Kuantitas`, item.kuantitas || 1);
    formData.append(
      `Details[${idx}].KondisiSerah`,
      item.kondisiSerah || "Good",
    );
  });

  $.ajax({
    url: $("#formPeminjamanAset").attr("action") || "/PeminjamanAset/Create",
    type: "POST",
    data: formData,
    contentType: false,
    processData: false,
    success: function (res) {
      if (res.success) {
        toastr.success(res.message || "Peminjaman aset berhasil disimpan!");
        setTimeout(
          () => (window.location.href = "/PeminjamanAset/Index"),
          1000,
        );
      } else {
        toastr.error(res.message || "Terjadi kesalahan saat menyimpan data.");
      }
    },
    error: () => toastr.error("Terjadi kesalahan koneksi server."),
  });
});

// Submit Form Pengembalian Aset
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
          setTimeout(
            () => (window.location.href = "/PeminjamanAset/Index"),
            1000,
          );
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
