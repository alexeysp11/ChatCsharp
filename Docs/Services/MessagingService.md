# MessagingService

Read this in other languages: [English](MessagingService.md), [Russian/Русский](MessagingService.ru.md).

Service for processing messages 

![SystemOverview](../img/SystemOverview.png)

## Description  

- Starting a conversation: 
    - User chooses another user to start a conversation;
    - Server queries and gets if these two users were in private chat already; 
    - Display all the info related to the private chat.
- Handles messages from the users:
    - Gets message from the sender;
    - Inserts the message into DB `MessagingDB`;
    - If receiver is online and not set "Notifications off" for the chat, then send the message: 
        - If the message is sent, then mark it as "sent".
        - If the message is no sent, then mark it as "pending", set receiver's status offline and send the info about the status using RabbitMQ to the **Last seen service**.
    - If receiver is offline, then mark the massage as "pending".
- Handles client's request for getting all messages in a chat (either all or that are appeared after the specified timestamp).
- Handles client's request for changing message status.
- Uses queues in RabbitMQ for communicating with **Last seen service** about the user's statuses (reading and writing).

## Network messages format

- Sending messages: 
    - Forward pass:
        - `message_uid: string` (only for receiver), 
        - `sender_uid: string`, 
        - `receiver_uid: string`, 
        - `text_content: string`, 
        - `send_timestamp: DateTime`,
        - `receiver_type_uid: string` (personal chat, group chat).
    - Backward pass (only for sender): 
        - `message_uid: string`,
        - `status_uid: string` (pending, sent).
- Getting messages:
    - Request: 
        - `to_use_timestamp: DateTime` (if needed, timestamp will be calculated on the server).
    - Response: 
        - collection of messages (see forward pass).
- Changing message status: 
    - `message_uid: string`,
    - `user_uid: string`,
    - `status_uid: string` (read, deleted_for_sender, deleted_for_everybody).

## Tables in DB

- user: 
    - `user_id: integer not null`, 
    - `user_uid: varchar not null`, 
    - `username: varchar not null`, 
    - `email: varchar`, 
    - `phone: varchar not null`, 
    - `image: blob`, 
    - `chat_entity_status_id: integer not null` -> chat_entity_status, 
    - `last_seen_timestamp: timestamp not null`.
- message: 
    - `message_id: integer not null`, 
    - `message_uid: varchar not null`,
    - `sender_id: integer not null` -> user, 
    - `receiver_id: integer` -> user, 
    - `conversation_id: integer` -> conversation,
    - `group_id: integer` -> group,
    - `chat_entity_type_id: integer not null` -> chat_entity_type,
    - `text_content: text`, 
    - `send_timestamp: timestamp not null`, 
    - `message_status_id: integer not null` -> message_status.
- change_status_request (for changing status of a message):
    - `message_id: integer not null` -> message, 
    - `user_id: integer not null` -> user,
    - `message_status_id: integer not null` -> message_status,
    - `request_timestamp: timestamp not null`.
- message_status: 
    - columns: 
        - `message_status_id: integer not null`,
        - `uid: varchar not null`,
        - `name: varchar not null`.
    - possible values: 
        - pending, 
        - sent, 
        - read, 
        - deleted_for_sender,
        - deleted_for_everybody.
- conversation: 
    - `conversation_id: integer not null`,
    - `uid: varchar not null`,
    - `user_one_id: integer not null` -> user,
    - `user_two_id: integer not null` -> user,
    - `user_one_np_id: integer not null` -> notification_policies,
    - `user_two_np_id: integer not null` -> notification_policies, 
    - `user_one_status_id: integer` -> chat_entity_status (shows if the **user one** was blocked by **user two**), 
    - `user_two_status_id: integer` -> chat_entity_status (shows if the **user two** was blocked by **user one**).
- chat_entity_type:
    - columns: 
        - `chat_entity_type_id: integer not null`,
        - `uid: varchar not null`,
        - `name: varchar not null`.
    - possible values: 
        - user,
        - personal chat (conversation), 
        - group chat,
        - bot.
- chat_entity_status: 
    - coulumns: 
        - `chat_entity_status_id: integer not null`, 
        - `uid: varchar not null`,
        - `name: varchar not null`.
    - possible values:
        - online, 
        - offline, 
        - blocked, 
        - deleted.
- notification_policies:
    - coulumns: 
        - `notification_policies_id: integer not null`, 
        - `uid: varchar not null`,
        - `name: varchar not null`.
    - possible values: 
        - notifications on,
        - notifications off.
- e2ee_algorithm:
    - coulumns: 
        - `e2ee_algorithm_id: integer not null`, 
        - `uid: varchar not null`,
        - `name: varchar not null`.
    - possible values: 
        - AES,
        - DES,
        - Tripple DES, 
        - RSA, 
        - Blowfish, 
        - Twofish, 
        - RC4,
        - TEA, 
        - xxTEA.
