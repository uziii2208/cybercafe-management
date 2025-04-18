# Hướng Dẫn Cấu Hình

## Tổng Quan
Hướng dẫn này giải thích cách cấu hình Hệ Thống Quản Lý Tiệm Net cho môi trường của bạn.

## Cấu Hình Cơ Sở Dữ Liệu

### Cơ Sở Dữ Liệu SQLite
Ứng dụng sử dụng SQLite để lưu trữ dữ liệu. File cơ sở dữ liệu được tự động tạo tại:
```
src/Presentation/cybercafe.db
```

Bạn có thể thay đổi vị trí cơ sở dữ liệu bằng cách chỉnh sửa chuỗi kết nối trong `Program.cs`.

## Cấu Hình Ghi Log

### Cài Đặt Serilog
Log được ghi vào:
```
src/Presentation/logs/cybercafe.log
```

Cài đặt log được cấu hình trong `Program.cs`:
- Mức tối thiểu: Debug
- Bật hiển thị trên console
- Xoay vòng file log hàng ngày
- Định dạng log có cấu trúc

### Các Mức Log
- `Debug`: Thông tin chi tiết để gỡ lỗi
- `Information`: Sự kiện hoạt động thông thường
- `Warning`: Vấn đề không nghiêm trọng
- `Error`: Lỗi ứng dụng cần chú ý
- `Fatal`: Lỗi nghiêm trọng cần xử lý ngay

## Cấu Hình Quy Tắc Kinh Doanh

### Hạng Thành Viên
- Thường: Không có ưu đãi đặc biệt
- Bạc: Giảm 5% phí phiên chơi game
- Vàng: Giảm 10% phí phiên và 5% phí dịch vụ

### Hệ Thống Điểm
- 1 điểm cho mỗi giờ chơi game
- 1 điểm cho mỗi 10$ chi tiêu dịch vụ
- Ngưỡng điểm:
  - Bạc: 250 điểm
  - Vàng: 500 điểm

### Loại Máy
Giá mặc định theo giờ:
- Cao cấp (RTX 4080): 5.00$/giờ
- Trung cấp (RTX 4070): 4.00$/giờ
- Phổ thông (RTX 4060): 3.00$/giờ

### Loại Dịch Vụ
- Thức ăn
- Đồ uống
- Thiết bị ngoại vi
- Khác

### Quản Lý Tồn Kho
- Cảnh báo tồn kho thấp khi CurrentStock ≤ MinimumStock
- Ghi log cập nhật tồn kho để kiểm tra

## Cài Đặt Hiệu Năng

### Tối Ưu Cơ Sở Dữ Liệu
- Bật connection pooling
- Tắt lazy loading để tăng hiệu năng
- Đánh index cho các trường thường truy vấn

### Quản Lý Bộ Nhớ
- Phân trang cho kết quả lớn
- Mẫu truy vấn hiệu quả
- Dọn dẹp dữ liệu phiên cũ định kỳ

## Cài Đặt Bảo Mật

### Xác Thực Đầu Vào
- Bắt buộc các trường yêu cầu
- Xác thực kiểu dữ liệu
- Giới hạn độ dài
- Xử lý ký tự đặc biệt

### Bảo Vệ Dữ Liệu
- Xác thực địa chỉ email
- Định dạng số điện thoại
- Xử lý an toàn dữ liệu nhạy cảm

## Môi Trường Kiểm Thử

### Unit Tests
Chạy bằng lệnh:
```bash
dotnet test tests/UnitTests
```

### Integration Tests
Chạy bằng lệnh:
```bash
dotnet test tests/IntegrationTests
```

## Bảo Trì

### Bảo Trì Cơ Sở Dữ Liệu
- Khuyến nghị sao lưu thường xuyên
- Dọn dẹp dữ liệu cũ định kỳ
- Bảo trì index

### Quản Lý Log
- Xoay vòng log hàng ngày
- Lưu trữ log cũ sau 30 ngày
- Cảnh báo ngay lập tức với lỗi nghiêm trọng

## Xử Lý Sự Cố

### Vấn Đề Thường Gặp
1. Kết Nối Cơ Sở Dữ Liệu
   - Kiểm tra tồn tại file SQLite
   - Kiểm tra quyền truy cập file
   - Đảm bảo chuỗi kết nối chính xác

2. Hiệu Năng
   - Giám sát kích thước cơ sở dữ liệu
   - Kiểm tra chiến lược đánh index
   - Rà soát mẫu truy vấn

3. Ghi Log
   - Kiểm tra tồn tại thư mục log
   - Kiểm tra quyền ghi
   - Giám sát dung lượng ổ đĩa

### Giải Quyết Lỗi
1. Lỗi Cơ Sở Dữ Liệu
   - Sử dụng công cụ quản lý SQLite để kiểm tra tính toàn vẹn
   - Kiểm tra khóa hoặc vấn đề truy cập đồng thời
   - Xác thực cập nhật schema

2. Lỗi Ứng Dụng
   - Kiểm tra file log để biết chi tiết
   - Xác minh cài đặt cấu hình
   - Đảm bảo có sẵn tất cả dependencies

## Thực Hành Tốt Nhất

### Công Việc Hàng Ngày
1. Theo dõi file log
2. Kiểm tra cảnh báo tồn kho thấp
3. Xem báo cáo lỗi
4. Sao lưu cơ sở dữ liệu

### Công Việc Hàng Tuần
1. Xem xét số liệu hiệu năng
2. Kiểm tra dung lượng ổ đĩa
3. Lưu trữ log cũ
4. Kiểm tra tính toàn vẹn sao lưu

### Bảo Trì Hàng Tháng
1. Dọn dẹp dữ liệu phiên cũ
2. Cập nhật giá nếu cần
3. Rà soát cài đặt bảo mật
4. Kiểm tra tài nguyên hệ thống