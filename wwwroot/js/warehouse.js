let currentPage = 1;
const pageSize = 6;

$(document).ready(function () {
    // Toastr Configurations
    toastr.options = {
        "closeButton": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "timeOut": "3000"
    };

    loadData(currentPage);
});

function onSearchKeyup(e) {
    if (e.key === 'Enter') {
        loadData(1);
    }
}

// 1. Fetch & Render Cards + Pagination
function loadData(page) {
    currentPage = page;
    const search = $('#searchInput').val();
    const tipe = $('#filterTipe').val();

    $.ajax({
        url: `/Warehouses/GetList?pageIndex=${page}&pageSize=${pageSize}&search=${encodeURIComponent(search)}&tipe=${encodeURIComponent(tipe)}`,
        type: 'GET',
        success: function (res) {
            if (res.success) {
                $('#totalGudangBadge').text(res.data.totalItems);
                renderCards(res.data.items);
                renderPagination(res.data);
            } else {
                toastr.error(res.message);
            }
        },
        error: function () {
            toastr.error('Gagal mengambil data dari server!');
        }
    });
}

function renderCards(items) {
    let html = '';
    if (!items || items.length === 0) {
        html = `<div class="col-12 text-center py-5 text-muted">
                    <i class="bi bi-inbox fs-1 d-block mb-2"></i>
                    Tidak ada data gudang yang ditemukan.
                </div>`;
    } else {
        items.forEach(item => {
            const isVirtual = item.tipeGudang.toLowerCase() === 'virtual';
            const cardBorderClass = isVirtual ? 'border-purple' : 'border-primary';
            const cardBorderInlineStyle = isVirtual ? 'style="border-color: #6f42c1 !important;"' : '';
            const iconBoxClass = isVirtual ? 'bg-purple-subtle text-purple' : 'bg-primary-subtle text-primary';
            const iconBoxStyle = isVirtual ? 'style="background-color: #e0d5f2; color: #6f42c1;"' : '';
            const iconClass = isVirtual ? 'bi-cloud' : 'bi-building';
            const badgeClass = isVirtual ? 'bg-purple-subtle text-purple' : 'bg-primary-subtle text-primary';
            const badgeStyle = isVirtual ? 'style="background-color: #e0d5f2; color: #6f42c1;"' : '';
            const badgeText = isVirtual ? 'Virtual' : 'Fisik';
            const addressIcon = isVirtual ? 'bi-hdd-network' : 'bi-geo-alt';
            const addressText = item.alamatGudang ? item.alamatGudang : (isVirtual ? 'Cloud Database System' : '-');

            html += `
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 warehouse-card shadow-sm border-top ${cardBorderClass} border-4" ${cardBorderInlineStyle}>
                    <div class="card-body p-4">
                        <div class="d-flex align-items-center mb-3">
                            <div class="icon-box ${iconBoxClass} me-3 p-2 rounded-3" ${iconBoxStyle}>
                                <i class="bi ${iconClass} fs-4"></i>
                            </div>
                            <div>
                                <div class="text-muted small">${item.kodeGudang}</div>
                                <h5 class="card-title mb-0 fw-semibold">${item.namaGudang}</h5>
                            </div>
                            <span class="badge ${badgeClass} rounded-pill ms-auto" ${badgeStyle}>${badgeText}</span>
                        </div>

                        <div class="mb-3 text-muted small">
                            <p class="mb-1"><i class="bi ${addressIcon} me-2"></i>${addressText}</p>
                            <p class="mb-0"><i class="bi bi-person me-2"></i>PIC: ${item.picGudang}</p>
                        </div>

                        <hr class="text-muted opacity-25 mb-3">

                        <div class="row text-center mb-3">
                            <div class="col-6">
                                <div class="text-muted small">PRODUK</div>
                                <div class="fw-bold fs-5 text-dark">${item.totalProduk}</div>
                            </div>
                            <div class="col-6">
                                <div class="text-muted small">STOK BARANG</div>
                                <div class="fw-bold fs-5 text-primary">
                                    <i class="bi bi-box-seam me-1"></i>${item.totalStokBarang}
                                </div>
                            </div>
                        </div>

                        <div class="d-flex justify-content-center gap-2 mt-auto">
                            <button class="btn btn-outline-primary btn-sm rounded-circle"
                                    onclick="openModalEdit(${item.id})"
                                    title="Edit">
                                <i class="bi bi-pencil"></i>
                            </button>
                            <button class="btn btn-outline-danger btn-sm rounded-circle"
                                    onclick="openModalDelete(${item.id}, '${item.namaGudang.replace(/'/g, "\\'")}')"
                                    title="Hapus">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>`;
        });
    }
    $('#warehouseCardContainer').html(html);
}

function renderPagination(pagedData) {
    let html = '';
    const total = pagedData.totalItems;
    const currentPage = pagedData.pageIndex;
    const totalPages = pagedData.totalPages;
    
    const start = total === 0 ? 0 : ((currentPage - 1) * pagedData.pageSize) + 1;
    const end = Math.min(currentPage * pagedData.pageSize, total);

    $('#pageInfoText').text(`Menampilkan ${start} - ${end} dari ${total} data`);

    if (totalPages <= 1) {
        $('#paginationContainer').html('');
        return;
    }

    // Tombol Previous
    html += `<li class="page-item ${!pagedData.hasPreviousPage ? 'disabled' : ''}">
                <a class="page-link" href="javascript:void(0)" onclick="loadData(${currentPage - 1})">Previous</a>
             </li>`;
    let startPage = Math.max(1, currentPage - 5);
    let endPage = Math.min(totalPages, currentPage + 5);
    if (currentPage <= 6) {
        endPage = Math.min(totalPages, 11);
    } else if (currentPage + 5 >= totalPages) {
        startPage = Math.max(1, totalPages - 10);
    }

    // Tampilkan tombol halaman pertama + elipsis (...) jika rentang terpotong di kiri
    if (startPage > 1) {
        html += `<li class="page-item"><a class="page-link" href="javascript:void(0)" onclick="loadData(1)">1</a></li>`;
        if (startPage > 2) {
            html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
    }

    // Render loop nomor halaman
    for (let i = startPage; i <= endPage; i++) {
        html += `<li class="page-item ${i === currentPage ? 'active' : ''}">
                    <a class="page-link" href="javascript:void(0)" onclick="loadData(${i})">${i}</a>
                 </li>`;
    }

    // Tampilkan elipsis (...) + tombol halaman terakhir jika rentang terpotong di kanan
    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            html += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
        }
        html += `<li class="page-item"><a class="page-link" href="javascript:void(0)" onclick="loadData(${totalPages})">${totalPages}</a></li>`;
    }

    // Tombol Next
    html += `<li class="page-item ${!pagedData.hasNextPage ? 'disabled' : ''}">
                <a class="page-link" href="javascript:void(0)" onclick="loadData(${currentPage + 1})">Next</a>
             </li>`;

    $('#paginationContainer').html(html);
}

function clearFormErrors(prefix) {
    $(`#form${prefix === 'create' ? 'Tambah' : 'Edit'}Gudang .error-msg`).text('');
    $(`#form${prefix === 'create' ? 'Tambah' : 'Edit'}Gudang .form-control, #form${prefix === 'create' ? 'Tambah' : 'Edit'}Gudang .form-select`).removeClass('is-invalid');
}

// 2. CREATE CRUD
function openModalCreate() {
    clearFormErrors('create');
    $('#formTambahGudang')[0].reset();
    $('#create_statusGudang').prop('checked', true);
    $('#modalTambahGudang').modal('show');
}

function submitCreate() {
    clearFormErrors('create');

    const payload = {
        Id: 0,        
        NamaGudang: $('#create_namaGudang').val(),
        TipeGudang: $('#create_tipeGudang').val(),
        PicGudang: $('#create_picGudang').val(),
        AlamatGudang: $('#create_alamatGudang').val(),
        IsActive: $('#create_statusGudang').is(':checked')
    };

    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: '/Warehouses/Create',
        type: 'POST',
        contentType: 'application/json',
        headers: { "RequestVerificationToken": token },
        data: JSON.stringify(payload),
        success: function (res) {
            if (res.success) {
                $('#modalTambahGudang').modal('hide');
                toastr.success(res.message);
                loadData(currentPage);
            } else {
                if (res.errors) {
                    for (let key in res.errors) {
                        $(`#error-create-${key}`).text(res.errors[key][0]);
                        $(`#create_${key.charAt(0).toLowerCase() + key.slice(1)}`).addClass('is-invalid');
                    }
                }
                toastr.error(res.message);
            }
        },
        error: function () {
            toastr.error('Terjadi kesalahan sistem saat menyimpan data.');
        }
    });
}

// 3. EDIT CRUD
function openModalEdit(id) {
    clearFormErrors('edit');
    $.ajax({
        url: `/Warehouses/GetById/${id}`,
        type: 'GET',
        success: function (res) {
            if (res.success) {
                const d = res.data;
                $('#edit_gudangId').val(d.id);
                $('#edit_kodeGudang').val(d.kodeGudang);
                $('#edit_namaGudang').val(d.namaGudang);
                $('#edit_tipeGudang').val(d.tipeGudang.toLowerCase());
                $('#edit_picGudang').val(d.picGudang);
                $('#edit_alamatGudang').val(d.alamatGudang);
                $('#edit_statusGudang').prop('checked', d.isActive);

                $('#modalEditGudang').modal('show');
            } else {
                toastr.error(res.message);
            }
        }
    });
}

function submitEdit() {
    clearFormErrors('edit');

    const payload = {
        Id: parseInt($('#edit_gudangId').val()),
        KodeGudang: $('#edit_kodeGudang').val(),
        NamaGudang: $('#edit_namaGudang').val(),
        TipeGudang: $('#edit_tipeGudang').val(),        
        PicGudang: $('#edit_picGudang').val(),
        AlamatGudang: $('#edit_alamatGudang').val(),
        IsActive: $('#edit_statusGudang').is(':checked')
    };
    const token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: '/Warehouses/Update',
        type: 'POST',
        contentType: 'application/json',
        headers: { "RequestVerificationToken": token },
        data: JSON.stringify(payload),
        success: function (res) {
            if (res.success) {
                $('#modalEditGudang').modal('hide');
                toastr.success(res.message);
                loadData(currentPage);
            } else {
                if (res.errors) {
                    for (let key in res.errors) {
                        $(`#error-edit-${key}`).text(res.errors[key][0]);
                        $(`#edit_${key.charAt(0).toLowerCase() + key.slice(1)}`).addClass('is-invalid');
                    }
                }
                toastr.error(res.message);
            }
        },
        error: function () {
            toastr.error('Terjadi kesalahan sistem saat memperbarui data.');
        }
    });
}

// 4. DELETE CRUD
function openModalDelete(id, namaGudang) {
    $('#delete_gudangId').val(id);
    $('#delete_namaGudangText').text(namaGudang);
    $('#modalHapusGudang').modal('show');
}

function confirmDelete() {
    const id = $('#delete_gudangId').val();
    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: `/Warehouses/Delete/${id}`,
        type: 'POST',
        headers: { "RequestVerificationToken": token },
        success: function (res) {
            if (res.success) {
                $('#modalHapusGudang').modal('hide');
                toastr.success(res.message);
                loadData(currentPage);
            } else {
                toastr.error(res.message);
            }
        },
        error: function () {
            toastr.error('Terjadi kesalahan sistem saat menghapus data.');
        }
    });
}