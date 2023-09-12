# Messaging service

Read this in other languages: [English](MessagingService.md), [Russian/Русский](MessagingService.ru.md).

Service for processing messages 

![SystemOverview](../img/SystemOverview.png)

## Description  

- Handles messages from the users:
    - Gets message from the sender;
    - Inserts the message into DB `MessagingDB`;
    - If receiver is online, then send the message: 
        - If the message is sent, then mark it as "sent".
        - If the message is no sent, then mark it as "unsent", set receiver's status offline and send the info about the status using RabbitMQ to the **Last seen service**.
    - If receiver is offline, then mark the massage as "pending".
- Handles client's request for getting all messages (either all or that are appeared after the specified timestamp).
- Handles client's request for changing message status.
- Uses queues in RabbitMQ for communicating with **Last seen service** about the user's statuses (reading and writing).

## Network messages format

- Sending messages: 
    - Forward pass:
        - message_uid (only for receiver), 
        - sender_uid, 
        - receiver_uid, 
        - text_content, 
        - send_timestamp.
    - Backward pass (only for sender): 
        - message_uid,
        - status (unsent, sent).
- Getting messages:
    - Request: 
        - to_use_timestamp (if needed, timestamp will be calculated on the server).
    - Response: 
        - collection of messages (see forward pass).
- Changing message status: 
    - message_uid,
    - user_uid,
    - status (read, deleted_for_sender, deleted_for_everybody).

## Tables in DB

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
