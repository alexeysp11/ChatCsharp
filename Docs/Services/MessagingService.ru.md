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

- Отправка сообщения: 
    - Запрос: [MessageWF](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/MessageWF.md).
    - Ответ: [SendMsgResponseDTO](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/DTOs/SendMsgResponseDTO.md).
- Получение сообщений:
    - Запрос: [GetMsgRequestDTO](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/DTOs/GetMsgRequestDTO.md).
    - Ответ: [GetMsgResponseDTO](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/DTOs/GetMsgResponseDTO.md).
- Изменение статуса сообщения: 
    - Запрос: [SetMsgStatusRequestDTO](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/DTOs/SetMsgStatusRequestDTO.md).
    - Ответ: [SetMsgStatusResponseDTO](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/DTOs/SetMsgStatusResponseDTO.md).

## Таблицы в БД

- chat_user: [UserAccount](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/InformationSystem/UserAccount.md)
- chat_message: [MessageWF](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/MessageWF.md)
- chat_cms_request (change message status): [SetMsgStatusRequestDTO](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/DTOs/SetMsgStatusRequestDTO.md)
- chat_message_status: [MessageStatus](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/MessageStatus.md)
- chat_conversation: [Chatroom](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/Chatroom.md) and [ChannelUser](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/ChannelUser.md)
- chat_entity_type: [ChatroomType](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/ChatroomType.md)
- chat_entity_status: [UserStatus](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/InformationSystem/UserStatus.md)
- chat_notification_policies: [NotificationPolicies](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/Customers/NotificationPolicies.md)
- chat_e2ee_algorithm_type: [E2EEAlgorithmType](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Cryptography/E2EEAlgorithmType.md)
