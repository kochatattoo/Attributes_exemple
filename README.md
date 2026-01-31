# Hero Attributes & Inventory System (UniRx + Addressables)

Профессиональная система характеристик персонажа, экипировки и временных эффектов для RPG на Unity.
Демо проект представляет собой вариант использования системы атрибутов
Представляю меню создания-распределения очков атрибутов героя и дальнейшая система управления экипировкой / бафами 

## Основные возможности
- **Реактивность:** Весь UI обновляется мгновенно через `UniRx`.
- **Сложные статы:** Поддержка базовых значений, плоских и процентных модификаторов.
- **Экипировка:** Система слотов (Head, Chest, Weapon) с автоматической заменой предметов.
- **Временные эффекты:** Баффы и дебаффы с визуальными таймерами и логикой отката.
- **Static Data:** Загрузка данных через Addressables и централизованная база ScriptableObjects.

## Характеристики и Формулы

### Базовые атрибуты
| Атрибут | Ключ | Влияние на параметры |
| :--- | :--- | :--- |
| **Сила** | `str` | Атака (x2) |
| **Ловкость** | `agi` | Защита (x2) |
| **Интеллект** | `inte` | Мана (x5) |
| **Мудрость** | `wis` | Уклонение (x2) |
| **Выносливость** | `stm` | Здоровье (x10), Сопротивление (x2) |
<img width="1108" height="628" alt="Снимок экрана 2026-01-31 212551" src="https://github.com/user-attachments/assets/7068dae4-4c37-4539-8f72-e82ac5e70c6d" />

### Механика расчета (FinalValue) **Гибридная логика процентов**
Система использует трехэтапный расчет для исключения ошибок прогрессии:
1. Суммируются все **Flat** модификаторы к базе.
2. Применяется сумма всех положительный **Percentage** модификаторов.
3. Применяем мультипликативное значение отрицательных модификаторов **Percentage** к сумме положительных занчений


 **Положительные бонусы (Buffs):** Применяются **аддитивно**. 
    *   *Пример:* Два баффа по +15% дадут суммарный бонус +30%.
    *   *Формула:* `Value * (1 + 0.15 + 0.15) = Value * 1.3`.
 **Отрицательные эффекты (Debuffs):** Применяются **мультипликативно**. Это гарантирует, что характеристика не упадет до 0 при двух эффектах по -50%.
    *   *Пример:* Два яда по -50% уменьшат стат сначала до 50%, а затем до 25% от исходного.
    *   *Формула:* `Value * 0.5 * 0.5 = Value * 0.25`.

**Итоговая цепочка:** `(Base + Flat) -> Сложение баффов -> Перемножение дебаффов`.

## Временные эффекты (Buffs/Debuffs)
- **Применение:** Каждый расходник создает независимый экземпляр `ActiveBuff`.
- **Stacking:** Эффекты суммируются. Два "Яда" на -50% снизят характеристику до нуля.
- **Визуал:** Иконки имеют цветовую рамку (Зеленая — бафф, Красная — дебафф) и мигающий таймер, если осталось < 2 сек.
<img width="1572" height="891" alt="Снимок экрана 2026-01-31 212611" src="https://github.com/user-attachments/assets/7aed6cd0-ccc9-4d38-a7e8-c28b2cc34f04" />
<img width="1575" height="877" alt="Снимок экрана 2026-01-31 212619" src="https://github.com/user-attachments/assets/ffc4b2b9-62cf-4fc5-8977-d9075df1d327" />

## Инструкция: Создание нового предмета

Для добавления контента в игру не нужно писать код. Используйте Asset Menu:

1. **Создание предмета:**
   - Нажмите ПКМ в проекте -> `Create` -> `Items` -> `Equipment` (или `Consumable`).
   - Настройте **Slot** (для экипировки) или **Duration** (для расходников).
   - В списке **Modifiers** выберите атрибут из выпадающего списка и укажите значение.
<img width="463" height="490" alt="Снимок экрана 2026-01-31 212755" src="https://github.com/user-attachments/assets/cbceb43b-ac5b-42a3-81b2-5a11304eb416" />
<img width="461" height="508" alt="Снимок экрана 2026-01-31 212801" src="https://github.com/user-attachments/assets/cbfffaad-e6a7-4038-adba-305469937880" />

**В разработке**
2. **Регистрация в базе:**
   - Найдите файл `ItemDatabase` (ScriptableObject).
   - Перетащите ваш новый предмет в список `All Items`.

3. **Обновление в игре:**
   - Благодаря Addressables и StaticDataService, предмет появится в инвентаре автоматически при следующем запуске.

---
##  Структура системы атрибутов

CodeBase проекта располагается по пути : [**Code**](https://github.com/kochatattoo/Attributes_exemple/tree/master/Assets/Project/Code)
Все ключевые файлы системы расположены по пути: 
`Assets/Project/Code/Hero/Attributes` [**Attributes**](https://github.com/kochatattoo/Attributes_exemple/tree/master/Assets/Project/Code/Hero/Attributes)

### Ключевые классы:
*   [**Attribute.cs**](https://github.com/kochatattoo/Attributes_exemple/tree/master/Assets/Project/Code/Hero/Attributes) — Ядро системы. Содержит логику расчета `FinalValue`, обработку модификаторов и гибридную формулу процентов.
*   [**HeroAttributes.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/Hero/Attributes/HeroAttributes.cs) — Контейнер-словарь всех атрибутов героя. Обеспечивает доступ через индексатор и управление модификаторами.
*   [**AttributeModifier.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/Hero/Attributes/AttributeModifier.cs) — Модель данных модификатора (значение + тип: Flat/Percentage).
*   [**AttributeConstants.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/Hero/Attributes/AttributeConstants.cs) — Статические ключи-строки для доступа к характеристикам (`str`, `agi` и т.д.). Методы расширения для преобразования Enum в системные строки и красивые названия для UI.

### UI классы:
**Window**
*   [**NewGameWindow.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/Windows/NewGameWindow.cs) — Класс фасад окна Новой Игры в главном меню.
*   [**Demoindow.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/Windows/DemoWindow.cs) — Класс фасад окна 'DemoScene' в DemoScene для отображения работоспособности системы.

**Elements**
*   [**DistributePoints.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/MainMenuElements/DistributePoints.cs) — UI класс, отвечающий за распределение очков атрибутов.
*   [**HeroAttributesView.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/MainMenuElements/HeroAttributesView.cs) — UI класс-контейнер 'DistributePoints'.
*   [**HeroClassContainerUI.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/MainMenuElements/HeroClassContainerUI.cs) — UI класс, хранит в себе иконки HeroClassUI.
*   [**HeroClassUI.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/MainMenuElements/HeroClassUI.cs) — UI класс, отвечающий за взаимодействия выбора героя.
*   [**HeroClassDescriptionUI.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/MainMenuElements/HeroClassDescriptionUI.cs) — UI класс, показывающий описание героя в соответствующем текстовом поле. 
*   [**HeroParamentresUI.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/MainMenuElements/HeroParamentresUI.cs) — UI класс для отображения рассчитаных параметров от атрибутов класса/героя.
*   [**HeroAttributesUI.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/HeroWindow/HeroAttributesUI.cs) — UI класс для отображения атрибутов класса/героя.
*   [**HeroTimeEffectsUI.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/HeroWindow/HeroTimeEffectsUI.cs) — UI класс отображающий временные эффекты (баффы/дебаффы)
*   [**HeroWindowUI.cs**](https://github.com/kochatattoo/Attributes_exemple/blob/master/Assets/Project/Code/UI/HeroWindow/HeroWindowUI.cs) — UI класс контейнер для отображения параметров героя.

## Технологический стек системы
- **Unity 6.2+**
- **UniRx** — асинхронные операции и реактивные данные.
- **Addressables** — управление ресурсами и памятью.
- **TextMeshPro** — качественное отображение текстов.
