using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalServiceLab3
{
    // Структура для демонстрацiї (мiн. 4 поля рiзних типiв)
    struct VehicleSummary
    {
        public string Model;
        public string Type;
        public int PricePerDay;
        public bool IsAvailable;

        public VehicleSummary(string model, string type, int pricePerDay, bool isAvailable)
        {
            Model = model;
            Type = type;
            PricePerDay = pricePerDay;
            IsAvailable = isAvailable;
        }
    }

    class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public int PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public int RentedByClientId { get; set; }  // ← ЗМiНА: int замiсть Client
        public DateTime? RentedFrom { get; set; }
        public int RentedDays { get; set; }

        public Car(int id, string model, string type, int pricePerDay, bool isAvailable = true)
        {
            Id = id;
            Model = model;
            Type = type;
            PricePerDay = pricePerDay;
            IsAvailable = isAvailable;
            RentedByClientId = 0;
            RentedFrom = null;
            RentedDays = 0;
        }

        public override string ToString()
        {
            string status = IsAvailable ? "Доступне" : $"Орендоване (ID клiєнта: {RentedByClientId})";
            return $"{Model} ({Type}) - {PricePerDay} грн/день - {status}";
        }
    }
    class Client
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ID { get; set; }
        public int Age { get; set; }

        public Client(string name, string phone, string id, int age)
        {
            Name = name;
            Phone = phone;
            ID = id;
            Age = age;
        }
    }

    class Booking
    {
        public Car Car { get; set; }
        public Client Client { get; set; }
        public DateTime From { get; set; }
        public int Days { get; set; }
        public decimal TotalPrice => Car.PricePerDay * Days;

        public Booking(Car car, Client client, DateTime from, int days)
        {
            Car = car;
            Client = client;
            From = from;
            Days = days;
        }

        public override string ToString()
        {
            return $"{Car.Model} -> {Client.Name}, з {From:dd.MM.yyyy} на {Days} дн. ({TotalPrice} грн)";
        }
    }

    class Program
    {
        private const string CarsFile = "cars.csv";
        private const string CarsHeader = "Id,Model,Type,PricePerDay,IsAvailable,RentedByClientId,RentedFrom,RentedDays";

        private static List<Car> cars = new List<Car>();
        private static List<Client> clients = new List<Client>();
        private static List<Booking> bookings = new List<Booking>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; 
            Console.InputEncoding = System.Text.Encoding.UTF8;   

            
        
            if (!LoginDoWhile())
            {
                Console.WriteLine("Невдало. Програма завершує роботу.");
                return;
            }

            InitializeDefaultDataWithFor();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                PrintHeader("Сервiс прокату авто - головне меню");
                Console.WriteLine("1. Показати всi авто (звичайний список)");
                Console.WriteLine("2. Фiльтри / Пошук авто");
                Console.WriteLine("3. Додати авто (введення декiлькох через for)");
                Console.WriteLine("4. Орендувати авто");
                Console.WriteLine("5. Повернути авто");
                Console.WriteLine("6. Клiєнти (показати / додати)");
                Console.WriteLine("7. Звiти та статистика");
                Console.WriteLine("8. Форматований звiт");
                Console.WriteLine("9. Операцiї з колекцiєю");
                Console.WriteLine("10. Вихiд");
                Console.Write("\nВаш вибiр: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ShowAllCars();
                        break;
                    case "2":
                        ShowFilterMenu();
                        break;
                    case "3":
                        AddMultipleCarsByFor();
                        break;
                    case "4":
                        RentCar();
                        break;
                    case "5":
                        ReturnCar();
                        break;
                    case "6":
                        ShowClientsMenu();
                        break;
                    case "7":
                        ShowReports();
                        ShowStatistics();
                        break;
                    case "8":
                        PrintFormattedReport();
                        break;
                    case "9":
                        CollectionMenu();
                        break;
                    case "10":
                        exit = true;
                        Console.WriteLine("Дякуємо за користування. До побачення!");
                        break;
                    default:
                        Console.WriteLine("Невiрний вибiр. Спробуйте ще раз.");
                        break;






                }

                if (!exit)
                {
                    Console.WriteLine("\nНатиснiть будь-яку клавiшу для повернення...");
                    Console.ReadKey();
                }
            }
        }

        static void PrintHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"=== {title} ===\n");
            Console.ResetColor();
        }

        static bool LoginDoWhile()
        {
            const string correctLogin = "admin";
            const string correctPassword = "12345";
            int attempts = 0;
            const int maxAttempts = 3;
            string login, password;

            do
            {
                Console.Clear();
                Console.WriteLine("Вхiд у систему (макс 3 спроби)");
                Console.Write("Логiн: ");
                login = Console.ReadLine();
                Console.Write("Пароль: ");
                password = Console.ReadLine();

                attempts++;

                if (login == correctLogin && password == correctPassword)
                {
                    Console.WriteLine("Вхiд успiшний!");
                    Console.WriteLine("Натиснiть будь-яку клавiшу...");
                    Console.ReadKey();
                    return true;
                }
                else
                {
                    Console.WriteLine($"Невiрнi данi. Спроба {attempts}/{maxAttempts}");
                    if (attempts >= maxAttempts) break;
                    Console.WriteLine("Натиснiть будь-яку клавiшу для повтору...");
                    Console.ReadKey();
                }
            } while (attempts < maxAttempts);

            return false;
        }

        static void InitializeDefaultDataWithFor()
        {
            Console.Clear();
            PrintHeader("iнiцiалiзацiя початкових даних");

            // users.csv
            string usersPath = "users.csv";
            string usersHeader = "Id,Email,PasswordHash";
            if (!File.Exists(usersPath))
            {
                string adminLine = $"1,admin@example.com,{PasswordHasher.Hash("admin123")}";
                File.WriteAllText(usersPath, usersHeader + Environment.NewLine + adminLine + Environment.NewLine);
            }

            // cars.csv
            if (!File.Exists(CarsFile) || new FileInfo(CarsFile).Length == 0)
            {
                var defaultCars = new List<Car>();
                int nextId = IdGenerator.GetNextId(CarsFile, CarsHeader);

                defaultCars.Add(new Car(nextId++, "Toyota Corolla", "Легковий", 500));
                defaultCars.Add(new Car(nextId++, "BMW 5 Series", "Легковий", 700));
                defaultCars.Add(new Car(nextId++, "Tesla Model 3", "Електро", 1000));
                defaultCars.Add(new Car(nextId++, "Audi A4", "Легковий", 650));
                defaultCars.Add(new Car(nextId++, "Honda Civic", "Легковий", 550));
                defaultCars.Add(new Car(nextId++, "Toyota RAV4", "Позашляховик", 800));
                defaultCars.Add(new Car(nextId++, "Mercedes Sprinter", "Мiкроавтобус", 1200));

                SaveCars(defaultCars);
                Console.WriteLine("Створено cars.csv з початковими авто.");
            }

            Console.WriteLine("iнiцiалiзацiя завершена.");
            Console.ReadKey();
        }

        static void ShowAllCars()
        {
            Console.Clear();
            PrintHeader("Список авто");

            var cars = LoadCars();

            if (cars.Count == 0)
            {
                Console.WriteLine("Список автомобiлiв пустий.");
                return;
            }

            int idx = 1;
            foreach (var car in cars)
            {
                Console.WriteLine($"{idx}. {car}");
                idx++;
            }
        }

        static void ShowFilterMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("Фiльтри та пошук");
                Console.WriteLine("1. Показати доступнi авто");
                Console.WriteLine("2. Фiльтрувати за типом");
                Console.WriteLine("3. Фiльтрувати за цiною (<= вказане)");
                Console.WriteLine("4. Пошук за моделлю");
                Console.WriteLine("5. Повернутися");
                Console.Write("\nВибiр: ");
                string c = Console.ReadLine();

                switch (c)
                {
                    case "1":
                        ShowAvailableOnly();
                        break;
                    case "2":
                        FilterCarsByType();
                        break;
                    case "3":
                        FilterCarsByPrice();
                        break;
                    case "4":
                        SearchCarByModel();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Невiрний вибiр.");
                        break;
                }

                Console.WriteLine("\nНатиснiть будь-яку клавiшу...");
                Console.ReadKey();
            }
        }

        static void ShowAvailableOnly()
        {
            Console.Clear();
            PrintHeader("Доступнi авто");
            List<Car> available = new List<Car>();
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].IsAvailable) available.Add(cars[i]);
            }
            if (available.Count == 0)
            {
                Console.WriteLine("Немає доступних авто.");
                return;
            }
            int i2 = 1;
            foreach (var car in available)
            {
                Console.WriteLine($"{i2}. {car}");
                i2++;
            }
        }

        static void FilterCarsByType()
        {
            Console.Clear();
            PrintHeader("Фiльтрацiя за типом");
            Console.Write("Введiть тип (наприклад: Легковий, Позашляховик, Мiкроавтобус, Електро): ");
            string type = Console.ReadLine();

            List<Car> filtered = new List<Car>();
            for (int i = 0; i < cars.Count; i++)
            {
                if (string.Equals(cars[i].Type, type, StringComparison.OrdinalIgnoreCase))
                    filtered.Add(cars[i]);
            }

            if (filtered.Count == 0)
            {
                Console.WriteLine("Результатiв немає.");
                return;
            }
            foreach (var car in filtered)
                Console.WriteLine(car);
        }

        static void FilterCarsByPrice()
        {
            Console.Clear();
            PrintHeader("Фiльтрацiя за цiною");
            Console.Write("Показати авто з цiною не бiльше (грн/день): ");
            if (!int.TryParse(Console.ReadLine(), out int limit))
            {
                Console.WriteLine("Некоректне значення. Спробуйте ще раз.");
                return;
            }

            List<Car> filtered = new List<Car>();
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].PricePerDay <= limit) filtered.Add(cars[i]);
            }

            if (filtered.Count == 0)
            {
                Console.WriteLine("Результатiв немає.");
                return;
            }
            foreach (var car in filtered)
                Console.WriteLine(car);
        }

        static void SearchCarByModel()
        {
            Console.Clear();
            PrintHeader("Пошук за моделлю");
            Console.Write("Введiть (частково) модель: ");
            string q = Console.ReadLine();
            List<Car> results = new List<Car>();
            for (int i = 0; i < cars.Count; i++)
            {
                if (!string.IsNullOrEmpty(q) && cars[i].Model.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(cars[i]);
                }
            }

            if (results.Count == 0)
            {
                Console.WriteLine("Не знайдено.");
                return;
            }
            foreach (var car in results) Console.WriteLine(car);
        }

        static void AddMultipleCarsByFor()
        {
            Console.Clear();
            PrintHeader("Додавання кiлькох авто через for");

            Console.Write("Скiльки авто бажаєте додати? (цiле число): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Некоректне число.");
                Console.ReadKey();
                return;
            }

            var currentCars = LoadCars();  // завантажуємо всi авто з файлу

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\nДодавання авто #{i + 1}");
                Console.Write("Модель: ");
                string model = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Тип: ");
                string type = Console.ReadLine()?.Trim() ?? "";

                int price = 0;
                bool validPrice = false;
                do
                {
                    Console.Write("Цiна за день (грн): ");
                    if (int.TryParse(Console.ReadLine(), out price) && price > 0)
                        validPrice = true;
                    else
                        Console.WriteLine("Невiрний формат цiни — введiть цiле позитивне число.");
                } while (!validPrice);

                int newId = IdGenerator.GetNextId(CarsFile, CarsHeader);
                var newCar = new Car(newId, model, type, price, true);

                currentCars.Add(newCar);
                Console.WriteLine($"Авто додано (ID: {newId})");
            }

            SaveCars(currentCars);  // зберiгаємо ВСi авто назад у файл

            Console.WriteLine($"\nДодано {n} авто. Натиснiть будь-яку клавiшу...");
            Console.ReadKey();
        }
        static void ShowClientsMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("Клiєнти");
                Console.WriteLine("1. Показати всiх клiєнтiв");
                Console.WriteLine("2. Додати клiєнта");
                Console.WriteLine("3. Повернутися");
                Console.Write("Вибiр: ");
                string c = Console.ReadLine();
                if (c == "1")
                {
                    Console.Clear();
                    PrintHeader("Список клiєнтiв");
                    if (clients.Count == 0)
                    {
                        Console.WriteLine("Клiєнтiв немає.");
                    }
                    else
                    {
                        int i = 1;
                        foreach (var cl in clients)
                        {
                            Console.WriteLine($"{i}. {cl.Name} | Тел: {cl.Phone} | ID: {cl.ID} | Вiк: {cl.Age}");
                            i++;
                        }
                    }
                }
                else if (c == "2")
                {
                    AddClient();
                }
                else if (c == "3")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Невiрний вибiр.");
                }

                Console.WriteLine("\nНатиснiть будь-яку клавiшу...");
                Console.ReadKey();
            }
        }

        static void AddClient()
        {
            Console.Clear();
            PrintHeader("Додавання клієнта");

            Console.Write("Ім'я та прізвище: ");
            string name = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Телефон: ");
            string phone = Console.ReadLine()?.Trim() ?? "";

            Console.Write("ID (паспорт/інше): ");
            string id = Console.ReadLine()?.Trim() ?? "";

            int age = 0;
            Console.Write("Вік: ");
            if (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
            {
                Console.WriteLine("Некоректний вік. Клієнт не додано.");
                Console.ReadKey();
                return;
            }

            var allClients = LoadClients(); // завантажуємо поточних клієнтів

            int newId = IdGenerator.GetNextId("clients.csv", "Id,Name,Phone,ID,Age");

            var newClient = new Client(name, phone, id, age) { Id = newId };

            allClients.Add(newClient);

            SaveClients(allClients); // ЗБЕРІГАЄМО В ФАЙЛ

            Console.WriteLine($"Клієнта додано (ID: {newId}).");
            Console.ReadKey();
        }

        static void RentCar()
        {
            Console.Clear();
            PrintHeader("Оренда авто");

            var availableCars = LoadCars().Where(c => c.IsAvailable).ToList();

            if (availableCars.Count == 0)
            {
                Console.WriteLine("Немає доступних авто для оренди.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < availableCars.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableCars[i]}");
            }

            Console.Write("\nВведiть номер авто для оренди: ");
            if (!int.TryParse(Console.ReadLine(), out int sel) || sel < 1 || sel > availableCars.Count)
            {
                Console.WriteLine("Некоректний вибiр.");
                Console.ReadKey();
                return;
            }

            Car chosen = availableCars[sel - 1];

            // Показ клiєнтiв
            var allClients = LoadClients();  // додамо пiзнiше метод LoadClients()
            for (int i = 0; i < allClients.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {allClients[i].Name} | {allClients[i].Phone}");
            }
            Console.WriteLine($"{allClients.Count + 1}. Додати нового клiєнта");

            Console.Write("Ваш вибiр: ");
            if (!int.TryParse(Console.ReadLine(), out int clientSel) || clientSel < 1 || clientSel > allClients.Count + 1)
            {
                Console.WriteLine("Некоректний вибiр клiєнта.");
                Console.ReadKey();
                return;
            }

            Client client;
            if (clientSel == allClients.Count + 1)
            {
                AddClient();  // додаємо нового

                allClients = LoadClients();  // перезавантажуємо

                if (allClients.Count == 0)
                {
                    Console.WriteLine("Помилка: клієнт не додався. Спробуйте ще раз.");
                    Console.ReadKey();
                    return;
                }

                client = allClients[allClients.Count - 1];  // тепер безпечно
            }
            else
            {
                client = allClients[clientSel - 1];
            }
            

            Console.Write("На скiльки днiв орендує клiєнт? ");
            if (!int.TryParse(Console.ReadLine(), out int days) || days <= 0)
            {
                Console.WriteLine("Некоректна кiлькiсть днiв.");
                Console.ReadKey();
                return;
            }

            // Оновлюємо авто
            var allCars = LoadCars();
            var carToUpdate = allCars.FirstOrDefault(c => c.Id == chosen.Id);
            if (carToUpdate != null)
            {
                carToUpdate.IsAvailable = false;
                carToUpdate.RentedByClientId = client.Id;  // ← ЗМiНА: ID клiєнта
                carToUpdate.RentedFrom = DateTime.Now;
                carToUpdate.RentedDays = days;

                SaveCars(allCars);  // зберiгаємо всi авто назад
            }

            // Додаємо бронювання (пiзнiше адаптуємо bookings.csv)
            var booking = new Booking(chosen, client, DateTime.Now, days);
            // bookings.Add(booking);  // поки закоментуй, бо bookings ще не на CSV

            Console.WriteLine($"Автомобiль {chosen.Model} успiшно орендовано клiєнтом {client.Name} на {days} дн. Сума = {booking.TotalPrice} грн.");
            Console.ReadKey();
        }

        static void ReturnCar()
        {
            Console.Clear();
            PrintHeader("Повернення авто");

            var rentedCars = LoadCars().Where(c => !c.IsAvailable).ToList();

            if (rentedCars.Count == 0)
            {
                Console.WriteLine("Немає орендованих авто.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < rentedCars.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {rentedCars[i]}");
            }

            Console.Write("Введiть номер авто для повернення: ");
            if (!int.TryParse(Console.ReadLine(), out int sel) || sel < 1 || sel > rentedCars.Count)
            {
                Console.WriteLine("Некоректний вибiр.");
                Console.ReadKey();
                return;
            }

            Car chosen = rentedCars[sel - 1];

            Console.Write("Фактична кiлькiсть днiв користування (введiть цiле число): ");
            if (!int.TryParse(Console.ReadLine(), out int actualDays) || actualDays < 0)
            {
                Console.WriteLine("Некоректне значення днiв.");
                Console.ReadKey();
                return;
            }

            decimal total = chosen.PricePerDay * actualDays;

            // Оновлюємо авто
            var allCars = LoadCars();
            var carToUpdate = allCars.FirstOrDefault(c => c.Id == chosen.Id);
            if (carToUpdate != null)
            {
                carToUpdate.IsAvailable = true;
                carToUpdate.RentedByClientId = 0;  // ← очищаємо ID клiєнта
                carToUpdate.RentedFrom = null;
                carToUpdate.RentedDays = 0;

                SaveCars(allCars);  // зберiгаємо змiни
            }

            // Поки bookings не на CSV — просто повiдомлення (пiзнiше додамо)
            // var bookings = LoadBookings(); // додамо пiзнiше
            // видаляємо бронювання по CarId

            Console.WriteLine($"Авто {chosen.Model} повернуто. Пiдсумок до сплати: {total} грн.");
            Console.ReadKey();
        }

        static void ShowReports()
        {
            Console.Clear();
            PrintHeader("Звiт по всiх авто");
            foreach (var car in cars)
            {
                Console.WriteLine(car.ToString());
            }

            Console.WriteLine("\n=== Активнi бронювання ===");
            if (bookings.Count == 0)
            {
                Console.WriteLine("Бронювань немає.");
            }
            else
            {
                foreach (var b in bookings)
                    Console.WriteLine(b.ToString());
            }
        }

        static void ShowStatistics()
        {
            Console.WriteLine("\n=== Статистика ===");

            if (cars.Count == 0)
            {
                Console.WriteLine("Немає даних для статистики.");
                return;
            }

            long totalSum = 0;
            int countOver500 = 0;
            int minPrice = int.MaxValue;
            int maxPrice = int.MinValue;

            for (int i = 0; i < cars.Count; i++)
            {
                int p = cars[i].PricePerDay;
                totalSum += p;
                if (p > 500) countOver500++;
                if (p < minPrice) minPrice = p;
                if (p > maxPrice) maxPrice = p;
            }

            double avgPrice = (double)totalSum / cars.Count;

            Console.WriteLine($"Загальна сума (всi цiни/день): {totalSum} грн");
            Console.WriteLine($"Середня цiна: {avgPrice:F2} грн/день");
            Console.WriteLine($"Кiлькiсть авто з цiною > 500 грн: {countOver500}");
            Console.WriteLine($"Мiнiмальна цiна: {minPrice} грн/день");
            Console.WriteLine($"Максимальна цiна: {maxPrice} грн/день");

            Console.WriteLine("\nКiлькiсть авто за типами:");
            List<string> types = new List<string>();
            for (int i = 0; i < cars.Count; i++)
            {
                string t = cars[i].Type;
                bool found = false;
                for (int j = 0; j < types.Count; j++)
                    if (types[j] == t) { found = true; break; }
                if (!found) types.Add(t);
            }

            for (int ti = 0; ti < types.Count; ti++)
            {
                int cnt = 0;
                for (int i = 0; i < cars.Count; i++)
                    if (cars[i].Type == types[ti]) cnt++;
                Console.WriteLine($"{types[ti]}: {cnt}");
            }
        }

        static void PrintFormattedReport()
        {
            Console.Clear();
            PrintHeader("Форматований звiт");

            var sb = new StringBuilder();
            sb.AppendLine("====== Звiт сервiсу прокату авто ======");
            sb.AppendLine($"Дата звiту: {DateTime.Now:dd.MM.yyyy HH:mm}");
            sb.AppendLine();
            sb.AppendLine("Автомобiлi:");
            sb.AppendLine("№\tМодель\t\t\tТип\t\tЦiна(грн/день)\tСтатус");

            int i = 1;
            foreach (var car in cars)
            {
                string status = car.IsAvailable ? "Доступне" : $"Орендоване (ID: {car.RentedByClientId})";

                sb.AppendLine($"{i}\t{Truncate(car.Model, 16),-16}\t{Truncate(car.Type, 10),-10}\t{car.PricePerDay}\t\t{status}");
                i++;
            }

            sb.AppendLine();
            sb.AppendLine("Клiєнти:");
            sb.AppendLine("№\tIм'я\t\t\tТелефон\t\tID\t\tВiк");
            i = 1;
            foreach (var cl in clients)
            {
                sb.AppendLine($"{i}\t{Truncate(cl.Name, 16),-16}\t{Truncate(cl.Phone, 12),-12}\t{Truncate(cl.ID, 10),-10}\t{cl.Age}");
                i++;
            }

            sb.AppendLine();
            sb.AppendLine("Активнi бронювання:");
            if (bookings.Count == 0) sb.AppendLine("Немає активних бронювань.");
            else
            {
                foreach (var b in bookings)
                    sb.AppendLine(b.ToString());
            }

            sb.AppendLine();
            sb.AppendLine("Коротка статистика:");
            sb.AppendLine($"Загальна кiлькiсть авто: {cars.Count}");
            int availableCount = 0;
            for (int k = 0; k < cars.Count; k++) if (cars[k].IsAvailable) availableCount++;
            sb.AppendLine($"Доступнi: {availableCount}");
            int rentedCount = cars.Count - availableCount;
            sb.AppendLine($"Орендованi: {rentedCount}");
            double avg = 0;
            if (cars.Count > 0)
            {
                long s = 0;
                for (int k = 0; k < cars.Count; k++) s += cars[k].PricePerDay;
                avg = (double)s / cars.Count;
            }
            sb.AppendLine($"Середня цiна (грн/день): {avg:F2}");

            Console.WriteLine(sb.ToString());

            // Можна зберегти у файл, розкоментувавши
            // System.IO.File.WriteAllText("report.txt", sb.ToString());
            Console.WriteLine("Звiт сформовано. (Щоб зберегти у файл — розкоментуйте запис у кодi або додайте збереження.)");
        }

        static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= max ? s : s.Substring(0, max - 3) + "...";
        }

        // ----------------------- операцiї з колекцiєю -----------------------
        static void CollectionMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintHeader("Операцiї з колекцiєю (List<Car>)");
                Console.WriteLine("1. Вивести всi елементи у виглядi таблицi");
                Console.WriteLine("2. Додати елемент");
                Console.WriteLine("3. Пошук (за моделлю або за ID клiєнта у бронюваннях)");
                Console.WriteLine("4. Видалити елемент (за iндексом або моделлю)");
                Console.WriteLine("5. Сортування (вбудоване та бульбашка)");
                Console.WriteLine("6. Статистика по колекцiї");
                Console.WriteLine("7. Повернутися");
                Console.Write("Вибiр: ");
                string ch = Console.ReadLine();
                switch (ch)
                {
                    case "1":
                        PrintCarsTable();
                        break;
                    case "2":
                        AddCarInteractive();
                        break;
                    case "3":
                        CollectionSearchMenu();
                        break;
                    case "4":
                        DeleteCarMenu();
                        break;
                    case "5":
                        SortMenu();
                        break;
                    case "6":
                        CollectionStatistics();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Невiрний вибiр.");
                        break;
                }
                Console.WriteLine("\nНатиснiть будь-яку клавiшу...");
                Console.ReadKey();
            }
        }

        // 3. Виведення в виглядi таблицi з вирiвняними стовпцями
        static void PrintCarsTable()
        {
            Console.Clear();
            PrintHeader("Таблиця авто");
            if (cars.Count == 0)
            {
                Console.WriteLine("Колекцiя порожня.");
                return;
            }

            // Заголовок
            Console.WriteLine("{0,-4} {1,-20} {2,-14} {3,8} {4,12}", "№", "Модель", "Тип", "Цiна", "Статус");
            Console.WriteLine(new string('-', 64));

            for (int i = 0; i < cars.Count; i++)
            {
                var car = cars[i];
                string status = car.IsAvailable ? "Доступне" : "Орендоване";
                Console.WriteLine("{0,-4} {1,-20} {2,-14} {3,8} {4,12}", i + 1, Truncate(car.Model, 20), Truncate(car.Type, 14), car.PricePerDay, status);
            }
        }

        // 2. Додавання одного елемента (користувач вводить поля)
        static void AddCarInteractive()
        {
            Console.Clear();
            PrintHeader("Додавання одного авто");

            Console.Write("Модель: ");
            string model = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Тип: ");
            string type = Console.ReadLine()?.Trim() ?? "";

            int price;
            Console.Write("Цiна за день (цiле число): ");
            if (!int.TryParse(Console.ReadLine(), out price) || price <= 0)
            {
                Console.WriteLine("Некоректна цiна. Операцiю вiдмiнено.");
                Console.ReadKey();
                return;
            }

            // Завантажуємо всi авто
            var currentCars = LoadCars();

            // Генеруємо новий ID
            int newId = IdGenerator.GetNextId(CarsFile, CarsHeader);

            // Створюємо i додаємо нове авто
            var newCar = new Car(newId, model, type, price, true);
            currentCars.Add(newCar);

            // Зберiгаємо всi авто назад у файл
            SaveCars(currentCars);

            Console.WriteLine($"Авто додано у колекцiю (ID: {newId}).");
            Console.ReadKey();
        }

        // 4. Пошук: меню
        static void CollectionSearchMenu()
        {
            Console.Clear();
            PrintHeader("Пошук");
            Console.WriteLine("1. Пошук авто за моделлю (частково)");
            Console.WriteLine("2. Пошук бронювань за ID клiєнта");
            Console.WriteLine("3. Повернутися");
            Console.Write("Вибiр: ");
            string ch = Console.ReadLine();
            switch (ch)
            {
                case "1":
                    Console.Write("Введiть частину моделi: ");
                    string q = Console.ReadLine();
                    List<Car> found = new List<Car>();
                    for (int i = 0; i < cars.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(q) && cars[i].Model.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                            found.Add(cars[i]);
                    }
                    if (found.Count == 0) Console.WriteLine("Не знайдено авто за моделлю.");
                    else
                    {
                        Console.WriteLine("Знайденi авто:");
                        foreach (var car in found) Console.WriteLine(car);
                    }
                    break;
                case "2":
                    Console.Write("Введiть ID клiєнта для пошуку в бронюваннях: ");
                    string id = Console.ReadLine();
                    List<Booking> foundB = new List<Booking>();
                    for (int i = 0; i < bookings.Count; i++)
                    {
                        if (bookings[i].Client != null && bookings[i].Client.ID == id)
                            foundB.Add(bookings[i]);
                    }
                    if (foundB.Count == 0) Console.WriteLine("Бронювань для цього ID не знайдено.");
                    else
                    {
                        Console.WriteLine("Знайденi бронювання:");
                        foreach (var b in foundB) Console.WriteLine(b);
                    }
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Невiрний вибiр.");
                    break;
            }
        }

        // 5. Видалення: за iндексом або моделлю
        static void DeleteCarMenu()
        {
            Console.Clear();
            PrintHeader("Видалення авто");
            Console.WriteLine("1. Видалити за iндексом");
            Console.WriteLine("2. Видалити за моделлю (всi спiвпадiння або перше)");
            Console.WriteLine("3. Повернутися");
            Console.Write("Вибiр: ");
            string ch = Console.ReadLine();
            switch (ch)
            {
                case "1":
                    Console.Write("Введiть iндекс (номер у списку, починається з 1): ");
                    if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > cars.Count)
                    {
                        Console.WriteLine("Некоректний iндекс.");
                        return;
                    }
                    cars.RemoveAt(idx - 1);
                    Console.WriteLine("Елемент видалено.");
                    break;
                case "2":
                    Console.Write("Введiть модель (точно або частково): ");
                    string q = Console.ReadLine();
                    List<int> toRemove = new List<int>();
                    for (int i = 0; i < cars.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(q) && cars[i].Model.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                            toRemove.Add(i);
                    }
                    if (toRemove.Count == 0)
                    {
                        Console.WriteLine("Не знайдено моделей для видалення.");
                        return;
                    }
                    Console.WriteLine($"Знайдено {toRemove.Count} елементiв. Видалити всi? (y/n)");
                    string ans = Console.ReadLine();
                    if (ans != null && ans.ToLower() == "y")
                    {
                        // видаляємо з кiнця щоб не зрушувати iндекси
                        for (int i = toRemove.Count - 1; i >= 0; i--)
                        {
                            cars.RemoveAt(toRemove[i]);
                        }
                        Console.WriteLine("Всi знайденi елементи видаленi.");
                    }
                    else
                    {
                        // видалити перше
                        cars.RemoveAt(toRemove[0]);
                        Console.WriteLine("Перший знайдений елемент видалено.");
                    }
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Невiрний вибiр.");
                    break;
            }
        }

        // 6. Сортування: меню з вбудованим та бульбашкою
        static void SortMenu()
        {
            Console.Clear();
            PrintHeader("Сортування колекцiї");
            Console.WriteLine("1. Вбудоване сортування за моделлю (алфавiт)");
            Console.WriteLine("2. Вбудоване сортування за цiною (зростання)");
            Console.WriteLine("3. Бульбашкове сортування за цiною (зростання) — власний алгоритм");
            Console.WriteLine("4. Повернутися");
            Console.Write("Вибiр: ");
            string ch = Console.ReadLine();
            switch (ch)
            {
                case "1":
                    cars.Sort((a, b) => string.Compare(a.Model, b.Model, StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine("Сортування за моделлю завершено (вбудоване).");
                    break;
                case "2":
                    cars.Sort((a, b) => a.PricePerDay.CompareTo(b.PricePerDay));
                    Console.WriteLine("Сортування за цiною завершено (вбудоване).");
                    break;
                case "3":
                    BubbleSortByPrice(cars);
                    Console.WriteLine("Бульбашкове сортування за цiною завершено.");
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Невiрний вибiр.");
                    break;
            }
        }

        // власний алгоритм сортування (бульбашка)
        static void BubbleSortByPrice(List<Car> list)
        {
            if (list == null || list.Count <= 1) return;
            // Робимо копiю, щоб показати результат порiвняння — проте тут змiнюємо сам список (можна зробити копiю при потребi)
            for (int i = 0; i < list.Count - 1; i++)
            {
                bool swapped = false;
                for (int j = 0; j < list.Count - 1 - i; j++)
                {
                    if (list[j].PricePerDay > list[j + 1].PricePerDay)
                    {
                        var tmp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = tmp;
                        swapped = true;
                    }
                }
                if (!swapped) break; // оптимiзацiя
            }
        }

        // 7. Статистика по колекцiї (кiлькiсть, мiн, макс, сума, середнє)
        static void CollectionStatistics()
        {
            Console.Clear();
            PrintHeader("Статистика по колекцiї (List<Car>)");
            if (cars.Count == 0)
            {
                Console.WriteLine("Колекцiя порожня.");
                return;
            }

            int count = cars.Count;
            long sum = 0;
            int min = int.MaxValue;
            int max = int.MinValue;
            for (int i = 0; i < cars.Count; i++)
            {
                int p = cars[i].PricePerDay;
                sum += p;
                if (p < min) min = p;
                if (p > max) max = p;
            }
            double avg = (double)sum / count;

            Console.WriteLine($"Кiлькiсть елементiв: {count}");
            Console.WriteLine($"Мiнiмальна цiна: {min} грн/день");
            Console.WriteLine($"Максимальна цiна: {max} грн/день");
            Console.WriteLine($"Сума цiн: {sum} грн");
            Console.WriteLine($"Середня цiна: {avg:F2} грн/день");
        }

        static List<Car> LoadCars()
        {
            var carList = new List<Car>();
            var lines = FileHelper.ReadCsvLines(CarsFile, CarsHeader);

            foreach (var parts in lines)
            {
                if (parts.Length < 8) continue;

                if (int.TryParse(parts[0], out int id) &&
                    int.TryParse(parts[3], out int price) &&
                    bool.TryParse(parts[4], out bool available) &&
                    int.TryParse(parts[5], out int rentedById) &&
                    int.TryParse(parts[7], out int days))
                {
                    var car = new Car(id, parts[1], parts[2], price, available)
                    {
                        RentedByClientId = rentedById,
                        RentedDays = days
                    };

                    if (!string.IsNullOrEmpty(parts[6]) && DateTime.TryParse(parts[6], out DateTime rentedFrom))
                        car.RentedFrom = rentedFrom;

                    carList.Add(car);
                }
            }

            return carList;
        }

        static void SaveCars(List<Car> carsToSave)
        {
            var lines = new List<string> { CarsHeader };

            foreach (var car in carsToSave)
            {
                string rentedFromStr = car.RentedFrom.HasValue ? car.RentedFrom.Value.ToString("yyyy-MM-dd") : "";
                string line = $"{car.Id},{car.Model},{car.Type},{car.PricePerDay},{car.IsAvailable},{car.RentedByClientId},{rentedFromStr},{car.RentedDays}";
                lines.Add(line);
            }

            FileHelper.RewriteCsv(CarsFile, lines);
        }
        static List<Client> LoadClients()
        {
            var clientList = new List<Client>();
            var lines = FileHelper.ReadCsvLines("clients.csv", "Id,Name,Phone,ID,Age");

            foreach (var parts in lines)
            {
                if (parts.Length < 5) continue;

                if (int.TryParse(parts[0], out int id) &&
                    int.TryParse(parts[4], out int age))
                {
                    clientList.Add(new Client(parts[1], parts[2], parts[3], age) { Id = id });
                }
            }

            return clientList;
        }
        static void SaveClients(List<Client> clientsToSave)
        {
            string clientsFile = "clients.csv";
            string header = "Id,Name,Phone,ID,Age";

            var lines = new List<string> { header };

            foreach (var cl in clientsToSave)
            {
                string line = $"{cl.Id},{cl.Name.Replace(",", "")},{cl.Phone.Replace(",", "")},{cl.ID.Replace(",", "")},{cl.Age}";
                lines.Add(line);
            }

            FileHelper.RewriteCsv(clientsFile, lines);
        }

    }
}
