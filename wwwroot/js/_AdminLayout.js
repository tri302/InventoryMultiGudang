/** 
 * Berisi skrip global untuk pengaturan Sidebar, Sortable Widgets, dan Active Menu Navigation.
 */

document.addEventListener('DOMContentLoaded', function () {
    // 1. Sidebar OverlayScrollbars Setup
    const SELECTOR_SIDEBAR_WRAPPER = '.sidebar-wrapper';
    const Default = {
        scrollbarTheme: 'os-theme-light',
        scrollbarAutoHide: 'leave',
        scrollbarClickScroll: true,
    };

    const sidebarWrapper = document.querySelector(SELECTOR_SIDEBAR_WRAPPER);
    if (sidebarWrapper && typeof OverlayScrollbarsGlobal?.OverlayScrollbars !== 'undefined') {
        OverlayScrollbarsGlobal.OverlayScrollbars(sidebarWrapper, {
            scrollbars: {
                theme: Default.scrollbarTheme,
                autoHide: Default.scrollbarAutoHide,
                clickScroll: Default.scrollbarClickScroll,
            },
        });
    }

    // 2. Safe check agar Sortable tidak crash jika elemen tidak ditemukan di halaman non-dashboard
    const sortableElem = document.querySelector('.connectedSortable');
    if (sortableElem && typeof Sortable !== 'undefined') {
        new Sortable(sortableElem, {
            group: 'shared',
            handle: '.card-header',
        });

        const cardHeaders = document.querySelectorAll('.connectedSortable .card-header');
        cardHeaders.forEach((cardHeader) => {
            cardHeader.style.cursor = 'move';
        });
    }
});

// 3. Dynamic Active Menu Highlighting berdasarkan URL saat ini
$(document).ready(function () {
    var currentPath = window.location.pathname.toLowerCase();

    $('#navigation .nav-link').each(function () {
        var $this = $(this);
        var href = $this.attr('href');

        if (href && href !== '#' && currentPath.includes(href.toLowerCase())) {
            // Hapus class active dari semua menu/submenu lain
            $('#navigation .nav-link').removeClass('active');
            
            // Tambahkan class active ke menu yang sesuai
            $this.addClass('active');

            // Buka parent menu jika berada di dalam treeview
            $this.parents('.nav-item').addClass('menu-open');
            $this.closest('.nav-treeview').show();
        }
    });
});