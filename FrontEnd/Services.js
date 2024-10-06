const userRole = localStorage.getItem('userRole');

function goToAddStore(){
    if ( userRole == "Manager"){
        window.location.href = 'AddStore.html';
        
    }
    else {
        window.location.href = 'Subscriptions.html';
    }
}

function goToAddEvent(){

    if ( userRole == "Manager"){
        window.location.href = 'AddEvent.html';
        
    }
    else {
        window.location.href = 'Subscriptions.html';
    }
}
