# Empire of Sports 2.0: Быстрый старт

## Чек-лист для начала разработки

### Неделя 1: Подготовка

#### День 1-2: Выбор технологий
- [ ] Определиться с движком:
  - **Unreal Engine 5** — если важна графика и есть опыт C++
  - **Unity** — если важна скорость разработки и C#
- [ ] Создать аккаунты:
  - [ ] Epic Games / Unity ID
  - [ ] GitHub (приватный репозиторий)
  - [ ] Photon Engine (бесплатный tier)
  - [ ] PlayFab (бесплатный tier до 100K пользователей)

#### День 3-4: Инструменты
- [ ] Установить движок
- [ ] Настроить IDE (Rider / VS Code / Visual Studio)
- [ ] Зарегистрироваться в AI-сервисах:
  - [ ] Meshy.ai (3D генерация)
  - [ ] Move AI или Remocapp (motion capture)
  - [ ] Midjourney/DALL-E (концепт-арт)

#### День 5-7: Концепт
- [ ] Выбрать первый вид спорта для прототипа (рекомендуется теннис — проще всего)
- [ ] Набросать GDD (Game Design Document) на 2-3 страницы:
  - Основные механики
  - Система прогрессии
  - Экономика
  - Целевая аудитория

---

### Неделя 2-4: Прототип

#### Базовый персонаж
```
Задачи:
- [ ] Создать/сгенерировать базовую 3D модель персонажа
- [ ] Настроить rigging (скелет)
- [ ] Импортировать в движок
- [ ] Базовое управление (WASD + мышь)
```

#### Первый вид спорта (Теннис)
```
Механики:
- [ ] Площадка (можно сгенерировать через AI)
- [ ] Мяч с физикой
- [ ] Удары (forehand, backhand, serve)
- [ ] Базовый AI-противник
- [ ] Счёт и раунды
```

---

## 🎾 Детальная реализация тенниса в Unity

### Почему теннис — лучший выбор для MVP

1. **Простая механика** — 2 игрока, 1 мяч, чёткие правила
2. **Минимум анимаций** — idle, run, 3-4 удара, подача
3. **Понятный networking** — синхронизация позиций + мяч
4. **Готовые референсы** — TopSpin, Virtua Tennis, Wii Sports

### Структура проекта Unity

```
Assets/
├── Scenes/
│   ├── MainMenu.unity
│   ├── TennisCourt.unity         ← Основная сцена
│   └── TrainingMode.unity
├── Scripts/
│   ├── Player/
│   │   ├── PlayerController.cs   ← Управление игроком
│   │   ├── PlayerAnimator.cs     ← Анимации
│   │   └── PlayerStats.cs        ← Характеристики
│   ├── Ball/
│   │   ├── TennisBall.cs         ← Физика мяча
│   │   └── BallTrajectory.cs     ← Траектория
│   ├── Court/
│   │   ├── CourtManager.cs       ← Логика корта
│   │   └── ScoreManager.cs       ← Счёт
│   ├── AI/
│   │   └── AIOpponent.cs         ← AI-противник
│   └── Network/
│       ├── NetworkManager.cs     ← Photon интеграция
│       └── PlayerSync.cs         ← Синхронизация
├── Prefabs/
│   ├── Player.prefab
│   ├── TennisBall.prefab
│   └── Court.prefab
├── Models/                        ← 3D модели (из Meshy/Mixamo)
├── Animations/                    ← Анимации (из Mixamo)
└── Materials/
```

### Шаг 1: Создание корта

#### 1.1 Размеры теннисного корта (реальные)
```
Длина: 23.77 м
Ширина (одиночный): 8.23 м
Ширина (парный): 10.97 м
Высота сетки: 0.914 м (центр), 1.07 м (столбы)
```

#### 1.2 Скрипт CourtManager.cs
```csharp
using UnityEngine;

public class CourtManager : MonoBehaviour
{
    [Header("Court Dimensions (Unity units = meters)")]
    public float courtLength = 23.77f;
    public float courtWidth = 8.23f;
    public float netHeight = 0.914f;

    [Header("References")]
    public Transform playerSpawnPoint;
    public Transform opponentSpawnPoint;
    public Transform netTransform;

    [Header("Boundaries")]
    public BoxCollider[] outOfBoundsColliders;

    void OnDrawGizmos()
    {
        // Визуализация корта в редакторе
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position,
            new Vector3(courtWidth, 0.1f, courtLength));

        // Сетка
        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position + Vector3.up * (netHeight / 2),
            new Vector3(courtWidth, netHeight, 0.05f));
    }
}
```

### Шаг 2: Физика мяча

#### 2.1 TennisBall.cs — основной скрипт
```csharp
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class TennisBall : MonoBehaviour
{
    [Header("Ball Properties")]
    public float mass = 0.057f;           // Реальная масса (57 грамм)
    public float radius = 0.0335f;        // Реальный радиус (3.35 см)
    public float bounciness = 0.75f;      // Коэффициент отскока
    public float drag = 0.1f;             // Сопротивление воздуха

    [Header("Spin")]
    public float topSpinMultiplier = 1.2f;
    public float backSpinMultiplier = 0.8f;

    private Rigidbody rb;
    private Vector3 spinDirection;
    private float spinAmount;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetupPhysics();
    }

    void SetupPhysics()
    {
        rb.mass = mass;
        rb.linearDamping = drag;
        rb.angularDamping = 0.05f;
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Настройка материала для отскока
        var sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;

        var physicsMaterial = new PhysicsMaterial("TennisBall");
        physicsMaterial.bounciness = bounciness;
        physicsMaterial.dynamicFriction = 0.5f;
        physicsMaterial.staticFriction = 0.5f;
        physicsMaterial.bounceCombine = PhysicsMaterialCombine.Multiply;
        sphereCollider.material = physicsMaterial;
    }

    public void Hit(Vector3 direction, float power, float spin = 0f)
    {
        // Базовая скорость: 20-50 м/с для тенниса
        float speed = Mathf.Lerp(20f, 50f, power);
        rb.linearVelocity = direction.normalized * speed;

        // Применяем спин
        spinAmount = spin;
        if (spin > 0)
            spinDirection = Vector3.Cross(direction, Vector3.up);
        else
            spinDirection = Vector3.Cross(Vector3.up, direction);
    }

    void FixedUpdate()
    {
        // Эффект Магнуса (влияние вращения на траекторию)
        if (spinAmount != 0 && rb.linearVelocity.magnitude > 1f)
        {
            Vector3 magnusForce = spinDirection * spinAmount * 0.5f;
            rb.AddForce(magnusForce, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Court"))
        {
            // Звук отскока
            // AudioManager.Instance.PlayBounce();

            // Уменьшаем спин при отскоке
            spinAmount *= 0.6f;
        }
    }
}
```

### Шаг 3: Управление игроком

#### 3.1 PlayerController.cs
```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float sprintMultiplier = 1.5f;
    public float rotationSpeed = 720f;

    [Header("Hitting")]
    public float hitRadius = 1.5f;          // Радиус, в котором можно ударить
    public Transform racketTransform;
    public LayerMask ballLayer;

    [Header("Shot Types")]
    public float minPower = 0.3f;
    public float maxPower = 1f;
    public float chargeTime = 1f;           // Время зарядки максимального удара

    private CharacterController controller;
    private Animator animator;
    private Vector2 moveInput;
    private bool isSprinting;
    private float chargeStartTime;
    private bool isCharging;
    private TennisBall targetBall;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        DetectBall();
        HandleHitting();
    }

    void HandleMovement()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
            float speed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);

            controller.Move(move * speed * Time.deltaTime);

            // Поворот в направлении движения
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetFloat("Speed", move.magnitude * speed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void DetectBall()
    {
        // Ищем мяч в радиусе удара
        Collider[] hits = Physics.OverlapSphere(
            racketTransform.position, hitRadius, ballLayer);

        targetBall = hits.Length > 0
            ? hits[0].GetComponent<TennisBall>()
            : null;

        // Подсветка, когда можно ударить
        // UIManager.Instance.ShowHitIndicator(targetBall != null);
    }

    void HandleHitting()
    {
        if (targetBall == null) return;

        if (isCharging)
        {
            float chargedTime = Time.time - chargeStartTime;
            float power = Mathf.Clamp01(chargedTime / chargeTime);
            // UIManager.Instance.ShowPowerMeter(power);
        }
    }

    // Input System callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed;
    }

    public void OnHit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            chargeStartTime = Time.time;
            isCharging = true;
        }
        else if (context.canceled && isCharging)
        {
            PerformHit();
            isCharging = false;
        }
    }

    void PerformHit()
    {
        if (targetBall == null) return;

        float chargedTime = Time.time - chargeStartTime;
        float power = Mathf.Clamp(chargedTime / chargeTime, minPower, maxPower);

        // Направление к противоположной стороне корта
        Vector3 aimPoint = GetAimPoint();
        Vector3 direction = (aimPoint - targetBall.transform.position).normalized;

        // Добавляем дугу
        direction.y = 0.3f + power * 0.2f;
        direction.Normalize();

        targetBall.Hit(direction, power);

        // Анимация удара
        animator.SetTrigger("Hit");
    }

    Vector3 GetAimPoint()
    {
        // Базовая точка — центр противоположной стороны
        // В будущем: добавить наведение мышью/стиком
        Vector3 aim = transform.position;
        aim.z = -aim.z; // Противоположная сторона
        aim.x += Random.Range(-3f, 3f); // Небольшой разброс
        return aim;
    }

    void OnDrawGizmosSelected()
    {
        if (racketTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(racketTransform.position, hitRadius);
        }
    }
}
```

### Шаг 4: AI-противник

#### 4.1 AIOpponent.cs
```csharp
using UnityEngine;

public class AIOpponent : MonoBehaviour
{
    [Header("AI Settings")]
    public float reactionTime = 0.3f;       // Время реакции
    public float moveSpeed = 7f;
    public float accuracy = 0.8f;           // 0-1, влияет на точность ударов
    public float hitRadius = 1.2f;

    [Header("Difficulty")]
    [Range(0, 1)] public float difficulty = 0.5f;

    private TennisBall ball;
    private CharacterController controller;
    private Animator animator;
    private Vector3 targetPosition;
    private float lastReactionTime;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        ball = FindObjectOfType<TennisBall>();
        targetPosition = transform.position;
    }

    void Update()
    {
        PredictBallPosition();
        MoveToTarget();
        TryHitBall();
    }

    void PredictBallPosition()
    {
        if (ball == null) return;

        // Обновляем цель с задержкой (имитация времени реакции)
        if (Time.time - lastReactionTime < reactionTime * (1 - difficulty))
            return;
        lastReactionTime = Time.time;

        Rigidbody ballRb = ball.GetComponent<Rigidbody>();

        // Простое предсказание: куда мяч прилетит на нашу сторону
        if (ballRb.linearVelocity.z < 0) // Мяч летит к нам
        {
            float timeToReach = Mathf.Abs(
                (transform.position.z - ball.transform.position.z) /
                ballRb.linearVelocity.z);

            Vector3 predictedPos = ball.transform.position +
                ballRb.linearVelocity * timeToReach;

            // Добавляем "ошибку" AI в зависимости от сложности
            float error = (1 - accuracy) * (1 - difficulty) * 2f;
            predictedPos.x += Random.Range(-error, error);

            targetPosition = new Vector3(
                predictedPos.x,
                transform.position.y,
                Mathf.Clamp(predictedPos.z, -11f, -2f) // Ограничиваем нашей половиной
            );
        }
        else
        {
            // Мяч улетает — возвращаемся в центр
            targetPosition = new Vector3(0, transform.position.y, -8f);
        }
    }

    void MoveToTarget()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.5f)
        {
            float adjustedSpeed = moveSpeed * (0.7f + difficulty * 0.6f);
            controller.Move(direction.normalized * adjustedSpeed * Time.deltaTime);
            animator.SetFloat("Speed", adjustedSpeed);

            // Поворот
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    void TryHitBall()
    {
        if (ball == null) return;

        float distance = Vector3.Distance(transform.position, ball.transform.position);

        if (distance < hitRadius)
        {
            // Определяем силу и направление
            float power = 0.5f + difficulty * 0.5f;

            // Целимся в случайную точку на стороне игрока
            Vector3 aimPoint = new Vector3(
                Random.Range(-3f, 3f) * accuracy,
                0,
                Random.Range(2f, 10f)
            );

            Vector3 direction = (aimPoint - ball.transform.position).normalized;
            direction.y = 0.25f + Random.Range(0, 0.2f);

            ball.Hit(direction.normalized, power);
            animator.SetTrigger("Hit");
        }
    }
}
```

### Шаг 5: Система счёта

#### 5.1 ScoreManager.cs
```csharp
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score")]
    public int playerPoints;
    public int opponentPoints;
    public int playerGames;
    public int opponentGames;
    public int playerSets;
    public int opponentSets;

    [Header("Tennis Scoring")]
    private readonly string[] pointNames = { "0", "15", "30", "40", "AD" };

    [Header("Events")]
    public UnityEvent<string, string> OnScoreChanged;
    public UnityEvent<string> OnGameWon;
    public UnityEvent<string> OnSetWon;
    public UnityEvent<string> OnMatchWon;

    void Awake()
    {
        Instance = this;
    }

    public void PlayerScores()
    {
        AddPoint(ref playerPoints, ref opponentPoints, "Player");
    }

    public void OpponentScores()
    {
        AddPoint(ref opponentPoints, ref playerPoints, "Opponent");
    }

    void AddPoint(ref int scorer, ref int other, string who)
    {
        // Deuce logic
        if (scorer >= 3 && other >= 3)
        {
            if (scorer == other) // Deuce
            {
                scorer++; // Advantage
            }
            else if (scorer > other) // Had advantage
            {
                WinGame(who);
            }
            else // Opponent had advantage
            {
                other--; // Back to deuce
            }
        }
        else
        {
            scorer++;
            if (scorer >= 4)
            {
                WinGame(who);
            }
        }

        UpdateScoreDisplay();
    }

    void WinGame(string who)
    {
        if (who == "Player")
            playerGames++;
        else
            opponentGames++;

        playerPoints = 0;
        opponentPoints = 0;

        OnGameWon?.Invoke(who);

        // Check for set win (6 games, 2 game lead)
        if ((playerGames >= 6 || opponentGames >= 6) &&
            Mathf.Abs(playerGames - opponentGames) >= 2)
        {
            WinSet(playerGames > opponentGames ? "Player" : "Opponent");
        }
    }

    void WinSet(string who)
    {
        if (who == "Player")
            playerSets++;
        else
            opponentSets++;

        playerGames = 0;
        opponentGames = 0;

        OnSetWon?.Invoke(who);

        // Best of 3 sets
        if (playerSets >= 2 || opponentSets >= 2)
        {
            OnMatchWon?.Invoke(playerSets > opponentSets ? "Player" : "Opponent");
        }
    }

    void UpdateScoreDisplay()
    {
        string playerScore = playerPoints < 4 ? pointNames[playerPoints] : "AD";
        string opponentScore = opponentPoints < 4 ? pointNames[opponentPoints] : "AD";

        OnScoreChanged?.Invoke(playerScore, opponentScore);
    }

    public string GetScoreText()
    {
        string p = playerPoints < 4 ? pointNames[playerPoints] : "AD";
        string o = opponentPoints < 4 ? pointNames[opponentPoints] : "AD";
        return $"{p} - {o}  |  Games: {playerGames}-{opponentGames}  |  Sets: {playerSets}-{opponentSets}";
    }
}
```

### Ресурсы для тенниса

#### Готовые проекты на GitHub

| Репозиторий | Описание | Ссылка |
|-------------|----------|--------|
| **devolfer/tennis-game-3d** | Простой 3D теннис с auto-hit зоной | [GitHub](https://github.com/devolfer/tennis-game-3d) |
| **princeofpython/tennis-unity** | Базовая реализация | [GitHub](https://github.com/princeofpython/tennis-unity) |
| **angel208/Unity-Tennis-3D-Game** | Теннис vs AI (Android) | [GitHub](https://github.com/angel208/Unity-Tennis-3D-Game) |

#### Unity Asset Store

| Ассет | Цена | Описание |
|-------|------|----------|
| Tennis Mobile - full game | ~$30 | Полный шаблон игры |
| Tennis Game Assets | ~$15 | 3D модели корта и ракеток |

#### Бесплатные ассеты

- **Mixamo** — бесплатные персонажи + анимации тенниса
- **Poly Haven** — HDRi для освещения, текстуры травы
- **Freesound.org** — звуки ударов, отскоков

#### Прототип networking
```
- [ ] Интегрировать Photon Fusion/PUN2
- [ ] Синхронизация позиций игроков
- [ ] Синхронизация мяча
- [ ] Тестирование в LAN
```

---

### Месяц 2-3: Вертикальный срез

#### Расширение геймплея
- [ ] Система прогрессии (XP, уровни)
- [ ] 3-5 типов ударов с разными характеристиками
- [ ] Кастомизация персонажа (3-5 опций)
- [ ] Matchmaking (через PlayFab)

#### Визуал
- [ ] Стилизованная графика или реализм
- [ ] UI/UX (главное меню, матч, результаты)
- [ ] Базовые анимации (idle, run, удары)

#### Инфраструктура
- [ ] Настроить CI/CD (GitHub Actions)
- [ ] Тестовый сервер (AWS/Google Cloud)
- [ ] Система логирования

---

## Бюджет первых 3 месяцев (минимальный)

| Статья | Стоимость/мес | Итого 3 мес |
|--------|---------------|-------------|
| Движок | Бесплатно | $0 |
| Photon (20 CCU) | Бесплатно | $0 |
| PlayFab (до 100K) | Бесплатно | $0 |
| Meshy.ai Pro | $60 | $180 |
| Move AI / Remocapp | ~$50 | $150 |
| Сервер для тестов | $50-100 | $150-300 |
| Домен + хостинг | $15 | $45 |
| **Итого** | | **$525-675** |

---

## Полезные ресурсы

### Туториалы
- [Unreal Engine 5 - Multiplayer Basics](https://dev.epicgames.com/documentation/en-us/unreal-engine/networking-and-multiplayer-in-unreal-engine)
- [Unity Netcode for GameObjects](https://docs-multiplayer.unity3d.com/)
- [Photon Fusion Tutorials](https://doc.photonengine.com/fusion/current/tutorials/tutorials)

### Ассеты (бесплатные/дешёвые)
- [Mixamo](https://www.mixamo.com/) — бесплатные анимации
- [Sketchfab](https://sketchfab.com/) — 3D модели
- [Kenney Assets](https://kenney.nl/assets) — бесплатные игровые ассеты
- [OpenGameArt](https://opengameart.org/) — звуки и графика

### AI-генерация
- [Meshy.ai](https://www.meshy.ai/) — 3D из текста
- [Move AI](https://www.move.ai/) — motion capture
- [Remocapp](https://remocapp.com/) — веб-камера mocap
- [Suno.ai](https://suno.ai/) — музыка

### Сообщества
- r/gamedev — общий геймдев
- r/unrealengine — Unreal комьюнити
- r/unity3d — Unity комьюнити
- Discord серверы движков

---

## Первые шаги прямо сейчас

### Если ты один:

1. **Установи Unity/Unreal** (сегодня)
2. **Пройди базовый туториал** движка (2-3 дня)
3. **Создай персонажа через Meshy.ai** (1 час)
4. **Сделай "running around" прототип** (неделя)
5. **Добавь мяч и удары** (неделя)
6. **Покажи друзьям, собери фидбек**

### Если есть дизайнер:

1. **Распределите роли:**
   - Ты: код, интеграции
   - Дизайнер: модели через AI, UI, концепты
2. **Параллельная работа:**
   - Ты делаешь механики
   - Дизайнер готовит ассеты
3. **Ежедневные синки** (15 минут)

### Если ищешь команду:

**Где искать:**
- [r/INAT](https://www.reddit.com/r/INAT/) — I Need A Team
- [r/gameDevClassifieds](https://www.reddit.com/r/gameDevClassifieds/)
- [Itch.io Jams](https://itch.io/jams) — game jams для нетворкинга
- Discord серверы геймдева
- GameDev.ru / DTF (русскоязычные)

**Кого искать в первую очередь:**
1. 3D-художник с опытом AI-генерации
2. Второй программист (для networking)
3. Геймдизайнер

---

## Метрики успеха прототипа

### Через 1 месяц:
- [ ] Можно играть в теннис против бота
- [ ] Персонаж анимирован
- [ ] Есть счёт

### Через 3 месяца:
- [ ] Можно играть 1v1 онлайн
- [ ] Есть система уровней
- [ ] UI выглядит прилично
- [ ] 5+ человек тестировали и дали фидбек

### Через 6 месяцев:
- [ ] 2-3 вида спорта
- [ ] Закрытая альфа (50-100 игроков)
- [ ] Первые метрики retention

---

## Частые ошибки (избегай!)

1. **Слишком большой scope** — начни с ОДНОГО вида спорта
2. **Перфекционизм** — выпускай ugly prototype быстро
3. **Кастомный движок** — используй готовые решения
4. **Игнорирование networking** — интегрируй его рано
5. **Отсутствие фидбека** — показывай игру людям с первых недель

---

## Контакты для вдохновения

### Indie MMO разработчики:
- Project Gorgon — [@projectgorgon](https://twitter.com/projectgorgon)
- Embers Adrift — сообщество на Discord

### Спортивные игры:
- Rocket League — изучи их подход к physics + networking
- TopSpin 2K25 — посмотри современный теннис

---

*Удачи в разработке! Начинай прямо сейчас.*
