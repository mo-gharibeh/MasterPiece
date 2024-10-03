const userRole = localStorage.getItem('userRole');

function goToAddStore(){
    if ( userRole == "User"){
        window.location.href = 'Subscriptions.html';
        
    }
    else {
        window.location.href = 'AddStore.html';
    }
}

function goToAddEvent(){

    if ( userRole == "User"){
        window.location.href = 'Subscriptions.html';
        
    }
    else {
        window.location.href = 'AddEvent.html';
    }
}
