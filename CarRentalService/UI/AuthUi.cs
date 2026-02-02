using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.UI
{
    class AuthUi
    {
        public static bool LoginDoWhile()
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
    }
}
