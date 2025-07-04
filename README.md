# AutoriaBot – Курсовий проект

## Опис

**AutoriaBot** — це Telegram-бот та Web API, який дозволяє шукати, порівнювати та зберігати оголошення про продаж авто за заданими фільтрами, а також отримувати технічну інформацію по VIN-коду. Проект створено на основі ASP.NET Core Web API та C# із використанням публічних API та мокових даних.

---

## Основний функціонал

- Пошук авто за фільтрами: марка, модель, рік, місто, тип палива
- Вибір авто через шаблони запитів
- Збереження улюблених авто
- Перегляд історії пошуків
- Порівняння двох авто за характеристиками
- Декодування VIN-коду (через Моки)
- Очищення історії, збережених та порівняльного списку авто
- Користувацький інтерфейс через Telegram-бота (меню, кнопки)

---

## Як запустити проект локально

### Вимоги:
- .NET 8.0 SDK (або .NET 6.0, якщо змінено TargetFramework)
- Telegram Bot Token (отримати в @BotFather)
- [Необов’язково] Railway/Render/Azure для деплою

### Інструкція:

1. **Клонувати репозиторій:**
    ```bash
    git clone https://github.com/sablii-barbos/qursach.git
    cd qursach
    ```

2. **Налаштувати секрети:**
   - Вказати ключі Telegram, AutoRia, Google Maps у файлі `Constants.cs`:
     ```csharp
     public const string TelegramApiKey = "ВАШ_TG_TOKEN";
     public const string AutoRiaApiKey = "ВАШ_API_KEY";
     public const string GoogleMapsApiKey = "ВАШ_API_KEY";
     ```

3. **Встановити залежності та побудувати проект:**
    ```bash
    dotnet restore
    dotnet build
    ```

4. **Запустити Web API та Telegram-бота:**
    ```bash
    dotnet run --project ./ConsoleApp25/ConsoleApp25.csproj
    ```

---

## Як користуватись Telegram-ботом

- **/start** — відкрити головне меню
- **🔍 Пошук авто** — пошук через фільтри (марка, модель, рік, місто, паливо)
- **📑 Шаблони** — вибір із популярних шаблонів пошуку
- **📂 Збережені авто** — перегляд улюблених, видалення
- **🕘 Історія** — перегляд попередніх результатів пошуку, очищення історії
- **⚖️ Порівняння** — порівняння двох обраних авто
- **🌝 VIN-декодер** — ввести VIN, отримати розшифровку
- **🔙 Назад, 🏠 Головне меню** — зручна навігація по меню

---

## Основні методи Web API

- `POST /api/autoapi/search` — пошук авто за фільтрами (SearchFilters)
- `GET /api/autoapi/vin/{vin}` — декодування VIN через публічний API
- `PUT /api/autoapi/update` — (опціонально) оновлення даних авто
- `DELETE /api/autoapi/delete/{title}` — (опціонально) видалення авто

**Детальні приклади запитів — у Postman, або дивись код контролера.**

---



## Ліцензія

Навчальний проєкт. Для некомерційного використання.

---

## Автор

Саблій Остап, курсова робота, 2025  
НТУУ “КПІ ім. Ігоря Сікорського”
