namespace QuaMonBa_Store.data
{
    public class GioHang
    {
        public int MaHH { get; set; }
        public string TenHH { get; set; } = string.Empty;
        public string Hinh { get; set; } = string.Empty;
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien => SoLuong * DonGia;
    }
}
