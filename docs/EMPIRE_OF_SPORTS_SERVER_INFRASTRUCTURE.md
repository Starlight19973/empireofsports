# Empire of Sports 2.0: Серверная инфраструктура и лицензии

## Содержание
1. [Обзор архитектуры](#обзор-архитектуры)
2. [Типы необходимых серверов](#типы-необходимых-серверов)
3. [Варианты хостинга](#варианты-хостинга)
4. [Лицензии и подписки](#лицензии-и-подписки)
5. [Безопасность (DDoS, SSL)](#безопасность)
6. [Базы данных](#базы-данных)
7. [Мониторинг и аналитика](#мониторинг-и-аналитика)
8. [Калькуляция по масштабам](#калькуляция-по-масштабам)
9. [Рекомендуемая архитектура](#рекомендуемая-архитектура)
10. [Чек-лист перед запуском](#чек-лист-перед-запуском)

---

## Обзор архитектуры

### Компоненты MMO-инфраструктуры

```
┌─────────────────────────────────────────────────────────────────┐
│                         ИГРОКИ                                  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    CDN + DDoS Protection                        │
│              (Cloudflare / AWS CloudFront)                      │
└─────────────────────────────────────────────────────────────────┘
                              │
              ┌───────────────┼───────────────┐
              ▼               ▼               ▼
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│   Web Server    │  │  Game Servers   │  │   API Server    │
│   (Frontend)    │  │   (Matches)     │  │   (Backend)     │
└─────────────────┘  └─────────────────┘  └─────────────────┘
              │               │               │
              └───────────────┼───────────────┘
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Backend Services                             │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐        │
│  │ PlayFab  │  │  Photon  │  │ Matchm.  │  │ Leaderb. │        │
│  │ (BaaS)   │  │ (Netcode)│  │          │  │          │        │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘        │
└─────────────────────────────────────────────────────────────────┘
                              │
              ┌───────────────┼───────────────┐
              ▼               ▼               ▼
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│   PostgreSQL    │  │     Redis       │  │  File Storage   │
│   (Persistent)  │  │   (Cache/RT)    │  │   (S3/GCS)      │
└─────────────────┘  └─────────────────┘  └─────────────────┘
```

---

## Типы необходимых серверов

### 1. Game Servers (Игровые серверы)
Обрабатывают реальный геймплей — матчи, физику, синхронизацию.

| Параметр | Рекомендации |
|----------|--------------|
| CPU | 4+ cores, высокая частота (4+ GHz) |
| RAM | 8-16 GB |
| Сеть | Низкая латентность, 1 Gbps+ |
| ОС | Linux (дешевле) или Windows |
| Количество | 1 сервер на ~50-200 CCU (зависит от игры) |

### 2. API/Backend Server
REST API, аутентификация, экономика, профили игроков.

| Параметр | Рекомендации |
|----------|--------------|
| CPU | 2-4 cores |
| RAM | 4-8 GB |
| Сеть | Стандартная |
| Масштабирование | Горизонтальное (несколько инстансов) |

### 3. Database Server
Хранение данных игроков, прогресса, транзакций.

| Параметр | Рекомендации |
|----------|--------------|
| CPU | 4+ cores |
| RAM | 16-32 GB (для кэширования) |
| Диск | SSD/NVMe, IOPS важны |
| Репликация | Master-Slave для надёжности |

### 4. Web Server
Сайт, лаунчер, статический контент.

| Параметр | Рекомендации |
|----------|--------------|
| CPU | 1-2 cores |
| RAM | 2-4 GB |
| Через CDN | Обязательно |

### 5. Redis/Cache Server
Кэш, сессии, real-time данные.

| Параметр | Рекомендации |
|----------|--------------|
| RAM | 2-8 GB (in-memory) |
| Persistence | Опционально |

---

## Варианты хостинга

### Сравнение провайдеров

#### Облачные платформы (рекомендуется для масштабирования)

| Провайдер | Плюсы | Минусы | Стоимость |
|-----------|-------|--------|-----------|
| **AWS GameLift** | Авто-масштабирование, Spot-инстансы (до 85% экономии), интеграция с UE5 | Сложная настройка | ~$1/игрок/мес при оптимизации |
| **Google Cloud + Agones** | Kubernetes, GKE Autopilot | Требует DevOps-экспертизы | Сопоставимо с AWS |
| **Azure PlayFab** | Интеграция с PlayFab, бесплатно до 100K пользователей | Привязка к экосистеме MS | $0 dev, pay-as-you-go prod |

#### Специализированные игровые платформы

| Платформа | Плюсы | Минусы | Стоимость |
|-----------|-------|--------|-----------|
| **Edgegap** | 615+ локаций, простая интеграция | Менее контроля | По запросу |
| **Multiplay (Unity)** | $800 бесплатного кредита | Закрывается в марте 2026 | Pay-as-you-go |
| **i3D.net** | Специализация на играх | Меньше автоматизации | По запросу |

#### VPS/Dedicated (для старта и тестирования)

| Провайдер | Минимальная цена | Особенности |
|-----------|------------------|-------------|
| **Hetzner** | ~€4/мес (VPS) | Лучшее соотношение цена/качество в EU |
| **OVH** | €3.49/мес (VPS) | Anti-DDoS включён, 30+ ДЦ |
| **Vultr** | $2.50/мес (VPS) | 30+ локаций, простой UI |
| **DigitalOcean** | $4/мес (Droplet) | Хорошая документация |
| **Linode** | $5/мес | Надёжность |

### Рекомендации по выбору

**Для MVP/Альфы (до 500 CCU):**
- VPS на Hetzner/OVH/Vultr
- 1-2 сервера
- Бюджет: $50-150/мес

**Для Беты (500-2000 CCU):**
- AWS GameLift или Google Cloud
- Photon Cloud
- Бюджет: $300-800/мес

**Для Production (2000+ CCU):**
- AWS GameLift с Spot-инстансами
- Kubernetes (Agones)
- Бюджет: $1000+/мес

---

## Лицензии и подписки

### Игровые движки

#### Unreal Engine 5
| Статус | Условия |
|--------|---------|
| **Бесплатно** | До $1M lifetime revenue |
| **Роялти** | 5% после $1M (кроме продаж в Epic Games Store) |
| **Серверы** | Dedicated servers — бесплатно |

#### Unity
| План | Стоимость | Условия |
|------|-----------|---------|
| Personal | Бесплатно | До $100K revenue/год |
| Plus | $399/год/seat | До $200K revenue/год |
| Pro | $2,040/год/seat | Без ограничений |

### Networking Middleware

#### Photon Engine

| План | CCU | Стоимость | Примечание |
|------|-----|-----------|------------|
| Free | 100 | $0 | Достаточно для ~40K MAU |
| Plus | 200 | $95/год | 100 paid + 100 free |
| Growth 500 | 500 | $125/мес | +1.5 TB трафика |
| Growth 1000 | 1000 | $250/мес | +3 TB трафика |
| Growth 2000 | 2000 | $500/мес | +6 TB трафика |
| Premium | 2000+ | $0.50/CCU | DDoS защита, SLA |

**Пример:** Stumble Guys платит ~$25,000/мес за 50,000 CCU (20M игроков/мес)

#### PlayFab (Azure)

| Режим | Пользователей | Стоимость |
|-------|---------------|-----------|
| Development | До 100K | Бесплатно |
| Pay-as-you-go | Без лимита | По метрикам |

**Бесплатная квота Multiplayer Servers:**
- 750 core-hours/мес (1x A1v2 VM)

### SSL-сертификаты

| Тип | Провайдер | Стоимость |
|-----|-----------|-----------|
| DV (Domain Validation) | Let's Encrypt | Бесплатно (90 дней) |
| DV | ZeroSSL | Бесплатно (3 сертификата) |
| DV Wildcard | ZeroSSL | $10/мес (подписка) |
| OV/EV (для платежей) | DigiCert, Sectigo | $50-300/год |

**Рекомендация:** Let's Encrypt + автообновление для игровых серверов

### Дополнительные лицензии

| Сервис | Стоимость | Назначение |
|--------|-----------|------------|
| FMOD (звук) | Бесплатно до $200K | Аудио-движок |
| Wwise (звук) | Бесплатно до $150K | Аудио-движок |
| SpeedTree | $19/мес | Растительность |
| Quixel Megascans | Бесплатно (UE5) | Ассеты |

---

## Безопасность

### DDoS-защита

| Решение | Бесплатно | Платно | Рекомендуется для |
|---------|-----------|--------|-------------------|
| **Cloudflare** | Да (unmetered L3/L4/L7) | $20-200/мес | Инди, стартапы |
| **AWS Shield Standard** | Да (базовый L3/L4) | — | Если уже на AWS |
| **AWS Shield Advanced** | — | $3,000/мес | Enterprise |
| **Azure DDoS** | — | $3,000-4,000/мес | Enterprise |
| **OVH Anti-DDoS** | Включено | — | Если хостинг на OVH |

**Важно:** DDoS-атаки выросли на 56% в 2024, средние потери — $6,130/минуту.

**Рекомендация для MVP:** Cloudflare Free + OVH/Hetzner

### CDN (Content Delivery Network)

| Провайдер | Бесплатный план | Платный | Для игр |
|-----------|-----------------|---------|---------|
| **Cloudflare** | Да (базовый) | $20/мес+ | Отлично |
| **AWS CloudFront** | 1M запросов + 100GB | $15/мес+ | Отлично |
| **Bunny CDN** | — | $0.01/GB | Дёшево для трафика |
| **KeyCDN** | — | $0.04/GB | Просто |

**Кейс:** Игровая компания снизила расходы на CDN с $3,000 до $1,700/мес (40% экономии) с CloudFront.

---

## Базы данных

### Рекомендуемый стек

```
PostgreSQL (основная БД)
    ↓
Redis (кэш + real-time)
    ↓
S3/GCS (файлы, ассеты)
```

### PostgreSQL (Managed)

| Провайдер | Минимум | Особенности |
|-----------|---------|-------------|
| **DigitalOcean** | $15/мес (1GB RAM, 10GB) | Просто |
| **Heroku** | $9/мес (Hobby) | Очень просто |
| **AWS RDS** | ~$15/мес (db.t3.micro) | Масштабируемо |
| **Supabase** | Бесплатно (500MB) | Postgres + API |
| **Neon** | Бесплатно (3GB) | Serverless |

### Redis (Managed)

| Провайдер | Минимум | Особенности |
|-----------|---------|-------------|
| **Redis Cloud** | Бесплатно (30MB) | Официальный |
| **Upstash** | Бесплатно (10K команд/день) | Serverless |
| **AWS ElastiCache** | ~$12/мес | Enterprise |
| **DigitalOcean** | $15/мес | Managed |

### Рекомендация для Roblox-масштаба
Roblox использует 400+ Redis-инстансов в продакшене для 48M игроков/мес.

---

## Мониторинг и аналитика

### Сравнение решений

| Решение | Стоимость | Для кого |
|---------|-----------|----------|
| **Grafana + Prometheus** | Бесплатно (self-hosted) | Если есть DevOps |
| **Grafana Cloud** | $0-200/мес | Managed, дешевле Datadog |
| **Datadog** | $15/хост/мес+ | Enterprise, быстро растёт цена |
| **New Relic** | Бесплатно (100GB/мес) | Хороший free tier |

### Что мониторить

1. **Серверы:** CPU, RAM, диск, сеть
2. **Игра:** CCU, матчи/мин, latency
3. **Бизнес:** DAU, retention, revenue
4. **Ошибки:** Crashes, disconnects

### Рекомендация для инди
- **Бесплатно:** Grafana Cloud Free или New Relic Free
- **Платно:** Grafana Cloud ($50-200/мес)

---

## Калькуляция по масштабам

### Сценарий 1: Soft Launch / Альфа
**100-500 CCU, ~5,000-20,000 MAU**

| Статья | Сервис | Стоимость/мес |
|--------|--------|---------------|
| Game Server | Hetzner VPS (4 vCPU, 16GB) | €15 |
| API Server | Hetzner VPS (2 vCPU, 4GB) | €6 |
| Database | DigitalOcean Managed Postgres | $15 |
| Redis | Upstash Free | $0 |
| Networking | Photon Free (100 CCU) | $0 |
| Backend | PlayFab Dev Mode | $0 |
| CDN + DDoS | Cloudflare Free | $0 |
| SSL | Let's Encrypt | $0 |
| Домен | — | $12/год |
| Мониторинг | Grafana Cloud Free | $0 |
| **ИТОГО** | | **~$50-80/мес** |

---

### Сценарий 2: Closed Beta
**500-2,000 CCU, ~20,000-80,000 MAU**

| Статья | Сервис | Стоимость/мес |
|--------|--------|---------------|
| Game Servers (x3) | Hetzner Dedicated (Ryzen) | €120 |
| API Server | Hetzner VPS (4 vCPU, 8GB) | €12 |
| Database | DigitalOcean (4GB RAM) | $60 |
| Redis | DigitalOcean Managed | $15 |
| Networking | Photon Growth 500 | $125 |
| Backend | PlayFab Pay-as-you-go | ~$50 |
| CDN | Cloudflare Pro | $20 |
| SSL | Let's Encrypt | $0 |
| Мониторинг | Grafana Cloud | $50 |
| Backup | S3/Backblaze | $20 |
| **ИТОГО** | | **~$450-550/мес** |

---

### Сценарий 3: Open Beta / Early Access
**2,000-5,000 CCU, ~100,000-200,000 MAU**

| Статья | Сервис | Стоимость/мес |
|--------|--------|---------------|
| Game Servers | AWS GameLift (Spot) | $400-600 |
| API Cluster | AWS ECS/EKS | $150-200 |
| Database | AWS RDS (Multi-AZ) | $200 |
| Redis | AWS ElastiCache | $100 |
| Networking | Photon Growth 2000 | $500 |
| Backend | PlayFab | $100-200 |
| CDN | CloudFront Pro | $200 |
| DDoS | Cloudflare Business | $200 |
| SSL | ACM (бесплатно на AWS) | $0 |
| Мониторинг | Datadog / Grafana | $200 |
| Backup & Storage | S3 | $50 |
| **ИТОГО** | | **~$2,000-2,500/мес** |

---

### Сценарий 4: Production Launch
**10,000+ CCU, 500,000+ MAU**

| Статья | Сервис | Стоимость/мес |
|--------|--------|---------------|
| Game Servers | AWS GameLift + Spot | $2,000-4,000 |
| API Cluster | EKS (multi-region) | $500-1,000 |
| Database | RDS (provisioned) | $500-1,000 |
| Redis Cluster | ElastiCache | $300-500 |
| Networking | Photon Premium | $5,000+ |
| Backend | PlayFab Enterprise | $500+ |
| CDN | CloudFront | $500-1,000 |
| DDoS | AWS Shield Advanced | $3,000 |
| Мониторинг | Datadog | $500+ |
| DevOps (человек) | — | $5,000-10,000 |
| **ИТОГО** | | **$15,000-25,000/мес** |

---

## Рекомендуемая архитектура

### Для MVP (минимум)

```
[Cloudflare Free]
       │
       ▼
[Hetzner VPS - Game + API]
       │
       ├── [DigitalOcean Postgres]
       ├── [Upstash Redis Free]
       └── [Photon Free 100 CCU]
```

**Стоимость:** ~$50/мес
**Поддерживает:** До 100 CCU / ~4,000 MAU

---

### Для Beta (рекомендуется)

```
[Cloudflare Pro]
       │
       ▼
[Load Balancer]
       │
   ┌───┴───┐
   ▼       ▼
[Game 1] [Game 2]  ← Hetzner/OVH Dedicated
       │
       ├── [Managed Postgres] ← DigitalOcean
       ├── [Managed Redis] ← DigitalOcean
       ├── [Photon Cloud] ← 500-2000 CCU
       └── [PlayFab] ← BaaS
```

**Стоимость:** ~$500/мес
**Поддерживает:** До 2,000 CCU / ~80,000 MAU

---

### Для Production

```
[Cloudflare Enterprise / AWS Shield]
              │
              ▼
[AWS Global Accelerator]
              │
    ┌─────────┼─────────┐
    ▼         ▼         ▼
[EU Region] [US Region] [Asia Region]
    │
    ├── [GameLift Fleet + Agones]
    │       └── [Spot Instances]
    ├── [EKS API Cluster]
    ├── [RDS Multi-AZ Postgres]
    ├── [ElastiCache Redis Cluster]
    ├── [Photon Premium]
    └── [S3 + CloudFront]
```

**Стоимость:** $10,000+/мес
**Поддерживает:** 10,000+ CCU

---

## Чек-лист перед запуском

### Инфраструктура
- [ ] Game servers настроены и протестированы
- [ ] Database с бэкапами и репликацией
- [ ] Redis для кэша и сессий
- [ ] Load balancer настроен
- [ ] Auto-scaling протестирован

### Безопасность
- [ ] SSL-сертификаты установлены
- [ ] DDoS-защита активна
- [ ] Firewall правила настроены
- [ ] Secrets хранятся безопасно (AWS Secrets Manager, Vault)
- [ ] Rate limiting на API

### Мониторинг
- [ ] Метрики серверов собираются
- [ ] Алерты настроены (CPU > 80%, память, диск)
- [ ] Логи централизованы
- [ ] Uptime monitoring (UptimeRobot, Pingdom)

### Бэкапы
- [ ] Автоматические бэкапы БД (ежедневно)
- [ ] Тестирование восстановления
- [ ] Off-site storage (S3, GCS)

### Документация
- [ ] Runbook для инцидентов
- [ ] Схема инфраструктуры
- [ ] Процедуры деплоя
- [ ] Контакты команды

### Юридическое
- [ ] Политика конфиденциальности
- [ ] Условия использования
- [ ] GDPR compliance (для EU)
- [ ] EULA для игры

---

## Полезные ссылки

### Калькуляторы
- [AWS Pricing Calculator](https://calculator.aws/)
- [Google Cloud Calculator](https://cloud.google.com/products/calculator)
- [PlayFab MPS Calculator](https://playfab.com/mps-calculator/)
- [Photon Pricing](https://www.photonengine.com/fusion/pricing)

### Документация
- [AWS GameLift](https://docs.aws.amazon.com/gamelift/)
- [Agones (Kubernetes)](https://agones.dev/site/docs/)
- [Photon Fusion Docs](https://doc.photonengine.com/fusion/)
- [PlayFab Docs](https://learn.microsoft.com/en-us/gaming/playfab/)

### Инструменты
- [Terraform](https://www.terraform.io/) — Infrastructure as Code
- [Pulumi](https://www.pulumi.com/) — IaC альтернатива
- [Ansible](https://www.ansible.com/) — Configuration management
- [GitHub Actions](https://github.com/features/actions) — CI/CD

---

## Итоговая таблица затрат

| Этап | CCU | MAU | Серверы | Общий бюджет/мес |
|------|-----|-----|---------|------------------|
| MVP / Альфа | 100-500 | 5K-20K | 1-2 VPS | $50-100 |
| Closed Beta | 500-2K | 20K-80K | 3-5 серверов | $400-600 |
| Open Beta | 2K-5K | 100K-200K | Облако | $2,000-3,000 |
| Launch | 5K-10K | 200K-500K | Multi-region | $5,000-10,000 |
| Growth | 10K+ | 500K+ | Global | $15,000+ |

---

*Документ подготовлен: декабрь 2025*
*Цены актуальны на момент написания и могут измениться*
