document.getElementById("follow_btn").addEventListener("click", follow());
function follow() {
    var userId = "611a46b0-33d0-4609-b43d-f2b47617792b";
    var response = fetch(`/User/Profile/Follow?userId=${userId}`, {
        method: "POST",
        body: JSON.stringify({ id: userId }),
        headers: {
            'Content-Type': 'application/json'
        }
    });

    var responseData = response.JSON;
    console.log(responseData);
}