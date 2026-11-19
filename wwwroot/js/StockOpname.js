/**
 * StockOpname.js - Module penanganan Ajax CRUD, Live Search, Pagination, dan Validasi
 */

var StockOpname = (function () {
  var currentPage = 1;

  // Inisialisasi Event Handler untuk Halaman Index
  function initIndex() {
    loadData(1);

    // Filter Live Search Ajax
    $("#searchKey").on("keyup input", function () {
      currentPage = 1;
      loadData(currentPage);
    });

    $("#filterGudang, #filterStatus").on("change", function () {
      currentPage = 1;
      loadData(currentPage);
    });

    // Load Gudang secara dinamis ke modalTargetAudit
    loadGudangOptionsModal();
  }

  // Fungsi Fetching Data Tabel Utama via Ajax
  function loadData(page) {
    currentPage = page;
    var searchVal = $("#searchKey").val();
    var gudangVal = $("#filterGudang").val();
    var statusVal = $("#filterStatus").val();

    $.ajax({
      url: "/StockOpname/GetPagedData",
      type: "GET",
      data: {
        search: searchVal,
        gudangId: gudangVal,
        status: statusVal,
        page: page,
        pageSize: 5,
      },
      success: function (res) {
        if (res.success) {
          renderTable(res.data.items);
          renderPagination(res.data);

          // Update Teks Informasi di Sebelah Kiri
          var totalItems = res.data.totalItems || 0;
          var pageSize = res.data.pageSize || 5;
          var pageNumber = res.data.pageNumber || page;
          
          var startItem = totalItems === 0 ? 0 : (pageNumber - 1) * pageSize + 1;
          var endItem = Math.min(pageNumber * pageSize, totalItems);

          $("#paginationInfoText").html(
            `Menampilkan <strong>${startItem}</strong> - <strong>${endItem}</strong> dari <strong>${totalItems}</strong> data`
          );
        } else {
          toastr.error("Gagal memuat data.");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan sistem saat memuat data.");
      },
    });
  }

  // Render Baris Tabel dengan Fallback jika Kosong
  function renderTable(items) {
    var $tbody = $("#tableBodyOpname");
    $tbody.empty();

    if (!items || items.length === 0) {
      // Fallback jika data masih kosong
      var emptyRow = `
                <tr>
                    <td colspan="6" class="text-center py-5 text-muted fw-semibold">
                        <i class="bi bi-inbox fs-2 d-block mb-2 text-secondary"></i>
                        Belum ada data untuk ditampilkan !
                    </td>
                </tr>`;
      $tbody.html(emptyRow);
      return;
    }

    $.each(items, function (idx, item) {
      var statusBadge =
        item.status === "Draft"
          ? `<div class="box-alert-amber inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-xs"><i class="bi bi-clock-history"></i> Draft</div>`
          : item.status === "Selesai"
            ? `<span class="badge-aktif d-inline-flex align-items-center gap-1"><i class="bi bi-check-circle-fill fs-7"></i> Selesai</span>`
            : `<span class="badge bg-danger-subtle text-danger px-3 py-1 rounded-full text-xs">Dibatalkan</span>`;

      var tglSelesai = item.tanggalSelesai
        ? new Date(item.tanggalSelesai).toLocaleDateString("id-ID", {
            day: "2-digit",
            month: "short",
            year: "numeric",
          })
        : '<span class="text-muted fst-italic small">Masih draft...</span>';

      var deleteBtn =
        item.status === "Draft"
          ? `<button onclick="StockOpname.confirmDelete(${item.id}, '${item.noReferensi}')" class="btn btn-light btn-sm rounded-2 text-danger border-0" title="Hapus"><i class="bi bi-trash fs-5"></i></button>`
          : "";

      var row = `
                <tr class="border-bottom" style="border-color: var(--border-color) !important;">
                    <td class="py-3 px-4">
                        <div class="d-flex align-items-start gap-3">
                            <div class="p-2 d-flex align-items-center justify-content-center rounded-3" style="background-color: #fef2f2; color: #ef4444; width: 42px; height: 42px;">
                                <i class="bi bi-clipboard-check fs-4"></i>
                            </div>
                            <div>
                                <h6 class="mb-1 fw-bold text-indigo-dark fs-6">${item.noReferensi}</h6>
                                <small class="text-muted text-truncate-2 d-block style-desc">${item.catatan || "-"}</small>
                            </div>
                        </div>
                    </td>
                    <td class="py-3">
                        <span class="fw-semibold text-indigo-dark d-block fs-6 mb-0">${item.namaGudang}</span>
                        <span class="text-muted tracking-wider fw-bold" style="font-size: 0.7rem;">${item.kodeGudang}</span>
                    </td>
                    <td class="py-3">
                        <span class="fw-semibold text-indigo-dark fs-6">${item.auditor}</span>
                    </td>
                    <td class="py-3">${statusBadge}</td>
                    <td class="py-3">${tglSelesai}</td>
                    <td class="py-3 px-4 text-end">
                        <div class="d-inline-flex gap-1">
                            <a href="/StockOpname/Details/${item.id}" class="btn btn-light btn-sm rounded-2 text-indigo border-0" title="Lihat Detail">
                                <i class="bi bi-eye fs-5"></i>
                            </a>
                            ${deleteBtn}
                        </div>
                    </td>
                </tr>`;
      $tbody.append(row);
    });
  }

  // Render Tombol Pagination Bootstrap (Dibatasi 5 ke kiri & 5 ke kanan)
  function renderPagination(pagedData) {
    var $pagi = $("#paginationContainer");
    $pagi.empty();

    if (pagedData.totalPages <= 1) {
      if (pagedData.totalItems === 0) {
        $("#paginationInfoText").text("Menampilkan 0 data");
      }
      return;
    }

    var html = `<ul class="pagination pagination-sm m-0 justify-content-end">`;

    // Prev Button
    var prevDisabled = pagedData.pageNumber === 1 ? "disabled" : "";
    html += `<li class="page-item ${prevDisabled}"><a class="page-link" href="javascript:void(0)" onclick="StockOpname.goToPage(${pagedData.pageNumber - 1})">Previous</a></li>`;

    // Hitung batasan 5 ke kiri dan 5 ke kanan dari halaman aktif
    var startPage = Math.max(1, pagedData.pageNumber - 5);
    var endPage = Math.min(pagedData.totalPages, pagedData.pageNumber + 5);

    // Tampilkan halaman 1 dan indikator "..." jika rentang awal jauh dari halaman 1
    if (startPage > 1) {
      html += `<li class="page-item"><a class="page-link" href="javascript:void(0)" onclick="StockOpname.goToPage(1)">1</a></li>`;
      if (startPage > 2) {
        html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
      }
    }

    // Render nomor halaman dalam rentang limit (Max 5 Kiri & 5 Kanan)
    for (var i = startPage; i <= endPage; i++) {
      var activeClass = i === pagedData.pageNumber ? "active" : "";
      html += `<li class="page-item ${activeClass}"><a class="page-link" href="javascript:void(0)" onclick="StockOpname.goToPage(${i})">${i}</a></li>`;
    }

    // Tampilkan halaman terakhir dan indikator "..." jika rentang akhir jauh dari totalPages
    if (endPage < pagedData.totalPages) {
      if (endPage < pagedData.totalPages - 1) {
        html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
      }
      html += `<li class="page-item"><a class="page-link" href="javascript:void(0)" onclick="StockOpname.goToPage(${pagedData.totalPages})">${pagedData.totalPages}</a></li>`;
    }

    // Next Button
    var nextDisabled =
      pagedData.pageNumber === pagedData.totalPages ? "disabled" : "";
    html += `<li class="page-item ${nextDisabled}"><a class="page-link" href="javascript:void(0)" onclick="StockOpname.goToPage(${pagedData.pageNumber + 1})">Next</a></li>`;

    html += `</ul>`;
    $pagi.html(html);
  }

  // Load Gudang Dinamis untuk Modal Target Audit
  function loadGudangOptionsModal() {
    $.ajax({
      url: "/StockOpname/GetGudangList",
      type: "GET",
      success: function (res) {
        if (res.success) {
          var $select = $("#selectGudangTarget");
          $select.empty();
          $select.append(
            '<option value="" selected disabled>-- Pilih Gudang --</option>',
          );
          $.each(res.data, function (i, g) {
            $select.append(
              `<option value="${g.id}">${g.kodeGudang} - ${g.namaGudang}</option>`,
            );
          });
        }
      },
    });
  }

  // Eksekusi Pemilihan Gudang
  function submitTargetGudang() {
    var gudangId = $("#selectGudangTarget").val();

    // Ambil token dari input terdekat atau meta tag/document
    var token = $('input[name="__RequestVerificationToken"]').val();

    if (!gudangId) {
      toastr.warning("Silakan pilih gudang target terlebih dahulu!");
      return;
    }

    var $btn = $("#btnSubmitTargetGudang");
    $btn
      .prop("disabled", true)
      .html(
        '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Memproses...',
      );

    $.ajax({
      url: "/StockOpname/InitAudit",
      type: "POST",
      contentType: "application/json",
      headers: {
        RequestVerificationToken: token,
      },
      data: JSON.stringify({ gudangId: parseInt(gudangId) }),
      success: function (res) {
        if (res.success) {
          window.location.href = res.redirectUrl;
        } else {
          toastr.error(res.message || "Gagal memulai audit.");
          $btn
            .prop("disabled", false)
            .html('<i class="bi bi-play-fill fs-5"></i> Mulai Audit Stok');
        }
      },
      error: function (xhr, status, error) {
        console.error("Error Detail:", xhr.responseText);
        toastr.error(
          "Terjadi kesalahan sistem (" + xhr.status + "). Cek console log.",
        );
        $btn
          .prop("disabled", false)
          .html('<i class="bi bi-play-fill fs-5"></i> Mulai Audit Stok');
      },
    });
  }

  // Parse Validasi Unobtrusive Form Dynamic
  function parseFormValidation($form) {
    if (
      typeof $.validator !== "undefined" &&
      typeof $.validator.unobtrusive !== "undefined"
    ) {
      $form.removeData("validator").removeData("unobtrusiveValidation");
      $.validator.unobtrusive.parse($form);
    }
  }

  // Simpan Draf Opname (Create / Edit)
  function saveDraftOpname() {
    var $form = $("#formAuditStockOpname");
    parseFormValidation($form);

    if ($form.length && !$form.valid()) {
      toastr.warning("Mohon lengkapi semua field yang wajib diisi!");
      return;
    }

    var model = {
      id: parseInt($("#AuditId").val() || "0"),
      gudangId: parseInt($("#GudangId").val()),
      catatan: $("#CatatanAudit").val(),
      items: [],
    };

    $(".audit-item-row").each(function () {
      var $row = $(this);
      var produkId = parseInt($row.find(".produk-id").val());
      var stokSistem = parseInt($row.find(".stok-sistem").text()) || 0;

      // Ambil nilai alasan selisih dan keterangan dari baris item terkait jika ada
      var alasanSelisihVal = $row.find(".alasan-selisih-input").val() || "";
      var keteranganVal = $row.find(".keterangan-input").val() || "";

      var hasSnItems = false;
      var snDetails = [];

      // Cek apakah produk ini memiliki Serial Number (SN Items)
      $row.find(".sn-select-item").each(function () {
        hasSnItems = true;
        var $snBox = $(this);
        var $snContainer = $snBox.closest(".p-3"); // Container pembungkus per item SN

        var snVal = $snBox.data("sn") || $snBox.attr("data-sn");
        var kondisiVal = $snBox.val();
        var produkItemId =
          parseInt(
            $snBox.data("produkitemid") || $snBox.attr("data-produkitemid"),
          ) || 0;

        // Ambil alasan dan keterangan spesifik dari card SN tersebut
        var alasanSn = $snContainer.find(".sn-alasan").val() || "";
        var ketSn = $snContainer.find(".sn-keterangan").val() || "";

        // Jika kondisi OK, kosongkan alasannya agar tidak tersimpan sia-sia di database
        if (kondisiVal === "Ditemukan (OK)") {
          alasanSn = "";
          ketSn = "";
        }

        var fisikItem = kondisiVal !== "Hilang (Lost)" ? 1 : 0;

        snDetails.push({
          produkId: produkId,
          produkItemId: produkItemId,
          stokSistem: stokSistem,
          stokFisik: fisikItem,
          kondisi: kondisiVal,
          status: kondisiVal,
          serialNumber: snVal,
          alasanSelisih: alasanSn,
          keterangan: ketSn,
        });
      });
      if (hasSnItems) {
        // Jika produk ber-SN, masukkan semua detail per nomor serinya
        snDetails.forEach(function (detail) {
          model.items.push(detail);
        });
      } else {
        // Jika produk non-SN (tidak punya serial number), ambil dari input manual stok fisik
        var stokFisikManual =
          parseInt($row.find(".stok-fisik-manual").val()) || 0;
        var itemNonSn = {
          produkId: produkId,
          produkItemId: 0,
          stokSistem: stokSistem,
          stokFisik: stokFisikManual,
          kondisi: "Ditemukan (OK)",
          status: "Ditemukan (OK)",
          alasanSelisih: alasanSelisihVal,
          keterangan: keteranganVal,
        };
        model.items.push(itemNonSn);
      }
    });

    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
      url: "/StockOpname/SaveDraft",
      type: "POST",
      contentType: "application/json",
      headers: { RequestVerificationToken: token },
      data: JSON.stringify(model),
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          setTimeout(function () {
            window.location.href = res.redirectUrl;
          }, 1000);
        } else {
          toastr.error(res.message);
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan saat menyimpan draf.");
      },
    });
  }

  // Eksekusi Batalkan Audit
  function executeBatalkanAudit(id) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
      url: "/StockOpname/BatalkanAudit/" + id,
      type: "POST",
      headers: { RequestVerificationToken: token },
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          setTimeout(function () {
            window.location.href = res.redirectUrl;
          }, 1000);
        } else {
          toastr.error(res.message);
        }
      },
    });
  }

  // Eksekusi Selesaikan Audit & Sinkronisasi
  function executeSelesaikanAudit(id) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
      url: "/StockOpname/SelesaikanAudit/" + id,
      type: "POST",
      headers: { RequestVerificationToken: token },
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          setTimeout(function () {
            window.location.href = res.redirectUrl;
          }, 1000);
        } else {
          toastr.error(res.message);
        }
      },
    });
  }

  // Konfirmasi dan Eksekusi Hapus
  function confirmDelete(id, noRef) {
    $("#deleteOpnameId").val(id);
    $("#deleteNoRefText").text(noRef);
    $("#modalHapusOpname").modal("show");
  }

  function executeDelete() {
    var id = $("#deleteOpnameId").val();
    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
      url: "/StockOpname/Delete/" + id,
      type: "POST",
      headers: { RequestVerificationToken: token },
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          $("#modalHapusOpname").modal("hide");
          loadData(currentPage);
        } else {
          toastr.error(res.message);
        }
      },
    });
  }

  return {
    initIndex: initIndex,
    goToPage: function (p) {
      loadData(p);
    },
    submitTargetGudang: submitTargetGudang,
    saveDraftOpname: saveDraftOpname,
    executeBatalkanAudit: executeBatalkanAudit,
    executeSelesaikanAudit: executeSelesaikanAudit,
    confirmDelete: confirmDelete,
    executeDelete: executeDelete,
  };
})();
