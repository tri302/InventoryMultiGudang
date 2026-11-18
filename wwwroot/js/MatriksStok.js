/**
 * MatriksStok.js
 * Logika Front-End untuk modul Matriks Stok dengan AJAX, Live Search Gudang, dan Toastr Notifications.
 */
let currentPage = 1;
let selectedGudangId = '';

function loadMatriksStokData(gudangId, page) {
    $.ajax({
        url: '/MatriksStok/GetData',
        type: 'GET',
        cache: false, // Menghindari browser menyajikan data lama dari cache
        data: {
            gudangId: gudangId,
            page: page,
            pageSize: 10
        },
        success: function (response) {
            $('#matriksStokContainer').html(response);
        },
        error: function (xhr) {
            let errorMsg = "Terjadi kesalahan saat memuat data.";
            if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMsg = xhr.responseJSON.message;
            }
            toastr.error(errorMsg, "Gagal !");
        }
    });
}

$(document).ready(function () {    
    // Memuat data pertama kali saat halaman dibuka
    loadMatriksStokData(selectedGudangId, currentPage);

    // Inisialisasi Select2 pada formselect dengan fitur AJAX live search & pembatasan data
    $('#selectGudangFilter').select2({
        theme: 'bootstrap-5',
        placeholder: 'Semua Gudang',
        allowClear: true,
        ajax: {
            url: '/MatriksStok/SearchWarehouses',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term // Parameter pencarian teks
                };
            },
            processResults: function (data) {
                // Memetakan data dari database ke format Select2
                let results = [{ id: '', text: 'Semua Gudang' }];
                data.forEach(function (item) {
                    results.push({
                        id: item.id,
                        text: `${item.namaGudang} (${item.kodeGudang})`
                    });
                });
                return { results: results };
            },
            cache: true
        }
    });

    // Event handler ketika filter gudang diubah
    $('#selectGudangFilter').on('change', function () {
        selectedGudangId = $(this).val();
        currentPage = 1; // Reset ke halaman pertama
        loadMatriksStokData(selectedGudangId, currentPage);
    });

    // Event handler untuk pagination menggunakan AJAX (tanpa reload page)
    $(document).on('click', '.pagination-btn', function (e) {
        e.preventDefault();
        let page = $(this).data('page');
        if (page && page !== currentPage) {
            currentPage = page;
            loadMatriksStokData(selectedGudangId, currentPage);
        }
    });

    /**
     * Fungsi utama untuk mengambil data tabel via AJAX
     * @param {number|string} gudangId 
     * @param {number} page 
     */    
});
// Event ketika tombol edit safety stock diklik
$(document).on('click', '.btn-edit-safety-stock', function () {
    let productId = $(this).data('product-id');
    let productName = $(this).data('product-name');
    let sku = $(this).data('sku');
    let warehouseId = $(this).data('warehouse-id');
    let warehouseName = $(this).data('warehouse-name');
    let safetyStock = $(this).data('safety-stock');

    // Masukkan data ke dalam elemen modal
    $('#editProductId').val(productId);
    $('#editWarehouseId').val(warehouseId);
    $('#lblNamaProduk').text(productName);
    $('#lblSku').text('SKU: ' + sku);
    $('#lblNamaGudang').text(warehouseName);
    $('#safety-stock').val(safetyStock);

    // Tampilkan modal menggunakan Bootstrap 5 Modal API
    let modalElement = document.getElementById('modalEditStok');
    if (modalElement) {
        let modal = new bootstrap.Modal(modalElement);
        modal.show();
    }
});

// Event submit form edit safety stock via AJAX)
$('#formEditStok').on('submit', function (e) {
    e.preventDefault();

    $.ajax({
        url: '/MatriksStok/UpdateSafetyStock',
        type: 'POST',
        data: {
            productId: $('#editProductId').val(),
            warehouseId: $('#editWarehouseId').val(),
            safetyStock: $('#safety-stock').val()
        },
        success: function (response) {
            toastr.success(response.message, "Berhasil!");

            // Tutup modal
            let modalElement = document.getElementById('modalEditStok');
            let modal = bootstrap.Modal.getInstance(modalElement);
            modal.hide();

            // Reload data tabel agar tampilan terupdate            
            loadMatriksStokData(selectedGudangId, currentPage);            
        },
        error: function (xhr) {
            toastr.error("Gagal menyimpan perubahan.", "Error!");
        }
    });
});