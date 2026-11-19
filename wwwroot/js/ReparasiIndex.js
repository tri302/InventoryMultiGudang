let currentPage = 1;
let searchQuery = "";

$(document).ready(function () {
  loadData(1);

  // Inisialisasi Select2 untuk Aset dengan AJAX  
  $("#ProdukItemId").select2({
    theme: "bootstrap-5",
    placeholder: "-- Pilih Aset --",
    allowClear: true,
    dropdownParent: $("#modalReparasi"),
    ajax: {
      url: "/ReparasiAsets/GetProdukItemOptions",
      dataType: "json",
      delay: 250,
      data: function (params) {
        return {
          q: params.term || "", // Keyword pencarian
          page: params.page || 1 // Halaman aktif untuk infinite scroll / pagination select2
        };
      },
      processResults: function (data, params) {
        params.page = params.page || 1;
        return {
          results: data.items, // Format: [{ id: id, text: "SN - Nama Barang" }]
          pagination: {
            more: data.pagination.more // True jika masih ada sisa data berikutnya
          }
        };
      },
      cache: true
    },
    minimumInputLength: 0
  });  

  // Event handler pencarian (Enter)
  $("#searchInput").on("keypress", function (e) {
    if (e.which === 13) {
      searchQuery = $(this).val();
      currentPage = 1;
      loadData(currentPage);
    }
  });

  // ==========================================
  // FORMAT RUPIAH DINAMIS SAAT KETIK
  // ==========================================
  $("#Biaya").on("input", function (e) {
    let inputVal = $(this)
      .val()
      .replace(/[^0-9]/g, "");
    if (inputVal === "") {
      $(this).val("0");
    } else {
      let formatted = parseInt(inputVal, 10).toLocaleString("id-ID");
      $(this).val(formatted);
    }
    validateField(this);
  });

  // ==========================================
  // CUSTOM REALTIME VALIDATION (LIVE VALIDATION)
  // ==========================================
  function validateField(el) {
    let $el =$(el);
    let val = $el.val();
    let id = $el.attr("id");
    let isValid = true;

    if (id === "ProdukItemId") {
      isValid = val !== "" && val !== null;
      let $select2Container =$el
        .next(".select2-container")
        .find(".select2-selection");
      if (isValid) {
        $select2Container
          .removeClass("border-danger")
          .addClass("border-success");
        $el.siblings(".custom-error-msg").hide();
      } else {
        $select2Container
          .removeClass("border-success")
          .addClass("border-danger");
        $el.siblings(".custom-error-msg").show();
      }
    } else if (id === "Biaya") {
      let rawNum = val.replace(/[^0-9]/g, "");
      isValid = rawNum !== "" && parseInt(rawNum, 10) >= 0;
      applyStyle($el, isValid);
    } else {
      isValid = val !== "" && val !== null && val.trim() !== "";
      applyStyle($el, isValid);
    }
  }

  function applyStyle($el, isValid) {
    if (isValid) {
      $el
        .removeClass("is-invalid border-danger")
        .addClass("is-valid border-success");
      $el.siblings(".custom-error-msg").hide();
    } else {
      $el
        .removeClass("is-valid border-success")
        .addClass("is-invalid border-danger");
      $el.siblings(".custom-error-msg").show();
    }
  }

  $("#TanggalMasuk, #Keluhan").on("input change", function () {
    validateField(this);
  });

  $("#ProdukItemId").on("change", function () {
    validateField(this);
  });

  // ==========================================
  // EVENT SUBMIT FORM VIA AJAX
  // ==========================================
  $("#reparasiForm").on("submit", function (e) {
    e.preventDefault();

    let pItem = $("#ProdukItemId").val();
    let tMasuk = $("#TanggalMasuk").val();
    let keluhan = $("#Keluhan").val();
    let biayaStr = $("#Biaya")
      .val()
      .replace(/[^0-9]/g, "");

    let isFormValid = true;

    if (!pItem) {
      isFormValid = false;
      $("#ProdukItemId")
        .next(".select2-container")
        .find(".select2-selection")
        .addClass("border-danger");
      $("#ProdukItemId").siblings(".custom-error-msg").show();
    }
    if (!tMasuk) {
      isFormValid = false;
      applyStyle($("#TanggalMasuk"), false);
    }
    if (!keluhan || keluhan.trim() === "") {
      isFormValid = false;
      applyStyle($("#Keluhan"), false);
    }
    if (biayaStr === "" || parseInt(biayaStr, 10) < 0) {
      isFormValid = false;
      applyStyle($("#Biaya"), false);
    }

    if (!isFormValid) {
      toastr.warning(
        "Mohon lengkapi semua field yang wajib diisi dengan benar sebelum menyimpan.",
        "Peringatan Validasi!",
      );
      return;
    }

    // Konversi tanggal dari dd-mm-yyyy ke yyyy-mm-dd untuk C#
    function convertDateToISO(dateStr) {
      if (!dateStr) return null;
      let parts = dateStr.split("-");
      if (parts.length === 3) {
        let day = parts[0];
        let month = parts[1];
        let year = parts[2];
        if (year.length === 2) {
          year = "20" + year;
        }
        return `${year}-${month}-${day}`;
      }
      return dateStr;
    }

    let formData = {
      id: parseInt($("#Id").val()),
      produkItemId: parseInt($("#ProdukItemId").val()),
      tanggalMasuk: convertDateToISO($("#TanggalMasuk").val()),
      tanggalKeluar: $("#TanggalKeluar").val()
        ? convertDateToISO($("#TanggalKeluar").val())
        : null,
      keluhan: $("#Keluhan").val(),
      biaya: parseFloat(biayaStr),
      statusReparasi: parseInt($("#StatusReparasi").val()),
    };

    $.ajax({
      url: "/ReparasiAsets/SaveReparasi",
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(formData),
      success: function (res) {
        if (res.success) {
          bootstrap.Modal.getInstance(
            document.getElementById("modalReparasi"),
          ).hide();
          toastr.success(res.message, "Berhasil!");
          loadData(currentPage);
        } else {
          toastr.error(res.message, "Gagal!");
        }
      },
      error: function (err) {
        toastr.error(
          err.responseJSON?.message || "Terjadi kesalahan sistem.",
          "Error!",
        );
      },
    });
  });
});

// --- FUNGSI GLOBAL ---

function loadData(page = 1) {
  $.ajax({
    url: "/ReparasiAsets/GetReparasiData",
    type: "GET",
    data: { search: searchQuery, page: page, pageSize: 10 },
    success: function (res) {
      let rows = "";
      if (res.items.length === 0) {
        rows = `<tr><td colspan="7" class="text-center py-4 text-muted">Tidak ada data reparasi.</td></tr>`;
      } else {
        res.items.forEach((item) => {
          let badge =
            item.statusReparasi === 0
              ? "bg-warning-subtle text-warning"
              : item.statusReparasi === 1
                ? "bg-success-subtle text-success"
                : "bg-danger-subtle text-danger";
          rows += `
                    <tr>
                      <td class="ps-4 fw-bold text-danger">${item.serialNumber}</td>
                      <td>${item.namaBarang}</td>
                      <td>${item.keluhan}</td>
                      <td>${item.tanggalMasuk} → ${item.tanggalKeluar}</td>
                      <td>Rp ${item.biaya.toLocaleString("id-ID")}</td>
                      <td><span class="badge ${badge} border px-2.5 py-1 text-uppercase">${item.statusLabel}</span></td>
                      <td class="pe-4 text-center">
                        <button class="btn btn-sm btn-light border text-primary px-2 py-1 rounded me-1" onclick="openModalEdit(${item.id})"><i class="bi bi-pencil"></i></button>
                        <button class="btn btn-sm btn-light border text-danger px-2 py-1 rounded" onclick="deleteData(${item.id})"><i class="bi bi-trash"></i></button>
                      </td>
                    </tr>`;
        });
      }
      $("#tableBody").html(rows);
      $("#tableInfo").text(
        `Menampilkan halaman ${res.currentPage} dari ${res.totalPages || 1} (Total: ${res.totalCount} data)`,
      );

      let pagHtml = "";
      pagHtml += `<li class="page-item ${res.currentPage === 1 ? "disabled" : ""}"><a class="page-link" href="#" onclick="changePage(${res.currentPage - 1})">Previous</a></li>`;

      // Hitung batasan 5 ke kiri dan 5 ke kanan dari halaman aktif
      let startPage = Math.max(1, res.currentPage - 5);
      let endPage = Math.min(res.totalPages, res.currentPage + 5);

      // Tampilkan halaman 1 dan indikator "..." jika rentang awal jauh dari halaman 1
      if (startPage > 1) {
        pagHtml += `<li class="page-item"><a class="page-link" href="#" onclick="changePage(1)">1</a></li>`;
        if (startPage > 2) {
          pagHtml += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
      }

      // Render nomor halaman dalam rentang limit (Max 5 Kiri & 5 Kanan)
      for (let i = startPage; i <= endPage; i++) {
        pagHtml += `<li class="page-item ${i === res.currentPage ? "active" : ""}"><a class="page-link" href="#" onclick="changePage(${i})">${i}</a></li>`;
      }

      // Tampilkan halaman terakhir dan indikator "..." jika rentang akhir jauh dari totalPages
      if (endPage < res.totalPages) {
        if (endPage < res.totalPages - 1) {
          pagHtml += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
        pagHtml += `<li class="page-item"><a class="page-link" href="#" onclick="changePage(${res.totalPages})">${res.totalPages}</a></li>`;
      }
      pagHtml += `<li class="page-item ${res.currentPage === res.totalPages || res.totalPages === 0 ? "disabled" : ""}"><a class="page-link" href="#" onclick="changePage(${res.currentPage + 1})">Next</a></li>`;
      $("#paginationContainer").html(pagHtml);
    },
  });
}

function setSelect2Value(url, id, text) {
  let $select = $("#ProdukItemId");
  if (id) {
    // Bersihkan opsi lama yang manual agar tidak duplikat, lalu buat option baru dengan format yang sama
    $select.empty();
    let option = new Option(text, id, true, true);
    $select.append(option).trigger("change");
  } else {
    $select.empty().append('<option value="">-- Pilih Aset --</option>').val(null).trigger("change");
  }
}

// Helper untuk ubah format yyyy-mm-dd (dari server) ke dd-mm-yyyy untuk UI Datepicker
function formatDateToUI(dateString) {
  if (!dateString) return "";
  let datePart = dateString.split("T")[0];
  let p = datePart.split("-");
  if (p.length === 3) {
    return `${p[2]}-${p[1]}-${p[0]}`;
  }
  return datePart;
}

function openModalAdd() {
  $("#reparasiForm")[0].reset();
  $("#Id").val(0);
  $("#modalTitle").text("Tambah Reparasi Aset");

  $("#ProdukItemId").val(null).trigger("change");
  $("#Biaya").val("0");

  $("#reparasiForm input, #reparasiForm textarea").removeClass(
    "is-invalid is-valid border-danger border-success",
  );
  $("#ProdukItemId")
    .next(".select2-container")
    .find(".select2-selection")
    .removeClass("border-danger border-success");
  $(".custom-error-msg").hide();

  var myModal = new bootstrap.Modal(document.getElementById("modalReparasi"));
  myModal.show();
}

function openModalEdit(id) {
  $.get("/ReparasiAsets/GetReparasiById", { id: id }, function (data) {
    $("#Id").val(data.id);

    // Format teks akan langsung bernilai "Serial Number - Nama Barang" dari backend
    let fullText =
      data.serialNumber && data.namaBarang
        ? `${data.serialNumber} - ${data.namaBarang}`
        : `Aset ID: ${data.produkItemId}`;
        
    setSelect2Value(
      "/ReparasiAsets/GetProdukItemOptions",
      data.produkItemId,
      fullText
    );

    // Set tanggal dengan format dd-mm-yyyy agar sesuai dengan datepicker UI
    $("#TanggalMasuk").val(formatDateToUI(data.tanggalMasuk));
    $("#TanggalKeluar").val(formatDateToUI(data.tanggalKeluar));
    
    $("#Keluhan").val(data.keluhan);

    let formattedBiaya =
      data.biaya !== undefined && data.biaya !== null
        ? parseInt(data.biaya, 10).toLocaleString("id-ID")
        : "0";
    $("#Biaya").val(formattedBiaya);

    $("#StatusReparasi").val(data.statusReparasi);
    $("#modalTitle").text("Edit Reparasi Aset");

    $("#reparasiForm input, #reparasiForm textarea").removeClass(
      "is-invalid is-valid border-danger border-success",
    );
    $("#ProdukItemId")
      .next(".select2-container")
      .find(".select2-selection")
      .removeClass("border-danger border-success");
    $(".custom-error-msg").hide();

    var myModal = new bootstrap.Modal(document.getElementById("modalReparasi"));
    myModal.show();
  });
}

function deleteData(id) {
  Swal.fire({
    title: "Hapus data reparasi?",
    text: "Data yang dihapus tidak dapat dikembalikan!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#d33",
    cancelButtonColor: "#6c757d",
    confirmButtonText: "Ya, Hapus!",
    cancelButtonText: "Batal",
  }).then((result) => {
    if (result.isConfirmed) {
      $.post("/ReparasiAsets/DeleteReparasi", { id: id }, function (res) {
        if (res.success) {
          toastr.success(res.message, "Terhapus!");
          loadData(currentPage);
        } else {
          toastr.error(res.message, "Gagal!");
        }
      });
    }
  });
}

function changePage(p) {
  if (p < 1) return;
  currentPage = p;
  loadData(currentPage);
}