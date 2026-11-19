$(document).ready(function () {
let currentPage = 1;
let searchQuery = "";

function loadData(page = 1) {
  $.ajax({
    url: '/LaporanAsset/GetSerialNumbers',
    type: "GET",
    data: { search: searchQuery, page: page, pageSize: 15 },
    success: function (response) {
      let rows = "";
      if (response.items.length === 0) {
        rows = `<tr><td colspan="6" class="text-center py-4 text-muted">Tidak ada data ditemukan.</td></tr>`;
      } else {
        response.items.forEach((item) => {
          rows += `
          <tr>
          <td class="ps-4">
            <span class="px-3 py-2 fw-bold rounded-2 text-danger-emphasis" style="background-color: #fff5f5; font-size: 0.825rem; letter-spacing: 0.5px;">${item.serialNumber}</span>
          </td>
          <td>
            <div class="fw-bold text-dark mb-0">${item.namaBarang}</div>
            <div class="text-muted" style="font-size: 0.725rem;">SKU: <span class="text-uppercase">${item.sku}</span> | Brand: ${item.merek || "—"}</div>
          </td>
          <td>
            <span class="text-dark fw-medium"><i class="bi bi-building me-1 text-muted"></i> ${item.kodeGudang}</span>
            <span class="text-muted mx-1">/</span>
            <span class="text-muted small">${item.namaRak}</span>
          </td>
          <td class="text-muted">—</td>
          <td>
            <span class="badge bg-success-subtle text-success border border-success-subtle px-2.5 py-1 text-uppercase" style="font-size: 0.7rem; font-weight: 700; letter-spacing: 0.5px;">${item.status}</span>
          </td>
          <td class="pe-4 text-center">
            <a href="/LaporanAsset/Details?sn=${item.serialNumber}" class="btn btn-sm btn-light border text-muted px-2 py-1 rounded shadow-sm">
              <i class="bi bi-arrow-right-short" style="font-size: 1.1rem;"></i>
            </a>
          </td>
        </tr>`;
        });
      }
      $("#tableBody").html(rows);

      // Hitung rentang item yang sedang ditampilkan (Format: Menampilkan X - Y dari Z unit)
      let startItem =
        response.totalCount === 0
          ? 0
          : (response.currentPage - 1) * response.pageSize + 1;
      let endItem = Math.min(
        response.currentPage * response.pageSize,
        response.totalCount,
      );
      
      $("#tableInfo").html(
        `Menampilkan <span class="fw-bold text-dark">${startItem}</span> - <span class="fw-bold text-dark">${endItem}</span> dari <span class="fw-bold text-dark">${response.totalCount}</span> unit.`,
      );

      // Render Bootstrap Pagination dengan Batasan Maksimal 5 ke Kiri & 5 ke Kanan
      let paginationHtml = "";
      let prevDisabled = response.currentPage === 1 ? "disabled" : "";
      paginationHtml += `<li class="page-item ${prevDisabled}"><a class="page-link text-muted border-0 bg-transparent" href="#" onclick="changePage(${response.currentPage - 1})">&laquo; Previous</a></li>`;

      let startPage = Math.max(1, response.currentPage - 5);
      let endPage = Math.min(response.totalPages, response.currentPage + 5);

      if (startPage > 1) {
        paginationHtml += `<li class="page-item"><a class="page-link text-dark bg-white border rounded-3 mx-1" href="#" onclick="changePage(1)">1</a></li>`;
        if (startPage > 2) {
          paginationHtml += `<li class="page-item disabled"><span class="page-link border-0 bg-transparent">...</span></li>`;
        }
      }

      for (let i = startPage; i <= endPage; i++) {
        let activeClass =
          i === response.currentPage
            ? "bg-danger border-danger fw-bold text-white"
            : "text-dark bg-white";
        paginationHtml += `<li class="page-item"><a class="page-link border rounded-3 mx-1 ${activeClass}" href="#" onclick="changePage(${i})">${i}</a></li>`;
      }

      if (endPage < response.totalPages) {
        if (endPage < response.totalPages - 1) {
          paginationHtml += `<li class="page-item disabled"><span class="page-link border-0 bg-transparent">...</span></li>`;
        }
        paginationHtml += `<li class="page-item"><a class="page-link text-dark bg-white border rounded-3 mx-1" href="#" onclick="changePage(${response.totalPages})">${response.totalPages}</a></li>`;
      }

      let nextDisabled =
        response.currentPage === response.totalPages ||
        response.totalPages === 0
          ? "disabled"
          : "";
      paginationHtml += `<li class="page-item ${nextDisabled}"><a class="page-link text-dark border-0 bg-transparent fw-medium" href="#" onclick="changePage(${response.currentPage + 1})">Next &raquo;</a></li>`;

      $("#paginationContainer").html(paginationHtml);
    },
  });
}

function changePage(page) {
  if (page < 1) return;
  currentPage = page;
  loadData(currentPage);
}

// Live Search event listener (Trigger saat tekan Enter atau mengetik)
$("#searchInput").on("keypress", function (e) {
  if (e.which === 13) {
    searchQuery = $(this).val();
    currentPage = 1;
    loadData(currentPage);
  }
});

// Load data pertama kali saat halaman dibuka
$(document).ready(function () {
  loadData(1);
});
});