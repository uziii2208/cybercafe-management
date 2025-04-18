using Core.Entities;
using Core.Interfaces;
using Serilog;

namespace Presentation.Services;

public class MenuService
{
    private readonly ICustomerService _customerService;
    private readonly IStationService _stationService;
    private readonly ISessionService _sessionService;
    private readonly IServiceManagementService _serviceManagementService;
    private readonly IServiceOrderManagementService _serviceOrderManagementService;

    public MenuService(
        ICustomerService customerService,
        IStationService stationService,
        ISessionService sessionService,
        IServiceManagementService serviceManagementService,
        IServiceOrderManagementService serviceOrderManagementService)
    {
        _customerService = customerService;
        _stationService = stationService;
        _sessionService = sessionService;
        _serviceManagementService = serviceManagementService;
        _serviceOrderManagementService = serviceOrderManagementService;
    }

    public async Task RunAsync()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== He Thong Quan Ly Cybercafe ===");
            Console.WriteLine("1. Quan Ly Khach Hang");
            Console.WriteLine("2. Quan Ly May Tram");
            Console.WriteLine("3. Quan Ly Phien");
            Console.WriteLine("4. Quan Ly Dich Vu");
            Console.WriteLine("5. Xem Bao Cao");
            Console.WriteLine("0. Thoat");
            Console.Write("\nNhap lua chon cua ban: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    switch (choice)
                    {
                        case 1:
                            await ShowCustomerMenuAsync();
                            break;
                        case 2:
                            await ShowStationMenuAsync();
                            break;
                        case 3:
                            await ShowSessionMenuAsync();
                            break;
                        case 4:
                            await ShowServiceMenuAsync();
                            break;
                        case 5:
                            await ShowReportsMenuAsync();
                            break;
                        case 0:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Lua chon khong hop le. Nhan phim bat ky de tiep tuc...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Loi xay ra trong menu chinh");
                    Console.WriteLine($"Loi: {ex.Message}");
                    Console.WriteLine("Nhan phim bat ky de tiep tuc...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task ShowCustomerMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Quan Ly Khach Hang ===");
            Console.WriteLine("1. Them Khach Hang Moi");
            Console.WriteLine("2. Xem Chi Tiet Khach Hang");
            Console.WriteLine("3. Cap Nhat Khach Hang");
            Console.WriteLine("4. Xoa Khach Hang");
            Console.WriteLine("5. Xem Tat Ca Khach Hang");
            Console.WriteLine("0. Quay Lai Menu Chinh");
            Console.Write("\nNhap lua chon cua ban: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    switch (choice)
                    {
                        case 1:
                            await AddNewCustomerAsync();
                            break;
                        case 2:
                            await ViewCustomerDetailsAsync();
                            break;
                        case 3:
                            await UpdateCustomerAsync();
                            break;
                        case 4:
                            await DeleteCustomerAsync();
                            break;
                        case 5:
                            await ViewAllCustomersAsync();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Lua chon khong hop le. Nhan phim bat ky de tiep tuc...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Loi xay ra trong menu khach hang");
                    Console.WriteLine($"Loi: {ex.Message}");
                    Console.WriteLine("Nhan phim bat ky de tiep tuc...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task AddNewCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Them Khach Hang Moi ===");
        
        Console.Write("Nhap Ten: ");
        string name = Console.ReadLine() ?? string.Empty;
        
        Console.Write("Nhap Email: ");
        string email = Console.ReadLine() ?? string.Empty;
        
        Console.Write("Nhap So Dien Thoai: ");
        string phone = Console.ReadLine() ?? string.Empty;

        var customer = new Customer
        {
            Name = name,
            Email = email,
            Phone = phone
        };

        await _customerService.RegisterCustomerAsync(customer);
        Console.WriteLine("\nThem khach hang thanh cong!");
        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewCustomerDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Chi Tiet Khach Hang ===");
        
        Console.Write("Nhap ID Khach Hang: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var customer = await _customerService.GetCustomerAsync(id);
            if (customer != null)
            {
                Console.WriteLine($"\nID: {customer.Id}");
                Console.WriteLine($"Ten: {customer.Name}");
                Console.WriteLine($"Email: {customer.Email}");
                Console.WriteLine($"So Dien Thoai: {customer.Phone}");
                Console.WriteLine($"Hang Thanh Vien: {customer.MembershipTier}");
                Console.WriteLine($"Diem Tich Luy: {customer.MembershipPoints}");
            }
            else
            {
                Console.WriteLine("Khong tim thay khach hang.");
            }
        }
        else
        {
            Console.WriteLine("ID khong hop le.");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task UpdateCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Cap Nhat Khach Hang ===");
        
        Console.Write("Nhap ID Khach Hang: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var customer = await _customerService.GetCustomerAsync(id);
        if (customer == null)
        {
            Console.WriteLine("Khach hang khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nThong Tin Hien Tai:");
        Console.WriteLine($"Ten: {customer.Name}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine($"So Dien Thoai: {customer.Phone}");

        Console.WriteLine("\nNhap thong tin moi (nhan Enter de giu nguyen gia tri hien tai):");
        
        Console.Write("Ten Moi: ");
        string name = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(name))
            customer.Name = name;
        
        Console.Write("Email Moi: ");
        string email = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(email))
            customer.Email = email;
        
        Console.Write("So Dien Thoai Moi: ");
        string phone = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(phone))
            customer.Phone = phone;

        await _customerService.UpdateCustomerAsync(customer);
        Console.WriteLine("\nCap nhat khach hang thanh cong!");
        Console.WriteLine("Nhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task DeleteCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Xoa Khach Hang ===");
        
        Console.Write("Nhap ID Khach Hang: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var customer = await _customerService.GetCustomerAsync(id);
        if (customer == null)
        {
            Console.WriteLine("Khach hang khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nThong Tin Khach Hang:");
        Console.WriteLine($"Ten: {customer.Name}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine($"So Dien Thoai: {customer.Phone}");

        Console.Write("\nBan co chac chan muon xoa khach hang nay? (y/N): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            await _customerService.DeleteCustomerAsync(id);
            Console.WriteLine("Xoa khach hang thanh cong!");
        }
        else
        {
            Console.WriteLine("Huy xoa.");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewAllCustomersAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Danh Sach Khach Hang ===\n");
        
        var customers = await _customerService.GetAllCustomersAsync();
        if (!customers.Any())
        {
            Console.WriteLine("Khong tim thay khach hang nao.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tTen\tEmail\tSo DT\tHang\tDiem");
        Console.WriteLine(new string('-', 70));
        
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.Id}\t{customer.Name}\t{customer.Email}\t{customer.Phone}\t{customer.MembershipTier}\t{customer.MembershipPoints}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ShowStationMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Quan Ly May Tram ===");
            Console.WriteLine("1. Them May Moi");
            Console.WriteLine("2. Xem Chi Tiet May");
            Console.WriteLine("3. Cap Nhat May");
            Console.WriteLine("4. Xoa May");
            Console.WriteLine("5. Xem Tat Ca May");
            Console.WriteLine("6. Thay Doi Trang Thai May");
            Console.WriteLine("0. Quay Lai Menu Chinh");
            Console.Write("\nNhap lua chon cua ban: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    switch (choice)
                    {
                        case 1:
                            await AddNewStationAsync();
                            break;
                        case 2:
                            await ViewStationDetailsAsync();
                            break;
                        case 3:
                            await UpdateStationAsync();
                            break;
                        case 4:
                            await DeleteStationAsync();
                            break;
                        case 5:
                            await ViewAllStationsAsync();
                            break;
                        case 6:
                            await ChangeStationStatusAsync();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Lua chon khong hop le. Nhan phim bat ky de tiep tuc...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Loi xay ra trong menu may tram");
                    Console.WriteLine($"Loi: {ex.Message}");
                    Console.WriteLine("Nhan phim bat ky de tiep tuc...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task AddNewStationAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Them May Moi ===");
        
        Console.Write("Nhap So May: ");
        if (!int.TryParse(Console.ReadLine(), out int stationNumber))
        {
            Console.WriteLine("So may khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.Write("Nhap Cau Hinh Phan Cung: ");
        string specs = Console.ReadLine() ?? string.Empty;

        Console.Write("Nhap Gia/Gio: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal rate))
        {
            Console.WriteLine("Gia khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var station = new Station
        {
            StationNumber = stationNumber,
            HardwareSpecification = specs,
            HourlyRate = rate,
            Status = StationStatus.Free
        };

        await _stationService.AddStationAsync(station);
        Console.WriteLine("\nThem may thanh cong!");
        Console.WriteLine("Nhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewStationDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Xem Chi Tiet May ===");
        
        Console.Write("Nhap ID May: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var station = await _stationService.GetStationAsync(id);
        if (station != null)
        {
            Console.WriteLine($"\nID: {station.Id}");
            Console.WriteLine($"So May: {station.StationNumber}");
            Console.WriteLine($"Cau Hinh: {station.HardwareSpecification}");
            Console.WriteLine($"Trang Thai: {station.Status}");
            Console.WriteLine($"Gia/Gio: ${station.HourlyRate:F2}");

            // Show active session if any
            var activeSession = await _sessionService.GetActiveSessionByStationAsync(id);
            if (activeSession != null)
            {
                Console.WriteLine("\nPhien Hien Tai:");
                Console.WriteLine($"Khach Hang: {activeSession.Customer.Name}");
                Console.WriteLine($"Bat Dau: {activeSession.StartTime}");
                Console.WriteLine($"Thoi Gian: {(DateTime.UtcNow - activeSession.StartTime).TotalHours:F2} gio");
            }
        }
        else
        {
            Console.WriteLine("Khong tim thay may.");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task UpdateStationAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Cap Nhat May ===");
        
        Console.Write("Nhap ID May: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var station = await _stationService.GetStationAsync(id);
        if (station == null)
        {
            Console.WriteLine("May khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nThong Tin Hien Tai:");
        Console.WriteLine($"So May: {station.StationNumber}");
        Console.WriteLine($"Cau Hinh: {station.HardwareSpecification}");
        Console.WriteLine($"Gia/Gio: ${station.HourlyRate:F2}");

        Console.WriteLine("\nNhap thong tin moi (nhan Enter de giu nguyen gia tri hien tai):");
        
        Console.Write("Cau Hinh Moi: ");
        string specs = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(specs))
            station.HardwareSpecification = specs;

        Console.Write("Gia/Gio Moi: ");
        string rateStr = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(rateStr) && decimal.TryParse(rateStr, out decimal rate))
            station.HourlyRate = rate;

        await _stationService.UpdateStationAsync(station);
        Console.WriteLine("\nCap nhat may thanh cong!");
        Console.WriteLine("Nhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task DeleteStationAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Xoa May ===");
        
        Console.Write("Nhap ID May: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var station = await _stationService.GetStationAsync(id);
        if (station == null)
        {
            Console.WriteLine("May khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var activeSession = await _sessionService.GetActiveSessionByStationAsync(id);
        if (activeSession != null)
        {
            Console.WriteLine("Khong the xoa may dang co phien hoat dong.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nThong Tin May:");
        Console.WriteLine($"So May: {station.StationNumber}");
        Console.WriteLine($"Cau Hinh: {station.HardwareSpecification}");
        Console.WriteLine($"Trang Thai: {station.Status}");

        Console.Write("\nBan co chac chan muon xoa may nay? (y/N): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            await _stationService.DeleteStationAsync(id);
            Console.WriteLine("Xoa may thanh cong!");
        }
        else
        {
            Console.WriteLine("Huy xoa.");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewAllStationsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Tat Ca May ===\n");
        
        var stations = await _stationService.GetAllStationsAsync();
        if (!stations.Any())
        {
            Console.WriteLine("Khong tim thay may nao.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tSo May\tTrang Thai\tGia/Gio\tCau Hinh");
        Console.WriteLine(new string('-', 70));
        
        foreach (var station in stations)
        {
            Console.WriteLine($"{station.Id}\t{station.StationNumber}\t{station.Status}\t${station.HourlyRate:F2}\t{station.HardwareSpecification}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ChangeStationStatusAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Thay Doi Trang Thai May ===");
        
        Console.Write("Nhap ID May: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var station = await _stationService.GetStationAsync(id);
        if (station == null)
        {
            Console.WriteLine("May khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nTrang Thai Hien Tai: {station.Status}");
        Console.WriteLine("\nTrang Thai Co San:");
        Console.WriteLine("1. Free");
        Console.WriteLine("2. Occupied");
        Console.WriteLine("3. Maintenance");
        
        Console.Write("\nNhap trang thai moi (1-3): ");
        if (int.TryParse(Console.ReadLine(), out int statusChoice))
        {
            var newStatus = statusChoice switch
            {
                1 => StationStatus.Free,
                2 => StationStatus.Occupied,
                3 => StationStatus.Maintenance,
                _ => station.Status
            };

            if (newStatus != station.Status)
            {
                await _stationService.UpdateStationStatusAsync(id, newStatus);
                Console.WriteLine("Cap nhat trang thai thanh cong!");
            }
            else
            {
                Console.WriteLine("Lua chon trang thai khong hop le.");
            }
        }
        else
        {
            Console.WriteLine("Nhap khong hop le.");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ShowSessionMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Quan Ly Phien ===");
            Console.WriteLine("1. Bat Dau Phien Moi");
            Console.WriteLine("2. Ket Thuc Phien");
            Console.WriteLine("3. Xem Phien Dang Hoat Dong");
            Console.WriteLine("4. Xem Chi Tiet Phien");
            Console.WriteLine("5. Them Dich Vu Vao Phien");
            Console.WriteLine("0. Quay Lai Menu Chinh");
            Console.Write("\nNhap lua chon cua ban: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    switch (choice)
                    {
                        case 1:
                            await StartNewSessionAsync();
                            break;
                        case 2:
                            await EndSessionAsync();
                            break;
                        case 3:
                            await ViewActiveSessionsAsync();
                            break;
                        case 4:
                            await ViewSessionDetailsAsync();
                            break;
                        case 5:
                            await AddServiceToSessionAsync();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Lua chon khong hop le. Nhan phim bat ky de tiep tuc...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Loi xay ra trong menu phien");
                    Console.WriteLine($"Loi: {ex.Message}");
                    Console.WriteLine("Nhan phim bat ky de tiep tuc...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task StartNewSessionAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Bat Dau Phien Moi ===");
        
        Console.Write("Nhap ID Khach Hang: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("ID khach hang khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var customer = await _customerService.GetCustomerAsync(customerId);
        if (customer == null)
        {
            Console.WriteLine("Khach hang khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var activeSession = await _sessionService.GetActiveSessionByCustomerAsync(customerId);
        if (activeSession != null)
        {
            Console.WriteLine("Khach hang da co phien dang hoat dong.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        // Show available stations
        var stations = await _stationService.GetAvailableStationsAsync();
        if (!stations.Any())
        {
            Console.WriteLine("Khong co may nao san sang.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nMay San Sang:");
        Console.WriteLine("ID\tSo May\tGia/Gio\tCau Hinh");
        Console.WriteLine(new string('-', 50));
        foreach (var station in stations)
        {
            Console.WriteLine($"{station.Id}\t{station.StationNumber}\t${station.HourlyRate:F2}\t{station.HardwareSpecification}");
        }

        Console.Write("\nNhap ID May: ");
        if (!int.TryParse(Console.ReadLine(), out int stationId))
        {
            Console.WriteLine("ID may khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        if (!stations.Any(s => s.Id == stationId))
        {
            Console.WriteLine("Lua chon may khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var session = await _sessionService.StartSessionAsync(customerId, stationId);
        Console.WriteLine("\nBat dau phien thanh cong!");
        Console.WriteLine($"ID Phien: {session.Id}");
        Console.WriteLine($"Bat Dau: {session.StartTime}");
        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task EndSessionAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Ket Thuc Phien ===");
        
        Console.Write("Nhap ID Phien: ");
        if (!int.TryParse(Console.ReadLine(), out int sessionId))
        {
            Console.WriteLine("ID phien khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        try
        {
            var session = await _sessionService.EndSessionAsync(sessionId);
            Console.WriteLine("\nKet thuc phien thanh cong!");
            Console.WriteLine($"Thoi Gian: {(session.EndTime!.Value - session.StartTime).TotalHours:F2} gio");
            Console.WriteLine($"Tong Chi Phi: ${session.TotalCost:F2}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loi: {ex.Message}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewActiveSessionsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Phien Dang Hoat Dong ===\n");
        
        var sessions = await _sessionService.GetActiveSessionsAsync();
        if (!sessions.Any())
        {
            Console.WriteLine("Khong co phien dang hoat dong.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tKhach Hang\tMay So\tBat Dau\tThoi Gian (gio)");
        Console.WriteLine(new string('-', 70));
        
        foreach (var session in sessions)
        {
            var duration = (DateTime.UtcNow - session.StartTime).TotalHours;
            Console.WriteLine($"{session.Id}\t{session.Customer.Name}\t{session.Station.StationNumber}\t{session.StartTime:HH:mm:ss}\t{duration:F2}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewSessionDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Chi Tiet Phien ===");
        
        Console.Write("Nhap ID Phien: ");
        if (!int.TryParse(Console.ReadLine(), out int sessionId))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var session = await _sessionService.GetActiveSessionByCustomerAsync(sessionId);
        if (session != null)
        {
            Console.WriteLine($"\nID Phien: {session.Id}");
            Console.WriteLine($"Khach Hang: {session.Customer.Name}");
            Console.WriteLine($"May So: {session.Station.StationNumber}");
            Console.WriteLine($"Bat Dau: {session.StartTime}");
            
            var duration = (DateTime.UtcNow - session.StartTime).TotalHours;
            Console.WriteLine($"Thoi Gian: {duration:F2} gio");
            
            var cost = await _sessionService.CalculateSessionCostAsync(sessionId);
            Console.WriteLine($"Tong Chi Phi: ${cost:F2}");

            // Show ordered services
            var orders = await _serviceOrderManagementService.GetSessionOrdersAsync(sessionId);
            if (orders.Any())
            {
                Console.WriteLine("\nDich Vu Da Dat:");
                Console.WriteLine("Ten\tSo Luong\tGia\tTrang Thai");
                Console.WriteLine(new string('-', 50));
                foreach (var order in orders)
                {
                    Console.WriteLine($"{order.Service.Name}\t{order.Quantity}\t${order.TotalPrice:F2}\t{order.Status}");
                }
            }
        }
        else
        {
            Console.WriteLine("Khong tim thay phien hoac phien da ket thuc.");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task AddServiceToSessionAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Them Dich Vu Vao Phien ===");
        
        Console.Write("Nhap ID Phien: ");
        if (!int.TryParse(Console.ReadLine(), out int sessionId))
        {
            Console.WriteLine("ID phien khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var session = await _sessionService.GetActiveSessionByCustomerAsync(sessionId);
        if (session == null)
        {
            Console.WriteLine("Khong tim thay phien hoac phien da ket thuc.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        // Show available services
        var services = await _serviceManagementService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("Khong co dich vu nao.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nDich Vu Hien Co:");
        Console.WriteLine("ID\tTen\tGia\tTon Kho");
        Console.WriteLine(new string('-', 50));
        foreach (var service in services)
        {
            Console.WriteLine($"{service.Id}\t{service.Name}\t${service.Price:F2}\t{service.CurrentStock}");
        }

        Console.Write("\nNhap ID Dich Vu: ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("ID dich vu khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.Write("Nhap So Luong: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("So luong khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        try
        {
            await _serviceOrderManagementService.CreateOrderAsync(sessionId, serviceId, quantity);
            Console.WriteLine("Them dich vu vao phien thanh cong!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loi: {ex.Message}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ShowServiceMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Quan Ly Dich Vu ===");
            Console.WriteLine("1. Them Dich Vu Moi");
            Console.WriteLine("2. Xem Chi Tiet Dich Vu");
            Console.WriteLine("3. Cap Nhat Dich Vu");
            Console.WriteLine("4. Xoa Dich Vu");
            Console.WriteLine("5. Xem Tat Ca Dich Vu");
            Console.WriteLine("6. Cap Nhat Ton Kho");
            Console.WriteLine("7. Xem Dich Vu Sap Het");
            Console.WriteLine("0. Quay Lai Menu Chinh");
            Console.Write("\nNhap lua chon cua ban: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    switch (choice)
                    {
                        case 1:
                            await AddNewServiceAsync();
                            break;
                        case 2:
                            await ViewServiceDetailsAsync();
                            break;
                        case 3:
                            await UpdateServiceAsync();
                            break;
                        case 4:
                            await DeleteServiceAsync();
                            break;
                        case 5:
                            await ViewAllServicesAsync();
                            break;
                        case 6:
                            await UpdateServiceStockAsync();
                            break;
                        case 7:
                            await ViewLowStockServicesAsync();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Lua chon khong hop le. Nhan phim bat ky de tiep tuc...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Loi xay ra trong menu dich vu");
                    Console.WriteLine($"Loi: {ex.Message}");
                    Console.WriteLine("Nhan phim bat ky de tiep tuc...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task AddNewServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Them Dich Vu Moi ===");
        
        Console.Write("Nhap Ten Dich Vu: ");
        string name = Console.ReadLine() ?? string.Empty;

        Console.Write("Nhap Mo Ta: ");
        string description = Console.ReadLine() ?? string.Empty;

        Console.Write("Nhap Gia (USD): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Gia khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.Write("Nhap So Luong Ban Dau: ");
        if (!int.TryParse(Console.ReadLine(), out int stock))
        {
            Console.WriteLine("So luong khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.Write("Nhap So Luong Toi Thieu: ");
        if (!int.TryParse(Console.ReadLine(), out int minStock))
        {
            Console.WriteLine("So luong toi thieu khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nChon Loai Dich Vu:");
        Console.WriteLine("1. Thuc An");
        Console.WriteLine("2. Do Uong");
        Console.WriteLine("3. Thiet Bi");
        Console.WriteLine("4. Khac");
        Console.Write("Nhap lua chon (1-4): ");

        ServiceCategory category = ServiceCategory.Other;
        if (int.TryParse(Console.ReadLine(), out int categoryChoice))
        {
            category = categoryChoice switch
            {
                1 => ServiceCategory.Food,
                2 => ServiceCategory.Beverage,
                3 => ServiceCategory.Peripheral,
                _ => ServiceCategory.Other
            };
        }

        var service = new Service
        {
            Name = name,
            Description = description,
            Price = price,
            CurrentStock = stock,
            MinimumStock = minStock,
            Category = category
        };

        await _serviceManagementService.AddServiceAsync(service);
        Console.WriteLine("\nThem dich vu thanh cong!");
        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewServiceDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Chi Tiet Dich Vu ===");
        
        Console.Write("Nhap ID Dich Vu: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var service = await _serviceManagementService.GetServiceAsync(id);
        if (service != null)
        {
            Console.WriteLine($"\nID: {service.Id}");
            Console.WriteLine($"Ten: {service.Name}");
            Console.WriteLine($"Mo Ta: {service.Description}");
            Console.WriteLine($"Loai: {service.Category}");
            Console.WriteLine($"Gia: ${service.Price:F2}");
            Console.WriteLine($"Ton Kho: {service.CurrentStock}");
            Console.WriteLine($"Ton Kho Toi Thieu: {service.MinimumStock}");
        }
        else
        {
            Console.WriteLine("Khong tim thay dich vu.");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task UpdateServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Cap Nhat Dich Vu ===");
        
        Console.Write("Nhap ID Dich Vu: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var service = await _serviceManagementService.GetServiceAsync(id);
        if (service == null)
        {
            Console.WriteLine("Dich vu khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nThong Tin Hien Tai:");
        Console.WriteLine($"Ten: {service.Name}");
        Console.WriteLine($"Mo Ta: {service.Description}");
        Console.WriteLine($"Gia: ${service.Price:F2}");
        Console.WriteLine($"Ton Kho Toi Thieu: {service.MinimumStock}");

        Console.WriteLine("\nNhap thong tin moi (nhan Enter de giu nguyen gia tri hien tai):");
        
        Console.Write("Ten Moi: ");
        string name = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(name))
            service.Name = name;

        Console.Write("Mo Ta Moi: ");
        string description = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(description))
            service.Description = description;

        Console.Write("Gia Moi: ");
        string priceStr = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(priceStr) && decimal.TryParse(priceStr, out decimal price))
            service.Price = price;

        Console.Write("Ton Kho Toi Thieu Moi: ");
        string minStockStr = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(minStockStr) && int.TryParse(minStockStr, out int minStock))
            service.MinimumStock = minStock;

        await _serviceManagementService.UpdateServiceAsync(service);
        Console.WriteLine("\nCap nhat dich vu thanh cong!");
        Console.WriteLine("Nhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task DeleteServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Xoa Dich Vu ===");
        
        Console.Write("Nhap ID Dich Vu: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var service = await _serviceManagementService.GetServiceAsync(id);
        if (service == null)
        {
            Console.WriteLine("Dich vu khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nThong Tin Dich Vu:");
        Console.WriteLine($"Ten: {service.Name}");
        Console.WriteLine($"Mo Ta: {service.Description}");
        Console.WriteLine($"Gia: ${service.Price:F2}");
        Console.WriteLine($"Ton Kho: {service.CurrentStock}");

        Console.Write("\nBan co chac chan muon xoa dich vu nay? (y/N): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            await _serviceManagementService.DeleteServiceAsync(id);
            Console.WriteLine("Xoa dich vu thanh cong!");
        }
        else
        {
            Console.WriteLine("Huy xoa.");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewAllServicesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Tat Ca Dich Vu ===\n");
        
        var services = await _serviceManagementService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("Khong tim thay dich vu nao.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");;
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tTen\tLoai\tGia (USD)\tTon Kho\tTon Toi Thieu");
        Console.WriteLine(new string('-', 90));
        
        foreach (var service in services)
        {
            Console.WriteLine($"{service.Id}\t{service.Name}\t{service.Category}\t${service.Price:F2}\t{service.CurrentStock}\t{service.MinimumStock}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task UpdateServiceStockAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Cap Nhat Ton Kho Dich Vu ===");
        
        Console.Write("Nhap ID Dich Vu: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var service = await _serviceManagementService.GetServiceAsync(id);
        if (service == null)
        {
            Console.WriteLine("Dich vu khong ton tai.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nTon Kho Hien Tai: {service.CurrentStock}");
        Console.Write("Nhap thay doi ton kho (+/- so luong): ");
        if (!int.TryParse(Console.ReadLine(), out int change))
        {
            Console.WriteLine("So luong khong hop le.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        try
        {
            await _serviceManagementService.UpdateStockAsync(id, change);
            Console.WriteLine("Cap nhat ton kho thanh cong!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Loi: {ex.Message}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ViewLowStockServicesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Dich Vu Sap Het ===\n");
        
        var services = await _serviceManagementService.GetLowStockServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("Khong co dich vu nao sap het.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tTen\tLoai\tTon Kho\tTon Toi Thieu");
        Console.WriteLine(new string('-', 70));
        
        foreach (var service in services)
        {
            Console.WriteLine($"{service.Id}\t{service.Name}\t{service.Category}\t{service.CurrentStock}\t{service.MinimumStock}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ShowReportsMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Bao Cao ===");
            Console.WriteLine("1. Bao Cao Phien Trong Ngay");
            Console.WriteLine("2. Bao Cao Doanh Thu Ngay");
            Console.WriteLine("3. Bao Cao Su Dung May");
            Console.WriteLine("4. Bao Cao Dich Vu Pho Bien");
            Console.WriteLine("5. Bao Cao Hoat Dong Khach Hang");
            Console.WriteLine("0. Quay Lai Menu Chinh");
            Console.Write("\nNhap lua chon cua ban: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                try
                {
                    switch (choice)
                    {
                        case 1:
                            await ShowDailySessionReportAsync();
                            break;
                        case 2:
                            await ShowDailyRevenueReportAsync();
                            break;
                        case 3:
                            await ShowStationUsageReportAsync();
                            break;
                        case 4:
                            await ShowPopularServicesReportAsync();
                            break;
                        case 5:
                            await ShowCustomerActivityReportAsync();
                            break;
                        case 0:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Lua chon khong hop le. Nhan phim bat ky de tiep tuc...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Loi xay ra trong menu bao cao");
                    Console.WriteLine($"Loi: {ex.Message}");
                    Console.WriteLine("Nhan phim bat ky de tiep tuc...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task ShowDailySessionReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Bao Cao Phien Trong Ngay ===\n");

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var sessions = await _sessionService.GetSessionsByDateRangeAsync(today, tomorrow);

        if (!sessions.Any())
        {
            Console.WriteLine("Khong co phien nao trong ngay hom nay.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        var totalSessions = sessions.Count();
        var completedSessions = sessions.Count(s => s.EndTime.HasValue);
        var activeSessions = sessions.Count(s => !s.EndTime.HasValue);
        var totalRevenue = sessions.Where(s => s.EndTime.HasValue).Sum(s => s.TotalCost);
        var avgDuration = sessions.Where(s => s.EndTime.HasValue)
            .Average(s => (s.EndTime!.Value - s.StartTime).TotalHours);

        Console.WriteLine($"Ngay: {today:d}");
        Console.WriteLine($"Tong So Phien: {totalSessions}");
        Console.WriteLine($"Phien Da Ket Thuc: {completedSessions}");
        Console.WriteLine($"Phien Dang Hoat Dong: {activeSessions}");
        Console.WriteLine($"Tong Doanh Thu: ${totalRevenue:F2}");
        Console.WriteLine($"Thoi Gian Trung Binh: {avgDuration:F2} gio");

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ShowDailyRevenueReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Bao Cao Doanh Thu Ngay ===\n");

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var sessions = await _sessionService.GetSessionsByDateRangeAsync(today, tomorrow);

        if (!sessions.Any())
        {
            Console.WriteLine("Khong co doanh thu trong ngay hom nay.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        decimal sessionRevenue = sessions.Where(s => s.EndTime.HasValue).Sum(s => s.TotalCost);
        
        var orders = sessions.SelectMany(s => s.ServiceOrders)
            .Where(o => o.Status == OrderStatus.Completed);
        
        var serviceRevenue = orders.Sum(o => o.TotalPrice);
        var totalRevenue = sessionRevenue + serviceRevenue;

        Console.WriteLine($"Ngay: {today:d}");
        Console.WriteLine($"Doanh Thu Phien Choi Game: ${sessionRevenue:F2}");
        Console.WriteLine($"Doanh Thu Dich Vu: ${serviceRevenue:F2}");
        Console.WriteLine($"Tong Doanh Thu: ${totalRevenue:F2}");

        Console.WriteLine("\nDoanh Thu Theo Loai Dich Vu:");
        var categoryRevenue = orders
            .GroupBy(o => o.Service.Category)
            .Select(g => new { Category = g.Key, Revenue = g.Sum(o => o.TotalPrice) });

        foreach (var category in categoryRevenue)
        {
            Console.WriteLine($"{category.Category}: ${category.Revenue:F2}");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ShowStationUsageReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Bao Cao Su Dung May ===\n");

        var stations = await _stationService.GetAllStationsAsync();
        if (!stations.Any())
        {
            Console.WriteLine("Khong tim thay may nao.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Tong Ket Su Dung May:");
        Console.WriteLine("So May\tTrang Thai\tTong Phien\tTong Gio\tDoanh Thu");
        Console.WriteLine(new string('-', 70));

        foreach (var station in stations)
        {
            var sessions = station.Sessions.Where(s => s.EndTime.HasValue);
            var totalHours = sessions.Sum(s => (s.EndTime!.Value - s.StartTime).TotalHours);
            var revenue = sessions.Sum(s => s.TotalCost);
            var sessionCount = sessions.Count();

            Console.WriteLine($"{station.StationNumber}\t{station.Status}\t{sessionCount}\t{totalHours:F2}\t${revenue:F2}");
        }

        // Usage statistics
        var totalStations = stations.Count();
        var availableStations = stations.Count(s => s.Status == StationStatus.Free);
        var occupiedStations = stations.Count(s => s.Status == StationStatus.Occupied);
        var maintenanceStations = stations.Count(s => s.Status == StationStatus.Maintenance);

        Console.WriteLine("\nTong Ket Trang Thai Hien Tai:");
        Console.WriteLine($"Tong So May: {totalStations}");
        Console.WriteLine($"San Sang: {availableStations}");
        Console.WriteLine($"Dang Su Dung: {occupiedStations}");
        Console.WriteLine($"Bao Tri: {maintenanceStations}");

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ShowPopularServicesReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Bao Cao Dich Vu Pho Bien ===\n");

        var services = await _serviceManagementService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("Khong tim thay dich vu nao.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Dich Vu Pho Bien Nhat:");
        Console.WriteLine("Ten\tLoai\tSo Don\tSo Luong Da Ban\tDoanh Thu (USD)");
        Console.WriteLine(new string('-', 90));

        foreach (var service in services)
        {
            var completedOrders = service.Orders.Where(o => o.Status == OrderStatus.Completed);
            var orderCount = completedOrders.Count();
            var quantitySold = completedOrders.Sum(o => o.Quantity);
            var revenue = completedOrders.Sum(o => o.TotalPrice);

            Console.WriteLine($"{service.Name}\t{service.Category}\t{orderCount}\t{quantitySold}\t${revenue:F2}");
        }

        // Stock alerts
        var lowStockServices = await _serviceManagementService.GetLowStockServicesAsync();
        if (lowStockServices.Any())
        {
            Console.WriteLine("\nCanh Bao Ton Kho Thap:");
            foreach (var service in lowStockServices)
            {
                Console.WriteLine($"{service.Name}: con {service.CurrentStock} (Toi thieu: {service.MinimumStock})");
            }
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }

    private async Task ShowCustomerActivityReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Bao Cao Hoat Dong Khach Hang ===\n");

        var customers = await _customerService.GetAllCustomersAsync();
        if (!customers.Any())
        {
            Console.WriteLine("Khong tim thay khach hang nao.");
            Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Tong Ket Hoat Dong Khach Hang:");
        Console.WriteLine("Ten\tHang\tTong Phien\tTong Gio\tTong Chi (USD)");
        Console.WriteLine(new string('-', 100));

        foreach (var customer in customers)
        {
            var completedSessions = customer.Sessions.Where(s => s.EndTime.HasValue);
            var sessionCount = completedSessions.Count();
            var totalHours = completedSessions.Sum(s => (s.EndTime!.Value - s.StartTime).TotalHours);
            var totalSpent = completedSessions.Sum(s => s.TotalCost);

            Console.WriteLine($"{customer.Name}\t{customer.MembershipTier}\t{sessionCount}\t{totalHours:F2}\t${totalSpent:F2}");
        }

        // Membership statistics
        var membershipStats = customers.GroupBy(c => c.MembershipTier)
            .Select(g => new { Tier = g.Key, Count = g.Count() })
            .OrderBy(x => x.Tier);

        Console.WriteLine("\nPhan Bo Hang Thanh Vien:");
        foreach (var stat in membershipStats)
        {
            Console.WriteLine($"{stat.Tier}: {stat.Count} khach hang");
        }

        Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
        Console.ReadKey();
    }
}