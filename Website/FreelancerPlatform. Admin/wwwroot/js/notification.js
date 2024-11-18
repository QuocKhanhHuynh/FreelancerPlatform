function showNotification(message, type) {

    const notification = document.createElement('div');
    notification.classList.add('notification');


    if (type === 'success') {
        notification.classList.add('notification-success');
    } else if (type === 'error') {
        notification.classList.add('notification-error');
    }
    notification.innerText = message;
    document.body.appendChild(notification);

    setTimeout(() => {
        notification.classList.add('notification-show');
    }, 100);

    setTimeout(() => {
        notification.classList.remove('notification-show');
        setTimeout(() => {
            notification.remove();
        }, 500);
    }, 3000);
}
function showSuccess(message) {
    showNotification(message, 'success');
}

function showError(message) {
    console.log('goi vao')
    showNotification(message, 'error');
}