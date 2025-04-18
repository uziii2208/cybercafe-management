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
            Console.WriteLine("=== Hệ Thống Quản Lý Cybercafe ===");
            Console.WriteLine("1. Quản Lý Khách Hàng");
            Console.WriteLine("2. Quản Lý Máy Trạm");
            Console.WriteLine("3. Quản Lý Phiên");
            Console.WriteLine("4. Quản Lý Dịch Vụ");
            Console.WriteLine("5. Xem Báo Cáo");
            Console.WriteLine("0. Thoát");
            Console.Write("\nNhập lựa chọn của bạn: ");

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
                            Console.WriteLine("Lựa chọn không hợp lệ. Nhấn phím bất kỳ để tiếp tục...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Lỗi xảy ra trong menu chính");
                    Console.WriteLine($"Lỗi: {ex.Message}");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục...");
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
            Console.WriteLine("=== Quản Lý Khách Hàng ===");
            Console.WriteLine("1. Thêm Khách Hàng Mới");
            Console.WriteLine("2. Xem Chi Tiết Khách Hàng");
            Console.WriteLine("3. Cập Nhật Khách Hàng");
            Console.WriteLine("4. Xóa Khách Hàng");
            Console.WriteLine("5. Xem Tất Cả Khách Hàng");
            Console.WriteLine("0. Quay Lại Menu Chính");
            Console.Write("\nNhập lựa chọn của bạn: ");

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
                            Console.WriteLine("Lựa chọn không hợp lệ. Nhấn phím bất kỳ để tiếp tục...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Lỗi xảy ra trong menu khách hàng");
                    Console.WriteLine($"Lỗi: {ex.Message}");
                    Console.WriteLine("Nhấn phím bất kỳ để tiếp tục...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task AddNewCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Thêm Khách Hàng Mới ===");
        
        Console.Write("Nhập Tên: ");
        string name = Console.ReadLine() ?? string.Empty;
        
        Console.Write("Nhập Email: ");
        string email = Console.ReadLine() ?? string.Empty;
        
        Console.Write("Nhập Số Điện Thoại: ");
        string phone = Console.ReadLine() ?? string.Empty;

        var customer = new Customer
        {
            Name = name,
            Email = email,
            Phone = phone
        };

        await _customerService.RegisterCustomerAsync(customer);
        Console.WriteLine("\nThêm khách hàng thành công!");
        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ViewCustomerDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Chi Tiết Khách Hàng ===");
        
        Console.Write("Nhập ID Khách Hàng: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var customer = await _customerService.GetCustomerAsync(id);
            if (customer != null)
            {
                Console.WriteLine($"\nID: {customer.Id}");
                Console.WriteLine($"Tên: {customer.Name}");
                Console.WriteLine($"Email: {customer.Email}");
                Console.WriteLine($"Số Điện Thoại: {customer.Phone}");
                Console.WriteLine($"Hạng Thành Viên: {customer.MembershipTier}");
                Console.WriteLine($"Điểm Tích Lũy: {customer.MembershipPoints}");
            }
            else
            {
                Console.WriteLine("Không tìm thấy khách hàng.");
            }
        }
        else
        {
            Console.WriteLine("ID không hợp lệ.");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task UpdateCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Customer ===");
        
        Console.Write("Enter Customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var customer = await _customerService.GetCustomerAsync(id);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nCurrent Details:");
        Console.WriteLine($"Name: {customer.Name}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine($"Phone: {customer.Phone}");

        Console.WriteLine("\nEnter new details (press Enter to keep current value):");
        
        Console.Write("New Name: ");
        string name = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(name))
            customer.Name = name;
        
        Console.Write("New Email: ");
        string email = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(email))
            customer.Email = email;
        
        Console.Write("New Phone: ");
        string phone = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(phone))
            customer.Phone = phone;

        await _customerService.UpdateCustomerAsync(customer);
        Console.WriteLine("\nCustomer updated successfully!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private async Task DeleteCustomerAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Customer ===");
        
        Console.Write("Enter Customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var customer = await _customerService.GetCustomerAsync(id);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nCustomer Details:");
        Console.WriteLine($"Name: {customer.Name}");
        Console.WriteLine($"Email: {customer.Email}");
        Console.WriteLine($"Phone: {customer.Phone}");

        Console.Write("\nAre you sure you want to delete this customer? (y/N): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            await _customerService.DeleteCustomerAsync(id);
            Console.WriteLine("Customer deleted successfully!");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ViewAllCustomersAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Danh Sách Khách Hàng ===\n");
        
        var customers = await _customerService.GetAllCustomersAsync();
        if (!customers.Any())
        {
            Console.WriteLine("Không tìm thấy khách hàng nào.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tTên\tEmail\tSố ĐT\tHạng\tĐiểm");
        Console.WriteLine(new string('-', 70));
        
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.Id}\t{customer.Name}\t{customer.Email}\t{customer.Phone}\t{customer.MembershipTier}\t{customer.MembershipPoints}");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ShowStationMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Quản Lý Máy Trạm ===");
            Console.WriteLine("1. Thêm Máy Mới");
            Console.WriteLine("2. Xem Chi Tiết Máy");
            Console.WriteLine("3. Cập Nhật Máy");
            Console.WriteLine("4. Xóa Máy");
            Console.WriteLine("5. Xem Tất Cả Máy");
            Console.WriteLine("6. Thay Đổi Trạng Thái Máy");
            Console.WriteLine("0. Quay Lại Menu Chính");
            Console.Write("\nNhập lựa chọn của bạn: ");

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
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occurred in station menu");
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task AddNewStationAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Station ===");
        
        Console.Write("Enter Station Number: ");
        if (!int.TryParse(Console.ReadLine(), out int stationNumber))
        {
            Console.WriteLine("Invalid station number format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter Hardware Specifications: ");
        string specs = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter Hourly Rate: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal rate))
        {
            Console.WriteLine("Invalid rate format.");
            Console.WriteLine("\nPress any key to continue...");
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
        Console.WriteLine("\nStation added successfully!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private async Task ViewStationDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Xem Chi Tiết Máy ===");
        
        Console.Write("Enter Station ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        var station = await _stationService.GetStationAsync(id);
        if (station != null)
        {
            Console.WriteLine($"\nID: {station.Id}");
            Console.WriteLine($"Số Máy: {station.StationNumber}");
            Console.WriteLine($"Cấu Hình: {station.HardwareSpecification}");
            Console.WriteLine($"Trạng Thái: {station.Status}");
            Console.WriteLine($"Giá/Giờ: ${station.HourlyRate:F2}");

            // Show active session if any
            var activeSession = await _sessionService.GetActiveSessionByStationAsync(id);
            if (activeSession != null)
            {
                Console.WriteLine("\nPhiên Hiện Tại:");
                Console.WriteLine($"Khách Hàng: {activeSession.Customer.Name}");
                Console.WriteLine($"Bắt Đầu: {activeSession.StartTime}");
                Console.WriteLine($"Thời Gian: {(DateTime.UtcNow - activeSession.StartTime).TotalHours:F2} giờ");
            }
        }
        else
        {
            Console.WriteLine("Không tìm thấy máy.");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task UpdateStationAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Station ===");
        
        Console.Write("Enter Station ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var station = await _stationService.GetStationAsync(id);
        if (station == null)
        {
            Console.WriteLine("Station not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nCurrent Details:");
        Console.WriteLine($"Station Number: {station.StationNumber}");
        Console.WriteLine($"Hardware Specs: {station.HardwareSpecification}");
        Console.WriteLine($"Hourly Rate: ${station.HourlyRate:F2}");

        Console.WriteLine("\nEnter new details (press Enter to keep current value):");
        
        Console.Write("New Hardware Specifications: ");
        string specs = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(specs))
            station.HardwareSpecification = specs;

        Console.Write("New Hourly Rate: ");
        string rateStr = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(rateStr) && decimal.TryParse(rateStr, out decimal rate))
            station.HourlyRate = rate;

        await _stationService.UpdateStationAsync(station);
        Console.WriteLine("\nStation updated successfully!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private async Task DeleteStationAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Station ===");
        
        Console.Write("Enter Station ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var station = await _stationService.GetStationAsync(id);
        if (station == null)
        {
            Console.WriteLine("Station not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var activeSession = await _sessionService.GetActiveSessionByStationAsync(id);
        if (activeSession != null)
        {
            Console.WriteLine("Cannot delete station with active session.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nStation Details:");
        Console.WriteLine($"Station Number: {station.StationNumber}");
        Console.WriteLine($"Hardware Specs: {station.HardwareSpecification}");
        Console.WriteLine($"Status: {station.Status}");

        Console.Write("\nAre you sure you want to delete this station? (y/N): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            await _stationService.DeleteStationAsync(id);
            Console.WriteLine("Station deleted successfully!");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ViewAllStationsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Stations ===\n");
        
        var stations = await _stationService.GetAllStationsAsync();
        if (!stations.Any())
        {
            Console.WriteLine("No stations found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tNumber\tStatus\tRate\tSpecs");
        Console.WriteLine(new string('-', 70));
        
        foreach (var station in stations)
        {
            Console.WriteLine($"{station.Id}\t{station.StationNumber}\t{station.Status}\t${station.HourlyRate:F2}\t{station.HardwareSpecification}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ChangeStationStatusAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Change Station Status ===");
        
        Console.Write("Enter Station ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var station = await _stationService.GetStationAsync(id);
        if (station == null)
        {
            Console.WriteLine("Station not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nCurrent Status: {station.Status}");
        Console.WriteLine("\nAvailable Statuses:");
        Console.WriteLine("1. Free");
        Console.WriteLine("2. Occupied");
        Console.WriteLine("3. Maintenance");
        
        Console.Write("\nEnter new status (1-3): ");
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
                Console.WriteLine("Status updated successfully!");
            }
            else
            {
                Console.WriteLine("Invalid status choice.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ShowSessionMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Quản Lý Phiên ===");
            Console.WriteLine("1. Bắt Đầu Phiên Mới");
            Console.WriteLine("2. Kết Thúc Phiên");
            Console.WriteLine("3. Xem Phiên Đang Hoạt Động");
            Console.WriteLine("4. Xem Chi Tiết Phiên");
            Console.WriteLine("5. Thêm Dịch Vụ Vào Phiên");
            Console.WriteLine("0. Quay Lại Menu Chính");
            Console.Write("\nNhập lựa chọn của bạn: ");

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
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occurred in session menu");
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task StartNewSessionAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Start New Session ===");
        
        Console.Write("Enter Customer ID: ");
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Invalid customer ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var customer = await _customerService.GetCustomerAsync(customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var activeSession = await _sessionService.GetActiveSessionByCustomerAsync(customerId);
        if (activeSession != null)
        {
            Console.WriteLine("Customer already has an active session.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        // Show available stations
        var stations = await _stationService.GetAvailableStationsAsync();
        if (!stations.Any())
        {
            Console.WriteLine("No stations available.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nAvailable Stations:");
        Console.WriteLine("ID\tNumber\tRate\tSpecs");
        Console.WriteLine(new string('-', 50));
        foreach (var station in stations)
        {
            Console.WriteLine($"{station.Id}\t{station.StationNumber}\t${station.HourlyRate:F2}\t{station.HardwareSpecification}");
        }

        Console.Write("\nEnter Station ID: ");
        if (!int.TryParse(Console.ReadLine(), out int stationId))
        {
            Console.WriteLine("Invalid station ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        if (!stations.Any(s => s.Id == stationId))
        {
            Console.WriteLine("Invalid station selection.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var session = await _sessionService.StartSessionAsync(customerId, stationId);
        Console.WriteLine("\nSession started successfully!");
        Console.WriteLine($"Session ID: {session.Id}");
        Console.WriteLine($"Start Time: {session.StartTime}");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task EndSessionAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Kết Thúc Phiên ===");
        
        Console.Write("Enter Session ID: ");
        if (!int.TryParse(Console.ReadLine(), out int sessionId))
        {
            Console.WriteLine("ID phiên không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        try
        {
            var session = await _sessionService.EndSessionAsync(sessionId);
            Console.WriteLine("\nKết thúc phiên thành công!");
            Console.WriteLine($"Thời Gian: {(session.EndTime!.Value - session.StartTime).TotalHours:F2} hours");
            Console.WriteLine($"Tổng Chi Phí: ${session.TotalCost:F2}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ViewActiveSessionsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Phiên Đang Hoạt Động ===\n");
        
        var sessions = await _sessionService.GetActiveSessionsAsync();
        if (!sessions.Any())
        {
            Console.WriteLine("Không có phiên đang hoạt động.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tKhách Hàng\tMáy Số\tBắt Đầu\tThời Gian (giờ)");
        Console.WriteLine(new string('-', 70));
        
        foreach (var session in sessions)
        {
            var duration = (DateTime.UtcNow - session.StartTime).TotalHours;
            Console.WriteLine($"{session.Id}\t{session.Customer.Name}\t{session.Station.StationNumber}\t{session.StartTime:HH:mm:ss}\t{duration:F2}");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ViewSessionDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Chi Tiết Phiên ===");
        
        Console.Write("Nhập ID Phiên: ");
        if (!int.TryParse(Console.ReadLine(), out int sessionId))
        {
            Console.WriteLine("ID không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        var session = await _sessionService.GetActiveSessionByCustomerAsync(sessionId);
        if (session != null)
        {
            Console.WriteLine($"\nID Phiên: {session.Id}");
            Console.WriteLine($"Khách Hàng: {session.Customer.Name}");
            Console.WriteLine($"Máy Số: {session.Station.StationNumber}");
            Console.WriteLine($"Bắt Đầu: {session.StartTime}");
            
            var duration = (DateTime.UtcNow - session.StartTime).TotalHours;
            Console.WriteLine($"Thời Gian: {duration:F2} giờ");
            
            var cost = await _sessionService.CalculateSessionCostAsync(sessionId);
            Console.WriteLine($"Tổng Chi Phí: ${cost:F2}");

            // Show ordered services
            var orders = await _serviceOrderManagementService.GetSessionOrdersAsync(sessionId);
            if (orders.Any())
            {
                Console.WriteLine("\nDịch Vụ Đã Đặt:");
                Console.WriteLine("Tên\tSố Lượng\tGiá\tTrạng Thái");
                Console.WriteLine(new string('-', 50));
                foreach (var order in orders)
                {
                    Console.WriteLine($"{order.Service.Name}\t{order.Quantity}\t${order.TotalPrice:F2}\t{order.Status}");
                }
            }
        }
        else
        {
            Console.WriteLine("Không tìm thấy phiên hoặc phiên đã kết thúc.");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task AddServiceToSessionAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Thêm Dịch Vụ Vào Phiên ===");
        
        Console.Write("Enter Session ID: ");
        if (!int.TryParse(Console.ReadLine(), out int sessionId))
        {
            Console.WriteLine("ID phiên không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        var session = await _sessionService.GetActiveSessionByCustomerAsync(sessionId);
        if (session == null)
        {
            Console.WriteLine("Không tìm thấy phiên hoặc phiên đã kết thúc.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        // Show available services
        var services = await _serviceManagementService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("Không có dịch vụ nào.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nDịch Vụ Hiện Có:");
        Console.WriteLine("ID\tTên\tGiá\tTồn Kho");
        Console.WriteLine(new string('-', 50));
        foreach (var service in services)
        {
            Console.WriteLine($"{service.Id}\t{service.Name}\t${service.Price:F2}\t{service.CurrentStock}");
        }

        Console.Write("\nNhập ID Dịch Vụ: ");
        if (!int.TryParse(Console.ReadLine(), out int serviceId))
        {
            Console.WriteLine("ID dịch vụ không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.Write("Nhập Số Lượng: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Số lượng không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        try
        {
            await _serviceOrderManagementService.CreateOrderAsync(sessionId, serviceId, quantity);
            Console.WriteLine("Thêm dịch vụ vào phiên thành công!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ShowServiceMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Quản Lý Dịch Vụ ===");
            Console.WriteLine("1. Thêm Dịch Vụ Mới");
            Console.WriteLine("2. Xem Chi Tiết Dịch Vụ");
            Console.WriteLine("3. Cập Nhật Dịch Vụ");
            Console.WriteLine("4. Xóa Dịch Vụ");
            Console.WriteLine("5. Xem Tất Cả Dịch Vụ");
            Console.WriteLine("6. Cập Nhật Tồn Kho");
            Console.WriteLine("7. Xem Dịch Vụ Sắp Hết");
            Console.WriteLine("0. Quay Lại Menu Chính");
            Console.Write("\nNhập lựa chọn của bạn: ");

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
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occurred in service menu");
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task AddNewServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Thêm Dịch Vụ Mới ===");
        
        Console.Write("Nhập Tên Dịch Vụ: ");
        string name = Console.ReadLine() ?? string.Empty;

        Console.Write("Nhập Mô Tả: ");
        string description = Console.ReadLine() ?? string.Empty;

        Console.Write("Nhập Giá (USD): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine("Giá không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.Write("Nhập Số Lượng Ban Đầu: ");
        if (!int.TryParse(Console.ReadLine(), out int stock))
        {
            Console.WriteLine("Số lượng không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.Write("Nhập Số Lượng Tối Thiểu: ");
        if (!int.TryParse(Console.ReadLine(), out int minStock))
        {
            Console.WriteLine("Số lượng tối thiểu không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nChọn Loại Dịch Vụ:");
        Console.WriteLine("1. Thức Ăn");
        Console.WriteLine("2. Đồ Uống");
        Console.WriteLine("3. Thiết Bị");
        Console.WriteLine("4. Khác");
        Console.Write("Nhập lựa chọn (1-4): ");

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
        Console.WriteLine("\nThêm dịch vụ thành công!");
        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ViewServiceDetailsAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Chi Tiết Dịch Vụ ===");
        
        Console.Write("Nhập ID Dịch Vụ: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID không hợp lệ.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        var service = await _serviceManagementService.GetServiceAsync(id);
        if (service != null)
        {
            Console.WriteLine($"\nID: {service.Id}");
            Console.WriteLine($"Tên: {service.Name}");
            Console.WriteLine($"Mô Tả: {service.Description}");
            Console.WriteLine($"Loại: {service.Category}");
            Console.WriteLine($"Giá: ${service.Price:F2}");
            Console.WriteLine($"Tồn Kho: {service.CurrentStock}");
            Console.WriteLine($"Tồn Kho Tối Thiểu: {service.MinimumStock}");
        }
        else
        {
            Console.WriteLine("Không tìm thấy dịch vụ.");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task UpdateServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Service ===");
        
        Console.Write("Enter Service ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var service = await _serviceManagementService.GetServiceAsync(id);
        if (service == null)
        {
            Console.WriteLine("Service not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nCurrent Details:");
        Console.WriteLine($"Name: {service.Name}");
        Console.WriteLine($"Description: {service.Description}");
        Console.WriteLine($"Price: ${service.Price:F2}");
        Console.WriteLine($"Minimum Stock: {service.MinimumStock}");

        Console.WriteLine("\nEnter new details (press Enter to keep current value):");
        
        Console.Write("New Name: ");
        string name = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(name))
            service.Name = name;

        Console.Write("New Description: ");
        string description = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(description))
            service.Description = description;

        Console.Write("New Price: ");
        string priceStr = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(priceStr) && decimal.TryParse(priceStr, out decimal price))
            service.Price = price;

        Console.Write("New Minimum Stock: ");
        string minStockStr = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(minStockStr) && int.TryParse(minStockStr, out int minStock))
            service.MinimumStock = minStock;

        await _serviceManagementService.UpdateServiceAsync(service);
        Console.WriteLine("\nService updated successfully!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private async Task DeleteServiceAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Service ===");
        
        Console.Write("Enter Service ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var service = await _serviceManagementService.GetServiceAsync(id);
        if (service == null)
        {
            Console.WriteLine("Service not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nService Details:");
        Console.WriteLine($"Name: {service.Name}");
        Console.WriteLine($"Description: {service.Description}");
        Console.WriteLine($"Price: ${service.Price:F2}");
        Console.WriteLine($"Current Stock: {service.CurrentStock}");

        Console.Write("\nAre you sure you want to delete this service? (y/N): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            await _serviceManagementService.DeleteServiceAsync(id);
            Console.WriteLine("Service deleted successfully!");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ViewAllServicesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Tất Cả Dịch Vụ ===\n");
        
        var services = await _serviceManagementService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("Không tìm thấy dịch vụ nào.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");;
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tTên\tLoại\tGiá (USD)\tTồn Kho\tTồn Tối Thiểu");
        Console.WriteLine(new string('-', 90));
        
        foreach (var service in services)
        {
            Console.WriteLine($"{service.Id}\t{service.Name}\t{service.Category}\t${service.Price:F2}\t{service.CurrentStock}\t{service.MinimumStock}");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task UpdateServiceStockAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Update Service Stock ===");
        
        Console.Write("Enter Service ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        var service = await _serviceManagementService.GetServiceAsync(id);
        if (service == null)
        {
            Console.WriteLine("Service not found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\nCurrent Stock: {service.CurrentStock}");
        Console.Write("Enter stock change (+/- quantity): ");
        if (!int.TryParse(Console.ReadLine(), out int change))
        {
            Console.WriteLine("Invalid quantity format.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        try
        {
            await _serviceManagementService.UpdateStockAsync(id, change);
            Console.WriteLine("Stock updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ViewLowStockServicesAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Low Stock Services ===\n");
        
        var services = await _serviceManagementService.GetLowStockServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("No services are low on stock.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("ID\tName\tCategory\tCurrent Stock\tMinimum Stock");
        Console.WriteLine(new string('-', 70));
        
        foreach (var service in services)
        {
            Console.WriteLine($"{service.Id}\t{service.Name}\t{service.Category}\t{service.CurrentStock}\t{service.MinimumStock}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ShowReportsMenuAsync()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("=== Báo Cáo ===");
            Console.WriteLine("1. Báo Cáo Phiên Trong Ngày");
            Console.WriteLine("2. Báo Cáo Doanh Thu Ngày");
            Console.WriteLine("3. Báo Cáo Sử Dụng Máy");
            Console.WriteLine("4. Báo Cáo Dịch Vụ Phổ Biến");
            Console.WriteLine("5. Báo Cáo Hoạt Động Khách Hàng");
            Console.WriteLine("0. Quay Lại Menu Chính");
            Console.Write("\nNhập lựa chọn của bạn: ");

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
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error occurred in reports menu");
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task ShowDailySessionReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Báo Cáo Phiên Trong Ngày ===\n");

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var sessions = await _sessionService.GetSessionsByDateRangeAsync(today, tomorrow);

        if (!sessions.Any())
        {
            Console.WriteLine("Không có phiên nào trong ngày hôm nay.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        var totalSessions = sessions.Count();
        var completedSessions = sessions.Count(s => s.EndTime.HasValue);
        var activeSessions = sessions.Count(s => !s.EndTime.HasValue);
        var totalRevenue = sessions.Where(s => s.EndTime.HasValue).Sum(s => s.TotalCost);
        var avgDuration = sessions.Where(s => s.EndTime.HasValue)
            .Average(s => (s.EndTime!.Value - s.StartTime).TotalHours);

        Console.WriteLine($"Ngày: {today:d}");
        Console.WriteLine($"Tổng Số Phiên: {totalSessions}");
        Console.WriteLine($"Phiên Đã Kết Thúc: {completedSessions}");
        Console.WriteLine($"Phiên Đang Hoạt Động: {activeSessions}");
        Console.WriteLine($"Tổng Doanh Thu: ${totalRevenue:F2}");
        Console.WriteLine($"Thời Gian Trung Bình: {avgDuration:F2} giờ");

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ShowDailyRevenueReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Báo Cáo Doanh Thu Ngày ===\n");

        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var sessions = await _sessionService.GetSessionsByDateRangeAsync(today, tomorrow);

        if (!sessions.Any())
        {
            Console.WriteLine("Không có doanh thu trong ngày hôm nay.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        decimal sessionRevenue = sessions.Where(s => s.EndTime.HasValue).Sum(s => s.TotalCost);
        
        var orders = sessions.SelectMany(s => s.ServiceOrders)
            .Where(o => o.Status == OrderStatus.Completed);
        
        var serviceRevenue = orders.Sum(o => o.TotalPrice);
        var totalRevenue = sessionRevenue + serviceRevenue;

        Console.WriteLine($"Ngày: {today:d}");
        Console.WriteLine($"Doanh Thu Phiên Chơi Game: ${sessionRevenue:F2}");
        Console.WriteLine($"Doanh Thu Dịch Vụ: ${serviceRevenue:F2}");
        Console.WriteLine($"Tổng Doanh Thu: ${totalRevenue:F2}");

        Console.WriteLine("\nDoanh Thu Theo Loại Dịch Vụ:");
        var categoryRevenue = orders
            .GroupBy(o => o.Service.Category)
            .Select(g => new { Category = g.Key, Revenue = g.Sum(o => o.TotalPrice) });

        foreach (var category in categoryRevenue)
        {
            Console.WriteLine($"{category.Category}: ${category.Revenue:F2}");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ShowStationUsageReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Station Usage Report ===\n");

        var stations = await _stationService.GetAllStationsAsync();
        if (!stations.Any())
        {
            Console.WriteLine("No stations found.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Station Usage Summary:");
        Console.WriteLine("Number\tStatus\tTotal Sessions\tTotal Hours\tRevenue");
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

        Console.WriteLine("\nCurrent Status Summary:");
        Console.WriteLine($"Total Stations: {totalStations}");
        Console.WriteLine($"Available: {availableStations}");
        Console.WriteLine($"Occupied: {occupiedStations}");
        Console.WriteLine($"In Maintenance: {maintenanceStations}");

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private async Task ShowPopularServicesReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Báo Cáo Dịch Vụ Phổ Biến ===\n");

        var services = await _serviceManagementService.GetAllServicesAsync();
        if (!services.Any())
        {
            Console.WriteLine("Không tìm thấy dịch vụ nào.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Dịch Vụ Phổ Biến Nhất:");
        Console.WriteLine("Tên\tLoại\tSố Đơn\tSố Lượng Đã Bán\tDoanh Thu (USD)");
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
            Console.WriteLine("\nCảnh Báo Tồn Kho Thấp:");
            foreach (var service in lowStockServices)
            {
                Console.WriteLine($"{service.Name}: còn {service.CurrentStock} (Tối thiểu: {service.MinimumStock})");
            }
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }

    private async Task ShowCustomerActivityReportAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Báo Cáo Hoạt Động Khách Hàng ===\n");

        var customers = await _customerService.GetAllCustomersAsync();
        if (!customers.Any())
        {
            Console.WriteLine("Không tìm thấy khách hàng nào.");
            Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Tổng Kết Hoạt Động Khách Hàng:");
        Console.WriteLine("Tên\tHạng\tTổng Phiên\tTổng Giờ\tTổng Chi (USD)");
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

        Console.WriteLine("\nPhân Bố Hạng Thành Viên:");
        foreach (var stat in membershipStats)
        {
            Console.WriteLine($"{stat.Tier}: {stat.Count} khách hàng");
        }

        Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
        Console.ReadKey();
    }
}