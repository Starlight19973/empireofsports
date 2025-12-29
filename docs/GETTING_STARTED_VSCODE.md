# Empire of Sports 2.0: Начало работы в VS Code

## Для кого этот документ
Вы программист, новичок в Unity, работаете на Windows в VS Code. Первый спорт - теннис.

---

## Подготовка окружения

### Что уже должно быть установлено
- [x] Unity Hub
- [x] Unity Editor (рекомендуется Unity 6 LTS или Unity 2022.3 LTS)
- [x] VS Code
- [x] Расширения VS Code:
  - C# (Microsoft)
  - Unity (Microsoft)
  - C# Dev Kit (Microsoft)

### Проверка установки

```powershell
# Откройте PowerShell и выполните:
code --version
# Должна появиться версия VS Code
```

---

## Создание проекта Unity

### Шаг 1: Создать новый проект

1. Откройте Unity Hub
2. Projects → New project
3. Выберите шаблон: **3D (Built-in Render Pipeline)** или **3D (URP)**
   - Built-in: проще, быстрее для начала
   - URP: современнее, красивее графика
4. Настройки проекта:
   ```
   Project name: EmpireOfSports
   Location: C:\Projects\Unity\EmpireOfSports
   ```
5. Create project

### Шаг 2: Настроить VS Code как редактор

В Unity:
```
Edit → Preferences → External Tools
External Script Editor → Visual Studio Code
```

Проверьте галочки:
```
[x] Embedded packages
[x] Local packages
[x] Registry packages
[x] Built-in packages
[x] Generate .csproj files for:
```

### Шаг 3: Установить необходимые Unity пакеты

В Unity: **Window → Package Manager**

Установите:
```
[ ] Input System (новая система ввода для управления)
[ ] TextMeshPro (красивый текст UI)
[ ] Cinemachine (управление камерой)
[ ] ProBuilder (опционально - для быстрого прототипирования)
```

**Важно:** После установки Input System Unity попросит перезапуститься - соглашайтесь.

---

## Структура проекта

После создания проекта организуйте папки:

```
EmpireOfSports/
├── Assets/
│   ├── Scenes/                    # Сцены игры
│   │   ├── MainMenu.unity
│   │   ├── TennisCourt.unity
│   │   └── TrainingMode.unity
│   ├── Scripts/                   # Весь C# код
│   │   ├── Core/                  # Базовые системы
│   │   ├── Player/                # Логика игрока
│   │   ├── Sports/                # Спортивные механики
│   │   │   └── Tennis/            # Теннис
│   │   ├── Network/               # Multiplayer
│   │   ├── UI/                    # UI скрипты
│   │   └── Managers/              # Game managers
│   ├── Prefabs/                   # Заготовки объектов
│   ├── Materials/                 # Материалы
│   ├── Textures/                  # Текстуры
│   ├── Models/                    # 3D модели (из AI)
│   ├── Animations/                # Анимации (из Mixamo)
│   ├── Audio/                     # Звуки и музыка
│   └── Resources/                 # Динамически загружаемые ассеты
├── Packages/                      # Unity пакеты
├── ProjectSettings/               # Настройки проекта
└── docs/                          # Документация (этот файл)
```

Создайте эти папки в Unity:
```
Project panel → правый клик → Create → Folder
```

---

## Настройка Git

### Инициализация репозитория

```powershell
cd "C:\Empire  of sports"
git init
git add .
git commit -m "Initial commit: Unity project setup"
```

### .gitignore уже создан?

Проверьте наличие файла `.gitignore` в корне проекта. Если его нет, посмотрите содержимое в `docs/UNITY_SETUP_WINDOWS.md`.

---

## Первый запуск в VS Code

### Открыть проект

```powershell
cd "C:\Projects\Unity\EmpireOfSports"
code .
```

Или через VS Code: File → Open Folder → выберите папку проекта

### Настройка IntelliSense

При первом открытии:
1. VS Code предложит установить C# extension - соглашайтесь
2. Откройте любой .cs файл
3. Внизу должен появиться статус C# extension (может занять 1-2 минуты)

Если IntelliSense не работает:
```
Ctrl+Shift+P → OmniSharp: Select Project → выберите .sln файл
```

---

## Установка Claude Code в VS Code

### Шаг 1: Установить Node.js (если ещё нет)

Скачайте с [nodejs.org](https://nodejs.org/) (LTS версия)

### Шаг 2: Установить Claude Code CLI

```powershell
npm install -g @anthropic-ai/claude-code
```

Проверка:
```powershell
claude --version
```

### Шаг 3: Первый запуск и авторизация

```powershell
cd "C:\Empire  of sports"
claude
```

При первом запуске:
1. Откроется браузер
2. Войдите в аккаунт Claude (claude.ai)
3. Разрешите доступ
4. Вернитесь в терминал

### Шаг 4: Использование Claude Code

Откройте интегрированный терминал в VS Code (`` Ctrl+` ``) и запустите:

```powershell
claude
```

Теперь можно задавать вопросы:
```
> Объясни структуру Unity проекта
> Создай скрипт для движения персонажа WASD
> Как работает Input System в Unity?
```

---

## Первый скрипт: Hello Unity

### Создание через Unity

1. В Unity: Project → Assets → Scripts
2. Правый клик → Create → C# Script
3. Назовите: `HelloWorld`
4. Двойной клик - откроется в VS Code

### Код

```csharp
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Empire of Sports - Tennis prototype started!");
        Debug.Log($"Unity version: {Application.unityVersion}");
        Debug.Log($"Platform: {Application.platform}");
    }

    void Update()
    {
        // Нажмите Space для теста
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed! Ready to code tennis mechanics.");
        }
    }
}
```

### Применение

1. В Unity: Hierarchy → Create Empty → назовите "GameManager"
2. Перетащите HelloWorld.cs на GameManager
3. Нажмите Play
4. Откройте Console (Ctrl+Shift+C)
5. Должны увидеть сообщения
6. Нажмите Space - появится новое сообщение

**Работает? Отлично! Можно приступать к теннису.**

---

## Начало работы над теннисом

### Загрузить базовый код

Все скрипты для тенниса уже подготовлены в `docs/EMPIRE_OF_SPORTS_QUICKSTART.md`:
- TennisBall.cs
- PlayerController.cs
- AIOpponent.cs
- ScoreManager.cs
- CourtManager.cs

### План первой недели

#### День 1-2: Корт и мяч
```
[ ] Создать сцену TennisCourt.unity
[ ] Добавить плоскость (корт)
[ ] Создать CourtManager.cs и настроить размеры
[ ] Создать TennisBall.cs с физикой
[ ] Протестировать отскоки
```

#### День 3-4: Игрок
```
[ ] Скачать персонаж с Mixamo (бесплатно)
[ ] Импортировать в Unity
[ ] Создать PlayerController.cs
[ ] Настроить Input System
[ ] Тест: управление WASD
```

#### День 5-7: Первый удар
```
[ ] Добавить анимации ударов (Mixamo)
[ ] Реализовать механику удара
[ ] Настроить попадание по мячу
[ ] Базовая траектория полёта
```

### Ресурсы

**Бесплатные модели:**
- [Mixamo](https://www.mixamo.com/) - персонажи + анимации
- [Poly Haven](https://polyhaven.com/) - текстуры, HDRi
- [Sketchfab](https://sketchfab.com/feed) - поиск "tennis court" с Free license

**AI-генерация:**
- [Meshy.ai](https://www.meshy.ai/) - 200 бесплатных кредитов/мес
- [Tripo3D](https://www.tripo3d.ai/) - 150 бесплатных кредитов

---

## Работа с Claude Code для Unity

### Полезные промпты для начала

```
> Объясни как работает MonoBehaviour lifecycle в Unity

> Создай скрипт для теннисного мяча с реалистичной физикой

> Как настроить Input System для управления персонажем?

> Покажи пример синхронизации мяча через Photon Fusion

> Объясни разницу между Update() и FixedUpdate()
```

### Workflow с Claude Code

1. **Планирование фичи**
   ```
   > Я хочу добавить систему зарядки удара.
     Зажимаешь кнопку - растёт сила.
     Как это реализовать?
   ```

2. **Генерация кода**
   ```
   > Создай скрипт PowerMeter.cs для UI индикатора силы удара
   ```

3. **Отладка**
   ```
   > Мяч летит слишком высоко. Вот мой код TennisBall.cs [вставить код]
   ```

4. **Рефакторинг**
   ```
   > Посмотри PlayerController.cs и предложи как оптимизировать
   ```

---

## Debugging в VS Code

### Настройка отладчика

1. Установите расширение **Debugger for Unity** в VS Code
2. В Unity: Edit → Preferences → External Tools
   - Убедитесь что VS Code выбран
   - [x] Editor Attaching

### Использование

1. Поставьте breakpoint в VS Code (клик слева от номера строки)
2. В VS Code: Run → Start Debugging (F5)
3. Выберите "Unity Debugger"
4. В Unity нажмите Play
5. Код остановится на breakpoint

### Горячие клавиши

| Клавиша | Действие |
|---------|----------|
| F5 | Старт отладки |
| F9 | Поставить/убрать breakpoint |
| F10 | Step over |
| F11 | Step into |
| Shift+F5 | Остановить отладку |

---

## Частые проблемы и решения

### 1. IntelliSense не работает

**Решение:**
```
1. Закройте VS Code
2. В Unity: Assets → Open C# Project
3. Дождитесь загрузки
4. Ctrl+Shift+P → OmniSharp: Restart OmniSharp
```

### 2. Скрипт не компилируется в Unity

**Проверьте:**
- Имя файла совпадает с именем класса
- Класс наследуется от MonoBehaviour
- Нет синтаксических ошибок

**Как посмотреть ошибки:**
```
Unity → Console (Ctrl+Shift+C)
```

### 3. Изменения в скрипте не применяются

**Решение:**
```
1. Ctrl+S в VS Code (сохранить)
2. Вернитесь в Unity
3. Дождитесь компиляции (внизу справа)
4. Если не помогло: Assets → Refresh (Ctrl+R)
```

### 4. Claude Code не видит файлы проекта

**Решение:**
```powershell
# Запускайте Claude Code из корня проекта Unity
cd "C:\Projects\Unity\EmpireOfSports"
claude
```

---

## Полезные расширения VS Code для Unity

```
1. Unity Code Snippets - сниппеты для Unity
2. Unity Tools - дополнительные инструменты
3. Shader languages support - подсветка шейдеров
4. Todo Tree - TODO комментарии
5. GitLens - расширенный Git
```

Установка:
```
Ctrl+Shift+X → поиск → Install
```

---

## Горячие клавиши Unity + VS Code

### Unity Editor

| Клавиша | Действие |
|---------|----------|
| Ctrl+P | Play/Stop |
| Ctrl+Shift+P | Pause |
| W | Move tool |
| E | Rotate tool |
| R | Scale tool |
| F | Focus на объекте |
| Ctrl+D | Дублировать |
| Ctrl+S | Сохранить сцену |

### VS Code

| Клавиша | Действие |
|---------|----------|
| Ctrl+` | Открыть терминал |
| Ctrl+P | Быстрый поиск файлов |
| Ctrl+Shift+F | Поиск в проекте |
| F12 | Перейти к определению |
| Alt+Shift+F | Форматировать код |
| Ctrl+K Ctrl+C | Закомментировать |
| Ctrl+K Ctrl+U | Раскомментировать |

---

## Следующие шаги

После того как освоитесь с базовыми скриптами:

1. **Изучите документацию** в `docs/EMPIRE_OF_SPORTS_QUICKSTART.md`
   - Полный код тенниса с объяснениями
   - AI-противник
   - Система счёта

2. **Настройте Photon** для мультиплеера
   - Бесплатный tier: 100 CCU
   - Документация: [Photon Fusion](https://doc.photonengine.com/fusion/)

3. **Используйте AI для ассетов**
   - Сгенерируйте персонажей через Meshy.ai
   - Анимации через Mixamo
   - Концепт-арт через Midjourney

4. **Спрашивайте Claude Code**
   - Конкретные вопросы по Unity
   - Помощь с кодом
   - Рефакторинг и оптимизация

---

## Полезные ссылки

### Документация
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)
- [Input System Docs](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest)

### Туториалы для новичков
- [Unity Learn - Essentials](https://learn.unity.com/pathway/unity-essentials)
- [Brackeys YouTube](https://www.youtube.com/@Brackeys) - отличные туториалы
- [CodeMonkey](https://www.youtube.com/@CodeMonkeyUnity) - C# для Unity

### Сообщества
- r/Unity3D - Reddit комьюнити
- Unity Forum - официальный форум
- Unity Discord - живое общение

---

## Чек-лист готовности к разработке

```
[ ] Unity Hub установлен
[ ] Unity Editor установлен (версия 6 LTS или 2022.3 LTS)
[ ] VS Code установлен
[ ] Расширения C# + Unity установлены
[ ] Проект EmpireOfSports создан
[ ] VS Code настроен как External Editor в Unity
[ ] Git инициализирован
[ ] Claude Code установлен и авторизован
[ ] Первый скрипт HelloWorld.cs работает
[ ] Папки структурированы (Scripts, Prefabs, etc.)
[ ] Input System пакет установлен
[ ] Mixamo аккаунт создан (бесплатно)
[ ] Прочитан docs/EMPIRE_OF_SPORTS_QUICKSTART.md
```

---

**Готовы? Откройте терминал в VS Code и напишите:**

```powershell
claude
```

**Затем:**
```
> Я готов начать разработку тенниса для Empire of Sports.
  С чего мне начать создание корта и мяча?
```

Удачи! Вы создаёте что-то крутое.

---

*Документ создан: декабрь 2025*
*Для вопросов используйте Claude Code в терминале VS Code*
