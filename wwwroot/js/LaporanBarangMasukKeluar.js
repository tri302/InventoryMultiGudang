$(document).ready(function () {  
  // Inisialisasi awal pemuatan data dropdown secara AJAX
  loadGudangDropdown("");
  loadProdukDropdown("");

  // Panggil fetchFilteredReportData() agar data dan metrik langsung terisi saat load pertama kali
  fetchFilteredReportData();

  // Event change otomatis pada semua field filter
  $(
    "#selectGudang, #selectTipeTransaksi, #selectNamaProduk, #tanggalMulai, #tanggalSelesai",
  ).on("change changeDate", function () {
    fetchFilteredReportData();
  });

  // Event ketika Gudang diubah -> Reset/Reload dropdown produk sesuai Gudang aktif
  $("#selectGudang").on("change", function () {
    let selectedGudangId = $(this).val();
    loadProdukDropdown("", selectedGudangId); // Reload produk berdasarkan gudang
    fetchFilteredReportData();
  });

  $("#searchGudangInput").on("keyup", function () {
    loadGudangDropdown($(this).val());
  });

  $("#searchProdukInput").on("keyup", function () {
    loadProdukDropdown($(this).val());
  });
});

// ==========================================
  // LIVE SEARCH GUDANG (AJAX)
  // ==========================================
  // Jika Anda ingin user bisa mengetik untuk mencari gudang, kita bisa mendengarkan 
  // event input pada elemen pencarian atau mengubah select menjadi searchable.
  // Jika menggunakan input teks terpisah (misal: #searchGudangInput):
  $("#searchGudangInput").on("keyup input", function () {
    let keyword = $(this).val();
    loadGudangDropdown(keyword);
  });

  // ==========================================
  // LIVE SEARCH PRODUK (AJAX)
  // ==========================================
  $("#searchProdukInput").on("keyup input", function () {
    let keyword = $(this).val();
    let selectedGudangId = $("#selectGudang").val();
    loadProdukDropdown(keyword, selectedGudangId);
  });

function loadGudangDropdown(searchTerm) {
  $.ajax({
    url: "/LaporanBarangMasukKeluar/GetGudangList",
    type: "GET",
    data: { q: searchTerm },
    success: function (response) {
      let $select = $("#selectGudang");
      $select.empty().append('<option selected value="">Semua Gudang</option>');
      if (response && response.length > 0) {
        $.each(response.slice(0, 5), function (index, item) {
          $select.append(`<option value="${item.id}">${item.text}</option>`);
        });
      }
    }
  });
}

// Fungsi load Produk dengan parameter opsional gudangId
function loadProdukDropdown(searchTerm, gudangId) {
  $.ajax({
    url: "/LaporanBarangMasukKeluar/GetProdukList",
    type: "GET",
    data: { q: searchTerm, gudangId: gudangId },
    success: function (response) {
      let $select = $("#selectNamaProduk");
      $select.empty().append('<option selected value="">--Pilih--</option>');
      if (response && response.length > 0) {
        // Batasi maksimal 5 data sesuai ketentuan
        $.each(response.slice(0, 5), function (index, item) {
          $select.append(`<option value="${item.id}">${item.text}</option>`);
        });
      }
    }
  });
}

function fetchFilteredReportData() {
  let dataFilter = {
    tanggalMulai: $("#tanggalMulai").val(),
    tanggalSelesai: $("#tanggalSelesai").val(),
    gudangId: $("#selectGudang").val(),
    tipeTransaksi: $("#selectTipeTransaksi").val(),
    produkId: $("#selectNamaProduk").val(),
  };

  $.ajax({
    url: "/LaporanBarangMasukKeluar/FilterData",
    type: "GET",
    data: dataFilter,
    success: function (response) {
      if (response.success) {
        let resData = response.data;
        let items = resData.listTransaksi || resData.ListTransaksi;

        updateTableContent(items);

        // Update metrik ringkasan secara dinamis
        $("#metricTotalMasuk").text(
          resData.totalUnitMasuk ?? resData.TotalUnitMasuk ?? 0,
        );
        $("#metricTotalKeluar").text(
          resData.totalUnitKeluar ?? resData.TotalUnitKeluar ?? 0,
        );
        let mutasi = resData.mutasiBersih ?? resData.MutasiBersih ?? 0;
        $("#metricMutasiBersih").text((mutasi > 0 ? "+" : "") + mutasi);
      }
    },
    error: function () {
      toastr.error("Gagal memuat data laporan.");
    },
  });
}

function updateTableContent(items) {
  let $tbody = $("#tabelLaporanBody");
  $tbody.empty();

  if (!items || items.length === 0) {
    $tbody.append(`
            <tr>
                <td colspan="8" class="text-center py-4 text-muted fw-semibold font-monospace">
                    Belum ada data untuk ditampilkan !
                </td>
            </tr>`);
    return;
  }

  $.each(items, function (index, item) {
    let isMasuk = item.tipeTransaksi.toLowerCase().includes("in");
    let badgeClass = isMasuk
      ? "bg-success-subtle text-success border-success-subtle"
      : "bg-danger-subtle text-danger border-danger-subtle";
    let badgeText = isMasuk ? "MASUK (IN)" : "KELUAR (OUT)";
    let totalClass = isMasuk ? "text-success" : "text-danger";
    let collapseId = `detailRow_${index}`;

    let mainRow = `
            <tr data-bs-toggle="collapse" data-bs-target="#${collapseId}" style="cursor: pointer;" class="accordion-toggle">
                <td class="ps-4 font-monospace text-muted small"><i class="bi bi-chevron-down small me-2"></i>${item.tanggalWaktu}</td>
                <td class="fw-semibold text-dark font-monospace">${item.noReferensi}</td>
                <td>${item.namaGudang}</td>
                <td class="text-center">
                    <span class="badge ${badgeClass} border rounded-pill px-2" style="font-size: 0.675rem; font-weight: 700;">${badgeText}</span>
                </td>
                <td class="text-muted">${item.supplierMitra}</td>
                <td class="text-center fw-medium text-dark">${item.jumlahSku} SKU</td>
                <td class="text-end fw-bold ${totalClass}">${item.totalUnit} unit</td>
                <td class="pe-4 text-muted">${item.pencatat}</td>
            </tr>`;

    let detailsHtml = "";
    if (item.details && item.details.length > 0) {
      $.each(item.details, function (dIndex, det) {
        detailsHtml += `
                    <tr>
                        <td class="fw-semibold font-monospace">${det.sku}</td>
                        <td>${det.namaProduk}</td>
                        <td class="fw-bold">${det.kuantitas} ${det.satuanJumlah}</td>
                        <td class="text-muted"><i class="bi bi-geo-alt me-1"></i>${det.lokasiRak}</td>
                        <td><span class="badge bg-light text-dark border">${det.serialNumbers}</span></td>
                    </tr>`;
      });
    } else {
      detailsHtml = `<tr><td colspan="5" class="text-center text-muted small">Tidak ada rincian barang.</td></tr>`;
    }

    let detailRow = `
    <tr>
        <td colspan="8" class="p-0 bg-light">
            <div id="${collapseId}" class="collapse p-3">
                <div class="card border shadow-none p-3">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <h6 class="fw-bold text-dark mb-0"><i class="bi bi-box-seam me-2"></i>Rincian Barang Transaksi</h6>
                        <span class="small text-muted italic">Keterangan: ${item.catatan || item.Catatan || "-"}</span>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-sm table-bordered mb-0 bg-white" style="font-size: 0.8rem;">
                            <thead class="table-light">
                                <tr>
                                    <th>SKU</th>
                                    <th>Nama Produk</th>
                                    <th>Jumlah</th>
                                    <th>Lokasi Rak</th>
                                    <th>Serial Numbers</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${detailsHtml}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </td>
    </tr>`;

    $tbody.append(mainRow + detailRow);
  });
}

function syncFilterCetak() {
  document.getElementById("hiddenTanggalMulai").value =
    $("#tanggalMulai").val() || "";
  document.getElementById("hiddenTanggalSelesai").value =
    $("#tanggalSelesai").val() || "";
  document.getElementById("hiddenGudangId").value =
    $("#selectGudang").val() || "";

  let tipe = $("#selectTipeTransaksi").val() || "";
  if (tipe === "Semua Tipe" || tipe === "--Pilih--") {
    tipe = "";
  }
  document.getElementById("hiddenTipeTransaksi").value = tipe;
  document.getElementById("hiddenProdukId").value =
    $("#selectNamaProduk").val() || "";
}
