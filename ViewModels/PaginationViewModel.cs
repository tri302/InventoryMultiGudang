namespace InventoryMultiGudang.ViewModels
{
    public class PaginationViewModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }        
        public bool HasPrevious =>
            CurrentPage > 1;
        public bool HasNext =>
            CurrentPage < TotalPages;
        public int TotalPages =>
            TotalItems == 0
                ? 0
                : (int)Math.Ceiling((double)TotalItems / PageSize);
        public int StartItem =>
            TotalItems == 0
                ? 0
                : ((CurrentPage - 1) * PageSize) + 1;
        public int EndItem =>
            Math.Min(CurrentPage * PageSize, TotalItems);
    }
}
