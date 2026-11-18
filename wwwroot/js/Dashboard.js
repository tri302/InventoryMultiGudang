let chartMutasiInstance = null;
let chartStatusAsetInstance = null;
let currentGlobalPage = 1;
let chartValuasiGudangInstance = null;
let chartDistribusiKategoriInstance = null;
let chartPenguasaanDeptInstance = null;
let chartKomposisiValuasiTipeInstance = null;

$(document).ready(function () {
  // Set default tanggal input (14 hari terakhir)
  var defaultEnd = new Date();
  var defaultStart = new Date();
  defaultStart.setDate(defaultEnd.getDate() - 13);

  $("#tglMulaiFilter").val(defaultStart.toISOString().split("T")[0]);
  $("#tglAkhirFilter").val(defaultEnd.toISOString().split("T")[0]);

  // Initial load
  loadDashboardData(1);

  // Event Listener Filter Gudang & Tanggal
  $("#filterGudang").on("change", function () {
    loadDashboardData(1);
  });

  $("#btnFilterGrafik").on("click", function () {
    loadDashboardData(1);
  });
});

function loadDashboardData(page) {
  var gudangId = $("#filterGudang").val();
  var tglMulai = $("#tglMulaiFilter").val();
  var tglAkhir = $("#tglAkhirFilter").val();

  $.ajax({
    url: "/Dashboard/GetDashboardData",
    type: "GET",
    data: {
      gudangId: gudangId,
      tglMulai: tglMulai,
      tglAkhir: tglAkhir,
      page: page,
    },
    dataType: "json",
    success: function (response) {
      if (response.success) {
        var d = response.data;
                
        var $dropdown = $("#filterGudang");
        var selectedVal = $dropdown.val(); // Simpan pilihan yang sedang aktif
        
        // Cek apakah opsi gudang sudah pernah dirender atau belum
        if ($dropdown.children("option").length <= 1 && d.listGudang) {
            $.each(d.listGudang, function (i, gudang) {
                $dropdown.append($('<option>', {
                    value: gudang.id,
                    text: gudang.namaGudang
                }));
            });
            $dropdown.val(selectedVal); // Kembalikan nilai yang dipilih sebelumnya
        }
        // ============================================================

        renderOverdueLoans(d.overdueLoanList);
        renderStokKritisList(d.stokKritisList);
        renderChartValuasiGudang(d.valuasiGudangList);
        renderChartDistribusiKategori(d.distribusiKategoriList);
        renderChartPenguasaanDepartemen(d.penguasaanDepartemenList);
        renderChartKomposisiValuasiTipe(d.komposisiValuasiTipe);

        $("#valStokKritisCount").text(d.totalStokKritis + " varian produk");

        // 1. Update KPI Card Data
        $("#valValuasiFinansial").text(
          "Rp " + formatRupiah(d.totalValuasiFinansial),
        );
        $("#valTotalUnitSn").html(
          d.totalUnitAsetSn +
            ' <span class="fs-6 fw-normal text-muted">Unit</span>',
        );
        $("#valAsetCustody").html(
          d.totalAsetCustody +
            ' <span class="fs-6 fw-normal text-muted">Unit</span>',
        );
        $("#valPeminjamanOverdue").html(
          d.totalPeminjamanOverdue +
            ' <span class="fs-6 fw-normal text-muted">Dokumen</span>',
        );
        $("#valTotalSku").text(d.totalVarianSku + " Item");
        $("#valTotalStokFisik").text(d.totalStokFisik + " Pcs");
        $("#valGudangAktif").text(d.totalGudangAktif + " Lokasi");
        $("#valAsetDipinjam").text(d.totalAsetDipinjam + " Unit");
        $("#valVarianHabis").text(d.totalVarianHabis + " SKU");

        // 2. Render Kedua Grafik ApexCharts
        renderChartMutasiGudang(d.grafikMutasi);
        renderChartStatusAset(d.grafikStatusAset);

        // 3. Render Tabel Aktivitas Mutasi & Pagination
        renderTabelMutasi(d.listAktivitasMutasi);
        renderPaginationInfoAndControls(
          d.currentPage,
          d.pageSize,
          d.totalItems,
          d.totalPages,
        );
      }
    },
    error: function () {
      toastr.error("Terjadi kesalahan saat memuat data dashboard.", "Error");
    },
  });
}

/**
 * Render Tabel Aktivitas Mutasi (Jurnal Kartu Stok)
 */
function renderTabelMutasi(listData) {
  var $tbody = $("#tableAktivitasMutasi tbody");
  $tbody.empty();

  if (!listData || listData.length === 0) {
    $tbody.append(
      '<tr><td colspan="7" class="text-center text-muted py-3">Tidak ada data aktivitas mutasi untuk rentang waktu ini.</td></tr>',
    );
    return;
  }

  $.each(listData, function (i, item) {
    // Tentukan badge warna berdasarkan tipe transaksi
    var badgeClass = "bg-secondary-subtle text-secondary";
    if (item.tipeTransaksi === "Stock In") {
      badgeClass = "bg-success-subtle text-success";
    } else if (item.tipeTransaksi === "Stock Out") {
      badgeClass = "bg-danger-subtle text-danger";
    } else if (item.tipeTransaksi && item.tipeTransaksi.includes("Transfer")) {
      badgeClass = "bg-warning-subtle text-warning";
    }

    var row = `<tr>
            <td>
                <div class="fw-bold text-dark">${item.namaProduk}</div>
                <small class="text-muted" style="font-size: 11px;">SKU: ${item.sku}</small>
            </td>
            <td>${item.namaGudang}</td>
            <td><span class="badge ${badgeClass} px-2 py-1 fw-bold">${item.tipeTransaksi}</span></td>
            <td class="fw-bold">${item.qtyMutasi}</td>
            <td>${item.saldoStok}</td>
            <td><span class="text-indigo fw-semibold">${item.nomorReferensi}</span></td>
            <td class="text-muted small">${item.waktuPencatatan}</td>
        </tr>`;
    $tbody.append(row);
  });
}

/**
 * Render Informasi Paginasi dan Tombol Bootstrap Pagination di Kanan
 */
function renderPaginationInfoAndControls(
  currentPage,
  pageSize,
  totalItems,
  totalPages,
) {
  var $infoText = $("#tableInfoText");
  var $paginationContainer = $("#paginationContainer");

  $paginationContainer.empty();

  if (totalItems === 0) {
    $infoText.text("Menampilkan 0 - 0 dari 0 data");
    return;
  }

  var startItem = (currentPage - 1) * pageSize + 1;
  var endItem = Math.min(currentPage * pageSize, totalItems);

  // Set teks informasi sebelah kiri: "Menampilkan 1 - 10 dari 20 data"
  $infoText.text(
    `Menampilkan ${startItem} - ${endItem} dari ${totalItems} data`,
  );

  // Render Tombol Previous
  var prevDisabled = currentPage === 1 ? "disabled" : "";
  $paginationContainer.append(`
        <li class="page-item ${prevDisabled}">
            <a class="page-link" href="#" data-page="${currentPage - 1}">Sebelumnya</a>
        </li>
    `);

  // Render Nomor Halaman
  for (var i = 1; i <= totalPages; i++) {
    var activeClass = i === currentPage ? "active" : "";
    $paginationContainer.append(`
            <li class="page-item ${activeClass}">
                <a class="page-link" href="#" data-page="${i}">${i}</a>
            </li>
        `);
  }

  // Render Tombol Next
  var nextDisabled =
    currentPage === totalPages || totalPages === 0 ? "disabled" : "";
  $paginationContainer.append(`
        <li class="page-item ${nextDisabled}">
            <a class="page-link" href="#" data-page="${currentPage + 1}">Selanjutnya</a>
        </li>
    `);

  // Event Klik Tombol Paginasi
  $paginationContainer.find(".page-link").on("click", function (e) {
    e.preventDefault();
    var selectedPage =
      $(this).closest(".page-item").hasClass("disabled") ||
      $(this).closest(".page-item").hasClass("active")
        ? null
        : $(this).data("page");

    if (selectedPage && selectedPage !== currentPage) {
      loadDashboardData(selectedPage);
    }
  });
}

function renderOverdueLoans(dataList) {
  var $tbody = $("#tableOverdueBody");
  $tbody.empty();

  if (!dataList || dataList.length === 0) {
    $tbody.append(
      '<tr><td colspan="5" class="text-center text-muted py-3">Tidak ada peminjaman overdue.</td></tr>',
    );
    return;
  }

  $.each(dataList, function (i, item) {
    var row = `<tr>
            <td class="fw-bold text-indigo">${item.nomorDokumen}</td>
            <td>${item.namaPeminjam}</td>
            <td>
                <div class="fw-bold text-dark">${item.namaBarang}</div>
                <small class="text-muted" style="font-size: 11px;">SN: ${item.serialNumber}</small>
            </td>
            <td class="text-muted">${item.batasKembali}</td>
            <td><span class="badge bg-danger-subtle text-danger px-2 py-1 fw-bold rounded-pill">${item.hariKeterlambatan} Hari</span></td>
        </tr>`;
    $tbody.append(row);
  });
}

function renderStokKritisList(dataList) {
  var $container = $("#listStokKritisContainer");
  $container.empty();

  if (!dataList || dataList.length === 0) {
    $container.append(
      '<p class="text-center text-muted small my-auto">Semua stok barang dalam kondisi aman (di atas safety stock).</p>',
    );
    return;
  }

  $.each(dataList, function (i, item) {
    var htmlItem = `
            <div>
                <div class="d-flex justify-content-between align-items-center mb-1 small">
                    <span class="fw-bold text-dark text-truncate" style="max-width: 65%;" title="${item.namaProduk}">${item.namaProduk}</span>
                    <span class="text-muted fw-semibold" style="font-size: 12px;">${item.stokSaatIni} / ${item.safetyStock} <span class="text-secondary">${item.sku}</span></span>
                </div>
                <div class="progress" style="height: 6px;">
                    <div class="progress-bar bg-warning" role="progressbar" style="width: ${item.persentase}%;" aria-valuenow="${item.persentase}" aria-valuemin="0" aria-valuemax="100"></div>
                </div>
                <small class="text-muted" style="font-size: 10px;">${item.namaGudang}</small>
            </div>
        `;
    $container.append(htmlItem);
  });
}

/**
 * Render Grafik 3: Distribusi Penguasaan Aset per Departemen (Horizontal Bar Chart)
 */
function renderChartPenguasaanDepartemen(dataList) {
  var categories =
    dataList.length > 0
      ? dataList.map((x) => x.namaDepartemen)
      : ["Belum Ada Data"];
  var values = dataList.length > 0 ? dataList.map((x) => x.jumlahAset) : [0];

  var options = {
    series: [{ name: "Jumlah Aset", data: values }],
    chart: {
      type: "bar",
      height: 300,
      toolbar: { show: false },
    },
    colors: ["#38bdf8"], // Warna Cyan/Sky-Blue terang persis gambar contoh
    plotOptions: {
      bar: {
        horizontal: true,
        barHeight: "35%",
        borderRadius: 5,
      },
    },
    dataLabels: {
      enabled: false, // Tanpa angka tebal di dalam bar
    },
    xaxis: {
      categories: categories,
      labels: {
        formatter: function (val) {
          return Math.floor(val) + " Unit";
        },
      },
    },
    tooltip: {
      y: {
        formatter: function (val) {
          return val + " Unit Aset";
        },
      },
    },
  };

  if (chartPenguasaanDeptInstance) {
    chartPenguasaanDeptInstance.destroy();
  }
  chartPenguasaanDeptInstance = new ApexCharts(
    document.querySelector("#chartPenguasaanDepartemen"),
    options,
  );
  chartPenguasaanDeptInstance.render();
}

/**
 * Render Grafik 4: Komposisi Valuasi per Tipe Produk (Donut Chart)
 */
function renderChartKomposisiValuasiTipe(dataKomposisi) {
  var valInventory = dataKomposisi ? dataKomposisi.valuasiInventory : 0;
  var valConsumable = dataKomposisi ? dataKomposisi.valuasiConsumable : 0;
  var valAsset = dataKomposisi ? dataKomposisi.valuasiAsset : 0;
  var totalVal = dataKomposisi ? dataKomposisi.totalValuasi : 0;

  var options = {
    series: [valInventory, valConsumable, valAsset],
    labels: [
      "Persediaan (Inventory)",
      "Habis Pakai (Consumable)",
      "Aset (Asset)",
    ],
    chart: {
      type: "donut",
      height: 300,
    },
    colors: ["#6366f1", "#f43f5e", "#10b981"], // Biru/Nila, Merah, Emerald persis contoh UI
    plotOptions: {
      pie: {
        donut: {
          size: "72%",
          labels: {
            show: true,
            total: {
              show: true,
              label: "Total",
              formatter: function () {
                return "Rp " + formatCompactRupiah(totalVal);
              },
            },
          },
        },
      },
    },
    legend: {
      position: "bottom",
      horizontalAlign: "center",
    },
    tooltip: {
      y: {
        formatter: function (val) {
          return "Rp " + formatRupiah(val);
        },
      },
    },
  };

  if (chartKomposisiValuasiTipeInstance) {
    chartKomposisiValuasiTipeInstance.destroy();
  }
  chartKomposisiValuasiTipeInstance = new ApexCharts(
    document.querySelector("#chartKomposisiValuasiTipe"),
    options,
  );
  chartKomposisiValuasiTipeInstance.render();
}

// Helper untuk format angka ringkas (misal: Rp 2.7M / Rp 500Jt)
function formatCompactRupiah(val) {
  if (val >= 1000000000) {
    return (val / 1000000000).toFixed(1) + "M";
  } else if (val >= 1000000) {
    return (val / 1000000).toFixed(1) + "Jt";
  } else if (val >= 1000) {
    return (val / 1000).toFixed(0) + "Rb";
  }
  return val;
}

function renderChartValuasiGudang(dataList) {
  var categories = dataList.map((x) => x.namaGudang);
  var values = dataList.map((x) => x.valuasi);

  var options = {
    series: [{ name: "Valuasi", data: values }],
    chart: {
      type: "bar",
      height: 280,
      toolbar: { show: false },
    },
    // Warna berbeda untuk setiap batang (bar)
    colors: ["#3b82f6", "#10b981", "#f59e0b", "#8b5cf6", "#ec4899"],
    plotOptions: {
      bar: {
        horizontal: true,
        borderRadius: 4,
        distributed: true, // Membuat setiap bar memiliki warna unik dari array 'colors'
      },
    },
    dataLabels: {
      enabled: false, // Menghilangkan angka/teks di dalam batang bar
    },
    xaxis: {
      categories: categories,
      labels: {
        formatter: function (val) {
          // Memformat angka sumbu X menjadi format ringkas (contoh: 15jt, 50jt) agar tidak bertumpuk
          if (val >= 1000000000) {
            return (val / 1000000000).toFixed(1) + " M";
          } else if (val >= 1000000) {
            return (val / 1000000).toFixed(0) + " Jt";
          } else if (val >= 1000) {
            return (val / 1000).toFixed(0) + " Rb";
          }
          return val;
        },
      },
    },
    legend: {
      show: false, // Karena menggunakan distributed colors per bar, legend nama gudang sudah ada di sumbu Y
    },
    tooltip: {
      y: {
        formatter: function (val) {
          return "Rp " + formatRupiah(val);
        },
      },
    },
  };

  if (chartValuasiGudangInstance) {
    chartValuasiGudangInstance.destroy();
  }
  chartValuasiGudangInstance = new ApexCharts(
    document.querySelector("#chartValuasiGudang"),
    options,
  );
  chartValuasiGudangInstance.render();
}

function renderChartDistribusiKategori(dataList) {
  var categories = dataList.map((x) => x.kategoriName);
  var skuData = dataList.map((x) => x.varianSku);
  var assetData = dataList.map((x) => x.jumlahAset);

  var options = {
    series: [
      { name: "Varian SKU", data: skuData },
      { name: "Jumlah Aset (Pcs)", data: assetData },
    ],
    chart: { type: "bar", height: 280, toolbar: { show: false } },
    colors: ["#8b5cf6", "#06b6d4"],
    plotOptions: {
      bar: { horizontal: false, columnWidth: "55%", borderRadius: 4 },
    },
    xaxis: { categories: categories },
    legend: { position: "top", horizontalAlign: "right" },
  };

  if (chartDistribusiKategoriInstance)
    chartDistribusiKategoriInstance.destroy();
  chartDistribusiKategoriInstance = new ApexCharts(
    document.querySelector("#chartDistribusiKategori"),
    options,
  );
  chartDistribusiKategoriInstance.render();
}

/**
 * Render Grafik 1: Aktivitas Mutasi Gudang (Line / Spline Area Chart)
 */
function renderChartMutasiGudang(dataMutasi) {
  var options = {
    series: [
      { name: "Stock In (Masuk)", data: dataMutasi.stockIn },
      { name: "Stock Out (Keluar)", data: dataMutasi.stockOut },
      { name: "Transfer (Mutasi)", data: dataMutasi.transfer },
      { name: "Penyesuaian (Opname)", data: dataMutasi.opname },
    ],
    chart: {
      type: "line",
      height: 320,
      toolbar: { show: false },
      zoom: { enabled: false },
    },
    colors: ["#10b981", "#ef4444", "#f59e0b", "#8b5cf6"], // Warna khas seperti desain
    stroke: { curve: "smooth", width: 3 },
    xaxis: { categories: dataMutasi.dates },
    yaxis: { min: 0, forceNiceScale: true },
    legend: { position: "top", horizontalAlign: "left" },
    tooltip: {
      shared: true,
      intersect: false,
    },
  };

  if (chartMutasiInstance) {
    chartMutasiInstance.destroy();
  }
  chartMutasiInstance = new ApexCharts(
    document.querySelector("#chartMutasiGudang"),
    options,
  );
  chartMutasiInstance.render();
}

/**
 * Render Grafik 2: Status Kepemilikan & Kondisi Aset (Donut Chart)
 */
function renderChartStatusAset(dataStatus) {
  var options = {
    series: [
      dataStatus.readyGudang,
      dataStatus.assignedCustody,
      dataStatus.onLoan,
      dataStatus.underRepair,
      dataStatus.lost,
      dataStatus.soldDisposal,
    ],
    labels: [
      "Ready (Gudang)",
      "Assigned (Custody)",
      "On Loan (Dipinjam)",
      "Under Repair (Servis)",
      "Lost (Hilang)",
      "Sold (Terjual/Disposal)",
    ],
    chart: {
      type: "donut",
      height: 320,
    },
    colors: [
      "#10b981",
      "#3b82f6",
      "#f59e0b",
      "#ec4899",
      "#ef4444",
      "#6b7280",
      "#8b5cf6",
    ],
    plotOptions: {
      pie: {
        donut: {
          size: "70%",
          labels: {
            show: true,
            total: {
              show: true,
              label: "Total Aset",
              formatter: function () {
                return dataStatus.totalAset + " Pcs";
              },
            },
          },
        },
      },
    },
    legend: { position: "bottom" },
  };

  if (chartStatusAsetInstance) {
    chartStatusAsetInstance.destroy();
  }
  chartStatusAsetInstance = new ApexCharts(
    document.querySelector("#chartStatusAset"),
    options,
  );
  chartStatusAsetInstance.render();
}

/**
 * Render Grafik 2: Status Kepemilikan & Kondisi Aset (Donut Chart)
 */
function renderChartStatusAset(dataStatus) {
  var options = {
    series: [
      dataStatus.readyGudang,
      dataStatus.assignedCustody,
      dataStatus.onLoan,
      dataStatus.underRepair,
      dataStatus.lost,
      dataStatus.soldDisposal,
    ],
    labels: [
      "Ready (Gudang)",
      "Assigned (Custody)",
      "On Loan (Dipinjam)",
      "Under Repair (Servis)",
      "Lost (Hilang)",
      "Sold (Terjual/Disposal)",
    ],
    chart: {
      type: "donut",
      height: 320,
    },
    colors: ["#10b981", "#3b82f6", "#f59e0b", "#ec4899", "#ef4444", "#6b7280"],
    plotOptions: {
      pie: {
        donut: {
          size: "70%",
          labels: {
            show: true,
            total: {
              show: true,
              label: "Total Aset",
              formatter: function (w) {
                return (
                  w.globals.seriesTotals.reduce((a, b) => a + b, 0) + " Pcs"
                );
              },
            },
          },
        },
      },
    },
    legend: { position: "bottom" },
  };

  if (chartStatusAsetInstance) {
    chartStatusAsetInstance.destroy();
  }
  chartStatusAsetInstance = new ApexCharts(
    document.querySelector("#chartStatusAset"),
    options,
  );
  chartStatusAsetInstance.render();
}

function formatRupiah(angka) {
  return new Intl.NumberFormat("id-ID").format(angka);
}
