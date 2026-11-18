namespace InventoryMultiGudang.Models
{
    public class TransferOrderDetail
    {
        public int Id { get; set; }
        public int TransferOrderId { get; set; }
        public TransferOrder? TransferOrder { get; set; }

        public int ProdukId { get; set; }
        public Produk? Produk { get; set; }

        public int KuantitasPermintaan { get; set; }
        public int KuantitasDiterima { get; set; }
    }
}
