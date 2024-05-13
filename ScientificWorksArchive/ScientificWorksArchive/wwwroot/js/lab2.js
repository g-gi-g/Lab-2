﻿const uri = 'api/ProjectStatuses';
let projectStatuses = [];

function getProjectStatuses() {
    fetch(`${uri}/GetProjectStatuses`)
        .then(response => response.json())
        .then(data => _displayProjectStatuses(data))
        .catch(error => console.error('Неможливо отримати статуси проєктів.', error));
}

function addProjectStatus() {
    const addNameTextbox = document.getElementById('add-name');
    const addDescriptionTextbox = document.getElementById('add-description');

    const projectStatuses = {
        Name: addNameTextbox.value.trim(),
        Description: addDescriptionTextbox.value.trim(),
    };

    fetch(`${uri}/PostProjectStatus`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(projectStatuses)
    })
        .then(response => response.json())
        .then(() => {
            getProjectStatuses();
            addNameTextbox.value = '';
            addDescriptionTextbox.value = '';
        })
        .catch(error => console.error('Неможливо додати статус проєкту.', error));
}

function deleteProjectStatus(id) {
    fetch(`${uri}/DeleteProjectStatus/${id}`, {
        method: 'DELETE'
    })
        .then(() => getProjectStatuses())
        .catch(error => console.error('Неможливо видалити статус проєкту.', error));
}

function displayEditForm(id) {
    const projectStatus = projectStatuses.find(projectStatus => projectStatus.id === id);

    document.getElementById('edit-id').value = projectStatus.id;
    document.getElementById('edit-name').value = projectStatus.name;
    document.getElementById('edit-description').value = projectStatus.description;
    document.getElementById('editForm').style.display = 'block';
}

function updateProjectStatus() {
    const projectStatusId = document.getElementById('edit-id').value;
    const projectStatus = {
        Id: parseInt(projectStatusId, 10),
        Name: document.getElementById('edit-name').value.trim(),
        Description: document.getElementById('edit-description').value.trim()
    };

    fetch(`${uri}/PutProjectStatus/${projectStatusId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(projectStatus)
    })
        .then(() => getProjectStatuses())
        .catch(error => console.error('Неможливо оновити статус проєкту.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayProjectStatuses(data) {
    const tBody = document.getElementById('projectStatuses');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(projectStatus => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Редагувати';
        editButton.setAttribute('onclick', `displayEditForm(${projectStatus.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Видалити';
        deleteButton.setAttribute('onclick', `deleteProjectStatus(${projectStatus.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(projectStatus.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNodeInfo = document.createTextNode(projectStatus.description);
        td2.appendChild(textNodeInfo);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    projectStatuses = data;
}