let currentPage = 1;
const pageSize = 8;

$(document).ready(function () {
    // Konfigurasi Global Toastr
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    loadRakData(currentPage);

    // Inisialisasi Select2 Gudang untuk Modal Create & Edit
    initSelect2Gudang('#createGudangId', '#modalTambahRak');
    initSelect2Gudang('#editGudangId', '#modalEditRak');
});

function getAntiForgeryToken() {
    return $('input[name="__RequestVerificationToken"]').val();
}

function initSelect2Gudang(selector, dropdownParent) {
    $(selector).select2({
        theme: 'bootstrap-5',
        placeholder: 'Cari & Pilih Gudang...',
        allowClear: true,
        dropdownParent: $(dropdownParent),
        ajax: {
            url: '/Raks/GetGudangOptions',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    search: params.term,
                    page: params.page || 1
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.results,
                    pagination: {
                        more: data.pagination.more
                    }
                };
            },
            cache: true
        },
        minimumInputLength: 0 // Menampilkan 10 data awal saat diklik
    });
}

function loadRakData(page) {
    currentPage = page;
    $.ajax({
        url: `/Raks/GetPagedData?pageIndex=${page}&pageSize=${pageSize}`,
        type: 'GET',
        success: function (res) {
            if (res.success) {
                renderGrid(res.data.items);
                renderPagination(res.data);
            }
        },
        error: function () {
            toastr.error('Gagal mengambil data lokasi.', 'Error!');
        }
    });
}

function renderGrid(items) {
    const container = $('#rakGridContainer');
    container.empty();

    if (!items || items.length === 0) {
        container.html(`
            <div class="col-12">
                <div class="alert alert-warning text-center p-4 rounded-3 shadow-sm my-3" role="alert">
                    <i class="bi bi-exclamation-triangle-fill fs-3 d-block mb-2"></i>
                    <h5 class="fw-bold mb-1">Belum ada data untuk ditampilkan !</h5>
                    <p class="mb-0 text-muted">Silakan tambahkan data lokasi rak baru dengan menekan tombol diatas.</p>
                </div>
            </div>
        `);
        return;
    }

    items.forEach(item => {
        const badgeClass = item.isActive ? 'badge-aktif' : 'bg-secondary text-white';
        const badgeText = item.isActive ? 'AKTIF' : 'NON-AKTIF';
        const deskripsi = item.deskripsi ? item.deskripsi : '-';
        const namaGudang = item.namaGudang ? item.namaGudang : '-';

        const cardHtml = `
            <div class="col-12 col-sm-6 col-md-4 col-xl-3">
                <div class="location-card-grid">
                    <div class="card-header-grid d-flex justify-content-between align-items-center">
                        <div class="icon-tag-box">
                            <i class="bi bi-tag-fill"></i>
                        </div>
                        <span class="badge ${badgeClass}">${badgeText}</span>
                    </div>
                    <div class="card-body-grid">
                        <h3 class="info-id">${item.kodeLokasi}</h3>
                        <p class="rak-desc text-truncate mb-1" title="${item.namaRak} (${item.area})">
                            ${item.namaRak} (${item.area})
                        </p>
                        <p class="small text-primary mb-2 text-truncate" title="Gudang: ${namaGudang}">
                            <i class="bi bi-building me-1"></i>${namaGudang}
                        </p>
                        <p class="notes-text text-truncate-2" title="${deskripsi}">
                            ${deskripsi}
                        </p>
                    </div>
                    <div class="card-footer-grid justify-content-center">
                        <button class="btn-action-grid" title="Edit" onclick="openEditModal(${item.id})">
                            <i class="bi bi-pencil-fill"></i>
                        </button>
                        <button class="btn-action-grid btn-delete-grid" title="Hapus" onclick="openDeleteModal(${item.id}, '${item.kodeLokasi}')">
                            <i class="bi bi-trash-fill"></i>
                        </button>
                    </div>
                </div>
            </div>
        `;
        container.append(cardHtml);
    });
}

function renderPagination(pagedData) {
    const container = $('#paginationContainer');
    container.empty();

    // Jika total data kosong
    if (!pagedData.totalItems || pagedData.totalItems === 0) {
        container.html('<div class="text-muted small">Menampilkan 0 dari 0 data</div>');
        return;
    }

    // Menghitung rentang data yang sedang ditampilkan
    const startItem = ((pagedData.pageIndex - 1) * pageSize) + 1;
    const endItem = Math.min(pagedData.pageIndex * pageSize, pagedData.totalItems);

    let paginationHtml = `
        <div class="d-flex justify-content-between align-items-center flex-wrap w-100">
            <div class="text-muted small mb-2 mb-md-0">
                Menampilkan ${startItem} - ${endItem} dari ${pagedData.totalItems} data
            </div>
    `;

    // Jika total halaman <= 1, cukup tampilkan teks info saja
    if (pagedData.totalPages <= 1) {
        paginationHtml += `</div>`;
        container.html(paginationHtml);
        return;
    }

    paginationHtml += '<nav><ul class="pagination pagination-sm mb-0">';

    // Tombol Previous
    const prevDisabled = !pagedData.hasPreviousPage ? 'disabled' : '';
    paginationHtml += `
        <li class="page-item ${prevDisabled}">
            <a class="page-link" href="javascript:void(0)" onclick="loadRakData(${pagedData.pageIndex - 1})">Previous</a>
        </li>`;

    // Logika Batas 5 ke Kiri dan 5 ke Kanan dari Halaman Aktif
    const maxButtons = 5;
    let startPage = Math.max(1, pagedData.pageIndex - maxButtons);
    let endPage = Math.min(pagedData.totalPages, pagedData.pageIndex + maxButtons);

    // Menyesuaikan jika berada di ujung awal atau akhir agar jumlah tombol tetap konsisten jika memungkinkan
    if (pagedData.pageIndex - maxButtons < 1) {
        endPage = Math.min(pagedData.totalPages, endPage + (1 - (pagedData.pageIndex - maxButtons)));
    }
    if (pagedData.pageIndex + maxButtons > pagedData.totalPages) {
        startPage = Math.max(1, startPage - ((pagedData.pageIndex + maxButtons) - pagedData.totalPages));
    }

    // Tombol Nomor Halaman dengan Limit
    for (let i = startPage; i <= endPage; i++) {
        const activeClass = i === pagedData.pageIndex ? 'active' : '';
        paginationHtml += `
            <li class="page-item ${activeClass}">
                <a class="page-link" href="javascript:void(0)" onclick="loadRakData(${i})">${i}</a>
            </li>`;
    }

    // Tombol Next
    const nextDisabled = !pagedData.hasNextPage ? 'disabled' : '';
    paginationHtml += `
        <li class="page-item ${nextDisabled}">
            <a class="page-link" href="javascript:void(0)" onclick="loadRakData(${pagedData.pageIndex + 1})">Next</a>
        </li>`;

    paginationHtml += '</ul></nav></div>';
    
    container.html(paginationHtml);
}

function resetFormCreate() {
    $('#formTambahRak')[0].reset();
    $('#createCheckLokasiAktif').prop('checked', true);
    $('#createGudangId').val(null).trigger('change');
}

function submitCreateRak(event) {
    event.preventDefault();

    const gudangId = $('#createGudangId').val();
    if (!gudangId) {
        toastr.warning('Silakan pilih Gudang terlebih dahulu!', 'Peringatan!');
        return;
    }

    const payload = {
        GudangId: parseInt(gudangId),
        Area: $('#createArea').val(),
        NamaRak: $('#createNamaRak').val(),
        BinNo: $('#createBinNo').val(),
        KodeLokasi: $('#createKodeLokasi').val(),
        Deskripsi: $('#createDeskripsi').val(),
        IsActive: $('#createCheckLokasiAktif').is(':checked')
    };

    $.ajax({
        url: '/Raks/Create',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        headers: { 'RequestVerificationToken': getAntiForgeryToken() },
        success: function (res) {
            if (res.success) {
                $('#modalTambahRak').modal('hide');
                toastr.success(res.message, 'Berhasil!');
                loadRakData(currentPage);
            } else {
                if (res.errors && res.errors.length > 0) {
                    res.errors.forEach(err => toastr.warning(err, 'Peringatan!'));
                } else {
                    toastr.error(res.message || 'Terjadi kesalahan.', 'Gagal!');
                }
            }
        },
        error: function () {
            toastr.error('Terjadi kesalahan pada server.', 'Error!');
        }
    });
}

function openEditModal(id) {
    $.ajax({
        url: `/Raks/GetById/${id}`,
        type: 'GET',
        success: function (res) {
            if (res.success) {
                populateEditModal(res.data);
                $('#modalEditRak').modal('show');
            } else {
                toastr.error(res.message, 'Error!');
            }
        },
        error: function () {
            toastr.error('Gagal mengambil data detail.', 'Error!');
        }
    });
}

function populateEditModal(data) {
    $('#editId').val(data.id);
    $('#editArea').val(data.area);
    $('#editNamaRak').val(data.namaRak);
    $('#editBinNo').val(data.binNo);
    $('#editKodeLokasi').val(data.kodeLokasi);
    $('#editDeskripsi').val(data.deskripsi);
    $('#editCheckLokasiAktif').prop('checked', data.isActive);

    // Set Select2 untuk Modal Edit
    if (data.gudangId && data.namaGudang) {
        var option = new Option(data.namaGudang, data.gudangId, true, true);
        $('#editGudangId').html(option).trigger('change');
    } else {
        $('#editGudangId').val(null).trigger('change');
    }
}

function submitEditRak(event) {
    event.preventDefault();

    const gudangId = $('#editGudangId').val();
    if (!gudangId) {
        toastr.warning('Silakan pilih Gudang terlebih dahulu!', 'Peringatan!');
        return;
    }

    const payload = {
        Id: parseInt($('#editId').val()),
        GudangId: parseInt(gudangId),
        Area: $('#editArea').val(),
        NamaRak: $('#editNamaRak').val(),
        BinNo: $('#editBinNo').val(),
        KodeLokasi: $('#editKodeLokasi').val(),
        Deskripsi: $('#editDeskripsi').val(),
        IsActive: $('#editCheckLokasiAktif').is(':checked')
    };

    $.ajax({
        url: '/Raks/Edit',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        headers: { 'RequestVerificationToken': getAntiForgeryToken() },
        success: function (res) {
            if (res.success) {
                $('#modalEditRak').modal('hide');
                toastr.success(res.message, 'Berhasil!');
                loadRakData(currentPage);
            } else {
                if (res.errors && res.errors.length > 0) {
                    res.errors.forEach(err => toastr.warning(err, 'Peringatan!'));
                } else {
                    toastr.error(res.message || 'Terjadi kesalahan.', 'Gagal!');
                }
            }
        },
        error: function () {
            toastr.error('Terjadi kesalahan pada server.', 'Error!');
        }
    });
}

function openDeleteModal(id, kodeLokasi) {
    $('#deleteId').val(id);
    $('#deleteTargetName').text(kodeLokasi);
    $('#modalHapusGudang').modal('show');
}

function confirmDeleteRak() {
    const id = $('#deleteId').val();

    $.ajax({
        url: `/Raks/Delete?id=${id}`,
        type: 'POST',
        headers: { 'RequestVerificationToken': getAntiForgeryToken() },
        success: function (res) {
            if (res.success) {
                $('#modalHapusGudang').modal('hide');
                toastr.success(res.message, 'Berhasil!');
                loadRakData(currentPage);
            } else {
                toastr.error(res.message || 'Terjadi kesalahan.', 'Gagal!');
            }
        },
        error: function () {
            toastr.error('Gagal menghapus data.', 'Error!');
        }
    });
}