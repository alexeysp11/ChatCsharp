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

The [ChatMessagingService](../Core/Services/ChatMessagingService.md) class is responsible for processing network requests. 

## Tables in DB

- chat_user: [UserAccount](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/InformationSystem/UserAccount.md)
- chat_message: [MessageWF](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/MessageWF.md)
- chat_cms_request (change message status): [SetMsgStatusRequestDTO](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/DTOs/SetMsgStatusRequestDTO.md)
- chat_message_status: [MessageStatus](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/MessageStatus.md)
- chat_conversation: [Chatroom](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/Chatroom.md) and [ChannelUser](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/ChannelUser.md)
- chat_entity_type: [ChatroomType](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/SocialCommunication/ChatroomType.md)
- chat_entity_status: [UserStatus](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/InformationSystem/UserStatus.md)
- chat_notification_policies: [NotificationPolicies](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Business/Customers/NotificationPolicies.md)
- chat_e2ee_algorithm_type: [E2EEAlgorithmType](https://github.com/alexeysp11/workflow-lib/blob/main/docs/Models/Cryptography/E2EEAlgorithmType.md)
