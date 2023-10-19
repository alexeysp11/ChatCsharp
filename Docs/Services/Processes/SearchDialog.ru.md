# SearchDialog Process 

Доступно на других языках: [English/Английский](SearchDialog.md), [Russian/Русский](SearchDialog.ru.md).

- Выбор собеседника для начала общения: 
    - Пользователь находится на странице "Начать диалог".
    - Пользователь вводит логин/email/номер телефона потенциального собеседника в поле поиска;
    - На бэкенд отправляется запрос для того, чтобы проверить, есть ли такой пользователь в БД: 
        - Если пользователь не найден, бэкенд отправляет ответ "Пользователь не найден".
        - Если найден хотя бы один пользователь, то отображаем всех пользователей, которые соответствуют заданному фильтру.
    - Если пользователь нажимает "Отмена", то стераются введённые данные в поле поиска.
    - Если пользователь кликает на кнопку "Найти" повторно, то запрос не отправляется, т.к. данные в поле поиска не были изменены.
    - Если пользователь нажимает на пользователя, то происходит переадресация на страницу "Диалог" (прогружается переписка с выбранным собеседником).