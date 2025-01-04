# FileStorage API

<div align="center">

![Version](https://img.shields.io/badge/version-1.0.0-blue.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)
![.NET Core](https://img.shields.io/badge/.NET%20Core-6.0-purple.svg)

</div>

## 📋 Содержание

- [Описание](#описание)
- [Особенности](#особенности)
- [Технологии](#технологии)
- [Начало работы](#начало-работы)
- [API Endpoints](#api-endpoints)
- [Структура проекта](#структура-проекта)
- [Конфигурация](#конфигурация)
- [Тестирование](#тестирование)
- [Развертывание](#развертывание)
- [Участие в проекте](#участие-в-проекте)
- [Лицензия](#лицензия)

## 📝 Описание

FileStorage API - это современное, высокопроизводительное RESTful API для управления файлами, разработанное на платформе ASP.NET Core. Сервис предоставляет комплексное решение для хранения, управления и обработки файлов с поддержкой масштабирования и высокой доступности.

## ✨ Особенности

### Основные возможности

- **Управление файлами**
  - Загрузка одиночных файлов с валидацией
  - Пакетная загрузка множества файлов
  - Безопасное удаление файлов
  - Скачивание файлов с поддержкой возобновления
  - Получение метаданных файлов

### Дополнительные функции

- **Архивация**
  - Автоматическое создание ZIP-архивов для множественных загрузок
  - Настраиваемые параметры сжатия

- **Безопасность**
  - Валидация файлов по размеру и типу
  - Защита от вредоносных файлов
  - Поддержка CORS
  - Аутентификация и авторизация

- **Производительность**
  - Асинхронная обработка запросов
  - Потоковая передача данных
  - Кэширование метаданных
  - Оптимизированное использование памяти

## 🛠 Технологии

- **Backend**
  - ASP.NET Core 6.0
  - Entity Framework Core
  - AutoMapper
  - FluentValidation

- **Хранение данных**
  - PostgreSQL (хранение метаданных)
  - MinIO (объектное хранилище файлов)
  - Entity Framework Core Npgsql Provider

- **Документация**
  - Swagger/OpenAPI
  - XML-документация

## 🚀 Начало работы

### Предварительные требования

- .NET SDK 6.0 или выше
- PostgreSQL 14 или выше
- Git

### Установка

1. **Клонирование репозитория**
   ```bash
   git clone https://github.com/yourusername/FileStorage.git
   cd FileStorage
   ```

2. **Восстановление зависимостей**
   ```bash
   dotnet restore
   ```

3. **Настройка базы данных**
   ```bash
   dotnet ef database update
   ```

4. **Настройка конфигурации**
   - Скопируйте `appsettings.example.json` в `appsettings.json`
   - Настройте строки подключения и другие параметры

5. **Запуск приложения**
   ```bash
   dotnet run --project FileStorage.Application
   ```

## 📡 API Endpoints

### Файловые операции

#### Загрузка файла
```http
POST /api/filestorage/file
Content-Type: multipart/form-data

Parameters:
- file: File (required)
- description: string (optional)
```

#### Получение файла
```http
GET /api/filestorage/file/{id}
```

#### Удаление файла
```http
DELETE /api/filestorage/file/{id}
```

#### Информация о файле
```http
GET /api/filestorage/file/{id}/info
```

### Операции с коллекциями файлов

#### Загрузка нескольких файлов
```http
POST /api/filestorage/filecollection
Content-Type: multipart/form-data
```

#### Скачивание архива
```http
GET /api/filestorage/filecollection?ids={id1},{id2},{id3}
```

## 📁 Структура проекта

```
FileStorage/
├── src/
│   ├── FileStorage.Application/    # API и конфигурация
│   ├── FileStorage.Domain/         # Модели и интерфейсы
│   ├── FileStorage.Logic/          # Бизнес-логика
│   ├── FileStorage.Database/       # Доступ к данным
│   └── FileStorage.Common/         # Общие компоненты
├── tests/
│   ├── FileStorage.UnitTests/
│   └── FileStorage.IntegrationTests/
└── docs/                          # Документация
```

### Описание компонентов

- **FileStorage.Application**
  - Контроллеры API
  - Middleware
  - Конфигурация приложения
  - Dependency Injection

- **FileStorage.Domain**
  - Доменные модели
  - Интерфейсы
  - DTOs

- **FileStorage.Logic**
  - Сервисы
  - Валидаторы
  - Обработчики файлов

- **FileStorage.Database**
  - Entity Framework контекст
  - Репозитории
  - Миграции

## ⚙️ Конфигурация

### Основные настройки (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=filestorage;Username=postgres;Password=your_password"
  },
  "Minio": {
    "Endpoint": "localhost:9000",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin",
    "BucketName": "filestorage"
  },
  "FileStorage": {
    "MaxFileSize": 104857600,
    "AllowedExtensions": [".jpg", ".png", ".pdf"],
    "StoragePath": "uploads/"
  }
}
```

## 🧪 Тестирование

### Запуск тестов

```bash
# Запуск всех тестов
dotnet test

# Запуск конкретного проекта тестов
dotnet test FileStorage.UnitTests
```

### Типы тестов

- **Модульные тесты**: Проверка отдельных компонентов
- **Интеграционные тесты**: Проверка взаимодействия компонентов
- **End-to-End тесты**: Проверка полного цикла операций

## 📦 Развертывание

### Docker

```bash
# Сборка образа
docker build -t filestorage-api .

# Запуск контейнера
docker run -p 5206:80 filestorage-api
```

### Docker Compose

```yaml
version: '3.8'
services:
  api:
    build: .
    ports:
      - "5206:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=filestorage;Username=postgres;Password=your_password
      - Minio__Endpoint=minio:9000
      - Minio__AccessKey=minioadmin
      - Minio__SecretKey=minioadmin
      - Minio__BucketName=filestorage
    depends_on:
      - db
      - minio
  
  db:
    image: postgres:14-alpine
    environment:
      POSTGRES_DB: filestorage
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: your_password
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  minio:
    image: minio/minio
    ports:
      - "9000:9000"
      - "9001:9001"
    environment:
      MINIO_ROOT_USER: minioadmin
      MINIO_ROOT_PASSWORD: minioadmin
    volumes:
      - minio_data:/data
    command: server /data --console-address ":9001"

volumes:
  postgres_data:
  minio_data:
```

## 🤝 Участие в проекте

1. Форкните репозиторий
2. Создайте ветку для новой функции
3. Внесите изменения и создайте коммиты
4. Отправьте пулл-реквест

### Правила участия

- Следуйте существующему стилю кода
- Добавляйте тесты для новой функциональности
- Обновляйте документацию

## 📄 Лицензия

Copyright © 2024 FileStorage

Распространяется под лицензией MIT. Смотрите файл [LICENSE](LICENSE) для получения дополнительной информации.
