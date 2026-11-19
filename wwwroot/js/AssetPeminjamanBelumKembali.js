$(document).ready(function () {
  let currentPage = 1;
  let searchQuery = "";

  function formatDate(dateString) {
    if (!dateString) return "-";
    const date = new Date(dateString);
    return date.toISOString().split("T")[0];
  }

  function loadData(page = 1) {
    $.ajax({
      url: '/LaporanAsset/GetPeminjamanBelumKembaliData',
      type: "GET",
      data: { search: searchQuery, page: page, pageSize: 15 },
      success: function (response) {
        let rows = "";
        if (response.items.length === 0) {
          rows = `<tr><td colspan="6" class="text-center py-4 text-muted">Tidak ada data peminjaman aktif/terlambat.</td></tr>`;
        } else {
          response.items.forEach((item) => {
            let badgeStatus = item.isOverdue
              ? `<span class="badge bg-danger-subtle text-danger border border-danger-subtle px-2.5 py-1 text-uppercase" style="font-size: 0.7rem; font-weight: 700; letter-spacing: 0.5px;">Terlambat (Overdue)</span>`
              : `<span class="badge bg-warning-subtle text-warning-emphasis border border-warning-subtle px-2.5 py-1 text-uppercase" style="font-size: 0.7rem; font-weight: 700; letter-spacing: 0.5px;">Dipinjam</span>`;

            let dateDisplay = `
                                <div class="d-flex align-items-center gap-1 text-dark fw-medium">
                                    <i class="bi bi-calendar3 text-muted me-1"></i>
                                    <span>${formatDate(item.tanggalPinjam)}</span>
                                    <span class="text-muted">→</span>
                                    <span class="${item.isOverdue ? "text-danger fw-bold" : ""}">${formatDate(item.tanggalJatuhTempo)}</span>
                                </div>`;

            let emailLink =
              item.emailPeminjam && item.emailPeminjam !== "-"
                ? `<a href="mailto:${item.emailPeminjam}" class="text-muted btn btn-link p-0" style="font-size: 1.1rem;" title="${item.emailPeminjam}"><i class="bi bi-envelope"></i></a>`
                : `<span class="text-muted">—</span>`;

            rows += `
                                <tr>
                                    <td class="ps-4 fw-bold text-dark">${item.kodePinjam}</td>
                                    <td>
                                        <div class="fw-bold text-dark mb-0">${item.namaPeminjam}</div>
                                        <div class="text-muted text-uppercase" style="font-size: 0.675rem; letter-spacing: 0.5px;">NIK: ${item.nikPeminjam}</div>
                                    </td>
                                    <td>
                                        <div class="d-inline-flex align-items-center bg-light px-2.5 py-1.5 rounded" style="font-size: 0.8rem; border: 1px solid #f1f5f9;">
                                            <span class="text-dark fw-medium me-1">${item.namaBarang}</span>
                                            <span class="text-danger" style="font-size: 0.75rem; font-weight: 500;">(${item.serialNumber})</span>
                                        </div>
                                    </td>
                                    <td>${dateDisplay}</td>
                                    <td>${badgeStatus}</td>
                                    <td class="pe-4 text-center">${emailLink}</td>
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

  // Event Handler untuk Ajax Live Search saat tekan Enter
  $("#searchInput").on("keypress", function (e) {
    if (e.which === 13) {
      searchQuery = $(this).val();
      currentPage = 1;
      loadData(currentPage);
    }
  });

  $(document).ready(function () {
    loadData(1);
  });
});