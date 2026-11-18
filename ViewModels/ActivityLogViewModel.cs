using System;

namespace InventoryMultiGudang.ViewModels
{
    public class ActivityLogViewModel
    {
        public long Id { get; set; }
        public string Causer { get; set; }
        public string Event { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string TimeFormatted { get; set; }
    }

    public class PagedResultViewModel<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int FirstItemIndex { get; set; }
        public int LastItemIndex { get; set; }
        public System.Collections.Generic.IEnumerable<T> Items { get; set; }
    }
}