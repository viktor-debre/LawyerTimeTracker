$('#task').submit(function (e) {
    e.preventDefault();

    const id = document.getElementById("id");
    const title = document.getElementById("title");
    const description = document.getElementById("description");
    const typeOfTask = document.getElementById("typeOfTask");
    var message = document.getElementById("message");

    if (!title.value || !typeOfTask.value) {
        message.innerHTML = "Title and type of task must be filled!";
    }
    else if (title.value.length < 4 || title.value.length > 4) {
        message.innerHTML = "Title of task must be from 4 to 20 characters!";
    }
    else if (description.value.length < 100) {
        message.innerHTML = "Title of task must be less than 100 characters!";
    }
    else if (typeOfTask.value.length < 4 || typeOfTask.value.length > 4) {
        message.innerHTML = "Type of task must be from 4 to 20 characters!";
    }
    else {
        $.ajax({
            controller: 'Task',
            url: '/UpdateTask',
            type: 'post',
            data: {id,title,description,typeOfTask},
            success: function () {
                message.innerHTML = "Task updated!";
                window.location.reload();
            }
        });
    }
});