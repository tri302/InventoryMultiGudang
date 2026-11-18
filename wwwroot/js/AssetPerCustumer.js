let currentPage = 1;
let searchQuery = "";

function loadData(page = 1) {
  $.ajax({
    url: '/LaporanAsset/GetAssetPerCustomerData', // Sesuaikan URL endpoint Anda jika berbeda
    type: "GET",
    data: { search: searchQuery, page: page, pageSize: 15 },
    success: function (response) {
      let rows = "";
      if (response.items.length === 0) {
        rows = `<tr><td colspan="5" class="text-center py-4 text-muted">Tidak ada data customer ditemukan.</td></tr>`;
      } else {
        response.items.forEach((item) => {
          rows += `
            <tr>
                <td class="ps-4 fw-bold text-dark">${item.kodeCustomer || item.kodeCust || "-"}</td>
                <td class="fw-bold text-dark">${item.namaCustomer || item.namaCust}</td>
                <td>
                    <div class="text-primary text-decoration-none">${item.email || "-"}</div>
                    <div class="text-muted small" style="font-size: 0.75rem;">${item.noTelepon || "-"}</div>
                </td>
                <td>
                    <span class="badge bg-primary-subtle text-primary border border-primary-subtle px-2.5 py-1" style="font-size: 0.75rem; font-weight: 600; border-radius: 6px;">${item.jumlahAsetAktif} Unit</span>
                </td>
                <td class="pe-4 text-center">
                    <button onclick="showDetail(${item.customerId || item.id}, '${item.namaCustomer || item.namaCust}')"
                            class="btn btn-sm bg-light border text-muted px-2 py-1 rounded shadow-sm" style="font-size: 0.9rem;">
                        <i class="bi bi-eye"></i>
                    </button>
                </td>
            </tr>`;
        });
      }
      $("#tableBody").html(rows);

      // Hitung rentang item yang sedang ditampilkan
      let startItem =
        response.totalCount === 0
          ? 0
          : (response.currentPage - 1) * response.pageSize + 1;
      let endItem = Math.min(
        response.currentPage * response.pageSize,
        response.totalCount,
      );
      
      // Update teks informasi di sebelah kiri footer (Format: Menampilkan X - Y dari Z data)
      $("#tableInfo").html(
        `Menampilkan <span class="fw-bold text-dark">${startItem}</span> - <span class="fw-bold text-dark">${endItem}</span> dari <span class="fw-bold text-dark">${response.totalCount}</span> data.`
      );

      let paginationHtml = "";
      let prevDisabled = response.currentPage === 1 ? "disabled" : "";
      paginationHtml += `<li class="page-item ${prevDisabled}"><a class="page-link text-muted border-0 bg-transparent" href="#" onclick="changePage(${response.currentPage - 1})">&laquo; Previous</a></li>`;

      // Hitung batasan maksimal 5 ke kiri dan 5 ke kanan dari halaman aktif
      let startPage = Math.max(1, response.currentPage - 5);
      let endPage = Math.min(response.totalPages, response.currentPage + 5);

      // Tampilkan halaman 1 & ellipsis awal jika rentang awal jauh dari 1
      if (startPage > 1) {
        paginationHtml += `<li class="page-item"><a class="page-link text-dark bg-white border rounded-3 mx-1" href="#" onclick="changePage(1)">1</a></li>`;
        if (startPage > 2) {
          paginationHtml += `<li class="page-item disabled"><span class="page-link border-0 bg-transparent">...</span></li>`;
        }
      }

      // Render nomor halaman dalam rentang limit (Max 5 Kiri & 5 Kanan)
      for (let i = startPage; i <= endPage; i++) {
        let activeClass =
          i === response.currentPage
            ? "bg-danger border-danger fw-bold text-white"
            : "text-dark bg-white";
        paginationHtml += `<li class="page-item"><a class="page-link border rounded-3 mx-1 ${activeClass}" href="#" onclick="changePage(${i})">${i}</a></li>`;
      }

      // Tampilkan ellipsis akhir & halaman terakhir jika rentang akhir jauh dari totalPages
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

function showDetail(customerId, customerName) {
  $("#modalCustomerName").text(customerName);
  $("#modalAssetBody").html(
    '<div class="text-center py-3 text-muted">Memuat data...</div>',
  );
  
  // Sesuaikan ID modal dengan yang ada di file .cshtml (modalAssetPerCustumer)
  var myModal = new bootstrap.Modal(
    document.getElementById("modalAssetPerCustumer"),
  );
  myModal.show();

  $.ajax({      
    url: '/LaporanAsset/GetCustomerAssetDetails',
    type: "GET",
    data: { customerId: customerId },
    success: function (data) {
      let html = "";
      if (data.length === 0) {
        html = `<div class="text-center text-muted py-3">Tidak ada aset aktif pada customer ini.</div>`;
      } else {
        data.forEach((item) => {
          html += `
            <div class="p-3 border-0 shadow-sm mb-2" style="background-color: #f8fafc; border: 1px solid #e2e8f0 !important; border-radius: 14px;">
                <div class="d-flex justify-content-between align-items-start mb-2 flex-wrap gap-2">
                    <span class="fw-bold text-dark" style="font-size: 0.925rem;">${item.namaBarang}</span>
                    <span class="px-2.5 py-1.5 fw-bold rounded-2 text-danger" style="background-color: #fff5f5; font-size: 0.725rem; letter-spacing: 0.3px;">
                        ${item.serialNumber}
                    </span>
                </div>
                <div class="row g-2 text-secondary" style="font-size: 0.8rem;">
                    <div class="col-6">
                        <div class="mb-1">SKU: <span class="fw-bold text-dark">${item.sku || '-'}</span></div>
                        <div>Kondisi: <span class="fw-bold text-dark">${item.kondisi || 'Good'}</span></div>
                    </div>
                    <div class="col-6 text-end">
                        <div class="mb-1">Rak/Bin: <span class="fw-semibold text-secondary">${item.rakBin || '-'}</span></div>
                        <div>Diserahkan: <span class="fw-bold text-dark">${item.tanggalDiserahkan || '-'}</span></div>
                    </div>
                </div>
            </div>`;
        });
      }
      $("#modalAssetBody").html(html);
    },
  });
}

function changePage(page) {
  if (page < 1) return;
  currentPage = page;
  loadData(currentPage);
}

// Inisialisasi utama saat dokumen siap
$(document).ready(function () {
  loadData(1);

  $("#searchInput").on("keypress", function (e) {
    if (e.which === 13) {
      searchQuery = $(this).val();
      currentPage = 1;
      loadData(currentPage);
    }
  });
});