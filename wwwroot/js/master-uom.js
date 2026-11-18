let currentPage = 1;
let pageSize = 10;
let searchKeyword = "";

// Konfigurasi Toastr (Posisi di atas tengah layar)
toastr.options = {
  closeButton: true,
  debug: false,
  newestOnTop: true,
  progressBar: true,
  positionClass: "toast-top-right",
  preventDuplicates: false,
  onclick: null,
  showDuration: "300",
  hideDuration: "1000",
  timeOut: "3000",
  extendedTimeOut: "1000",
  showEasing: "swing",
  hideEasing: "linear",
  showMethod: "fadeIn",
  hideMethod: "fadeOut",
};

$(document).ready(function () {
  loadUomData();

  // Event pencarian dengan debounce
  let searchTimer;
  $("#searchKeyword").on("keyup", function () {
    clearTimeout(searchTimer);
    searchKeyword = $(this).val();
    searchTimer = setTimeout(function () {
      currentPage = 1;
      loadUomData();
    }, 400);
  });

  // Tombol Tambah diklik (Reset form bersih tanpa error merah)
  $("#btnTambah").on("click", function () {
    $("#uomForm")[0].reset();
    $("#Id").val("0");
    $("#uomModalLabel").html(
      '<i class="bi bi-plus-circle me-2"></i> Tambah UOM Baru',
    );

    // Bersihkan kelas validasi/error
    $("#uomForm")
      .find(".form-control")
      .removeClass("input-validation-error valid-field");
    $("#uomForm").validate().resetForm();

    $("#uomModal").modal("show");
  });

  // Custom konfigurasi jQuery Validation untuk live validasi warna (Merah / Hijau)
  $.validator.setDefaults({
    highlight: function (element) {
      $(element).addClass("input-validation-error").removeClass("valid-field");
    },
    unhighlight: function (element) {
      $(element).removeClass("input-validation-error").addClass("valid-field");
    },
  });

  // Submit Form (Create / Update) via AJAX
  $("#uomForm").on("submit", function (e) {
    e.preventDefault();

    if (!$(this).valid()) {
      toastr.warning(
        "Mohon lengkapi field yang wajib diisi dengan benar.",
        "Peringatan Validasi",
      );
      return;
    }

    var formData = {
      Id: $("#Id").val(),
      Code: $("#Code").val(),
      Name: $("#Name").val(),
      Description: $("#Description").val(),
      IsActive: $("#IsActive").is(":checked"),
    };

    var token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
      url: "/MasterUom/Save",
      type: "POST",
      data: {
        __RequestVerificationToken: token,
        model: formData,
      },
      success: function (response) {
        if (response.success) {
          toastr.success(response.message, "Berhasil");
          $("#uomModal").modal("hide");
          loadUomData();
        }
      },
      error: function (xhr) {
        let errorMsg = "Terjadi kesalahan pada sistem.";
        if (xhr.responseJSON && xhr.responseJSON.message) {
          errorMsg = xhr.responseJSON.message;
        }
        toastr.error(errorMsg, "Gagal");
      },
    });
  });
});

// Fungsi memuat data tabel melalui Ajax
function loadUomData() {
  $.ajax({
    url: "/MasterUom/GetData",
    type: "GET",
    data: {
      search: searchKeyword,
      page: currentPage,
      pageSize: pageSize,
    },
    success: function (res) {
      if (res.success) {
        let tbody = $("#tableUomBody");
        tbody.empty();

        if (res.data.length === 0) {
          tbody.append(
            `<tr><td colspan="6" class="text-center py-4 text-muted">Tidak ada data ditemukan.</td></tr>`,
          );
          $("#paginationInfo").text("Menampilkan 0 - 0 dari 0 data");
          $("#paginationContainer").empty();
          return;
        }

        let startRow = (res.currentPage - 1) * res.pageSize + 1;
        let endRow = Math.min(res.currentPage * res.pageSize, res.totalCount);
        $("#paginationInfo").text(
          `Menampilkan ${startRow} - ${endRow} dari ${res.totalCount} data`,
        );

        res.data.forEach((item, index) => {
          let badgeStatus = item.isActive
            ? `<span class="badge bg-success bg-opacity-10 text-success px-2 py-1">Aktif</span>`
            : `<span class="badge bg-danger bg-opacity-10 text-danger px-2 py-1">Non-Aktif</span>`;

          let row = `
                        <tr>
                            <td>${startRow + index}</td>
                            <td class="fw-bold">${escapeHtml(item.code)}</td>
                            <td>${escapeHtml(item.name)}</td>
                            <td>${escapeHtml(item.description || "-")}</td>
                            <td>${badgeStatus}</td>
                            <td class="text-center">
                                <button class="btn btn-sm btn-outline-primary me-1" onclick="editUom(${item.id})" title="Edit">
                                    <i class="bi bi-pencil-square"></i>
                                </button>
                                <button class="btn btn-sm btn-outline-danger" onclick="deleteUom(${item.id}, '${escapeHtml(item.code)}')" title="Hapus">
                                    <i class="bi bi-trash"></i>
                                </button>
                            </td>
                        </tr>
                    `;
          tbody.append(row);
        });

        renderPagination(res.totalPages, res.currentPage);
      }
    },
    error: function () {
      toastr.error("Gagal memuat data dari server.", "Error");
    },
  });
}

// Fungsi Render Pagination Bootstrap
function renderPagination(totalPages, currentPage) {
  let container = $("#paginationContainer");
  container.empty();

  if (totalPages <= 1) return;

  // Tambahkan class pagination-sm agar ukurannya kecil
  container.addClass("pagination-sm");

  // Tombol Previous
  let prevDisabled = currentPage === 1 ? "disabled" : "";
  container.append(`
        <li class="page-item ${prevDisabled}">
            <a class="page-link" href="#" onclick="changePage(${currentPage - 1})">Previous</a>
        </li>
    `);

  // Batasi rentang halaman (5 ke kiri dan 5 ke kanan dari currentPage)
  let startPage = Math.max(1, currentPage - 5);
  let endPage = Math.min(totalPages, currentPage + 5);

  // Opsi tambahan agar rentang selalu menampilkan hingga 10 tombol jika memungkinkan
  if (currentPage <= 6) {
    endPage = Math.min(totalPages, 11);
  }
  if (currentPage + 5 >= totalPages) {
    startPage = Math.max(1, totalPages - 10);
  }

  // Tombol halaman pertama & Ellipsis jika jauh dari startPage
  if (startPage > 1) {
    container.append(`
            <li class="page-item">
                <a class="page-link" href="#" onclick="changePage(1)">1</a>
            </li>
        `);
    if (startPage > 2) {
      container.append(`
                <li class="page-item disabled">
                    <span class="page-link">...</span>
                </li>
            `);
    }
  }

  // Nomor Halaman dalam rentang
  for (let i = startPage; i <= endPage; i++) {
    let active = i === currentPage ? "active" : "";
    container.append(`
            <li class="page-item ${active}">
                <a class="page-link" href="#" onclick="changePage(${i})">${i}</a>
            </li>
        `);
  }

  // Ellipsis & Tombol halaman terakhir jika jauh dari endPage
  if (endPage < totalPages) {
    if (endPage < totalPages - 1) {
      container.append(`
                <li class="page-item disabled">
                    <span class="page-link">...</span>
                </li>
            `);
    }
    container.append(`
            <li class="page-item">
                <a class="page-link" href="#" onclick="changePage(${totalPages})">${totalPages}</a>
            </li>
        `);
  }

  // Tombol Next
  let nextDisabled = currentPage === totalPages ? "disabled" : "";
  container.append(`
        <li class="page-item ${nextDisabled}">
            <a class="page-link" href="#" onclick="changePage(${currentPage + 1})">Next</a>
        </li>
    `);
}

function changePage(page) {
  if (page < 1) return;
  currentPage = page;
  loadUomData();
}

// Fungsi Edit UOM (Ambil data untuk dimasukkan ke Modal)
function editUom(id) {
  $.ajax({
    url: "/MasterUom/GetById",
    type: "GET",
    data: { id: id },
    success: function (res) {
      if (res.success) {
        let data = res.data;
        $("#Id").val(data.id);
        $("#Code").val(data.code);
        $("#Name").val(data.name);
        $("#Description").val(data.description);
        $("#IsActive").prop("checked", data.isActive);

        $("#uomModalLabel").html(
          '<i class="bi bi-pencil-square me-2"></i> Edit UOM',
        );
        $("#uomForm")
          .find(".form-control")
          .removeClass("input-validation-error")
          .addClass("valid-field");
        $("#uomForm").validate().resetForm();

        $("#uomModal").modal("show");
      }
    },
    error: function () {
      toastr.error("Gagal mengambil data untuk diedit.", "Gagal");
    },
  });
}

// Fungsi Hapus UOM dengan SweetAlert2 & Toastr
function deleteUom(id, code) {
  Swal.fire({
    title: "Konfirmasi Hapus",
    text: `Apakah Anda yakin ingin menghapus UOM dengan kode "${code}"?`,
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#311b92",
    cancelButtonColor: "#d33",
    confirmButtonText: "Ya, Hapus!",
    cancelButtonText: "Batal",
  }).then((result) => {
    if (result.isConfirmed) {
      var token = $('input[name="__RequestVerificationToken"]').val();

      $.ajax({
        url: "/MasterUom/Delete",
        type: "POST",
        data: {
          __RequestVerificationToken: token,
          id: id,
        },
        success: function (res) {
          if (res.success) {
            toastr.success(res.message, "Berhasil");
            loadUomData();
          }
        },
        error: function (xhr) {
          let errorMsg = "Gagal menghapus data.";
          if (xhr.responseJSON && xhr.responseJSON.message) {
            errorMsg = xhr.responseJSON.message;
          }
          toastr.error(errorMsg, "Gagal");
        },
      });
    }
  });
}

// Mencegah XSS injection sederhana
function escapeHtml(text) {
  if (!text) return "";
  return $("<div>").text(text).html();
}
