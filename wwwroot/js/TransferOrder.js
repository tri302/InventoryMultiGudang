/**
 * TransferOrder.js - Handling Client-Side CRUD & Live Search Ajax
 */

$(document).ready(function () {

    // ==========================================
    // 1. EVENT HANDLER TERIMA BARANG (DETAIL PAGE)
    // ==========================================

    $(document).on('click', '#btnTerimaBarang', function (e) {
        e.preventDefault();        
        $('#formTerimaBarang').submit();
    });

    // Listener Submit Form
    $(document).on('submit', '#formTerimaBarang', function (e) {
        e.preventDefault();
        let id = $('#DetailTransferOrderId').val();

        if (!id || id === "0") {
            let urlSegments = window.location.pathname.split('/');
            id = urlSegments[urlSegments.length - 1];
        }

        // Cek Token CSRF
        let token = $('input[name="__RequestVerificationToken"]').val();

        if (!id || isNaN(id)) {
            console.log("[ERROR] ID Transfer Order tidak valid!");
            toastr.error('ID Transfer Order tidak valid!');
            return;
        }

        // Cek Data Kuantitas
        let receivedQtyMap = {};
        let count = 0;

        $('.input-qty-diterima').each(function (idx, el) {
            let itemId = $(el).attr('data-item-id');
            let qty = parseInt($(el).val()) || 0;

            if (itemId) {
                receivedQtyMap[itemId] = qty;
                count++;
            }
        });

        if (count === 0) {
            console.log("[ERROR] Tidak ada item dengan data-item-id yang ditemukan!");
            toastr.error('Error: Tidak ada data barang yang terbaca!');
            return;
        }

        let $btn = $('#btnTerimaBarang');
        $btn.prop('disabled', true).html('<span class="spinner-border spinner-border-sm me-1"></span> Memproses...');

        $.ajax({
            url: '/TransferOrder/ReceiveBarang?id=' + id,
            type: 'POST',
            contentType: 'application/json',
            headers: { 'RequestVerificationToken': token },
            data: JSON.stringify(receivedQtyMap),
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message || 'Penerimaan berhasil!');
                    window.location.reload();
                } else {
                    toastr.error('Gagal dari Server: ' + res.message);
                    $btn.prop('disabled', false).html('<i class="bi bi-box-arrow-down"></i> Terima Barang (Received)');
                }
            },
            error: function (xhr, status, error) {
                console.log("=== [AJAX ERROR] ===");
                console.log("Status:", xhr.status);
                console.log("Response Text:", xhr.responseText);
                console.log("Error Detail:", error);
                toastr.error('AJAX Error ' + xhr.status + ': Cek Console Browser!');
                $btn.prop('disabled', false).html('<i class="bi bi-box-arrow-down"></i> Terima Barang (Received)');
            }
        });
    });

    // ==========================================
    // 2. EVENT HANDLER HAPUS DATA (DETAIL PAGE)
    // ==========================================
    $(document).on('click', '#btnConfirmDeleteDetail', function (e) {
        e.preventDefault();

        let detailVal = $('#DetailTransferOrderId').val();
        let id = (typeof selectedDeleteId !== 'undefined' && selectedDeleteId && selectedDeleteId !== 0) ? selectedDeleteId : detailVal;
        let token = $('input[name="__RequestVerificationToken"]').val();

        if (!id || id == "0") {
            toastr.error('ID Transfer Order tidak ditemukan!');
            return;
        }

        let $btn = $(this);
        $btn.prop('disabled', true).html('<span class="spinner-border spinner-border-sm me-1"></span> Menghapus...');

        $.ajax({
            url: '/TransferOrder/Delete?id=' + id,
            type: 'POST',
            headers: { 'RequestVerificationToken': token },
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message);
                    $('#modalHapusTO').modal('hide');

                    setTimeout(function () {
                        if ($('#toTableBody').length > 0) {
                            $btn.prop('disabled', false).text('Ya, Hapus');
                            if (typeof selectedDeleteId !== 'undefined') selectedDeleteId = 0;
                            loadTransferOrders(currentPage);
                        } else {
                            window.location.href = '/TransferOrder/Index';
                        }
                    }, 800);
                } else {
                    toastr.error(res.message);
                    $btn.prop('disabled', false).text('Ya, Hapus');
                }
            },
            error: function () {
                toastr.error('Gagal menghapus data!');
                $btn.prop('disabled', false).text('Ya, Hapus');
            }
        });
    });

    // ==========================================
    // 3. LOGIKA HALAMAN INDEX (Hanya Jika Elemen Tabel Ada)
    // ==========================================
    let currentPage = 1;
    let selectedDeleteId = 0;

    if ($('#toTableBody').length > 0) {
        loadTransferOrders(currentPage);
        initSelect2Gudang('.select2-gudang');
    }

    // Live search filter listener
    $('#filterSearch').on('keyup debounce', function () {
        currentPage = 1;
        loadTransferOrders(currentPage);
    });

    $('#filterGudang, #filterStatus').on('change', function () {
        currentPage = 1;
        loadTransferOrders(currentPage);
    });

    // Reset Form Saat Modal Create Dibuka
    $('#modalTransferOrder').on('show.bs.modal', function () {
        $('#formCreateTO')[0].reset();
        $('.select2-gudang').val(null).trigger('change');
        $('#detailProdukContainer').empty();
        tambahBarisProduk();
        bindUnobtrusiveValidation($('#formCreateTO'));
    });

    $('#btnTambahBaris').on('click', function () {
        tambahBarisProduk();
    });

    $(document).on('click', '.btn-remove-row', function () {
        if ($('#detailProdukContainer .row-produk-item').length > 1) {
            $(this).closest('.row-produk-item').remove();
            reindexRows();
        } else {
            toastr.warning('Minimal harus ada 1 produk dalam daftar!');
        }
    });

    $('#formCreateTO').on('submit', function (e) {
        e.preventDefault();

        let $form = $(this);
        bindUnobtrusiveValidation($form);

        if (!$form.valid && typeof $form.valid === 'function') {
            if (!$form.valid()) return false;
        }

        let isValid = true;
        $('.validation-warning').text('');

        if (!$('#GudangAsalId').val()) {
            $('[data-valmsg-for="GudangAsalId"]').text('Gudang Asal wajib dipilih!');
            isValid = false;
        }
        if (!$('#GudangTujuanId').val()) {
            $('[data-valmsg-for="GudangTujuanId"]').text('Gudang Tujuan wajib dipilih!');
            isValid = false;
        }

        if ($('#GudangAsalId').val() && $('#GudangTujuanId').val() && $('#GudangAsalId').val() === $('#GudangTujuanId').val()) {
            $('[data-valmsg-for="GudangTujuanId"]').text('Gudang Asal & Tujuan tidak boleh sama!');
            toastr.warning('Peringatan: Gudang Asal dan Gudang Tujuan tidak boleh sama!');
            isValid = false;
        }

        let detailItems = [];
        $('#detailProdukContainer .row-produk-item').each(function (index, el) {
            let produkId = $(el).find('.select2-produk').val();
            let qty = $(el).find('.input-qty').val();
            let warnSpan = $(el).find('.validation-warning');

            if (!produkId) {
                warnSpan.text('Produk wajib dipilih!');
                isValid = false;
            }

            if (!qty || qty < 1) {
                isValid = false;
                toastr.warning('Kuantitas produk minimal 1!');
            }

            detailItems.push({
                ProdukId: parseInt(produkId),
                KuantitasPermintaan: parseInt(qty)
            });
        });

        if (!isValid) return;

        let payload = {
            GudangAsalId: parseInt($('#GudangAsalId').val()),
            GudangTujuanId: parseInt($('#GudangTujuanId').val()),
            Catatan: $('#Catatan').val(),
            Details: detailItems
        };

        let token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: '/TransferOrder/Create',
            type: 'POST',
            contentType: 'application/json',
            headers: { 'RequestVerificationToken': token },
            data: JSON.stringify(payload),
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message);
                    $('#modalTransferOrder').modal('hide');
                    if ($('#toTableBody').length > 0) loadTransferOrders(currentPage);
                } else {
                    toastr.error(res.message);
                }
            },
            error: function () {
                toastr.error('Terjadi kesalahan sistem!');
            }
        });
    });

    $(document).on('click', '.btn-delete-to', function () {
        selectedDeleteId = $(this).data('id');
        let docNum = $(this).data('doc');
        $('#deleteDocNum').text(docNum);
        $('#modalHapusTO').modal('show');
    });

    $('#btnConfirmDelete').on('click', function () {
        if (!selectedDeleteId) return;
        let token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: '/TransferOrder/Delete?id=' + selectedDeleteId,
            type: 'POST',
            headers: { 'RequestVerificationToken': token },
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message);
                    $('#modalHapusTO').modal('hide');
                    if ($('#toTableBody').length > 0) loadTransferOrders(currentPage);
                } else {
                    toastr.error(res.message);
                }
            },
            error: function () {
                toastr.error('Gagal menghapus data!');
            }
        });
    });

    function loadTransferOrders(page) {
        let search = $('#filterSearch').val();
        let gudangId = $('#filterGudang').val();
        let status = $('#filterStatus').val();
        let pageSize = 5;

        $.ajax({
            url: '/TransferOrder/GetListAjax',
            type: 'GET',
            data: { searchKey: search, gudangId: gudangId, status: status, page: page, pageSize: pageSize },
            success: function (res) {
                if (res.success) {
                    renderTable(res.data.items);
                    renderPagination(res.data, pageSize);
                    $('#totalDokumen').text(res.data.totalItems);
                }
            }
        });
    }

    function renderTable(items) {
        let $tbody = $('#toTableBody');
        $tbody.empty();

        if (!items || items.length === 0) {
            $tbody.append(`
            <tr>
                <td colspan="7" class="text-center py-5 text-muted">
                    <i class="bi bi-inbox fs-1 d-block mb-2 text-secondary"></i>
                    Belum ada data untuk ditampilkan !
                </td>
            </tr>
        `);
            return;
        }

        items.forEach(function (item) {
            let badgeStatus = '';

            // Normalisasi string status
            let st = item.status !== null && item.status !== undefined ? String(item.status).toLowerCase().replace(/\s+/g, '') : '';

            // Format menggunakan badge bawaan Bootstrap + teks ber-spasi
            if (st === '1' || st === 'dalamperjalanan' || st === 'intransit' || st === 'in-transit') {
                badgeStatus = '<span class="badge bg-warning text-dark px-3 py-1 rounded-pill fw-bold">Dalam Perjalanan</span>';
            } else if (st === '2' || st === 'diterima' || st === 'received' || st === 'completed') {
                badgeStatus = '<span class="badge bg-success px-3 py-1 rounded-pill fw-bold">Diterima</span>';
            } else {
                badgeStatus = '<span class="badge bg-secondary px-3 py-1 rounded-pill fw-bold">Pending</span>';
            }

            let rowHtml = `
            <tr>
                <td class="py-3 px-4">
                    <div class="d-flex align-items-center gap-3">
                        <div class="avatar-box flex-shrink-0"><i class="bi bi-file-earmark-text-fill fs-5"></i></div>
                        <span class="fw-bold text-indigo-dark">${item.nomorDokumen}</span>
                    </div>
                </td>
                <td class="py-3">
                    <div class="fw-semibold text-indigo-dark mb-0">${item.gudangAsal ? item.gudangAsal.namaGudang : '-'}</div>
                    <small class="text-muted text-uppercase tracking-wide" style="font-size: 0.7rem;">${item.gudangAsal ? item.gudangAsal.kodeGudang : ''}</small>
                </td>
                <td class="py-3">
                    <div class="fw-semibold text-indigo-dark mb-0">${item.gudangTujuan ? item.gudangTujuan.namaGudang : '-'}</div>
                    <small class="text-muted text-uppercase tracking-wide" style="font-size: 0.7rem;">${item.gudangTujuan ? item.gudangTujuan.kodeGudang : ''}</small>
                </td>
                <td class="py-3 text-center">${badgeStatus}</td>
                <td class="py-3 text-muted small">
                    <i class="bi bi-calendar3 me-1"></i> ${new Date(item.tanggalDibuat).toLocaleString()}
                </td>
                <td class="py-3 text-muted fw-semibold">${item.pembuat || '-'}</td>
                <td class="py-3 text-center">
                    <div class="d-flex align-items-center justify-content-center gap-2">
                        <a href="/TransferOrder/DetailTransferOrder/${item.id}" class="btn btn-action-view btn-sm d-flex align-items-center gap-1">
                            <i class="bi bi-eye"></i> Detail
                        </a>
                        <button type="button" data-id="${item.id}" data-bs-toggle="modal" data-bs-target="#modalHapusTO" data-doc="${item.nomorDokumen}" class="btn btn-action-light text-danger btn-sm p-2 rounded-2 btn-delete-to">
                            <i class="bi bi-trash3 fs-6"></i>
                        </button>
                    </div>
                </td>
            </tr>
        `;
            $tbody.append(rowHtml);
        });
    }

    function renderPagination(pagedData, pageSize = 5) {
        let $footer = $('#paginationFooter');
        let $info = $('#paginationInfo');
        let $container = $('#paginationContainer');

        $info.empty();
        $container.empty();

        if (!pagedData || pagedData.totalItems === 0) {
            $footer.hide();
            return;
        }

        $footer.show();

        let size = pagedData.pageSize || pageSize;
        let startItem = ((pagedData.currentPage - 1) * size) + 1;
        let endItem = Math.min(pagedData.currentPage * size, pagedData.totalItems);

        $info.html(`Menampilkan <strong>${startItem}</strong> - <strong>${endItem}</strong> dari <strong>${pagedData.totalItems}</strong> data`);

        let currentPage = pagedData.currentPage;
        let totalPages = pagedData.totalPages;

        let paginationHtml = '<ul class="pagination pagination-sm mb-0 justify-content-end">';

        // Tombol Previous
        paginationHtml += `<li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
            <a class="page-link page-nav" data-page="${currentPage - 1}" href="#">Previous</a>
        </li>`;

        // Logika Pembatasan Tombol (Maksimal menampilkan ~5 nomor halaman aktif di sekitar halaman saat ini)
        let maxVisiblePages = 5;
        let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
        let endPage = Math.min(totalPages, startPage + maxVisiblePages - 1);

        if (endPage - startPage + 1 < maxVisiblePages) {
            startPage = Math.max(1, endPage - maxVisiblePages + 1);
        }

        // Halaman Pertama & Titik-titik di awal jika jauh
        if (startPage > 1) {
            paginationHtml += `<li class="page-item"><a class="page-link page-nav" data-page="1" href="#">1</a></li>`;
            if (startPage > 2) {
                paginationHtml += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
            }
        }

        // Render Nomor Halaman Utama
        for (let i = startPage; i <= endPage; i++) {
            paginationHtml += `<li class="page-item ${currentPage === i ? 'active' : ''}">
                <a class="page-link page-nav" data-page="${i}" href="#">${i}</a>
            </li>`;
        }

        // Titik-titik & Halaman Terakhir jika jauh
        if (endPage < totalPages) {
            if (endPage < totalPages - 1) {
                paginationHtml += `<li class="page-item disabled"><span class="page-link">...</span></li>`;
            }
            paginationHtml += `<li class="page-item"><a class="page-link page-nav" data-page="${totalPages}" href="#">${totalPages}</a></li>`;
        }

        // Tombol Next
        paginationHtml += `<li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
            <a class="page-link page-nav" data-page="${currentPage + 1}" href="#">Next</a>
        </li>`;

        paginationHtml += '</ul>';
        $container.append(paginationHtml);

        // Event klik halaman
        $('.page-nav').off('click').on('click', function (e) {
            e.preventDefault();
            if ($(this).parent().hasClass('disabled') || $(this).parent().hasClass('active')) return;
            
            let targetPage = parseInt($(this).data('page'));
            if (targetPage >= 1 && targetPage <= totalPages) {
                loadTransferOrders(targetPage);
            }
        });
    }

    function tambahBarisProduk() {
        let index = $('#detailProdukContainer .row-produk-item').length;
        let rowHtml = `
            <div class="bg-light-subtle p-3 rounded-3 border mb-2 row-produk-item">
                <div class="row g-2 align-items-center">
                    <div class="col-12 col-md-6">
                        <select name="Details[${index}].ProdukId" class="form-select field-produk select2-produk" data-placeholder="-- Pilih Produk --">
                            <option value=""></option>
                        </select>
                        <br/>
                        <span class="text-danger small fw-semibold validation-warning"></span>
                    </div>
                    <div class="col-9 col-md-3">
                        <input type="number" name="Details[${index}].KuantitasPermintaan" class="form-control text-center input-qty" value="1" min="1">
                    </div>
                    <div class="col-3 col-md-1 text-center">
                        <button type="button" class="btn btn-link text-muted p-2 link-danger btn-remove-row"><i class="bi bi-trash3 fs-5"></i></button>
                    </div>
                </div>
            </div>
        `;

        $('#detailProdukContainer').append(rowHtml);
        initSelect2Produk($('#detailProdukContainer .row-produk-item').last().find('.select2-produk'));
    }

    function reindexRows() {
        $('#detailProdukContainer .row-produk-item').each(function (idx, el) {
            $(el).find('.select2-produk').attr('name', `Details[${idx}].ProdukId`);
            $(el).find('.input-qty').attr('name', `Details[${idx}].KuantitasPermintaan`);
        });
    }

    function initSelect2Gudang(selector) {
        $(selector).select2({
            dropdownParent: $('#modalTransferOrder'),
            width: '100%',
            ajax: {
                url: '/TransferOrder/GetGudangOptions',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return { q: params.term };
                },
                processResults: function (data) {
                    return { results: data.results };
                },
                cache: true
            },
            placeholder: '-- Pilih Gudang --',
            allowClear: true,
            minimumInputLength: 0
        });
    }

    function initSelect2Produk($element) {
        $element.select2({
            dropdownParent: $('#modalTransferOrder'),
            width: '100%', // Pastikan width diset agar ukurannya pas
            ajax: {
                url: '/TransferOrder/GetProdukOptions',
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        q: params.term,
                        gudangId: $('#GudangAsalId').val()
                    };
                },
                processResults: function (data) {
                    return { results: data.results };
                },
                cache: true
            },
            placeholder: '-- Pilih Produk --',
            allowClear: true,
            minimumInputLength: 0
        });
        
        $element.on('select2:open', function () {
            let searchField = document.querySelector('.select2-container--open .select2-search__field');
            if (searchField) {
                searchField.focus();
            }
        });
    }

    function bindUnobtrusiveValidation($form) {
        if (typeof $.validator !== 'undefined' && typeof $.validator.unobtrusive !== 'undefined') {
            $form.removeData("validator").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse($form);
        }
    }
});

$('#GudangAsalId').on('change', function () {
    $('.select2-produk').val(null).trigger('change');
});