namespace InventoryMultiGudang.ViewModels
{
    public class KartuStokFilterViewModel
    {
        public int GudangId { get; set; }

        public int ProdukId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
