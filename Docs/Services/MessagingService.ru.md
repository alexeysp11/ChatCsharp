# Messaging service

Доступно на других языках: [English/Английский](MessagingService.md), [Russian/Русский](MessagingService.ru.md).

Сервис обработки сообщений 

![SystemOverview](../img/SystemOverview.png)

## Описание 

- Обрабатывает сообщения от пользователей:
    - Получает сообщение от отправителя;
    - Записывает сообщение в БД `MessagingDB`;
    - Если получатель онлайн, то отправляем сообщение:
        - Если сообщение отправлено, то помечаем сообщение как "отправлено".
        - Если не получается отправить, то помечаем сообщение как "ожидающее отправку", ставим статус получателя оффлайн и отправляем информацию о статусе по RabbitMQ на сервис **Last seen service**.
    - Если получатель оффлайн, то помечаем сообщение как "ожидающее отправку".
- Обрабатывает клиентский запрос на получение всех сообщений (за всё время или после определенного времени).
- Клиентский запрос на изменение статуса сообщения.
- Использует очереди в RabbitMQ для коммуникации с сервисом **Last seen service** по поводу статуса пользователей (чтение и запись). 

## Описание сетевого взаимодействия 

- Отправка сообщения: 
    - Прямой ход:
        - message_uid (только для получателя), 
        - sender_uid, 
        - receiver_uid, 
        - text_content, 
        - send_timestamp.
    - Обратный ход (только для отправителя): 
        - message_uid,
        - status (unsent, sent).
- Получение сообщений:
    - Запрос: 
        - to_use_timestamp (при необходимости актуальный timestamp будет вычислен на сервере).
    - Ответ: 
        - массив сообщений (см. прямой ход).
- Изменение статуса сообщения: 
    - message_uid,
    - user_uid,
    - status (read, deleted_for_sender, deleted_for_everybody).

## Таблицы в БД

- user: 
    - user_id, 
    - user_uid, 
    - username, 
    - email, 
    - phone, 
    - user_status_id, 
    - last_seen_timestamp.
- message: 
    - message_id, 
    - message_uid,
    - sender_id, 
    - receiver_id, 
    - text_content, 
    - send_timestamp, 
    - message_status_id.
- status_request:
    - message_id, 
    - user_id,
    - message_status_id,
    - request_timestamp.
- message_status: 
    - unsent, 
    - sent, 
    - read, 
    - deleted_for_sender,
    - deleted_for_everybody.
- user_status: 
    - online, 
    - offline, 
    - blocked, 
    - deleted.
