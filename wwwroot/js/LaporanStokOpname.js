$(document).ready(function () {
  loadReportData();

  $("#filterStartDate, #filterEndDate, #filterGudang, #filterStatus").on(
    "change",
    function () {
      loadReportData();
    },
  );

  function loadReportData() {
    const filters = {
      startDate: $("#filterStartDate").val() || "",
      endDate: $("#filterEndDate").val() || "",
      gudangId: $("#filterGudang").val() || "",
      status: $("#filterStatus").val() || "",
    };

    $.ajax({
      url: "/LaporanStockOpname/GetFilteredData",
      type: "GET",
      data: filters,
      success: function (response) {
        updateSummaryCards(response.summary);
        updateDaftarSesiTable(response.daftarSesi);
        updateRekapitulasiTable(response.rekapitulasi);
      },
      error: function (xhr, status, error) {
        toastr.error("Gagal mengambil data laporan:", error);
      },
    });
  }

  function updateSummaryCards(summary) {
    $("#cardOpnameSelesai").text(`${summary.opnameSelesai} Sesi`);
    $("#cardItemDiperiksa").text(`${summary.itemDiperiksa} SKU`);
    $("#cardSelisihLebih").text(`+${summary.totalSelisihLebih} unit`);
    $("#cardSelisihKurang").text(`${summary.totalSelisihKurang} unit`);
  }

  function updateDaftarSesiTable(data) {
    const tbody = $("#tableDaftarSesi tbody");
    tbody.empty();

    if (!data || data.length === 0) {
      tbody.append(
        '<tr><td colspan="10" class="text-center text-muted py-3">Tidak ada data ditemukan.</td></tr>',
      );
      return;
    }

    data.forEach((item) => {
      const tanggalFormatted = new Date(item.tanggalPosting)
        .toLocaleString("id-ID", {
          day: "2-digit",
          month: "short",
          year: "numeric",
          hour: "2-digit",
          minute: "2-digit",
        })
        .replace(".", ",");

      const statusBadge =
        item.status === 1
          ? '<span class="badge bg-success-subtle text-success border border-success-subtle px-2 py-1"><i class="fas fa-check-circle me-1"></i> SELESAI</span>'
          : '<span class="badge bg-warning-subtle text-warning border border-warning-subtle px-2 py-1"><i class="fas fa-exclamation-triangle me-1"></i> DRAFT / PENDING</span>';

      const row = `
                <tr>
                    <td>${tanggalFormatted}</td>
                    <td class="fw-bold">${item.noReferensi}</td>
                    <td>${item.gudang}</td>
                    <td>${item.jumlahSku} SKU</td>
                    <td class="text-success fw-semibold">+${item.selisihLebih} unit</td>
                    <td class="text-danger fw-semibold">${item.selisihKurang} unit</td>
                    <td class="fw-semibold">${item.selisihBersih >= 0 ? "+" : ""}${item.selisihBersih} unit</td>
                    <td>${item.pencatat}</td>
                    <td>${statusBadge}</td>
                    <td>
                        <a href="/StockOpname/Details/${item.id}" class="btn btn-sm btn-light text-primary">
                            <i class="fas fa-eye me-1"></i> Detail
                        </a>
                    </td>
                </tr>
            `;
      tbody.append(row);
    });
  }

  function updateRekapitulasiTable(data) {
    const tbody = $("#tableRekapitulasi tbody");
    tbody.empty();

    if (!data || data.length === 0) {
      tbody.append(
        '<tr><td colspan="7" class="text-center text-muted py-3">Tidak ada data rekapitulasi ditemukan.</td></tr>',
      );
      return;
    }

    data.forEach((item) => {
      const selisihClass =
        item.selisihAkumulasi > 0
          ? "text-success"
          : item.selisihAkumulasi < 0
            ? "text-danger"
            : "";
      const row = `
                <tr>
                    <td class="fw-bold">${item.sku}</td>
                    <td>${item.namaProduk}</td>
                    <td><span class="badge bg-light text-dark border">${item.kategori}</span></td>
                    <td>${item.totalStokSistem}</td>
                    <td>${item.totalStokFisik}</td>
                    <td class="${selisihClass} fw-semibold">${item.selisihAkumulasi >= 0 ? (item.selisihAkumulasi > 0 ? "+" + item.selisihAkumulasi : "0") : item.selisihAkumulasi}</td>
                    <td><span class="badge bg-warning-subtle text-warning border">${item.frekuensiSelisih} Kali</span></td>
                </tr>
            `;
      tbody.append(row);
    });
  }

  // Event listener tombol cetak
  $("#btnCetak").on("click", function () {
    const startDate = $("#filterStartDate").val() || "";
    const endDate = $("#filterEndDate").val() || "";
    const gudangId = $("#filterGudang").val() || "";
    const status = $("#filterStatus").val() || "";

    const params = new URLSearchParams({
      startDate: startDate,
      endDate: endDate,
      gudangId: gudangId,
      status: status,
    });

    // Buka tab baru halaman cetak laporan dengan membawa parameter filter aktif
    window.open(`/LaporanStockOpname/Report?${params.toString()}`, "_blank");
  });
});
