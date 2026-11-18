$(document).ready(function () {
  const $form = $("#formTransaksiStok");

  // ==========================================
  // 1. INITIALIZE SELECT2 AJAX HANDLERS
  // ==========================================
  function initSelect2Elements($parent) {
    $parent = $parent || $(document);

    // Gudang
    $parent.find(".select2-gudang").each(function () {
      if ($(this).hasClass("select2-hidden-accessible")) return;
      $(this).select2({
        theme: "bootstrap-5",
        placeholder: $(this).data("placeholder") || "Pilih Gudang...",
        allowClear: true,
        ajax: {
          url: "/TransaksiStokInOut/GetDropdownGudang",
          dataType: "json",
          delay: 300,
          data: function (params) {
            return { q: params.term, page: params.page || 1 };
          },
          processResults: function (data) {
            return { results: data.results, pagination: data.pagination };
          },
        },
      });
    });

    // Supplier
    $parent.find(".select2-supplier").each(function () {
      if ($(this).hasClass("select2-hidden-accessible")) return;
      $(this).select2({
        theme: "bootstrap-5",
        placeholder: $(this).data("placeholder") || "Pilih Supplier...",
        allowClear: true,
        ajax: {
          url: "/TransaksiStokInOut/GetDropdownSupplier",
          dataType: "json",
          delay: 300,
          data: function (params) {
            return { q: params.term, page: params.page || 1 };
          },
          processResults: function (data) {
            return { results: data.results, pagination: data.pagination };
          },
        },
      });
    });

    // Produk (Dependent pada GudangId)
    $parent.find(".select2-produk").each(function () {
      if ($(this).hasClass("select2-hidden-accessible")) return;
      const $produkSelect = $(this);

      $produkSelect.select2({
        theme: "bootstrap-5",
        placeholder: $produkSelect.data("placeholder") || "-- Pilih Produk --",
        allowClear: true,
        ajax: {
          url: "/TransaksiStokInOut/GetDropdownProdukByGudang",
          dataType: "json",
          delay: 300,
          data: function (params) {
            return {
              gudangId: $("#GudangId").val(), // Mengirim GudangId yang sedang dipilih
              q: params.term,
              page: params.page || 1,
            };
          },
          processResults: function (data) {
            return { results: data.results, pagination: data.pagination };
          },
        },
      });
    });

    // Event ketika Gudang diubah: Reset Rak dan Produk
    $("#GudangId").on("change", function () {
      $(".select2-rak").val(null).trigger("change");
      $(".select2-produk").val(null).trigger("change");
    });

    // Opsional: Validasi tambahan saat select produk diklik tapi gudang kosong
    $(document).on("select2:opening", ".select2-produk", function (e) {
      const gudangId = $("#GudangId").val();
      if (!gudangId) {
        e.preventDefault();
        if (typeof toastr !== "undefined") {
          toastr.warning("Pilih Gudang Target terlebih dahulu!");
        } else {
          alert("Pilih Gudang Target terlebih dahulu!");
        }
      }
    });

    // Rak (Dependent pada GudangId)
    $parent.find(".select2-rak").each(function () {
      if ($(this).hasClass("select2-hidden-accessible")) return;
      const $rakSelect = $(this);
      $rakSelect.select2({
        theme: "bootstrap-5",
        placeholder: $rakSelect.data("placeholder") || "-- Pilih Lokasi/Rak --",
        allowClear: true,
        ajax: {
          url: "/TransaksiStokInOut/GetDropdownRakByGudang",
          dataType: "json",
          delay: 300,
          data: function (params) {
            return {
              gudangId: $("#GudangId").val(),
              q: params.term,
              page: params.page || 1,
            };
          },
          processResults: function (data) {
            var list = data.results || [];
            return {
              results: list.map(function (item) {
                return {
                  id: item.id,
                  text: item.text || item.namaRak,
                  kode: item.kode || "",
                  namaRak: item.namaRak || "",
                  area: item.area || "-",
                  binNo: item.binNo || "-",
                  deskripsi: item.deskripsi || "",
                };
              }),
            };
          },
        },
        templateResult: formatRakOption,
        templateSelection: formatRakSelection,
      });
    });
  }

  function formatRakOption(item) {
    if (item.loading || !item.id) return item.text;
    var namaDisplay = item.namaRak || item.text;
    var kodeDisplay = item.kode
      ? `<span class="badge bg-primary me-1">${item.kode}</span>`
      : "";
    var areaDisplay = `<span class="text-muted small ms-1">(Area ${item.area} - Bin ${item.binNo})</span>`;

    return $(`
            <div class="py-1">
                <div class="d-flex align-items-center justify-content-between">
                    <div>
                        ${kodeDisplay}
                        <strong class="text-dark">${namaDisplay}</strong>
                        ${areaDisplay}
                    </div>
                </div>
            </div>
        `);
  }

  function formatRakSelection(item) {
    if (!item.id) return item.text;
    var label = item.namaRak || item.text;
    return item.kode ? `[${item.kode}] ${label}` : label;
  }

  // Inisialisasi awal
  initSelect2Elements();

  // Reset Rak saat Gudang berubah
  $("#GudangId").on("change", function () {
    $(".select2-rak").val(null).trigger("change");
  });

  // ==========================================
  // 2. VALIDATION UTILITY & DYNAMIC ROW
  // ==========================================
  function rebindValidation() {
    if (
      typeof $.validator !== "undefined" &&
      typeof $.validator.unobtrusive !== "undefined"
    ) {
      $form.removeData("validator").removeData("unobtrusiveValidation");
      $.validator.unobtrusive.parse($form);
    }
  }

  $("#btnTambahBaris").on("click", function () {
    const index = $(".mutasi-row").length;
    const template = $("#rowTemplate")
      .html()
      .replace(/{INDEX}/g, index);
    const $newRow = $(template);

    $("#mutasiContainer").append($newRow);
    initSelect2Elements($newRow);
    rebindValidation();
  });

  $(document).on("click", ".btn-hapus-baris", function () {
    if ($(".mutasi-row").length > 1) {
      $(this).closest(".mutasi-row").remove();
      reindexRows();
      rebindValidation();
    } else {
      toastr.warning("Minimal harus menyisakan 1 baris item mutasi.");
    }
  });

  function reindexRows() {
    $(".mutasi-row").each(function (i, row) {
      $(row).find(".field-produk").attr("name", `Details[${i}].ProdukId`);
      $(row).find(".field-rak").attr("name", `Details[${i}].RakId`);
      $(row).find(".field-jumlah").attr("name", `Details[${i}].Jumlah`);
      $(row).find(".field-sn").attr("name", `Details[${i}].SerialNumbers`);
    });
  }

  // ==========================================
  // 3. SUBMIT FORM VIA AJAX
  // ==========================================
  function validateFormManual() {
    let isValid = true;
    $(".validation-warning").text("");

    if (!$("#GudangId").val()) {
      $("#GudangId")
        .siblings(".validation-warning")
        .text("Pilih Gudang Target terlebih dahulu!");
      isValid = false;
    }

    $(".mutasi-row").each(function (idx, el) {
      const produk = $(el).find(".field-produk").val();
      const rak = $(el).find(".field-rak").val();
      const jumlah = $(el).find(".field-jumlah").val();

      if (!produk) {
        $(el)
          .find(".field-produk")
          .siblings(".validation-warning")
          .text("Produk wajib dipilih.");
        isValid = false;
      }
      if (!rak) {
        $(el)
          .find(".field-rak")
          .siblings(".validation-warning")
          .text("Lokasi/Rak wajib dipilih.");
        isValid = false;
      }
      if (!jumlah || parseInt(jumlah) < 1) {
        $(el)
          .find(".field-jumlah")
          .siblings(".validation-warning")
          .text("Jumlah min 1.");
        isValid = false;
      }
    });

    return isValid;
  }

  $form.on("submit", function (e) {
    e.preventDefault();

    if (!validateFormManual()) {
      toastr.error("Mohon lengkapi field wajib yang masih kosong!");
      return;
    }

    const formData = new FormData(this);

    $.ajax({
      url: "/TransaksiStokInOut/Save",
      type: "POST",
      data: formData,
      contentType: false,
      processData: false,
      success: function (res) {
        if (res.success) {
          toastr.success(res.message);
          setTimeout(function () {
            window.location.href = "/TransaksiStokInOut/Index";
          }, 1000);
        } else {
          toastr.error(res.message || "Gagal menyimpan data!");
        }
      },
      error: function () {
        toastr.error("Terjadi kesalahan pada server!");
      },
    });
  });
});
// Reset Rak & Produk saat Gudang berubah
$("#GudangId").on("change", function () {
  $(".select2-rak").val(null).trigger("change");
  $(".select2-produk").val(null).trigger("change");
});
// Event ketika produk dipilih pada Select2
$(document).on("select2:select", ".field-produk", function (e) {
  const $row = $(this).closest(".mutasi-row");
  const produkId = e.params.data.id;
  const gudangId = $("#GudangId").val();
  const tipeTransaksi =
    $('input[name="TipeTransaksi"]').val() ||
    $('#formTransaksiStok input[name="TipeTransaksi"]').val();

  if (!gudangId) {
    if (typeof toastr !== "undefined")
      toastr.warning("Pilih Gudang terlebih dahulu!");
    else alert("Pilih Gudang terlebih dahulu!");
    return;
  }

  $.ajax({
    url: "/TransaksiStokInOut/GetProdukDetail",
    type: "GET",
    data: {
      produkId: produkId,
      gudangId: gudangId,
      tipeTransaksi: tipeTransaksi,
    },
    success: function (res) {
      if (res.success) {
        const data = res.data;
        const $snInput = $row.find(".field-sn");

        if (data.isBerSN) {
          $snInput.prop("readonly", false);
          if (
            tipeTransaksi === "Stock Out" &&
            data.availableSerialNumbers.length > 0
          ) {
            // Otomatis isi SN yang tersedia untuk Stock Out
            $snInput.val(data.availableSerialNumbers.join(", "));
          } else {
            // Stock In (Ber-SN): Kosongkan agar bisa diinput manual / auto-generate oleh backend
            $snInput.val("");
            $snInput.attr("placeholder", "Wajib isi SN (Ber-SN)");
          }
        } else {
          // Produk Non-SN: Kosongkan dan biarkan NULL di database (transaksistokdetail)
          $snInput.val("");
          $snInput.prop("readonly", true);
          $snInput.attr("placeholder", "Non-SN (Allow Null)");
        }
      }
    },
  });
});

// Event jika produk dihapus/direset dari Select2
$(document).on("select2:clear", ".field-produk", function () {
  const $row = $(this).closest(".mutasi-row");
  $row.find(".field-sn").val("").prop("readonly", false);
});
// Fungsi Helper untuk mengambil SN berdasarkan produk dan jumlah (Qty)
function updateProductSN($row) {
  const produkId = $row.find(".field-produk").val();
  const gudangId = $("#GudangId").val();
  const tipeTransaksi =
    $('input[name="TipeTransaksi"]').val() ||
    $('#formTransaksiStok input[name="TipeTransaksi"]').val();
  const jumlah = parseInt($row.find(".field-jumlah").val()) || 1;

  if (!produkId || !gudangId) return;

  $.ajax({
    url: "/TransaksiStokInOut/GetProdukDetail",
    type: "GET",
    data: {
      produkId: produkId,
      gudangId: gudangId,
      tipeTransaksi: tipeTransaksi,
      jumlah: jumlah, // Kirim kuantitas/jumlah barang
    },
    success: function (res) {
      if (res.success) {
        const data = res.data;
        const $snInput = $row.find(".field-sn");

        if (data.isBerSN) {
          $snInput.prop("readonly", false);
          if (tipeTransaksi === "Stock Out") {
            if (data.availableSerialNumbers.length > 0) {
              // Hanya ambil sejumlah Qty yang diinput
              $snInput.val(data.availableSerialNumbers.join(", "));
            } else {
              $snInput.val("");
              if (typeof toastr !== "undefined")
                toastr.warning("Stok SN tidak mencukupi di gudang ini!");
            }
          } else {
            // Stock In (Ber-SN)
            $snInput.val("");
            $snInput.attr("placeholder", "Wajib isi SN (Ber-SN)");
          }
        } else {
          // Produk Non-SN (Allow Null)
          $snInput.val("");
          $snInput.prop("readonly", true);
          $snInput.attr("placeholder", "Non-SN (Allow Null)");
        }
      }
    },
  });
}

// Trigger saat Select2 Produk Dipilih
$(document).on("select2:select", ".field-produk", function () {
  const $row = $(this).closest(".mutasi-row");
  updateProductSN($row);
});

// Trigger saat input Jumlah (Qty) diubah
$(document).on("change keyup", ".field-jumlah", function () {
  const $row = $(this).closest(".mutasi-row");
  updateProductSN($row);
});

// Trigger saat Select2 Produk Dihapus/Direset
$(document).on("select2:clear", ".field-produk", function () {
  const $row = $(this).closest(".mutasi-row");
  $row.find(".field-sn").val("").prop("readonly", false);
});
