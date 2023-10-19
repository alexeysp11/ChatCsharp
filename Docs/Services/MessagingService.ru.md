# MessagingService

Доступно на других языках: [English/Английский](MessagingService.md), [Russian/Русский](MessagingService.ru.md).

Сервис обработки сообщений 

![SystemOverview](../img/SystemOverview.png)

## Описание 

- Начало диалог:
    - Пользователь выбирает другого пользователя, чтобы начать диалог;
    - Сервер запрашивает информацию о том, были ли эти два пользователя уже в приватном чате;
    - Отображать всю информацию, связанную с приватным чатом.
- Обрабатывает сообщения от пользователей:
    - Получает сообщение от отправителя;
    - Записывает сообщение в БД `MessagingDB`;
    - Если получатель онлайн и не проставлено "Отключить уведомления" для этого чата, то отправляем сообщение:
        - Если сообщение отправлено, то помечаем сообщение как "отправлено".
        - Если не получается отправить, то помечаем сообщение как "ожидающее отправку", ставим статус получателя оффлайн и отправляем информацию о статусе по RabbitMQ на сервис **Last seen service**.
    - Если получатель оффлайн, то помечаем сообщение как "ожидающее отправку".
- Обрабатывает клиентский запрос на получение всех сообщений (за всё время или после определенного времени).
- Клиентский запрос на изменение статуса сообщения.
- Использует очереди в RabbitMQ для коммуникации с сервисом **Last seen service** по поводу статуса пользователей (чтение и запись). 

## Описание сетевого взаимодействия 

За обработку сетевых запросов ответственен класс [ChatMessagingService](../Core/Services/ChatMessagingService.md).

## Таблицы в БД

- [chat_user](../DbTables/chat_user.md)
- [chat_message](../DbTables/chat_message.md)
- [chat_cms_request](../DbTables/chat_cms_request.md) (change message status)
- [chat_message_status](../DbTables/chat_message_status.md)
- [chat_conversation](../DbTables/chat_conversation.md)
- [chat_entity_type](../DbTables/chat_entity_type.md)
- [chat_entity_status](../DbTables/chat_entity_status.md)
- [chat_notification_policies](../DbTables/chat_notification_policies.md)
- [chat_e2ee_algorithm_type](../DbTables/chat_e2ee_algorithm_type.md)
