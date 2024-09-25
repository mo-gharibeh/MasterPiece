function goToAddStore(){
    const token = localStorage.getItem('token');
    if ( token == null){
        window.location.href = 'Subscriptions.html';
        
    }
    else {
        window.location.href = 'AddStore.html';
    }
}

function goToAddEvent(){
    const token = localStorage.getItem('token');
    if ( token == null){
        window.location.href = 'Subscriptions.html';
        
    }
    else {
        window.location.href = 'AddEvent.html';
    }
}
