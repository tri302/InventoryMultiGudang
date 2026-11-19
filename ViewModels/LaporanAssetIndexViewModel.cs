namespace InventoryMultiGudang.ViewModels
{
    public class LaporanAssetIndexViewModel
    {
        public int TotalAsetBerseri { get; set; }
        public int AsetTersedia { get; set; }
        public int AsetDipinjamkan { get; set; }
        public int AsetDitugaskan { get; set; }
        public int DalamReparasi { get; set; }
        public int AsetHilang { get; set; }
        public int Dihapuskan { get; set; }
        public double RasioUtilitas { get; set; }
        public int Sold { get; set; }
    }
}