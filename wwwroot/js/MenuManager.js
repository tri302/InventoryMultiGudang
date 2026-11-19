/**
 * Menu Manager Module
 * Menangani CRUD AJAX Dinamis untuk Menu Item dan Group
 */

toastr.options = {
  closeButton: true,
  progressBar: true,
  positionClass: "toast-top-center",
  preventDuplicates: true,
  showDuration: "300",
  hideDuration: "1000",
  timeOut: "3000",
};

let currentPage = 1;
const pageSize = 5;

$(document).ready(function () {
  // 1. Inisialisasi Load Data Menu Utama
  loadMenuData(currentPage);
  // 2. Picker Icon Event
  $(document).on("click", ".icon-option", function () {
    $(".icon-option").removeClass("active border-primary bg-light");
    $(this).addClass("active border-primary bg-light");
    const selectedIcon = $(this).data("icon");
    $("#addIconClass").val(selectedIcon);
  });

  // 3. Buka Modal Add Menu Item (Tambah Baru)  
  $("#btnOpenAddModal").on("click", function () {
    const $form = $("#formAddMenu");
    $form[0].reset();
    $form.find(".is-valid, .is-invalid").removeClass("is-valid is-invalid");
    
    // Reset Hidden ID menjadi 0 (Tanda Tambah Baru)
    $("#addMenuId").val(0);

    $(".icon-option").removeClass("active border-primary bg-light");
    $(".icon-option").first().addClass("active border-primary bg-light");
    $("#addIconClass").val($(".icon-option").first().data("icon"));

    // Populate dropdown group & load roles/permissions dinamis dari DB
    populateGroupDropdown();
    loadRolesAndPermissions();

    // Set judul modal dinamis untuk Tambah Baru (id = 0)
    $("#modalAddMenuLabel").html(
      '<i class="bi bi-plus-circle me-2 text-indigo"></i>Add Menu Item',
    );
    $("#modalAddMenu").modal("show");
  });

  // 4. Buka Modal Manage Groups -> Load Data Group Dinamis dari DB
  $('[data-bs-target="#modalManageGroups"]').on("click", function () {
    loadGroupData();
  });

  // 5. Live Validation
  $("#formAddMenu input, #formAddMenu select").on("input change", function () {
    validateInput($(this));
  });

  // 6. Submit Add / Edit Menu via AJAX (Satu Endpoint SaveMenu)
  $("#formAddMenu").on("submit", function (e) {
    e.preventDefault();

    let isAllValid = true;
    $(this)
      .find("input[required], select[required]")
      .each(function () {
        if (!validateInput($(this))) {
          isAllValid = false;
        }
      });

    if (!isAllValid) {
      toastr.warning(
        "Mohon lengkapi semua field yang wajib diisi!",
        "Peringatan Validasi",
      );
      return;
    }

    const token = $('input[name="__RequestVerificationToken"]').val();

    const formData = {
      Id: parseInt($("#addMenuId").val()) || 0, // 0 = Insert, > 0 = Update
      MenuTitle: $("#addMenuTitle").val(),
      Url: $("#addUrl").val(),
      GroupName: $("#addGroupName").val(),
      ParentMenu: $("#addParentMenu").val() || null,
      SortOrder: parseInt($("#addSortOrder").val()) || 1,
      IconClass: $("#addIconClass").val(),
      SelectedRoles: $('input[name="SelectedRoles"]:checked')
        .map(function () {
          return $(this).val();
        })
        .get(),
      SelectedPermissions: $('input[name="SelectedPermissions"]:checked')
        .map(function () {
          return $(this).val();
        })
        .get(),
    };

    $.ajax({
      url: "/MenuManager/SaveMenu",
      type: "POST",
      contentType: "application/json",
      headers: { RequestVerificationToken: token },
      data: JSON.stringify(formData),
      success: function (response) {
        if (response.success) {
          toastr.success(response.message, "Berhasil");
          $("#modalAddMenu").modal("hide");
          loadMenuData(currentPage);
        } else {
          toastr.error(response.message || "Gagal menyimpan data.", "Error");
        }
      },
      error: function (xhr) {
        let msg = "Terjadi kesalahan sistem.";
        if (xhr.responseJSON && xhr.responseJSON.message) {
          msg = xhr.responseJSON.message;
        }
        toastr.error(msg, "Gagal");
      },
    });
  });

  // 7. Delete Menu Item
  $(document).on("click", ".btn-delete-menu", function (e) {
    e.preventDefault();
    const menuId = $(this).data("id");
    const menuTitle = $(this).data("title");

    Swal.fire({
      title: "Hapus Menu Item?",
      html: `Apakah Anda yakin ingin menghapus menu <strong>${menuTitle}</strong>?<br><span class="text-danger small">Tindakan ini tidak dapat dibatalkan.</span>`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#6610f2",
      cancelButtonColor: "#6c757d",
      confirmButtonText: '<i class="bi bi-trash"></i> Ya, Hapus',
      cancelButtonText: "Batal",
      customClass: {
        confirmButton: "btn btn-danger btn-sm px-3",
        cancelButton: "btn btn-secondary btn-sm px-3",
      },
    }).then((result) => {
      if (result.isConfirmed) {
        deleteMenu(menuId);
      }
    });
  });

  // 8. Rename Group Handler
  $(document).on("click", ".btn-trigger-rename", function () {
    const groupName = $(this).data("group-name");
    $("#oldGroupName").val(groupName);
    $("#newGroupName").val(groupName);

    $("#modalManageGroups").modal("hide");
    $("#modalRenameGroup").modal("show");
  });

  $("#btnSubmitRename").on("click", function () {
    const oldName = $("#oldGroupName").val();
    const newName = $("#newGroupName").val();

    if (!newName.trim()) {
      toastr.warning("Nama group baru tidak boleh kosong!", "Peringatan");
      return;
    }

    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
      url: "/MenuManager/RenameGroup",
      type: "POST",
      contentType: "application/json",
      headers: { RequestVerificationToken: token },
      data: JSON.stringify({ OldGroupName: oldName, NewGroupName: newName }),
      success: function (response) {
        if (response.success) {
          toastr.success(response.message, "Berhasil");
          $("#modalRenameGroup").modal("hide");
          loadMenuData(currentPage);
          loadGroupData();
        } else {
          toastr.error(response.message, "Gagal");
        }
      },
      error: function () {
        toastr.error("Gagal mengubah nama group.", "Error");
      },
    });
  });

  // 9. Delete Group Handler
  $(document).on("click", ".btn-trigger-delete-group", function () {
    const groupName = $(this).data("group-name");

    Swal.fire({
      title: "Hapus Menu Group?",
      html: `Apakah Anda yakin ingin menghapus group <strong>${groupName}</strong>?<br><span class="text-danger small">Tindakan ini akan menghapus permanen semua menu item di dalam group ini!</span>`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#6610f2",
      cancelButtonColor: "#6c757d",
      confirmButtonText: '<i class="bi bi-trash"></i> Ya, Hapus',
      cancelButtonText: "Batal",
    }).then((result) => {
      if (result.isConfirmed) {
        $.ajax({
          url: `/MenuManager/DeleteGroup?groupName=${encodeURIComponent(groupName)}`,
          type: "DELETE",
          success: function (response) {
            if (response.success) {
              toastr.success(response.message, "Terhapus!");
              $("#modalManageGroups").modal("hide");
              loadMenuData(currentPage);
              loadGroupData();
            } else {
              toastr.error(response.message, "Gagal");
            }
          },
          error: function () {
            toastr.error("Gagal menghapus group menu.", "Error");
          },
        });
      }
    });
  });

  // 10. Tambah Group Baru dari Modal Manage Groups
  $("#btnSaveNewGroup").on("click", function () {
    const groupName = $("#inputNewGroupName").val().trim();

    if (!groupName) {
      toastr.warning("Nama group tidak boleh kosong!", "Peringatan");
      return;
    }

    const token = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
      url: "/MenuManager/CreateGroup",
      type: "POST",
      contentType: "application/json",
      headers: { RequestVerificationToken: token },
      data: JSON.stringify({ NewGroupName: groupName }),
      success: function (response) {
        if (response.success) {
          toastr.success(response.message, "Berhasil");
          $("#inputNewGroupName").val("");
          loadGroupData();
        } else {
          toastr.error(response.message, "Gagal");
        }
      },
      error: function (xhr) {
        let msg = "Terjadi kesalahan sistem.";
        if (xhr.responseJSON && xhr.responseJSON.message) {
          msg = xhr.responseJSON.message;
        }
        toastr.error(msg, "Gagal");
      },
    });
  });

  // 11. Trigger Modal Edit Menu Item (Sinkron dengan #modalAddMenu)
  $(document).on("click", ".btn-edit-menu", function () {
    const menuId = $(this).data("id");

    $.ajax({
      url: `/MenuManager/GetMenuById/${menuId}`,
      type: "GET",
      success: function (response) {
        if (response.success && response.data) {
          const menu = response.data;

          // Load group dropdown & roles/permissions dengan data yang sudah ada dari menu
          populateGroupDropdown(menu.groupName);
          loadRolesAndPermissions(menu.requireRoles, menu.requirePermissions);

          // Isi form modal dengan data dari database (id > 0)
          $("#addMenuId").val(menu.id);
          $("#addMenuTitle").val(menu.menuTitle);
          $("#addUrl").val(menu.url);
          $("#addParentMenu").val(menu.parentMenu || "");
          $("#addSortOrder").val(menu.sortOrder);
          $("#addIconClass").val(menu.iconClass || "bi-speedometer2");

          // Highlight icon yang sesuai di grid icon picker
          $(".icon-option").removeClass("active border-primary bg-light");
          $(`.icon-option[data-icon="${menu.iconClass}"]`).addClass("active border-primary bg-light");

          // Set judul modal dinamis untuk Edit (id > 0)
          $("#modalAddMenuLabel").html(
            '<i class="bi bi-pencil-square me-2 text-indigo"></i>Edit Menu Item',
          );

          // Tampilkan modal
          $("#modalAddMenu").modal("show");
        } else {
          toastr.error("Data menu tidak ditemukan.", "Gagal");
        }
      },
      error: function () {
        toastr.error("Gagal mengambil data menu dari server.", "Error");
      },
    });
  });
});

/**
 * Fetch dan Render Daftar Group secara Dinamis dari Database
 */
function loadGroupData() {
  const $container = $("#groupListContainer");

  $.ajax({
    url: "/MenuManager/GetGroups",
    type: "GET",
    dataType: "json",
    success: function (response) {
      if (response.success && response.data) {
        renderGroupList(response.data);
      } else {
        $container.html(
          '<div class="text-muted text-center py-3">Tidak ada group ditemukan.</div>',
        );
      }
    },
    error: function () {
      $container.html(
        '<div class="text-danger text-center py-3">Gagal memuat daftar group.</div>',
      );
    },
  });
}

function renderGroupList(groups) {
  const $container = $("#groupListContainer");
  $container.empty();

  if (groups.length === 0) {
    $container.html(
      '<div class="text-muted text-center py-3">Belum ada group menu.</div>',
    );
    return;
  }

  let html = "";
  groups.forEach((group, index) => {
    const borderClass = index === groups.length - 1 ? "" : "border-bottom";
    html += `
      <div class="group-item-row d-flex justify-content-between align-items-center py-3 ${borderClass}">
          <span class="group-item-title fw-semibold text-secondary">${group}</span>
          <div class="d-flex align-items-center gap-2">
              <button type="button" class="btn btn-sm btn-light text-primary border-0 btn-trigger-rename" data-group-name="${group}" title="Edit Group">
                  <i class="bi bi-pencil"></i>
              </button>
              <button type="button" class="btn btn-sm btn-light text-danger border-0 btn-trigger-delete-group" data-group-name="${group}" title="Hapus Group">
                  <i class="bi bi-trash"></i>
              </button>
          </div>
      </div>
    `;
  });

  $container.html(html);
}

/**
 * Mengisi Dropdown Select Group Name secara Dinamis
 */
function populateGroupDropdown(selectedGroup = "") {
  $.ajax({
    url: "/MenuManager/GetGroups",
    type: "GET",
    dataType: "json",
    success: function (response) {
      const $select = $("#addGroupName");
      $select.empty().append('<option value="">-- Pilih Group --</option>');

      if (response.success && response.data) {
        response.data.forEach((group) => {
          const isSelected = group === selectedGroup ? "selected" : "";
          $select.append(`<option value="${group}" ${isSelected}>${group}</option>`);
        });
      }
    },
  });
}

/**
 * Fetch dan Render Checkbox Roles & Permissions secara Dinamis dari Database
 */
function loadRolesAndPermissions(selectedRoles = "", selectedPermissions = "") {
  const rolesArray = selectedRoles ? selectedRoles.split(",") : [];
  const permsArray = selectedPermissions ? selectedPermissions.split(",") : [];

  $.ajax({
    url: "/MenuManager/GetRolesAndPermissions",
    type: "GET",
    dataType: "json",
    success: function (response) {
      if (response.success) {
        const $rolesContainer = $("#rolesCheckboxContainer");
        const $permsContainer = $("#permissionsCheckboxContainer");

        $rolesContainer.empty();
        $permsContainer.empty();

        // Render Roles Checkbox
        if (response.roles && response.roles.length > 0) {
          response.roles.forEach((role, index) => {
            const isChecked = rolesArray.includes(role) ? "checked" : "";
            $rolesContainer.append(`
              <div class="form-check">
                  <input class="form-check-input" type="checkbox" name="SelectedRoles" value="${role}" id="role_${index}" ${isChecked}>
                  <label class="form-check-label small" for="role_${index}">${role}</label>
              </div>
            `);
          });
        } else {
          $rolesContainer.html('<div class="text-muted small text-center">Tidak ada role.</div>');
        }

        // Render Permissions Checkbox
        if (response.permissions && response.permissions.length > 0) {
          response.permissions.forEach((perm, index) => {
            const isChecked = permsArray.includes(perm) ? "checked" : "";
            $permsContainer.append(`
              <div class="form-check">
                  <input class="form-check-input" type="checkbox" name="SelectedPermissions" value="${perm}" id="perm_${index}" ${isChecked}>
                  <label class="form-check-label small" for="perm_${index}">${perm}</label>
              </div>
            `);
          });
        } else {
          $permsContainer.html('<div class="text-muted small text-center">Tidak ada permission.</div>');
        }
      }
    },
    error: function () {
      toastr.error("Gagal memuat daftar role dan permission dari database.", "Error");
    },
  });
}

function validateInput($input) {
  const val = $.trim($input.val());
  const isRequired = $input.prop("required");

  if (isRequired && val === "") {
    $input.removeClass("is-valid").addClass("is-invalid");
    return false;
  } else {
    $input.removeClass("is-invalid").addClass("is-valid");
    return true;
  }
}

function loadMenuData(page) {
  console.log("[DEBUG] Memulai AJAX request untuk memuat data menu...");

  $.ajax({
    url: "/MenuManager/GetAllMenus",
    type: "GET",
    dataType: "json",
    success: function (response) {
      console.log("[DEBUG] Respon AJAX berhasil diterima dari server:", response);

      if (response.success) {
        console.log("[DEBUG] Jumlah data menu yang didapat:", response.data ? response.data.length : 0);
        
        renderMenuList(response.data);
        renderPagination(response.data ? response.data.length : 0, page);
      } else {
        console.warn("[DEBUG] Server merespons sukses, tetapi properti success bernilai false:", response.message);
        toastr.error(response.message || "Gagal memuat data.", "Error");
        
        $("#menuListContainer").html(`
          <div class="alert alert-danger text-center py-3" role="alert">
              <i class="bi bi-exclamation-triangle fs-4 d-block mb-2"></i>
              Gagal memuat data menu dari server.
          </div>
        `);
      }
    },
    error: function (xhr, status, error) {
      console.error("[DEBUG] AJAX Error terjadi!");
      console.error("Status:", status);
      console.error("Error thrown:", error);
      console.error("XHR Response Text:", xhr.responseText);

      toastr.error("Gagal memuat data dari server.", "Error");
      
      // Tampilkan error detail di UI container agar terlihat langsung
      $("#menuListContainer").html(`
        <div class="alert alert-danger text-center py-3" role="alert">
            <i class="bi bi-exclamation-triangle fs-4 d-block mb-2"></i>
            Terjadi kesalahan saat menghubungi server (Status: ${xhr.status}). Cek Console F12 untuk detail.
        </div>
      `);
    },
  });
}

function renderMenuList(data) {
  const $container = $("#menuListContainer");
  $container.empty();

  if (!data || data.length === 0) {
    $container.html(`
      <div class="alert alert-info text-center py-4" role="alert">
          <i class="bi bi-info-circle fs-4 d-block mb-2"></i>
          Belum ada data menu item yang dikonfigurasi.
      </div>
    `);
    return;
  }

  const grouped = data.reduce((acc, obj) => {
    const key = obj.groupName || "Other";
    if (!acc[key]) acc[key] = [];
    acc[key].push(obj);
    return acc;
  }, {});

  let html = "";
  for (const groupName in grouped) {
    const items = grouped[groupName];
    html += `
      <div class="mb-4">
          <div class="menu-group-header d-flex justify-content-between align-items-center p-2 rounded mb-2" style="background-color: #f8f9fa; border-left: 4px solid #6610f2;">
              <span class="fw-bold text-indigo"><i class="bi bi-folder2-open me-2"></i> Group: ${groupName}</span>
              <span class="badge bg-dark text-white fw-bold">${items.length} Items</span>
          </div>
    `;

    items.forEach((item) => {
      html += `
        <div class="menu-item-card p-3 mb-2 border rounded bg-white shadow-sm">
            <div class="row align-items-center gy-2">
                <div class="col-auto">
                    <i class="bi bi-grip-vertical text-muted"></i>
                </div>
                <div class="col-auto">
                    <div class="icon-box d-flex align-items-center justify-content-center bg-light rounded" style="width: 38px; height: 38px;">
                        <i class="bi ${item.iconClass || "bi-link-45deg"} text-indigo fs-5"></i>
                    </div>
                </div>
                <div class="col">
                    <div class="fw-bold text-dark">${item.menuTitle}</div>
                    <div class="small text-muted">${item.url}</div>
                </div>
                <div class="col-md-3">
                    <span class="badge bg-light text-dark border">
                        <i class="bi bi-shield-check text-success me-1"></i> ${item.requirePermissions || "Public (All users)"}
                    </span>
                </div>
                <div class="col-auto ms-auto d-flex align-items-center gap-2">
                    <span class="badge bg-secondary">Order: #${item.sortOrder}</span>
                    <button class="btn btn-sm btn-outline-primary btn-edit-menu" data-id="${item.id}" title="Edit"><i class="bi bi-pencil"></i></button>
                    <button class="btn btn-sm btn-outline-danger btn-delete-menu" data-id="${item.id}" data-title="${item.menuTitle}" title="Hapus"><i class="bi bi-trash"></i></button>
                </div>
            </div>
        </div>
      `;
    });

    html += `</div>`;
  }

  $container.html(html);
}

function renderPagination(totalItems, page) {
  const totalPages = Math.ceil(totalItems / pageSize) || 1;
  const startItem = totalItems === 0 ? 0 : (page - 1) * pageSize + 1;
  const endItem = Math.min(page * pageSize, totalItems);

  $("#paginationInfo").text(
    `Menampilkan ${startItem} - ${endItem} dari ${totalItems} data`,
  );

  let $nav = $("#paginationNav");
  $nav.empty();

  $nav.append(`
    <li class="page-item ${page === 1 ? "disabled" : ""}">
        <a class="page-link" href="#" onclick="changePage(${page - 1}); return false;">Previous</a>
    </li>
  `);

  for (let i = 1; i <= totalPages; i++) {
    $nav.append(`
      <li class="page-item ${i === page ? "active" : ""}">
          <a class="page-link" href="#" onclick="changePage(${i}); return false;">${i}</a>
      </li>
    `);
  }

  $nav.append(`
    <li class="page-item ${page === totalPages || totalItems === 0 ? "disabled" : ""}">
        <a class="page-link" href="#" onclick="changePage(${page + 1}); return false;">Next</a>
    </li>
  `);
}

function changePage(page) {
  currentPage = page;
  loadMenuData(currentPage);
}

function deleteMenu(id) {
  $.ajax({
    url: `/MenuManager/DeleteMenu/${id}`,
    type: "DELETE",
    success: function (response) {
      if (response.success) {
        toastr.success(response.message, "Terhapus!");
        loadMenuData(currentPage);
      } else {
        toastr.error(response.message, "Gagal");
      }
    },
    error: function (xhr) {
      let msg = "Gagal menghapus data menu.";
      if (xhr.responseJSON && xhr.responseJSON.message) {
        msg = xhr.responseJSON.message;
      }
      toastr.error(msg, "Error");
    },
  });
}