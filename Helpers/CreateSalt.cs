using System.Security.Cryptography;

namespace QuaMonBa_Store.Helpers
{
    public class CreateSalt
    {
        /// <summary>
        /// Tạo một chuỗi Salt ngẫu nhiên và an toàn.
        /// </summary>
        /// <param name="size">Kích thước của Salt (tính bằng byte). Mặc định là 16 byte (128 bit).</param>
        /// <returns>Chuỗi Salt đã được mã hóa Base64 để dễ lưu vào Database.</returns>
        public static string GenerateSalt(int size = 16)
        {
            // Khởi tạo mảng byte để chứa dữ liệu ngẫu nhiên
            byte[] saltBytes = new byte[size];

            // Sử dụng RandomNumberGenerator để lấp đầy mảng bằng các byte ngẫu nhiên
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // Trả về chuỗi Base64 (để dễ dàng lưu kiểu NVARCHAR/VARCHAR trong SQL Server)
            return Convert.ToBase64String(saltBytes);
        }
    }
}
