let currentPage = 1;
let searchTimer = null;

$(document.body).ready(function () {
    loadCustomerData(1);

    // Live Search Ajax tanpa reload page saat mengetik (Debounce 400ms)
    $('#txtSearch').on('keyup input', function () {
        clearTimeout(searchTimer);
        searchTimer = setTimeout(function () {
            currentPage = 1;
            loadCustomerData(currentPage);
        }, 400);
    });

    // Filter Status Ajax
    $('#selectStatus').on('change', function () {
        currentPage = 1;
        loadCustomerData(currentPage);
    });

    // Submit Create Form -> Reload Ke Index.cshtml
    $('#formTambahCustomer').on('submit', function (e) {
        e.preventDefault();
        var $form = $(this);

        if (!$form.valid()) return false;

        var formData = new FormData(this);
        var token = $('#__RequestVerificationToken').val();

        $.ajax({
            url: '/Customer/Create',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            headers: { "RequestVerificationToken": token },
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message);
                    $('#modalTambahCustomer').modal('hide');

                    setTimeout(function () {
                        window.location.reload();
                    }, 500);
                } else {
                    toastr.error(res.message || "Gagal menyimpan data.");
                }
            },
            error: function () {
                toastr.error("Terjadi kesalahan sistem.");
            }
        });
    });

    // Submit Edit Form -> Reload Ke Index.cshtml
    $('#formEditCustomer').on('submit', function (e) {
        e.preventDefault();
        var $form = $(this);

        if (!$form.valid()) return false;

        var formData = new FormData(this);
        var token = $('#__RequestVerificationToken').val();

        $.ajax({
            url: '/Customer/Edit',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            headers: { "RequestVerificationToken": token },
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message);
                    $('#modalEditCustomer').modal('hide');

                    setTimeout(function () {
                        window.location.reload();
                    }, 500);
                } else {
                    toastr.error(res.message || "Gagal memperbarui data.");
                }
            },
            error: function () {
                toastr.error("Terjadi kesalahan sistem.");
            }
        });
    });

    // Confirm Delete Action -> Reload Ke Index.cshtml
    $('#btnConfirmDelete').on('click', function () {
        var id = $('#Delete_Id').val();
        var token = $('#__RequestVerificationToken').val();

        $.ajax({
            url: '/Customer/Delete',
            type: 'POST',
            data: { id: id },
            headers: { "RequestVerificationToken": token },
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message);
                    $('#modalHapusCustomer').modal('hide');

                    setTimeout(function () {
                        window.location.reload();
                    }, 500);
                } else {
                    toastr.error(res.message || "Gagal menghapus data.");
                }
            },
            error: function () {
                toastr.error("Terjadi kesalahan sistem.");
            }
        });
    });
});

// Load Data Function (Live Search & Pagination)
function loadCustomerData(page) {
    currentPage = page || 1;
    var search = $('#txtSearch').val();
    var status = $('#selectStatus').val();

    $.ajax({
        url: '/Customer/GetData',
        type: 'GET',
        data: {
            search: search,
            status: status,
            page: currentPage,
            pageSize: 10
        },
        success: function (response) {
            $('#customerTableContainer').html(response);
        },
        error: function () {
            toastr.error("Gagal memuat data customer.");
        }
    });
}

function loadPage(page) {
    loadCustomerData(page);
}

// Reset Form Tambah
function resetFormCreate() {
    var $form = $('#formTambahCustomer');
    $form[0].reset();
    $form.find('.text-danger').text('');    
    if (typeof $.validator !== 'undefined' && typeof $.validator.unobtrusive !== 'undefined') {
        $form.removeData("validator").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($form);
    }
}

// Open Edit Modal & Populating Data
function editCustomer(id) {
    $.ajax({
        url: '/Customer/GetById',
        type: 'GET',
        data: { id: id },
        success: function (res) {
            if (res.success) {
                var data = res.data;
                $('#Edit_Id').val(data.id);
                $('#Edit_NamaCustomer').val(data.namaCustomer);
                $('#Edit_Email').val(data.email);
                $('#Edit_NoTelepon').val(data.noTelepon);
                $('#Edit_Alamat').val(data.alamat);
                $('#Edit_checkCustomerAktif').prop('checked', data.isActive);

                var $form = $('#formEditCustomer');
                $form.find('.text-danger').text('');
                
                if (typeof $.validator !== 'undefined' && typeof $.validator.unobtrusive !== 'undefined') {
                    $form.removeData("validator").removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse($form);
                }

                $('#modalEditCustomer').modal('show');
            } else {
                toastr.error(res.message);
            }
        }
    });
}

// Open Delete Modal
function confirmDeleteCustomer(id) {
    $('#Delete_Id').val(id);
    $('#modalHapusCustomer').modal('show');
}