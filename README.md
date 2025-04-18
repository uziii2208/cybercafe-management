# Hệ Thống Quản Lý Tiệm Net (Cybercafe)

Một ứng dụng console mạnh mẽ để quản lý hoạt động tiệm net, được xây dựng bằng .NET 8.0 và tuân theo nguyên tắc kiến trúc sạch.

## Tính Năng

### 1. Quản Lý Khách Hàng
- Thêm, cập nhật và xóa thông tin khách hàng
- Theo dõi hạng thành viên (Thường, Bạc, Vàng)
- Giám sát hoạt động và điểm tích lũy của khách hàng
- Xem lịch sử phiên chơi game

### 2. Quản Lý Máy Trạm
- Quản lý danh sách máy chơi game
- Theo dõi trạng thái máy (Trống, Đang sử dụng, Bảo trì)
- Cấu hình thông số máy và giá theo giờ
- Theo dõi tình hình sử dụng và doanh thu

### 3. Quản Lý Phiên
- Bắt đầu và kết thúc phiên chơi game
- Theo dõi phiên theo thời gian thực
- Tự động tính toán chi phí
- Thêm dịch vụ vào phiên đang hoạt động

### 4. Quản Lý Dịch Vụ
- Quản lý đồ ăn, đồ uống và thiết bị ngoại vi
- Theo dõi mức tồn kho
- Thiết lập giá và mức tồn kho tối thiểu
- Giám sát doanh số và các mặt hàng phổ biến

### 5. Báo Cáo Kinh Doanh
- Báo cáo phiên trong ngày
- Phân tích doanh thu
- Thống kê sử dụng máy
- Theo dõi hoạt động khách hàng
- Giám sát mức tồn kho

## Yêu Cầu Hệ Thống

- .NET 8.0 SDK
- SQLite
- Terminal hoặc Command Prompt

## Cài Đặt

1. Clone repository:
```bash
git clone https://github.com/uziii2208/cybercafe-management.git
cd CybercafeApp
```

2. Khôi phục gói NuGet:
```bash
dotnet restore
```

3. Build giải pháp:
```bash
dotnet build
```

4. Chạy ứng dụng:
```bash
cd src/Presentation
dotnet run
```

## Cấu Trúc Dự Án

```
CybercafeApp/
├── src/
│   ├── Core/                 # Logic nghiệp vụ và entities
│   ├── Data/                 # Truy cập và repositories dữ liệu
│   └── Presentation/         # Giao diện console và tương tác người dùng
├── tests/
│   ├── UnitTests/           # Unit tests
│   └── IntegrationTests/    # Integration tests
├── docs/                     # Tài liệu
└── lib/                     # Thư viện bên ngoài
```

### Lớp Core
- Chứa entities và interfaces nghiệp vụ
- Định nghĩa logic nghiệp vụ thông qua các services
- Quy tắc nghiệp vụ độc lập với triển khai

### Lớp Data
- Triển khai truy cập dữ liệu sử dụng Entity Framework Core
- Triển khai mẫu Repository
- Tích hợp cơ sở dữ liệu SQLite
- Chức năng khởi tạo dữ liệu mẫu

### Lớp Presentation
- Giao diện người dùng dạng console
- Tương tác qua menu
- Kiểm tra đầu vào và xử lý lỗi
- Tạo báo cáo

## Cấu Trúc Cơ Sở Dữ Liệu

Ứng dụng sử dụng SQLite với các bảng chính sau:

1. Khách Hàng (Customers)
   - Id (Khóa chính)
   - Tên
   - Email
   - Số điện thoại
   - Hạng thành viên
   - Điểm tích lũy
   - Ngày tạo

2. Máy Trạm (Stations)
   - Id (Khóa chính)
   - Số máy
   - Thông số phần cứng
   - Trạng thái
   - Giá theo giờ
   - Ngày tạo

3. Phiên (Sessions)
   - Id (Khóa chính)
   - Id khách hàng (Khóa ngoại)
   - Id máy (Khóa ngoại)
   - Thời gian bắt đầu
   - Thời gian kết thúc
   - Tổng chi phí
   - Ngày tạo

4. Dịch Vụ (Services)
   - Id (Khóa chính)
   - Tên
   - Mô tả
   - Loại
   - Giá
   - Tồn kho hiện tại
   - Tồn kho tối thiểu
   - Ngày tạo

5. Đơn Hàng Dịch Vụ (ServiceOrders)
   - Id (Khóa chính)
   - Id phiên (Khóa ngoại)
   - Id dịch vụ (Khóa ngoại)
   - Số lượng
   - Đơn giá
   - Tổng tiền
   - Trạng thái
   - Ngày tạo

## Hướng Dẫn Sử Dụng

### Menu Chính
Ứng dụng hiển thị menu chính với các lựa chọn sau:
1. Quản lý khách hàng
2. Quản lý máy trạm
3. Quản lý phiên
4. Quản lý dịch vụ
5. Xem báo cáo
0. Thoát

### Các Thao Tác Thường Dùng

#### Quản Lý Khách Hàng
1. Chọn "1" từ menu chính
2. Chọn từ các tùy chọn:
   - Thêm khách hàng mới
   - Xem chi tiết khách hàng
   - Cập nhật khách hàng
   - Xóa khách hàng
   - Xem tất cả khách hàng

#### Bắt Đầu Phiên
1. Chọn "3" từ menu chính
2. Chọn "Bắt đầu phiên mới"
3. Nhập ID khách hàng
4. Chọn máy trống
5. Phiên tự động bắt đầu

#### Thêm Dịch Vụ Vào Phiên
1. Chọn "3" từ menu chính
2. Chọn "Thêm dịch vụ vào phiên"
3. Nhập ID phiên
4. Chọn dịch vụ và số lượng
5. Xác nhận đơn hàng

#### Kết Thúc Phiên
1. Chọn "3" từ menu chính
2. Chọn "Kết thúc phiên"
3. Nhập ID phiên
4. Xem lại và xác nhận chi phí

#### Xem Báo Cáo
1. Chọn "5" từ menu chính
2. Chọn loại báo cáo:
   - Báo cáo phiên trong ngày
   - Báo cáo doanh thu ngày
   - Báo cáo sử dụng máy
   - Báo cáo dịch vụ phổ biến
   - Báo cáo hoạt động khách hàng

## Kiểm Thử

### Chạy Unit Tests
```bash
cd tests/UnitTests
dotnet test
```

### Chạy Integration Tests
```bash
cd tests/IntegrationTests
dotnet test
```

### Phạm Vi Kiểm Thử
Bộ kiểm thử bao gồm:
- Kiểm thử lớp service
- Kiểm thử repository
- Kiểm thử xác thực entity
- Kiểm thử logic nghiệp vụ

## Ghi Log

Ứng dụng sử dụng Serilog để ghi log:
- Vị trí: `logs/cybercafe.log`
- Ghi log tất cả hoạt động và lỗi
- Xoay vòng log hàng ngày
- Hiển thị lỗi nghiêm trọng trên console

## Xử Lý Lỗi

Ứng dụng triển khai xử lý lỗi toàn diện:
- Kiểm tra đầu vào
- Lỗi thao tác cơ sở dữ liệu
- Vi phạm quy tắc nghiệp vụ
- Thông báo lỗi rõ ràng cho người dùng
- Ghi log chi tiết để gỡ lỗi

## Tính Năng Bảo Mật

- Làm sạch đầu vào
- Xác thực dữ liệu
- Thao tác cơ sở dữ liệu an toàn
- Làm sạch thông báo lỗi

## Cân Nhắc Hiệu Năng

- Tối ưu hóa truy vấn cơ sở dữ liệu
- Tải dữ liệu hiệu quả
- Quản lý kết nối hợp lý
- Tối ưu hóa sử dụng bộ nhớ

## Đóng Góp

1. Fork repository
2. Tạo nhánh tính năng
3. Commit thay đổi
4. Push lên nhánh
5. Tạo Pull Request

## Giấy Phép

Dự án này là mã nguồn mở, theo dõi thêm tại [Website Của Tôi](https://uziw3b.site) nhé!

## Hỗ Trợ

Để được hỗ trợ hoặc có thắc mắc, vui lòng [Liên Hệ Tôi](https://t.me/normalz101).