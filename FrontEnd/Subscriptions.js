

async function OneTimePlane(userId) {
    try {
        debugger
        let userId = localStorage.getItem("userId");
        // إرسال طلب PUT إلى الـ API لتحديث الدور
        let response = await fetch(`https://localhost:44398/api/Users/updateRole/${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        let data = await response.json();

        if (response.ok) {
            alert(data.message); // رسالة النجاح
            // يمكن إضافة أي إجراءات إضافية هنا (مثل إعادة توجيه المستخدم)
            localStorage.setItem('token', 1);
            location.href = "Services.html";
        } else {
            alert('Failed to update user role');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}