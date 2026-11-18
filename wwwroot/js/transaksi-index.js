$(document).ready(function () {
  let currentPage = 1;
  const pageSize = 5;
  let selectedTxId = null;

  // Select2 Filter Gudang
  $("#filterGudang").select2({
    theme: "bootstrap-5",
    placeholder: "Semua Gudang",
    allowClear: true,
    ajax: {
      url: "/TransaksiStokInOut/GetDropdownGudang",
      dataType: "json",
      delay: 300,
      data: function (params) {
        return { q: params.term, page: params.page || 1 };
      },
      processResults: function (data) {
        return {
          results: data.results,
          pagination: data.pagination,
        };
      },
    },
  });

  loadTableData(currentPage);

  let searchTimeout;
  $("#searchInput").on("keyup", function () {
    clearTimeout(searchTimeout);
    searchTimeout = setTimeout(function () {
      currentPage = 1;
      loadTableData(currentPage);
    }, 400);
  });

  $("#filterGudang, #filterTipe").on("change", function () {
    currentPage = 1;
    loadTableData(currentPage);
  });

  function loadTableData(page) {
    const search = $("#searchInput").val();
    const gudangId = $("#filterGudang").val();
    const tipe = $("#filterTipe").val();

    $.ajax({
      url: "/TransaksiStokInOut/GetPagedData",
      type: "GET",
      data: {
        search: search,
        gudangId: gudangId,
        tipe: tipe,
        page: page,
        pageSize: pageSize,
      },
      success: function (res) {
        if (res.success) {
          renderTable(res.data);
          renderPagination(res.totalCount, res.page, res.pageSize);
          $("#totalCountBadge").text(res.totalCount);
        }
      },
      error: function () {
        if (typeof toastr !== "undefined")
          toastr.error("Gagal memuat data transaksi!");
      },
    });
  }

  function renderTable(data) {
    const $tbody = $("#tableBody");
    $tbody.empty();

    if (!data || data.length === 0) {
      $tbody.html(`
                <tr>
                    <td colspan="7" class="text-center py-5 text-muted">
                        <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                        <strong>Belum ada data untuk ditampilkan !</strong>
                    </td>
                </tr>
            `);
      return;
    }

    data.forEach((item) => {
      const isStockIn = item.tipeTransaksi === "Stock In";
      const badgeClass = isStockIn
        ? "bg-success-subtle text-success"
        : "bg-danger-subtle text-danger";
      const iconBgClass = isStockIn
        ? 'style="background-color: var(--emerald-light); color: var(--emerald-green);"'
        : "";

      const row = `
                <tr>
                    <td class="py-3 px-4">
                        <div class="d-flex align-items-center gap-3">
                            <div class="avatar-box flex-shrink-0" ${iconBgClass}>
                                <i class="bi bi-file-earmark-text-fill fs-5"></i>
                            </div>
                            <span class="fw-bold text-indigo-dark">${item.nomorReferensi}</span>
                        </div>
                    </td>
                    <td class="py-3">
                        <div class="fw-semibold text-indigo-dark mb-0">${item.gudangNama || "-"}</div>
                        <small class="text-muted text-uppercase tracking-wide" style="font-size: 0.7rem;">${item.gudangKode || ""}</small>
                    </td>
                    <td class="py-3 text-center">
                        <span class="badge ${badgeClass} px-3 py-1.5 rounded-pill fw-bold small">${item.tipeTransaksi}</span>
                    </td>
                    <td class="py-3">
                        <div class="fw-bold text-indigo-dark">${item.skuCount} SKU</div>
                        <small class="text-muted">Total: ${item.totalUnit} unit</small>
                    </td>
                    <td class="py-3 text-muted fw-semibold">${item.pembuat}</td>
                    <td class="py-3 text-muted small">
                        <i class="bi bi-calendar3 me-1"></i> ${item.tanggalPosting}
                    </td>
                    <td class="py-3 text-center">
                        <div class="dropdown">
                            <button class="btn btn-action-view btn-sm dropdown-toggle fw-semibold align-middle" type="button" data-bs-toggle="dropdown">
                                Aksi
                            </button>
                            <ul class="dropdown-menu dropdown-menu-end border-0 shadow rounded-3 p-2">
                                <li><a class="dropdown-item rounded-2 py-2 text-muted small btn-detail" href="javascript:void(0)" data-id="${item.id}"><i class="bi bi-eye me-2 text-indigo"></i> Detail</a></li>
                                <li><a class="dropdown-item rounded-2 py-2 text-muted small btn-cetak" href="javascript:void(0)" data-id="${item.id}"><i class="bi bi-printer me-2 text-indigo"></i> Cetak</a></li>
                                <li><hr class="dropdown-divider my-1"></li>
                                <li><a class="dropdown-item rounded-2 py-2 text-danger small btn-hapus" href="javascript:void(0)" data-id="${item.id}"><i class="bi bi-trash3 me-2"></i> Hapus</a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            `;
      $tbody.append(row);
    });
  }

  function renderPagination(totalCount, page, pageSize) {
        const $pagination = $('#paginationContainer');
        $pagination.empty();

        const totalPages = Math.ceil(totalCount / pageSize);
        
        // Update teks informasi halaman di sebelah kiri footer
        $('#paginationInfoText').text(`Menampilkan halaman ${page} dari ${totalPages || 1} (Total: ${totalCount} data)`);

        if (totalPages <= 1) return;

        // Tombol Previous
        $pagination.append(`
            <li class="page-item ${page === 1 ? 'disabled' : ''}">
                <a class="page-link" href="javascript:void(0)" data-page="${page - 1}">Previous</a>
            </li>
        `);

        // Hitung batasan 5 ke kiri dan 5 ke kanan dari halaman aktif
        let startPage = Math.max(1, page - 5);
        let endPage = Math.min(totalPages, page + 5);

        // Tampilkan halaman 1 dan indikator "..." jika rentang awal jauh dari halaman 1
        if (startPage > 1) {
            $pagination.append(`
                <li class="page-item">
                    <a class="page-link" href="javascript:void(0)" data-page="1">1</a>
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
            $pagination.append(`
                <li class="page-item ${i === page ? 'active' : ''}">
                    <a class="page-link" href="javascript:void(0)" data-page="${i}">${i}</a>
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
                    <a class="page-link" href="javascript:void(0)" data-page="${totalPages}">${totalPages}</a>
                </li>
            `);
        }

        // Tombol Next
        $pagination.append(`
            <li class="page-item ${page === totalPages ? 'disabled' : ''}">
                <a class="page-link" href="javascript:void(0)" data-page="${page + 1}">Next</a>
            </li>
        `);
    }

  $(document).on("click", "#paginationContainer .page-link", function (e) {
    e.preventDefault();
    const page = $(this).data("page");
    if (page && page !== currentPage) {
      currentPage = page;
      loadTableData(currentPage);
    }
  });

  // 1. Trigger Modal Detail (FETCH DATA DARI DB)
  $(document).on("click", ".btn-detail", function (e) {
    e.preventDefault();
    selectedTxId = $(this).data("id");

    $.ajax({
      url: "/TransaksiStokInOut/GetDetailJson",
      type: "GET",
      data: { id: selectedTxId },
      success: function (res) {
        if (res.success) {
          const d = res.data;
          $("#detailJudul").text("Rincian Transaksi " + d.tipeTransaksi);
          $("#detailRef").text(d.nomorReferensi);
          $("#detailGudang").text(`${d.gudangNama} (${d.gudangKode})`);
          $("#detailUser").text(d.pembuat);
          $("#detailTanggal").text(d.tanggalPosting);
          $("#detailCatatan").text(d.catatan);

          // Render List Items
          const $list = $("#detailItemList");
          $list.empty();

          d.details.forEach((item) => {
            const row = `
                            <tr>
                                <td class="ps-3 py-3">
                                    <div class="fw-bold text-dark">${item.namaProduk}</div>
                                    <div class="text-muted fs-8">${item.sku}</div>
                                </td>
                                <td class="py-3 text-muted">
                                    [${item.kodeLokasi}] ${item.namaRak}
                                </td>
                                <td class="text-center fw-semibold text-dark">${item.jumlah} Unit</td>                                
                                <td class="text-end pe-3">
                                    <span class="fw-bold">${item.stokAwal}</span> 
                                    <i class="bi bi-arrow-right text-muted mx-1"></i> 
                                    <span class="fw-bold text-success">${item.stokAkhir}</span>
                                </td>
                            </tr>
                        `;
            $list.append(row);
          });

          $("#modalDetail").modal("show");
        } else {
          alert(res.message);
        }
      },
      error: function () {
        alert("Gagal mengambil data detail!");
      },
    });
  });

  // 2. Trigger Print Window (CETAK DARI DB)
  $(document).on("click", ".btn-cetak, #btnCetakDariDetail", function (e) {
    e.preventDefault();
    const txId = $(this).data("id") || selectedTxId;

    if (txId) {
      window.open("/TransaksiStokInOut/Print/" + txId, "_blank");
    }
  });

  // 3. Trigger Modal Hapus
  $(document).on("click", ".btn-hapus", function (e) {
    e.preventDefault();
    selectedTxId = $(this).data("id");
    $("#modalHapus").modal("show");
  });

  // 4. Konfirmasi Eksekusi Hapus
  $("#btnConfirmDelete").on("click", function () {
    if (!selectedTxId) return;

    $.ajax({
      url: "/TransaksiStokInOut/Delete",
      type: "POST",
      data: { id: selectedTxId },
      success: function (response) {
        $("#modalHapus").modal("hide");
        if (response.success) {
          if (typeof toastr !== "undefined") toastr.success(response.message);
          else alert(response.message);
          loadTableData(currentPage);
        } else {
          if (typeof toastr !== "undefined") toastr.error(response.message);
          else alert(response.message);
        }
      },
      error: function () {
        alert("Gagal menghapus transaksi.");
      },
    });
  });
});
