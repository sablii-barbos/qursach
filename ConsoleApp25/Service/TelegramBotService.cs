using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InputFiles;
using AutoriaBot.Models;
using AutoriaBot.Services;
using AutoriaBot.UserData;
using System.Text;
using System.IO;
using AutoriaBotNew;

namespace AutoriaBot.Telegram;

public class TelegramBotService
{
    private readonly TelegramBotClient _bot;
    private readonly UserDataStore _store;
    private readonly VinDecoderService _vin;

    // ---------------- МОКИ ----------------
    private readonly List<AutoSummary> _mockCars = new()
    {
        new AutoSummary
        {
            Title = "Volkswagen Passat B8 2017",
            Year = 2017,
            Vin = "WVWZZZ3CZHE012345",
            Price = 15600,
            City = "Харків",
            Link = "https://auto.ria.com/fake-vwpassatb8",
            Description = "Volkswagen Passat B8, дизель 2.0 л, 150 к.с., автомат DSG. Авто свіжопригнане з Німеччини, ідеальний стан, без підкрасів. Повна сервісна історія, сервісна книжка. Зручний і місткий салон, клімат-контроль, парктроніки, датчик світла та дощу. Дуже економний двигун (5,2 л/100 км). Комплект нової зимової гуми у подарунок. Можливий торг біля авто!",
            FuelType = "Дизель",
            Gearbox = "Автомат",
            BodyType = "Універсал",
            Mileage = 110000,
            PhotoUrls = new() { "volkswagen_passat-b8.jpg" }
        },
        new AutoSummary
        {
            Title = "Toyota Camry 2019",
            Year = 2019,
            Vin = "JTNBF3HK103123456",
            Price = 21800,
            City = "Дніпро",
            Link = "https://auto.ria.com/fake-camry19",
            Description = "Toyota Camry Hybrid, 2.5 л, 218 к.с., гібрид, автомат. Купувалась новою в Україні, всі ТО у офіціалів. Один власник. Дуже економічна — витрата до 5,5 л/100 км, ідеально для міста. Стан ідеальний, салон без потертостей. Є Apple CarPlay, камера заднього огляду, підігрів сидінь, дистанційний запуск двигуна. Ходова не стукає, кузов без підфарбувань. Стан сів-поїхав, авто вкладень не потребує. Продаж у зв'язку з оновленням парку.",
            FuelType = "Гібрид",
            Gearbox = "Автомат",
            BodyType = "Седан",
            Mileage = 53000,
            PhotoUrls = new() { "toyota_camry.jpg" }
        },
        new AutoSummary
        {
            Title = "Tesla Model 3 2021",
            Year = 2021,
            Vin = "5YJ3E1EA7MF123456",
            Price = 36500,
            City = "Одеса",
            Link = "https://auto.ria.com/fake-tesla3",
            Description = "Tesla Model 3 Long Range, акумулятор 82 кВт·год, запас ходу до 550 км. Привезена з США без ДТП, батарея у відмінному стані. Комплектація — автопілот, преміум-інтер’єр, шкіряні сидіння, преміальна аудіосистема. Унікальна динаміка — розгін до 100 км/год за 4,6 сек. Збережена сервісна документація, два ключ-карти. Без зауважень по електроніці чи ходовій, працюють усі системи. Дуже комфортна для міста та дальніх поїздок. Надзвичайно дешеве обслуговування!",
            FuelType = "Електро",
            Gearbox = "Автомат",
            BodyType = "Седан",
            Mileage = 28000,
            PhotoUrls = new() { "tesla_model-3.jpg" }
        },
        new AutoSummary
        {
            Title = "Audi Q7 2018",
            Year = 2018,
            Vin = "WAUZZZ4M1JD123456",
            Price = 31500,
            City = "Львів",
            Link = "https://auto.ria.com/fake-audiq7",
            Description = "Audi Q7 quattro, повний привід, надійний дизельний двигун 3.0 TDI (249 к.с.). Пригнана з Німеччини у 2021 році, обслуговувалась лише на фірмовому сервісі. В авто — LED оптика, пневмопідвіска, панорамний дах, пам'ять сидінь, безключовий доступ, підігрів усіх сидінь. Без підфарбувань, перевірена товщиноміром. Є комплект нової зимової гуми. Витрата пального 7,5 л/100 км. Дуже чистий салон, не курили, авто не потребує вкладень — сіла родина та поїхала в подорож!",
            FuelType = "Дизель",
            Gearbox = "Автомат",
            BodyType = "Позашляховик",
            Mileage = 75000,
            PhotoUrls = new() { "audi_q7.jpg" }
        },
        new AutoSummary
        {
            Title = "BMW X5 2020",
            Year = 2020,
            Vin = "WBAKS01060G123456",
            Price = 28500,
            City = "Київ",
            Link = "https://auto.ria.com/fake-bmwx5",
            Description = "Автомобіль куплений у офіційного дилера, перший і єдиний власник. Без ДТП, рідний пробіг, сервісна історія доступна. Комплектація xDrive30d: шкіряний салон, мультимедіа Harman/Kardon, цифрова панель приладів, проекція на лобове скло, адаптивний круїз-контроль, круговий огляд, двозонний клімат-контроль. Двигун дизельний 3.0 л, 265 к.с., витрата пального 7 л/100 км. Мотор та коробка працюють ідеально, салон доглянутий, ходова не стукає. Нові літні шини, комплект зимових у подарунок. Ідеальний стан — сів і поїхав!",
            FuelType = "Дизель",
            Gearbox = "Автомат",
            BodyType = "Позашляховик",
            Mileage = 62000,
            PhotoUrls = new() { "bmw_x5.jpg" }
        }
    };

    private readonly List<(string Name, SearchFilters Filters)> _templates = new()
    {
        ("BMW X5 2020", new SearchFilters{ Brand = "BMW", Model = "X5", YearFrom = 2019, YearTo = 2021 }),
        ("Audi Q7 2018", new SearchFilters{ Brand = "Audi", Model = "Q7", YearFrom = 2017, YearTo = 2019 }),
        ("Tesla Model 3 2021", new SearchFilters{ Brand = "Tesla", Model = "Model 3", YearFrom = 2019, YearTo = 2021 }),
        ("Toyota Camry 2019", new SearchFilters{ Brand = "Toyota", Model = "Camry", YearFrom = 2017, YearTo = 2020 }),
        ("Volkswagen Passat B8 2017", new SearchFilters{ Brand = "Volkswagen", Model = "Passat", YearFrom = 2015, YearTo = 2018 })
    };

    private readonly List<string> _years = Enumerable.Range(2015, 10).Select(x => x.ToString()).ToList();
    private readonly List<string> _cities = new() { "Київ", "Львів", "Одеса", "Дніпро", "Харків" };
    private readonly List<string> _fuels = new() { "Бензин", "Дизель", "Електро", "Гібрид" };

    private enum FilterStep
    {
        None, Brand, Model, YearFrom, YearTo, City, Fuel
    }

    public TelegramBotService(
        AutoRiaService autoRia, GoogleMapsService maps, VinDecoderService vin, UserDataStore store)
    {
        _bot = new TelegramBotClient(token: Constants.TelegramApiKey);
        _store = store;
        _vin = vin;
        _bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
        Console.WriteLine("🤖 TelegramBotService запущено");
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            var msg = update.Message;
            var session = _store.GetOrCreateSession(msg.Chat.Id);

            // --- Головне меню або повернення назад ---
            if (msg.Text.StartsWith("/start") || msg.Text == "🏠 Головне меню")
            {
                session.Reset();
                await ShowMainMenu(msg.Chat.Id);
                return;
            }
            if (msg.Text == "🔙 Назад")
            {
                await GoBack(msg.Chat.Id, session);
                return;
            }

            switch (msg.Text)
            {
                case "🔍 Пошук авто":
                    session.State = FilterStep.Brand.ToString();
                    session.CurrentFilters = new SearchFilters();
                    await _bot.SendTextMessageAsync(msg.Chat.Id, "Введи марку авто:", replyMarkup: GetBackKeyboard());
                    break;
                case "🌝 VIN-декодер":
                    session.State = "vin";
                    await _bot.SendTextMessageAsync(msg.Chat.Id, "Введи VIN-код:", replyMarkup: GetBackKeyboard());
                    break;
                case "📂 Збережені авто":
                    await ShowSavedCars(msg.Chat.Id, session);
                    break;
                case "🕘 Історія":
                    await ShowHistory(msg.Chat.Id, session);
                    break;
                case "📑 Шаблони":
                    await ShowTemplates(msg.Chat.Id);
                    break;
                case "⚖️ Порівняння":
                    await ShowComparison(msg.Chat.Id, session);
                    break;
                case "🗑 Очистити історію":
                    session.History.Clear();
                    await _bot.SendTextMessageAsync(msg.Chat.Id, "Історію очищено.", replyMarkup: GetMainMenuKeyboard());
                    break;
                case "🗑 Очистити збережені":
                    session.SavedCars.Clear();
                    await _bot.SendTextMessageAsync(msg.Chat.Id, "Збережені авто очищено.", replyMarkup: GetMainMenuKeyboard());
                    break;
                case "🗑 Очистити порівняння":
                    session.ComparisonList.Clear();
                    await _bot.SendTextMessageAsync(msg.Chat.Id, "Список для порівняння очищено.", replyMarkup: GetMainMenuKeyboard());
                    break;
                default:
                    await HandleInput(msg.Chat.Id, msg.Text, session);
                    break;
            }
        }
        else if (update.Type == UpdateType.CallbackQuery)
        {
            var cb = update.CallbackQuery!;
            var session = _store.GetOrCreateSession(cb.Message.Chat.Id);
            await HandleCallback(cb, session);
        }
    }

    private async Task GoBack(long chatId, UserSession session)
    {
        switch (session.State)
        {
            case "Model":
                session.State = FilterStep.Brand.ToString();
                await _bot.SendTextMessageAsync(chatId, "Введи марку авто:", replyMarkup: GetBackKeyboard());
                break;
            case "YearFrom":
                session.State = FilterStep.Model.ToString();
                await _bot.SendTextMessageAsync(chatId, "Введи модель:", replyMarkup: GetBackKeyboard());
                break;
            case "YearTo":
                session.State = FilterStep.YearFrom.ToString();
                await _bot.SendTextMessageAsync(chatId, "Вибери рік ВІД:", replyMarkup: GetKeyboard(_years, true));
                break;
            case "City":
                session.State = FilterStep.YearTo.ToString();
                await _bot.SendTextMessageAsync(chatId, "Вибери рік ДО:", replyMarkup: GetKeyboard(_years, true));
                break;
            case "Fuel":
                session.State = FilterStep.City.ToString();
                await _bot.SendTextMessageAsync(chatId, "Вибери місто:", replyMarkup: GetKeyboard(_cities, true));
                break;
            default:
                session.State = null;
                await ShowMainMenu(chatId);
                break;
        }
    }

    private async Task HandleInput(long chatId, string text, UserSession session)
    {
        switch (session.State)
        {
            case "vin":
                var carVin = _mockCars.FirstOrDefault(x => string.Equals(x.Vin, text, StringComparison.OrdinalIgnoreCase));
                if (carVin != null)
                {
                    await SendFullAutoInfo(chatId, carVin, showButtons: false);
                }
                else
                {
                    var result = await _vin.DecodeVinAsync(text);
                    await _bot.SendTextMessageAsync(chatId, $"<b>Дані з VIN-Decoder:</b>\n<code>{result.Substring(0, Math.Min(400, result.Length))}...</code>", ParseMode.Html);
                }
                session.State = null;
                await ShowMainMenu(chatId);
                break;
            case "Brand":
                session.CurrentFilters.Brand = text;
                session.State = FilterStep.Model.ToString();
                await _bot.SendTextMessageAsync(chatId, "Введи модель:", replyMarkup: GetBackKeyboard());
                break;
            case "Model":
                session.CurrentFilters.Model = text;
                session.State = FilterStep.YearFrom.ToString();
                await _bot.SendTextMessageAsync(chatId, "Вибери рік ВІД:", replyMarkup: GetKeyboard(_years, true));
                break;
            case "YearFrom":
                if (_years.Contains(text))
                {
                    session.CurrentFilters.YearFrom = int.Parse(text);
                    session.State = FilterStep.YearTo.ToString();
                    await _bot.SendTextMessageAsync(chatId, "Вибери рік ДО:", replyMarkup: GetKeyboard(_years, true));
                }
                else
                {
                    await _bot.SendTextMessageAsync(chatId, "Оберіть рік зі списку.", replyMarkup: GetKeyboard(_years, true));
                }
                break;
            case "YearTo":
                if (_years.Contains(text))
                {
                    session.CurrentFilters.YearTo = int.Parse(text);
                    session.State = FilterStep.City.ToString();
                    await _bot.SendTextMessageAsync(chatId, "Вибери місто:", replyMarkup: GetKeyboard(_cities, true));
                }
                else
                {
                    await _bot.SendTextMessageAsync(chatId, "Оберіть рік зі списку.", replyMarkup: GetKeyboard(_years, true));
                }
                break;
            case "City":
                if (_cities.Contains(text))
                {
                    session.CurrentFilters.City = text;
                    session.State = FilterStep.Fuel.ToString();
                    await _bot.SendTextMessageAsync(chatId, "Вибери тип пального:", replyMarkup: GetKeyboard(_fuels, true));
                }
                else
                {
                    await _bot.SendTextMessageAsync(chatId, "Оберіть місто зі списку.", replyMarkup: GetKeyboard(_cities, true));
                }
                break;
            case "Fuel":
                if (_fuels.Contains(text))
                {
                    session.CurrentFilters.Fuel = text;
                    session.State = null;
                    await SearchAndShowResults(chatId, session.CurrentFilters, session, "main");
                    await ShowMainMenu(chatId);
                }
                else
                {
                    await _bot.SendTextMessageAsync(chatId, "Оберіть тип пального зі списку.", replyMarkup: GetKeyboard(_fuels, true));
                }
                break;
            default:
                await _bot.SendTextMessageAsync(chatId, "Не зрозумів. Вибери опцію з меню або введи /start.", replyMarkup: GetMainMenuKeyboard());
                break;
        }
    }

    private ReplyKeyboardMarkup GetKeyboard(List<string> values, bool withBack = false)
    {
        var rows = values.Select(x => new[] { new KeyboardButton(x) }).ToList();
        if (withBack) rows.Add(new[] { new KeyboardButton("🔙 Назад"), new KeyboardButton("🏠 Головне меню") });
        return new ReplyKeyboardMarkup(rows) { ResizeKeyboard = true, OneTimeKeyboard = true };
    }

    private ReplyKeyboardMarkup GetMainMenuKeyboard() => new(new[]
    {
        new[] { new KeyboardButton("🔍 Пошук авто"), new KeyboardButton("📑 Шаблони") },
        new[] { new KeyboardButton("📂 Збережені авто"), new KeyboardButton("🕘 Історія") },
        new[] { new KeyboardButton("⚖️ Порівняння"), new KeyboardButton("🌝 VIN-декодер") }
    })
    { ResizeKeyboard = true };

    private ReplyKeyboardMarkup GetBackKeyboard() =>
        new(new[]
        {
            new[] { new KeyboardButton("🔙 Назад"), new KeyboardButton("🏠 Головне меню") }
        })
        { ResizeKeyboard = true, OneTimeKeyboard = true };

    private async Task SearchAndShowResults(long chatId, SearchFilters filters, UserSession session, string context)
    {
        var cars = _mockCars.Where(c =>
            (string.IsNullOrEmpty(filters.Brand) || c.Title.ToLower().Contains(filters.Brand.ToLower())) &&
            (string.IsNullOrEmpty(filters.Model) || c.Title.ToLower().Contains(filters.Model.ToLower())) &&
            (filters.YearFrom == 0 || c.Year >= filters.YearFrom) &&
            (filters.YearTo == 0 || c.Year <= filters.YearTo) &&
            (string.IsNullOrEmpty(filters.City) || c.City.ToLower() == filters.City.ToLower()) &&
            (string.IsNullOrEmpty(filters.Fuel) || c.FuelType.ToLower() == filters.Fuel.ToLower())
        ).ToList();

        if (!cars.Any())
        {
            await _bot.SendTextMessageAsync(chatId, "Нічого не знайдено за цими параметрами.", replyMarkup: GetMainMenuKeyboard());
            return;
        }

        foreach (var car in cars)
        {
            session.History.Add(car);
            if (session.History.Count > 10)
                session.History.RemoveAt(0);

            await SendFullAutoInfo(chatId, car, showButtons: true);
        }
    }

    private async Task SendFullAutoInfo(long chatId, AutoSummary car, bool showButtons, InlineKeyboardMarkup? extraButtons = null)
    {
        var msg = new StringBuilder();
        msg.AppendLine($"<b>{car.Title}</b>");
        msg.AppendLine($"<b>Ціна:</b> <code>{car.Price}$</code>");
        msg.AppendLine($"<b>Місто:</b> {car.City}");
        msg.AppendLine($"<b>Тип:</b> {car.BodyType}");
        msg.AppendLine($"<b>Двигун:</b> {car.FuelType} {ParseLiters(car.Description)}");
        msg.AppendLine($"<b>Пробіг:</b> {car.Mileage} км");
        msg.AppendLine($"<b>Опис:</b> {car.Description}");
        msg.AppendLine($"<b>Силка:</b> <a href=\"{car.Link}\">Відкрити оголошення</a>");

        InlineKeyboardMarkup? buttons = null;
        if (showButtons)
        {
            buttons = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("💾 Зберегти", $"save|{car.Title}"),
                    InlineKeyboardButton.WithCallbackData("⚖️ Додати до порівняння", $"compare|{car.Title}")
                }
            });
        }
        else if (extraButtons != null)
        {
            buttons = extraButtons;
        }

        var photoPath = car.PhotoUrls.FirstOrDefault();
        if (!string.IsNullOrEmpty(photoPath) && System.IO.File.Exists(photoPath))
        {
            using (var stream = System.IO.File.OpenRead(photoPath))
            {
                var inputFile = new InputOnlineFile(stream, Path.GetFileName(photoPath));
                await _bot.SendPhotoAsync(
                    chatId,
                    inputFile,
                    msg.ToString(),
                    ParseMode.Html,
                    replyMarkup: buttons
                );
            }
        }
        else
        {
            await _bot.SendTextMessageAsync(chatId, msg.ToString(), ParseMode.Html, replyMarkup: buttons);
        }
    }

    private string ParseLiters(string desc)
    {
        var l = desc.ToLower().Split(' ').FirstOrDefault(x => x.Contains("л"));
        if (l != null && l.Length <= 5) return l;
        return "";
    }

    private async Task ShowTemplates(long chatId)
    {
        var buttons = _templates.Select(t => new[] { InlineKeyboardButton.WithCallbackData(t.Name, $"tpl|{t.Name}") }).ToArray();
        var markup = new InlineKeyboardMarkup(buttons);
        await _bot.SendTextMessageAsync(chatId, "Оберіть шаблон:", replyMarkup: markup);
    }

    private async Task HandleCallback(CallbackQuery cb, UserSession session)
    {
        if (cb.Data.StartsWith("save|"))
        {
            var title = cb.Data.Split('|')[1];
            var car = _mockCars.FirstOrDefault(c => c.Title == title);
            if (car != null && !session.SavedCars.Any(x => x.Title == car.Title))
            {
                session.SavedCars.Add(car);
                await _bot.AnswerCallbackQueryAsync(cb.Id, "Авто збережено!");
            }
            else
                await _bot.AnswerCallbackQueryAsync(cb.Id, "Вже в збережених!");
        }
        else if (cb.Data.StartsWith("compare|"))
        {
            var title = cb.Data.Split('|')[1];
            var car = _mockCars.FirstOrDefault(c => c.Title == title);
            if (car != null && !session.ComparisonList.Any(x => x.Title == car.Title) && session.ComparisonList.Count < 2)
            {
                session.ComparisonList.Add(car);
                await _bot.AnswerCallbackQueryAsync(cb.Id, "Додано до порівняння!");
            }
            else if (session.ComparisonList.Count >= 2)
                await _bot.AnswerCallbackQueryAsync(cb.Id, "Вже обрано 2 авто для порівняння.");
            else
                await _bot.AnswerCallbackQueryAsync(cb.Id, "Вже в списку для порівняння!");
        }
        else if (cb.Data.StartsWith("tpl|"))
        {
            var name = cb.Data.Split('|')[1];
            var tpl = _templates.FirstOrDefault(t => t.Name == name);
            if (tpl.Filters != null)
            {
                session.CurrentFilters = tpl.Filters;
                await SearchAndShowResults(cb.Message.Chat.Id, tpl.Filters, session, "template");
                await ShowMainMenu(cb.Message.Chat.Id);
            }
            await _bot.AnswerCallbackQueryAsync(cb.Id);
        }
        else if (cb.Data.StartsWith("remove_saved|"))
        {
            var title = cb.Data.Split('|')[1];
            var car = session.SavedCars.FirstOrDefault(c => c.Title == title);
            if (car != null)
            {
                session.SavedCars.Remove(car);
                await _bot.AnswerCallbackQueryAsync(cb.Id, "Видалено зі збережених!");
            }
        }
    }

    private async Task ShowHistory(long chatId, UserSession session)
    {
        if (!session.History.Any())
        {
            await _bot.SendTextMessageAsync(chatId, "Історія порожня.", replyMarkup: GetMainMenuKeyboard());
            return;
        }
        foreach (var car in session.History)
        {
            await SendFullAutoInfo(chatId, car, showButtons: false);
        }
        var kb = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("🗑 Очистити історію"), new KeyboardButton("🔍 Пошук авто") },
            new[] { new KeyboardButton("🏠 Головне меню") }
        })
        { ResizeKeyboard = true };
        await _bot.SendTextMessageAsync(chatId, "Дії з історією:", replyMarkup: kb);
    }

    private async Task ShowSavedCars(long chatId, UserSession session)
    {
        if (!session.SavedCars.Any())
        {
            await _bot.SendTextMessageAsync(chatId, "Немає збережених авто.", replyMarkup: GetMainMenuKeyboard());
            return;
        }
        foreach (var car in session.SavedCars)
        {
            var kb = new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("🗑 Видалити збережене авто", $"remove_saved|{car.Title}")
            );
            await SendFullAutoInfo(chatId, car, showButtons: false, kb);
        }
        var menu = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("🗑 Очистити збережені"), new KeyboardButton("🔍 Пошук авто") },
            new[] { new KeyboardButton("🏠 Головне меню") }
        })
        { ResizeKeyboard = true };
        await _bot.SendTextMessageAsync(chatId, "Дії збережених:", replyMarkup: menu);
    }

    private async Task ShowComparison(long chatId, UserSession session)
    {
        if (session.ComparisonList.Count < 2)
        {
            await _bot.SendTextMessageAsync(chatId, "Додайте 2 авто для порівняння через кнопку '⚖️ Додати до порівняння'.", replyMarkup: GetMainMenuKeyboard());
            return;
        }
        var c1 = session.ComparisonList[0];
        var c2 = session.ComparisonList[1];

        var sb = new StringBuilder();
        sb.AppendLine("<b>Порівняння авто:</b>");
        sb.AppendLine($"\n🔷 <b>{c1.Title}</b>");
        sb.AppendLine($"Ціна: {c1.Price}$\nПробіг: {c1.Mileage} км\nДвигун: {c1.FuelType} {ParseLiters(c1.Description)}\nМісто: {c1.City}\nКузов: {c1.BodyType}");
        sb.AppendLine($"\n🔶 <b>{c2.Title}</b>");
        sb.AppendLine($"Ціна: {c2.Price}$\nПробіг: {c2.Mileage} км\nДвигун: {c2.FuelType} {ParseLiters(c2.Description)}\nМісто: {c2.City}\nКузов: {c2.BodyType}");

        var kb = new ReplyKeyboardMarkup(new[]
        {
            new[] { new KeyboardButton("🗑 Очистити порівняння"), new KeyboardButton("🏠 Головне меню") }
        })
        { ResizeKeyboard = true };

        await _bot.SendTextMessageAsync(chatId, sb.ToString(), ParseMode.Html, replyMarkup: kb);

        // (Опціонально: можна також надіслати фото обох авто окремо)
        foreach (var car in session.ComparisonList)
            await SendFullAutoInfo(chatId, car, showButtons: false);
    }

    private async Task ShowMainMenu(long chatId)
    {
        await _bot.SendTextMessageAsync(chatId, "Вибери дію:", replyMarkup: GetMainMenuKeyboard());
    }

    private string FormatVinResult(string json)
    {
        return $"<b>VIN-інформація:</b>\n<code>{json.Substring(0, Math.Min(400, json.Length))}...</code>";
    }

    private Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken ct)
    {
        Console.WriteLine($"❌ ПОМИЛКА: {ex.Message}");
        return Task.CompletedTask;
    }
}
