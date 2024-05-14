const uri = 'api/ProjectStatuses';
let projectStatuses = [];

function getProjectStatuses() {
    fetch(`${uri}`)
        .then(response => response.json())
        .then(data => _displayProjectStatuses(data))
        .catch(error => console.error('Unable to get statuses.', error));
}

function addProjectStatus() {
    const addNameTextbox = document.getElementById('add-name');
    const addDescriptionTextbox = document.getElementById('add-description');

    const projectStatuses = {
        Name: addNameTextbox.value.trim(),
        Description: addDescriptionTextbox.value.trim(),
    };

    fetch(`${uri}`, {
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
        .catch(error => console.error('Unable to add status.', error));
}

function deleteProjectStatus(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getProjectStatuses())
        .catch(error => console.error('Unable to delete status.', error));
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

    fetch(`${uri}/${projectStatusId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(projectStatus)
    })
        .then(() => getProjectStatuses())
        .catch(error => console.error('Unable to update status.', error));

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
        editButton.classList.add('blueBtn');
        editButton.innerText = 'Редагувати';
        editButton.setAttribute('onclick', `displayEditForm(${projectStatus.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.classList.add('redBtn');
        deleteButton.innerText = 'Видалити';
        deleteButton.setAttribute('onclick', `deleteProjectStatus(${projectStatus.id})`);

        let searchProjectsButton = button.cloneNode(false);
        searchProjectsButton.classList.add('lightBlueBtn');
        searchProjectsButton.innerText = 'Проєкти';
        searchProjectsButton.setAttribute('onclick', `getProjectsWithStatus(${projectStatus.id})`)

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(projectStatus.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNodeDescription = document.createTextNode(projectStatus.description);
        td2.appendChild(textNodeDescription);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(searchProjectsButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    projectStatuses = data;
}

function getProjectsWithStatus(id)
{
    document.getElementById('projectResultTable').style.display = 'block';
    document.getElementById('projectResultTableHeader').style.display = 'block';
    document.getElementById('projectResultTableCloseBtn').style.display = 'block';

    fetch(`${uri}/${id}/projects`)
        .then(response => response.json())
        .then(data => _displayProjects(data))
        .catch(error => console.error('Unable to get projects with status.', error));
}

function _displayProjects(data)
{
    const tBody = document.getElementById('projectResultTableBody');
    tBody.innerHTML = '';

    data.forEach(project => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(project.projectName);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNodeDescription = document.createTextNode(project.projectDescription);
        td2.appendChild(textNodeDescription);
    });
}

function closeProjectResultTable()
{
    document.getElementById('projectResultTable').style.display = 'none';
    document.getElementById('projectResultTableHeader').style.display = 'none';
    document.getElementById('projectResultTableCloseBtn').style.display = 'none';
}