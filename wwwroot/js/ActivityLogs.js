$(document).ready(function () {
    // Konfigurasi Toastr agar posisi berada di atas tengah layar (top-center)
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "4000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    let currentPage = 1;
    let pageSize = 10;
    let searchKeyword = "";
    let searchTimer = null;

    // Load data pertama kali saat halaman dibuka
    loadActivityLogs(currentPage, searchKeyword);

    // Event handler pencarian (realtime dengan delay/debounce agar tidak terlalu sering request)
    $('#searchInput').on('keyup', function () {
        clearTimeout(searchTimer);
        searchKeyword = $(this).val();
        currentPage = 1; // Reset ke halaman 1 saat melakukan pencarian baru

        searchTimer = setTimeout(function () {
            loadActivityLogs(currentPage, searchKeyword);
        }, 400); // Delay 400ms
    });

    // Fungsi utama untuk memanggil data via AJAX tanpa reload halaman
    function loadActivityLogs(page, search) {
        $.ajax({
            url: '/ActivityLogs/GetData',
            type: 'GET',
            data: { search: search, page: page, pageSize: pageSize },
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    renderTable(response.data);
                    renderPagination(response.data);
                } else {
                    toastr.error(response.message || "Gagal memuat data activity logs.", "Gagal");
                }
            },
            error: function (xhr) {
                let errorMsg = "Terjadi kesalahan pada sistem.";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMsg = xhr.responseJSON.message;
                }
                toastr.error(errorMsg, "Error");
            }
        });
    }

    // Fungsi untuk merender baris data tabel
    function renderTable(result) {
        let tbody = $('#logTableBody');
        tbody.empty();

        // Update counter teks di atas
        $('#counterInfo').html(`Showing <strong>${result.totalItems}</strong> events`);
        $('#paginationInfoText').text(`Menampilkan ${result.firstItemIndex} - ${result.lastItemIndex} dari ${result.totalItems} data`);

        if (!result.items || result.items.length === 0) {
            let emptyHtml = `
                <tr>
                    <td colspan="6" class="text-center py-5">
                        <div class="empty-state-container">
                            <div class="empty-state-icon fs-1 text-muted mb-2">
                                <i class="bi bi-clock-history"></i>
                            </div>
                            <h6 class="fw-bold text-dark mb-1">No activity logs found</h6>
                            <p class="text-muted small mb-0">Try adjusting your search criteria</p>
                        </div>
                    </td>
                </tr>`;
            tbody.append(emptyHtml);
            return;
        }

        // Loop data dan masukkan ke tabel
        $.each(result.items, function (index, item) {
            let badgeClass = "bg-secondary";
            if (item.event === "CREATE") badgeClass = "bg-success";
            else if (item.event === "UPDATE") badgeClass = "bg-primary";
            else if (item.event === "DELETE") badgeClass = "bg-danger";
            else if (item.event === "LOGIN") badgeClass = "bg-info";

            let row = `
                <tr>
                    <td class="fw-medium text-dark">${escapeHtml(item.causer)}</td>
                    <td><span class="badge ${badgeClass}">${escapeHtml(item.event)}</span></td>
                    <td>${escapeHtml(item.description)}</td>
                    <td><span class="text-muted">${escapeHtml(item.subject)}</span></td>
                    <td class="text-muted small">${escapeHtml(item.timeFormatted)}</td>
                    <td class="text-end">
                        <button class="btn btn-sm btn-outline-primary btn-detail" data-id="${item.Id || item.id}" title="Lihat Detail">
                            <i class="bi bi-eye"></i>
                        </button>
                    </td>
                </tr>`;
            tbody.append(row);
        });
    }

    // Fungsi untuk merender komponen Bootstrap Pagination sebelah kanan
    function renderPagination(result) {
        let paginationContainer = $('#paginationContainer');
        paginationContainer.empty();

        if (result.totalPages <= 1) return;

        // Tombol Previous
        let prevDisabled = result.pageNumber === 1 ? 'disabled' : '';
        paginationContainer.append(`
            <li class="page-item ${prevDisabled}">
                <a class="page-link" href="#" data-page="${result.pageNumber - 1}">Previous</a>
            </li>
        `);

        // Nomor Halaman Dinamis
        for (let i = 1; i <= result.totalPages; i++) {
            // Batasi render halaman agar tidak terlalu panjang jika halaman banyak
            if (i === 1 || i === result.totalPages || (i >= result.pageNumber - 1 && i <= result.pageNumber + 1)) {
                let activeClass = i === result.pageNumber ? 'active' : '';
                paginationContainer.append(`
                    <li class="page-item ${activeClass}">
                        <a class="page-link" href="#" data-page="${i}">${i}</a>
                    </li>
                `);
            } else if (i === result.pageNumber - 2 || i === result.pageNumber + 2) {
                paginationContainer.append(`<li class="page-item disabled"><span class="page-link">...</span></li>`);
            }
        }

        // Tombol Next
        let nextDisabled = result.pageNumber === result.totalPages ? 'disabled' : '';
        paginationContainer.append(`
            <li class="page-item ${nextDisabled}">
                <a class="page-link" href="#" data-page="${result.pageNumber + 1}">Next</a>
            </li>
        `);
    }

    // Event Klik Pagination tanpa reload halaman
    $(document).on('click', '#paginationContainer .page-link', function (e) {
        e.preventDefault();
        let targetPage = $(this).closest('.page-item').data('page') || $(this).data('page');
        
        if (targetPage && targetPage !== currentPage && !$(this).closest('.page-item').hasClass('disabled')) {
            currentPage = targetPage;
            loadActivityLogs(currentPage, searchKeyword);
        }
    });

    // Event klik tombol Detail
    $(document).on('click', '.btn-detail', function () {
        let id = $(this).data('id');
        
        $.ajax({
            url: '/ActivityLogs/GetDetail',
            type: 'GET',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    let d = response.data;
                    $('#modalCauser').text(d.causer);
                    $('#modalEvent').text(d.event);
                    $('#modalDescription').text(d.description);
                    $('#modalSubject').text(d.subject);
                    $('#modalTime').text(d.timeFormatted);
                    
                    $('#detailLogModal').modal('show');
                    toastr.success(response.message, "Sukses");
                } else {
                    toastr.warning(response.message, "NotFound");
                }
            },
            error: function (xhr) {
                let msg = xhr.responseJSON ? xhr.responseJSON.message : "Gagal mengambil detail log.";
                toastr.error(msg, "Error");
            }
        });
    });

    // Helper aman dari XSS
    function escapeHtml(text) {
        if (!text) return "";
        return $('<div>').text(text).html();
    }
});